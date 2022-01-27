

$.fn.loadTopMenu = function (url) {

    function getTopMenuTemplate(l1Item) {
        var result = '';

        result += '<div class="headerMenuItem ">';
        result += '<div class="headerMenuItemInner ' + (l1Item.childs && l1Item.childs.length > 0 ? 'headerMenuItemHasArrow' : '') + '">';
        if (l1Item.link)
            result += '<a href="' + (l1Item.link ? l1Item.link : '#') + '" class="headerMenuItemLink">' + l1Item.title + '</a>';
        else
            result += '<span class="headerMenuItemLink">' + l1Item.title + '</span>';
        result += '</div>';

        if (l1Item.childs && l1Item.childs.length > 0) {
            result += '<div class="headerMenuItemSumMenuItems">';
            result += '<div class="myContainer">';
            result += '<div class="headerMenuItemSumMenuItemsInner">';
            result += '<div class="headerMenuItemGroup headerMenuMoveBackButton">';
            result += '<span class="headerMenuItemLink">' + l1Item.title + '</span>';
            result += '</div>';
            for (var j = 0; j < l1Item.childs.length; j++) {
                var level2Item = l1Item.childs[j];
                result += '<div class="headerMenuItemGroup">';
                result += '<div class="headerMenuItemGroupTitle">' + level2Item.title + '</div>';
                if (level2Item.childs && level2Item.childs.length > 0) {
                    for (var m = 0; m < level2Item.childs.length; m++) {
                        result += '<a href="' + (level2Item.childs[m].link ? level2Item.childs[m].link : '#') + '" title="' + level2Item.childs[m].title + '" class="headerMenuItemGroupItem">' + level2Item.childs[m].title + '</a>';
                    }
                }
                result += '</div>';
            }
            result += '</div>';
            result += '</div>';
            result += '</div>';
        }
        result += '</div>';

        return result;
    }
    return this.each(function () {
        postForm(url, new FormData(), function (res) {
            var template = '';
            if (res && res.length > 0) {
                for (var i = 0; i < res.length; i++) {
                    var level1Item = res[i];
                    template += getTopMenuTemplate(level1Item);

                }
            }
            $(this.curThis).html(template);
            $('.headerMenu').initNavResButton();
        }.bind({ curThis: $(this) }));
    });
}


$.fn.initNavResButton = function () {
    return this.each(function () {
        $(this).find('.topMenuBtton').click(function (e) {
            $(this).closest('.headerMenu').toggleClass('topMenuBttonOpen');
        });
        $(this).find('.headerMenuMoveBackButton').click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            $(this).closest('.headerMenuItem').addClass('closeHoverMenu');
        });
        $(this).find('.headerMenuItem').click(function (e) {
            $(this).removeClass('closeHoverMenu');
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
        $('#footerTell2, #footerTell1').html(res && res.tell ? res.tell : '');
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

function bindFooterExteraLinks() {
    postForm('/Home/GetFooterExteraLink', new FormData(), function (res) {
        $('#holderfooterProductMenuItems').html('');
        if (res && res.length > 0)
            for (var i = 0; i < res.length; i++)
                $('#holderfooterProductMenuItems').append('<a href="' + res[i].l + '" title="' + res[i].t + '" class="footerProductMenuItem">' + res[i].t + '</a>');
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
                        childTemplates += '<a href="' + res[i].chi[j].l + '" title="' + res[i].chi[j].t + '" class="footerNavGroupItemsItem">' + res[i].chi[j].t + '</a>';
                template += '<div class="footerNavGroupItems"><div class="footerNavGroupItemsTitle">' + res[i].t + '</div>' + childTemplates + '</div>';
            }
        $('#footerNavMenu').html(template);
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