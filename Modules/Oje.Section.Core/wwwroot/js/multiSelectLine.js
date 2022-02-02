
$.fn.initMultiSelectLine = function (onChange) {
    return this.each(function () {
        $(this)[0].unselectAll = function () {
            var curThisQ = $(this);
            curThisQ.find('.multiSelectLineBodyItem').each(function () {
                curThisQ[0].unSelect(this);
            });
        };
        $(this)[0].selectAll = function () {
            var curThisQ = $(this);
            curThisQ.find('.multiSelectLineBodyItem').each(function () {
                curThisQ[0].select(this);
            });
        };
        $(this)[0].select = function (curElement, inputName, currValue) {
            var sQuery = $(curElement);
            if (!sQuery.hasClass('multiSelectLineBodyItemActive')) {
                sQuery.addClass('multiSelectLineBodyItemActive');
                sQuery.append('<input type="hidden" name="' + inputName + '" value="' + currValue +'" />');
            }
        };
        $(this)[0].unSelect = function (curElement) {
            var sQuery = $(curElement);
            if (sQuery.hasClass('multiSelectLineBodyItemActive')) {
                sQuery.removeClass('multiSelectLineBodyItemActive');
                sQuery.find('input[type=hidden]').remove();
            }
        };

        $(this).find('.multiSelectLineBodyItem').click(function () {
            var parentQ = $(this).closest('.multiSelectLine');
            var curThisQ = $(this);
            if (curThisQ.hasClass('multiSelectLineBodyItemActive')) {
                parentQ[0].unSelect(this);
                if (onChange)
                    eval(onChange);
            } else {
                parentQ[0].select(this, parentQ.attr('data-name'), $(this).attr('data-id'));
                if (onChange)
                    eval(onChange);
            }
        });
    });
}