var app = angular.module('BookAppointmentApp');
// angular.module('BookAppointmentApp',['ui.calendar', 'ui.bootstrap', 'angular-underscore', 'flash'])
  app.controller('BookAppointmentCtrl',function($scope, $modal, $log, $http, $compile, $timeout, Flash, $rootScope) {
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
      { "id": 2, "label": "IPD"},
      { "id": 3, "label": "OPD"},
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
    }

    $scope.checkNotValidTime = function(start_date){
      return moment(new Date()).isAfter(start_date);
    };

    $scope.alertOnEventClick = function(event, jsEvent, view){
      if($scope.checkNotValidTime(event.start)){
        $scope.openPastTime(event, jsEvent, view, '');
      }else{
        $scope.openEdit(event, jsEvent, view, '');
      }
      // $scope.alertMessage = (event.title + ' was clicked ');
    };
    /* alert on Drop */
    $scope.alertOnDropOrResize = function(event, delta, revertFunc, jsEvent, ui, view){
      var eventInSource = $scope.findWhere($scope.events, {id: event.id});
      if($scope.checkNotValidTime(eventInSource.start)){
        $scope.appointmentPastEvent();
        revertFunc();
      }else{
        if($scope.checkNotValidTime(event.start)){
          $scope.appointmentPastDate();
          revertFunc();
        }else{
          // $scope.alertMessage = ('Event Droped to make dayDelta ' + delta);
          if($scope.stopEventOverloap(event.start, event.end, event.id)){
            $scope.appointmentNotUpdated();
            revertFunc();
          }else{
            $scope.appointmentUpdated();
            if(eventInSource){
              eventInSource.start = event.start;
              eventInSource.end = event.end;
            }
          }
        }
      }
    };

    $scope.stopEventOverloap = function(start, end, event_id){
      event_id = typeof event_id !== 'undefined' ? event_id : 0;
      for(i in $scope.events){
        if(event_id != $scope.events[i].id){
          if (end > $scope.events[i].start && start < $scope.events[i].end){
            return true;
          }
        }
      }
      return false;
    };

    $scope.slotSelected = function(start, end, jsEvent, view){
      // start.format('hh:mm') , start.hours()
      if($scope.checkNotValidTime(start)){
        $scope.appointmentPastDate();
        $('#appointmentBookingCalendar').fullCalendar('unselect');
      }else{
        if($scope.stopEventOverloap(start, end)){
          $scope.appointmentNotUpdated();
          $('#appointmentBookingCalendar').fullCalendar('unselect');
        }else{
          $scope.open(start, end, jsEvent, view, '');
        }
      }
    };

    $scope.eventRenderContent = function(event, element, view){
      if(event.subject)
        element.find('.fc-title').append(" - " + event.subject);
      if(!$scope.checkNotValidTime(event.start) && event.appointment_type == "Patient Appointment"){
        if(event.is_paid){
          element.find('.fc-title').append('<sapn><i class="fa fa-inr pull-right prepay-symbol-green"></i></span>');
        }else{
          element.find('.fc-title').append('<sapn><i class="fa fa-inr pull-right prepay-symbol-red"></i></span>');
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
      if (jsEvent.target.classList.contains('fc-bgevent')) {
        $timeout( function(){
          $(jsEvent.target).trigger('click');
        }, 2000);
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

    slotArr = $scope.uiConfig.calendar.slotDuration.split(':');
    slotArr[0] = slotArr[0] != "00" ? Math.floor(slotArr[0] * 60) : 00;
    slotArr[1] = slotArr[1];
    slotArr[2] = slotArr[2] != "00" ? Math.floor(slotArr[2] / 60) : 00;
    $rootScope.slot = (parseInt(slotArr[0]) + parseInt(slotArr[1]) + parseInt(slotArr[2]));


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

    $scope.getInitialData = function(){
      $http.get('../api/calendar/calendar_details', {params:
        {doclocation_id: $scope.locationId,
         start: $scope.viewStartDate,
         end: $scope.viewEndDate
        }})
        .success(function (response) {
          var minTime = response.calendar.min.toString(),
          maxTime = response.calendar.max.toString();
          $scope.uiConfig.calendar.slotDuration = response.calendar.slot_duration;
          $scope.uiConfig.calendar.minTime = minTime.slice(0, -2) + ":" + minTime.slice(-2);
          $scope.uiConfig.calendar.maxTime = maxTime.slice(0, -2) + ":" + maxTime.slice(-2);
          $scope.events.splice(0,$scope.events.length);
          $scope.each(response.events, function(event){
            $scope.events.push($scope.formatEvent(event));
        });
      });
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
          hash['patientId'] = '114';
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
              return {
                  'id': selectedItem.event.id,
                  'appointmentTitle': selectedItem.subject,
                  'appointmentStartTime': selectedItem.start.format('MM/DD/YYYY HH:mm'),
                  'appointmentEndTime': selectedItem.end.format('MM/DD/YYYY HH:mm'),
                  'isAllDayEvent':'false',
                  'doctorId': $scope.doctorId,
                  'doctorLocationId': $scope.locationId,
                  'appointmentType': selectedItem.appointment_type ? selectedItem.appointment_type : '0' ,
                  'prepayAmount': selectedItem.prepay_amount || '0',
                  'cancelOverlapped': 'false',
                  'email': selectedItem.email || '',
                  'mobileno': selectedItem.mobile_number || '',
                  'prepayBy': selectedItem.prepay_date ? (selectedItem.prepay_date + ' ' + selectedItem.prepay_time) : '12/12/2015 09:30:00'
                };
            };

            $scope.postEditedData = function(selectedItem){
              var url_to_post = '../api/calendar/update_appointment',
              data = $scope.getEditedData(selectedItem);
              $http.post(url_to_post, data)
                .success(function (response) {
                  if(response.IsSuccess){
                    $('#appointmentBookingCalendar').fullCalendar('updateEvent', selectedItem.event);
                    $scope.appointmentUpdated();
                  }
                });
            };

            modalInstance.result.then(function (selectedItem) {
              $scope.selected_event = selectedItem;
              if($scope.selected_event.changeCloseType){
                $http.get('../api/calendar/delete_appointment?appointmentId=' + selectedItem.event.id)
                  .success(function (response) {
                    if(response.IsSuccess == true)
                      $scope.events.splice($scope.findIndex($scope.events, {id: selectedItem.event.id}),1);
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
