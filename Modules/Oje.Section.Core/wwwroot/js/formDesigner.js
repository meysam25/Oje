var isEditModeActive = false;
function initDisigner(ctrl) {
    if (ctrl) {
        var quirySelector = $('#' + ctrl.id);
        if (quirySelector.length > 0) {
            var template = generateDesigner(ctrl);
            quirySelector.html(template);

            initDisignerEventAndFunction(ctrl, quirySelector);
        }
    }
}

function generateDesigner(ctrl) {
    var result = '';

    result += `

<div class="formDesigner" >
    <div class="formDesignerPlateTopMenu" >${getTopMenuActions(ctrl)}</div>
    <div class="formDesignerPlateHolders" >
        <div class="formDesignerCtrlHolders" >${getActiveCtrls(ctrl)}</div>
        <div class="formDesignerPlate" >
            <div id="${ctrl.id}_renderPlace" ></div>
        </div>
        <div class="formDesignerCtrlProperties" ></div>
    </div>
</div>

`;

    return result;
}

function getActiveCtrls() {
    var result = '';

    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemTextBox" ><img src="/Modules/Images/textbox.png" alt="تکس باکس" /><span title ="تکس باکس" >تکس باکس</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemTextBoxArea" ><img src="/Modules/Images/textarea.png" alt="تکس اریا" /><span title ="تکس اریا" >تکس اریا</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemRadioBox" ><img src="/Modules/Images/radio.png" alt="ردیو باکس" /><span title ="ردیو باکس" >ردیو باکس</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemCheckBox" ><img src="/Modules/Images/checkbox.png" alt="چک باکس" /><span title ="چک باکس" >چک باکس</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemUploadFile" ><img src="/Modules/Images/fileupload.png" alt="فایل" /><span title ="فایل" >فایل</span></div>';

    return result;
}

function getTopMenuActions() {
    var result = '';

    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuNew fa fa-plus-circle" style="color:red;" title="جدید" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuExport fa fa-download" title="خروجی" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuImport fa fa-upload" title="ورودی" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuUndo fa fa-undo" title="قبلی" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuRedo fa fa-redo" title="بعدی" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuInfo fa fa-info" title="خصوصیات" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuSelectElement fa fa-mouse-pointer" title="انتخاب" ></div>';

    return result;
}

function initDisignerEventAndFunction(ctrl, quirySelector) {
    var curObj = quirySelector[0];
    curObj.ctrl = ctrl;
    initReNewFunction(curObj, quirySelector);
    initSelectElementFunction(curObj, quirySelector);
    initShowCtrlInfoMenu(curObj, quirySelector);
    initGetSelectedCtrlJsonObj(curObj);
    initMouseEvents(ctrl, quirySelector);

    curObj.reNew();

}

function initGetSelectedCtrlJsonObj(curObj) {
    curObj.getSelectedCtrl = function () {
        var foundQuiry = $(this).find('.makeSelectedDP')
        if (foundQuiry.length > 0 && foundQuiry.find('.myCtrl').length > 0) {
            return foundQuiry.find('.myCtrl')[0].ctrl;
        }

        return null;
    }
}

function initShowCtrlInfoMenu(curObj, quirySelector) {
    curObj.showSpecMenue = function () {
        var foundItem = this.getSelectedCtrl();
        if (!foundItem) {
            $.toast({
                heading: 'خطا',
                text: 'لطفا ابتدا یک کنترول را انتخاب کنید',
                textAlign: 'right',
                position: 'bottom-right',
                showHideTransition: 'slide',
                icon: 'error'
            });
            return;
        }
        bindSpecPanelInputs(this, foundItem);
    }

    quirySelector.find('.formDesignerPlateTopMenuInfo').click(function () { $(this).closest('.myFormDesigner')[0].showSpecMenue(); })
}

