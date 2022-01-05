
$.fn.initInquiryTab = function () {
    return this.each(function () {
        var curElement = $(this)[0];
        $(curElement).find('.inquirySectionTabHeaderItemInner').click(function () {
            $('body').click();
            $(this).closest('.inquirySectionTab')[0].clickHeaderTabItem(this);
        });

        curElement.getSelectTabIndex = function (curThis) {
            var foundIndex = -1;
            $(curThis).closest('.inquirySectionTabHeaderItems').find('.inquirySectionTabHeaderItemInner').each(function (curIndex) {
                if ($(curThis)[0] == $(this)[0]) {
                    foundIndex = curIndex;
                    return false;
                }
            });
            return foundIndex;
        }

        curElement.clickHeaderTabItem = function (curThis) {
            if ($(curThis).hasClass('inquirySectionTabHeaderItemActive'))
                return false;
            var foundIndex = $(curThis).closest('.inquirySectionTab')[0].getSelectTabIndex(curThis);
            var curUrl = $(curThis).closest('.inquirySectionTabHeaderItem').attr('data-json-url');
            $(curThis).closest('.inquirySectionTabHeaderItems').find('.inquirySectionTabHeaderItemActive').removeClass('inquirySectionTabHeaderItemActive');
            $(curThis).addClass('inquirySectionTabHeaderItemActive');
            var selectQuery = $(curThis).closest('.inquirySectionTab').find('.inquirySectionTabBodyItems');
            if (foundIndex > -1) {
                selectQuery.find('.inquirySectionTabHeaderItemActive').removeClass('inquirySectionTabHeaderItemActive').html('');
                var holderSelector = selectQuery.find('.inquirySectionTabBodyItem').eq(foundIndex);
                holderSelector.addClass('inquirySectionTabHeaderItemActive');
                $(curThis).closest('.inquirySectionTab')[0].loadStepConfig(curUrl, holderSelector);
            }
        }

        curElement.showLoader = function (holderSelector) {
            holderSelector.html('<div style="height:100px;"></div>');
            showLoader(holderSelector);
        };

        curElement.loadStepConfig = function (url, holderSelector) {
            $(this)[0].showLoader(holderSelector);
            loadJsonConfig(url, holderSelector);
        };

        $(curElement).find('.inquirySectionTabHeaderItem:first-child').find('.inquirySectionTabHeaderItemInner').click();

    });
};