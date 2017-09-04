var app = angular.module('myApp', ['ui.calendar', 'ui.bootstrap']); // added u.bootstrap for modal dialog
app.controller('myNgController', ['$scope', '$http', 'uiCalendarConfig', '$uibModal', function ($scope, $http, uiCalendarConfig, $uibModal) {

    $scope.SelectedEvent = null;
    var isFirstTime = true;

    $scope.events = [];
    $scope.eventSources = [$scope.events];

    $scope.NewEvent = {};
    //this function for get datetime from json date
    function getDate(datetime) {
        if (datetime != null) {
            var mili = datetime.replace(/\/Date\((-?\d+)\)\//, '$1');
            return new Date(parseInt(mili));
        }
        else {
            return "";
        }
    }
    // this function for clear clender enents
    function clearCalendar() {
        if (uiCalendarConfig.calendars.myCalendar != null) {
            uiCalendarConfig.calendars.myCalendar.fullCalendar('removeEvents');
            uiCalendarConfig.calendars.myCalendar.fullCalendar('unselect');
        }
    }


    // will put this to a method 
    //Load events from server
    $http.get('/Event/getevents', {
        cache: true,
        params: {}
    }).then(function (data) {
        $scope.events.slice(0, $scope.events.length);
        angular.forEach(data.data, function (value) {
            $scope.events.push({
                title: value.Title,
                description: value.Description,
                start: new Date(parseInt(value.StartAt.substr(6))),
                end: new Date(parseInt(value.EndAt.substr(6))),
                allDay: value.IsFullDay,
                stick: true
            });
        });
    });
    function populate() {
        clearCalendar();
        $http.get('/Event/getevents', {
            cache: false,
            params: {}
        }).then(function (data) {
            $scope.events.slice(0, $scope.events.length);
            angular.forEach(data.data, function (value) {
                $scope.events.push({
                    id: value.Event_ID,
                    title: value.Title,
                    description: value.Description,
                    start: new Date(parseInt(value.StartAt.substr(6))),
                    end: new Date(parseInt(value.EndAt.substr(6))),
                    allDay: value.IsFullDay,
                    stick: true
                });
            });
        });
    }
    populate();
    //configure calendar
    $scope.uiConfig = {
        calendar: {
            height: 450,
            editable: true,
            displayEventTime: true,
            header: {
                left: 'month,agendaWeek,agendaDay',
                center: 'title',
                right: 'today prev,next'
            },
            timeFormat: {
                month: ' ', // for hide on month view
                agenda: 'h:mm t'
            },
            selectable: true,
            selectHelper: true,
            select: function (start, end) {
                var fromDate = moment(start).format('YYYY/MM/DD LT');
                var endDate = moment(end).format('YYYY/MM/DD LT');
                $scope.NewEvent = {
                    Event_ID: 0,
                    StartAt: fromDate,
                    EndAt: endDate,
                    IsFullDay: false,
                    Title: '',
                    Description: ''
                }
                $scope.ShowModal();
            },
            eventClick: function (event) {
                $scope.SelectedEvent = event;
                var fromDate = moment(event.start).format('YYYY/MM/DD LT');
                var endDate = moment(event.end).format('YYYY/MM/DD LT');
                $scope.NewEvent = {
                    Event_ID: event.id,
                    StartAt: fromDate,
                    EndAt: endDate,
                    IsFullDay: false,
                    Title: event.title,
                    Description: event.description
                }
                $scope.ShowModal();
            },
            eventAfterAllRender: function () {
                if ($scope.events.length > 0 && isFirstTime) {
                    //Focus first event
                    uiCalendarConfig.calendars.myCalendar.fullCalendar('gotoDate', $scope.events[0].start);
                    isFirstTime = false;
                }
            }
        }
    };

    //This function for show modal dialog
    $scope.ShowModal = function () {
        $scope.option = {
            templateUrl: 'modalContent.html',
            controller: 'modalController',
            backdrop: 'static',
            resolve: {
                NewEvent: function () {
                    return $scope.NewEvent;
                }
            }
        };
        var modal = $uibModal.open($scope.option);
        modal.result.then(function (data) {
            $scope.NewEvent = data.event;
            switch (data.operation) {
                case 'Save':
                    //Save here
                    $http({
                        method: 'POST',
                        url: '/home/SaveEvent',
                        data: $scope.NewEvent
                    }).then(function (response) {
                        if (response.data.status) {
                            populate();
                        }
                    })
                    break;
                case 'Delete':
                    //Delete here $http({
                    $http({
                        method: 'POST',
                        url: '/home/DeleteEvent',
                        data: { 'eventID': $scope.NewEvent.Event_ID }
                    }).then(function (response) {
                        if (response.data.status) {
                            populate();
                        }
                    })
                    break;
                default:
                    break;
            }
        }, function () {
            console.log('Modal dialog closed');
        })
    }

}])

// create a new controller for modal 
app.controller('modalController', ['$scope', '$uibModalInstance', 'NewEvent', function ($scope, $uibModalInstance, NewEvent) {
    $scope.NewEvent = NewEvent;
    $scope.Message = "";
    $scope.ok = function () {
        if ($scope.NewEvent.Title.trim() != "") {
            $uibModalInstance.close({ event: $scope.NewEvent, operation: 'Save' });
        }
        else {
            $scope.Message = "Event title required!";
        }
    }
    $scope.delete = function () {
        $uibModalInstance.close({ event: $scope.NewEvent, operation: 'Delete' });
    }
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
}])