var exteraModelParams = null;
var isUserLogin = false;
var isUserNeedToGoToDashboard = false;
$.fn.initLeftColDocument = function (url)
{
    return this.each(function ()
    {
        $(this).html('');
        postForm(url, new FormData(), function (res)
        {
            if (res && res.length > 0) {
                for (var i = 0; i < res.length; i++) {
                    $(this).append('<a class="documentLink" href="' + res[i].src +'" title="' + res[i].title +'">'+ res[i].title +'</a>');
                }
            }
        }.bind(this), null, function () { });
    });
};


$.fn.loadLoginPanel = function (url, whatToDoNext) {
    return this.each(function () {
        $(this).html('<div id="holderLoginPanel" class="customePanel" ></div>');
        showLoader($('#holderLoginPanel'));
        loadJsonConfig(url, 'holderLoginPanel', function () { if (whatToDoNext) whatToDoNext();  });
    });
};

registerConfig = null;
function loadAllRegistersConfig() {
    postForm('/Register/GetAllConfig', new FormData(), function (res)
    {
        registerConfig = res;
    });
}

function getMainButtonTemplate(buttonObj) {
    var result = '';

    if (buttonObj) {
        result = `
            <div class="iconButtonNew">
                <div class="buttonArrow"></div>
                <div data-id="${buttonObj.key}" class="iconButtonNewBody">
                    <div class="iconButtonNewBodyIconSection">
                        <span class="newButtonIcon"><i class="fa ${buttonObj.extera}" ></i></span>
                    </div>
                    <div class="iconButtonNewBodyTitleSection">
                        ${buttonObj.value}
                    </div>
                </div>
                <div class="buttonArrow"></div>
            </div>
`;
    }

    return result;
}

function getRightNavigationButtonTemplate(buttonObj, curId) {
    var result = '';

    if (buttonObj) {
        result = `
            <div data-id="${buttonObj.key}" class="navigationButton ${buttonObj.key == curId ? 'navigationButtonActive' : ''}">
                <div>${buttonObj.value}</div>
            </div>
`;
    }

    return result;
}

function showRightNavRegButtons(id) {
    $('.navigationButtonRoute').html('');
    if (registerConfig)
        for (var i = registerConfig.length-1; i >= 0; i--) {
            $('.navigationButtonRoute').append(getRightNavigationButtonTemplate(registerConfig[i], id));
        }

    $('.navigationButtonRoute').find('.navigationButton').click(function ()
    {
        showRegisterForm($(this).attr('data-id'));
    });
}

function showRegisterForm(id) {
    exteraModelParams = { fid: id };
    $('.mainContentBody').html('<div id="holderppf" class="customePPFPanel" ></div>');
    loadJsonConfig('/Register/GetJsonConfig?fid=' + id, $('#holderppf'), function () { hideShowNavTitle(false); showRightNavRegButtons(id); addMoveToMainStepButton($('.customePPFPanel')); });
}

function showRegisterButtons() {
    $('.mainContentBody').html('');
    if (registerConfig)
        for (var i = 0; i < registerConfig.length; i++) {
            $('.mainContentBody').append(getMainButtonTemplate(registerConfig[i]));
        }

    addMoveToMainStepButton();
   
    $('.iconButtonNewBody').click(function ()
    {
        showRegisterForm($(this).attr('data-id'));
    });
}

function addMoveToMainStepButton(targetQuiry) {
    var quiry = $('.mainContentBody');
    if (targetQuiry)
        quiry = targetQuiry;
    quiry.append('<div class="backButtonToMainPage">بازگشت به صفحه اصلی</div>');
    $('.backButtonToMainPage').click(function () {
        $('.mainContentBody').loadLoginPanel('/TenderWeb/GetLoginConfig', addRegisterButton);
        hideShowNavTitle(true);
        setNavigationTitle('ورود به سامانه');
        initWhatToDoLogin();
    });
}

function addRegisterButton() {
    $('#holderLoginPanel').append('<div style="padding-right:10px;padding-left:10px;"><span class="makeSmallerFont" >حساب کاربری ندارید؟</span><span class="mainRegisterButton makeSmallerFont" >ثبت نام کنید</span></div>');
    $('.mainRegisterButton').click(function ()
    {
        if (isUserLogin) {
            showRegisterButtons();
            hideShowNavTitle(true);
            setNavigationTitle('ثبت نام در سامانه');
        } else {
            $('.mainContentBody').loadLoginPanel('/TenderWeb/GetLoginForRegConfig');
            setNavigationTitle('ثبت نام در سامانه');
            registerWhatToDoNextForReg();
            addMoveToMainStepButton();
        }
        
    });
}

function setNavigationTitle(title) {
    $('.currentSectionTitle').text(title);
}

function hideShowNavTitle(isShow) {
    if (isShow) {
        $('.currentSection').css('display', 'block');
        $('.rightNavigation').removeClass('rightNavigationMakeSmall');
    } else {
        $('.currentSection').css('display', 'none');
        $('.rightNavigation').addClass('rightNavigationMakeSmall');
    }
}

function detectIfUserIsLogin() {
    postForm('/Account/Dashboard/GetLoginUserInfo', new FormData(), function (res)
    {
        if (res && res.isSuccess) {
            isUserLogin = true;
            if (!res.isUser)
                isUserNeedToGoToDashboard = true;
        }
    });
}



function initWhatToDoLogin() {
    $('.navigationButtonRoute').html('');
    whatToDoAfterUserLogin = [];
    whatToDoAfterUserLogin.push({
        curFun: function () {
            showLoader($('.mainBox'));
            location.href = '/Account/Dashboard/Index';
        }
    });
}

function registerWhatToDoNextForReg() {
    whatToDoAfterUserLogin = [];
    whatToDoAfterUserLogin.push({
        curFun: function () {
            showRegisterButtons();
            hideShowNavTitle(true);
            setNavigationTitle('ثبت نام در سامانه');
        }
    });
}