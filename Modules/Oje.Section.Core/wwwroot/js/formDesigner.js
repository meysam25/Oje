var isEditModeActive = false;
var isMoveModeActive = false;
var curMouseMove = null;
var curMoveCtrl = null;
var curMoveCtrlDocument = null;

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

    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemButtonMoveNextStep" ><img src="/Modules/Images/button.png" alt="مرحله بعد" /><span title ="مرحله بعد" >مرحله بعد</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemLabel" ><img src="/Modules/Images/label.png" alt="متن" /><span title ="متن" >متن</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemDropdown" ><img src="/Modules/Images/dropdown.png" alt="دراپ دان" /><span title ="دراپ دان" >دراپ دان</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemDropdownCompany" ><img src="/Modules/Images/dropdown.png" alt="شرکت بیمه" /><span title ="شرکت بیمه" >شرکت بیمه</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemTokenBox" ><img src="/Modules/Images/multiselect.png" alt="چند انتخابی" /><span title ="چند انتخابی" >چند انتخابی</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemTextBox" ><img src="/Modules/Images/textbox.png" alt="تکس باکس" /><span title ="تکس باکس" >تکس باکس</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemTextBoxArea" ><img src="/Modules/Images/textarea.png" alt="تکس اریا" /><span title ="تکس اریا" >تکس اریا</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemPersianDate" ><img src="/Modules/Images/datetime.png" alt="تقویم فارسی" /><span title ="تقویم فارسی" >تقویم فارسی</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemPassword" ><img src="/Modules/Images/password.png" alt="کلمه عبور" /><span title ="کلمه عبور" >کلمه عبور</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemColor" ><img src="/Modules/Images/color.png" alt="رنگ" /><span title ="رنگ" >رنگ</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemRadioBox" ><img src="/Modules/Images/radio.png" alt="ردیو باکس" /><span title ="ردیو باکس" >ردیو باکس</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemCheckBox" ><img src="/Modules/Images/checkbox.png" alt="چک باکس" /><span title ="چک باکس" >چک باکس</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemUploadFile" ><img src="/Modules/Images/fileupload.png" alt="فایل" /><span title ="فایل" >فایل</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemCarPlaque" ><img src="/Modules/Images/carPlaque.png" alt="پلاک خودرو" /><span title ="پلاک خودرو" >پلاک خودرو</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemEmpty" ><img src="/Modules/Images/empty.png" alt="سطر خالی" /><span title ="سطر خالی" >سطر خالی</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemMultiRow" ><img src="/Modules/Images/groupCtrl.png" alt="چند سطر" /><span title ="چند سطر" >چند سطر</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemMap" ><img src="/Modules/Images/map.png" alt="نقشه" /><span title ="نقشه" >نقشه</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemHtml" ><img src="/Modules/Images/html.png" alt="html" /><span title ="html" >html</span></div>';
    //result += '<div class="formDesignerCtrlItem formDesignerCtrlItemHtmlLaw" ><img src="/Modules/Images/html.png" alt="قوانین حاکم" /><span title ="قوانین حاکم" >قوانین حاکم</span></div>';

    return result;
}

function getTopMenuActions() {
    var result = '';

    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuNew fa fa-plus-circle" style="color:red;" title="جدید" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuImport fa fa-download" title="ورودی" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuExport fa fa-upload" title="خروجی" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuUndo fa fa-undo" title="قبلی" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuRedo fa fa-redo" title="بعدی" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuInfo fa fa-info" title="خصوصیات" ></div>';
    result += '<div style="color:red;" class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuDelete fa fa-trash" title="حذف" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuSelectElement fa fa-mouse-pointer" title="انتخاب" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuMoveElement fa fa-expand-arrows" title="جا به جایی" ></div>';

    return result;
}

function initDisignerEventAndFunction(ctrl, quirySelector) {
    var curObj = quirySelector[0];
    curObj.ctrl = ctrl;
    initReNewFunction(curObj, quirySelector);
    initSelectElementFunction(curObj, quirySelector);
    initShowCtrlInfoMenu(curObj, quirySelector);
    initDeleteButtonMenu(curObj, quirySelector);
    initMoveButtonMenu(curObj, quirySelector);
    initGetSelectedCtrlJsonObj(curObj);
    initExportButtonMenu(curObj, quirySelector);
    initImportButtonMenu(curObj, quirySelector);
    initUndoRedoButtonMenu(curObj, quirySelector);
    initMouseEvents(ctrl, quirySelector);

    curObj.reNew();
    

}

function initGetSelectedCtrlJsonObj(curObj) {
    curObj.getSelectedCtrl = function () {
        var foundQuiry = $(this).find('.makeSelectedDP');
        if (foundQuiry.length > 0 && foundQuiry.hasClass('myPanel'))
            return foundQuiry[0].panel;
        else if (foundQuiry.length > 0 && foundQuiry.hasClass('panelSWizard'))
            return foundQuiry[0].wizard;
        else if (foundQuiry.length > 0 && foundQuiry.find('.myCtrl').length > 0)
            return foundQuiry.find('.myCtrl')[0].ctrl;

        return null;
    }
}

