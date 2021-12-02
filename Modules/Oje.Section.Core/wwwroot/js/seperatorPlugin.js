
function postCommanInNumberPlugin(inputNumber) {
    return inputNumber.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

$.fn.seleperator = function () {
    return this.each(function () {
        $(this).keyup(function () {
            var inputNumber = '';
            try { inputNumber = parseInt($(this).val().replace(/,/g, '')); } catch (e) {};
            if (isNaN(inputNumber))
                inputNumber = '';
            var targetValue = postCommanInNumberPlugin(inputNumber);
            $(this).val(targetValue);
        });
        $(this).focus(function () {
            var inputNumber = '';
            try { inputNumber = parseInt($(this).val().replace(/,/g, '')); } catch (e) { };
            if (isNaN(inputNumber))
                inputNumber = '';
            if (inputNumber > 0)
                $(this).val(postCommanInNumberPlugin(inputNumber));
        });
        $(this).blur(function () {
            $(this).val($(this).val().replace(/,/g, ''));
        });
    });
};

