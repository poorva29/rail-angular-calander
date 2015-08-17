app = angular.module('BookAppointmentApp');

  app.controller('ModalInstanceCtrl', function ($scope, $modalInstance, items) {
    $scope.docLocInfo = items;
    $scope.workingHours = $scope.docLocInfo.workingHours;
    $scope.selected = {};
    $scope.slotTimimgs = [];

    $scope.populateTimeSlot = function(){
      $scope.slotTimimgs = [];
      var hash = {};
      var day_name = moment($scope.current_date).format('dddd');
      hash[day_name] = true;
      $scope.each($scope.where($scope.workingHours, hash), function(workingHour){
        var fromtime = null, totime = null, hash = {}, constructed_time = null;
        hash = $scope.pick(workingHour, 'fromtime', 'totime');
        fromtime = hash.fromtime.slice(0, -2) + ":" + hash.fromtime.slice(-2);
        totime = hash.totime.slice(0, -2) + ":" + hash.totime.slice(-2);
        fromtime = moment(fromtime, 'hh:mm').format('hh:mm A');
        totime = moment(totime, 'hh:mm').format('hh:mm A');
        constructed_time = fromtime + '-' + totime;
        $scope.slotTimimgs.push({time: constructed_time});
      });
    };

    $scope.currentDateTime = function() {
      $scope.current_date = new Date();
      $scope.startTime = new Date();
      $scope.endTime = new Date();
      $scope.populateTimeSlot();
    };
    $scope.currentDateTime();

    $scope.openCalendar = function($event) {
      $event.stopPropagation();
      $scope.is_open_calendar = !$scope.is_open_calendar;
    };

    $scope.getDataToPost = function() {
      $scope.extend($scope.selected, {
        'docLocationId': $scope.docLocInfo.locationId,
        'delayDate': moment($scope.current_date).format('MM/DD/YYYY'),
        'delayTimeSlot': $scope.slotSelected,
        'time': $scope.time_delay
      });
    };

    $scope.ok = function () {
      $scope.getDataToPost();
      $modalInstance.close($scope.selected);
    };

    $scope.cancel = function () {
      $modalInstance.dismiss('cancel');
    };
  });