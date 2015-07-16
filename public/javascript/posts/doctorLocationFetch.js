var app = angular.module('BookAppointmentApp', ['ui.calendar', 'ui.bootstrap', 'angular-underscore', 'flash']);
  app.controller('doctorLocationFetchCtrl',function($scope, $modal, $log, $http, Flash) {
    $scope.locationId = null;
    $scope.doctors = [{id: 0, firstname: '----- Select -----', lastname: ''}];
    $scope.doctorId = $scope.doctors[0].id;
    $scope.locations = [];
    $scope.doctorsLocations = null;
    $scope.disableLocation = true;

    $http.get("/api/calendar/doclocations")
      .success(function (response) {
        $scope.doctorsLocations = response;
        $scope.each($scope.doctorsLocations, function(doc_loc){
          $scope.doctors.push($scope.pick(doc_loc, 'id', 'firstname', 'lastname'));
        });
    });

    $scope.getLocations = function(doctorId){
      if(doctorId == 0){
        $scope.disableLocation = true;
        $scope.locations = [];
        $scope.initRestId(null);
      }else{
        var doctorDetails = $scope.findWhere($scope.doctorsLocations, {id: doctorId});
        if(doctorDetails){
          $scope.disableLocation = false;
          $scope.locations = doctorDetails.locations;
          $scope.locationId = $scope.locations[0].id;
          $scope.fetchCalenderForDoctorLocation($scope.locations[0].id);
        }
      }
    };

    $scope.fetchCalenderForDoctorLocation = function(locationId){
      if(locationId)
        $scope.initRestId(locationId);
    }

    $scope.initRestId = function(locationId){
      $scope.$root.$broadcast("doctorLocation",{
          locationId: locationId
        });
    };
  });