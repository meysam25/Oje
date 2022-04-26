

$.fn.initResponsiveClickMenu = function () {
    return $(this).each(function () {
        if ($('body').width() <= 1100) {
            $(this).click(function (e) {
                if ($(this).find('>a>i.moreMenuItems').length > 0) {
                    $(this).toggleClass('openThisMenuItem');
                }
                e.stopPropagation();
            });
        }
    });
}

$.fn.initTopMenuResponsiveButton = function () {
    $('.topMenuNew li').initResponsiveClickMenu();
    function showTopMenue() {
        $('.topMenuNew').addClass('topMenuNewActive');
    }
    function hideTopMenue() {
        $('.topMenuNew').removeClass('topMenuNewActive');
    }
    return this.each(function () {
        $(this).click(function () {
            if ($(this).hasClass('topMenuNewResponsiveButtonActive')) {
                if ($(this).hasClass('topMenuNewResponsiveButtonActiveInvert'))
                    showTopMenue();
                else
                    hideTopMenue();

                $(this).toggleClass('topMenuNewResponsiveButtonActiveInvert');

            } else {
                $(this).addClass('topMenuNewResponsiveButtonActive');
                showTopMenue();
            }
        });
    });
}

