

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
                    $(this.curThis).removeClass('makeMyContainer100').addClass('floatingFooterSectionMakeFloat')
                } else {
                    $(this.curThis).removeClass('floatingFooterSectionMakeFloat').addClass('makeMyContainer100')
                }
            }.bind({ curPlaceHolderObj: curPlaceHolderObj, curThis: this }), true);

            $(window).on('DOMContentLoaded load resize scroll', handler);
        }
    });
}

function isElementInViewport(el) {
    if (typeof jQuery === "function" && el instanceof jQuery)
        el = el[0];

    var rect = el.getBoundingClientRect();

    return (
        rect.top >= 0 &&
        rect.left >= 0 &&
        rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
        rect.right <= (window.innerWidth || document.documentElement.clientWidth)
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