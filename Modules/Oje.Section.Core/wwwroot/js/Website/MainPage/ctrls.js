
$.fn.initTabCtrl = function ()
{
    return this.each(function ()
    {
        $(this).find('.TabHeaderItem').click(function ()
        {
            if (!$(this).hasClass('TabHeaderItemActive')) {
                var foundIndex = -1;
                var curThis = this;
                $(this).closest('.holderTabHeaderItems').find('.TabHeaderItem').each(function (curIndex)
                {
                    if ($(this)[0] == $(curThis)[0]) {
                        foundIndex = curIndex;
                        return false;
                    }
                });
                $(this).closest('.holderTabHeaderItems').find('.TabHeaderItemActive').removeClass('TabHeaderItemActive');
                $(this).addClass('TabHeaderItemActive');
                if (foundIndex != -1) {
                    var contentHolder = $(this).closest('.tabCtrl').find('.holderTabContentItems');
                    contentHolder.find('.TabContentItemActive').removeClass('TabContentItemActive');
                    contentHolder.find('.TabContentItem').eq(foundIndex).addClass('TabContentItemActive');
                }
            }
        });
    });
};