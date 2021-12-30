
var animationIndex = 0;

$.fn.initScrollAnimation = function ()
{
    return this.each(function ()
    {
        animationIndex++;
        var foundObj = $(this);
        $("<div id='tempObj_animation_before_" + animationIndex + "' ></div>").insertBefore(foundObj);
        $("<div id='tempObj_animation_after_" + animationIndex + "' ></div>").insertAfter(foundObj);
        var handler = onVisibilityChange($('#tempObj_animation_before_' + animationIndex), function () {
            doAddClassWork(this.curThis);
        }.bind({ curThis: foundObj }));
        $(window).on('DOMContentLoaded load resize scroll', handler);

        var handler2 = onVisibilityChange($('#tempObj_animation_after_' + animationIndex), function () {
            doAddClassWork(this.curThis);
        }.bind({ curThis: foundObj }));
        $(window).on('DOMContentLoaded load resize scroll', handler2);
    });
}

function doAddClassWork(curThis) {
    if ($(curThis).hasClass('animationOnScrollOpacity')) {
        $(curThis).addClass('animationOnScrollOpacityShow');
    } else if ($(curThis).hasClass('animationOnScrollMarginTop')) {
        $(curThis).addClass('animationOnScrollMarginTopShow');
    } else if ($(curThis).hasClass('animationOnScrollOpacityFlip')) {
        $(curThis).addClass('animationOnScrollOpacityFlipShow');
    }
}