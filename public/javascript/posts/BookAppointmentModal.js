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
      $scope.patientId = "114";
      $scope.selected_event.doctorId = 1;
      $scope.selected_event.doctorlocationId = 2;
      $scope.selected_event.cretaedDate = new Date(); //the date on which appointment is booked
      $scope.selected_event.createdby = 1; //doctor / patient / assistant id who is creating an appointment
      $scope.selected_event.appointmentTitle = $scope.subjectSelected;
      $scope.selected_event.appointmentStartTime = $scope.selected_event.start;
      $scope.selected_event.appointmentEndTime = $scope.selected_event.end;
      $scope.selected_event.isAllDayEvent = false;
      $scope.selected_event.appointmentType = $scope.radioAppointment.selected_type=='1' ? $scope.updatedObject : '';
      $scope.selected_event.cancelOverlapped = false;
      $scope.selected_event.patienttype = "unregpatient";
      $scope.selected_event.patname = $scope.patientName;
      $scope.selected_event.mobileno = $scope.patientNumber;
      $scope.selected_event.email = $scope.patientEmail;
      $scope.selected_event.prepayAmount = $scope.prepayAmount;
      $scope.selected_event.prepayBy = $scope.prepayAmountBy;
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
    $scope.showPatient = $scope.selected_event.appointmentType ? false : true;
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

    var current_appointment_type = "";
    $scope.subjectSelected = $scope.selected_event.appointmentTitle;

    if($scope.selected_event.appointmentType){
     current_appointment_type = $scope.findWhere($scope.appointmentTypes, {"id": $scope.selected_event.appointmentType});
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
        return selected_event.appointmentTitle || ' - ';
      },

      patientName: function(){
        return selected_event.patname || ' - ';
      },

      patientNumber: function(){
        return parseInt(selected_event.mobileno || ' - ');
      },

      patientEmail: function(){
        return selected_event.email || ' - ';
      },

      paymentSelected: function(){
        return true;
      },

      prepayAmount: function(){
        return selected_event.prepayAmount || ' - ';
      },

      prepayAmountBy: function(){
        return selected_event.prepayBy + ' - ' + selected_event.prepay_time || ' - ';
      },

      updatedObject: function(){
        return selected_event.appointmentType || 1;
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
