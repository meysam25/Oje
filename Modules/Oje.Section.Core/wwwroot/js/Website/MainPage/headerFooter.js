
function bindTopLeftIcons() {
    var url = '/Home/GetTopLeftIconList';
    postForm(url, new FormData(), function (res) {
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
            $('#placeholderTopLeftIcon').replaceWith(html);
            $('.mainHeaderIranLogo,.mainHeaderElectronDevelopLogo').each(function () {
                var title = $(this).attr('title');
                var desc = $(this).attr('data-des');
                if (desc) {
                    var modalObj = {
                        id: uuidv4RemoveDash(),
                        title: title,
                        modelBody: desc,
                        class: "makeImage100"
                    };
                    $(this).attr('data-modal-id', modalObj.id);
                    $('body').append(getModualTemplate(modalObj))
                }
            });
            $('.mainHeaderIranLogo[data-des],.mainHeaderElectronDevelopLogo[data-des]').click(function () {
                var modalId = $(this).attr('data-modal-id');
                $('#' + modalId).modal('show');
            });
        }
    });
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
        } else {
            $('#aTelegeram').css('display', 'none');
            $('#aWhatapp').css('display', 'none');
            $('#aInestageram').css('display', 'none');
            $('#aLinkin').css('display', 'none');
        }
    });
}

function addTopLeftIcons(title, imgSrc, des, isVisibleAllways) {
    return '<img ' + (des ? 'style="cursor:pointer"' : '') + '  class="' + (isVisibleAllways ? 'mainHeaderIranLogo' : 'mainHeaderElectronDevelopLogo') + '" title="' + title + '" src="' + imgSrc + '" width="85" height="77" ' + (des ? 'data-des=\'' + des + '\'' : '') + ' />';
}