function initSelectElementFunction(curObj, quirySelector) {
    curObj.togleSelectE = function () {
        var sQuiry = $(this).find('.formDesignerPlateTopMenuSelectElement');
        sQuiry.toggleClass('formDesignerPlateTopMenuSelectElementSelected');
        $(this)[0].isSelectedElement = sQuiry.hasClass('formDesignerPlateTopMenuSelectElementSelected');
        isEditModeActive = sQuiry.hasClass('formDesignerPlateTopMenuSelectElementSelected');
    };
    curObj.offSelecteE = function () {
        var sQuiry = $(this).find('.formDesignerPlateTopMenuSelectElement');
        sQuiry.removeClass('formDesignerPlateTopMenuSelectElementSelected');
        $(this)[0].isSelectedElement = sQuiry.hasClass('formDesignerPlateTopMenuSelectElementSelected');
        isEditModeActive = false;
    };
    quirySelector.find('.formDesignerPlateTopMenuSelectElement').click(function () { $(this).closest('.myFormDesigner')[0].togleSelectE(); })
}

function initReNewFunction(curObj, quirySelector) {
    curObj.reNew = function () {
        var curCtrl = this.ctrl;
        if (curCtrl && curCtrl.baseConfig) {
            $('#' + curCtrl.id + '_renderPlace').html('');
            generateForm(JSON.parse(JSON.stringify(curCtrl.baseConfig)), curCtrl.id + '_renderPlace')
        }
    };
    quirySelector.find('.formDesignerPlateTopMenuNew').click(function () { $(this).closest('.myFormDesigner')[0].reNew(); })
}

var curMouseMove = null;

function initMouseEvents(ctrl, quirySelector) {
    quirySelector.mousedown(function (e) {
        if (e.which != 1)
            return;
        if (e && e.target && $(e.target).closest('.formDesignerCtrlItem').length > 0) {
            createMouseCtrlShadow($(e.target).closest('.formDesignerCtrlItem'), quirySelector, e);
        }
    });
    quirySelector.mouseup(function (e) {
        if (e.which != 1)
            return;
        if (curMouseMove && curMouseMove.length > 0) {
            addNewCtrlToDesigner(curMouseMove, e, this.quirySelector);
            curMouseMove.remove();
            curMouseMove = null;
        }
    }.bind({ quirySelector: quirySelector }));
    quirySelector.mousemove(function (e) {
        if (curMouseMove) {
            curMouseMove.css('left', (e.clientX + 10) + 'px');
            curMouseMove.css('top', (e.clientY - 10) + 'px');
            markeCurrentActiveCtrl(e, this.quirySelector);
        } else if (this.quirySelector[0].isSelectedElement) {
            markeCurrentActiveCtrl(e, this.quirySelector);
        }
    }.bind({ quirySelector: quirySelector }));
}

function removeAllMarked(quirySelector) {
    quirySelector.find('span[class*="makeSelectedDP"]').remove();
    quirySelector.find('.makeSelectedDP').removeClass('makeSelectedDP').unbind();
}

function isSelectable(targetQuiry) {
    if (targetQuiry.hasClass('col-xs-12') && targetQuiry.parent() && targetQuiry.parent().hasClass('row') && targetQuiry.closest('.myPanel').length > 0 && !targetQuiry.hasClass('myPanel'))
        return targetQuiry;
    else if (targetQuiry.closest('.myCtrl').length > 0)
        return targetQuiry.closest('.myCtrl').parent();

    return null;
}

function markeCurrentActiveCtrl(e, quirySelector) {
    removeAllMarked(quirySelector);

    if (e && e.target && $(e.target).closest('.formDesignerPlate').length > 0) {
        var targetQuiry = $(e.target);
        var tempTargetQuiry = isSelectable(targetQuiry);
        if (tempTargetQuiry) {
            targetQuiry = tempTargetQuiry;
            if (!targetQuiry.hasClass('makeSelectedDP')) {
                targetQuiry.addClass('makeSelectedDP');
                targetQuiry.append('<span class="makeSelectedDPLeftBottom"></span>');
                targetQuiry.append('<span class="makeSelectedDPLeftTop"></span>');
                targetQuiry.append('<span class="makeSelectedDPRightBottom"></span>');
                targetQuiry.append('<span class="makeSelectedDPRightTop"></span>');
                targetQuiry.click(function () {
                    $(this).closest('.myFormDesigner')[0].offSelecteE();
                });

            }
        }
    }
}

