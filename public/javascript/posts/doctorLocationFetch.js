var app = angular.module('BookAppointmentApp', ['ui.calendar', 'ui.bootstrap', 'angular-underscore', 'flash', 'dnTimepicker', 'ngSanitize', 'ui.select', 'angular-bootbox']);
  app.controller('doctorLocationFetchCtrl', function($scope, $modal, $log, $http, Flash) {
    $scope.doctors = [{ id: 0, firstname: '----- Select -----', lastname: ''}];
    $scope.doctorId = $scope.doctors[0].id;
    $scope.locations = [];
    $scope.doctorsLocations = null;
    $scope.disableLocation = true;
    $scope.location = {};

    $http.get("../api/calendar/doclocations")
      .success(function(response) {
        $scope.doctorsLocations = response;
        $scope.each($scope.doctorsLocations, function(doc_loc) {
          $scope.doctors.push($scope.pick(doc_loc, 'id', 'firstname', 'lastname'));
        });
      });

    $scope.getLocations = function(doctorId) {
      if(doctorId == 0 || typeof doctorId === 'undefined') {
        $scope.disableLocation = true;
        $scope.locations = [];
        $scope.location.selected = undefined;
        $scope.doctors.selected = undefined;
        $scope.initRestId(null);
      } else {
        var doctorDetails = $scope.findWhere($scope.doctorsLocations, { id: doctorId });
        if (doctorDetails) {
          $scope.disableLocation = false;
          $scope.locations = doctorDetails.locations;
          if ($scope.locations.length > 0) {
            if ($scope.findWhere($scope.locations, { id: -1 }) === undefined)
              $scope.locations.push({ 'id': -1, 'name': 'All' });
            $scope.location.selected = $scope.locations[0];
            $scope.doctorId = doctorId;
            $scope.fetchCalenderForDoctorLocation($scope.location.selected.id);
          }
        }
      }
    };

    $scope.fetchCalenderForDoctorLocation = function(locationId) {
      if (locationId)
        $scope.initRestId(locationId);
    }

    $scope.initRestId = function(locationId) {
      $scope.$root.$broadcast("doctorLocation", {
        locationId: locationId,
        doctorId: $scope.doctorId
      });
    };
});
