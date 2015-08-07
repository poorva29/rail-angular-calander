var app = angular.module('BookAppointmentApp');
// angular.module('BookAppointmentApp',['ui.calendar', 'ui.bootstrap', 'angular-underscore', 'flash'])
  app.controller('BookAppointmentCtrl',function($scope, $modal, $log, $http, $compile, $timeout, Flash, $rootScope, bootbox) {
    /* Calendar specific changes
      This has calendar configurations and event binding for the calendar
    */
    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();
    $scope.events = [];
    $scope.showCalendar = false;
    $scope.appointmentTypes = [
      { "id": 1, "label": "Conference Travel", "isDefault": true},
      { "id": 2, "label": "OPD"},
      { "id": 3, "label": "IPD"},
      { "id": 4, "label": "OT Schedule"},
      { "id": 5, "label": "Unscheduled Emergencies"}
    ];

    $scope.viewRenderWeekAgenda = function(view, element){
      $scope.viewStartDate = view.start.format('MM-DD-YYYY');
      $scope.viewEndDate = view.end.format('MM-DD-YYYY');
      if($scope.locationId)
        $scope.getInitialData()
    }

    $scope.showAlert = function (type, message) {
      Flash.create(type, message);
    };

    $scope.appointmentUpdated = function(){
      var message = '<strong> Booked !</strong>  Appointment Updated Successfully.';
      $scope.showAlert('success', message);
    };

    $scope.appointmentNotUpdated = function(){
      var message = '<strong> Not Booked !</strong>  Appointment Booked For Selected Time.';
      $scope.showAlert('danger', message);
    };

    $scope.appointmentBooked = function(){
      var message = '<strong> Booked !</strong>  Appointment Created Successfully.';
      $scope.showAlert('success', message);
    };

    $scope.appointmentPastDate = function(){
      var message = '<strong> Not Booked !</strong>  Please do not select past date or time.';
      $scope.showAlert('danger', message);
    };

    $scope.appointmentNotBooked = function(){
      var message = '<strong> Not Booked !</strong> Appointment Can Not Be Created.';
      $scope.showAlert('danger', message);
    };

    $scope.appointmentPastEvent = function(){
      var message = '<strong> Past Event !</strong> Cannot Be Updated.';
      $scope.showAlert('danger', message);
    };

    $scope.eventCreateFailure = function(){
      var message = '<strong> Event Creation Failure !</strong> Cannot Be Created.';
      $scope.showAlert('danger', message);
    };

    $scope.appointmentDeleted = function(){
      var message = '<strong> Deleted !</strong>  Appointment Deleted Successfully.';
      $scope.showAlert('success', message);
    };

    $scope.nonWorkingSlot = function(){
      var message = '<strong> Non Working Slot !</strong> Appointment Cannot Be Updated.';
      $scope.showAlert('danger', message);
    };

    $scope.appointmentNotBookedInMonthView = function(){
      var message = '<strong> Not Booked !</strong> Appointment Cannot Be Updated In Month View.';
      $scope.showAlert('danger', message);
    };

    $scope.checkNotValidTime = function(start_date){
      return moment(new Date()).isAfter(start_date);
    };

    $scope.alertOnEventClick = function(event, jsEvent, view){
      if(view.name == 'month' || $scope.locationId == -1){
        $scope.openPastTime(event, jsEvent, view, '');
      }else{
        if($scope.checkNotValidTime(event.start)){
          $scope.openPastTime(event, jsEvent, view, '');
        }else{
          $scope.openEdit(event, jsEvent, view, '');
        }
      }
      // $scope.alertMessage = (event.title + ' was clicked ');
    };

    $scope.updateEventSource = function(event){
      var eventInSource = $scope.findWhere($scope.events, {id: event.id});
      if(eventInSource){
        eventInSource.start = event.start;
        eventInSource.end = event.end;
      }
    };

    $scope.getUpdatedData = function(selectedItem){
      return {
        'id': selectedItem.id,
        'appointmentTitle': selectedItem.subject,
        'appointmentStartTime': selectedItem.start.format('MM/DD/YYYY HH:mm'),
        'appointmentEndTime': selectedItem.end.format('MM/DD/YYYY HH:mm'),
        'isAllDayEvent':'false',
        'doctorId': $scope.doctorId,
        'doctorLocationId': $scope.locationId,
        'appointmentType': selectedItem.appointment_type ? selectedItem.appointment_type : '0',
        'prepayAmount': selectedItem.prepay_amount || '0',
        'cancelOverlapped': 'false',
        'email': selectedItem.email || '',
        'mobileno': selectedItem.mobile_number || '',
        'prepayBy': selectedItem.prepay_date ? (selectedItem.prepay_date + ' ' + selectedItem.prepay_time) : selectedItem.prepay_by
      };
    };

    $scope.postUpdatedData = function(selectedItem, revertFunc){
      var url_to_post = '../api/calendar/update_appointment',
      data = $scope.getUpdatedData(selectedItem);
      if(data){
        $http.post(url_to_post, data)
          .success(function (response) {
            if(response.IsSuccess){
              $scope.updateEventSource(selectedItem);
              $scope.appointmentUpdated();
            }else{
              $scope.nonWorkingSlot();
              revertFunc();
            }
        });
      }
    };

    /* alert on Drop */
    $scope.alertOnDropOrResize = function(event, delta, revertFunc, jsEvent, ui, view){
      var eventInSource = $scope.findWhere($scope.events, {id: event.id}), data = {};
      if(view.name == 'month'){
        revertFunc();
        $scope.appointmentNotBookedInMonthView();
        return;
      }
      if($scope.locationId == -1){
        revertFunc();
        bootbox.alert("Please select a specific clinic / hospital to update appointment.", function() {
          return true;
        });
        return;
      }
      if($scope.checkNotValidTime(eventInSource.start)){
        $scope.appointmentPastEvent();
        revertFunc();
      }else{
        if($scope.checkNotValidTime(event.start)){
          $scope.appointmentPastDate();
          revertFunc();
        }else{
          // $scope.alertMessage = ('Event Droped to make dayDelta ' + delta);
          data = $scope.stopEventOverloap(event.start, event.end, event.id);
          if(data.is_valid){
            if(data.event_type == 'non-working'){
              $scope.nonWorkingSlot();
            }else{
              $scope.appointmentNotUpdated();
            }
            revertFunc();
          }else{
            $http.get('../api/calendar/appointment_details?appointment_id=' + event.id)
              .success(function (response) {
                $scope.extend(event, $scope.omit(response, 'start', 'end'));
                $scope.postUpdatedData(event, revertFunc);
              });
          }
        }
      }
    };

    $scope.stopEventOverloap = function(start, end, event_id){
      var data = {
        'is_valid' : false,
        'event_type': 'booking'
      };
      event_id = typeof event_id !== 'undefined' ? event_id : 0;
      for(i in $scope.events){
        if(event_id != $scope.events[i].id){
          if (end > $scope.events[i].start && start < $scope.events[i].end){
            data.event_type = $scope.events[i].event_type;
            data.is_valid = true;
            break;
          }else{
            data.is_valid = false;
          }
        }
      }
      return data;
    };

    $scope.slotSelected = function(start, end, jsEvent, view){
      var data = {};
      // start.format('hh:mm') , start.hours()
      if(view.name == 'month')
        return;
      if($scope.locationId == -1){
        bootbox.alert("Please select a specific clinic / hospital to book appointment.", function() {
          return true;
        });
      }else{
        if($scope.checkNotValidTime(start)){
          $scope.appointmentPastDate();
          $('#appointmentBookingCalendar').fullCalendar('unselect');
        }else{
          data = $scope.stopEventOverloap(start, end);
          if(data.is_valid){
            if(data.event_type == 'non-working'){
              $scope.nonWorkingSlot();
            }else{
              $scope.appointmentNotUpdated();
            }
            $('#appointmentBookingCalendar').fullCalendar('unselect');
          }else{
            $scope.open(start, end, jsEvent, view, '');
          }
        }
      }
    };

    $scope.eventRenderContent = function(event, element, view){
      if(event.subject)
        element.find('.fc-title').append(" - " + event.subject);
      if(!$scope.checkNotValidTime(event.start) && event.appointment_type == "Patient Appointment" && event.prepay_amount > 0){
        if(event.is_paid){
          element.find('.fc-content').append('<div class="rupee-sticker"><i class="fa fa-inr prepay-symbol-green"></i></div>');
        }else{
          element.find('.fc-content').append('<div class="rupee-sticker"><i class="fa fa-inr prepay-symbol-red"></i></div>');
        }
      }
      if(event.event_type == 'non-working'){
        element.attr({'tooltip': 'Closed Slot',
                      'tooltip-append-to-body': true,
                      'tooltip-trigger':"click"});
        $compile(element)($scope);
      }
    };

    $scope.dayEvent = function(date, jsEvent, view) {
      if(view.name == 'month'){
        $('#appointmentBookingCalendar').fullCalendar('gotoDate', date);
        $('#appointmentBookingCalendar').fullCalendar('changeView', 'agendaWeek');
      }else{
        if (jsEvent.target.classList.contains('fc-bgevent')) {
          $timeout( function(){
            $(jsEvent.target).trigger('click');
          }, 2000);
        }
      }
    };

    $scope.uiConfig = {
      calendar:{
        firstDay: new Date().getDay(),
        defaultView: 'agendaWeek',
        height: 'auto',
        header:{
          left: 'agendaDay agendaWeek month',
          center: 'title',
          right: 'today prev,next'
        },
        views: {
          agendaWeek: {
            titleFormat: 'MMMM DD, YYYY',
          }
        },
        slotDuration: '01:00:00',
        allDaySlot: false,
        selectable: true,
        selectOverlap: false,
        eventClick: $scope.alertOnEventClick,
        eventDrop: $scope.alertOnDropOrResize,
        eventResize: $scope.alertOnDropOrResize,
        eventRender: $scope.eventRenderContent,
        dayClick: $scope.dayEvent,
        select: $scope.slotSelected,
        viewRender: $scope.viewRenderWeekAgenda,
        editable: true,
        timezone: 'local'
      }
    };

    $scope.addPatientAttributes = function(event){
      return {
        title: event.patient_name,
        subject: event.subject
      };
    };

    $scope.addDoctorAttributes = function(event){
      return {
        title: event.appointment_type,
        subject: event.subject,
        backgroundColor: '#58BBEC'
      };
    };

    $scope.formatEvent = function(event){
      $scope.extend(event,{
                            stick: true,
                            start: moment(event.start, 'YYYY-MM-DDThh:mm'),
                            end: moment(event.end, 'YYYY-MM-DDThh:mm')
                          });
      switch(event.event_type){
        case 'booking':
                      if(event.appointment_type == "Patient Appointment")
                        $scope.extend(event, $scope.addPatientAttributes(event));
                      else
                        $scope.extend(event, $scope.addDoctorAttributes(event));
                      break;
        default:
                $scope.extend(event,{rendering: 'background', backgroundColor: '#646464'});
      }
      return event;
    };

    $scope.dateClicked = function(gotoDate){
      var date = moment(gotoDate, 'DD/MM/YYYY');
      $('#appointmentBookingCalendar').fullCalendar('gotoDate', date);
      $('#appointmentBookingCalendar').fullCalendar('changeView', 'agendaDay');
    };

    $scope.dateClicable = function(){
      var week_dates =$('.fc-agendaWeek-view .fc-widget-header .fc-day-header.fc-widget-header');
      $scope.each(week_dates, function(day_td){
        var attr = $(day_td).attr('ng-click');
        if (typeof attr == typeof undefined || attr == false) {
          date = $(day_td).text();
          date = moment(date.substr(4,7), 'MM/DD').format('DD/MM/YYYY')
          $(day_td).attr({'ng-click': "dateClicked('" + date + "')"});
          $compile($(day_td))($scope);
        }
      });
    };

    $scope.getInitialData = function(){
      if($scope.startDate !== $scope.viewStartDate || $scope.endDate !== $scope.viewEndDate ||
        $scope.changedLocationId !== $scope.locationId || $scope.changedDoctorId !== $scope.doctorId){
        $scope.startDate = $scope.viewStartDate;
        $scope.endDate = $scope.viewEndDate;
        $scope.changedLocationId = $scope.locationId;
        $scope.changedDoctorId = $scope.doctorId;
        $http.get('../api/calendar/calendar_details', {params:
          {doclocation_id: $scope.locationId,
           start: $scope.viewStartDate,
           end: $scope.viewEndDate,
           doctor_id: $scope.doctorId
          }})
          .success(function (response) {
            var minTime = response.calendar.min.toString(),
            maxTime = response.calendar.max.toString(), slot = null,
            docMaxTime = null, docMinTime = null;
            slot = response.calendar.slot_duration
            $scope.uiConfig.calendar.slotDuration = slot;
            slot = moment(slot, 'hh:mm:ss');
            $rootScope.slot = (slot.hour() * 60) + slot.minutes();
            docMinTime = minTime.slice(0, -2) + ":" + minTime.slice(-2);
            docMaxTime = maxTime.slice(0, -2) + ":" + maxTime.slice(-2);
            $scope.uiConfig.calendar.minTime = docMinTime;
            $scope.uiConfig.calendar.maxTime = docMaxTime;
            $rootScope.minTime = moment(docMinTime, 'hh:mm').format('hh:mm a');
            $rootScope.maxFromTime = moment(docMaxTime, 'hh:mm').subtract($rootScope.slot, 'minutes').format('hh:mm a');
            $rootScope.maxToTime = moment(docMaxTime, 'hh:mm').format('hh:mm a');
            $scope.events.splice(0,$scope.events.length);
            $scope.each(response.events, function(event){
              $scope.events.push($scope.formatEvent(event));
          });
        });
      }
      $scope.dateClicable();
    };

    $scope.$on("doctorLocation", function (event, args) {
      $scope.locationId = args.locationId;
      $scope.doctorId = args.doctorId;
      if($scope.locationId){
        $scope.getInitialData();
        $scope.showCalendar = true;
      }else{
        $scope.showCalendar = false;
      }
    });

    $scope.eventSources = [$scope.events];

    /* Modal specific changes
      This has modal implementation and value assignment for the input fields
      in the modal with data fetching
    */

    $scope.items = [];

    $scope.animationsEnabled = true;

    $scope.open = function (start, end, jsEvent, view, size) {

      var modalInstance = $modal.open({
        animation: $scope.animationsEnabled,
        templateUrl: 'appointmentBooking.html',
        controller: 'BookAppointmentModalInstanceCtrl',
        size: size,
        resolve: {
          items: function () {
            $scope.items = {
              'start': start,
              'end': end,
              'jsEvent': jsEvent,
              'view': view,
              'doctorId': $scope.doctorId,
              'locationId': $scope.locationId
            };
            return $scope.items;
          }
        }
      });

      $scope.addEvent= function(event_id){
        var title = '',current_appt_type = '', appointment_type = $scope.selected_event.appointment_type,
            backgroundColor = '';
        if(appointment_type == 'Patient Appointment'){
          title = $scope.selected_event.patient_name;
        }else{
          current_appt_type = $scope.findWhere($scope.appointmentTypes, {"id": $scope.selected_event.appointment_type});
          title = current_appt_type.label;
          backgroundColor = '#58BBEC'
        }
        $scope.events.push({
          id: event_id,
          title: title,
          subject: $scope.selected_event.subject ? $scope.selected_event.subject : "",
          start: $scope.selected_event.start,
          end: $scope.selected_event.end,
          className: ['openSesame'],
          stick: true,
          backgroundColor: backgroundColor,
          event_type: 'booking',
          prepay_amount: $scope.selected_event.prepay_amount,
          is_paid: $scope.selected_event.paymentSelected ? true : false,
          appointment_type: appointment_type
        });
      };

      $scope.getDataToSend = function(event_hash){
        var hash = {};
        hash['doctorId'] = $scope.doctorId;
        hash['doctorlocationId'] = $scope.locationId;
        hash['appointmentStartTime'] = event_hash.start.format('MM/DD/YYYY HH:mm');
        hash['appointmentEndTime'] = event_hash.end.format('MM/DD/YYYY HH:mm');
        hash['appointmentTitle'] = event_hash.subject || '';
        hash['cancelOverlapped'] = event_hash.cancel_overlapped_event;
        hash['isAllDayEvent'] = event_hash.is_all_day_event;
        hash['createdDate'] = moment(new Date()).format('MM/DD/YYYY HH:mm');
        hash['createdby'] = event_hash.created_by;
        hash['patname'] = event_hash.patient_name || '';
        hash['patienttype'] = event_hash.patient_type || '';
        hash['mobileno'] = event_hash.mobile_number || '';
        hash['email'] = event_hash.email || '';
        hash['prepayAmount'] = event_hash.prepay_amount || 0.0;
        hash['prepayBy'] = event_hash.prepay_by || moment(new Date()).format('MM/DD/YYYY HH:mm');
        if(event_hash.appointment_type == 'Patient Appointment'){
          hash['patientId'] = event_hash.patient_id;
          hash['appointmentType'] = '0';
        }else {
          hash['patientId'] = null;
          hash['appointmentType'] = event_hash.appointment_type || 1;
        }
        return hash;
      };

      $scope.bookAppointment = function(event_hash){
        var data = $scope.getDataToSend(event_hash);
        var url_to_post = '../api/calendar/add_appointment';
        $http.post(url_to_post, data).success(function(response){
          if(response.IsSuccess){
            $scope.addEvent(response.ApptId);
            $scope.appointmentBooked();
          }else{
            $scope.appointmentNotBooked();
          }
        });
      };

      modalInstance.result.then(function (selectedItem) {
        var event_hash = {};
        $scope.selected_event = selectedItem;
        $scope.extend(event_hash, $scope.omit($scope.selected_event, 'jsEvent', 'view'));
        if(event_hash.appointment_type != 'Patient Appointment'){
          event_hash.appointment_type = event_hash.appointment_type;
        }
        $scope.bookAppointment(event_hash);
      }, function () {
        $log.info('Modal dismissed at: ' + new Date());
      });
    };

    $scope.openEdit = function (event, jsEvent, view, size) {
      $http.get('../api/calendar/appointment_details?appointment_id=' + event.id)
        .success(function (response) {
          if(response){
            var modalInstance = $modal.open({
              animation: $scope.animationsEnabled,
              templateUrl: 'appointmentBookingEdit.html',
              controller: 'BookAppointmentEditModalInstanceCtrl',
              size: size,
              resolve: {
                items: function () {
                  $scope.items = {
                    'event': event,
                    'start': event.start,
                    'end': event.end,
                    'jsEvent': jsEvent,
                    'view': view,
                    'changeCloseType': false // default type for modalInstance.result is ok() which is considered as 'true'
                  };
                  $scope.extend($scope.items, $scope.omit(response, 'start', 'end'));
                  return $scope.items;
                }
              }
            });

           $scope.getEditedData = function(selectedItem){
              var data = {};
              if($scope.checkNotValidTime(selectedItem.start)){
                $scope.appointmentPastDate();
              }else{
                data = $scope.stopEventOverloap(selectedItem.start, selectedItem.end, selectedItem.event.id);
                if(data.is_valid){
                  if(data.event_type == 'non-working'){
                    $scope.nonWorkingSlot();
                  }else{
                    $scope.appointmentNotUpdated();
                  }
                }else{
                  selectedItem.event.start = selectedItem.start;
                  selectedItem.event.end = selectedItem.end;
                  selectedItem.event.prepay_amount = selectedItem.prepay_amount;
                  $('#appointmentBookingCalendar').fullCalendar('updateEvent', selectedItem.event);
                  return {
                    'id': selectedItem.event.id,
                    'appointmentTitle': selectedItem.subject,
                    'appointmentStartTime': selectedItem.start.format('MM/DD/YYYY HH:mm'),
                    'appointmentEndTime': selectedItem.end.format('MM/DD/YYYY HH:mm'),
                    'isAllDayEvent':'false',
                    'doctorId': $scope.doctorId,
                    'doctorLocationId': $scope.locationId,
                    'appointmentType': selectedItem.appointment_type ? selectedItem.appointment_type : '0',
                    'prepayAmount': selectedItem.prepay_amount || '0',
                    'cancelOverlapped': 'false',
                    'email': selectedItem.email || '',
                    'mobileno': selectedItem.mobile_number || '',
                    'prepayBy': selectedItem.prepay_date ? (selectedItem.prepay_date + ' ' + selectedItem.prepay_time) : selectedItem.prepay_by
                  };
                }
              }
            };

            $scope.postEditedData = function(selectedItem){
              var url_to_post = '../api/calendar/update_appointment',
              data = $scope.getEditedData(selectedItem);
              if(data){
                $http.post(url_to_post, data)
                  .success(function (response) {
                    if(response.IsSuccess){
                      $scope.updateEventSource(selectedItem.event);
                      $('#appointmentBookingCalendar').fullCalendar('updateEvent', selectedItem.event);
                      $scope.appointmentUpdated();
                    }
                });
              }
            };

            modalInstance.result.then(function (selectedItem) {
              $scope.selected_event = selectedItem;
              if($scope.selected_event.changeCloseType){
                $http.get('../api/calendar/delete_appointment?appointmentId=' + selectedItem.event.id)
                  .success(function (response) {
                    if(response.IsSuccess == true)
                      $scope.events.splice($scope.findIndex($scope.events, {id: selectedItem.event.id}),1);
                      $scope.appointmentDeleted();
                  });
              }else{
                $scope.postEditedData(selectedItem);
              }
            }, function () {
              $log.info('Modal dismissed at: ' + new Date());
            });
          }else{
            $scope.eventCreateFailure();
          }
        });
    };

    $scope.openPastTime = function (event, jsEvent, view, size) {
      $http.get('../api/calendar/appointment_details?appointment_id=' + event.id)
        .success(function (response) {
          if(response){
            var modalInstance = $modal.open({
              animation: $scope.animationsEnabled,
              templateUrl: 'appointmentBookingPastTime.html',
              controller: 'pastTimeModalInstanceCtrl',
              size: size,
              resolve: {
                items: function () {
                  $scope.items = {
                    'event': event,
                    'start': event.start,
                    'end': event.end,
                    'jsEvent': jsEvent,
                    'view': view
                  };
                  $scope.extend($scope.items, $scope.omit(response, 'start', 'end'));
                  return $scope.items;
                }
              }
            });

            modalInstance.result.then(function (selectedItem) {
              $scope.selected_event = selectedItem;
            }, function () {
              $log.info('Modal dismissed at: ' + new Date());
            });
          }
        })
    };
  });
