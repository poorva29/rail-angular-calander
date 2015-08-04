// Please note that $modalInstance represents a modal window (instance) dependency.
// It is not the same as the $modal service used above.

app = angular.module('BookAppointmentApp');

  app.filter('propsFilter', function() {
    return function(items, props) {
      var out = [];

      if (angular.isArray(items)) {
        items.forEach(function(item) {
          var itemMatches = false;

          var keys = Object.keys(props);
          for (var i = 0; i < keys.length; i++) {
            var prop = keys[i];
            var text = props[prop].toLowerCase();
            if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
              itemMatches = true;
              break;
            }
          }

          if (itemMatches) {
            out.push(item);
          }
        });
      } else {
        // Let the output be the input untouched
        out = items;
      }

      return out;
    };
  });

  app.controller('BookAppointmentModalInstanceCtrl', function ($scope, $http ,$modalInstance, items) {
    $scope.selected_event = {};
    $scope.selected_event = items;
    $scope.showPatient = true;
    $scope.prepay_time = new Date();
    $scope.dateSelected = $scope.selected_event.start.format('DD MMM YYYY, hh:mm a') + ' - ' +$scope.selected_event.end.format('hh:mm a');
    $scope.updatedObject = [{ "id": 1, "label": "Conference Travel", "isDefault": true}]; //show default value in appointmentType dropdown
    $scope.appointmentTypes = [
      { "id": 1, "label": "Conference Travel", "isDefault": true},
      { "id": 2, "label": "IPD"},
      { "id": 3, "label": "OPD"},
      { "id": 4, "label": "OT Schedule"},
      { "id": 5, "label": "Unscheduled Emergencies"}
    ];
    $scope.submitted = false;

    $scope.patient = {
      registered: false
    }

    $scope.patientsInfo = null;
    var url_to_fetch = '../api/calendar/patients?doctor_id=' + $scope.selected_event.doctorId + '&doclocation_id=' + $scope.selected_event.locationId;
    $http.get(url_to_fetch)
      .success(function (response) {
        $scope.patientsInfo = response;
    });

    $scope.refreshPatientNames = function(name){
      console.log(name);
    };

    $scope.fillPatientDetails = function(){
      if($scope.patient.selected){
        var patientDetails = $scope.findWhere($scope.patientsInfo, {id: $scope.patient.selected.id});
        $scope.patientNumber = patientDetails.mobile ? parseInt(patientDetails.mobile): '';
        $scope.patientEmail = patientDetails.email;
        $scope.patient.registered = true;
      }else{
        $scope.patientNumber = undefined;
        $scope.patientEmail = undefined;
        $scope.patient.registered = false;
      }
    };

    $scope.toggleView = function(){
      $scope.showPatient = !$scope.showPatient;
    };

    $scope.changeType = function(){
      $scope.toggleView();
    };

    $scope.addInfo = function(){
      $scope.selected_event.doctor_id = 1;
      $scope.selected_event.location_id = 2;
      $scope.selected_event.cretaed_date = new Date(); //the date on which appointment is booked
      $scope.selected_event.created_by = 1; //(doctor or patient or assistant) id who is creating an appointment
      $scope.selected_event.subject = $scope.subjectSelected;
      $scope.selected_event.start = $scope.selected_event.start;
      $scope.selected_event.end = $scope.selected_event.end;
      $scope.selected_event.is_all_day_event = false;
      $scope.selected_event.appointment_type = $scope.radioAppointment.selected_type == '1' ? $scope.updatedObject : 'Patient Appointment';
      $scope.selected_event.event_type = 'booking'; // the event is created for doctor or patient
      $scope.selected_event.cancel_overlapped_event = false;
      $scope.selected_event.patient_type = "unregpatient";
      $scope.selected_event.patient_name = $scope.patient.registered ? $scope.patient.selected.name : $('.ui-select-search').val();
      $scope.selected_event.mobile_number = $scope.patientNumber;
      $scope.selected_event.email = $scope.patientEmail;
      $scope.selected_event.prepay_amount = $scope.prepayAmount;
      $scope.selected_event.prepay_by = moment($scope.prepay_date).format('MM/DD/YYYY') + ' ' + moment($scope.prepay_time).format('HH:mm:ss');
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

    $scope.currentDay = function() {
      $scope.prepay_date = new Date();
    };
    $scope.currentDay();

    $scope.clear = function () {
      $scope.current_date = null;
    };

    $scope.dateOptions = {
      formatYear: 'yy',
      startingDay: 1
    };
  });

angular.module('BookAppointmentApp')
  .controller('BookAppointmentEditModalInstanceCtrl', function ($scope, $modalInstance, items, eventDetails) {
    $scope.selected_event = items;
    var appointment_type = $scope.selected_event.appointment_type;
    var appointment_type_check = (appointment_type == 'Patient Appointment' || appointment_type == 0 || appointment_type == -1);
    $scope.showPatient =  appointment_type_check ? true : false;
    eventDetails.selecteEventSet($scope.selected_event);
    $scope.dateSelectedToEdit = eventDetails.dateSelectedToEdit();
    $scope.fromTimeEdit = eventDetails.fromTimeEdit();
    $scope.toTimeEdit = eventDetails.toTimeEdit();
    $scope.subjectSelected = eventDetails.subjectSelected();
    $scope.prepay_time = eventDetails.prepayTime();
    $scope.appointmentTypes = [
      { "id": 1, "label": "Conference Travel", "isDefault": true},
      { "id": 2, "label": "IPD"},
      { "id": 3, "label": "OPD"},
      { "id": 4, "label": "OT Schedule"},
      { "id": 5, "label": "Unscheduled Emergencies"}
    ];
    if($scope.showPatient){
      $scope.patientName = eventDetails.patientName();
      $scope.patientNumber = eventDetails.patientNumber();
      $scope.patientEmail = eventDetails.patientEmail();
      $scope.paymentSelected = eventDetails.paymentSelected();
      $scope.prepayAmount = eventDetails.prepayAmount();
      $scope.prepayAmountBy = eventDetails.prepayAmountBy();
      if($scope.selected_event.prepay_amount > 0)
        $scope.check_if_paid = $scope.selected_event.is_paid ? 'prepay-symbol-green' : 'prepay-symbol-red';
    }else{
      var current_appointment_type = "";
      current_appointment_type = $scope.findWhere($scope.appointmentTypes, {"id": $scope.selected_event.appointment_type});
      $scope.current_appt = current_appointment_type.label;
      $scope.updatedObject = current_appointment_type.id;
      $scope.copy_of_appt_types = $scope.appointmentTypes;
      $scope.appointmentTypes = $scope.without($scope.appointmentTypes, current_appointment_type);
    }

    $scope.startTime = new Date($scope.selected_event.start);
    $scope.endTime = new Date($scope.selected_event.end);

    $scope.toggleView = function(){
      $scope.showPatient = !$scope.showPatient;
    };

    $scope.changeType = function(){
      $scope.toggleView();
    };

    $scope.ok = function () {
      $scope.selected_event.event.subject = $scope.subjectSelected;
      if($scope.updatedObject){
        $scope.selected_event.event.title = ($scope.findWhere($scope.copy_of_appt_types, {"id": $scope.updatedObject})).label;
      }

      $scope.extend($scope.selected_event, {
        'subject': $scope.subjectSelected,
        'appointment_type': $scope.updatedObject,
        'prepay_amount': $scope.prepayAmount,
        'email': $scope.patientEmail || '',
        'mobile_number': $scope.patientNumber || '',
        'prepayBy': $scope.prepayAmountBy
      });
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
      $scope.prepay_date = new Date();
    };
    $scope.currentDay();

    $scope.clear = function () {
      $scope.current_date = null;
    };

    $scope.dateOptions = {
      formatYear: 'yy',
      startingDay: 1
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
        return selected_event.start.format('hh:mm a');
      },

      toTimeEdit: function(){
        return selected_event.end.format('hh:mm a');
      },

      subjectSelected: function(){
        return selected_event.subject || ' - ';
      },

      patientName: function(){
        return selected_event.name || selected_event.patient_name || ' - ';
      },

      patientNumber: function(){
        return parseInt(selected_event.mobile_number || ' - ');
      },

      patientEmail: function(){
        return selected_event.email || ' - ';
      },

      paymentSelected: function(){
        return selected_event.prepay_amount > 0 ? true : false;
      },

      prepayAmount: function(){
        return selected_event.prepay_amount || 0.0;
      },

      prepayAmountBy: function(){
        return selected_event.prepay_by || (selected_event.prepay_date + ' - ' + selected_event.prepay_time) || ' - ';
      },

      updatedObject: function(){
        return selected_event.appointment_type || 1;
      },

      prepayTime: function(){
        var time = selected_event.prepay_by ? selected_event.prepay_by : new Date();
        return moment(time).format('hh:mm A');
      },

      dateSelectedToEdit: function(){
        return moment(selected_event.prepay_by).format('DD MMM YYYY')
      },

      fromTimeEdit: function(){
        return moment(selected_event.prepay_by).format('hh:mm A');
      }
    };
  }]);