function createMouseCtrlShadow(curCtrlQuiry, quirySelector, e) {
    var newId = uuidv4RemoveDash();
    var template = '<div id="' + newId + '" class="' + curCtrlQuiry.attr('class') + ' formDesignerCtrlItemMovable">' + curCtrlQuiry.html() + '</div>';

    quirySelector.append(template);
    curMouseMove = $('#' + newId);
    curMouseMove.css('left', (e.clientX + 10) + 'px');
    curMouseMove.css('top', (e.clientY - 10) + 'px');
    e.preventDefault();
}

function addNewCtrlToDesigner(curMouseMove, e, quirySelector) {
    if (e && e.target && $(e.target).closest('.formDesignerPlate').length > 0) {
        var targetQuiry = $(e.target);
        var tempTargetQuiry = isSelectable(targetQuiry);
        if (tempTargetQuiry) {
            targetQuiry = tempTargetQuiry;
            var isRight = false;
            var curWidth = targetQuiry.width() / 2;
            var ofsetX = e.offsetX;
            if (!$(e.target).hasClass('col-xs-12'))
                ofsetX = ofsetX + e.target.offsetLeft;
            if (ofsetX <= curWidth) {
                isRight = true;
            }
            var curCtrlHtml = getCurCtrlTemplate(curMouseMove);
            if (curCtrlHtml) {
                if (isRight == true)
                    targetQuiry.after(curCtrlHtml);
                else
                    targetQuiry.before(curCtrlHtml);

                executeArrFunctions();
                removeAllMarked(quirySelector);
            }

        }
    }
}

function generateRandomName() {
    return 'name_' + new Date().getTime();
}

function getCurCtrlTemplate(curMouseMove) {
    var result = '';

    if (curMouseMove.hasClass('formDesignerCtrlItemTextBox')) {
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getTextBoxTemplate({
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            name: generateRandomName(),
            type: "text",
            label: "عنوان",
            isRequired: true
        });
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemTextBoxArea')) {
        result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">'
        result += getTextAreaTemplate({
            id: uuidv4RemoveDash(),
            parentCL: "col-md-12 col-sm-12 col-xs-12 col-lg-12",
            name: generateRandomName(),
            type: "textarea",
            label: "عنوان",
            isRequired: true
        });
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemUploadFile')) {
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getFileCTRLTemplate({
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            name: generateRandomName(),
            type: "file",
            label: "عنوان",
            isRequired: true,
            acceptEx: '.jpg,.png,.jpeg'
        });
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemCheckBox')) {
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getCheckboxButtonTemplate({
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            name: generateRandomName(),
            type: "checkBox",
            label: "عنوان",
            isRequired: true
        });
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemRadioBox')) {
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getRadioButtonTemplate({
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            name: generateRandomName(),
            type: "radio",
            textfield: "title",
            valuefield: "id",
            label: "عنوان",
            values: [
                {
                    id: "خیر",
                    title: "خیر"
                },
                {
                    id: "بلی",
                    title: "بلی"
                }
            ]
        });
        result += '</div>';
    }

    return result;
}

function bindSpecPanelInputs(curDesigner, foundConfig) {
    var quirySelector = $(curDesigner).find('.formDesignerCtrlProperties');
    var template = '';

    template += '<div class="propertyTitle" ><span class="propertyCloseButton fa fa-times" ></span>خصوصیات</div>';

    template += '<div style="padding-top:15px;" class="row">';

    template += getClassInput(foundConfig);

    switch (foundConfig.type) {
        case 'text':
        case 'textarea':
        case 'number':
            template += getLabelInput(foundConfig);
            template += getNameInput(foundConfig);
            template += getSizeInput(foundConfig);
            template += getChangeDirectionTemplate(foundConfig);
            template += getValidationInput(foundConfig);
            template += getMaskTemplate(foundConfig);
            template += getDisabledInput(foundConfig);
            break;
        case 'dropDown':
            template += getLabelInput(foundConfig);
            template += getNameInput(foundConfig);
            template += getSizeInput(foundConfig);
            template += getSourceUrlTemplate(foundConfig);
            template += getTextAndValueSchemaFields(foundConfig);
            template += getRequiredValidation(foundConfig);
            template += getValuesTemplate(foundConfig);
            break;
        case 'radio':
            template += getLabelInput(foundConfig);
            template += getNameInput(foundConfig);
            template += getSizeInput(foundConfig);
            template += getTextAndValueSchemaFields(foundConfig);
            template += getValuesTemplate(foundConfig);
            template += get
            break;
        default:
    }

    template += getSaveButtonTemplate();

    template += '</div>';

    quirySelector.addClass('formDesignerCtrlPropertiesShow').html(template);

    quirySelector.find('.propertyCloseButton').click(function () { $(this).closest('.formDesignerCtrlPropertiesShow').removeClass('formDesignerCtrlPropertiesShow').html(''); });
    executeArrFunctions();
}

