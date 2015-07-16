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
    $scope.dateSelected = $scope.selected_event.start.format('d MMM YYYY, hh:mm t') + ' - ' + $scope.selected_event.end.format('hh:mm t');
    $scope.appointmentTypes = [
      { "id": 1, "label": "Conference Travel", "isDefault": true},
      { "id": 2, "label": "IPD"},
      { "id": 3, "label": "OPD"},
      { "id": 4, "label": "OPT Schedule"},
      { "id": 5, "label": "Inscheduled Emergencies"}
    ];

    $scope.startTime = new Date($scope.selected_event.start);
    $scope.endTime = new Date($scope.selected_event.end);

    $scope.toggleView = function(){
      $scope.showPatient = !$scope.showPatient;
    }

    $scope.changeType = function(){
      $scope.toggleView();
    }

    $scope.ok = function () {
      $scope.startTime.setDate($scope.current_date.getDate());
      $scope.startTime.setMonth($scope.current_date.getMonth());
      $scope.startTime.setFullYear($scope.current_date.getFullYear());
      $scope.endTime.setDate($scope.current_date.getDate());
      $scope.endTime.setMonth($scope.current_date.getMonth());
      $scope.endTime.setFullYear($scope.current_date.getFullYear());

      $scope.selected_event.event.start = $scope.startTime;
      $scope.selected_event.event.end = $scope.endTime;

      $modalInstance.close($scope.selected_event);
    };

    $scope.delete = function () {
      $scope.selected_event.changeCloseType = true;
      $modalInstance.close($scope.selected_event);
    };

    $scope.cancel = function () {
      $modalInstance.dismiss('cancel');
    };

    $scope.currentDay = function() {
      $scope.current_date = new Date($scope.selected_event.start);
    };
    $scope.currentDay();

    $scope.clear = function () {
      $scope.current_date = null;
    };

    $scope.open = function($event) {
      $event.preventDefault();
      $event.stopPropagation();

      $scope.opened = true;
    };

    $scope.dateOptions = {
      formatYear: 'yy',
      startingDay: 1
    };

  });
