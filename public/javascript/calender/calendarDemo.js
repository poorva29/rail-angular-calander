angular.module('calendarDemoApp',['ui.calendar', 'ui.bootstrap']).controller('CalendarCtrl',function($scope,$compile,uiCalendarConfig) {
  var date = new Date();
  var d = date.getDate();
  var m = date.getMonth();
  var y = date.getFullYear();

  $scope.alertOnEventClick = function( date, jsEvent, view){
    $scope.alertMessage = (date.title + ' was clicked ');
  };
  /* alert on Drop */
  $scope.alertOnDrop = function(event, delta, revertFunc, jsEvent, ui, view){
    $scope.alertMessage = ('Event Droped to make dayDelta ' + delta);
  };
  /* alert on Resize */
  $scope.alertOnResize = function(event, delta, revertFunc, jsEvent, ui, view ){
    $scope.alertMessage = ('Event Resized to make dayDelta ' + delta);
  };

  $scope.events = [
      {title: 'Birthday Party',start: new Date(y, m, d + 1, 19, 0),end: new Date(y, m, d + 1, 22, 30),stick: true},
    ];

  $scope.uiConfig = {
      calendar:{
        defaultView: 'agendaWeek',
        height: 500,
        editable: true,
        header:{
          left: 'agendaDay agendaWeek',
          center: 'title',
          right: 'today prev,next'
        },
        views: {
          agendaWeek: {
            titleFormat: 'YYYY, MM, DD',
          }
        },
        slotDuration: '00:45:01',
        allDaySlot: false,
        selectable: true,
        eventClick: $scope.alertOnEventClick,
        eventDrop: $scope.alertOnDrop,
        eventResize: $scope.alertOnResize,
        eventRender: $scope.eventRender
      }
    };

  $scope.eventSources = [$scope.events];
});