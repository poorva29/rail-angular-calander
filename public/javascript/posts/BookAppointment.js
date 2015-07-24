var app = angular.module('BookAppointmentApp');
// angular.module('BookAppointmentApp',['ui.calendar', 'ui.bootstrap', 'angular-underscore', 'flash'])
  app.controller('BookAppointmentCtrl',function($scope, $modal, $log, $http, Flash) {
    /* Calendar specific changes
      This has calendar configurations and event binding for the calendar
    */
    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();
    $scope.events = [];
    $scope.showCalendar = false;

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
      $scope.showAlert('success', message);
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
                      $scope.extend(event, $scope.addPatientAttributes(event));
                      break;
        case 'blocked':
                      $scope.extend(event, $scope.addDoctorAttributes(event));
                      break;
        default:
                $scope.extend(event,{rendering: 'background', backgroundColor: '#646464'});
      }
      return event;
    };

    $scope.getInitialData = function(){
      $http.get('/api/calendar/calendar_details', {params:
        {location: $scope.locationId,
         start: $scope.viewStartDate,
         end: $scope.viewEndDate
        }})
        .success(function (response) {
          var minTime = response.calendar.minTime.toString(),
          maxTime = response.calendar.maxTime.toString();
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
              'view': view
            };
            return $scope.items;
          }
        }
      });

      $scope.addEvent= function(event_id){
        var title = '', event_type = $scope.selected_event.event_type,
            backgroundColor = '';
        if(event_type == 'blocked'){
          title = $scope.selected_event.appointment_type.label;
          backgroundColor = '#58BBEC'
        }else{
          title = $scope.selected_event.patient_name;
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
          event_type: event_type
        });
      };

      $scope.getDataToSend = function(event_hash){
        var hash = {};
        hash['doctorId'] = $scope.doctorId;
        hash['doctorlocationId'] = $scope.locationId;
        hash['appointmentStartTime'] = event_hash.start;
        hash['appointmentEndTime'] = event_hash.end;
        hash['appointmentTitle'] = event_hash.subject;
        hash['cancelOverlapped'] = event_hash.cancel_overlapped_event;
        hash['isAllDayEvent'] = event_hash.is_all_day_event;
        hash['cretaedDate'] = event_hash.cretaed_date;
        hash['createdby'] = event_hash.created_by;

        if(event_hash.event_type == 'booking'){
          hash['patname'] = event_hash.patient_name;
          hash['mobileno'] = event_hash.mobile_number;
          hash['prepayAmount'] = event_hash.prepay_amount;
          hash['prepayBy'] = event_hash.prepay_by;
          hash['email'] = event_hash.email;
          hash['patienttype'] = event_hash.patient_type;
          hash['patientId'] = "114";
        }else if(event_hash.event_type == 'blocked'){
          hash['appointmentType'] = event_hash.appointment_type;
        }
        return hash;
      };

      $scope.bookAppointment = function(event_hash){
        var data = $scope.getDataToSend(event_hash);
        var url_to_post = 'http://connect.s.miraihealth.com/CalendarService/CalendarService.svc/AddAppointment';
        $http.post(url_to_post, data).success(function(response){
          if(response.IsSuccess){
            $scope.addEvent(response.event_id);
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
        if(event_hash.appointment_type){
          event_hash.appointment_type = event_hash.appointment_type.id;
        }
        $scope.bookAppointment(event_hash);
      }, function () {
        $log.info('Modal dismissed at: ' + new Date());
      });
    };

    $scope.openEdit = function (event, jsEvent, view, size) {
      $http.post('/get_event_data', {id: event.id, event: $scope.pick(event, 'patient_name', 'subject', 'event_type')})
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
                  'appointmentId': selectedItem.id,
                  'appointmentTitle': selectedItem.subject,
                  'appointmentStartTime': selectedItem.start.format('DD/MM/YYYY hh:mm'),
                  'appointmentEndTime': selectedItem.end.format('DD/MM/YYYY hh:mm'),
                  'isAllDayEvent':'0',
                  'doctorId': $scope.doctorId,
                  'doctorlocationId': $scope.locationId,
                  "appointmentType": selectedItem.appointment_type ? selectedItem.appointment_type : '0' ,
                  "prepayAmount": selectedItem.prepay_amount,
                  "prepayBy": selectedItem.prepay_date ? (selectedItem.prepay_date + ' ' + selectedItem.prepay_time) : '',
                  'id': selectedItem.id
                };
            };

            $scope.postEditedData = function(selectedItem){
              // var url_to_post = 'http://connect.s.miraihealth.com/CalendarService/CalendarService.svc/Update';
              var url_to_post = '/post_edited_data';
              $http.post('/post_event_data', $scope.getEditedData(selectedItem))
                .success(function (response) {
                  console.log('Data Posted !');
                });
            };

            modalInstance.result.then(function (selectedItem) {
              $scope.selected_event = selectedItem;
              if($scope.selected_event.changeCloseType){
                $scope.events.splice($scope.findIndex($scope.events, {id: selectedItem.event.id}),1);
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
      $http.post('/get_event_data', {id: event.id, event: $scope.pick(event, 'patient_name', 'subject', 'event_type')})
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
                    'view': view,
                  };
                  $scope.extend($scope.items, response);
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
