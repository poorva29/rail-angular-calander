
var app = angular.module('BookAppointmentApp', ['ui.calendar', 'ui.bootstrap', 'angular-underscore', 'flash',
                         'dnTimepicker', 'ngSanitize', 'ui.select', 'angular-bootbox', 'ngTable', 'ngIdle']);
  app.controller('doctorLocationFetchCtrl', function ($scope, $modal, $log, $http, Flash, ngTableParams, Idle, Keepalive) {
    $scope.doctors = [{ id: 0, firstname: ' ----- Select -----', lastname: ''}];
    $scope.doctorId = $scope.doctors[0].id;
    $scope.locations = [];
    $scope.doctorsLocations = null;
    $scope.disableLocation = true;
    $scope.location = {};
    $scope.todays_date = moment(new Date()).format('ddd, DD MMM YYYY');

    $http.get("../api/calendar/doclocations")
      .success(function(response) {
          $scope.doctorsLocations = response;
          $scope.each($scope.doctorsLocations, function(doc_loc) {
              $scope.doctors.push($scope.pick(doc_loc, 'id', 'firstname', 'lastname'));
          });
      });

    $scope.showAlert = function(type, message) {
      Flash.create(type, message);
    };

    $scope.locationNotSelected = function() {
      var message = '<strong> Select location !</strong>  Doctor Or Location Not Selected.';
      $scope.showAlert('danger', message);
    };

    $scope.delayMessageSent = function() {
      var message = '<strong> Delay Message Sent !</strong> Patients Will Be Notified By SMS .';
      $scope.showAlert('success', message);
    };

    $scope.delayMessageNotSent = function() {
      var message = '<strong> Delay Message NOT Sent !</strong> Patients Will NOT Be Notified.';
      $scope.showAlert('danger', message);
    };

    $scope.noLocationsAvailable = function() {
      $scope.location.selected = 'undefined';
      $scope.$root.showCalendar = false;
      var message = '<strong> No Clinic Added !</strong> Doctor Does Not Have Locations Added.';
      $scope.showAlert('danger', message);
    };

    $scope.getLocations = function(doctorId) {
      if(doctorId == 0 || typeof doctorId === 'undefined') {
        $scope.disableLocation = true;
        $scope.locations = [];
        $scope.doctorId = doctorId;
        $scope.location.selected = undefined;
        $scope.doctors.selected = undefined;
        $scope.initRestId(null);
        } else {
          var doctorDetails = $scope.findWhere($scope.doctorsLocations, { id: doctorId });
          if (doctorDetails) {
          $scope.disableLocation = false;
          $scope.locations = doctorDetails.locations;
          $scope.doctor = doctorDetails;
          if ($scope.locations.length > 0) {
            if ($scope.findWhere($scope.locations, { id: -1 }) === undefined)
                $scope.locations.push({ 'id': -1, 'name': 'All' });
            $scope.location.selected = $scope.locations[0];
            $scope.doctorId = doctorId;
            $scope.fetchCalenderForDoctorLocation($scope.location.selected.id);
          }else{
            $scope.noLocationsAvailable();
          }
        }
      }
    };

    $scope.fetchCalenderForDoctorLocation = function(locationId) {
      if (locationId) {
        $scope.$root.showCalendar = true;
        $scope.show_todays_schedule = false;
        $scope.initRestId(locationId);
      }
    }

    $scope.initRestId = function(locationId) {
      $scope.$root.$broadcast("doctorLocation", {
        locationId: locationId,
        doctorId: $scope.doctorId,
        locations: $scope.locations
      });
    };

    $scope.getTodaysSchedule = function () {
      if($scope.location.selected){
        $http.get('../api/calendar/todays_schedule', {
          params: {
            doclocation_id: $scope.location.selected.id,
            doctor_id: $scope.doctorId
          }
        }).success(function (data, status) {
            $scope.todays_appointments = data;
        });
        $scope.show_todays_schedule = true;
        $scope.$root.showCalendar = false;
      }else{
        $scope.locationNotSelected();
      }
    };

    $scope.doctorIdValid = function() {
      if($scope.doctorId === 0 || typeof $scope.doctorId === 'undefined') {
        return false;
      } else {
        return true;
      }
    }

    $scope.showCalendar = function () {
      $scope.show_todays_schedule = false;
      $scope.$root.showCalendar = true;
    }

    $scope.$on("workingHours", function (event, args) {
      $scope.workingHours = args.workingHours;
    });

    $scope.delay_appointment = function() {
      if($scope.location.selected) {
        $scope.animationsEnabled = true;

        $scope.open = function(size) {

          var modalInstance = $modal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            size: size,
            resolve: {
              items: function() {
                return {
                  'locationId': $scope.location.selected.id,
                  'workingHours': $scope.workingHours
                };
              }
            }
          });

          modalInstance.result.then(function(selectedItem) {
            $scope.selected = selectedItem;
            var url_to_post = '../api/calendar/delay_appointments';
            $http.post(url_to_post, $scope.selected)
              .success(function(response) {
                if(response.success) {
                  $scope.delayMessageSent();
                } else {
                  $scope.delayMessageNotSent();
                }
              });
          }, function() {
            $log.info('Modal dismissed at: ' + new Date());
          });
        };

        $scope.toggleAnimation = function() {
          $scope.animationsEnabled = !$scope.animationsEnabled;
        };
        $scope.open('');

      }else{
        $scope.locationNotSelected();
      }
    };

    // This is implemented so that the user does not logout
    $scope.$on('Keepalive', function() {
      $http.get("../api/calendar/ping")
      .success(function(response) {
      })
      .error(function(response, status) {
        if (status == 403) {
          var url = "http://" + $window.location.host + "/User/logout";
          $window.location.href = url;
        }
      });
    });
  });

  app.config(['KeepaliveProvider', 'IdleProvider', function(KeepaliveProvider, IdleProvider) {
    IdleProvider.idle(120);
    IdleProvider.timeout(1);
    KeepaliveProvider.interval(120);
  }])
  .run(function(Idle) {
    Idle.watch();
  });
