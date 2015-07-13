angular.module('BookAppointmentApp',['ui.calendar', 'ui.bootstrap', 'angular-underscore', 'flash'])
  .controller('BookAppointmentCtrl',function($scope, $modal, $log, Flash) {
    /* Calendar specific changes
      This has calendar configurations and event binding for the calendar
    */

    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();

    $scope.alertOnEventClick = function(event, jsEvent, view){
      $scope.openEdit(event, jsEvent, view, '');
      // $scope.alertMessage = (event.title + ' was clicked ');
    };
    /* alert on Drop */
    $scope.alertOnDrop = function(event, delta, revertFunc, jsEvent, ui, view){
      // $scope.alertMessage = ('Event Droped to make dayDelta ' + delta);
    };
    /* alert on Resize */
    $scope.alertOnResize = function(event, delta, revertFunc, jsEvent, ui, view ){
      // $scope.alertMessage = ('Event Resized to make dayDelta ' + delta);
    };

    $scope.stopEventOverloap = function(start, end){
      for(i in $scope.events){
        if (end > $scope.events[i].start && start < $scope.events[i].end){
          return true;
        }
      }
      return false;
    };

    $scope.showAlert = function (type, message) {
      Flash.create(type, message);
    }

    $scope.slotSelected = function(start, end, jsEvent, view){
      // start.format('hh:mm') , start.hours()
        if($scope.stopEventOverloap(start, end)){
          var message = '<strong> Not Booked !</strong>  Appointment Booked For Selected Time.';
          $scope.showAlert('danger', message);
          $('#appointmentBookingCalendar').fullCalendar('unselect');
        }else{
          $scope.open(start, end, jsEvent, view, '');
        }
    };

    $scope.generateUniqueEventId = function(start_date){
      return parseInt(start_date.format('MDDYYYY')+ '' + Math.floor(Math.random() * 10000) + 1);
    };

    $scope.events = [
      {
        id: $scope.generateUniqueEventId(moment(new Date(y, m, d + 1))),
        title: 'Birthday Party',
        start: moment.utc(new Date(y, m, d + 1, 19, 30)),
        end: moment.utc(new Date(y, m, d + 1, 22, 30)),
        stick: true
      },
    ];

    $scope.uiConfig = {
      calendar:{
        defaultView: 'agendaWeek',
        height: 550,
        editable: true,
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
        eventDrop: $scope.alertOnDrop,
        eventResize: $scope.alertOnResize,
        eventRender: $scope.eventRender,
        select: $scope.slotSelected
      }
    };
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
        var message = '<strong> Booked !</strong>  Appointment Created Successfully.';
        $scope.showAlert('success', message);
      }, function () {
        $log.info('Modal dismissed at: ' + new Date());
      });
    };

    $scope.openEdit = function (event, jsEvent, view, size) {

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
        }
      }, function () {
        $log.info('Modal dismissed at: ' + new Date());
      });
    };
  });
