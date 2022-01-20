
$.fn.loadImageOnScroll = function () {
    return this.each(function ()
    {
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