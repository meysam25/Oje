
var isUserLogin = false;

$.fn.initLoginButton = function (url, modalId) {
    return this.each(function () {
        loadLoginConfigIfNotExist(url, modalId);
        $(this).click(function (e) {
            e.preventDefault();
            e.stopPropagation();
            if ($('#' + modalId).length > 0) {
                $('#' + modalId).modal('show');
            }
            else {
                if ($('#swPanelId').length > 0) {
                    $('#swPanelId').remove();
                    if ($('.onlineChat').length > 0) {
                        $('.onlineChat')[0].close();
                    }
                }
                loadLoginConfigIfNotExist(url, modalId, true);
            }
            return false;
        });
    });
}

function loadLoginConfigIfNotExist(url, modalId, showModal) {
    var newId = uuidv4RemoveDash();
    $('body').append('<div id="' + newId + '" ></div>');
    loadJsonConfig(url, newId, function () {
        if (this.showModal) {
            $('#' + this.modalId).modal('show');
        }
    }.bind({ modalId: modalId, showModal: showModal }), 'GET');
    bindIfUserAreadyLogin(modalId);
}

function bindIfUserAreadyLogin(modalId) {
    postForm('/Account/Dashboard/GetLoginUserInfo', new FormData(), function (res) {
        if (res && res.isSuccess == true) {
            isUserLogin = true;
            var userFullname = res.firstname ? (res.firstname + ' ' + res.lastname) : res.username;
            localStorage.setItem('curLogin', JSON.stringify(res));
            userLoginWeb(userFullname, res.isUser);
            disableFloatingFooter();
        } else {
            isUserLogin = false;
            whatToDoAfterUserLogin.push({ curFun: function () { disableFloatingFooter() }, type: 'df' });
        }
        $('.newTicketMainPageButton').click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            if (isUserLogin == false) {
                $('#' + modalId).modal('show');
                whatToDoAfterUserLogin.push({ curFun: function () { loadAndShowAddNewTicket() }, , type: 'nt' });
            } else {
                loadAndShowAddNewTicket();
            }
            return false;
        });
    }, null, null, true);
}

function loadAndShowAddNewTicket() {
    if ($('#TicketUserAddGridModal').length == 0) {
        postForm('/Ticket/TicketUser/GetJsonConfig', new FormData(), function (res) {
            if ($('#TicketUserAddGridModal').length == 0) {
                if (res && res.panels && res.panels[0].moduals && res.panels[0].moduals.length > 0) {
                    $('body').append(getModualTemplate(res.panels[0].moduals[0]));
                    executeArrFunctions();
                }
            }
            $('#TicketUserAddGridModal').modal('show');
        });
    } else {
        clearForm($('#TicketUserAddGridModal'));
        $('#TicketUserAddGridModal').modal('show');
    }
}

function userLoginWeb(userfullname, isUser) {
    isUserLogin = true;
    $('.holderRigAndLogUser').find('a').css('display', 'none');
    $('.holderRigAndLogUser').append(getLoginUserMenuTemplate(userfullname, isUser));
    if ($('#swPanelId').parent().parent().hasClass('onlineChatMessages')) {
        $('#swPanelId').remove();
    }
}

function getLoginUserMenuTemplate(userfullanme, isUser) {
    return `
    <div class="logedInUserMenu">
        <span class="logedInUserMenuFullname" ><img src="" />${userfullanme}<i style="font-size:2em;vertical-align:-9px;margin-right:10px;" class="fa fa-angle-down" ></i><i style="font-size:2em;vertical-align:-9px;margin-right:10px;" class="fa fa-user-circle" ></i></span>
        <div class="logedInUserMenuItems">
            <a href="${(isUser ? '/Dashboard' : '/Account/Dashboard/Index')}" >داشبورد</a>
            <a href="/UserAccount/UserProfile/Index" >به روز رسانی پروفایل</a>
            <a href="/Account/Dashboard/Logout" >خروج</a>
        </div>
    </div>
`;
}