function fillFoundObj(foundConfig, formDataObj) {
    foundConfig.parentCL = formDataObj.size;
    foundConfig.textfield = formDataObj.textfield;
    foundConfig.valuefield = formDataObj.valuefield;
    foundConfig.mask = formDataObj.mask;
    foundConfig.ltr = formDataObj.ltr;
    foundConfig.label = formDataObj.inputLabel;
    foundConfig.name = formDataObj.inputName;
    foundConfig.isRequired = formDataObj.isRequired;
    foundConfig.dataurl = formDataObj.dataurl;
    foundConfig.disabled = formDataObj.disabled;
    foundConfig.maxLengh = formDataObj.maxLength;
    foundConfig.class = formDataObj.class;
    addOrRemoveOnlyValidationOnlyNumber(foundConfig, formDataObj.justNumber);
    addOrRemoveOnlyValidationNumberCount(foundConfig, formDataObj.numberLength);
    addOrRemoveValidationStartWith(foundConfig, formDataObj.startWithN);
    addOrRemoveValidationEmail(foundConfig, formDataObj.email);
    if (foundConfig.type == 'number')
        foundConfig.type = 'text';

    if (formDataObj.codeMeli)
        foundConfig.nationalCodeValidation = true;
    else
        foundConfig.nationalCodeValidation = null;
    if (formDataObj.values) {
        var jsObj = JSON.parse(formDataObj.values);
        if (jsObj && jsObj.length > 0)
            foundConfig.values = jsObj;

    }

    console.log(foundConfig);
}

function saveSelectedController(curThis) {
    var rootCtrl = $(curThis).closest('.myFormDesigner');
    if (rootCtrl.length > 0) {
        var foundConfig = rootCtrl[0].getSelectedCtrl();
        if (foundConfig) {
            var formSelector = $(curThis).closest('.formDesignerCtrlProperties');
            var formData = getFormData(formSelector);
            var formDataObj = {};
            formData.forEach(function (value, key) {
                formDataObj[key] = value;
            });
            fillFoundObj(foundConfig, formDataObj);

            if (rootCtrl.find('.makeSelectedDP').length > 0) {
                rootCtrl.find('.makeSelectedDP').replaceWith(getInputTemplate(foundConfig));
                executeArrFunctions();
                $(curThis).closest('.formDesignerCtrlProperties').find('.propertyCloseButton').click();
            }
        }
    }
}

function addOrRemoveValidationEmail(foundConfig, value) {
    if (!foundConfig.validations)
        foundConfig.validations = [];

    if (value) {
        var foundItem = foundConfig.validations.filter(function (item) { return item.reg == '^\\S+@\\S+\\.\\S+$'; });
        if (!foundItem || foundItem.length == 0)
            foundConfig.validations.push({
                reg: '^\\S+@\\S+\\.\\S+$',
                msg: 'ایمیل وارد شده صحیح نمی باشد'
            });
        else {
            foundItem[0].reg = '^\\S+@\\S+\\.\\S+$';
            foundItem[0].msg = 'ایمیل وارد شده صحیح نمی باشد';
        }
    } else
        foundConfig.validations = foundConfig.validations.filter(function (item) { return item.reg != '^\\S+@\\S+\\.\\S+$'; });
}

