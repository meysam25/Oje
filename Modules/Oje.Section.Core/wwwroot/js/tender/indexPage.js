var exteraModelParams = null;
var isUserLogin = false;
var isUserNeedToGoToDashboard = false;

$.fn.initLeftColDocument = function (url, id) {
    return this.each(function () {
        $(this).html('');
        var postData = new FormData();
        postData.append('formId', id);
        postForm(url, postData, function (res) {
            if (res && res.length > 0) {
                for (var i = 0; i < res.length; i++) {
                    $(this).append('<a class="documentLink" href="' + res[i].src + '" title="' + res[i].title + '">' + res[i].title + '</a>');
                }
            }
        }.bind(this), null, function () { }.bind(this));
    });
};

$.fn.loadLoginPanel = function (url, whatToDoNext) {
    return this.each(function () {
        $(this).html('<div id="holderLoginPanel" class="customePanel" ></div>');
        showLoader($('#holderLoginPanel'));
        loadJsonConfig(url, 'holderLoginPanel', function () { if (whatToDoNext) whatToDoNext(); });
    });
};

var registerConfig = null;
function loadAllRegistersConfig() {
    postForm('/Register/GetAllConfig', new FormData(), function (res) {
        registerConfig = res;
    });
}

function getMainButtonTemplate(buttonObj) {
    var result = '';

    if (buttonObj) {
        result = `
            <div class="iconButtonNew">
                <div class="buttonArrow"></div>
                <div data-id="${buttonObj.childs && buttonObj.childs.length > 0 ? buttonObj.childs[0].key : ''}" class="iconButtonNewBody">
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
        for (var i = registerConfig.length - 1; i >= 0; i--) {
            var curItem = registerConfig[i];
            if (curItem.childs) {
                var hasThisId = curItem.childs.filter(function (item) { return item.key == id });
                if (hasThisId && hasThisId.length > 0)
                    for (var j = 0; j < curItem.childs.length; j++) {
                        $('.navigationButtonRoute').append(getRightNavigationButtonTemplate(curItem.childs[j], id));
                    }
            }
        }

    $('.navigationButtonRoute').find('.navigationButton').click(function () {
        showRegisterForm($(this).attr('data-id'));
    });
}

function initContactUsButton() {
    $('.contactUsButton').click(function () {
        $('.mySlider').parent().hide();
        showLoader($('.mainContent'));
        postForm('/ContactUs', new FormData(), function (res) {
            var selectQuiry = $('.mainContentBodyScroll');
            if (selectQuiry.length > 0) {
                selectQuiry.html('<div class="makeCenterContentForCC aboutUsSectionFixMapProblem"></div>');
                loadJsonConfig('/ContactUs/GetJsonConfig', $('.makeCenterContentForCC'), function () {
                    if (res.subTitle) {
                        selectQuiry.find('.makeCenterContentForCC').prepend('<h4 class="aboutUsSubTitle" >' + res.subTitle + '</h3>');
                    }
                    if (res.title) {
                        selectQuiry.find('.makeCenterContentForCC').prepend('<h3 class="aboutUsTitle" >' + res.title + '</h3>');
                    }
                });
            }
        }, null, function () { hideLoader($('.mainContent')); }, null, 'GET', { Platform: 'mobile' });
    });
}

function initAboutUsButton() {
    $('.aboutUsButton').click(function () {
        var url = $(this).attr('data-url');
        if (url) {
            $('.mySlider').parent().hide();
            showLoader($('.mainContent'));
            postForm(url + '?ignoreL=true', new FormData(), function (res) {
                var selectQuiry = $('.mainContentBodyScroll');
                if (selectQuiry.length > 0) {
                    selectQuiry.html(res);
                    selectQuiry.find('[data-src]').each(function () { $(this).attr('src', $(this).attr('data-src')); });
                }
            }, null, function () {
                hideLoader($('.mainContent'));
            }, null, 'GET')
        }
    });
}

function initLicenceButton() {
    $('.licenceButton').click(function () {
        loadLicenceF($('.mainContentBodyScroll'));
    });
}

function loadLicenceF(sQuiry) {
    var selectQuiry = sQuiry;
    if (selectQuiry.length > 0) {
        selectQuiry.html('<div class="holderLicences" ></div>');
        selectQuiry = selectQuiry.find('.holderLicences');
        showLoader($('.mainContent'));
        postForm('/Home/GetFooterSambole', new FormData(), function (res) {
            selectQuiry.append(res && res.enamad ? ('<div class="holderLicenceDiv" >' + res.enamad + '</div>') : '');
            selectQuiry.append(res && res.samandehi ? ('<div class="holderLicenceDiv" >' + res.samandehi + '</div>') : '');
            selectQuiry.find('img[data-src]').loadImageOnScroll();
            postForm('/Home/GetTopLeftIconList', new FormData(), function (res) {
                hideLoader($('.mainContent'));
                if (res) {
                    var html = '';
                    if (res.mainFile1_address && res.title1) {
                        html += addTopLeftIcons(res.title1, res.mainFile1_address, res.description1, true);
                    }
                    if (res.mainFile2_address && res.title2) {
                        html += addTopLeftIcons(res.title2, res.mainFile2_address, res.description2);
                    }
                    if (res.mainFile3_address && res.title3) {
                        html += addTopLeftIcons(res.title3, res.mainFile3_address, res.description3);
                    }
                    selectQuiry.prepend(html);
                    selectQuiry.find('.mainHeaderIranLogo,.mainHeaderElectronDevelopLogo').each(function () {
                        var title = $(this).attr('title');
                        var desc = $(this).attr('data-des');
                        if (desc) {
                            var modalObj = {
                                id: uuidv4RemoveDash(),
                                title: title,
                                modelBody: desc,
                                class: "makeImage100",
                                actions: [
                                    {
                                        "title": "متوجه شدم",
                                        "onClick": "closeThisModal(this)",
                                        "class": "btn-secondary"
                                    }
                                ]
                            };
                            $(this).attr('data-modal-id', modalObj.id);
                            $('.mainContent').append(getModualTemplate(modalObj))
                        }
                    });
                    selectQuiry.find('.mainHeaderIranLogo[data-des],.mainHeaderElectronDevelopLogo[data-des]').click(function () {
                        var modalId = $(this).attr('data-modal-id');
                        $('#' + modalId).modal('show');
                    });

                }
            }, null, null, null, 'GET');
            selectQuiry.find('a[href]').click(function (e) {
                e.stopPropagation();
                e.preventDefault();

                openModal($(this).attr('href'), $(this).text());

                return false;
            });
            selectQuiry.find('img[onclick]').each(function () {
                $(this).attr('data-onclick', $(this).attr('onclick'));
                $(this).removeAttr('onclick');
            });
            selectQuiry.find('img[data-onclick]').click(function (e) {
                e.stopPropagation();
                e.preventDefault();
                var onClick = $(this).attr('data-onclick');
                if (onClick && onClick.indexOf('window.open(') > -1) {
                    var url = onClick.split('window.open(')[1];
                    if (url.indexOf('"') > -1)
                        url = url.split('"')[1];
                    else if (url.indexOf("'") > -1)
                        url = url.split("'")[1];
                    openModal(url, $(this).attr('alt'));
                }
                return false;
            });
        }, null, null, null, 'GET');

    }
}

function addTopLeftIcons(title, imgSrc, des, isVisibleAllways) {
    return '<div class="holderLicenceDiv"><img ' + (des ? 'style="cursor:pointer"' : '') + '  class="' + (isVisibleAllways ? 'mainHeaderIranLogo' : 'mainHeaderElectronDevelopLogo') + '" title="' + title + '" src="' + imgSrc + '"  ' + (des ? 'data-des=\'' + des + '\'' : '') + ' /></div>';
}

function initStartButton() {
    $('.startButton').click(function () {
        $('.mainContentBodyScroll').html('<div class="fgSection"></div>');
        $('.fgSection').loadLoginPanel('/TenderWeb/GetLoginConfig', addRegisterButton);
        $('.mySlider').parent().hide();
    });
}

function getReminderTemplate() {
    return `
<div id="reminderSection">
    <section class="reminderSection">
        <div class="reminderSectionDescription">
            <img class="reminderMainLogo" width="64" height="64" />
            <div class="reminderSectionDescriptionTexts">
                <div class="reminderSectionDescriptionTextsTitle"></div>
                <div class="reminderSectionDescriptionTextsDescription"></div>
            </div>
        </div>
        <div class="reminderSectionInput "></div>
    </section>
</div>
`;
}

function initReminderButton() {
    $('.reminderButton').click(function () {
        showLoader($('.mainContentBody'))
        $('.mainContentBodyScroll').html('<div class="makeCenterContentForCC">' + getReminderTemplate() + '</div>');
        loadJsonConfig('/Home/GetReminderConfig', $('#reminderSection .reminderSectionInput'), function () {
            $('#reminderSection').loadAndBindRemindUsSection('/Reminder/GetMainPageDescription', function () {
                hideLoader($('.mainContentBody'))
            });
        }, 'GET');
    });
}

function hideAllButtonAndJustShowHomeButton() {
    $('.showLoginModal').css('display', 'none');
    $('.moveToHome').css('display', 'block');
}



$.fn.initAutoNumber = function (calceTime) {
    if (!calceTime)
        return this;
    return this.each(function () {
        var foundQuery = $(this).find('[data-target-value]');
        if (foundQuery.length > 0) {
            var foundObj = $(this).find('[data-target-value]')[0];
            foundObj.startAutoNumber = function ()
            {
                var curValue = 0;
                var maxValue = Number.parseInt($(this).attr('data-target-value'));
                var timerInterval = 1;
                var valueIncremant = 1;

                if (maxValue < calceTime) {
                    timerInterval = Math.floor(calceTime / maxValue);
                    valueIncremant = 1;
                }
                else {
                    timerInterval = 11;
                    valueIncremant = Math.floor(maxValue / calceTime) * 11;
                }

                if (maxValue > 19000)
                    console.log(valueIncremant);

                this.tInterval = setInterval(function () {
                    if (valueIncremant <= 0)
                        valueIncremant = 1;
                    curValue = curValue + valueIncremant;
                    if (curValue >= maxValue) {
                        curValue = maxValue;
                        clearInterval(this.curThis.tInterval);
                    }
                    $(this.curThis).html(curValue);
                }.bind({ curThis: this }), timerInterval);
            };
            var startFunction = function () {
                if (!this.curThis.isAutoStarted) {
                    this.curThis.startAutoNumber();
                    this.curThis.isAutoStarted = true;
                }
            }.bind({ curThis: foundObj });
            onVisibilityChange(foundObj, startFunction);
            startFunction();
            //handler();
            //$(window).on('DOMContentLoaded load resize scroll', handler);
        }
    });
};

function loadOurPrice() {
    var sQuiry = $('.fgSection');
    if (sQuiry.length > 0) {
        sQuiry.html(`
<div id="ourPrideDiv" style="display:none;width:100%;" >
    <section class="ourPrideSection ">
        <div class="ourPrideSectionTitle"><span></span></div>
        <div class="ourPrideDescription"></div>
        <div class="ourPrideSectionItems"></div>
    </section>
</div>
`);
        $('#ourPrideDiv').loadAndBindOurPride('/TenderWeb/GetOurPrideMainPage');
    }
}

$.fn.loadAndBindOurPride = function (url) {

    function getOurPrideItem(imgSrc, title) {
        var result = '';

        if (title && title.trim()) {
            result += '<div class="ourPrideSectionItem">';
            if (imgSrc)
                result += '<img width="80" alt="' + title.replace('{', '').replace('}', '') + '" height="80" data-src="' + imgSrc + '" />';
            result += ' <div >';
            result += getOurPrideItemTemplate(title);
            result += '</div>';
            result += '</div>';
        }

        return result;
    }

    function getOurPrideItemTemplate(title) {
        var result = '';

        if (title && title.trim()) {
            if (title.indexOf('{') == -1 || title.indexOf('}') == -1) {
                result = '<span class="ourPrideSectionItemLightText">' + title + '</span>';
            } else {
                var leftPart = title.split('{')[0];
                var rightPart = title.split('}')[1];
                var centerNumberPart = title.split('{')[1].split('}')[0];
                if (isNaN(centerNumberPart))
                    centerNumberPart = 1000;
                if (leftPart)
                    result += '<span class="ourPrideSectionItemLightText">' + leftPart + '</span>';
                if (centerNumberPart) {
                    result += '<span class="ourPrideSectionItemLightBoldTxt" data-target-value="' + centerNumberPart + '">0</span>';
                }
                if (rightPart)
                    result += '<span class="ourPrideSectionItemLightText">' + rightPart + '</span>';
            }
        }

        return result;
    }

    return this.each(function () {

        postForm(url, new FormData(), function (res) {
            var template = '';
            if (res) {
                template += getOurPrideItem(res.image1_address, res.title1);
                template += getOurPrideItem(res.image2_address, res.title2);
                template += getOurPrideItem(res.image3_address, res.title3);
                template += getOurPrideItem(res.image4_address, res.title4);
                $(this.curThis).find('.ourPrideSectionTitle span').html(res.title);
                $(this.curThis).find('.ourPrideDescription').html(res.desc);
                if (res.readMoreLink) {
                    $('.readMoreAboutUs').attr('href', res.readMoreLink);
                    $('.readMoreAboutUs').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        var url = $(this).attr('href');
                        if (url) {
                            showLoader($('.mainContent'));
                            postForm(url + '?ignoreL=true', new FormData(), function (res) {
                                var selectQuiry = $('.mainContentBodyScroll');
                                if (selectQuiry.length > 0) {
                                    selectQuiry.html(res);
                                    selectQuiry.find('[data-src]').each(function () { $(this).attr('src', $(this).attr('data-src')); });
                                }
                            }, null, function () {
                                hideLoader($('.mainContent'));
                            }, null, 'GET')
                        }
                    });
                } else {
                    $('.readMoreAboutUs').css('display', 'none');
                }
                $(this.curThis).css('display', 'block');
            } else {
                $(this.curThis).css('display', 'none');
            }
            $(this.curThis).find('.ourPrideSectionItems').html(template);
            $(this.curThis).find('img[data-src]').loadImageOnScroll();
            $(this.curThis).find('.ourPrideSectionItem').initAutoNumber(2000);
        }.bind({ curThis: this }), null, null, null, 'GET');
    });
}


$.fn.loadAndBindRemindUsSection = function (url, whatToDoOfterFinished) {
    return this.each(function () {
        postForm(url, new FormData(), function (res) {
            var selectQuery = $(this.curThis);
            if (res) {
                selectQuery.find('.reminderMainLogo').attr('data-src', res.mainImage_address).attr('alt', res.title);
                selectQuery.find('.reminderSectionDescriptionTextsTitle').html(res.title);
                selectQuery.find('.reminderSectionDescriptionTextsDescription').html(res.desc);
                selectQuery.css('display', 'block');
                selectQuery.find('img[data-src]').loadImageOnScroll();
            } else {
                selectQuery.css('display', 'none');
            }
            if (whatToDoOfterFinished)
                whatToDoOfterFinished();
        }.bind({ curThis: this }), null, null, null, 'GET');
    });
}

function showRegisterForm(id) {
    exteraModelParams = { fid: id };
    $('.fgSection').html('<div id="holderppf" class="customePPFPanel" ></div>');
    loadJsonConfig('/Register/GetJsonConfig?fid=' + id, $('#holderppf'), function () {
        hideShowNavTitle(false);
        showRightNavRegButtons(id);
        //addMoveToMainStepButton($('.customePPFPanel'));
    });
    $('.holderDocumentLinks').initLeftColDocument('/TenderWeb/GetTenderFiles', id);
    hideAllButtonAndJustShowHomeButton();
}

function showRegisterButtons() {
    $('.fgSection').html('');
    if (registerConfig)
        for (var i = 0; i < registerConfig.length; i++) {
            $('.fgSection').append(getMainButtonTemplate(registerConfig[i]));
        }

    addMoveToMainStepButton();

    $('.iconButtonNewBody').click(function () {
        if (!isUserLogin) {
            $('.fgSection').loadLoginPanel('/TenderWeb/GetLoginForRegConfig');
            setNavigationTitle('ثبت نام در سامانه');
            registerWhatToDoNextForReg($(this).attr('data-id'));
            addMoveToMainStepButton();
        } else {
            showRegisterForm($(this).attr('data-id'));
        }
    });
}


$.fn.initMainPageAboutUs = function (url) {
    return this.each(function () {
        postForm(url, new FormData(), function (res) {
            var selectQuery = $(this.curThis);
            if (res) {
                var imagesQuerySelect = selectQuery.find('.aboutUsSectionAbilityIcon img');
                if (imagesQuerySelect.length == 3) {
                    imagesQuerySelect.eq(0).attr('data-src', res.rightFile_address).attr('alt', res.rightFileTitle);
                    imagesQuerySelect.eq(1).attr('data-src', res.centerFile_address).attr('alt', res.centerFileTitle);
                    imagesQuerySelect.eq(2).attr('data-src', res.leftFile_address).attr('alt', res.leftFileTitle);
                }
                var imagesSubTitleQuerySelect = selectQuery.find('.aboutUsSectionAbilityIcon div');
                if (imagesSubTitleQuerySelect.length == 3) {
                    imagesSubTitleQuerySelect.eq(0).html(res.rightFileTitle);
                    imagesSubTitleQuerySelect.eq(1).html(res.centerFileTitle);
                    imagesSubTitleQuerySelect.eq(2).html(res.leftFileTitle);
                }
                selectQuery.find('.aboutUsSectionTitle .ourPrideSectionTitle > span').html(res.title);
                selectQuery.find('.aboutUsSectionSubTitle').html(res.subTitle);
                selectQuery.find('.aboutUsSectionDescription').html(res.desc);
                if (res.readMoreUrl) {
                    selectQuery.find('.readMoreAboutUs').attr('href', res.readMoreUrl);
                    selectQuery.find('.readMoreAboutUs').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        var url = $(this).attr('href');
                        if (url) {
                            showLoader($('.mainContent'));
                            postForm(url + '?ignoreL=true', new FormData(), function (res) {
                                var selectQuiry = $('.mainContentBodyScroll');
                                if (selectQuiry.length > 0) {
                                    selectQuiry.html(res);
                                    selectQuiry.find('[data-src]').each(function () { $(this).attr('src', $(this).attr('data-src')); });
                                }
                            }, null, function () {
                                hideLoader($('.mainContent'));
                            }, null, 'GET')
                        }
                    });
                }
                else
                    selectQuery.find('.readMoreAboutUs').css('display', 'none');

                selectQuery.css('display', 'block');
                selectQuery.find('img[data-src]').loadImageOnScroll();
            } else {
                selectQuery.css('display', 'none');
            }
        }.bind({ curThis: this }), null, null, null, 'GET');
    });
}

function openModal(linkUrl, title) {
    if (linkUrl) {
        popupCenter({ url: linkUrl, title: title, w: 580, h: 600 });
    }
}

const popupCenter = ({ url, title, w, h }) => {
    const dualScreenLeft = window.screenLeft !== undefined ? window.screenLeft : window.screenX;
    const dualScreenTop = window.screenTop !== undefined ? window.screenTop : window.screenY;

    const width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
    const height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

    const systemZoom = width / window.screen.availWidth;
    const left = (width - w) / 2 / systemZoom + dualScreenLeft
    const top = (height - h) / 2 / systemZoom + dualScreenTop
    const newWindow = window.open(url, title,
        `
      scrollbars=yes,
      width=${w / systemZoom}, 
      height=${h / systemZoom}, 
      top=${top}, 
      left=${left}
      `
    )

    if (window.focus) newWindow.focus();
}

function addMoveToMainStepButton(targetQuiry) {
    var quiry = $('.fgSection');
    if (targetQuiry)
        quiry = targetQuiry;
    quiry.append('<div style="text-align:left;" ><div class="backButtonToMainPage">بازگشت به ورود</div></div>');
    $('.backButtonToMainPage').click(function () {
        $('.fgSection').loadLoginPanel('/TenderWeb/GetLoginConfig', addRegisterButton);
        hideShowNavTitle(true);
        setNavigationTitle('ورود به سامانه');
        initWhatToDoLogin();
        //$('.mySlider').parent().show();
        $('.holderDocumentLinks').initLeftColDocument('/TenderWeb/GetTenderFiles');
        $('.moveToHome').show();
    });
}

function addRegisterButton() {
    $('#holderLoginPanel').append('<div style="padding-right:10px;padding-left:10px;text-align:left;"><span class="makeSmallerFont" >حساب کاربری ندارید؟</span><span class="mainRegisterButton makeSmallerFont" >ثبت نام کنید</span></div>');
    $('.mainRegisterButton').click(function () {
        showRegisterButtons();
        hideShowNavTitle(true);
        setNavigationTitle('ثبت نام در سامانه');
        //$('.mySlider').parent().hide();
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
    postForm('/Account/Dashboard/GetLoginUserInfo', new FormData(), function (res) {
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

function registerWhatToDoNextForReg(formId) {
    whatToDoAfterUserLogin = [];
    whatToDoAfterUserLogin.push({
        curFun: function () {
            //showRegisterButtons();
            showRegisterForm(this.formId);
            hideShowNavTitle(true);
            setNavigationTitle('ثبت نام در سامانه');
            isUserLogin = true;
        }.bind({ formId: formId })
    });
}

function loadOurCustomer() {
    postForm('/Home/GetOurCustomerList', new FormData(), function (res) {
        if (res && res.length > 0) {
            $('#ourCustomer').css('display', 'block');
            $('#ourCustomer .mySlider').initSlider({
                bigCount: 5,
                normalCount: 5,
                smallCount: 2,
                autoStart: 3000,
                data: res,
                dontShowTitle: true
            });
        }
        else
            $('#ourCustomer').css('display', 'none');

    }, function () { $('#ourCustomer').css('display', 'none') }, null, null, 'GET');
}

function loadOurcompany() {
    postForm('/Home/GetOurCompanyList', new FormData(), function (res) {
        if (res && res.length > 0) {
            $('#ourCompany').css('display', 'block');
            $('#ourCompany .mySlider').initSlider({
                bigCount: 5,
                normalCount: 5,
                smallCount: 2,
                autoStart: 3000,
                data: res,
                dontShowTitle: true
            });
        }
        else
            $('#ourCompany').css('display', 'none');

    }, function () { $('#ourCompany').css('display', 'none') }, null, null, 'GET');
}

function isElementInViewport(el) {
    if (typeof jQuery === "function" && el instanceof jQuery)
        el = el[0];

    var rect = el.getBoundingClientRect();

    return (
        rect.top >= 0 &&
        rect.left >= 0 &&
        (rect.bottom - rect.height) <= (window.innerHeight || document.documentElement.clientHeight) &&
        (rect.right - rect.width) <= (window.innerWidth || document.documentElement.clientWidth)
    );
}

function onVisibilityChange(el, callback, isFalseVisbleNeeded) {
    var old_visible;
    return function () {
        var visible = isElementInViewport(el);
        if (((!isFalseVisbleNeeded && visible) || isFalseVisbleNeeded) && visible != old_visible) {
            old_visible = visible;
            if (typeof callback == 'function') {
                callback(visible);
            }
        }
    }
}

$.fn.loadImageOnScroll = function () {
    return this.each(function () {
        var foundObj = $(this);
        var handler = onVisibilityChange(foundObj, function () {
            if ($(this.curThis).attr('data-src')) {
                if (!$(this.curThis).attr('src'))
                    $(this.curThis).attr('src', $(this.curThis).attr('data-src'));
                $(this.curThis).removeAttr('data-src')
            }

        }.bind({ curThis: foundObj }));
        handler(foundObj);
        $(window).on('DOMContentLoaded load resize scroll', handler);
    });
}

function bindFooterPhoneAndAddress() {
    postForm('/Home/GetFooterInfor', new FormData(), function (res) {
        //$('#footerAddress').html(res && res.add ? res.add : '');
        //$('#footerTell1').html(res && res.tell ? res.tell : '');
        //$('#footerTell2').html(res && res.mob ? res.mob : '');
        //$('#footerEmail').html(res && res.email ? res.email : '');
        $('#supportPhone').html(res && res.tell ? res.tell : '');
    }, null, null, null, 'GET');
}

function bindFooterIcons() {
    postForm('/home/GetFooterSocialUrl', new FormData(), function (res) {
        if (res) {
            if (res.linkIn)
                $('#aLinkin').attr('href', res.linkIn);
            else
                $('#aLinkin').css('display', 'none');
            if (res.instageram)
                $('#aInestageram').attr('href', res.instageram);
            else
                $('#aInestageram').css('display', 'none');
            if (res.watapp)
                $('#aWhatapp').attr('href', res.watapp);
            else
                $('#aWhatapp').css('display', 'none');
            if (res.telegeram)
                $('#aTelegeram').attr('href', res.telegeram);
            else
                $('#aTelegeram').css('display', 'none');

            $('.footerSharingIconSection').find('a').click(function (e) {
                e.stopPropagation();
                e.preventDefault();

                openModal($(this).attr('href'), $(this).text());

                return false;
            });
        } else {
            $('#aTelegeram').css('display', 'none');
            $('#aWhatapp').css('display', 'none');
            $('#aInestageram').css('display', 'none');
            $('#aLinkin').css('display', 'none');
        }
    }, null, null, null, 'GET');
}

$(document).ready(function () {
    $('img[data-alt-src]').mouseenter(function () {
        var src = $(this).attr('src');
        var altSrc = $(this).attr('data-alt-src');
        if (src && altSrc) {
            $(this)[0].prevSrc = src;
            $(this).attr('src', altSrc);
        }
    });
    $('img[data-alt-src]').mouseleave(function () {
        var src = $(this)[0].prevSrc;
        if (src) {
            $(this).attr('src', src);
        }
    });
})