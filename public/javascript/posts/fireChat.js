var app = angular.module("BookAppointmentApp");

  app.controller("fireChatCtrl", ["$scope", "$firebaseArray",
    function($scope, $firebaseArray) {
      //CREATE A FIREBASE REFERENCE
      var ref = new Firebase("https://upmxuo8hxo5.firebaseio-demo.com/");

      // GET MESSAGES AS AN ARRAY
      $scope.messages = $firebaseArray(ref);

      //ADD MESSAGE METHOD
      $scope.addMessage = function(e) {

        //LISTEN FOR RETURN KEY
        if (e.keyCode === 13 && $scope.msg) {
          //ALLOW CUSTOM OR ANONYMOUS USER NAMES
          var name = $scope.name || "anonymous";

          //ADD TO FIREBASE
          $scope.messages.$add({
            from: name,
            body: $scope.msg
          });

          //RESET MESSAGE
          $scope.msg = "";
        }
      }
    }
  ]);