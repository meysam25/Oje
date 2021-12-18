
if (window['signalR'] && $.toast) {
    var connection = new signalR.HubConnectionBuilder().withUrl("/notification").build();
    connection.on("notify", function (subject, body) {
        $.toast({
            heading: subject,
            text: body,
            textAlign: 'left',
            position: 'bottom-left',
            showHideTransition: 'plain',
            icon: 'Information'
        })
        var prevValue = $('.notificationCount').text();
        try {
            var pNumber = Number.parseInt(prevValue);
            if (isNaN(pNumber))
                pNumber = 0;
            pNumber = pNumber + 1;
            $('.notificationCount').text(pNumber)
        } catch (e) {

        }

    });
    var wacherInterval = null;
    connection.onclose(function (e) {
        wacherInterval = setInterval(function () {
            if (connection.state == 'Disconnected') {
                connection.start();
            }
            else if (connection.state == 'Connected') {
                clearInterval(wacherInterval)
            }
        }, 3000);
    })
    connection.start().then(function () {
        console.log('connect')
    }).catch(function (err) {
        return console.log(err.toString());
    });
}