function addOrRemoveValidationStartWith(foundConfig, value) {
    if (!foundConfig.validations)
        foundConfig.validations = [];

    if (value) {
        var foundItem = foundConfig.validations.filter(function (item) { return /\^\([\w]+\)/.test(item.reg); });
        if (!foundItem || foundItem.length == 0)
            foundConfig.validations.push({
                reg: '^(' + value + ')',
                msg: foundConfig.label + ' باید با ' + value + ' آغاز شود'
            });
        else {
            foundItem[0].reg = '^(' + value + ')';
            foundItem[0].msg = foundConfig.label + ' باید با ' + value + ' آغاز شود';
        }
    } else
        foundConfig.validations = foundConfig.validations.filter(function (item) { return !/\^\([\w]+\)/.test(item.reg); });
}

function addOrRemoveOnlyValidationNumberCount(foundConfig, value) {
    if (!foundConfig.validations)
        foundConfig.validations = [];

    if (value) {
        var foundItem = foundConfig.validations.filter(function (item) { return item.reg.indexOf('^([0-9]){') == 0; });
        if (!foundItem || foundItem.length == 0)
            foundConfig.validations.push({
                reg: '^([0-9]){' + value + '}$',
                msg: foundConfig.label + 'باید ' + value + ' عدد باشد'
            });
        else {
            foundItem[0].reg = '^([0-9]){' + value + '}$';
            foundItem[0].msg = foundConfig.label + 'باید ' + value + ' عدد باشد';
        }

    } else
        foundConfig.validations = foundConfig.validations.filter(function (item) { return item.reg.indexOf('^([0-9]){') == -1; });
}

function addOrRemoveOnlyValidationOnlyNumber(foundConfig, value) {
    if (!foundConfig.validations)
        foundConfig.validations = [];

    if (value == 'true') {
        var foundItem = foundConfig.validations.filter(function (item) { return item.reg == '^[0-9]*$'; });
        if (!foundItem || foundItem.length == 0)
            foundConfig.validations.push({
                reg: '^[0-9]*$',
                msg: 'لطفا فقط عدد وارد کنید'
            });

    } else
        foundConfig.validations = foundConfig.validations.filter(function (item) { return item.reg != '^[0-9]*$'; });
}

function getTextAndValueSchemaFields(foundConfig) {
    var result = '';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getTextBoxTemplate({
        name: "textfield",
        type: "text",
        label: "عنوان اسکیما",
        dfaultValue: foundConfig.textfield
    });
    result += '</div>';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getTextBoxTemplate({
        name: "valuefield",
        type: "text",
        label: "شناسه اسکیما",
        dfaultValue: foundConfig.valuefield
    });
    result += '</div>';

    return result;
}

function getSaveButtonTemplate() {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getButtonTemplateWidthLabel({
        class: 'btn-primary btn-block',
        type: 'button',
        title: 'ذخیره',
        onClick: 'saveSelectedController(this)'
    });

    result += '</div>';

    return result;
}

function getDisabledInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getCheckboxButtonTemplate({
        id: uuidv4RemoveDash(),
        name: "disabled",
        type: "checkBox",
        label: "غیر فعال؟",
        dfaultValue: foundConfig.disabled
    });

    result += '</div>';

    return result;
}

function getLabelInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getTextBoxTemplate({
        name: "inputLabel",
        type: "text",
        label: "عنوان",
        dfaultValue: foundConfig.label
    });

    result += '</div>';

    return result;
}

function getClassInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getTextBoxTemplate({
        name: "class",
        type: "text",
        label: "کلاس",
        dfaultValue: foundConfig.class
    });

    result += '</div>';

    return result;
}

function getChangeDirectionTemplate(foundConfig) {
    var result = '';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getCheckboxButtonTemplate({
        id: uuidv4RemoveDash(),
        name: "ltr",
        type: "checkBox",
        label: "چپ به راست ؟",
        dfaultValue: foundConfig.ltr
    });
    result += '</div>';

    return result;
}

function getRequiredValidation(foundConfig) {
    var result = '';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getCheckboxButtonTemplate({
        id: uuidv4RemoveDash(),
        name: "isRequired",
        type: "checkBox",
        label: "اجباری ؟",
        dfaultValue: foundConfig.isRequired
    });
    result += '</div>';

    return result;
}

