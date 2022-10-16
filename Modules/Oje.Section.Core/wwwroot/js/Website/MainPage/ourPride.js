
$.fn.initAutoNumber = function (calceTime) {
    if (!calceTime)
        return this;
    return this.each(function () {
        var foundQuery = $(this).find('[data-target-value]');
        if (foundQuery.length > 0) {
            var foundObj = $(this).find('[data-target-value]')[0];
            foundObj.startAutoNumber = function () {
                var curValue = 0;
                var maxValue = Number.parseInt($(this).attr('data-target-value'));
                var valueIncremant = Math.floor(maxValue / calceTime);
                this.tInterval = setInterval(function () {
                    if (valueIncremant <= 0)
                        valueIncremant = 1;
                    curValue = curValue + valueIncremant;
                    if (curValue >= maxValue) {
                        curValue = maxValue;
                        clearInterval(this.curThis.tInterval);
                    }
                    $(this.curThis).html(curValue);
                }.bind({ curThis: this }), (valueIncremant == 0 ? 250 : 5));
            };
            var handler = onVisibilityChange(foundObj, function () {
                if (!this.curThis.isAutoStarted) {
                    this.curThis.startAutoNumber();
                    this.curThis.isAutoStarted = true;
                }
            }.bind({ curThis: foundObj }));

            $(window).on('DOMContentLoaded load resize scroll', handler);
        }
    });
};


$.fn.loadAndBindOurPride = function (url) {

    function getOurPrideItem(imgSrc, title) {
        var result = '';

        if (title) {
            result += '<div class="ourPrideSectionItem">';
            if (imgSrc)
                result += '<img width="80" alt="'+ title.replace('{','').replace('}','') +'" height="80" data-src="' + imgSrc + '" />';
            result += ' <div >';
            result += getOurPrideItemTemplate(title);
            result += '</div>';
            result += '</div>';
        }

        return result;
    }

    function getOurPrideItemTemplate(title) {
        var result = '';

        if (title) {
            if (title.indexOf('{') == -1 || title.indexOf('}') == -1) {
                result = '<span class="ourPrideSectionItemLightText">' + title + '</span>';
            } else {
                var leftPart = title.split('{')[0];
                var rightPart = title.split('}')[1];
                var centerNumberPart = title.split('{')[1].split('}')[0];
                if (isNaN(centerNumberPart))
                    centerNumberPart = 1000;
                if (leftPart)
                    result += '<span class="ourPrideSectionItemLightText">' + leftPart + '</span>';
                if (centerNumberPart) {
                    result += '<span class="ourPrideSectionItemLightBoldTxt" data-target-value="' + centerNumberPart +'">0</span>';
                }
                if (rightPart)
                    result += '<span class="ourPrideSectionItemLightText">' + rightPart + '</span>';
            }
        }

        return result;
    }

    return this.each(function () {
        postForm(url, new FormData(), function (res) {
            var template = '';
            if (res) {
                template += getOurPrideItem(res.image1_address, res.title1);
                template += getOurPrideItem(res.image2_address, res.title2);
                template += getOurPrideItem(res.image3_address, res.title3);
                template += getOurPrideItem(res.image4_address, res.title4);
                $(this.curThis).find('.ourPrideSectionTitle span').html(res.title);
            } else {
                $(this.curThis).css('display', 'none');
            }
            $(this.curThis).find('.ourPrideSectionItems').html(template);
            $(this.curThis).find('img[data-src]').loadImageOnScroll();
            $(this.curThis).find('.ourPrideSectionItem').initAutoNumber(300);
        }.bind({ curThis: this }), null, null, null, 'GET');
    });
}