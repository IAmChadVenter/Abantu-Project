var tasks = null;
$.ajax({
    method: 'GET',
    url: '/tasks/userNotifications',
    contentType: "application/json",
    dataType: 'json',
    success: function (res) {
        if (res.message === undefined) {
            tasks = res;
            }
        else {
            console.log('User is not signed in');
        }

    },
    async: false,
    error: function (err) {
        console.log(err)
    }
});
// call the method 
notifyUser();
function notifyUser() {
    console.log(tasks);
    if (tasks !== null) {
        for (var i = 0; i < tasks.length; i++) {
            $.notify({
                // options
                icon: 'glyphicon glyphicon-bell',
                title: 'Task Notification<br/><br/>',
                message: 'Dear ' + tasks[i].userName + "<br/><br/>"+ tasks[i].message,
                url: '/tasks/details/' + tasks[i].ID,
                target: '_self'
            }, {
                // settings
                element: 'body',
                position: null,
                type: "info",
                allow_dismiss: true,
                newest_on_top: false,
                showProgressbar: false,
                placement: {
                    from: "top",
                    align: "right"
                },
                offset: 20,
                spacing: 10,
                z_index: 1031,
                delay: 5000,
                timer: 1000,
                url_target: '_self',
                mouse_over: null,
                animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                },
                onShow: null,
                onShown: null,
                onClose: null,
                onClosed: null,
                icon_type: 'class',
                template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                    '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                    '<span data-notify="icon"></span> ' +
                    '<span data-notify="title">{1}</span> ' +
                    '<span data-notify="message">{2}</span>' +
                    '<div class="progress" data-notify="progressbar">' +
                        '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                    '</div>' +
                    '<a href="{3}" target="{4}" data-notify="url"></a>' +
                '</div>'
            });
        }
    }
}