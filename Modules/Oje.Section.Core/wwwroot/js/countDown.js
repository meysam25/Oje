
$.fn.initCountDown = function () {

    return this.each(function () {
        var curElement = $(this)[0];

        curElement.start = function () {
            if (this.isStarted)
                return;
            this.isStarted = true;
            var maxNumber = Number.parseInt($(this).attr('data-maxNumber'));
            var cdText = $(this).attr('data-countDownText');
            var cdfText = $(this).attr('data-finishedCountDownText');
            $(this).removeClass('countDownNumberFinished');
            $(this).removeAttr('onClick');
            var curOnClickAttr = $(this).attr('data-onClick');

            this.curNumber = maxNumber;
            $(this).html(cdText.replace('{ph}', maxNumber));
            this.interValPointer =
                setInterval(function () {
                    var curNumber = this.curElement.curNumber;
                    curNumber--;
                    if (curNumber <= 0) {
                        clearInterval(this.curElement.interValPointer);
                        $(this.curElement).html(this.cdfText);
                        $(this.curElement).addClass('countDownNumberFinished');
                        this.curElement.isStarted = false;
                        $(this.curElement).attr('onClick', this.curOnClickAttr);
                    } else {
                        $(this.curElement).html(this.cdText.replace('{ph}', curNumber));
                        this.curElement.curNumber = curNumber;
                    }

                }.bind({ curElement: this, maxNumber: maxNumber, cdText: cdText, cdfText: cdfText, curOnClickAttr: curOnClickAttr }), 1000);
        };
    });
};