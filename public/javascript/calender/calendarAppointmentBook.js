angular.module('BookAppointmentApp',['ui.calendar', 'ui.bootstrap', 'angular-underscore', 'flash', 'dnTimepicker'])
  .controller('BookAppointmentCtrl',function($scope, $modal, $log, Flash, $http, $rootScope) {

    /* Calendar specific changes
      This has calendar configurations and event binding for the calendar
    */

    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();
    $scope.events = [];
    var slotArr = [];
    $rootScope.slot = "";

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

    $scope.alertOnEventClick = function(event, jsEvent, view){
      $scope.openEdit(event, jsEvent, view, '');
      // $scope.alertMessage = (event.title + ' was clicked ');
    };
    /* alert on Drop */
    $scope.alertOnDropOrResize = function(event, delta, revertFunc, jsEvent, ui, view){
      // $scope.alertMessage = ('Event Droped to make dayDelta ' + delta);
      if($scope.stopEventOverloap(event.start, event.end, event.event_id)){
        $scope.appointmentNotUpdated();
        revertFunc();
      }else{
        $scope.appointmentUpdated();
        var eventInSource = $scope.findWhere($scope.events, {event_id: event.event_id});
        if(eventInSource){
          eventInSource.start = event.start;
          eventInSource.end = event.end;
        }
      }
    };

    $scope.stopEventOverloap = function(start, end, event_id){
      event_id = typeof event_id !== 'undefined' ? event_id : 0;
      for(i in $scope.events){
        if(event_id != $scope.events[i].event_id){
          if (end > $scope.events[i].start && start < $scope.events[i].end){
            return true;
          }
        }
      }
      return false;
    };

    $scope.slotSelected = function(start, end, jsEvent, view){
      // start.format('hh:mm') , start.hours()
        if($scope.stopEventOverloap(start, end)){
          $scope.appointmentNotUpdated();
          $('#appointmentBookingCalendar').fullCalendar('unselect');
        }else{
          $scope.open(start, end, jsEvent, view, '');
        }
    };

    $scope.generateUniqueEventId = function(start_date){
      return parseInt(start_date.format('MDDYYYY')+ '' + Math.floor(Math.random() * 10000) + 1);
    };

    $http.get('jsons/appointments.json').success(function (data){
      var appointments = data;

      angular.forEach(appointments.events, function(app){
          app.event_id = $scope.generateUniqueEventId(moment(new Date(y, m, d + 1)));
          app.start = new Date(app.start);
          app.end = new Date(app.end);
          app.stick = true;
          // app.constraint= 'available_hours';
          $scope.events.push(app);
      });
    });

    $scope.uiConfig = {
      calendar:{
        firstDay: new Date().getDay(),
        defaultView: 'agendaWeek',
        height: 550,
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
        slotDuration: '00:15:01',
        allDaySlot: false,
        selectable: true,
        selectOverlap: false,
        eventClick: $scope.alertOnEventClick,
        eventDrop: $scope.alertOnDropOrResize,
        eventResize: $scope.alertOnDropOrResize,
        eventRender: $scope.eventRender,
        select: $scope.slotSelected,
        editable: true,
        businessHours: {
          start: '00:00', // a start time (10am)
          end: '24:00', // an end time (12pm)
          dow: [ 1,2,3,4,5 ] // days of week
        },
        selectConstraint: "businessHours",
        eventConstraint: "businessHours"
      }
    };

    slotArr = $scope.uiConfig.calendar.slotDuration.split(':');
    slotArr[0] = slotArr[0] != "00" ? Math.floor(slotArr[0] * 60) : 00;
    slotArr[1] = slotArr[1];
    slotArr[2] = slotArr[2] != "00" ? Math.floor(slotArr[2] / 60) : 00;
    $rootScope.slot = (parseInt(slotArr[0]) + parseInt(slotArr[1]) + parseInt(slotArr[2]));
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

      $scope.addEvent= function(){
        $scope.events.push({
          event_id: $scope.generateUniqueEventId($scope.selected_event.start),
          title: 'Open Sesame',
          start: $scope.selected_event.start,
          end: $scope.selected_event.end,
          className: ['openSesame'],
          stick: true
        });
      };

      modalInstance.result.then(function (selectedItem) {
        $scope.selected_event = selectedItem;
        $scope.addEvent();
        $scope.appointmentBooked();
      }, function () {
        $log.info('Modal dismissed at: ' + new Date());
      });
    };

    $scope.openEdit = function (event, jsEvent, view, size) {
      var currentEvent = {};

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
            return $scope.items;
          }
        }
      });

      modalInstance.result.then(function (selectedItem) {
        $scope.selected_event = selectedItem;
        if($scope.selected_event.changeCloseType){
          $scope.events.splice($scope.findIndex($scope.events, {event_id: selectedItem.event.event_id}),1);
        }else{
          currentEvent = _.find($scope.events, {event_id: $scope.selected_event.event.event_id});
          currentEvent.start = $scope.selected_event.event.start;
          currentEvent.end = $scope.selected_event.event.end;
        }
      }, function () {
        $log.info('Modal dismissed at: ' + new Date());
      });
    };
  });