function getValidationInput(foundConfig) {
    var result = '';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getCheckboxButtonTemplate({
        id: uuidv4RemoveDash(),
        name: "codeMeli",
        type: "checkBox",
        label: "کد ملی؟",
        dfaultValue: foundConfig.nationalCodeValidation
    });
    result += '</div>';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getCheckboxButtonTemplate({
        id: uuidv4RemoveDash(),
        name: "email",
        type: "checkBox",
        label: "ایمیل؟",
        dfaultValue: hasEmailValidation(foundConfig)
    });
    result += '</div>';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getCheckboxButtonTemplate({
        id: uuidv4RemoveDash(),
        name: "isRequired",
        type: "checkBox",
        label: "اجباری ؟",
        dfaultValue: foundConfig.isRequired
    });
    result += '</div>';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getCheckboxButtonTemplate({
        id: uuidv4RemoveDash(),
        name: "justNumber",
        type: "checkBox",
        label: "فقط عدد؟",
        dfaultValue: hasJustNumberValidation(foundConfig)
    });
    result += '</div>';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getTextBoxTemplate({
        name: "numberLength",
        type: "number",
        label: "تعداد اعداد اجباری",
        dfaultValue: getNumberLengthValidationNumber(foundConfig)
    });
    result += '</div>';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getTextBoxTemplate({
        name: "maxLength",
        type: "text",
        label: "حداکثر کارکتر مجاز",
        dfaultValue: foundConfig.maxLengh
    });
    result += '</div>';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getTextBoxTemplate({
        name: "startWithN",
        type: "text",
        label: "شروع شود با",
        dfaultValue: getStartWithValidationNumber(foundConfig)
    });
    result += '</div>';

    return result;
}

function hasEmailValidation(foundConfig) {
    if (foundConfig && foundConfig.validations)
        return foundConfig.validations.filter(function (item) { return item.reg == '^\\S+@\\S+\\.\\S+$' }).length > 0;

    return false;
}

function getNameInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getTextBoxTemplate({
        name: "inputName",
        type: "text",
        label: "نام",
        dfaultValue: foundConfig.name
    });

    result += '</div>';

    return result;
}

function getValuesTemplate(foundConfig) {
    var result = '';

    var values = foundConfig.values;
    if (!values)
        values = [];

    result += '<div data-options=\'' + JSON.stringify(values) + '\' class="col-md-12 col-sm-12 col-xs-12 col-lg-12">'
    result += getButtonTemplateWidthLabel({
        id: uuidv4RemoveDash(),
        class: 'btn-success btn-block',
        type: 'button',
        title: 'مقادیر',
        onClick: 'showValuesGridModal(this)'
    });
    result += '</div>';

    return result;
}

function addTIdToArr(arr) {
    if (arr)
        for (var i = 0; i < arr.length; i++) {
            arr[i].tId = uuidv4RemoveDash();
            arr[i].order = i + 1;
        }
}

