$.connection.hub.start()
        .done(function () {
            console.log("Connection Successful")
            $.connection.instantMessageHub.server.sendmsg();
        })
        .fail(function () { alert("Connection failed") });

$.connection.instantMessageHub.client.sendmsg = function (message) {
    alert(message)
}