function initDeleteButtonMenu(curObj, quirySelector) {
    curObj.deleteSelectedItem = function () {
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
        if ($('.makeSelectedDP').find('>.myCtrl').length > 0) {
            if ($('.makeSelectedDP').closest('.MultiRowInputRow').length > 0) {
                updateMultiRowInputs($('.makeSelectedDP').closest('.MultiRowInputRow'));
            }
            $('.makeSelectedDP').remove();
            $(this).removeClass('hideToolbar');
            this.addToUR();
        }
        else
            $.toast({
                heading: 'خطا',
                text: 'امکان حذف وجود ندارد',
                textAlign: 'right',
                position: 'bottom-right',
                showHideTransition: 'slide',
                icon: 'error'
            });
    }

    quirySelector.find('.formDesignerPlateTopMenuDelete').click(function () { $(this).closest('.myFormDesigner')[0].deleteSelectedItem(); })
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

function initMoveButtonMenu(curObj, quirySelector) {
    curObj.togleMoveE = function () {
        var sQuiry = $(this).find('.formDesignerPlateTopMenuMoveElement');
        sQuiry.toggleClass('formDesignerPlateTopMenuMoveElementSelected');
        isMoveModeActive = sQuiry.hasClass('formDesignerPlateTopMenuMoveElementSelected');
        $(this)[0].isMoveModeActive = isMoveModeActive;
        if (isMoveModeActive == true) {
            $(this)[0].offSelecteE();
            $(this).addClass('hideToolbar');
        } else {
            removeAllMarked(quirySelector);
            $(this).removeClass('hideToolbar');
        }
    };
    curObj.offMoveE = function () {
        var sQuiry = $(this).find('.formDesignerPlateTopMenuMoveElement');
        sQuiry.removeClass('formDesignerPlateTopMenuMoveElementSelected');
        $(this)[0].isMoveModeActive = false;
        isMoveModeActive = false;
    };
    quirySelector.find('.formDesignerPlateTopMenuMoveElement').click(function () { $(this).closest('.myFormDesigner')[0].togleMoveE(); })
}

function initSelectElementFunction(curObj, quirySelector) {
    curObj.togleSelectE = function () {
        var sQuiry = $(this).find('.formDesignerPlateTopMenuSelectElement');
        sQuiry.toggleClass('formDesignerPlateTopMenuSelectElementSelected');
        $(this)[0].isSelectedElement = sQuiry.hasClass('formDesignerPlateTopMenuSelectElementSelected');
        isEditModeActive = sQuiry.hasClass('formDesignerPlateTopMenuSelectElementSelected');
        if (isEditModeActive == true) {
            $(this)[0].offMoveE();
            $(this).addClass('hideToolbar');
        }
        else {
            removeAllMarked(quirySelector);
            $(this).removeClass('hideToolbar');
        }
    };
    curObj.offSelecteE = function () {
        var sQuiry = $(this).find('.formDesignerPlateTopMenuSelectElement');
        sQuiry.removeClass('formDesignerPlateTopMenuSelectElementSelected');
        $(this)[0].isSelectedElement = false;
        isEditModeActive = false;
    };
    quirySelector.find('.formDesignerPlateTopMenuSelectElement').click(function () { $(this).closest('.myFormDesigner')[0].togleSelectE(); })
}

function initReNewFunction(curObj, quirySelector) {
    curObj.reNew = function () {
        var curCtrl = this.ctrl;
        if (curCtrl && curCtrl.baseConfig) {
            $('#' + curCtrl.id + '_renderPlace').html('');
            generateForm(JSON.parse(JSON.stringify(curCtrl.baseConfig)), curCtrl.id + '_renderPlace');
        }
        this.newUndoRedo(); this.addToUR();
    };

    quirySelector.find('.formDesignerPlateTopMenuNew').click(function () { $(this).closest('.myFormDesigner')[0].reNew();  })
}

function initImportButtonMenu(curObj, quirySelector) {
    curObj.import = function () {
        showImportModal(this);
    };
    quirySelector.find('.formDesignerPlateTopMenuImport').click(function () { $(this).closest('.myFormDesigner')[0].import(); })
}

function initUndoRedoButtonMenu(curObj, quirySelector) {
    curObj.undo = function () {
        var arrUndoJson = this.arrUndoJson;
        var arrUndoJsonIndex = this.arrUndoJsonIndex;

        if (!arrUndoJsonIndex)
            arrUndoJsonIndex = 0;

        arrUndoJsonIndex--;
        if (arrUndoJson && arrUndoJson.length > 0 && arrUndoJsonIndex > 0) {
            this.arrUndoJsonIndex = arrUndoJsonIndex;
            $('#' + $(this).attr('id') + '_renderPlace').html('');
            generateForm(JSON.parse(JSON.stringify(arrUndoJson[arrUndoJsonIndex - 1])), $(this).attr('id') + '_renderPlace');
        }

    };
    curObj.redo = function () {
        var arrUndoJson = this.arrUndoJson;
        var arrUndoJsonIndex = this.arrUndoJsonIndex;

        if (!arrUndoJsonIndex)
            arrUndoJsonIndex = 0;

        arrUndoJsonIndex++;
        if (arrUndoJson && arrUndoJson.length > 0 && arrUndoJsonIndex > 0 && arrUndoJsonIndex <= arrUndoJson.length) {
            this.arrUndoJsonIndex = arrUndoJsonIndex;
            $('#' + $(this).attr('id') + '_renderPlace').html('');
            generateForm(JSON.parse(JSON.stringify(arrUndoJson[arrUndoJsonIndex - 1])), $(this).attr('id') + '_renderPlace');
        }
    };
    curObj.addToUR = function () {
        var jsObj = getWoleJsonObj(this);
        var arrUndoJson = this.arrUndoJson;
        var arrUndoJsonIndex = this.arrUndoJsonIndex;
        if (!arrUndoJsonIndex)
            arrUndoJsonIndex = 0;
        if (!arrUndoJson)
            arrUndoJson = [];

        if (arrUndoJsonIndex != arrUndoJson.length) {
            arrUndoJson.splice(arrUndoJsonIndex, arrUndoJson.length);
        }
        arrUndoJson.push({ panels: [JSON.parse(JSON.stringify(jsObj))] });
        arrUndoJsonIndex = arrUndoJson.length;

        this.arrUndoJson = arrUndoJson;
        this.arrUndoJsonIndex = arrUndoJsonIndex;
    };
    curObj.newUndoRedo = function () {
        this.arrUndoJson = [];
        this.arrUndoJsonIndex = 0;
    }
    quirySelector.find('.formDesignerPlateTopMenuRedo').click(function () { $(this).closest('.myFormDesigner')[0].redo(); })
    quirySelector.find('.formDesignerPlateTopMenuUndo').click(function () { $(this).closest('.myFormDesigner')[0].undo(); })
}

function initExportButtonMenu(curObj, quirySelector) {
    curObj.export = function () {
        var jsonObj = getWoleJsonObj(this);
        if (!jsonObj) {
            $.toast({
                heading: 'خطا',
                text: 'خطا در ساخت خروجی',
                textAlign: 'right',
                position: 'bottom-right',
                showHideTransition: 'slide',
                icon: 'error'
            });
            return;
        }
        copyTextToClipboard(JSON.stringify({ panels: [jsonObj]}));
        $.toast({
            heading: 'موفقیت',
            text: 'عملیات با موفقیت انجام گرفت',
            textAlign: 'right',
            position: 'bottom-right',
            showHideTransition: 'slide',
            icon: 'success'
        });
    };
    quirySelector.find('.formDesignerPlateTopMenuExport').click(function () { $(this).closest('.myFormDesigner')[0].export(); })
}

function getWoleJsonObj(curObj) {
    if (curObj) {
        var rootPanel = $(curObj).find('.formDesignerPlate').find('.myPanel');
        var foundPanel = rootPanel[0].panel;
        if (rootPanel.length > 0 && foundPanel) {
            fillFoundObj(foundPanel, foundPanel, $(curObj).find('.formDesignerPlate'));
            return foundPanel;
        }
    }
    return null;
}

function initMouseEvents(ctrl, quirySelector) {
    quirySelector.mousedown(function (e) {
        if (e.which != 1)
            return;
        if (e && e.target && $(e.target).closest('.formDesignerCtrlItem').length > 0) {
            createMouseCtrlShadow($(e.target).closest('.formDesignerCtrlItem'), quirySelector, e);
        } else if (isMoveModeActive && e && e.target && $(e.target).closest('.formDesignerPlate').length > 0) {
            createMouseCtrlShadowForMoveCtrl($(e.target), quirySelector, e);
        }
    });
    quirySelector.mouseup(function (e) {
        if (e.which != 1)
            return;
        if (curMouseMove && curMouseMove.length > 0) {
            addNewCtrlToDesigner(curMouseMove, e, this.quirySelector);
            curMouseMove.remove();
            curMouseMove = null;
        } else if (curMoveCtrlDocument != null && curMoveCtrlDocument.length > 0) {
            moveCtrlToDesigner(e, this.quirySelector);
        }
    }.bind({ quirySelector: quirySelector }));
    quirySelector.mousemove(function (e) {
        if (curMouseMove) {
            curMouseMove.css('left', (e.clientX + 10) + 'px');
            curMouseMove.css('top', (e.clientY - 10) + 'px');
            markeCurrentActiveCtrl(e, this.quirySelector);
        } else if (curMoveCtrlDocument) {
            curMoveCtrlDocument.css('left', (e.clientX + 20) + 'px');
            curMoveCtrlDocument.css('top', (e.clientY - 10) + 'px');
            markeCurrentActiveCtrl(e, this.quirySelector, false, isMoveModeActive);
        } else if (this.quirySelector[0].isSelectedElement) {
            markeCurrentActiveCtrl(e, this.quirySelector, this.quirySelector[0].isSelectedElement);
        } else if (whatToDoAfterSecoundSelect != null) {
            markeCurrentActiveCtrl2(e, quirySelector);
        } else if (isMoveModeActive) {
            markeCurrentActiveCtrl(e, this.quirySelector, false, isMoveModeActive);
        }
    }.bind({ quirySelector: quirySelector }));
}



function isSelectable(targetQuiry, isSelectedElement, isMoveModeActive) {
    if (isMoveModeActive) {
        if (targetQuiry.closest('.myCtrl').length > 0 && targetQuiry.closest('.myCtrl').parent().closest('.plaqueCtrl').length == 0)
            return targetQuiry.closest('.myCtrl').parent();
    }
    else {
        if (targetQuiry.hasClass('col-xs-12') && targetQuiry.parent() && targetQuiry.parent().hasClass('row') && targetQuiry.closest('.myPanel').length > 0 && !targetQuiry.hasClass('myPanel'))
            return targetQuiry;
        else if (targetQuiry.closest('.myCtrl').length > 0)
            return targetQuiry.closest('.myCtrl').parent();
        else if (isSelectedElement && (targetQuiry.hasClass('panelSWizard') || targetQuiry.closest('.panelSWizard').length > 0))
            return targetQuiry.hasClass('panelSWizard') ? targetQuiry : targetQuiry.closest('.panelSWizard');
        else if (isSelectedElement && ((targetQuiry.hasClass('myPanel')) && targetQuiry.find('>.panelSWizard').length > 0) || ((targetQuiry.parent().hasClass('myPanel')) && targetQuiry.parent().find('>.panelSWizard').length > 0))
            return targetQuiry.hasClass('myPanel') ? targetQuiry : targetQuiry.parent();
    }

    return null;
}

function removeAllMarked2(quirySelector) {
    quirySelector.find('span[class*="makeSelectedFDP"]').remove();
    quirySelector.find('.makeSelectedFDP').removeClass('makeSelectedFDP').unbind();
}

function markeCurrentActiveCtrl2(e, quirySelector) {
    removeAllMarked2(quirySelector);

    if (e && e.target && $(e.target).closest('.formDesignerPlate').length > 0) {
        var targetQuiry = $(e.target);
        var tempTargetQuiry = isSelectable(targetQuiry);
        if (tempTargetQuiry) {
            targetQuiry = tempTargetQuiry;
            if (!targetQuiry.hasClass('makeSelectedFDP')) {
                targetQuiry.addClass('makeSelectedFDP');
                targetQuiry.append('<span class="makeSelectedFDPLeftBottom"></span>');
                targetQuiry.append('<span class="makeSelectedFDPLeftTop"></span>');
                targetQuiry.append('<span class="makeSelectedFDPRightBottom"></span>');
                targetQuiry.append('<span class="makeSelectedFDPRightTop"></span>');
                targetQuiry.click(function () {
                    whatToDoAfterSecoundSelect(targetQuiry);
                    whatToDoAfterSecoundSelect = null;
                    removeAllMarked2(quirySelector);
                });
            }
        }
    }
}

function removeAllMarked(quirySelector) {
    quirySelector.find('span[class*="makeSelectedDP"]').remove();
    quirySelector.find('.makeSelectedDP').removeClass('makeSelectedDP').unbind();
}

function markeCurrentActiveCtrl(e, quirySelector, isSelectedElement, isMoveModeActive) {
    removeAllMarked(quirySelector);

    if (e && e.target && $(e.target).closest('.formDesignerPlate').length > 0) {
        var targetQuiry = $(e.target);
        var tempTargetQuiry = isSelectable(targetQuiry, isSelectedElement, isMoveModeActive);
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

function createMouseCtrlShadowForMoveCtrl(curCtrlQuiry, quirySelector, e) {
    var ctrlQuiry = curCtrlQuiry.closest('.myCtrl');
    if (ctrlQuiry.length > 0) {
        var curWidth = ctrlQuiry.width();
        curMoveCtrl = ctrlQuiry[0].ctrl;
        if (curMoveCtrl) {
            var holderCtrlQuiry = ctrlQuiry.closest('.col-xs-12');
            var newId = uuidv4RemoveDash();
            var template = '<div style="width:' + curWidth + 'px;" id="' + newId + '" class="formDesignerCtrlItemMovable lowOpacityForMovingObj">' + holderCtrlQuiry.html() + '</div>';

            quirySelector.append(template);
            curMoveCtrlDocument = $('#' + newId);
            curMoveCtrlDocument.css('left', (e.clientX + 10) + 'px');
            curMoveCtrlDocument.css('top', (e.clientY - 10) + 'px');
        }

        e.preventDefault();
    }

}

function moveCtrlToDesigner(e, quirySelector) {
    if (e && e.target && $(e.target).closest('.formDesignerPlate').length > 0) {
        var targetQuiry = $(e.target);
        var tempTargetQuiry = isSelectable(targetQuiry);
        if (tempTargetQuiry && tempTargetQuiry.length > 0) {
            if (tempTargetQuiry.find('.myCtrl')[0].ctrl == curMoveCtrl) {
                curMoveCtrlDocument.remove();
                curMoveCtrlDocument = null;
                return;
            }
            targetQuiry = tempTargetQuiry;
            var isRight = false;
            var curWidth = targetQuiry.width() / 2;
            var ofsetX = e.offsetX;
            if (!$(e.target).hasClass('col-xs-12'))
                ofsetX = ofsetX + e.target.offsetLeft;
            if (ofsetX <= curWidth) {
                isRight = true;
            }
            var curCtrlHtml = getInputTemplate(curMoveCtrl);
            curMoveCtrlDocument.remove();
            curMoveCtrlDocument = null;
            if (curCtrlHtml) {
                $('#' + curMoveCtrl.id).closest('.col-xs-12').remove();
                if (isRight == true)
                    targetQuiry.after(curCtrlHtml);
                else
                    targetQuiry.before(curCtrlHtml);
                executeArrFunctions();
                removeAllMarked(quirySelector);
                quirySelector[0].addToUR();
            }
        }
    }
    if (curMoveCtrlDocument) {
        curMoveCtrlDocument.remove();
        curMoveCtrlDocument = null;
    }
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
            var curCtrlHtml = getCurCtrlTemplate(curMouseMove, targetQuiry);
            if (curCtrlHtml) {
                if (isRight == true)
                    targetQuiry.after(curCtrlHtml);
                else
                    targetQuiry.before(curCtrlHtml);

                executeArrFunctions();
                removeAllMarked(quirySelector);
                setTimeout(function () { this.quirySelector[0].addToUR(); }.bind({ quirySelector: quirySelector }), 300);

            }
        }
    }
}

function generateRandomName() {
    return 'name_' + new Date().getTime();
}

function getCurCtrlTemplate(curMouseMove, targetQuiry) {
    var result = '';

    var curCtrl = null;

    if (curMouseMove.hasClass('formDesignerCtrlItemDropdown')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            name: generateRandomName(),
            type: "dropDown",
            label: "نوع سازه",
            isRequired: true,
            textfield: "title",
            valuefield: "id",
            values: [
                {
                    id: '',
                    title: ''
                },
                {
                    id: 'بتونی',
                    title: 'بتونی'
                }
            ]
        };
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getDropdownCTRLTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemDropdownCompany')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            name: 'cIds',
            type: "dropDown",
            label: "شرکت",
            isRequired: true,
            textfield: "title",
            valuefield: "id",
            dataurl: '/Core/BaseData/GetCompanyList'
        };
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getDropdownCTRLTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemLabel')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            type: "label",
            label: "متن مورد نظر",
            color: '#00ff00'
        };
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getLableTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemButtonMoveNextStep')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            class: "btn-primary btn-block ",
            type: "button",
            label: "تایید اطلاعات",
            onClick: "moveToNextStepForSW(this)"
        };
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getButtonTemplateWidthLabel(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemEmpty')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            type: "empty"
        };
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getEmptyCtrlTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemHtml')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            type: "template",
            html: "<div><p>test <strong>test2</strong> <span style='color:red'>test3</span></p<</div>"
        };
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemHtmlLaw')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12",
            type: "cTemplate",
            fieldMaps: [
                {
                    targetTemplate: "firstName",
                    sourceCtrl: [
                        "firstName",
                        "firstAgentName"
                    ]
                },
                {
                    targetTemplate: "lastName",
                    sourceCtrl: [
                        "lastName",
                        "lastAgentName"
                    ]
                },
                {
                    targetTemplate: "nationalCode",
                    sourceCtrl: [
                        "nationalCode",
                        "agentNationalCode"
                    ]
                },
                {
                    targetTemplate: "inputAddress",
                    sourceCtrl: [
                        "reciveAddress"
                    ]
                },
                {
                    targetTemplate: "agent",
                    sourceCtrl: [
                        "agentId"
                    ]
                }
            ],
            dataurl: "/ProposalFilledForm/Proposal/GetTermsHtml"
        };
        result += '<div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">'
        result += getCTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemTokenBox')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            name: generateRandomName(),
            type: "tokenBox",
            label: "نوع سازه",
            isRequired: true,
            textfield: "title",
            valuefield: "id",
            values: [
                {
                    id: '',
                    title: ''
                },
                {
                    id: 'بتونی',
                    title: 'بتونی'
                },
                {
                    id: 'فلزی',
                    title: 'فلزی'
                }
            ]
        };
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getTokennCTRLTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemTextBox')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            name: generateRandomName(),
            type: "text",
            label: "عنوان",
            isRequired: true
        };
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getTextBoxTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemCarPlaque')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            name: generateRandomName(),
            type: "carPlaque",
            label: "پلاک خودرو",
            isRequired: true
        };
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getPlaqueTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemPersianDate')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            name: generateRandomName(),
            type: "persianDateTime",
            label: "تاریخ تولید",
            isRequired: true
        };
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getTextBoxTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemColor')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            name: generateRandomName(),
            type: "color",
            label: "رنگ بدنه خودرو",
            isRequired: true,
            dfaultValue: '#ffffff'
        };
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getTextBoxTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemTextBoxArea')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-12 col-sm-12 col-xs-12 col-lg-12",
            name: generateRandomName(),
            type: "textarea",
            label: "عنوان",
            isRequired: true
        };
        result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">'
        result += getTextAreaTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemPassword')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            name: generateRandomName(),
            type: "password",
            label: "کلمه عبور",
            isRequired: true
        };
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getTextBoxTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemUploadFile')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            name: generateRandomName(),
            type: "file",
            label: "عنوان",
            isRequired: true,
            acceptEx: '.jpg,.png,.jpeg'
        }
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getFileCTRLTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemCheckBox')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-4 col-sm-6 col-xs-12 col-lg-3",
            name: generateRandomName(),
            type: "checkBox",
            label: "عنوان",
            isRequired: true
        }
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getCheckboxButtonTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemRadioBox')) {
        curCtrl = {
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
        }
        result += '<div class="col-md-4 col-sm-6 col-xs-12 col-lg-3">'
        result += getRadioButtonTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemMap')) {
        var newName = 'mapName' + uuidv4RemoveDash();
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-12 col-sm-12 col-xs-12 col-lg-12",
            names: {
                lat: newName + "Lat",
                lon: newName + "Lon",
                zoom: newName + "Zoom"
            },
            width: "100%",
            height: "312px",
            type: "map",
            label: "نقشه"
        };
        result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">'
        result += getMapTemplate(curCtrl);
        result += '</div>';
    } else if (curMouseMove.hasClass('formDesignerCtrlItemMultiRow')) {
        curCtrl = {
            id: uuidv4RemoveDash(),
            parentCL: "col-md-12 col-sm-12 col-xs-12 col-lg-12",
            type: "multiRowInput",
            name: "coverPersons",
            ctrls: [
                {
                    id: uuidv4RemoveDash(),
                    parentCL: "col-md-3 col-sm-6 col-xs-12 col-lg-3",
                    name: "firstName",
                    type: "text",
                    label: "نام",
                    isRequired: true
                },
                {
                    id: uuidv4RemoveDash(),
                    parentCL: "col-md-3 col-sm-6 col-xs-12 col-lg-3",
                    name: "lastName",
                    type: "text",
                    label: "نام خانوادگی",
                    isRequired: true
                },
                {
                    id: uuidv4RemoveDash(),
                    parentCL: "col-md-3 col-sm-6 col-xs-12 col-lg-3",
                    name: "nationalCode",
                    type: "text",
                    label: "کد ملی",
                    isRequired: true,
                    nationalCodeValidation: true
                },
                {
                    id: uuidv4RemoveDash(),
                    parentCL: "col-md-3 col-sm-6 col-xs-12 col-lg-3",
                    type: "dropDown",
                    textfield: "title",
                    valuefield: "id",
                    label: "نصبت با بیمه شده",
                    name: "typeOfRelation",
                    isRequired: true,
                    values: [
                        {
                            id: "",
                            title: ""
                        },
                        {
                            id: "خودم",
                            title: "خودم"
                        },
                        {
                            id: "همسرم",
                            title: "همسرم"
                        },
                        {
                            id: "پدر",
                            title: "پدر"
                        },
                        {
                            id: "مادر",
                            title: "مادر"
                        },
                        {
                            id: "فرزند",
                            title: "فرزند"
                        },
                        {
                            id: "نوه",
                            title: "نوه"
                        }
                    ]
                }
            ]
        }
        result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">'
        result += getMultiRowInputTemplate(curCtrl);
        result += '</div>';
    }
    if (targetQuiry.closest('.MultiRowInputRow').length > 0) {
        updateMultiRowInputs(targetQuiry.closest('.MultiRowInputRow'));
    }

    return result;
}

