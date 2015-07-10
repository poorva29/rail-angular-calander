angular.module('BookAppointmentApp',['ui.calendar', 'ui.bootstrap'])
  .controller('BookAppointmentCtrl',function($scope, $modal, $log) {
    // Calendar specific changes !

    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();

    $scope.alertOnEventClick = function(event, jsEvent, view){
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
    }

    $scope.events = [
      {title: 'Birthday Party',start: new Date(y, m, d + 1, 19, 0),end: new Date(y, m, d + 1, 22, 0),stick: true},
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

    // Modal specific changes !

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
          title: 'Open Sesame',
          start: $scope.selected_event.start,
          end: $scope.selected_event.end,
          className: ['openSesame']
        });
      }, function () {
        $log.info('Modal dismissed at: ' + new Date());
      });
    };

    $scope.toggleAnimation = function () {
      $scope.animationsEnabled = !$scope.animationsEnabled;
    };

  });

// Please note that $modalInstance represents a modal window (instance) dependency.
// It is not the same as the $modal service used above.

angular.module('BookAppointmentApp')
  .controller('BookAppointmentModalInstanceCtrl', function ($scope, $modalInstance, items) {
    $scope.selected_event = items;
    $scope.ok = function () {
      $modalInstance.close($scope.selected_event);
    };

    $scope.cancel = function () {
      $modalInstance.dismiss('cancel');
    };
  });