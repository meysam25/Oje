
$.fn.initSlider = function (options) {

    function getLeftRightButtonTemplate() {
        return '<div class="mySliderItemArrowLeft"></div><div class="mySliderItemArrowRight"></div>';
    }

    function getImageWidth(options, curWindowWidth) {
        var result = 0;

        if (curWindowWidth >= 1000)
            result = curWindowWidth / options.bigCount;
        else if (curWindowWidth < 1000 && curWindowWidth >= 500)
            result = curWindowWidth / options.normalCount;
        else if (curWindowWidth < 500)
            result = curWindowWidth / options.smallCount;

        if (isNaN(result) || result == 0)
            result = 300;

        return Math.floor(result);
    }

    function getSliderItemTemplate(item, options, curWindowWidth) {
        var result = '';

        if (item.img) {
            result += '<a title="' + item.title +'" ' + (item.url ? 'target="_blank"' : '') + (' href="' + (item.url ? item.url : '#') + '"') + '" style="width:' + getImageWidth(options, curWindowWidth) + 'px" class="mySliderItem">';
            result += '<img width="300" height="300" class="mySliderMainImage"  alt="' + (item.title ? item.title : '') + '" data-src="' + item.img + '" />';
            if (item.title && !options.dontShowTitle)
                result += '<div class="mySliderItemTitle">' + item.title + '</div>';
            if (item.subTitle)
                result += '<div class="mySliderItemSubTitle">' + item.subTitle + '</div>';
            result += '</a>';
        }

        return result;
    }

    function moveSliderToNext() {
        var curElement = $(this)[0];
        if (!curElement || !curElement.options || !curElement.options.data || curElement.options.data.length == 0)
            return;
        if (curElement.isProcessing)
            return;
        var holderQuery = $(this).find('.mySliderItemsInner');
        var eachItemWidth = getImageWidth(curElement.options, $(this).width());
        var curWidth = eachItemWidth * curElement.options.data.length;
        var holderInnerItemsMargin = holderQuery.css('margin-right');
        if (holderInnerItemsMargin)
            holderInnerItemsMargin = Number.parseInt(holderInnerItemsMargin.replace('px', ''));
        if (!holderInnerItemsMargin)
            holderInnerItemsMargin = 0;
        holderInnerItemsMargin -= eachItemWidth;
        if (holderQuery.width() < curWidth) {
            curElement.isProcessing = true;
            holderQuery.css('margin-right', holderInnerItemsMargin + 'px');
            setTimeout(function () { this.curThis.isProcessing = false; }.bind({ curThis: curElement }), 310);
        } else {
            curElement.isProcessing = true;
            holderQuery.css('margin-right', 0 + 'px');
            setTimeout(function () { this.curThis.isProcessing = false; }.bind({ curThis: curElement }), 310);
        }
        updateImageSrcIfNeeded(curElement);
    }

    function moveSliderToPrev() {
        var curElement = $(this)[0];
        if (!curElement || !curElement.options || !curElement.options.data || curElement.options.data.length == 0)
            return;
        if (curElement.isProcessing)
            return;
        var holderQuery = $(this).find('.mySliderItemsInner');
        var eachItemWidth = getImageWidth(curElement.options, $(this).width());
        var curWidth = eachItemWidth * curElement.options.data.length;
        var previewWidth = $(this).find('.mySliderItems').width();
        var holderInnerItemsMargin = holderQuery.css('margin-right');
        if (holderInnerItemsMargin)
            holderInnerItemsMargin = Number.parseInt(holderInnerItemsMargin.replace('px', ''));
        if (!holderInnerItemsMargin)
            holderInnerItemsMargin = 0;
        holderInnerItemsMargin += eachItemWidth;
        if ((holderInnerItemsMargin - eachItemWidth) < 0) {
            curElement.isProcessing = true;
            holderQuery.css('margin-right', holderInnerItemsMargin + 'px');
            setTimeout(function () { this.curThis.isProcessing = false; }.bind({ curThis: curElement }), 310);
        } else {
            curElement.isProcessing = true;
            holderQuery.css('margin-right', (-1 * (curWidth - previewWidth)) + 'px');
            setTimeout(function () { this.curThis.isProcessing = false; }.bind({ curThis: curElement }), 310);
        }
        updateImageSrcIfNeeded(curElement);
    }

    function updateImageSrcIfNeeded(curThis) {
        $(curThis).find('img[data-src]').each(function () {
            if ($(this).is(':visible')) {
                if (!$(this).attr('src'))
                    $(this).attr('src', $(this).attr('data-src'));
                $(this).removeAttr('data-src')
            }
        });
    }

    function initSliderFunctions(curThis) {
        $(curThis)[0].moveNext = moveSliderToNext;
        $(curThis)[0].movePrev = moveSliderToPrev;

        $(curThis).find('.mySliderItemArrowLeft').click(function () { $(this).closest('.mySlider')[0].moveNext() });
        $(curThis).find('.mySliderItemArrowRight').click(function () { $(this).closest('.mySlider')[0].movePrev() });
        $(curThis).find('a.mySliderItem').click(function (e) { if ($(this).attr('href') == '#') { e.preventDefault(); return false; } });
    }

    return this.each(function () {
        var template = '';
        var windowWidth = $(this).width();
        template += '<div class="mySliderItems"><div class="mySliderItemsInner">';
        if (options.data && options.data.length > 0) {
            for (var i = 0; i < options.data.length; i++) {
                template += getSliderItemTemplate(options.data[i], options, windowWidth);
            }
        }
        template += '</div></div>';

        template += getLeftRightButtonTemplate();

        if (options.data && options.data.length > 0)
            $(this).html(template);
        else
            $(this).css('display', 'none');
        $(this).find('img').loadImageOnScroll();
        $(this)[0].options = options;
        initSliderFunctions(this);
        if (options.autoStart) {
            $(this)[0].autoPlayInterval = setInterval(function () { $(this.curThis)[0].moveNext(); }.bind({ curThis: this }), options.autoStart);
            $(this).mouseenter(function () { clearInterval($(this)[0].autoPlayInterval); });
            $(this).mouseleave(function () { clearInterval($(this)[0].autoPlayInterval); $(this)[0].autoPlayInterval = setInterval(function () { $(this.curThis)[0].moveNext(); }.bind({ curThis: this }), options.autoStart); });
        }
        $(window).resize(function () {
            clearTimeout($(this.curThis)[0].reinitInterval);
            $(this.curThis)[0].reinitInterval = setTimeout(function () {
                clearInterval($(this.curThis)[0].autoPlayInterval);
                $(this.curThis).html('');
                $(this.curThis).initSlider($(this.curThis)[0].options);
            }.bind({ curThis: this.curThis }), 100);
        }.bind({ curThis: this }));
    });
};