function updateMultiRowInputs(targetQuiry) {
    if (targetQuiry) {
        setTimeout(function () {
            var parentCtrl = this.targetQuiry.closest('.myCtrl')[0];
            if (parentCtrl) {
                parentCtrl = parentCtrl.ctrl;
                if (parentCtrl)
                    parentCtrl.ctrls = getCtrls(this.targetQuiry.closest('.myCtrl').find('.MultiRowInputRow').eq(0), true);
            }
        }.bind({ targetQuiry: targetQuiry }), 200);
    }
}

function bindSpecPanelInputs(curDesigner, foundConfig) {
    var quirySelector = $(curDesigner).find('.formDesignerCtrlProperties');
    var template = '';

    template += '<div class="propertyTitle" ><span class="propertyCloseButton fa fa-times" ></span>خصوصیات</div>';

    template += '<div style="padding-top:15px;" class="row">';

    template += getIdInput(foundConfig);
    template += getClassInput(foundConfig);

    if (foundConfig.type) {
        switch (foundConfig.type) {
            case 'text':
            case 'textarea':
            case 'number':
                template += getLabelInput(foundConfig);
                template += getNameInput(foundConfig);
                template += getSizeInput(foundConfig);
                template += getDefaultInput(foundConfig);
                template += getChangeDirectionTemplate(foundConfig);
                template += getValidationInput(foundConfig);
                template += getMaskTemplate(foundConfig);
                template += getDisabledInput(foundConfig);
                template += getMultiOrSumPlayInputTemplate(foundConfig, 'multiPlay', 'ضرب خودکار');
                template += getMultiOrSumPlayInputTemplate(foundConfig, 'sumCalculator', 'جمع خودکار');
                template += getHelpHtmlInput(foundConfig);
                break;
            case 'persianDateTime':
                template += getLabelInput(foundConfig);
                template += getNameInput(foundConfig);
                template += getSizeInput(foundConfig);
                template += getMinDayInput(foundConfig);
                template += getMaxDayInput(foundConfig);
                template += getDefaultInput(foundConfig);
                template += getRequiredValidation(foundConfig);
                template += getHelpHtmlInput(foundConfig);
                break;
            case 'color':
                template += getLabelInput(foundConfig);
                template += getNameInput(foundConfig);
                template += getSizeInput(foundConfig);
                template += getDefaultInput(foundConfig);
                template += getRequiredValidation(foundConfig);
                break;
            case 'password':
                template += getLabelInput(foundConfig);
                template += getNameInput(foundConfig);
                template += getSizeInput(foundConfig);
                template += getRequiredValidation(foundConfig);
                template += getHelpHtmlInput(foundConfig);
                break;
            case 'map':
                template += getLabelInput(foundConfig);
                template += getMapNameInput(foundConfig);
                template += getSizeInput(foundConfig);
                template += getWidthInput(foundConfig);
                template += getHeightInput(foundConfig);
                template += getDefaultMapInput(foundConfig);
                break;
            case 'dropDown':
                template += getLabelInput(foundConfig);
                template += getNameInput(foundConfig);
                template += getSizeInput(foundConfig);
                template += getShowCondationInputTemplate(foundConfig);
                template += getSourceUrlTemplate(foundConfig);
                template += getTextAndValueSchemaFields(foundConfig);
                template += getRequiredValidation(foundConfig);
                template += getValuesTemplate(foundConfig);
                break;
            case 'radio':
                template += getLabelInput(foundConfig);
                template += getNameInput(foundConfig);
                template += getSizeInput(foundConfig);
                template += getShowCondationInputTemplate(foundConfig);
                template += getTextAndValueSchemaFields(foundConfig);
                template += getValuesTemplate(foundConfig);
                break;
            case 'checkBox':
                template += getLabelInput(foundConfig);
                template += getNameInput(foundConfig);
                template += getSizeInput(foundConfig);
                template += getRequiredValidation(foundConfig);
                break;
            case 'multiRowInput':
                template += getNameInput(foundConfig);
                template += getAddButtonInput(foundConfig);
                template += getDeleteInput(foundConfig);
                template += getSizeInput(foundConfig);
                break;
            case 'file':
                template += getLabelInput(foundConfig);
                template += getNameInput(foundConfig);
                template += getSizeInput(foundConfig);
                template += getValidExtensionTemplate(foundConfig, quirySelector);
                template += getPreviewImageTemplate(foundConfig);
                template += getRequiredValidation(foundConfig);
                break;
            case 'carPlaque':
                template += getNameInput(foundConfig);
                template += getSizeInput(foundConfig);
                break;
            case 'label':
                template += getLabelInput(foundConfig);
                template += getSizeInput(foundConfig);
                template += getColorInput(foundConfig);
                break;
            case 'empty':
                template += getSizeInput(foundConfig);
                break;
            case 'template':
                template += getSizeInput(foundConfig);
                template += getHtmlInput(foundConfig);
                break;
            case 'cTemplate':
                template += getSizeInput(foundConfig);
                template += getSourceUrlTemplate(foundConfig);
                break;
            case 'button':
                template += getLabelInput(foundConfig);
                template += getSizeInput(foundConfig);
                break;
            case 'tokenBox':
                template += getLabelInput(foundConfig);
                template += getNameInput(foundConfig);
                template += getSizeInput(foundConfig);
                template += getSourceUrlTemplate(foundConfig);
                template += getTextAndValueSchemaFields(foundConfig);
                template += getRequiredValidation(foundConfig);
                template += getValuesTemplate(foundConfig);
                break;
            default:
        }
    } else if ($('#' + foundConfig.id).hasClass('panelSWizard')) {
        template += getLastButtonSWTitleTemplate(foundConfig);
        template += getMoveBackButtonToTop(foundConfig);
        template += getStepInputTemplate(foundConfig);
    } else if ($('#' + foundConfig.id).hasClass('myPanel')) {
        template += getTitleInput(foundConfig);
        template += getIsInquiryInput(foundConfig);
        template += getIsAgentRequired(foundConfig);
        template += getIsCompanyListRequired(foundConfig);
    }

    template += getSaveButtonTemplate();

    template += '</div>';

    quirySelector.html(template);
    quirySelector.closest('.myFormDesigner').addClass('formDesignerCtrlPropertiesShow')

    quirySelector.find('.propertyCloseButton').click(function () { removeAllMarked($(this).closest('.formDesignerCtrlPropertiesShow')); $(this).closest('.formDesignerCtrlPropertiesShow').removeClass('hideToolbar').removeClass('formDesignerCtrlPropertiesShow'); whatToDoAfterSecoundSelect = null; });
    executeArrFunctions();
}

