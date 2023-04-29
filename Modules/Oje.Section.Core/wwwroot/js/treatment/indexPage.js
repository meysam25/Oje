


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
function initWhatToDoLogin() {
    whatToDoAfterUserLogin = [];
    whatToDoAfterUserLogin.push({
        curFun: function () {
            showLoader($('body'));
            location.href = '/Account/Dashboard/Index';
        }
    });
}
function addTopLeftIcons(title, imgSrc, des, isVisibleAllways) {
    return '<div class="holderLicenceDiv"><img ' + (des ? 'style="cursor:pointer"' : '') + '  class="' + (isVisibleAllways ? 'mainHeaderIranLogo' : 'mainHeaderElectronDevelopLogo') + '" title="' + title + '" src="' + imgSrc + '"  ' + (des ? 'data-des=\'' + des + '\'' : '') + ' /></div>';
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


$.fn.loadTopMenu = function (url) {

    function getTopMenuTemplate(l1Item) {
        var result = '';

        result += '<li>';
        if (l1Item.link)
            result += '<a title="' + l1Item.title + '" href="' + (l1Item.link ? l1Item.link : '#') + '">' + l1Item.title + (l1Item.childs && l1Item.childs.length > 0 ? '<i class="fa fa-angle-left moreMenuItems"></i>' : '') + '</a>';
        else
            result += '<a title="' + l1Item.title + '" href="#">' + l1Item.title + (l1Item.childs && l1Item.childs.length > 0 ? '<i class="fa fa-angle-left moreMenuItems"></i>' : '') + '</a>';

        if (l1Item.childs && l1Item.childs.length > 0) {
            result += '<ul>';
            for (var j = 0; j < l1Item.childs.length; j++) {
                var level2Item = l1Item.childs[j];
                result += '<li>';
                if (level2Item.link)
                    result += '<a title="' + level2Item.title + '" href="' + (level2Item.link ? level2Item.link : '#') + '">' + level2Item.title + (level2Item.childs && level2Item.childs.length > 0 ? '<i class="fa fa-angle-left moreMenuItems"></i>' : '') + '</a>';
                else
                    result += '<a title="' + level2Item.title + '" href="#">' + level2Item.title + (level2Item.childs && level2Item.childs.length > 0 ? '<i class="fa fa-angle-left moreMenuItems"></i>' : '') + '</a>';

                if (level2Item.childs && level2Item.childs.length > 0) {
                    result += '<ul>';
                    for (var m = 0; m < level2Item.childs.length; m++) {
                        var level3Item = level2Item.childs[m];
                        result += '<li>';
                        if (level3Item.link)
                            result += '<a title="' + level3Item.title + '" href="' + (level3Item.link ? level3Item.link : '#') + '">' + level3Item.title + (level3Item.childs && level3Item.childs.length > 0 ? '<i class="fa fa-angle-left moreMenuItems"></i>' : '') + '</a>';
                        else
                            result += '<a title="' + level3Item.title + '" href="#">' + level3Item.title + (level3Item.childs && level3Item.childs.length > 0 ? '<i class="fa fa-angle-left moreMenuItems"></i>' : '') + '</a>';

                        if (level3Item.childs && level3Item.childs.length > 0) {
                            result += '<ul>';
                            for (var n = 0; n < level3Item.childs.length; n++) {
                                result += '<li>';
                                var level4Item = level3Item.childs[n];
                                if (level4Item.link)
                                    result += '<a title="' + level4Item.title + '" href="' + (level4Item.link ? level4Item.link : '#') + '">' + level4Item.title + (level4Item.childs && level4Item.childs.length > 0 ? '<i class="fa fa-angle-left moreMenuItems"></i>' : '') + '</a>';
                                else
                                    result += '<a title="' + level4Item.title + '" href="#">' + level4Item.title + (level4Item.childs && level4Item.childs.length > 0 ? '<i class="fa fa-angle-left moreMenuItems"></i>' : '') + '</a>';
                                result += '</li>';
                            }
                            result += '</ul>';
                        }
                        result += '</li>';
                    }
                    result += '</ul>';
                }
                result += '</li>';
            }
            result += '</ul>';
        }
        result += '</li>';

        return result;
    }
    return this.each(function () {
        postForm(url, new FormData(), function (res) {
            var template = ''
            if ($('#mainSiteLogoIcon').length > 0)
                template += '<a title="' + $('#mainSiteLogoIcon').attr('title') + '" href="/"><img class="floatingIcon" width="50" height="50" alt="' + $('#mainSiteLogoIcon').attr('title') + '" src="' + $('#mainSiteLogoIcon').find('img').attr('src') + '" /></a>';
            if (res && res.length > 0) {
                template += '<ul class="topMenuNew">';
                for (var i = 0; i < res.length; i++) {
                    var level1Item = res[i];
                    template += getTopMenuTemplate(level1Item);

                }
                template += '</ul>';
            }
            $(this.curThis).html(template);
            $('.topMenuNewResponsiveButton').initTopMenuResponsiveButton();
        }.bind({ curThis: $(this) }), null, null, null, 'GET');
    });
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
        $('#supportPhone').html(res && res.tell ? res.tell : '');
        $('#supportPhone').attr('href', (res && res.tell ? ('tel:' + res.tell.replace(/ /g, '').replace('-', '')) : ''));
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

function bindloginButton() {
    $('.footerLoginBox').click(function () {
        var newId = 'holderLoginModal';
        var modalId = 'loginForgetPasswordModal';
        var url = '/ContractWeb/GetLoginConfig';
        var showModal = true;
        if ($('#' + newId).length == 0) {
            $(this).closest('.container').append('<div id="' + newId + '" ></div>');
            showLoader($('body'));
            loadJsonConfig(url, newId, function () {
                hideLoader($('body'));
                if (this.showModal) {
                    $('#' + this.modalId).modal('show');
                }
            }.bind({ modalId: modalId, showModal: showModal }), 'GET');
        } else {
            $('#' + modalId).modal('show');
        }
    });
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
    $('img[data-src]').loadImageOnScroll();
    bindFooterPhoneAndAddress();
    loadLicenceF($('.licenceHolderW'));
})