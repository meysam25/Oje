
$.fn.loadImageOnScroll = function () {
    return this.each(function ()
    {
        var foundObj = $(this);
        var handler = onVisibilityChange(foundObj, function () {
            if (!$(this.curThis).attr('src'))
                $(this.curThis).attr('src', $(this.curThis).attr('data-src'))
        }.bind({ curThis: foundObj }));
        setTimeout(function () { handler(foundObj) }, 100)
        $(window).on('DOMContentLoaded load resize scroll', handler);
    });
}