function fillFoundObj(foundConfig, formDataObj, formSelector) {
    foundConfig.parentCL = formDataObj.size;
    foundConfig.textfield = formDataObj.textfield;
    foundConfig.valuefield = formDataObj.valuefield;
    foundConfig.mask = formDataObj.mask;
    foundConfig.ltr = formDataObj.ltr;
    foundConfig.label = formDataObj.inputLabel;
    foundConfig.name = formDataObj.inputName;
    foundConfig.lastStepButtonTitle = formDataObj.lastStepButtonTitle;
    foundConfig.isRequired = formDataObj.isRequired;
    foundConfig.dataurl = formDataObj.dataurl;
    foundConfig.disabled = formDataObj.disabled;
    foundConfig.maxLengh = formDataObj.maxLength;
    foundConfig.class = formDataObj.class;
    foundConfig.moveBackButtonToTop = formDataObj.moveBackButtonToTop;
    foundConfig.id = formDataObj.id;
    foundConfig.hideImagePreview = formDataObj.hideImagePreview;
    foundConfig.dfaultValue = formDataObj.dfaultValue;
    foundConfig.title = formDataObj.title;
    foundConfig.help = formDataObj.help
    foundConfig.hasInquiry = formDataObj.hasInquiry;
    foundConfig.isAgentRequired = formDataObj.isAgentRequired;
    foundConfig.isCompanyListRequired = formDataObj.isCompanyListRequired;
    foundConfig.multiPlay = formDataObj.multiPlay;
    foundConfig.sumCalculator = formDataObj.sumCalculator;
    foundConfig.deleteTitle = formDataObj.deleteTitle;
    foundConfig.addTitle = formDataObj.addTitle;
    if (formDataObj.minDateValidation)
        foundConfig.minDateValidation = Number.parseInt(formDataObj.minDateValidation);
    if (formDataObj.maxDateValidation)
        foundConfig.maxDateValidation = Number.parseInt(formDataObj.maxDateValidation);
    foundConfig.width = formDataObj.width;
    foundConfig.height = formDataObj.height;
    foundConfig.color = formDataObj.color;
    foundConfig.html = formDataObj.html;

    addOrRemoveOnlyValidationOnlyNumber(foundConfig, formDataObj.justNumber);
    addOrRemoveOnlyValidationNumberCount(foundConfig, formDataObj.numberLength);
    addOrRemoveValidationStartWith(foundConfig, formDataObj.startWithN);
    addOrRemoveValidationEmail(foundConfig, formDataObj.email);

    if (foundConfig.type == 'number')
        foundConfig.type = 'text';

    if (formDataObj.mapLat && formDataObj.mapLon && formDataObj.mapZoom)
        foundConfig.values = { lat: formDataObj.mapLat, lon: formDataObj.mapLon, zoom: formDataObj.mapZoom };

    foundConfig.names = null;
    if (formDataObj.mapName)
        foundConfig.names = {
            lat: formDataObj.mapName + 'Lat',
            lon: formDataObj.mapName + 'Lon"',
            zoom: formDataObj.mapName + 'Zoom'
        };


    foundConfig.acceptEx = null;
    if (formDataObj.acceptEx) {
        foundConfig.acceptEx = '';
        if (formDataObj.acceptEx.constructor == Array)
            for (var i = 0; i < formDataObj.acceptEx.length; i++) {
                foundConfig.acceptEx = foundConfig.acceptEx + (i > 0 ? ',' : '') + formDataObj.acceptEx[i];
            }
        else
            foundConfig.acceptEx = formDataObj.acceptEx;
    }

    if (formDataObj.codeMeli)
        foundConfig.nationalCodeValidation = true;
    else
        foundConfig.nationalCodeValidation = null;
    if (formDataObj.values) {
        var jsObj = JSON.parse(formDataObj.values);
        if (jsObj && jsObj.length > 0)
            foundConfig.values = jsObj;

    }

    foundConfig.showHideCondation = null;

    if (formSelector.find('.showCondationHolder').length > 0) {
        var holderQuiry = formSelector.find('.showCondationHolder');
        var resultShowCondation = [];
        holderQuiry.find('.condationItem').each(function () {
            var foundQuiry = $(this).find('.condationItemTitle');
            var curValue = foundQuiry.attr('data-id');
            var isDefault = foundQuiry.hasClass('.condationItemTitleDefault');
            var allHideClass = [];
            var allShowClass = [];
            $(this).find('.condationSubItemHide').each(function () {
                allHideClass.push($(this).attr('data-tclass'));
            });
            $(this).find('.condationSubItemShow').each(function () {
                allShowClass.push($(this).attr('data-tclass'));
            });
            if (allHideClass.length > 0 || allShowClass.length > 0) {
                resultShowCondation.push({
                    value: curValue,
                    classHide: allHideClass,
                    classShow: allShowClass,
                    isDefault: isDefault
                });
            }
        });
        if (resultShowCondation && resultShowCondation.length > 0)
            foundConfig.showHideCondation = resultShowCondation;
    }

    var curElementQuiry = $('#' + formDataObj.id);
    if (!formDataObj.type && curElementQuiry.length > 0 && curElementQuiry.hasClass('panelSWizard')) {
        foundConfig.steps = [];
        if (formDataObj.steps) {
            for (var i = 0; i < formDataObj.steps.length; i++) {
                var tempStep = formDataObj.steps[i];
                tempStep.order = Number.parseFloat(tempStep.order);
                var allCtrls = tempStep.ctrls;
                if (!allCtrls)
                    allCtrls = '[]';

                tempStep.panels = [];
                tempStep.panels.push({ ctrls: JSON.parse(allCtrls) });
                delete tempStep.ctrls;
            }
            foundConfig.steps = formDataObj.steps;
        }
    } else if (!formDataObj.type && curElementQuiry.length > 0 && curElementQuiry.hasClass('myPanel')) {
        var foundWizardQuiry = curElementQuiry.find('.panelSWizard');
        foundConfig.stepWizards = [];
        if (foundWizardQuiry.length > 0) {
            var curWizard = foundWizardQuiry[0].wizard;
            if (curWizard && curWizard.steps && curWizard.steps.length > 0) {
                for (var m = 0; m < curWizard.steps.length; m++) {
                    var curStep = curWizard.steps[m];
                    var allCurStepCtrls = getCtrls($('#stepContent_' + curStep.id));
                    if (!curStep.panels || curStep.panels.length == 0) {
                        curStep.panels = [];
                        curStep.panels.push({});
                    }
                    curStep.panels[0].ctrls = allCurStepCtrls;
                }
            }
            if (curWizard)
                foundConfig.stepWizards.push(curWizard);
        }
    }

    for (var item in foundConfig) {
        if (foundConfig[item] == undefined || foundConfig[item] == null || foundConfig[item] == '' || foundConfig[item] == NaN)
            delete foundConfig[item];
        if (foundConfig[item] == 'true')
            foundConfig[item] = true;
        if (foundConfig[item] == 'false')
            foundConfig[item] = false;
    }
    //console.log(foundConfig, formDataObj);
}

