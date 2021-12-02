
$.fn.initSideMenu = function () {
    return this.each(function () {
        $(this).find('.sideMenuItem').click(function () {
            if ($(this).hasClass('sideSumMenuItemShow')) {
                $(this).removeClass('sideSumMenuItemShow');
                $(this).find('.sideSumMenuItems').slideUp();
                $(this).find('.sideMenuSubIcon').removeClass('fa-chevron-down').addClass('fa-chevron-right');
            } else {
                $(this).addClass('sideSumMenuItemShow');
                $(this).find('.sideSumMenuItems').slideDown();
                $(this).find('.sideMenuSubIcon').removeClass('fa-chevron-right').addClass('fa-chevron-down');
            }
        });
        $(this).find('.sideSumMenuItems').hide();
        var curURL = location.pathname;
        var hasFound = false;
        $(this).find('.sideSumMenuItems .sideSumMenuItem').each(function () {
            if ($(this).attr('href') == curURL) {
                $(this).closest('.sideMenuItem').click();
                $(this).addClass('sideSumMenuItemActive');
                hasFound = true;
            }
        });
        if (hasFound == false) {
            var allParts = curURL.split('/');
            allParts.pop();
            curURL = allParts.join('/');
            $(this).find('.sideSumMenuItems .sideSumMenuItem').each(function () {
                if ($(this).attr('href')) {
                    var allPartsCur = $(this).attr('href').split('/');
                    allPartsCur.pop();
                    var newCurUrl = allPartsCur.join('/');
                    if (newCurUrl == curURL) {
                        $(this).closest('.sideMenuItem').click();
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
        if ($(window).width() <= 900) {
            $(this)[0].closeSideMenu();
        }
    })
}


