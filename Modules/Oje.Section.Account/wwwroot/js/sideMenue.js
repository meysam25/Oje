
$.fn.initSideMenu = function () {
    return this.each(function () {

        $(this).find('.sideMenuItem').click(function (e) {
            e.stopPropagation();
            if ($(this).hasClass('sideSumMenuItemShow')) {
                $(this).removeClass('sideSumMenuItemShow');
                $(this).find('> .sideMenuItemInner > .sideSumMenuItems').slideUp();
                $(this).find('> .sideMenuItemInner > .sideMenuSubIcon').removeClass('fa-chevron-down').addClass('fa-chevron-left');
            } else {
                $(this).addClass('sideSumMenuItemShow');
                $(this).find('> .sideMenuItemInner > .sideSumMenuItems').slideDown();
                $(this).find('> .sideMenuItemInner > .sideMenuSubIcon').removeClass('fa-chevron-left').addClass('fa-chevron-down');
            }
        });

        var curURL = location.pathname;
        var hasFound = false;
        var openMenuInterval = null;
        $(this).find('a').each(function () {
            if ($(this).attr('href') == curURL) {
                var arrObj = [];
                var targetQuery = $(this).parent().closest('.sideMenuItem');
                while (targetQuery.length > 0) {
                    arrObj.push(targetQuery[0]);
                    targetQuery = targetQuery.parent().closest('.sideMenuItem');
                }
                $(this).addClass('sideSumMenuItemActive');
                openMenuInterval = setInterval(function () {
                    if (arrObj.length > 0) {
                        var lastObj = arrObj.pop();
                        $(lastObj).click();
                    } else {
                        clearInterval(openMenuInterval);
                        $('.sideMenuHolder2').animate({
                            scrollTop: $('a[href="' + curURL +'"]').offset().top
                        }, 1000);
                    }
                }, 300);
                hasFound = true;
            }
        });
        if (hasFound == false) {
            var allParts = curURL.split('/');
            allParts.pop();
            curURL = allParts.join('/');
            $(this).find('a').each(function () {
                if ($(this).attr('href')) {
                    var allPartsCur = $(this).attr('href').split('/');
                    allPartsCur.pop();
                    var newCurUrl = allPartsCur.join('/');
                    if (newCurUrl == curURL) {
                        var targetQuery = $(this).parent().closest('.sideMenuItem');
                        while (targetQuery.length > 0) {
                            targetQuery.click();
                            targetQuery = targetQuery.parent().closest('.sideMenuItem');
                        }
                        $(this).addClass('sideSumMenuItemActive');
                        hasFound = true;
                    }
                }
            });
        }


        $(this)[0].openSideMenu = function () {
            $('body').removeClass('closeSideMenu');
        }
        $(this)[0].closeSideMenu = function () {
            $('body').addClass('closeSideMenu');
        }
        $(this)[0].toggleSideMenu = function () {
            $('body').toggleClass('closeSideMenu');
        }
        if ($(window).width() <= 900 && location.pathname != '/Account/Dashboard/Index') {
            $(this)[0].closeSideMenu();
        }
    })
}


