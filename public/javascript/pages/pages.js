var app = angular.module('main', ['daypilot']).controller('DemoCtrl', function($scope, $http) {

    var dp = new DayPilot.Calendar("dp");

    dp.viewType = "Week";
    dp.allDayEventHeight = 25;
    dp.initScrollPos = 9 * 40;
    dp.moveBy = 'Full';
    dp.timeHeaderCellDuration = 30;
    dp.cellHeight = 40;
    dp.theme = "custom_theme";

    $http.get('jsons/appointments.json').success(function (data){
        var appointments = data;

        dp.startDate = new DayPilot.Date(appointments.start);  // or just dp.startDate = "2013-03-25";

        $scope.startOfWeek = dp.startDate.firstDayOfWeek().toString("MMMM dd");
        $scope.endOfWeek = dp.startDate.firstDayOfWeek().addDays(6).toString("MMMM dd");

        angular.forEach(appointments.events, function(app){
            app.start = new DayPilot.Date(app.start);
            app.end = new DayPilot.Date(app.end);
            dp.events.add(new DayPilot.Event(app));
        });
        $scope.startDate = dp.startDate.addDays(-7);
    });

    $scope.prev = function(){
        dp.startDate = dp.startDate.addDays(-7);
        $scope.startOfWeek = dp.startDate.firstDayOfWeek().toString("MMMM dd");
        $scope.endOfWeek = dp.startDate.firstDayOfWeek().addDays(6).toString("MMMM dd");
        dp.update();
    }

    $scope.next = function(){
        dp.startDate = dp.startDate.addDays(7);
        $scope.startOfWeek = dp.startDate.firstDayOfWeek().toString("MMMM dd");
        $scope.endOfWeek = dp.startDate.firstDayOfWeek().addDays(6).toString("MMMM dd");
        dp.update();
    }

    // bubble, with async loading
    dp.bubble = new DayPilot.Bubble({
        cssClassPrefix: "bubble_default",
        onLoad: function(args) {
            var ev = args.source;
            args.async = true;  // notify manually using .loaded()

            // simulating slow server-side load
            setTimeout(function() {
                args.html = "testing bubble for: <br>" + ev.text();
                args.loaded();
            }, 500);
        }
    });

    dp.eventDeleteHandling = "Update";

    dp.onEventDeleted = function(args) {
        dp.message("Event deleted: " + args.e.text());
    };

    // event moving
    dp.onEventMoved = function (args) {
        dp.message("Moved: " + args.e.text());
    };

    // event resizing
    dp.onEventResized = function (args) {
        dp.message("Resized: " + args.e.text());
    };

    // event creating
    dp.onTimeRangeSelected = function (args) {
        var name = prompt("New event name:", "Event");
        if (!name) return;
        var e = new DayPilot.Event({
            start: args.start,
            end: args.end,
            id: DayPilot.guid(),
            resource: args.resource,
            text: "Event"
        });
        dp.events.add(e);
        dp.clearSelection();
        dp.message("Created");
    };

    dp.onTimeRangeDoubleClicked = function(args) {
        alert("DoubleClick: start: " + args.start + " end: " + args.end + " resource: " + args.resource);
    };

    dp.init();
});