function showValuesGridModal(curObj) {
    var curQuiry = $(curObj);
    if (curQuiry.closest('[data-options]').length > 0) {
        var optionValues = JSON.parse(curQuiry.closest('[data-options]').attr('data-options'));
        var modalNewId = 'modal_' + uuidv4RemoveDash();
        var gridId = 'grid_' + uuidv4RemoveDash();
        var addGridModalId = 'modal_' + uuidv4RemoveDash();
        var editGridModalId = 'modal_' + uuidv4RemoveDash();
        addTIdToArr(optionValues);
        $('body').append(getModualTemplate(
            {
                id: modalNewId,
                class: 'modal-lg',
                title: 'مقادیر',
                modelBody: getGridTemplate({
                    id: gridId,
                    topActions: [
                        {
                            title: "افزودن",
                            onClick: "showModal('" + addGridModalId + "', this)"
                        }
                    ],
                    isClient: true,
                    itemPerPage: 100,
                    schema: {
                        data: 'data',
                        total: 'total'
                    },
                    showColumnConfigButton: true,
                    key: 'tId',
                    actions: {
                        cActions: [
                            {
                                template: "function bindRow (curRow) { return '<span onclick=\"showEditModal(\\''+ curRow.tId +'\\', \\'\\', \\'" + editGridModalId + "\\' , this, null, null, null, null, true)\" style=\"display:block;\" ><i style=\"color:white!important;\" class=\"fa fa-pen\" ></i></span>' };bindRow(data)"
                            },
                            {
                                template: "function bindRow (curRow) { return '<span onclick=\"deleteThisRowMGridD(\\''+ curRow.tId +'\\',this)\" style=\"display:block;\" ><i style=\"color:white!important;\" class=\"fa fa-trash\" ></i></span>' };bindRow(data)"
                            }
                        ]
                    },
                    columns: [
                        { field: 'order', caption: 'ترتیب', search: { searchType: 'text' }, sort: true },
                        { field: 'id', caption: 'شناسه', search: { searchType: 'text' }, sort: true },
                        { field: 'title', caption: 'عنوان', search: { searchType: 'text' }, sort: true }
                    ],
                    ds: { data: optionValues, total: optionValues.length }
                }),
                actions: [
                    {
                        title: "بستن",
                        onClick: "closeThisModal(this)",
                        class: "btn-secondary"
                    },
                    {
                        title: "ذخیره",
                        onClick: "updateOptions('" + gridId + "', '" + $(curObj).attr('id') + "', this)",
                        class: "btn-primary"
                    }
                ]
            }
        ));
        $('body').append(getModualTemplate(
            {
                id: addGridModalId,
                title: 'افزودن',
                ctrls: [
                    {
                        parentCL: 'col-md-12 col-sm-12 col-xs-12 col-lg-12',
                        name: "order",
                        type: "number",
                        label: "ترتیب نمایش"
                    },
                    {
                        parentCL: 'col-md-12 col-sm-12 col-xs-12 col-lg-12',
                        name: "id",
                        type: "text",
                        label: "شناسه"
                    },
                    {
                        parentCL: 'col-md-12 col-sm-12 col-xs-12 col-lg-12',
                        name: "title",
                        type: "text",
                        label: "عنوان"
                    }
                ],
                actions: [
                    {
                        title: "بستن",
                        onClick: "closeThisModal(this)",
                        class: "btn-secondary"
                    },
                    {
                        title: "ذخیره",
                        onClick: "addNewValueToGridForValues(this, '" + gridId + "')",
                        class: "btn-primary"
                    }
                ]
            }
        ));
        $('body').append(getModualTemplate(
            {
                id: editGridModalId,
                title: 'ویرایش',
                ctrls: [
                    {
                        name: "tId",
                        type: "hidden"
                    },
                    {
                        parentCL: 'col-md-12 col-sm-12 col-xs-12 col-lg-12',
                        name: "order",
                        type: "number",
                        label: "ترتیب نمایش"
                    },
                    {
                        parentCL: 'col-md-12 col-sm-12 col-xs-12 col-lg-12',
                        name: "id",
                        type: "text",
                        label: "شناسه"
                    },
                    {
                        parentCL: 'col-md-12 col-sm-12 col-xs-12 col-lg-12',
                        name: "title",
                        type: "text",
                        label: "عنوان"
                    }
                ],
                actions: [
                    {
                        title: "بستن",
                        onClick: "closeThisModal(this)",
                        class: "btn-secondary"
                    },
                    {
                        title: "ذخیره",
                        onClick: "updateValueToGridForValues(this, '" + gridId + "')",
                        class: "btn-primary"
                    }
                ]
            }
        ));
        executeArrFunctions();
        $('#' + modalNewId).modal('show');
    }
}

function updateOptions(gridId, buttonId, curButton) {
    if (gridId, buttonId, curButton) {
        var gridQuiry = $('#' + gridId);
        if (gridQuiry.length > 0) {
            var option = gridQuiry[0].option;
            if (option) {
                var sortedData = gridQuiry[0].setFiltersAndSorts(option.ds, option, new FormData());
                console.log(sortedData);
                $('#' + buttonId).closest('[data-options]').attr('data-options', JSON.stringify(sortedData.data));
            }
            closeThisModal(curButton);
        }
    }
}

