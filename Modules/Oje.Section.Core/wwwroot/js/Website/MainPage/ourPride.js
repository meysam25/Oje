
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
