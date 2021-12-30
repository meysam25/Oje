
$.fn.initInquiryTab = function ()
{
    return this.each(function ()
    {
        $(this).find('.inquirySectionTabHeaderItemInner').click(function ()
        {
            if ($(this).hasClass('inquirySectionTabHeaderItemActive'))
                return false;
            var curThis = this;
            var foundIndex = -1;
            $(this).closest('.inquirySectionTabHeaderItems').find('.inquirySectionTabHeaderItemInner').each(function (curIndex)
            {
                if ($(this)[0] == $(curThis)[0]) {
                    foundIndex = curIndex;
                    return false;
                }
            });
            $(this).closest('.inquirySectionTabHeaderItems').find('.inquirySectionTabHeaderItemActive').removeClass('inquirySectionTabHeaderItemActive');
            $(this).addClass('inquirySectionTabHeaderItemActive');
            var selectQuery = $(this).closest('.inquirySectionTab').find('.inquirySectionTabBodyItems');
            if (foundIndex > -1) {
                selectQuery.find('.inquirySectionTabHeaderItemActive').removeClass('inquirySectionTabHeaderItemActive');
                selectQuery.find('.inquirySectionTabBodyItem').eq(foundIndex).addClass('inquirySectionTabHeaderItemActive')
            }
        });
    });
};