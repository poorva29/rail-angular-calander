angular.module('BookAppointmentApp',['ui.calendar', 'ui.bootstrap', 'angular-underscore'])
  .controller('BookAppointmentCtrl',function($scope, $modal, $log) {
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

    $scope.slotSelected = function(start, end, jsEvent, view){
      // start.format('hh:mm') , start.hours()
      $scope.open(start, end, jsEvent, view, '');
    };

    $scope.generateUniqueEventId = function(start_date){
      return parseInt(start_date.format('MDDYYYY')+ '' + Math.floor(Math.random() * 10000) + 1);
    };

    $scope.events = [
      {
        id: $scope.generateUniqueEventId(moment(new Date(y, m, d + 1))),
        title: 'Birthday Party',
        start: new Date(y, m, d + 1, 19, 0),
        end: new Date(y, m, d + 1, 22, 0),
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
        eventClick: $scope.alertOnEventClick,
        eventDrop: $scope.alertOnDrop,
        eventResize: $scope.alertOnResize,
        eventRender: $scope.eventRender,
        select: $scope.slotSelected,
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

      modalInstance.result.then(function (selectedItem) {
        $scope.selected_event = selectedItem;
        $scope.events.push({
          id: $scope.generateUniqueEventId(selectedItem.start),
          title: 'Open Sesame',
          start: $scope.selected_event.start,
          end: $scope.selected_event.end,
          className: ['openSesame'],
          stick: true
        });
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

// Please note that $modalInstance represents a modal window (instance) dependency.
// It is not the same as the $modal service used above.

angular.module('BookAppointmentApp')
  .controller('BookAppointmentModalInstanceCtrl', function ($scope, $modalInstance, items) {
    $scope.selected_event = items;
    $scope.showPatient = true;
    $scope.dateSelected = $scope.selected_event.start.format('d MMM YYYY, hh:mm t') + ' - ' +$scope.selected_event.end.format('hh:mm t');
    $scope.appointmentTypes = [
      { "id": 1, "label": "Conference Travel", "isDefault": true},
      { "id": 2, "label": "IPD"},
      { "id": 3, "label": "OPD"},
      { "id": 4, "label": "OPT Schedule"},
      { "id": 5, "label": "Inscheduled Emergencies"}
    ];

    $scope.toggleView = function(){
      $scope.showPatient = !$scope.showPatient;
    }

    $scope.changeType = function(){
      $scope.toggleView();
    }

    $scope.ok = function () {
      $modalInstance.close($scope.selected_event);
    };

    $scope.cancel = function () {
      $modalInstance.dismiss('cancel');
    };
  });

angular.module('BookAppointmentApp')
  .controller('BookAppointmentEditModalInstanceCtrl', function ($scope, $modalInstance, items) {
    $scope.selected_event = items;
    $scope.showPatient = false; // set the showPatient = true to see patient view
    $scope.dateSelected = $scope.selected_event.start.format('d MMM YYYY, hh:mm t') + ' - ' +$scope.selected_event.end.format('hh:mm t');
    $scope.appointmentTypes = [
      { "id": 1, "label": "Conference Travel", "isDefault": true},
      { "id": 2, "label": "IPD"},
      { "id": 3, "label": "OPD"},
      { "id": 4, "label": "OPT Schedule"},
      { "id": 5, "label": "Inscheduled Emergencies"}
    ];

    $scope.toggleView = function(){
      $scope.showPatient = !$scope.showPatient;
    }

    $scope.changeType = function(){
      $scope.toggleView();
    }

    $scope.ok = function () {
      $modalInstance.close($scope.selected_event);
    };

    $scope.delete = function () {
      $scope.selected_event.changeCloseType = true;
      $modalInstance.close($scope.selected_event);
    };

    $scope.cancel = function () {
      $modalInstance.dismiss('cancel');
    };
  });