function saveSelectedController(curThis) {
    var rootCtrl = $(curThis).closest('.myFormDesigner');
    if (rootCtrl.length > 0) {
        var foundConfig = rootCtrl[0].getSelectedCtrl();
        if (foundConfig) {
            var formSelector = $(curThis).closest('.formDesignerCtrlProperties');
            var formData = getFormData(formSelector);
            var formDataObj = convertFormDataToJson(formData);
            fillFoundObj(foundConfig, formDataObj, formSelector);
            if (rootCtrl.find('.makeSelectedDP').length > 0) {
                var curElementQuiry = $('#' + formDataObj.id);
                if (!formDataObj.type && curElementQuiry.length > 0 && curElementQuiry.hasClass('myPanel'))
                    rootCtrl.find('.makeSelectedDP').replaceWith(getPanelTemplate(foundConfig));
                else if (!formDataObj.type && curElementQuiry.length > 0 && curElementQuiry.hasClass('panelSWizard'))
                    rootCtrl.find('.makeSelectedDP').replaceWith(getStepWizardTemplate(foundConfig));
                else
                    rootCtrl.find('.makeSelectedDP').replaceWith(getInputTemplate(foundConfig));
                executeArrFunctions();
                $(curThis).closest('.formDesignerCtrlProperties').find('.propertyCloseButton').click();
                rootCtrl[0].addToUR();
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

function getMultiOrSumPlayInputTemplate(foundConfig, name, title) {
    var result = '';

    var multiPlay = foundConfig[name];
    if (!multiPlay)
        multiPlay = [];

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12 sumOrMultiplyHolder">';

    result += '<div style="margin-bottom:10px;" class="sumOrMultiplyHeader" >' + title + '<i onclick="return addCtrlToSumOrMultiply(this, event, \'' + name + '\')" class="fa fa-plus addCtrlToShowCondationButtonShow" ></i></div>';

    for (var i = 0; i < multiPlay.length; i++) {
        result += '<div class="sumOrMultiplyItem sumOrMultiplyItemShow" >';
        var curCtrlTitle = $('#' + multiPlay[i]).closest('.myCtrl').find('label').text();
        result += '<span>' + (curCtrlTitle ? curCtrlTitle : 'خالی') + '<i class="fa fa-trash deleteSumOrMultiplyCB" onclick="removeThisMItem(this)" ></i><input type="hidden" name="' + name + '" value="' + multiPlay[i] + '" /></span>';
        result += '</div>';
    }

    result += '</div>';


    return result;
}

function removeThisMItem(curButton) {
    if (curButton) {
        $(curButton).closest('.sumOrMultiplyItem').remove();
    }
}

function addCtrlToSumOrMultiply(curButton, e, name) {
    e.stopPropagation();

    if (curButton) {
        var buttonQuiryButton = $(curButton);
        if (buttonQuiryButton.hasClass('fa-plus')) {
            buttonQuiryButton.removeClass('fa-plus').addClass('fa-ban');
            $(curButton).closest('.formDesigner').addClass('makeShowAllHideCtrl');
            whatToDoAfterSecoundSelect = function (targetObj) {
                var foundItemObj = $(targetObj).find('.myCtrl')[0].ctrl;
                var curId = '';

                if ($(targetObj).find('input').length > 0)
                    curId = $(targetObj).find('input').attr('id');
                else if ($(targetObj).find('select').length > 0)
                    curId = $(targetObj).find('select').attr('id');

                if (!curId) {
                    curId = uuidv4RemoveDash();
                    if ($(targetObj).find('input').length > 0)
                        $(targetObj).find('input').attr('id', curId);
                    else if ($(targetObj).find('select').length > 0)
                        $(targetObj).find('select').attr('id', curId);

                    foundItemObj.id = curId;
                }
                var holderQuiry = $(this.curButton).closest('.sumOrMultiplyHolder');
                var curCtrlTitle = $(targetObj).text();
                if (!curCtrlTitle)
                    curCtrlTitle = 'خالی';

                var curTemplate = '';
                curTemplate += '<div class="sumOrMultiplyItem sumOrMultiplyItemShow" >';
                curTemplate += '<span>' + (curCtrlTitle ? curCtrlTitle : 'خالی') + '<i class="fa fa-trash deleteSumOrMultiplyCB" onclick="removeThisMItem(this)" ></i><input type="hidden" name="' + name + '" value="' + curId + '" /></span>';
                curTemplate += '</div>';

                holderQuiry.append(curTemplate);

                $(this.curButton).removeClass('fa-ban').addClass('fa-plus');
                $(this.curButton).closest('.formDesigner').removeClass('makeShowAllHideCtrl');
            }.bind({ curButton: curButton });
        } else {
            buttonQuiryButton.removeClass('fa-ban').addClass('fa-plus');
            $(curButton).closest('.formDesigner').removeClass('makeShowAllHideCtrl');
            whatToDoAfterSecoundSelect = null;
        }
    }

    return false;
}

function getShowCondationInputTemplate(foundConfig) {
    var result = '';

    var options = [];
    var showHideCondation = foundConfig.showHideCondation;
    if (!showHideCondation)
        showHideCondation = [];
    if (foundConfig.type == 'dropDown' && foundConfig.id)
        $('#' + foundConfig.id).find('option').each(function () {
            options.push({ id: $(this).attr('value'), title: $(this).text() });
        });

    if (options.length == 0 && foundConfig.values && foundConfig.values.length > 0)
        options = foundConfig.values;

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12 showCondationHolder">';

    result += '<div class="showCondationHeader" >مخفی یا نمایش</div>';

    if (options.length == 0)
        result += '<div>فاقد مقدار</div>';
    else {
        for (var i = 0; i < options.length; i++) {
            var foundCondation = showHideCondation.filter(function (item) { return item.value == options[i].id; });
            if (foundCondation.length > 0)
                foundCondation = foundCondation[0];
            else
                foundCondation = null;
            result += '<div class="condationItem" >';
            result += '<span onclick="setDefaultShowCondation(this)" class="condationItemTitle ' + (foundCondation && foundCondation.isDefault ? 'condationItemTitleDefault' : '') + '" data-id="' + options[i].id + '" >' + (options[i].title ? options[i].title : 'خالی') + '<i onclick="return addCtrlToShowCondation(this, event)" class="fa fa-plus addCtrlToShowCondationButtonHide" ></i><i onclick="return addCtrlToShowCondation(this, event)" class="fa fa-plus addCtrlToShowCondationButtonShow" ></i></span>';

            if (foundCondation) {
                if (foundCondation.classShow && foundCondation.classShow.length > 0) {
                    for (var j = 0; j < foundCondation.classShow.length; j++) {
                        $('.' + foundCondation.classShow[j]).each(function () {
                            result += '<span title="' + foundCondation.classShow[j] + '" data-tClass="' + foundCondation.classShow[j] + '" class="condationSubItem condationSubItemShow">' + $(this).find('label').text() + '<i class="fa fa-trash deleteShowCondationCB" onclick="removeThisClass(this)" ></i></span>';
                        });
                    }
                }
                if (foundCondation.classHide && foundCondation.classHide.length > 0) {
                    for (var j = 0; j < foundCondation.classHide.length; j++) {
                        $('.' + foundCondation.classHide[j]).each(function () {
                            result += '<span title="' + foundCondation.classHide[j] + '" data-tClass="' + foundCondation.classHide[j] + '" class="condationSubItem condationSubItemHide">' + $(this).find('label').text() + '<i class="fa fa-trash deleteShowCondationCB" onclick="removeThisClass(this)" ></i></span>';
                        });
                    }
                }
            }

            result += '</div>';
        }
    }

    result += '<div class="simpleBorder" ></div>';
    result += '</div>';


    return result;
}

var whatToDoAfterSecoundSelect = null;

function addCtrlToShowCondation(curButton, e) {
    e.stopPropagation();

    if (curButton) {
        var buttonQuiryButton = $(curButton);
        var isShow = buttonQuiryButton.hasClass('addCtrlToShowCondationButtonShow');
        if (buttonQuiryButton.hasClass('fa-plus')) {
            buttonQuiryButton.removeClass('fa-plus').addClass('fa-ban');
            $(curButton).closest('.formDesigner').addClass('makeShowAllHideCtrl');
            whatToDoAfterSecoundSelect = function (targetObj) {
                var foundItemObj = $(targetObj).find('.myCtrl')[0].ctrl;
                var allClass = [];
                if (foundItemObj.class)
                    allClass = foundItemObj.class.split(' ');
                allClass = allClass.filter(function (item) { return item && item.trim(); });
                var foundClass = allClass.filter(function (item) { return item && item.substr(0, 14) == 'showHideClass_'; })
                if (foundClass.length == 0) {
                    var lookingForClass = 'showHideClass_' + uuidv4RemoveDash();
                    foundItemObj.class = foundItemObj.class + ' ' + lookingForClass;
                    $(targetObj).find('.myCtrl').addClass(lookingForClass);
                    foundClass.push(lookingForClass);
                }
                var targetClass = foundClass[0];
                var holderQuiry = $(this.curButton).closest('.condationItem');
                var isShow = this.isShow;
                $('.' + targetClass).each(function () {
                    if (isShow)
                        holderQuiry.append('<span title="' + targetClass + '" data-tClass="' + targetClass + '" class="condationSubItem condationSubItemShow">' + $(this).find('label').text() + '<i class="fa fa-trash deleteShowCondationCB" onclick="removeThisClass(this)" ></i></span>');
                    else
                        holderQuiry.append('<span title="' + targetClass + '" data-tClass="' + targetClass + '" class="condationSubItem condationSubItemHide">' + $(this).find('label').text() + '<i class="fa fa-trash deleteShowCondationCB" onclick="removeThisClass(this)" ></i></span>');
                });
                $(this.curButton).removeClass('fa-ban').addClass('fa-plus');
                $(this.curButton).closest('.formDesigner').removeClass('makeShowAllHideCtrl');
            }.bind({ curButton: curButton, isShow: isShow });
        } else {
            buttonQuiryButton.removeClass('fa-ban').addClass('fa-plus');
            $(curButton).closest('.formDesigner').removeClass('makeShowAllHideCtrl');
            whatToDoAfterSecoundSelect = null;
        }
    }

    return false;
}

function setDefaultShowCondation(curButton) {
    if (curButton && !$(curButton).hasClass('condationItemTitleDefault')) {

        $(curButton).closest('.showCondationHolder').find('.condationItemTitleDefault').removeClass('condationItemTitleDefault');
        $(curButton).addClass('condationItemTitleDefault');
    }
}

function removeThisClass(curButton) {
    if (curButton) {
        var parentQuiry = $(curButton).closest('.condationSubItem');
        var isShow = parentQuiry.hasClass('condationSubItemShow');
        var targetClass = parentQuiry.attr('data-tClass');
        if (targetClass) {
            if (isShow)
                parentQuiry.closest('.condationItem').find('.condationSubItemShow[data-tClass="' + targetClass + '"]').remove();
            else
                parentQuiry.closest('.condationItem').find('.condationSubItemHide[data-tClass="' + targetClass + '"]').remove();
        }
    }
}

function getLastButtonSWTitleTemplate(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getTextBoxTemplate({
        name: "lastStepButtonTitle",
        type: "text",
        label: "عنوان دکمه ذخیره",
        dfaultValue: foundConfig.lastStepButtonTitle
    });
    result += '</div>';

    return result;
}

function getColorInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getTextBoxTemplate({
        name: "color",
        type: "color",
        label: "رنگ",
        dfaultValue: foundConfig.color
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

function getMinDayInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getTextBoxTemplate({
        name: "minDateValidation",
        type: "number",
        label: "حداق روز",
        dfaultValue: foundConfig.minDateValidation
    });

    result += '</div>';

    return result;
}

function getMaxDayInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getTextBoxTemplate({
        name: "maxDateValidation",
        type: "text",
        label: "حداکثر روز",
        dfaultValue: foundConfig.maxDateValidation
    });

    result += '</div>';

    return result;
}

function getTitleInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getTextBoxTemplate({
        name: "title",
        type: "text",
        label: "عنوان",
        dfaultValue: foundConfig.title
    });

    result += '</div>';

    return result;
}

function getIdInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getTextBoxTemplate({
        name: "id",
        type: "text",
        label: "شناسه",
        dfaultValue: foundConfig.id
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

function getPreviewImageTemplate(foundConfig) {
    var result = '';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getCheckboxButtonTemplate({
        id: uuidv4RemoveDash(),
        name: "hideImagePreview",
        type: "checkBox",
        label: "مخفی پیش نمایش ؟",
        dfaultValue: foundConfig.hideImagePreview
    });
    result += '</div>';

    return result;
}

function getCtrls(holder, ignoreMultiRow) {
    var result = []
    if (holder && holder.length > 0) {
        holder.find('.myCtrl').each(function () {
            if ((ignoreMultiRow || $(this).closest('.MultiRowInputRow').length == 0) && $(this).closest('.plaqueRightPartLeft').length == 0) {
                var curCtrl = $(this)[0].ctrl;
                if (curCtrl) {
                    result.push(curCtrl);
                }
            }
        });
    }
    return result;
}

function getStepInputTemplate(foundConfig) {
    var result = '';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += '<div style="border-bottom:1px solid silver;margin-bottom:20px;color:red;" >کلیه استب ها <i onclick="addNewStepFD(this)" class="fa fa-plus addNewStepButton" ></i></div>';
    result += '</div>';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12 ">';
    result += '<div class="row holderAllStepsProperty">';
    if (foundConfig && foundConfig.steps) {
        var allSteps = foundConfig.steps.sort(function (a, b) { return a.order - b.order; });
        for (var i = 0; i < allSteps.length; i++) {
            var curStep = allSteps[i];

            result += getStepInputTemplateItem(curStep, i);
        }
    }
    result += '</div>';
    result += '</div>';


    return result;
}

function getStepInputTemplateItem(curStep, i, useInnerStepObj) {
    var result = '';

    result += '<div data-index="' + i + '" class="fdStepItem col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += '<div style="border-bottom:1px solid silver;margin-bottom:20px;" ><span  class="stepHeaderPInfo" >استپ شماره ' + (i + 1) + '</span><i onclick="removeThisStep(this)" class="fa fa-trash deleteStepButton"></i></div>';
    result += getTextBoxTemplate({
        name: "steps[" + i + "].id",
        type: "text",
        label: "شناسه",
        dfaultValue: curStep.id
    });
    result += '</div>';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getTextBoxTemplate({
        name: "steps[" + i + "].order",
        type: "number",
        label: "ترتیب",
        dfaultValue: curStep.order
    });
    result += '</div>';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getTextBoxTemplate({
        name: "steps[" + i + "].title",
        type: "text",
        label: "عنوان",
        dfaultValue: curStep.title
    });
    result += '</div>';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getCheckboxButtonTemplate({
        id: uuidv4RemoveDash(),
        name: "steps[" + i + "].hideMoveNextButton",
        type: "checkBox",
        label: "مخفی کردن دکمه مرحله بعد",
        dfaultValue: curStep.hideMoveNextButton
    });
    result += '<input type="hidden" name="steps[' + i + '].ctrls" value=\'' + (useInnerStepObj ? JSON.stringify(curStep.panels[0].ctrls) : JSON.stringify(getCtrls($('#stepContent_' + curStep.id)))) + '\' />';
    result += '</div>';

    result += '</div>';

    return result;
}

function addNewStepFD(curButton) {
    var modalId = 'addNewStepModalPPF';
    var designerQuiry = $(curButton).closest('.myFormDesigner');
    var baseUrl = designerQuiry.attr('data-baseUrl');
    var dsignerId = designerQuiry.attr('id');

    if (!baseUrl)
        return;

    if ($('#' + modalId).length == 0) {
        $('body').append(getModualTemplate({
            id: modalId,
            title: 'افزودن استب جدید',
            ctrls: [
                {
                    id: 'ppfDropdownId',
                    parentCL: 'col-md-12 col-sm-12 col-xs-12 col-lg-12',
                    type: 'dropDown2',
                    textfield: 'title',
                    valuefield: 'id',
                    label: 'فرم پیشنهاد',
                    name: 'fid',
                    isRequired: true,
                    dataurl: baseUrl + '/GetFormList',
                    onChange: 'updateStepDD("ppfDropdownId", "' + baseUrl + '")'
                },
                {
                    id: 'ppfDropdownStepId',
                    parentCL: 'col-md-12 col-sm-12 col-xs-12 col-lg-12',
                    type: 'dropDown',
                    textfield: 'title',
                    valuefield: 'id',
                    label: 'استب',
                    name: 'step',
                    isRequired: true,
                    values: []
                }
            ],
            actions: [
                {
                    title: 'بستن',
                    class: 'btn-secondary',
                    onClick: 'closeThisModal(this)'
                },
                {
                    title: 'افزودن',
                    class: 'btn-primary',
                    onClick: 'addNewStepF(this, \'' + dsignerId + '\')'
                }
            ]
        }));

        executeArrFunctions();
    }

    clearForm($('#' + modalId));
    $('#' + modalId).modal('show');
}

function addNewStepF(curButton, dsignerId) {
    if (dsignerId && curButton) {
        var formData = getFormData($(curButton).closest('.modal-content'));
        var formDataObj = convertFormDataToJson(formData);
        if (formDataObj && formDataObj.fid && formDataObj.step) {
            var curStep = JSON.parse(formDataObj.step);
            var stepHolderQuiry = $('#' + dsignerId).find('.formDesignerCtrlProperties').find('.holderAllStepsProperty');
            stepHolderQuiry.append(getStepInputTemplateItem(curStep, stepHolderQuiry.find('.fdStepItem').length, true));
            executeArrFunctions();
            closeThisModal(curButton);
        } else {
            $.toast({
                heading: 'خطا',
                text: 'لطفا استب را انتخاب کنید',
                textAlign: 'right',
                position: 'bottom-right',
                showHideTransition: 'slide',
                icon: 'error'
            });
        }
    }
}

function updateStepDD(ppfId, baseUrl) {
    var formQuiry = $('#' + ppfId).closest('.modal-body');
    var formData = getFormData(formQuiry);
    var formDataJsonOBj = convertFormDataToJson(formData);

    if (baseUrl && formDataJsonOBj && formDataJsonOBj.fid && formDataJsonOBj.fid != 'null') {
        showLoader(formQuiry);
        postForm(baseUrl + '/GetFormJsonConfig', formData, function (res) {
            if (res && res.panels && res.panels.length == 1 && res.panels[0].stepWizards && res.panels[0].stepWizards.length > 0 && res.panels[0].stepWizards[0].steps) {
                var allSteps = res.panels[0].stepWizards[0].steps;
                var source = [];
                source.push({ id: '', title: '' });
                allSteps.forEach(function (item) {
                    source.push({
                        id: JSON.stringify(item),
                        title: item.title
                    });
                });

                $('#ppfDropdownStepId').closest('.myDropdown').replaceWith(getDropdownCTRLTemplate({
                    id: 'ppfDropdownStepId',
                    parentCL: 'col-md-12 col-sm-12 col-xs-12 col-lg-12',
                    type: 'dropDown',
                    textfield: 'title',
                    valuefield: 'id',
                    label: 'استب',
                    name: 'step',
                    isRequired: true,
                    values: source
                }));

                executeArrFunctions();
            }
        }, null, function () { hideLoader(formQuiry) })
    }
}

function updateStepsKeys() {
    $('.formDesignerCtrlProperties').each(function () {
        var index = 0;
        $(this).find('.fdStepItem').each(function () {
            $(this).find('[name]').each(function () {
                var curName = $(this).attr('name');
                if (curName.indexOf('[') != -1 && curName.indexOf(']') != -1) {
                    var leftPart = curName.split('[')[0];
                    var rightPart = curName.split(']')[1];
                    $(this).attr('name', leftPart + '[' + index + ']' + rightPart);
                }
            });
            index++;
        });
    });
}

function removeThisStep(curButton) {
    if (curButton) {
        $(curButton).closest('.fdStepItem').remove();
        updateStepsKeys();
    }
}

function getMoveBackButtonToTop(foundConfig) {
    var result = '';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getCheckboxButtonTemplate({
        id: uuidv4RemoveDash(),
        name: "moveBackButtonToTop",
        type: "checkBox",
        label: "نمایش دکمه بازگشت در بالا ؟",
        dfaultValue: foundConfig.moveBackButtonToTop
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

function getIsInquiryInput(foundConfig) {
    var result = '';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getCheckboxButtonTemplate({
        id: uuidv4RemoveDash(),
        name: "hasInquiry",
        type: "checkBox",
        label: " نیاز به استعلام داره ؟",
        dfaultValue: foundConfig.hasInquiry
    });
    result += '</div>';

    return result;
}

function getIsAgentRequired(foundConfig) {
    var result = '';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getCheckboxButtonTemplate({
        id: uuidv4RemoveDash(),
        name: "isAgentRequired",
        type: "checkBox",
        label: "انتخاب نماینده اجباریه ؟",
        dfaultValue: foundConfig.isAgentRequired
    });
    result += '</div>';

    return result;
}

function getIsCompanyListRequired(foundConfig) {
    var result = '';

    result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getCheckboxButtonTemplate({
        id: uuidv4RemoveDash(),
        name: "isCompanyListRequired",
        type: "checkBox",
        label: "انتخاب شرکت بیمه به صورت چنتایی هست ؟",
        dfaultValue: foundConfig.isCompanyListRequired
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

function getDefaultInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getTextBoxTemplate({
        name: "dfaultValue",
        type: "text",
        label: "مقدار پیش فرض",
        dfaultValue: foundConfig.dfaultValue
    });
    result += '</div>';

    return result;
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

function getMapNameInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getTextBoxTemplate({
        name: "mapName",
        type: "text",
        label: "نام",
        dfaultValue: (foundConfig.names && foundConfig.names.lat ? foundConfig.names.lat.substr(0, foundConfig.names.lat.length - 3) : '')
    });
    result += '</div>';

    return result;
}

function getWidthInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getTextBoxTemplate({
        name: "width",
        type: "text",
        label: "پهنا",
        dfaultValue: foundConfig.width
    });
    result += '</div>';

    return result;
}

function getDefaultMapInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getMapTemplate({
        parentCL: "col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12",
        names: {
            lat: "mapLat",
            lon: "mapLon",
            zoom: "mapZoom"
        },
        values: foundConfig.values,
        width: "100%",
        height: "312px",
        type: "map",
        label: "مختصات پیش فرض"
    });
    result += '</div>';

    return result;
}

function getHeightInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getTextBoxTemplate({
        name: "height",
        type: "text",
        label: "ارتفاع",
        dfaultValue: foundConfig.height
    });
    result += '</div>';

    return result;
}

function getAddButtonInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getTextBoxTemplate({
        name: "addTitle",
        type: "text",
        label: "عنوان دکمه افزودن",
        dfaultValue: foundConfig.addTitle
    });

    result += '</div>';

    return result;
}

function getDeleteInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getTextBoxTemplate({
        name: "deleteTitle",
        type: "text",
        label: "عنوان دکمه حذف",
        dfaultValue: foundConfig.deleteTitle
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

function getHtmlInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getCkEditorTemplate({
        id: uuidv4RemoveDash(),
        name: "html",
        type: "ck",
        label: "محتوی",
        dfaultValue: foundConfig.html
    });

    result += '</div>';

    return result;
}

function getHelpHtmlInput(foundConfig) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';

    result += getCkEditorTemplate({
        id: uuidv4RemoveDash(),
        name: "help",
        type: "ck",
        label: "راهنما",
        dfaultValue: foundConfig.help
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

function getValidExtensionTemplate(foundConfig, quirySelector) {
    var result = '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
    result += getTokennCTRLTemplate(
        {
            name: "acceptEx",
            type: "tokenBox",
            label: "پسوند مورد قبول",
            textfield: 'title',
            valuefield: 'id',
            values: [
                {
                    id: "",
                    title: ""
                },
                {
                    id: ".jpg",
                    title: "jpg"
                },
                {
                    id: ".png",
                    title: "png"
                },
                {
                    id: ".jpeg",
                    title: "jpeg"
                },
                {
                    title: "pdf",
                    id: ".pdf"
                },
                {
                    title: "doc",
                    id: ".doc"
                },
                {
                    title: "docx",
                    id: ".docx"
                },
                {
                    title: "xls",
                    id: ".xls"
                },
                {
                    title: "xlsx",
                    id: ".xlsx"
                }
            ]
        }
    );
    result += '</div>';


    if (foundConfig.acceptEx)
        functionsList.push(function () {
            bindForm({ acceptEx: this.foundConfig.acceptEx.split(',') }, this.quirySelector);
        }.bind({ quirySelector: quirySelector, foundConfig: foundConfig }));

    return result;
}

function showImportModal(curButton) {
    var modalId = 'addNewModalPPF';
    var designerQuiry = $(curButton).closest('.myFormDesigner');
    var baseUrl = designerQuiry.attr('data-baseUrl');
    var dsignerId = designerQuiry.attr('id');

    if (!baseUrl)
        return;

    if ($('#' + modalId).length == 0) {
        $('body').append(getModualTemplate({
            id: modalId,
            title: 'بارگزاری فرم پیشنهاد',
            ctrls: [
                {
                    id: 'addNewModalPPFppfDropdownId',
                    parentCL: 'col-md-12 col-sm-12 col-xs-12 col-lg-12',
                    type: 'dropDown2',
                    textfield: 'title',
                    valuefield: 'id',
                    label: 'فرم پیشنهاد',
                    name: 'fid',
                    isRequired: true,
                    dataurl: baseUrl + '/GetFormList'
                }
            ],
            actions: [
                {
                    title: 'بستن',
                    class: 'btn-secondary',
                    onClick: 'closeThisModal(this)'
                },
                {
                    title: 'بارگزاری',
                    class: 'btn-primary',
                    onClick: 'loadPPF( \'' + dsignerId + '\', \'addNewModalPPFppfDropdownId\', \'' + baseUrl + '\', this)'
                }
            ]
        }));

        executeArrFunctions();
    }

    clearForm($('#' + modalId));
    $('#' + modalId).modal('show');
}

function loadPPF(designerId, ppfId, baseUrl, curButton) {
    var formQuiry = $('#' + ppfId).closest('.modal-body');
    var formData = getFormData(formQuiry);
    var formDataJsonOBj = convertFormDataToJson(formData);

    if (baseUrl && formDataJsonOBj && formDataJsonOBj.fid && formDataJsonOBj.fid != 'null') {
        showLoader(formQuiry);
        postForm(baseUrl + '/GetFormJsonConfig', formData, function (res) {
            if (res) {
                var curObj = $('#' + designerId)[0];
                var curCtrl = $('#' + designerId)[0].ctrl;
                $('#' + curCtrl.id + '_renderPlace').html('');
                generateForm(res, curCtrl.id + '_renderPlace');
                closeThisModal(curButton);
                curObj.newUndoRedo(); curObj.addToUR();
            }
        }, null, function () { hideLoader(formQuiry) })
    }
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