angular.module('BookAppointmentApp')
  .controller('pastTimeModalInstanceCtrl', function ($scope, $modalInstance, items, eventDetails) {
    $scope.selected_event = items;
    var appointment_type = $scope.selected_event.appointment_type;
    var appointment_type_check = (appointment_type == 'Patient Appointment' || appointment_type == 0 || appointment_type == -1);
    $scope.showPatient =  appointment_type_check ? true : false; // set the showPatient = true to see patient view
    eventDetails.selecteEventSet($scope.selected_event);
    $scope.dateSelectedToEdit = eventDetails.dateSelectedToEdit();
    $scope.fromTimeEdit = eventDetails.fromTimeEdit();
    $scope.toTimeEdit = eventDetails.toTimeEdit();
    $scope.subjectSelected = eventDetails.subjectSelected();
    $scope.prepay_time = eventDetails.prepayTime();
    $scope.appointmentTypes = [
      { "id": 1, "label": "Conference Travel", "isDefault": true},
      { "id": 2, "label": "IPD"},
      { "id": 3, "label": "OPD"},
      { "id": 4, "label": "OT Schedule"},
      { "id": 5, "label": "Unscheduled Emergencies"}
    ];
    if($scope.showPatient){
      $scope.patientName = eventDetails.patientName();
      $scope.patientNumber = eventDetails.patientNumber();
      $scope.patientEmail = eventDetails.patientEmail();
      $scope.paymentSelected = eventDetails.paymentSelected();
      $scope.prepayAmount = eventDetails.prepayAmount();
      $scope.prepayAmountBy = eventDetails.prepayAmountBy();
      $scope.dateSelectedToEdit = eventDetails.dateSelectedToEdit();
      $scope.fromTimeEdit = eventDetails.fromTimeEdit();
    }else{
      var current_appointment_type = "";
      current_appointment_type = $scope.findWhere($scope.appointmentTypes, {"id": $scope.selected_event.appointment_type});
      $scope.updatedObject = current_appointment_type.id;
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
