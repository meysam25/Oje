
$.fn.initLoginButton = function (url, modalId) {
    return this.each(function () {
        var newId = uuidv4RemoveDash();
        $('body').append('<div id="' + newId + '" ></div>');
        loadJsonConfig(url, newId);
        $(this).click(function () {
            if ($('#' + modalId).length > 0) {
                $('#' + modalId).modal('show');
            }
        });
        bindIfUserAreadyLogin();
    });
}

function bindIfUserAreadyLogin() {

    postForm('/Account/Dashboard/GetLoginUserInfo', new FormData(), function (res) {
        if (res) {
            var userFullname = res.firstname ? (res.firstname + ' ' + res.lastname) : res.username;
            localStorage.setItem('curLogin', JSON.stringify(res));
            userLoginWeb(userFullname);
        }
    });
}

function userLoginWeb(userfullname) {
    $('.holderRigAndLogUser').find('a').css('display', 'none');
    $('.holderRigAndLogUser').append(getLoginUserMenuTemplate(userfullname));
}

function getLoginUserMenuTemplate(userfullanme) {
    return `
    <div class="logedInUserMenu">
        <span class="logedInUserMenuFullname" ><img src="" />${userfullanme}<i style="font-size:2em;vertical-align:-9px;margin-right:10px;" class="fa fa-angle-down" ></i><i style="font-size:2em;vertical-align:-9px;margin-right:10px;" class="fa fa-user-circle" ></i></span>
        <div class="logedInUserMenuItems">
            <a href="#" >داشبورد</a>
            <a href="/Account/Dashboard/Logout" >خروج</a>
        </div>
    </div>
`;
}