

$.fn.initImageCrapper = function (imgUrl, min, max) {
    function initFunctions(curThis) {
        $(curThis)[0].crapImage = crapImage;
        var buttonsQS = '.siLeftTop,.siRightTop,.siLeftBottom,.siRightBottom';
        $(curThis).find(buttonsQS).mousedown(function () {
            $(curThis)[0].moveObject = this;

        });
        $(curThis).mouseup(function () { $(this)[0].moveObject = null; });
        $(curThis).find('.mouseMoveDetector' + ',' + buttonsQS).mousemove(function (e) {
            var targetMoveObj = $(curThis)[0].moveObject;
            if (targetMoveObj) {
                var curClass = $(targetMoveObj).attr('class');
                var ofSet = $(this).parent().offset();

                var x = e.pageX - ofSet.left - 7.5;
                var y = e.pageY - ofSet.top - 7.5;

                if (curClass == 'siLeftTop' || curClass == 'siLeftBottom') {
                    if (x + 100 > Number.parseInt($(curThis).find('.siRightBottom').css('left').replace('px', ''))) {
                        return;
                    }
                }

                if (curClass == 'siRightTop' || curClass == 'siRightBottom') {
                    if (x - 100 < Number.parseInt($(curThis).find('.siLeftBottom').css('left').replace('px', ''))) {
                        return;
                    }
                }

                if (curClass == 'siLeftTop' || curClass == 'siRightTop') {
                    if (y + 100 > Number.parseInt($(curThis).find('.siRightBottom').css('top').replace('px', ''))) {
                        return;
                    }
                }

                if (curClass == 'siLeftBottom' || curClass == 'siRightBottom') {
                    if (y - 100 < Number.parseInt($(curThis).find('.siLeftTop').css('top').replace('px', ''))) {
                        return;
                    }
                }

                $(targetMoveObj).css('top', y + 'px').css('left', x + 'px');
                updateShadow(curThis, targetMoveObj, x, y);
                crapImage(curThis);
                if (!isValidCrap(curThis))
                    $(curThis).addClass('invalidCImageCtrl');
                else if (min && max)
                    $(curThis).removeClass('invalidCImageCtrl');
            }
        });
    }

    function crapImage(curThis) {
        var imageSize = getCurCrapImageSize(curThis);
        var canvasId = 'tempCanvas';
        if ($('#' + canvasId).length == 0)
            $('body').append('<div style="width:0px;height:0px;overflow:hidden;" ><canvas id="' + canvasId + '"></canvas></div>');

        var img = $(curThis)[0].tempImage;
        var rw = $(curThis)[0].imageWith;
        var rh = $(curThis)[0].imageHeight;
        var nw = $(curThis).find('img').width();
        var nh = $(curThis).find('img').height();
        var th = imageSize.height;
        var tw = imageSize.width;
        var sx = imageSize.sx;
        var sy = imageSize.sy;
        var rx = (rw * sx) / nw;
        var ry = (rh * sy) / nh;
        var resw = (rw * tw) / nw;
        var resH = (rh * th) / nh;

        $('#' + canvasId).attr('width', tw).attr('height', th);
        var ctx = $('#' + canvasId)[0].getContext('2d');
        ctx.drawImage(img, rx, ry, resw, resH, 0, 0, tw, th);
    }

    function getCurCrapImageSize(curThis) {
        var leftPosition = Number.parseInt($(curThis).find('.siLeftTop').css('left').replace('px', ''));
        var topPosition = Number.parseInt($(curThis).find('.siLeftTop').css('top').replace('px', ''));
        var bottomPosition = Number.parseInt($(curThis).find('.siRightBottom').css('top').replace('px', ''));
        var rightPosition = Number.parseInt($(curThis).find('.siRightBottom').css('left').replace('px', ''));
        var width = rightPosition - leftPosition;
        var height = bottomPosition - topPosition;

        return { width: width + 15, height: height + 15, sx: leftPosition, sy: topPosition };
    }

    function isValidCrap(curThis) {
        var curImageSize = getCurCrapImageSize(curThis);
        if (curImageSize.width > curImageSize.height) {
            var curRate = (curImageSize.width / curImageSize.height);
            return curRate > min && curRate < max;
        }
        else {
            var curRate = (curImageSize.height / curImageSize.width);
            return curRate > min && curRate < max;
        }
    }

    function updateShadow(curObj, movingButtonObj, x, y) {
        var curClass = $(movingButtonObj).attr('class');
        if (curClass) {
            if (curClass.indexOf('Top') > -1)
                updateTopShadow(curObj, y, movingButtonObj);
            if (curClass.indexOf('Left') > -1)
                updateLeftShadow(curObj, x, movingButtonObj);
            if (curClass.indexOf('Bottom') > -1)
                updateBottomShadow(curObj, y, movingButtonObj);
            if (curClass.indexOf('Right') > -1)
                updateRightShadow(curObj, x, movingButtonObj);
        }
    }

    function updateRightShadow(curObj, x, movingButtonObj) {
        $(curObj).find('.siRightShado').css('left', (x + 15) + 'px');
        $(curObj).find('.siRightTop, .siRightBottom').each(function () {
            if ($(this)[0] != movingButtonObj) {
                $(this).css('left', (x) + 'px');
            }
        });
    }

    function updateBottomShadow(curObj, y, movingButtonObj) {
        $(curObj).find('.siBottomShado').css('top', (y + 15) + 'px');
        $(curObj).find('.siRightBottom, .siLeftBottom').each(function () {
            if ($(this)[0] != movingButtonObj) {
                $(this).css('top', (y) + 'px');
            }
        });
    }

    function updateTopShadow(curObj, y, movingButtonObj) {
        $(curObj).find('.siTopShado').css('bottom', 'calc(100% - ' + (y) + 'px)');
        $(curObj).find('.siLeftTop, .siRightTop').each(function () {
            if ($(this)[0] != movingButtonObj) {
                $(this).css('top', (y) + 'px');
            }
        });
    }

    function updateLeftShadow(curObj, x, movingButtonObj) {
        $(curObj).find('.siLeftShado').css('right', 'calc(100% - ' + (x) + 'px)');
        $(curObj).find('.siLeftTop, .siLeftBottom').each(function () {
            if ($(this)[0] != movingButtonObj) {
                $(this).css('left', (x) + 'px');
            }
        });
    }



    return this.each(function () {
        if (imgUrl) {
            var tempImage = new Image();
            tempImage.onload = function () {
                $(this.curThis)[0].imageWith = tempImage.width;
                $(this.curThis)[0].imageHeight = tempImage.height;
                $(this.curThis)[0].tempImage = tempImage;
            }.bind({ curThis: this });
            tempImage.src = imgUrl;
            var html = '<img class="bgImage" src="' + imgUrl + '" />';
            html += '<span class="siLeftTop" ></span><span class="siRightTop" ></span><span class="siLeftBottom" ></span><span class="siRightBottom" ></span>';
            html += '<span class="siLeftShado" ></span><span class="siRightShado" ></span><span class="siTopShado" ></span><span class="siBottomShado" ></span>';
            html += '<div class="mouseMoveDetector" ></div>';
            $(this).html(html);
            initFunctions(this);
        }
    });
};