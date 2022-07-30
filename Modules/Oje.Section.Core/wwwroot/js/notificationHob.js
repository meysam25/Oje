var hasAutoRefresh = false;
if (window['signalR'] && $.toast) {
    var connection = new signalR.HubConnectionBuilder().withUrl("/notification").build();
    connection.on("notify", function (subject, body, isModal, notifyType) {
        if (hasAutoRefresh == true) {
            refreshGridForNotification(notifyType);
            refreshTabButtonBox(notifyType);
            refreshChart(notifyType);
        }
        if (isModal) {
            notifiModal(subject, body);
        } else {
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
        }
    });
    connection.on("hasAutoRefresh", function (yesOrNo) {
        if (yesOrNo == true)
            hasAutoRefresh = true;
        
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

function refreshGridForNotification(notifyType) {
    if (notifyType) {
        var gridQuiry = $('.myGridCTRL[data-notify-' + notifyType + ']');
        var allGridQuiry = $('.myGridCTRL[data-notify-all]')
        if (gridQuiry.length > 0 && gridQuiry[0].refreshData) {
            gridQuiry[0].refreshData();
        } else if (allGridQuiry.length > 0 && allGridQuiry[0].refreshData) {
            allGridQuiry[0].refreshData();
        }
    }
}

function refreshTabButtonBox(notifyType) {
    if (notifyType) {
        var tabButtonQuiry = $('.tabButtonBox[data-notify-' + notifyType + ']');
        if (tabButtonQuiry.length > 0) {
            tabButtonQuiry.each(function () {
                if ($(this).attr('id'))
                    addCountNotificationToButtonIfHasGrid($(this).attr('id'));
            });
        }
    }
}


function refreshChart(notifyType) {
    if (notifyType) {
        var tabButtonQuiry = $('.chartHolderDiv[data-notify-' + notifyType + ']');
        if (tabButtonQuiry.length > 0) {
            tabButtonQuiry.each(function () {
                if ($(this)[0].refreshChart)
                    $(this)[0].refreshChart();
            });
        }
    }
}

function notifiModal(subject, body) {
    var newId = uuidv4RemoveDash();
    $('body').append(getModualTemplate({
        id: newId,
        title: subject,
        modelBody: body,
        actions: [
            {
                "title": "متوجه شدم",
                "onClick": "closeThisModal(this)",
                "class": "btn-secondary"
            }
        ]
    }));
    $('#' + newId).modal('show');
}