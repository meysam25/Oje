$.fn.initMenue = function (url) {
    return this.each(function ()
    {
        postForm(url, new FormData(), function (res)
        {
            var template = '';

            var curPath = location.pathname;

            if (res && res.length > 0) {
                for (var i = 0; i < res.length; i++) {
                    var item = res[i];
                    if (item.title) {
                        template += '<a href="' + (item.link ? item.link : '#') + '" title="' + item.title + '"  class="menuItem ' + (curPath && curPath.toLowerCase() == item.link ? 'activeMenuItem' : '') +'">'+ item.title +'</a>';
                    }
                }
            }

            $(this).find('.headerMenu').html(template);
        }.bind(this), null, null, null, 'GET');
        $(this).find('.topMenuNewResponsiveButton').click(function (e) { $(this).toggleClass('showMenue'); e.preventDefault(); e.stopPropagation(); }.bind(this));
        $(this).click(function (e) { e.preventDefault(); e.stopPropagation(); $(this).removeClass('showMenue') }.bind(this));
        $(this).click(function (e) { e.preventDefault(); e.stopPropagation(); $(this).removeClass('showMenue') }.bind(this));
        $(this).find('.headerMenu').click(function (e) { e.stopPropagation(); });
    });
};