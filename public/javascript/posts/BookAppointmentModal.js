// Please note that $modalInstance represents a modal window (instance) dependency.
// It is not the same as the $modal service used above.

angular.module('BookAppointmentApp')
  .controller('BookAppointmentModalInstanceCtrl', function ($scope, $modalInstance, items) {
    $scope.selected_event = {};
    $scope.selected_event = items;
    $scope.showPatient = true;
    $scope.dateSelected = $scope.selected_event.start.format('DD MMM YYYY, hh:mm t') + ' - ' +$scope.selected_event.end.format('hh:mm t');
    $scope.updatedObject = [{ "id": 1, "label": "Conference Travel", "isDefault": true}]; //show default value in appointmentType dropdown
    $scope.appointmentTypes = [
      { "id": 2, "label": "IPD"},
      { "id": 3, "label": "OPD"},
      { "id": 4, "label": "OPT Schedule"},
      { "id": 5, "label": "Inscheduled Emergencies"}
    ];
    $scope.submitted = false;

    $scope.toggleView = function(){
      $scope.showPatient = !$scope.showPatient;
    }

    $scope.changeType = function(){
      $scope.toggleView();
    }

    $scope.addInfo = function(){
      $scope.selected_event.doctor_id = 1;
      $scope.selected_event.location_id = 2;
      $scope.selected_event.cretaed_date = new Date(); //the date on which appointment is booked
      $scope.selected_event.created_by = 1; //(doctor or patient or assistant) id who is creating an appointment
      $scope.selected_event.subject = $scope.subjectSelected;
      $scope.selected_event.start = $scope.selected_event.start;
      $scope.selected_event.end = $scope.selected_event.end;
      $scope.selected_event.is_all_day_event = false;
      $scope.selected_event.appointment_type = $scope.updatedObject;
      $scope.selected_event.event_type = $scope.radioAppointment.selected_type == '1' ? 'blocked' : 'booking'; // the event is created for doctor or patient
      $scope.selected_event.cancel_overlapped_event = false;
      $scope.selected_event.patient_type = "unregpatient";
      $scope.selected_event.patient_name = $scope.patientName;
      $scope.selected_event.mobile_number = $scope.patientNumber;
      $scope.selected_event.email = $scope.patientEmail;
      $scope.selected_event.prepay_amount = $scope.prepayAmount;
      $scope.selected_event.prepay_by = $scope.prepayAmountBy;
    }

    $scope.ok = function () {
      if($scope.showPatient && $scope.appointmentForm.$invalid){
        $scope.submitted = true;
      }else{
        $scope.submitted = false;
      }

      if(!$scope.submitted){
        $scope.addInfo();
        $modalInstance.close($scope.selected_event);
      }
    };

    $scope.cancel = function () {
      $modalInstance.dismiss('cancel');
    };
  })

angular.module('BookAppointmentApp')
  .controller('BookAppointmentEditModalInstanceCtrl', function ($scope, $modalInstance, items, eventDetails) {
    $scope.selected_event = items;
    $scope.showPatient = $scope.selected_event.event_type == 'booking' ? true : false;
    eventDetails.selecteEventSet($scope.selected_event);
    $scope.dateSelectedToEdit = eventDetails.dateSelectedToEdit();
    $scope.fromTimeEdit = eventDetails.fromTimeEdit();
    $scope.toTimeEdit = eventDetails.toTimeEdit();
    $scope.subjectSelected = eventDetails.subjectSelected();
    $scope.appointmentTypes = [
      { "id": 1, "label": "Conference Travel", "isDefault": true},
      { "id": 2, "label": "IPD"},
      { "id": 3, "label": "OPD"},
      { "id": 4, "label": "OPT Schedule"},
      { "id": 5, "label": "Inscheduled Emergencies"}
    ];
    if($scope.showPatient){
      $scope.patientName = eventDetails.patientName();
      $scope.patientNumber = eventDetails.patientNumber();
      $scope.patientEmail = eventDetails.patientEmail();
      $scope.paymentSelected = eventDetails.paymentSelected();
      $scope.prepayAmount = eventDetails.prepayAmount();
      $scope.prepayAmountBy = eventDetails.prepayAmountBy();
    }else{
      var current_appointment_type = "";
      current_appointment_type = $scope.findWhere($scope.appointmentTypes, {"label": $scope.selected_event.appointment_type});
      $scope.updatedObject = current_appointment_type.label;
      $scope.appointmentTypes = $scope.without($scope.appointmentTypes, current_appointment_type);
    }

    var current_appointment_type = "";
    $scope.subjectSelected = $scope.selected_event.subject;

    if($scope.selected_event.appointment_type){
      current_appointment_type = $scope.findWhere($scope.appointmentTypes, {"id": $scope.selected_event.appointment_type});
      $scope.updatedObject = current_appointment_type.label;
      $scope.appointmentTypes = $scope.without($scope.appointmentTypes, current_appointment_type);
    }

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
    $scope.appointmentTypes = [
      { "id": 1, "label": "Conference Travel", "isDefault": true},
      { "id": 2, "label": "IPD"},
      { "id": 3, "label": "OPD"},
      { "id": 4, "label": "OPT Schedule"},
      { "id": 5, "label": "Inscheduled Emergencies"}
    ];
    if($scope.showPatient){
      $scope.patientName = eventDetails.patientName();
      $scope.patientNumber = eventDetails.patientNumber();
      $scope.patientEmail = eventDetails.patientEmail();
      $scope.paymentSelected = eventDetails.paymentSelected();
      $scope.prepayAmount = eventDetails.prepayAmount();
      $scope.prepayAmountBy = eventDetails.prepayAmountBy();
    }else{
      var current_appointment_type = "";
      current_appointment_type = $scope.findWhere($scope.appointmentTypes, {"label": $scope.selected_event.appointment_type});
      $scope.updatedObject = current_appointment_type.label;
      $scope.appointmentTypes = $scope.without($scope.appointmentTypes, current_appointment_type);
    }

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