$.fn.loadTopMenu = function (url) {

    function getTopMenuTemplate(l1Item) {
        var result = '';

        result += '<li>';
        if (l1Item.link)
            result += '<a href="' + (l1Item.link ? l1Item.link : '#') + '">' + l1Item.title + (l1Item.childs && l1Item.childs.length > 0 ? '<i class="fa fa-angle-left moreMenuItems"></i>' : '') + '</a>';
        else
            result += '<a href="#">' + l1Item.title + (l1Item.childs && l1Item.childs.length > 0 ? '<i class="fa fa-angle-left moreMenuItems"></i>' : '') + '</a>';

        if (l1Item.childs && l1Item.childs.length > 0) {
            result += '<ul>';
            for (var j = 0; j < l1Item.childs.length; j++) {
                var level2Item = l1Item.childs[j];
                result += '<li>';
                if (level2Item.link)
                    result += '<a href="' + (level2Item.link ? level2Item.link : '#') + '">' + level2Item.title + (level2Item.childs && level2Item.childs.length > 0 ? '<i class="fa fa-angle-left moreMenuItems"></i>' : '') + '</a>';
                else
                    result += '<a href="#">' + level2Item.title + (level2Item.childs && level2Item.childs.length > 0 ? '<i class="fa fa-angle-left moreMenuItems"></i>' : '') + '</a>';

                if (level2Item.childs && level2Item.childs.length > 0) {
                    result += '<ul>';
                    for (var m = 0; m < level2Item.childs.length; m++) {
                        var level3Item = level2Item.childs[m];
                        result += '<li>';
                        if (level3Item.link)
                            result += '<a href="' + (level3Item.link ? level3Item.link : '#') + '">' + level3Item.title + (level3Item.childs && level3Item.childs.length > 0 ? '<i class="fa fa-angle-left moreMenuItems"></i>' : '') + '</a>';
                        else
                            result += '<a href="#">' + level3Item.title + (level3Item.childs && level3Item.childs.length > 0 ? '<i class="fa fa-angle-left moreMenuItems"></i>' : '') + '</a>';

                        if (level3Item.childs && level3Item.childs.length > 0) {
                            result += '<ul>';
                            for (var n = 0; n < level3Item.childs.length; n++) {
                                result += '<li>';
                                var level4Item = level3Item.childs[n];
                                if (level4Item.link)
                                    result += '<a href="' + (level4Item.link ? level4Item.link : '#') + '">' + level4Item.title + (level4Item.childs && level4Item.childs.length > 0 ? '<i class="fa fa-angle-left moreMenuItems"></i>' : '') + '</a>';
                                else
                                    result += '<a href="#">' + level4Item.title + (level4Item.childs && level4Item.childs.length > 0 ? '<i class="fa fa-angle-left moreMenuItems"></i>' : '') + '</a>';
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
            var template = '<img class="floatingIcon" width="50" height="50" alt="' + $('#mainSiteLogoIcon').attr('title') + '" src="' + $('#mainSiteLogoIcon').find('img').attr('src') + '" />';
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
        }.bind({ curThis: $(this) }));
    });
}


$.fn.initNavResButton = function () {
    return this.each(function () {
        $(this).find('.topMenuBtton').click(function (e) {
            var sQuiry = $(this).closest('.headerMenu');
            sQuiry.toggleClass('topMenuBttonOpen');
            if (sQuiry.hasClass('topMenuBttonOpen'))
                $('#headerTagHolder').addClass('makeZIndex5');
            else
                $('#headerTagHolder').removeClass('makeZIndex5');
        });
        $(this).find('.headerMenuMoveBackButton').click(function (e) {
            e.stopPropagation();
            e.preventDefault();
        });
        $(this).find('.headerMenuItem').click(function (e) {
            $(this).toggleClass('openHeaderMenuItemSumMenuItems');
        });
        $(this).find('.headerMenuMoveBackButton > .headerMenuItemLink').click(function () {
            $(this).closest('.headerMenuItem').click();
        });
        $(this).find('.headerMenuItemSumMenuItemsInner').click(function (e) {
            e.stopPropagation();
        });
        $(this).find('.headerMenuItemSumMenuItems').click(function (e) {
            e.preventDefault();
            e.stopPropagation();
        });
        $(this).find('.moveBackButtonInResponsive').click(function (e) {
            e.stopPropagation();
            var sQuiry = $(this).closest('.headerMenu');
            sQuiry.toggleClass('topMenuBttonOpen');
            if (sQuiry.hasClass('topMenuBttonOpen'))
                $('#headerTagHolder').addClass('makeZIndex5');
            else
                $('#headerTagHolder').removeClass('makeZIndex5');
        });
    });
}

$.fn.initShowMoreButtons = function (targetClass) {
    return this.each(function () {
        $(this).click(function () {
            $('.' + targetClass).toggleClass('makeShowMore');
            $(this).toggleClass('moreButtonClicked');
            if ($(this).hasClass('moreButtonClicked')) {
                $(this).html('کمتر');
            } else {
                $(this).html('بیشتر');
            }
        });
    });
}

$.fn.initFloatingFooter = function (placeHolderId) {
    return this.each(function () {
        var curPlaceHolder = $('#' + placeHolderId);
        if (curPlaceHolder.length > 0) {
            var curPlaceHolderObj = curPlaceHolder[0];
            var handler = onVisibilityChange(curPlaceHolderObj, function (currVisible) {
                if (this.curThis[0].disableFloating)
                    return;
                if (currVisible == false) {
                    if (this.curThis.hasClass('makeMyContainer100'))
                        this.curThis.removeClass('makeMyContainer100')
                    if (!this.curThis.hasClass('floatingFooterSectionMakeFloat'))
                        this.curThis.addClass('floatingFooterSectionMakeFloat')
                } else {
                    if (this.curThis.hasClass('floatingFooterSectionMakeFloat'))
                        this.curThis.removeClass('floatingFooterSectionMakeFloat');
                    if (!this.curThis.hasClass('makeMyContainer100'))
                        this.curThis.addClass('makeMyContainer100');
                }
            }.bind({ curPlaceHolderObj: curPlaceHolderObj, curThis: $(this) }), true);

            $(window).on('DOMContentLoaded load resize scroll', handler);
        }
    });
}

$.fn.initFloatingTop = function (placeHolderId, exteraClass) {
    return this.each(function () {
        var curPlaceHolder = $('#' + placeHolderId);
        if (curPlaceHolder.length > 0) {
            var curPlaceHolderObj = curPlaceHolder[0];
            var handler = onVisibilityChange(curPlaceHolderObj, function (currVisible) {
                if (this.curThis[0].disableFloating)
                    return;
                if (currVisible == false) {
                    if (!this.curThis.hasClass('floatingTopSectionMakeFloat'))
                        this.curThis.addClass('floatingTopSectionMakeFloat')
                    if (exteraClass && !this.curThis.hasClass(exteraClass))
                        this.curThis.addClass(exteraClass);
                } else {
                    if (this.curThis.hasClass('floatingTopSectionMakeFloat'))
                        this.curThis.removeClass('floatingTopSectionMakeFloat');
                    if (exteraClass && this.curThis.hasClass(exteraClass))
                        this.curThis.removeClass(exteraClass);
                }
            }.bind({ curPlaceHolderObj: curPlaceHolderObj, curThis: $(this) }), true);

            $(window).on('DOMContentLoaded load resize scroll', handler);
        }
    });
}

$.fn.bindFooterDescription = function (url) {

    function getFooterDescriptionItem(imgSrc, title, desc) {
        var result = '';

        if (title) {
            result += '<div class="footerWeAreMoreThenGoodSectionReson">';

            if (imgSrc) {
                result += '<div>';
                result += '<img alt="' + title + '" width="80" height="80" data-src="' + imgSrc + '" />';
                result += '</div>';
            }

            result += '<div>';
            result += '<div class="footerWeAreMoreThenGoodSectionResonTitle">' + title + '</div>';
            if (desc)
                result += '<div class="footerWeAreMoreThenGoodSectionResonDescription">' + desc + '</div>';


            result += '</div>';
            result += '</div>';
        }

        return result;
    }

    return this.each(function () {
        postForm(url, new FormData(), function (res) {
            var template = '';
            if (res) {
                template += getFooterDescriptionItem(res.logo1_address, res.logoTitle1, res.logoDescription1);
                template += getFooterDescriptionItem(res.logo2_address, res.logoTitle2, res.logoDescription2);
                template += getFooterDescriptionItem(res.logo3_address, res.logoTitle3, res.logoDescription3);
            }
            $(this.curThis).html(template);
            $(this.curThis).find('img[data-src]').loadImageOnScroll();
        }.bind({ curThis: this }));
    });
}

function bindFooterPhoneAndAddress() {
    postForm('/Home/GetFooterInfor', new FormData(), function (res) {
        $('#footerAddress').html(res && res.add ? res.add : '');
        $('#footerTell2').html(res && res.tell ? res.tell : '');
        $('#footerTell1').html(res && res.mob ? res.mob : '');
        $('#footerEmail').html(res && res.email ? res.email : '');
    });
}

function bindFooterSambole() {
    postForm('/Home/GetFooterSambole', new FormData(), function (res) {
        $('#footerSharingIconSection').append(res && res.enamad ? res.enamad : '');
        $('#footerSharingIconSection').append(res && res.samandehi ? res.samandehi : '');
        $('#footerSharingIconSection').find('img[data-src]').loadImageOnScroll();
    });
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

function openModal(linkUrl, title) {
    if (linkUrl, title) {
        popupCenter({ url: linkUrl, title: title, w: 580, h: 600 });
    }
}

function bindFooterExteraLinks() {
    postForm('/Home/GetFooterExteraLink', new FormData(), function (res) {
        $('#holderfooterProductMenuItems').html('');
        if (res && res.length > 0) {
            for (var i = 0; i < res.length; i++)
                $('#holderfooterProductMenuItems').append('<a rel="nofollow sponsored" href="' + res[i].l + '" title="' + res[i].t + '" class="footerProductMenuItem">' + res[i].t + '</a>');
            $('#holderfooterProductMenuItems').find('a').click(function (e) {
                e.stopPropagation();
                e.preventDefault();

                openModal($(this).attr('href'), $(this).text());

                return false;
            });
        }

        else
            $('#holderfooterProductMenuItems').closest('.footerProductMenu').find('.showMoreButton').css('display', 'none');
    });
}

function bindFooterExteraLinkGroups() {
    postForm('/Home/GetFooterExteraLinkGroup', new FormData(), function (res) {
        var template = '';

        if (res && res.length > 0)
            for (var i = 0; i < res.length; i++) {
                var childTemplates = '';
                if (res[i].chi && res[i].chi.length > 0)
                    for (var j = 0; j < res[i].chi.length; j++)
                        childTemplates += '<a rel="nofollow sponsored" href="' + res[i].chi[j].l + '" title="' + res[i].chi[j].t + '" class="footerNavGroupItemsItem">' + res[i].chi[j].t + '</a>';
                template += '<div class="footerNavGroupItems"><div class="footerNavGroupItemsTitle">' + res[i].t + '</div>' + childTemplates + '</div>';
            }
        $('#footerNavMenu').html(template);
        $('#footerNavMenu').find('a').click(function (e) {
            e.stopPropagation();
            e.preventDefault();

            openModal($(this).attr('href'), $(this).text());

            return false;
        });
        $('#footerNavMenu').find('.footerNavGroupItems').click(function () { $(this).toggleClass('footerNavGroupItemsOpen'); });
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