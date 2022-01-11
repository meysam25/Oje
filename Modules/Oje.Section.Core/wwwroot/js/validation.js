
function getCtrlValidationAttribute(ctrl) {
    var result = '';

    if (ctrl) {
        result += getRequiredValidatorAttributeIfNeeded(ctrl);
        result += getRegExValidatorAttributeIfNeeded(ctrl);
        result += getNationalCodeValidatorAttributeIfNeeded(ctrl);
    }

    return result;
}

function getNationalCodeValidatorAttributeIfNeeded(ctrl) {
    var result = '';

    if (ctrl && ctrl.nationalCodeValidation) {
        var message = '';
        if (ctrl.label)
            message += ' ' + ctrl.label;
        else
            message += ' این کنترول';

        message += ' مجاز نمی باشد';

        result += 'data-validation-nationalcode="true" data-validation-nationalcode-msg="' + message + '"';
    }

    return result;
}

function getRequiredValidatorAttributeIfNeeded(ctrl) {
    var result = '';

    if (ctrl && ctrl.isRequired) {
        var message = 'لطفا';
        if (ctrl.label)
            message += ' ' + ctrl.label;
        else
            message += ' این کنترول';

        if (ctrl.type == 'persianDateTime' || ctrl.type == 'dropDown' || ctrl.type == 'dropDown2' || ctrl.type == 'tokenBox' || ctrl.type == 'tokenBox2' || ctrl.type == 'file')
            message += ' را انتخاب کنید';
        else if (ctrl.type == 'text' || ctrl.type == 'password' || ctrl.type == 'textarea')
            message += ' را وارد کنید';
        else
            message += ' را مشخص نمایید';

        result += 'data-validation-required="true" data-validation-required-msg="' + message + '"';
    }

    return result;
}

function getRegExValidatorAttributeIfNeeded(ctrl) {
    var result = '';

    if (ctrl && ctrl.validations && ctrl.validations.length > 0) {
        result += ' data-validation-reg=\'' + JSON.stringify(ctrl.validations) + '\'';
    }

    return result;
}

function validateForm(selectQuery) {
    var result = true;

    result = checkRequiredValidation(selectQuery, result);
    result = checkRegValidation(selectQuery, result);
    result = checkNationalCodeValidation(selectQuery, result);

    return result;
}

function isNatinalCodeValid(xv) {
    if (isNaN(xv)) {
        return false;
    } else if (xv == "") {
        return false;
    } else if (xv.length < 10) {
        return false;
    } else {
        var yy = 0;
        var yv = parseInt(yv);
        for (let i = 0; i < xv.length; i++) {
            yv = xv[i] * (xv.length - i);
            yy += yv;
        }
        var x = yy % 11;
        if (x === 0) {
            return true;
        } else {
            return false;
        }
        yy = 0;
    }

    return false;
}

function isVisibleCtrl(curThis) {
    return $(curThis).is(':visible') || ($(curThis).closest('.myDropdown').length > 0 && $(curThis).closest('.myDropdown').is(':visible'));
}

function checkNationalCodeValidation(selectQuery, result) {
    if (result == true) {
        $(selectQuery).find('input[data-validation-nationalcode="true"]').each(function () {
            if (isVisibleCtrl(this)) {
                if (!isNatinalCodeValid($(this).val())) {
                    result = false;
                    if (!$(this).closest('.myCtrl').hasClass('inValidInput'))
                        showValidationMessage(this, $(this).attr('data-validation-nationalcode-msg'));
                    return null;
                }
            }
        });
    }
    return result;
}

function checkRequiredValidation(selectQuery, result) {
    if (result == true) {
        $(selectQuery).find('input[data-validation-required="true"], select[data-validation-required="true"]').each(function () {
            if (isVisibleCtrl(this) || $(this).attr('data-select2-id') || $(this).attr('type') == 'file') {
                var curValue = $(this).val();
                if ($(this).parent().hasClass('tokenBox'))
                    curValue = $(this).parent().find('input[type="hidden"]').val();
                if (!curValue) {
                    result = false;
                    if (!$(this).closest('.myCtrl').hasClass('inValidInput'))
                        showValidationMessage(this, $(this).attr('data-validation-required-msg'));
                    return null;
                }
            }
        });
    }
    return result;
}

function checkRegValidation(selectQuery, result) {
    if (result == true) {
        $(selectQuery).find('input[data-validation-reg], select[data-validation-reg]').each(function () {
            if (isVisibleCtrl(this)) {
                var curValue = $(this).val();
                var curRegExpArrValues = JSON.parse($(this).attr('data-validation-reg'));
                if (curValue && curRegExpArrValues && curRegExpArrValues.length > 0) {
                    for (var i = 0; i < curRegExpArrValues.length; i++) {
                        var re = new RegExp(curRegExpArrValues[i].reg);
                        if (!re.test(curValue)) {
                            if (!$(this).closest('.myCtrl').hasClass('inValidInput'))
                                showValidationMessage(this, curRegExpArrValues[i].msg);
                            result = false;
                            return null;
                        }
                    }
                }
            }
        });
    }
    return result;
}

var canUseSetFocus = true;

function showValidationMessage(curObj, message) {
    if (canUseSetFocus == true) {
        $(curObj).focus();
        canUseSetFocus = false;
    }
    $(curObj).closest('.myCtrl').addClass('inValidInput').append('<div class="inValidInputMessage">' + message + '</div>');
    setTimeout(function () { canUseSetFocus = true; $(curObj).closest('.myCtrl').removeClass('inValidInput').find('.inValidInputMessage').remove(); }.bind({ curObj: curObj }), 3000);
}