function deleteThisRowMGridD(curKey, curButton) {
    if (curButton && curKey) {
        var gridQuiry = $(curButton).closest('.myGridCTRL');
        if (gridQuiry.length > 0) {
            var option = gridQuiry[0].option;
            if (option) {
                var foundDs = option.ds.data.filter(function (item) { return item.tId != curKey; });
                option.ds.data = foundDs;
                option.ds.total = foundDs.length;
            }
            gridQuiry[0].refreshData();
        }
    }
}

function updateValueToGridForValues(curButton, gridId) {
    if (curButton && gridId) {
        var gridQuiry = $('#' + gridId);
        if (gridQuiry.length > 0) {
            closeThisModal(curButton);
            var formData = getFormData($(curButton).closest('.modal-content'));
            var option = gridQuiry[0].option;
            if (option) {
                var model = {};
                formData.forEach(function (value, key) { model[key] = value; });
                var foundDs = option.ds.data.filter(function (item) { return item.tId == model.tId; });
                for (var item in model) {
                    foundDs[0][item] = model[item];
                }
            }
            gridQuiry[0].refreshData();
        }
    }
}

function addNewValueToGridForValues(curButton, gridId) {
    if (curButton && gridId) {
        var gridQuiry = $('#' + gridId);
        if (gridQuiry.length > 0) {
            closeThisModal(curButton);
            var formData = getFormData($(curButton).closest('.modal-content'));
            var option = gridQuiry[0].option;
            if (option) {
                var model = {}
                formData.forEach(function (value, key) { model[key] = value; })
                option.ds.data.push({ id: model.id, title: model.title, order: model.order, tId: uuidv4RemoveDash() });
                option.ds.total = option.ds.total + 1;
            }
            gridQuiry[0].refreshData();
        }
    }
}


function getSourceUrlTemplate(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getTextBoxTemplate({
        name: "dataurl",
        type: "text",
        label: "لینک منبع",
        dfaultValue: foundConfig.dataurl,
        ltr: true
    });

    result += '</div>';

    return result;
}

function getMaskTemplate(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getTextBoxTemplate({
        name: "mask",
        type: "text",
        label: "ماسک",
        dfaultValue: foundConfig.mask
    });

    result += '</div>';

    return result;
}

function getSizeInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getDropdownCTRLTemplate({
        name: "size",
        type: "dropDown",
        label: "اندازه",
        dfaultValue: foundConfig.parentCL,
        textfield: 'title',
        valuefield: 'id',
        values: [
            {
                id: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
                title: "یک سوم سطر(یک چهارم سطر)"
            },
            {
                id: "col-md-4 col-sm-6 col-xs-12 col-lg-4",
                title: "یک سوم سطر"
            },
            {
                id: "col-md-3 col-sm-3 col-xs-12 col-lg-3",
                title: "یک چهارم سطر"
            },
            {
                id: "col-md-9 col-sm-9 col-xs-12 col-lg-9",
                title: "سه چهارم سطر"
            },
            {
                id: "col-md-6 col-sm-6 col-xs-12 col-lg-6",
                title: "نصف سطر"
            },
            {
                id: "col-md-12 col-sm-12 col-xs-12 col-lg-12",
                title: "کل سطر"
            }
        ]
    });

    result += '</div>';

    return result;
}

function hasJustNumberValidation(foundConfig) {
    var result = false;

    if (foundConfig && foundConfig.validations)
        for (var i = 0; i < foundConfig.validations.length; i++)
            if (foundConfig.validations[i].reg == '^[0-9]*$') {
                result = true;
                break;
            }

    return result;
}

function getStartWithValidationNumber(foundConfig) {
    var result = '';
    for (var i = 0; foundConfig.validations && i < foundConfig.validations.length; i++)
        if (/\^\([\w]+\)/.test(foundConfig.validations[i].reg)) {
            result = foundConfig.validations[i].reg.split('^(')[1].split(')')[0];
            break;
        }

    return result;
}

function getNumberLengthValidationNumber(foundConfig) {
    var result = '';

    for (var i = 0; foundConfig.validations && i < foundConfig.validations.length; i++)
        if (foundConfig.validations[i].reg.indexOf('^([0-9]){') > -1 && foundConfig.validations[i].reg.indexOf('}$') > -1) {
            result = foundConfig.validations[i].reg.split('^([0-9]){')[1].split('}$')[0];
            break;
        }

    return result;
}