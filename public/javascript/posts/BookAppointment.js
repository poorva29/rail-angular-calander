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
            titleFormat: 'YYYY, MM, DD',
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
                $scope.extend(event,{rendering: 'background'});

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
          $scope.events.splice(0,$scope.events.length)
          $scope.each(response.events, function(event){
            $scope.events.push($scope.formatEvent(event));
            $scope.uiConfig.calendar.slotDuration = response.calendar.slot_duration;
            $scope.uiConfig.calendar.minTime = response.calendar.minTime;
            $scope.uiConfig.calendar.maxTime = response.calendar.maxTime;
        });
      });
    };

    $scope.$on("doctorLocation", function (event, args) {
      $scope.locationId = args.locationId;
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
        $scope.events.push({
          id: event_id,
          title: 'Open Sesame',
          start: $scope.selected_event.start,
          end: $scope.selected_event.end,
          className: ['openSesame'],
          stick: true,
          backgroundColor: $scope.selected_event.event_type == 'blocked' ? '#58BBEC' : ''
        });
      };

      $scope.bookAppointment = function(event_hash){
        $http.post('/book_appointment', event_hash).success(function(response){
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
        $scope.bookAppointment(event_hash);
      }, function () {
        $log.info('Modal dismissed at: ' + new Date());
      });
    };

    $scope.openEdit = function (event, jsEvent, view, size) {
      $http.post('/get_event_data', {id: event.id, event: $scope.pick(event, 'patient_name', 'subject', 'id', 'event_type')})
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
                  $scope.extend($scope.items, response);
                  return $scope.items;
                }
              }
            });

            modalInstance.result.then(function (selectedItem) {
              $scope.selected_event = selectedItem;
              if($scope.selected_event.changeCloseType){
                $scope.events.splice($scope.findIndex($scope.events, {id: selectedItem.event.id}),1);
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
      $http.post('/get_event_data', {id: event.id, event: $scope.pick(event, 'patient_name', 'subject', 'id', 'event_type')})
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
