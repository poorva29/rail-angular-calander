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

    $scope.addDoctorInfo = function(){
      $scope.selected_event.doctor_id = 1;
      $scope.selected_event.location_id = 2;
      $scope.selected_event.subject = $scope.subjectSelected;
      $scope.selected_event.appointment_type = $scope.updatedObject;
      $scope.selected_event.event_type = 'blocked';
    };

    $scope.addPatientInfo = function(){
      var name = $scope.patientName ? $scope.patientName.split(' '): '';
      $scope.selected_event.doctor_id = 1;
      $scope.selected_event.location_id = 2;
      $scope.selected_event.first_name = name[0];
      $scope.selected_event.last_name = name[1];
      $scope.selected_event.email = $scope.patientEmail;
      $scope.selected_event.mobile_number = $scope.patientNumber;
      $scope.selected_event.subject = $scope.subjectSelected;
      $scope.selected_event.event_type = 'booking';
      $scope.selected_event.prepay_amount = $scope.prepayAmount;
      $scope.selected_event.prepay_date = $scope.prepayAmountBy;
      $scope.selected_event.prepay_time = '';
    };

    $scope.ok = function () {
      switch($scope.radioAppointment.selected_type){
        case 1:
                $scope.addDoctorInfo();
                break;
        case 2:
                $scope.addPatientInfo();
                break;
      }
      $modalInstance.close($scope.selected_event);
    };

    $scope.cancel = function () {
      $modalInstance.dismiss('cancel');
    };
  });

angular.module('BookAppointmentApp')
  .controller('BookAppointmentEditModalInstanceCtrl', function ($scope, $modalInstance, items) {
    $scope.selected_event = items;
    $scope.showPatient = $scope.selected_event.event_type == 'booking' ? true : false; // set the showPatient = true to see patient view
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

angular.module('BookAppointmentApp')
  .controller('pastTimeModalInstanceCtrl', function ($scope, $modalInstance, items) {
    $scope.selected_event = items;
    $scope.showPatient = $scope.selected_event.event_type == 'booking' ? true : false; // set the showPatient = true to see patient view
    $scope.dateSelectedToEdit = "-";
    $scope.fromTimeEdit = '-';
    $scope.toTimeEdit = '-';
    $scope.subjectSelected  = '-';
    $scope.patientName = '-';
    $scope.patientNumber = '-';
    $scope.patientEmail = '-';
    $scope.dateSelectedToEdit = '-';
    $scope.fromTimeEdit = '-';
    $scope.toTimeEdit = '-';
    $scope.subjectSelected = '-';
    $scope.prepayAmount = '-';
    $scope.prepayAmountBy = '-';
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
      $modalInstance.close();
    };

    $scope.cancel = function () {
      $modalInstance.dismiss('cancel');
    };
  });
