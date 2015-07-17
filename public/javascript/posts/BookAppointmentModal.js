// Please note that $modalInstance represents a modal window (instance) dependency.
// It is not the same as the $modal service used above.

angular.module('BookAppointmentApp')
  .controller('BookAppointmentModalInstanceCtrl', function ($scope, $modalInstance, items) {
    $scope.selected_event = items;
    $scope.showPatient = true;
    $scope.dateSelected = $scope.selected_event.start.format('DD MMM YYYY, hh:mm t') + ' - ' +$scope.selected_event.end.format('hh:mm t');
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
  })

angular.module('BookAppointmentApp')
  .controller('BookAppointmentEditModalInstanceCtrl', function ($scope, $modalInstance, items, eventDetails) {
    $scope.selected_event = items;
    $scope.showPatient = $scope.selected_event.event_type == 'booking' ? true : false; // set the showPatient = true to see patient view
    eventDetails.selecteEventSet($scope.selected_event);
    $scope.dateSelectedToEdit = eventDetails.dateSelectedToEdit();
    $scope.fromTimeEdit = eventDetails.fromTimeEdit();
    $scope.toTimeEdit = eventDetails.toTimeEdit();
    $scope.subjectSelected = eventDetails.subjectSelected();
    if($scope.showPatient){
      $scope.patientName = eventDetails.patientName();
      $scope.patientNumber = eventDetails.patientNumber();
      $scope.patientEmail = eventDetails.patientEmail();
      $scope.paymentSelected = eventDetails.paymentSelected();
      $scope.prepayAmount = eventDetails.prepayAmount();
      $scope.prepayAmountBy = eventDetails.prepayAmountBy();
    }else{
      $scope.updatedObject = eventDetails.updatedObject();
    }
    $scope.appointmentTypes = [
      { "id": 1, "label": "Conference Travel", "isDefault": true},
      { "id": 2, "label": "IPD"},
      { "id": 3, "label": "OPD"},
      { "id": 4, "label": "OPT Schedule"},
      { "id": 5, "label": "Inscheduled Emergencies"}
    ];

    $scope.toggleView = function(){
      $scope.showPatient = !$scope.showPatient;
    };

    $scope.changeType = function(){
      $scope.toggleView();
    };

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
  })
  .factory('eventDetails', ['$window', function(win) {
    var selected_event = null;
    return {
      selecteEventSet: function(event_hash){
        selected_event = event_hash;
      },

      dateSelectedToEdit: function() {
        return selected_event.start.format('DD MMM YYYY');
      },

      fromTimeEdit: function(){
        return selected_event.start.format('hh:mm t');
      },

      toTimeEdit: function(){
        return selected_event.end.format('hh:mm t');
      },

      subjectSelected: function(){
        return selected_event.subject || ' - ';
      },

      patientName: function(){
        return selected_event.patient_name || ' - ';
      },

      patientNumber: function(){
        return parseInt(selected_event.mobile_number || ' - ');
      },

      patientEmail: function(){
        return selected_event.email || ' - ';
      },

      paymentSelected: function(){
        return true;
      },

      prepayAmount: function(){
        return selected_event.prepay_amount || ' - ';
      },

      prepayAmountBy: function(){
        return selected_event.prepay_date + ' - ' + selected_event.prepay_time || ' - ';
      },

      updatedObject: function(){
        return selected_event.appointment_type || 1;
      }
    };
  }]);

angular.module('BookAppointmentApp')
  .controller('pastTimeModalInstanceCtrl', function ($scope, $modalInstance, items, eventDetails) {
    $scope.selected_event = items;
    $scope.showPatient = $scope.selected_event.event_type == 'booking' ? true : false; // set the showPatient = true to see patient view
    eventDetails.selecteEventSet($scope.selected_event);
    $scope.dateSelectedToEdit = eventDetails.dateSelectedToEdit();
    $scope.fromTimeEdit = eventDetails.fromTimeEdit();
    $scope.toTimeEdit = eventDetails.toTimeEdit();
    $scope.subjectSelected = eventDetails.subjectSelected();
    if($scope.showPatient){
      $scope.patientName = eventDetails.patientName();
      $scope.patientNumber = eventDetails.patientNumber();
      $scope.patientEmail = eventDetails.patientEmail();
      $scope.paymentSelected = eventDetails.paymentSelected();
      $scope.prepayAmount = eventDetails.prepayAmount();
      $scope.prepayAmountBy = eventDetails.prepayAmountBy();
    }else{
      $scope.updatedObject = eventDetails.updatedObject();
    }
    $scope.appointmentTypes = [
      { "id": 1, "label": "Conference Travel", "isDefault": true},
      { "id": 2, "label": "IPD"},
      { "id": 3, "label": "OPD"},
      { "id": 4, "label": "OPT Schedule"},
      { "id": 5, "label": "Inscheduled Emergencies"}
    ];

    $scope.toggleView = function(){
      $scope.showPatient = !$scope.showPatient;
    };

    $scope.changeType = function(){
      $scope.toggleView();
    };

    $scope.ok = function () {
      $modalInstance.close();
    };

    $scope.cancel = function () {
      $modalInstance.dismiss('cancel');
    };
  });
