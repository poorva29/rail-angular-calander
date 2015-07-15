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
      if($scope.stopEventOverloap(event.start, event.end, event.id)){
        $scope.appointmentNotUpdated();
        revertFunc();
      }else{
        $scope.appointmentUpdated();
        var eventInSource = $scope.findWhere($scope.events, {id: event.id});
        if(eventInSource){
          eventInSource.start = event.start;
          eventInSource.end = event.end;
        }
      }
    };

    $scope.stopEventOverloap = function(start, end, id){
      id = typeof id !== 'undefined' ? id : 0;
      for(i in $scope.events){
        if(id != $scope.events[i].id){
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

    $scope.eventsF = function (start, end, timezone, callback) {
      var m = new Date(start).getMonth();
      var events = [{
        id: $scope.generateUniqueEventId(moment(new Date(y, m, d + 3))),
        title: 'Feed Me ' + m,
        start: moment(new Date(y, m, d + 3, 19, 30)),
        end: moment(new Date(y, m, d + 3, 22, 30)),
        stick: true
      }];
      callback(events);
    };

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
        timezone: 'local'
      }
    };

    slotArr = $scope.uiConfig.calendar.slotDuration.split(':');
    slotArr[0] = slotArr[0] != "00" ? Math.floor(slotArr[0] * 60) : 00;
    slotArr[1] = slotArr[1];
    slotArr[2] = slotArr[2] != "00" ? Math.floor(slotArr[2] / 60) : 00;
    $rootScope.slot = (parseInt(slotArr[0]) + parseInt(slotArr[1]) + parseInt(slotArr[2]));

    $scope.formatEvent = function(event){
      $scope.extend(event,{
                            stick: true,
                            start: moment(event.start),
                            end: moment(event.end)
                          });
      if(event.event_type == 'booking'){
        $scope.extend(event, {id: $scope.generateUniqueEventId(moment(new Date(y, m, d + 1)))});
      }else{
        $scope.extend(event,{rendering: 'background'});
      }
      return event;
    };

    $scope.getInitialDate = function(){
      $http.get("/events")
        .success(function (response) {
          $scope.each(response.events, function(event){
            $scope.events.push($scope.formatEvent(event));
            // $scope.uiConfig.calendar.slotDuration = response.calendar.slot_duration;
        });
      });
    }
    $scope.getInitialDate();

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
          id: $scope.generateUniqueEventId($scope.selected_event.start),
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
          $scope.events.splice($scope.findIndex($scope.events, {id: selectedItem.event.id}),1);
        }else{
          currentEvent = _.find($scope.events, {id: $scope.selected_event.event.id});
          currentEvent.start = $scope.selected_event.event.start;
          currentEvent.end = $scope.selected_event.event.end;
        }
      }, function () {
        $log.info('Modal dismissed at: ' + new Date());
      });
    };
  });
