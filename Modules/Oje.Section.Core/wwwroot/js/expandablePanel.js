
$.fn.initExpanablePanel = function () {
    return this.each(function ()
    {
        $(this)[0].open = function ()
        {
            if (!$(this).hasClass('expandablePanelOpen')) {
                $(this).addClass('expandablePanelOpen');
                $(this)[0].interValAction = setTimeout(function ()
                {
                    $(this.curThis).find('.expandablePanelBody').addClass('expandablePanelBodyScroll');
                    $(this.curThis)[0].interValAction = null;
                }.bind({curThis:this}), 500);
            }
        };
        $(this)[0].close = function ()
        {
            if ($(this).hasClass('expandablePanelOpen')) {
                $(this).removeClass('expandablePanelOpen');
                if ($(this)[0].interValAction) {
                    clearTimeout($(this)[0].interValAction);
                }
            }
        };
        $(this)[0].toggleOC = function ()
        {
            if (!$(this).hasClass('expandablePanelOpen')) {
                $(this)[0].open();
            } else {
                $(this)[0].close();
            }
        };
        $(this).find('.expandablePanelHeader').click(function ()
        {
            $(this).closest('.expandablePanel')[0].toggleOC();
        });
    });
}