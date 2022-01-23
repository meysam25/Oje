
var functionsList = [];

function generateForm(res, targetId) {
    var result = '';
    if (res && res.panels) {
        for (var i = 0; i < res.panels.length; i++) {
            result += getPanelTemplate(res.panels[i]);
        }
    }
    if (targetId) {
        if (typeof targetId === 'string' || targetId instanceof String) {
            $('#' + targetId).html(result);
            executeArrFunctions();
        }
        else {
            $(targetId).html(result);
            executeArrFunctions();
        }

    }
    else {
        $('.MainHolder').html(result);
        executeArrFunctions();
    }
}

function makeCtrlFocused(curThis) {
    $(curThis).closest('.myCtrl').addClass('myCtrlMakeActive');
}
function makeCtrlBlure(curThis) {
    $(curThis).closest('.myCtrl').removeClass('myCtrlMakeActive');
}

function executeArrFunctions() {
    while (functionsList.length > 0) {
        var sItem = functionsList.splice(0, 1);
        sItem[0]();
    }
}

function getPanelTemplate(panel) {
    var result = '';
    if (panel) {
        if (!panel.id)
            panel.id = uuidv4RemoveDash();
        result += '<div id="' + panel.id + '" ' + (panel.loadUrl ? 'data-url="' + panel.loadUrl + '"' : '') + '  class="myPanel ' + (panel.class ? panel.class : '') + '" >';
        if (panel.title)
            result += '<h4 style="padding:10px;padding-right:0px;">' + panel.title + '</h3>';
        if (panel.charts) {
            for (var i = 0; i < panel.charts.length; i++) {
                result += getChartTemplate(panel.charts[i]);
            }
        }
        if (panel.stepWizards) {
            for (var i = 0; i < panel.stepWizards.length; i++) {
                result += getStepWizardTemplate(panel.stepWizards[i]);
            }
        }
        if (panel.grids) {
            for (var i = 0; i < panel.grids.length; i++) {
                result += getGridTemplate(panel.grids[i]);
            }
        }
        if (panel.moduals) {
            for (var i = 0; i < panel.moduals.length; i++) {
                result += getModualTemplate(panel.moduals[i]);
            }
        }
        if (panel.treeViews) {
            for (var i = 0; i < panel.treeViews.length; i++) {
                result += getTreeViewTemplate(panel.treeViews[i]);
            }
        }

        if (panel.ctrls) {
            result += '<div class="row">';
            for (var i = 0; i < panel.ctrls.length; i++) {
                result += getInputTemplate(panel.ctrls[i]);
            }
            result += '</div>';
        }

        if (panel.actions && panel.actions.length > 0) {
            result += '<div style="text-align:right;padding-top:15px;">'
            for (var i = 0; i < panel.actions.length; i++) {
                result += getButtonTemplate(panel.actions[i]);
            }
            result += '</div>';
        }
        functionsList.push(function () {
            bindPanelByUrl($('#' + this.panelId));
        }.bind({ panelId: panel.id }));
        result += '</div>';
    }
    return result;
}

var isTryLoadingChart = false;

function getChartTemplate(chart) {
    var result = '';

    if (!window['Highcharts'] && !isTryLoadingChart) {
        loadJS("/Modules/Core/js/chart.min.js.gz");
        isTryLoadingChart = true;
    }

    if (chart && chart.config && chart.url && chart.dataSchmea) {
        if (!chart.id)
            chart.id = uuidv4RemoveDash();
        result += '<div id="' + chart.id + '" >';

        result += '</div>';


        functionsList.push(function () {
            postForm(chart.url, new FormData(), function (res) {
                this.config[this.dataSchmea] = res;
                initChartUntilSerciptLoaded(this.id, this.config);

            }.bind(this));
        }.bind(chart));
    }

    return result;
}

function initChartUntilSerciptLoaded(id, config) {
    if (id && config) {
        if ($('#' + id).length > 0) {
            if (window['Highcharts']) {
                Highcharts.chart(id, config);
            }
            else {
                $('#' + id)[0].interval = setInterval(function () {
                    if (window['Highcharts']) {
                        clearInterval($('#' + this.id)[0].interval);
                        Highcharts.chart(this.id, this.config);
                    }
                }.bind({ id: id, config: config }), 1000);
            }
        }
    }
}

function getTreeViewTemplate(treeView) {
    var result = '';

    if (treeView) {
        var treeViewId = uuidv4RemoveDash();
        result += '<div data-selected="' + treeView.dataSelected + '" data-bindsrcparameters="' + treeView.bindSrcParameters + '" data-url="' + treeView.dataUrl + '" data-textfield="' + treeView.dataTextfield + '" data-valuefield="' + treeView.dataValuefield + '" data-childfeild="' + treeView.dataChildfeild + '" id="' + treeViewId + '" class="treeView"></div>';
        functionsList.push(function () { $('#' + this.treeViewId).initTreeView(); }.bind({ treeViewId }));
    }

    return result;
}

function getModualTemplate(modual) {
    var modalId = (!modual.id) ? uuidv4RemoveDash() : modual.id
    var result =
        '<div class="modal fade" id="' + modalId + '"  role="dialog"  aria-hidden="true">' +
        '<div class="modal-dialog modal-dialog-centered  ' + modual.class + '" role="document">' +
        '<div class="modal-content">' +
        '<div class="modal-header">' +
        '<h5 class="modal-title">' + modual.title + '</h5>' +
        '<button type="button" class="close" data-dismiss="modal" aria-label="Close">' +
        '<span aria-hidden="true">&times;</span>' +
        '</button>' +
        '</div>' +
        '<div class="modal-body">' +
        getModualTemplateCTRL(modual) +
        '</div>' +
        getModualTemplateActionButton(modual) +
        '</div>' +
        '</div>' +
        '</div>';

    functionsList.push(function () {
        $('#' + modalId).keypress(function (e) {
            if (e.keyCode == 13)
                $(this).find('button.btn-primary').each(function () {
                    if ($(this).is(':visible')) {
                        $(this).click();
                    }
                });
        });
    }.bind({ modalId: modalId }));
    if (modual.autoShowModalOnFalse) {
        functionsList.push(function () {
            var isValid = true;
            if (modual.autoShowModalOnFalseUrlCondation && modual.autoShowModalOnFalseUrlCondation.length > 0) {
                var isValidCondation = false;
                for (var i = 0; i < modual.autoShowModalOnFalseUrlCondation.length; i++) {
                    if (modual.autoShowModalOnFalseUrlCondation[i] == location.pathname)
                        isValidCondation = true;
                }
                if (isValidCondation == false)
                    isValid = false;
            }
            if (isValid == true) {
                postForm(this.modual.autoShowModalOnFalse, new FormData(), function (res) {
                    if (!res) {
                        $('#' + this.modual.id).modal('show');
                    }
                }.bind({ modual: this.modual }));
            }
        }.bind({ modual: modual }));
    }

    return result;
}
function getModualTemplateCTRL(modual) {
    var result = '';

    if (modual && modual.ctrls) {
        result += '<div class="row">';
        for (var i = 0; i < modual.ctrls.length; i++) {
            result += getInputTemplate(modual.ctrls[i]);
        }
        result += '</div>';
    } else if (modual && modual.panels) {
        for (var i = 0; i < modual.panels.length; i++) {
            result += getPanelTemplate(modual.panels[i]);
        }
    }

    return result;
}
function getInputTemplate(ctrl) {
    var result = '';
    if (ctrl) {
        if (!ctrl.id)
            ctrl.id = uuidv4RemoveDash();
        if (ctrl.type != 'hidden')
            result += '<div class="' + ctrl.parentCL + '">';
        switch (ctrl.type) {
            case 'hidden':
                result += '<input autocomplete="off" type="hidden" name="' + ctrl.name + '" ' + (ctrl.dfaultValue ? 'value="' + ctrl.dfaultValue + '"' : '') + ' />';
                break;
            case 'text':
            case 'password':
            case 'persianDateTime':
                result += getTextBoxTemplate(ctrl);
                break;
            case 'dropDown':
            case 'dropDown2':
                result += getDropdownCTRLTemplate(ctrl);
                break;
            case 'tokenBox':
            case 'tokenBox2':
                result += getTokennCTRLTemplate(ctrl);
                break;
            case 'radio':
                result += getRadioButtonTemplate(ctrl);
                break;
            case 'checkBox':
                result += getCheckboxButtonTemplate(ctrl);
                break;
            case 'file':
                result += getFileCTRLTemplate(ctrl);
                break;
            case 'dynamicFileUpload':
                result += getDynamicFileUploadCtrlTemplate(ctrl);
                break;
            case 'dynamicFileUploadDepend':
                result += getDynamicFileUploadDependCtrlTemplate(ctrl);
                break;
            case 'textarea':
                result += getTextAreaTemplate(ctrl);
                break;
            case 'ck':
                result += getCkEditorTemplate(ctrl);
                break;
            case 'dynamiCtrls':
                result += getDynamicCtrlsTemplate(ctrl);
                break;
            case 'chkCtrlBox':
                result += getChkCtrlBoxTemplateCtrl(ctrl);
                break;
            case 'multiRowInput':
                result += getMultiRowInputTemplate(ctrl);
                break;
            case 'Content':
                result += getContentTemplate(ctrl);
                break;
            case 'Shortcut':
                result += getShortcutButtonTemplate(ctrl);
                break;
            case 'carPlaque':
                result += getPlaqueTemplate(ctrl);
                break;
            case 'button':
                result += getButtonTemplateWidthLabel(ctrl);
                break;
            case 'countDownButton':
                result += getCountDownButtonTemplate(ctrl);
                break;
            case 'empty':
            default:
                break;
        }
        if (ctrl.type != 'hidden')
            result += '</div>';
    }

    return result;
}

function getCountDownButtonTemplate(ctrl) {
    var result = '';

    if (ctrl && ctrl.startNumber && ctrl.countDownText && ctrl.finishedCountDownText) {
        result += '<label data-onClick="' + ctrl.onClick + '"  id="' + ctrl.id + '" data-finishedCountDownText="' + ctrl.finishedCountDownText + '" data-countDownText="' + ctrl.countDownText + '" data-maxNumber="' + ctrl.startNumber + '" class="countDownCtrl" disabled ></label>';
        functionsList.push(function () { $('#' + this.id).initCountDown(); }.bind({ id: ctrl.id }));
    }

    return result;
}

function getButtonTemplateWidthLabel(ctrl) {
    var result = '';

    result += '<div class="form-group" >'
    result += getButtonTemplate(ctrl);
    result += '</div>';

    return result;
}

function getPlaqueTemplate(ctrl) {
    var result = '';
    if (ctrl) {
        if (ctrl.label) {
            result += '<label style="display:block;"  >' + ctrl.label + (ctrl.isRequired ? '<span style="color:red" >*</span>' : '') + '</label>';
        }
        result += '<div class="plaqueCtrl form-group" >';
        result += '<div class="plaqueRightPart" >';
        result += '<div class="plaqueRightPartRight">';
        result += '<input type="text" placeholder="55" name="' + ctrl.name + '_1" />';
        result += '</div>';
        result += '<div class="plaqueRightPartLeft">';
        result += '<input type="text" placeholder="555" name="' + ctrl.name + '_2" />';
        result += getPlaqueDropdownItem(ctrl);
        result += '<input type="text" placeholder="55" name="' + ctrl.name + '_4" />';
        result += '</div>';
        result += '</div>';
        result += '<div class="plaqueLeftPart" >';
        result += '<img width="19" height="11" src="/Modules/Images/iran.png" />';
        result += '<div>I.R</div>';
        result += '<div>Iran</div>';
        result += '</div>';
        result += '</div>';
    }
    return result;
}

function getPlaqueDropdownItem(ctrl) {
    var result = '';

    result += '<select style="vertical-align:top" class="fa" name="' + ctrl.name + '_2" >';
    result += '<option value="الف">الف</option>';
    result += '<option class="fa" value="accessible"></option>';
    result += '<option value="ب">ب</option>';
    result += '<option value="پ">پ</option>';
    result += '<option value="ت">ت</option>';
    result += '<option value="ث">ث</option>';
    result += '<option value="ج">ج</option>';
    result += '<option value="ح">ح</option>';
    result += '<option value="د">د</option>';
    result += '<option value="ر">ر</option>';
    result += '<option value="ز">ز</option>';
    result += '<option value="ژ">ژ</option>';
    result += '<option value="س">س</option>';
    result += '<option value="ش">ش</option>';
    result += '<option value="ص">ص</option>';
    result += '<option value="ض">ض</option>';
    result += '<option value="ط">ط</option>';
    result += '<option value="ظ">ظ</option>';
    result += '<option value="ع">ع</option>';
    result += '<option value="ف">ف</option>';
    result += '<option value="ق">ق</option>';
    result += '<option value="ک">ک</option>';
    result += '<option value="گ">گ</option>';
    result += '<option value="ل">ل</option>';
    result += '<option value="م">م</option>';
    result += '<option value="ن">ن</option>';
    result += '<option value="و">و</option>';
    result += '<option value="ه">ه</option>';
    result += '<option value="ی">ی</option>';
    result += '</select>';

    return result;
}

function getShortcutButtonTemplate(ctrl) {
    var result = '';

    if (ctrl.url && ctrl.label) {
        result += '<a style="margin-bottom:10px;" class="btn btn-primary btn-block" href="' + ctrl.url + '" >' + ctrl.label + '</a>';
    }

    return result;
}

function getContentTemplate(ctrl) {
    var result = '';

    if (ctrl.url && ctrl.id) {
        result += '<div id="' + ctrl.id + '" ></div>';
        functionsList.push(function () {
            loadJsonConfig(this.ctrl.url, this.ctrl.id);
        }.bind({ ctrl: ctrl }));
    }

    return result;
}

function getMultiRowInputTemplate(ctrl) {
    var result = '';

    if (ctrl) {
        result += '<div id="' + ctrl.id + '" class="myCtrl row ' + (ctrl.class ? ctrl.class : '') + '" data-name="' + ctrl.name + '" >';
        result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">';
        if (ctrl.ctrls) {
            result += '<div class="MultiRowInputRow row">';
            for (var i = 0; i < ctrl.ctrls.length; i++) {
                var prevName = ctrl.ctrls[i].name;
                ctrl.ctrls[i].name = ctrl.name + '[0].' + ctrl.ctrls[i].name;
                result += getInputTemplate(ctrl.ctrls[i]);
                ctrl.ctrls[i].name = prevName;
            }
            result += '</div>';
        }
        result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12" style="text-align:center;" ><button style="margin:3px;" class="btn btn-primary btn-sm addNewRowForMultiRowCtrl">افزودن</button><button style="margin:3px;" class="btn btn-danger btn-sm deleteNewRowForMultiRowCtrl">حذف اخرین سطر</button></div>';
        result += '</div>';
        result += '</div>';

        functionsList.push(function () {
            initMoultiRowInputButton(this.ctrl);
            initInternalFunctions(this.ctrl);
        }.bind({ ctrl: ctrl }));
    }

    return result;
}

function initInternalFunctions(ctrl) {
    var qureSelector = $('#' + ctrl.id);
    if (qureSelector.length > 0) {
        qureSelector[0].addNewRowIfNeeded = function (index) {
            var currWo = $(this).find('.MultiRowInputRow').length;
            if (currWo < index)
                $(this).find('.addNewRowForMultiRowCtrl').click();
        }
    }
}

function initMoultiRowInputButton(ctrl) {
    var addNewButtonQuery = $('#' + ctrl.id).find('button.addNewRowForMultiRowCtrl');
    if (addNewButtonQuery.length > 0) {
        addNewButtonQuery[0].ctrl = ctrl;
        addNewButtonQuery.click(function () {
            var ctrlObj = $(this)[0].ctrl;
            if (ctrlObj.ctrls) {
                var countExist = $(this).closest('.myCtrl').find('.MultiRowInputRow').length;
                var newRowHtml = '<div class="MultiRowInputRow row">';
                for (var i = 0; i < ctrlObj.ctrls.length; i++) {
                    ctrlObj.ctrls[i].id = uuidv4RemoveDash();
                    var prevName = ctrlObj.ctrls[i].name;
                    ctrlObj.ctrls[i].name = ctrlObj.name + '[' + (countExist) + '].' + ctrlObj.ctrls[i].name;
                    newRowHtml += getInputTemplate(ctrlObj.ctrls[i]);
                    ctrlObj.ctrls[i].name = prevName;
                }
                newRowHtml += '</div>';
                $(this).closest('.myCtrl').find('.MultiRowInputRow:last').after(newRowHtml);
            }
            executeArrFunctions();
        });
    }

    var deleteNewButtonQuery = $('#' + ctrl.id).find('button.deleteNewRowForMultiRowCtrl');
    if (deleteNewButtonQuery.length > 0) {
        deleteNewButtonQuery[0].ctrl = ctrl;
        deleteNewButtonQuery.click(function () {
            var ctrlObj = $(this)[0].ctrl;
            if (ctrlObj.ctrls) {
                var countExist = $(this).closest('.myCtrl').find('.MultiRowInputRow').length;
                if (countExist <= 1) {
                    alert('امکان حذف وجود ندارد');
                    return;
                }
                $(this).closest('.myCtrl').find('.MultiRowInputRow:last').remove();
            }
        });
    }
}

function getCheckboxButtonTemplate(ctrl) {
    var result = '';

    if (ctrl) {
        result += '<div class="form-check form-switch myCtrl">';
        result += '<input class="form-check-input" name="' + ctrl.name + '" type="checkbox" value="' + (ctrl.defValue ? ctrl.defValue : 'true') + '" id="' + ctrl.id + '" />';
        result += '<label style="padding-right:20px;" class="form-check-label" for="' + ctrl.id + '">' + ctrl.label + '</label>';
        result += '</div>';
    }

    return result;
}

function getRadioButtonTemplate(ctrl) {
    var result = '';
    if (ctrl) {
        result += '<div class="myCtrl form-group ' + (ctrl.class ? ctrl.class : '') + '"' + '>';
        if (ctrl.label)
            result += '<label style="display:block" >' + ctrl.label + (ctrl.isRequired ? '<span style="color:red" >*</span>' : '') + '</label>';
        if (ctrl.values && ctrl.values.length > 0) {
            var idList = [];
            for (var i = 0; i < ctrl.values.length; i++) {
                var id = ctrl.values[i].oId ? ctrl.values[i].oId : uuidv4RemoveDash();
                idList.push(id);
                result += '<div class="form-check form-check-inline">';
                result += '<input ' + (i == 0 ? 'checked="checked"' : '') + ' class="form-check-input" name="' + ctrl.name + '" id="' + id + '" type="radio" value="' + ctrl.values[i][ctrl.valuefield] + '" />';
                result += '<label class="form-check-label" for="' + id + '" >' + ctrl.values[i][ctrl.textfield] + '</label>';
                result += '</div>';
            }
            ctrl.idList = idList;
        }
        result += '</div>';
        if (ctrl.showHideCondation) {
            functionsList.push(function () {
                var idList = this.idList;
                if (idList) {
                    for (var i = 0; i < idList.length; i++) {
                        $('#' + idList[i])[0].showHideCondation = this.showHideCondation;
                        $('#' + idList[i]).change(function () {
                            var showHideCondation = $(this)[0].showHideCondation;
                            var isChecked = $(this).is(':checked');
                            var curValue = isChecked == true ? $(this).val() : '';
                            showAndHideCtrl(curValue, showHideCondation, $(this).attr('id'));
                        });
                        showAndHideCtrl('', this.showHideCondation, this.id);
                    }
                }

            }.bind({ id: ctrl.id, showHideCondation: ctrl.showHideCondation, idList: ctrl.idList }));
        }
    }
    return result;
}

function getDynamicCtrlsTemplate(ctrl) {
    var result = '';

    if (ctrl && ctrl.id && ctrl.dataurl) {
        result += '<div class="row" id="' + ctrl.id + '" ></div>';
    }

    functionsList.push(function () {
        if (this.ctrl.dataurl) {
            var allTargetCtrl = this.ctrl.otherCtrls
            if (allTargetCtrl && allTargetCtrl.length > 0) {

                for (var i = 0; i < allTargetCtrl.length; i++) {
                    if ($('#' + allTargetCtrl[i]).length > 0) {
                        $('#' + allTargetCtrl[i])[0].ctrlObj = this;
                        $('#' + allTargetCtrl[i]).change(function () {
                            updateDynamicCtrls($(this)[0].ctrlObj);
                        });
                    }
                }
            }
            else {
                updateDynamicCtrls(this);
            }

        }
    }.bind({ ctrl: ctrl }));

    return result;
}

function updateDynamicCtrls(curThis) {
    var formData = new FormData();
    if (curThis.ctrl.otherCtrls) {
        for (var i = 0; i < curThis.ctrl.otherCtrls.length; i++) {
            var qure = $('#' + curThis.ctrl.otherCtrls[i]);
            formData.append(qure.attr('name'), qure.val());
        }
    }
    postForm(curThis.ctrl.dataurl, formData, function (res) {
        if ($('#' + this.ctrl.id).length > 0) {
            var htmlResult = '';

            if (res && res.panels && res.panels.length > 0) {
                for (var i = 0; i < res.panels.length; i++) {
                    htmlResult += getPanelTemplate(res.panels[i]);
                }
            }

            $('#' + this.ctrl.id).html(htmlResult);
            executeArrFunctions();
        }

    }.bind({ ctrl: curThis.ctrl }));
}

function getDynamicFileUploadDependCtrlTemplate(ctrl) {
    var result = '';

    if (ctrl) {
        result += '<div id="' + ctrl.id + '" data-spec-name="' + ctrl.name + '" class="debitFileUploader row"></div>';
    }

    functionsList.push(function () {
        if ($('#' + this.ctrl.id).length > 0) {
            $('#' + this.ctrl.id)[0].addNewRow = function (dataItems) {
                var arrHtml = '';
                if (dataItems) {
                    for (var i = 0; i < dataItems.length; i++) {
                        arrHtml += '<div class="col-md-3 col-sm-6 col-xs-12 col-lg-3">';
                        arrHtml += getFileCTRLTemplate({
                            label: dataItems[i][this.ctrl.schema.title],
                            type: 'file',
                            name: dataItems[i][this.ctrl.schema.name].replace(/ /g, ''),
                            acceptEx: this.ctrl.acceptEx,
                            isRequired: dataItems[i][this.ctrl.schema.isRequired],
                            sampleUrl: dataItems[i][this.ctrl.schema.sampleUrl]
                        });
                        arrHtml += '</div>';
                    }
                }
                $('#' + this.ctrl.id).html(arrHtml);
                executeArrFunctions();
            }.bind({ ctrl: this.ctrl });
        }
    }.bind({ ctrl: ctrl }));

    return result;
}

function getChkCtrlBoxTemplateCtrl(ctrl) {
    var result = '';

    if (ctrl) {
        result += '<div id="' + ctrl.id + '" data-spec-name="' + ctrl.name + '" class="checkCtrl row"></div>';
    }

    functionsList.push(function () {
        if ($('#' + this.ctrl.id).length > 0) {
            $('#' + this.ctrl.id)[0].addNewRow = function (dataItems) {
                var arrHtml = '';
                if (dataItems) {
                    for (var i = 0; i < dataItems.length; i++) {
                        arrHtml += '<div class="col-md-3 col-sm-6 col-xs-12 col-lg-3">';
                        arrHtml += getDropdownCTRLTemplate(
                            {
                                "parentCL": "col-md-3 col-sm-6 col-xs-12 col-lg-3",
                                "id": "checkBankId" + i,
                                "type": "dropDown",
                                "textfield": "title",
                                "valuefield": "id",
                                "dataurl": this.ctrl.bankUrl,
                                "label": "بانک",
                                "name": 'check[' + i + '].bankId',
                                "isRequired": true
                            });
                        arrHtml += '</div>';
                        arrHtml += '<div class="col-md-3 col-sm-6 col-xs-12 col-lg-3">';
                        arrHtml += getTextBoxTemplate(
                            {
                                "parentCL": "col-md-3 col-sm-6 col-xs-12 col-lg-3",
                                "id": "checkNumber" + i,
                                "name": 'check[' + i + '].checkNumber',
                                "type": "text",
                                "label": "شماره چک",
                                "isRequired": true
                            });
                        arrHtml += '</div>';
                        arrHtml += '<div class="col-md-3 col-sm-6 col-xs-12 col-lg-3">';
                        arrHtml += getTextBoxTemplate(
                            {
                                "parentCL": "col-md-3 col-sm-6 col-xs-12 col-lg-3",
                                "id": "checkDate" + i,
                                "name": 'check[' + i + '].checkDate',
                                "type": "text",
                                "label": "تاریخ چک",
                                "disabled": true,
                                "dfaultValue": dataItems[i][this.ctrl.schema.date]
                            });
                        arrHtml += '</div>';
                        arrHtml += '<div class="col-md-3 col-sm-6 col-xs-12 col-lg-3">';
                        arrHtml += getTextBoxTemplate(
                            {
                                "parentCL": "col-md-3 col-sm-6 col-xs-12 col-lg-3",
                                "id": "checkPrice" + i,
                                "name": 'check[' + i + '].checkPrice',
                                "type": "text",
                                "label": "مبلغ چک",
                                "disabled": true,
                                "dfaultValue": dataItems[i][this.ctrl.schema.price]
                            });
                        arrHtml += '</div>';
                    }
                }
                $('#' + this.ctrl.id).html(arrHtml);
                executeArrFunctions();
            }.bind({ ctrl: this.ctrl });
        }
    }.bind({ ctrl: ctrl }));

    return result;
}

function appendAllQueryStringToForm(formData) {
    var allParams = new URLSearchParams(window.location.search);
    for (var pair of allParams.entries()) {
        formData.append(pair[0], pair[1]);
    }
}

function getDynamicFileUploadCtrlTemplate(ctrl) {
    var result = '';

    if (ctrl.parentCL && ctrl.url && ctrl.schema) {
        result += '<div class="row" id="' + ctrl.id + '" ></div>';
        functionsList.push(function () {
            if (this.ctrl.url) {
                var formData = new FormData();
                appendAllQueryStringToForm(formData);
                postForm(this.ctrl.url, formData, function (res) {
                    if (res && res.length > 0) {
                        if (this.ctrl.schema) {
                            var template = '';
                            for (var i = 0; i < res.length; i++) {
                                template += '<div class="' + this.ctrl.css + '">';
                                template += getFileCTRLTemplate({
                                    label: res[i][this.ctrl.schema.title],
                                    type: 'file',
                                    name: res[i][this.ctrl.schema.name].replace(/ /g, ''),
                                    acceptEx: this.ctrl.acceptEx,
                                    isRequired: res[i][this.ctrl.schema.isRequired],
                                    sampleUrl: res[i][this.ctrl.schema.sampleUrl]
                                });
                                template += '</div>';
                            }
                            $('#' + this.ctrl.id).html(template);
                        }
                    }
                }.bind({ ctrl: this.ctrl }));
            }
        }.bind({ ctrl: ctrl }));
    }

    return result;
}

function getTextBoxTemplate(ctrl) {
    if (!ctrl.id)
        ctrl.id = uuidv4RemoveDash();
    var result = '';
    result += '<div class="myCtrl form-group ' + (ctrl.class ? ctrl.class : '') + '">';
    if (ctrl.label) {
        result += '<label for="' + ctrl.id + '"  >' + ctrl.label + (ctrl.isRequired ? '<span style="color:red" >*</span>' : '') + '</label>';
    }
    result += '<input autocomplete="off" ' + (ctrl.type == "persianDateTime" ? 'data-jdp ' : ' ') + getCtrlValidationAttribute(ctrl) + ' ' + (ctrl.disabled ? 'disabled="disabled"' : '') + ' ' + (ctrl.ph ? 'placeholder="' + ctrl.ph + '"' : '') + ' ' + (ctrl.id ? 'id="' + ctrl.id + '"' : '') + '" ' + (ctrl.dfaultValue ? 'value="' + ctrl.dfaultValue + '"' : '') + ' name="' + ctrl.name + '" class="form-control" />';
    result += '</div>';

    functionsList.push(function () {
        setTimeout(function ()
        {
            $('#' + this.id).attr('type', (this.type == 'persianDateTime' ? 'text' : this.type));
        }.bind(this), 300);
        inputNewLabelEventHandler(this.id);
    }.bind({ id: ctrl.id, type: ctrl.type }));

    if (ctrl.onEnter && ctrl.id) {
        functionsList.push(function () {
            var enterStr = this.onEnter;
            $('#' + this.id).keypress(function (keyEvent) {
                if (keyEvent.keyCode == 13) {
                    eval(enterStr);
                }
            });
        }.bind({ id: ctrl.id, onEnter: ctrl.onEnter }));
    }
    if (ctrl.seperator) {
        functionsList.push(function () {
            $('#' + this.id).seleperator();
        }.bind({ id: ctrl.id }));
    }
    if (ctrl.type == "persianDateTime") {
        functionsList.push(function () {
            setTimeout(function () {
                jalaliDatepicker.startWatch({
                    separatorChar: "/",
                    changeMonthRotateYear: true,
                    showTodayBtn: true,
                    showEmptyBtn: true
                });
            }.bind({ id: this.id }), 100);
        }.bind({ id: ctrl.id }));
    }
    if (ctrl.multiPlay) {
        functionsList.push(function () {
            for (var i = 0; i < this.ctrl.multiPlay.length; i++) {
                if ($('#' + this.ctrl.multiPlay[i]).length > 0) {
                    $('#' + this.ctrl.multiPlay[i])[0].multiPlayObj = this.ctrl;
                    $('#' + this.ctrl.multiPlay[i]).change(function () {
                        doCalceForMultiplay($(this)[0].multiPlayObj);
                    });
                }
            }
        }.bind({ ctrl: ctrl }));
    }


    return result;
}

function changeInputValueSetter(objectId) {
    var input = document.getElementById(objectId);
    var descriptor = Object.getOwnPropertyDescriptor(Object.getPrototypeOf(input), 'value');
    Object.defineProperty(input, 'value', {
        set: function (t) {
            var m = descriptor.set.apply(this, arguments);
            $(this).change();
            return m;
        },
        get: function () {
            return descriptor.get.apply(this);
        }
    });
}

function doCalceForMultiplay(multiPlayObj) {
    if (multiPlayObj && multiPlayObj.multiPlay) {
        var result = 0;
        for (var i = 0; i < multiPlayObj.multiPlay.length; i++) {
            try {
                if (result == 0)
                    result = Number.parseInt($('#' + multiPlayObj.multiPlay[i]).val().replace(/,/, ''));
                else
                    result = result * Number.parseInt($('#' + multiPlayObj.multiPlay[i]).val().replace(/,/, ''));
            }
            catch (e) {

            }
        }
        if (isNaN(result))
            result = 0;
        $('#' + multiPlayObj.id).val(postCommanInNumberPlugin(result));
    }
}
var isLoadCkEditorTrying = false;
function getCkEditorTemplate(ctrl) {
    var result = '';

    result += getTextAreaTemplate(ctrl);
    if (result) {
        functionsList.push(function () {
            if (isLoadCkEditorTrying == false) {
                loadJS('/Modules/Core/js/ce.js.gz');
                isLoadCkEditorTrying = true;
            }
        });
    }

    return result;
}

function inputNewLabelEventHandler(curId) {
    changeInputValueSetter(curId);
    $('#' + curId).focus(function () {
        makeCtrlFocused(this);
    });
    $('#' + curId).blur(function () {
        setTimeout(function () {
            if (!$(this).val()) {
                makeCtrlBlure(this);
            }
        }.bind(this), 150);
    });
    $('#' + curId).change(function () {
        if (!$(this).val()) {
            makeCtrlBlure(this);
        } else {
            makeCtrlFocused(this);
        }
    });
    if ($('#' + curId).val()) {
        makeCtrlFocused($('#' + curId));
    }
    $('#' + curId)[0].updateStatus = function () {
        if (!$(this).val()) {
            makeCtrlBlure(this);
        } else {
            makeCtrlFocused(this);
        }
    };
}

function loadJS(src) {
    var script = document.createElement("script");
    script.setAttribute("type", "text/javascript");
    script.setAttribute("src", src);
    document.getElementsByTagName("head")[0].appendChild(script);
}

function getTextAreaTemplate(ctrl) {
    var result = '';
    result += '<div class="myCtrl form-group ' + (ctrl.type == 'ck' ? 'makeLabelBGSilver' : '') +'">';
    if (ctrl.label) {
        result += '<label  >' + ctrl.label + (ctrl.isRequired ? '<span style="color:red" >*</span>' : '') + '</label>';
    }
    var id = (ctrl.id ? ctrl.id : uuidv4RemoveDash());
    if (!ctrl.id)
        ctrl.id = id;
    result += '<textarea id="' + ctrl.id + '" autocomplete="off" ' + getCtrlValidationAttribute(ctrl) + ' ' + (ctrl.ph ? 'placeholder="' + ctrl.ph + '"' : '') + ' ' + (ctrl.id ? 'id="' + ctrl.id + '"' : '') + ' type="' + ctrl.type + '" name="' + ctrl.name + '" class="form-control ' + (ctrl.type == 'ck' ? 'ckEditor' : '') + '" ></textarea>';
    result += '</div>';

    functionsList.push(function () {
        inputNewLabelEventHandler(this.id);
    }.bind({ id: ctrl.id }));

    return result;
}


function getFileCTRLTemplate(ctrl) {
    var result = '';

    if (!ctrl.id)
        ctrl.id = uuidv4RemoveDash();

    result += '<div class="myCtrl form-group myFileUpload">';

    if (ctrl.label) {
        result += '<label class="btn btn-primary btn-block" style="margin-bottom:0px;text-align:center;" for="file_' + ctrl.id + '" >' + ctrl.label + (ctrl.isRequired ? '<span style="color:red" >*</span>' : '') + '<a target="_blank" ' + (ctrl.sampleUrl ? 'href="' + ctrl.sampleUrl + '"' : '') + ' style="margin-left:5px;margin-right:5px;font-size:8pt;" data-name="' + ctrl.name + '_address" >' + (ctrl.sampleUrl ? '(مشاهده)' : '') + '</a></label>';
    }
    result += '<input id="file_' + ctrl.id + '" ' + getCtrlValidationAttribute(ctrl) + ' ' + (ctrl.acceptEx ? 'accept="' + ctrl.acceptEx + '"' : '') + (ctrl.id ? 'id="' + ctrl.id + '"' : '') + ' type="' + ctrl.type + '" name="' + ctrl.name + '" class="form-control" />';
    result += '</div>';

    return result;
}

function getTokennCTRLTemplate(ctrl) {
    if (ctrl.type == 'tokenBox')
        ctrl.type = 'dropDown';
    if (ctrl.type == 'tokenBox2')
        ctrl.type = 'dropDown2';

    if (!ctrl.class)
        ctrl.class = 'tokenBox';
    else
        ctrl.class += ' tokenBox';

    ctrl.moveNameToParent = true;

    var result = getDropdownCTRLTemplate(ctrl);

    functionsList.push(function () { initTokenBox($('#' + this.id).closest('.tokenBox')); }.bind({ id: ctrl.id }));

    return result;
}

function getDropdownCTRLTemplate(ctrl) {
    var result = '';

    if (!ctrl.id)
        ctrl.id = uuidv4RemoveDash();

    result += '<div id="myCtrlDivHolder' + ctrl.id + '" class="myCtrl ' + (ctrl.type == 'dropDown' && !ctrl.moveNameToParent ? 'myDropdown myDropdownHeight' : '') + ' form-group ' + (ctrl.class ? ctrl.class : '') + '"' + (ctrl.moveNameToParent ? ' name="' + ctrl.name + '"' : '') + '>';
    if (ctrl.label) {
        result += '<label  >' + ctrl.label + (ctrl.isRequired ? '<span style="color:red" >*</span>' : '') + '</label>';
    }
    result += '<select ' + (ctrl.disabled ? 'disabled="disabled"' : '') + ' ' + getCtrlValidationAttribute(ctrl) + ' ' + (ctrl.bindFormUrl ? ('bindFormUrl=' + ctrl.bindFormUrl) : '') + ' style="width: 100%" ' + (ctrl.dataS2 ? 'data-s2="true"' : '') + '  id="' + ctrl.id + '"  data-valuefield="' + ctrl.valuefield + '" data-textfield="' + ctrl.textfield + '" data-url2="' + (ctrl.dataurl ? ctrl.dataurl : '') + '" data-url="' + (ctrl.dataurl ? ctrl.dataurl : '') + '" ' + (!ctrl.moveNameToParent ? 'name="' + ctrl.name + '"' : '') + ' class="form-control" >';
    if (ctrl.values && ctrl.values.length > 0) {
        for (var i = 0; i < ctrl.values.length; i++) {
            result += '<option value="' + ctrl.values[i][ctrl.valuefield] + '" >' + ctrl.values[i][ctrl.textfield] + '</option>';
        }
    }
    result += '</select>';
    if (ctrl.type == 'dropDown' && !ctrl.moveNameToParent) {
        result += '<div class="myDropdownText myDropdownHeight" ><span class="myDropdownHeight"></span><i ></i></div>';
        result += '<div id="myCtrlDivHolder' + ctrl.id + '_HItems" class="myDropdownItems"></div>';
    }
    result += '</div>';

    if (ctrl.type == 'dropDown')
        functionsList.push(function () {
            if (!this.moveNameToParent)
                $('#' + this.id).closest('.myDropdown').initMyDropdown();
            initDropdown($('#' + this.id));
        }.bind({ id: ctrl.id, moveNameToParent: ctrl.moveNameToParent }));
    else {
        functionsList.push(function () {
            var exteraSelect2Parameters = {};

            if (window['exteraModelParams'])
                for (var prop in exteraModelParams)
                    exteraSelect2Parameters[prop] = exteraModelParams[prop];

            var select2Option = {
                placeholder: ctrl.ph ? ctrl.ph : "",
                ajax: {
                    url: this.dataurl,
                    data: function (params) {
                        addExteraParameterFromCtrls(this.exteraParameterIds, this.exParam)
                        this.exParam.search = params.term;
                        this.exParam.page = params.page || 1;
                        return this.exParam;
                    }.bind({ exParam: exteraSelect2Parameters, exteraParameterIds: this.exteraParameterIds })
                }
            };
            $('#' + this.id).select2(select2Option);
            if (this.exteraParameterIds) {
                for (var i = 0; i < this.exteraParameterIds.length; i++) {
                    $('#' + this.exteraParameterIds).change(function () {
                        var s2Obj = $('#' + this.id).data('select2');
                        if (s2Obj) {
                            s2Obj.val(['']);
                            s2Obj.trigger('change');
                        }
                    }.bind({ id: this.id }));
                }
            }
        }.bind({ id: ctrl.id, dataurl: ctrl.dataurl, exteraParameterIds: ctrl.exteraParameterIds }));
        functionsList.push(function ()
        {
            var querySelector = $('#' + this.id);
            $(querySelector)[0].updateStatus = function () {
                if (!$(this).val()) {
                    makeCtrlBlure(this);
                } else {
                    makeCtrlFocused(this);
                }
            };
            if (querySelector.attr('data-select2-id')) {
                if ($(querySelector).data('select2')) {
                    var curThis = querySelector;
                    $(querySelector).on('select2:open', function () {
                        makeCtrlFocused(curThis);
                    });
                    $(querySelector).on('select2:close', function () {
                        if (!$(curThis).data('select2').val())
                            makeCtrlBlure(curThis);
                    });
                }
            }
        }.bind({ id: ctrl.id}));
    }
       

    initDropDownExteraFunctions(ctrl);

    return result;
}

function addExteraParameterFromCtrls(exteraParameterIds, exteraSelect2Parameters) {
    if (exteraParameterIds && exteraParameterIds.length > 0) {
        for (var i = 0; i < exteraParameterIds.length; i++) {
            var qureSelect = $('#' + exteraParameterIds[i]);
            if (qureSelect.length > 0) {
                exteraSelect2Parameters[qureSelect.attr('name')] = qureSelect.val();
            }
        }
    }
}

function bindExteraDropDown(res, ctrlCss, targetCtrlId) {
    if (res && res.length > 0) {
        var ctrlHtmls = '';
        for (var i = 0; i < res.length; i++) {
            ctrlHtmls += '<div class="' + ctrlCss + ' dynamicCtrlsHolder">';
            ctrlHtmls += getDropdownCTRLTemplate({
                "id": "requiredDD" + res[i].id,
                "type": "dropDown",
                "textfield": "title",
                "valuefield": "id",
                "label": res[i].title,
                "name": "dynamicCTRLs",
                "isRequired": true,
                "values": res[i].values
            });
            ctrlHtmls += '</div>';
        }
        if (ctrlHtmls && $('#' + targetCtrlId).length > 0) {
            $('#' + targetCtrlId).closest('.myPanel').find('.dynamicCtrlsHolder').remove();
            $('#' + targetCtrlId).closest('.myCtrl').parent().after(ctrlHtmls);
            executeArrFunctions();
        }
    }
}

function changeAllLabel(changeOtherCtrLabel, curName, curValue) {
    if (curName && curValue) {
        for (var i = 0; i < changeOtherCtrLabel.length; i++) {
            var curConfig = changeOtherCtrLabel[i];
            if (curConfig.url && curConfig.targetCtrlId && curConfig.titleSchema) {
                var postData = new FormData();
                postData.append(curName, curValue);
                postForm(curConfig.url, postData, function (res) {
                    if (res && this.curConfig.titleSchema && $('#' + this.curConfig.targetCtrlId).length > 0) {
                        var targetQuery = $('#' + this.curConfig.targetCtrlId).closest('.myCtrl').find('label');
                        var curHtml = targetQuery.html();
                        var hasRequred = false;
                        if (curHtml && curHtml.trim().indexOf('<span style="color:red">*</span>') > -1)
                            hasRequred = true;
                        targetQuery.html(res[this.curConfig.titleSchema] + (hasRequred == true ? '<span style="color:red">*</span>' : ''));
                    }
                }.bind({ curConfig: curConfig }));
            }
        }
    }
}

function initDropDownExteraFunctions(ctrl) {
    if (ctrl.changeOtherCtrLabel && ctrl.changeOtherCtrLabel.length > 0) {
        functionsList.push(function () {
            if ($('#' + ctrl.id).length > 0) {
                var curCtrl = this.ctrl;
                $('#' + ctrl.id).change(function () {
                    var curValue = $(this).val();
                    var curName = $(this).attr('name');
                    changeAllLabel(curCtrl.changeOtherCtrLabel, curName, curValue);
                });
            }
        }.bind({ ctrl: ctrl }));
    }
    if (ctrl.bindFormUrl) {
        functionsList.push(function () {
            if ($('#' + this.ctrl.id).length > 0) {
                $('#' + this.ctrl.id)[0].ctrl = this.ctrl;
                $('#' + this.ctrl.id).change(function () {
                    var currCtrl = $(this)[0].ctrl;
                    if (currCtrl) {
                        if (currCtrl.bindFormUrl) {
                            var curId = $(this).find('option:selected').attr('value');
                            var formData = new FormData();
                            formData.append('id', curId);
                            if (window['exteraModelParams'])
                                for (var prop in exteraModelParams)
                                    formData.append(prop, exteraModelParams[prop]);
                            var formQuery = $(this).closest('.myPanel');
                            showLoader(formQuery);
                            postForm(currCtrl.bindFormUrl, formData, function (res) {
                                bindForm(res, $(this.curThis).closest('.myPanel'));
                            }.bind({ curThis: this }), null, function () { hideLoader(formQuery); });
                        }
                    }
                });
            }
        }.bind({ ctrl: ctrl }));
    }
    if (ctrl.dynamicCtrlConfig) {
        functionsList.push(function () {
            if ($('#' + this.ctrl.id).length > 0) {
                $('#' + this.ctrl.id)[0].dynamicCtrlConfig = this.ctrl.dynamicCtrlConfig;
                $('#' + this.ctrl.id).change(function () {
                    var dynamicCtrlConfig = $(this)[0].dynamicCtrlConfig;
                    if (dynamicCtrlConfig) {
                        var url = dynamicCtrlConfig.dataurl;
                        if (url && dynamicCtrlConfig.ctrlIds && dynamicCtrlConfig.ctrlIds.length > 0) {
                            var postFormData = new FormData();
                            for (var i = 0; i < dynamicCtrlConfig.ctrlIds.length; i++) {
                                postFormData.append(dynamicCtrlConfig.ctrlIds[i].postName, $('#' + dynamicCtrlConfig.ctrlIds[i].ctrlId).val());
                            }
                            postForm(url, postFormData, function (res) {
                                bindExteraDropDown(res, dynamicCtrlConfig.css, dynamicCtrlConfig.targetCtrlId);
                            });
                        }
                    }
                });
            }
        }.bind({ ctrl: ctrl }));
    }
    if (ctrl.showHideCondation) {
        functionsList.push(function () {
            $('#' + this.id)[0].showHideCondation = this.showHideCondation;
            $('#' + this.id).change(function () {
                var showHideCondation = $(this)[0].showHideCondation;
                var curValue = $(this).find('option:selected').attr('value');
                showAndHideCtrl(curValue, showHideCondation, $(this).attr('id'));
            });
            showAndHideCtrl('', this.showHideCondation, this.id);
        }.bind({ id: ctrl.id, showHideCondation: ctrl.showHideCondation }));
    }

    if (ctrl.targetDropDown) {
        functionsList.push(function () {
            $('#' + this.id)[0].targetDropDown = this.targetDropDown;
            $('#' + this.id).change(function () {
                var targetDropDown = $(this)[0].targetDropDown;
                var curValue = $(this).find('option:selected').attr('value');
                for (var i = 0; i < targetDropDown.length; i++) {
                    $('#' + targetDropDown[i])[0].resData = null;
                    initDropdown($('#' + targetDropDown[i]), true, curValue);
                }
            });
        }.bind({ id: ctrl.id, targetDropDown: ctrl.targetDropDown }));
    }

    if (ctrl.callChangeEvents) {
        functionsList.push(function () {
            $('#' + this.id)[0].callChangeEvents = this.callChangeEvents;
            $('#' + this.id).change(function () {
                var callChangeEvents = $(this)[0].callChangeEvents;
                for (var i = 0; i < callChangeEvents.length; i++) {
                    $('#' + callChangeEvents[i]).change();
                }
            });
        }.bind({ id: ctrl.id, callChangeEvents: ctrl.callChangeEvents }));
    }

    if (ctrl.childId) {
        functionsList.push(function () {
            setTimeout(function () {
                $('#' + this.id).change(function () {
                    var selectValue = $('#' + this.id).find('option:selected').attr('value');
                    if (selectValue) {
                        var url = $('#' + this.childId).attr('data-url2');
                        $('#' + this.childId).attr('data-url', url + '?id=' + selectValue);
                        $('#' + this.childId)[0].resData = null;
                        initDropdown($('#' + this.childId));
                    } else {
                        $('#' + this.childId).html('<option value=""></option>');
                        $('#' + this.childId).closest('.myDropdown')[0].updateItemFromSelect();
                    }

                }.bind({ childId: this.childId, id: this.id }));
            }.bind({ id: this.id, childId: this.childId }), 1000);

        }.bind({ id: ctrl.id, childId: ctrl.childId }));
    }
}

function showAndHideCtrl(curValue, showHideCondation, curCtrlId) {
    var defValue = showHideCondation.filter(function (curItem) { return curItem.isDefault == true; });
    if (defValue.length > 0)
        defValue = defValue[0];
    else
        defValue = null;
    var selectedValue = showHideCondation.filter(function (curItem) { return curItem.value == curValue; });
    if (selectedValue.length > 0)
        selectedValue = selectedValue[0];
    else
        selectedValue = null;
    var notSelectedValue = showHideCondation.filter(function (curItem) { return curItem.value && curItem.value.substr(0, 1) == '!' ? (curItem.value.replace('!', '') != curValue) : false; });
    if (notSelectedValue.length > 0)
        notSelectedValue = notSelectedValue[0];
    else
        notSelectedValue = null;

    if (selectedValue) {
        showAndHideByClass(selectedValue, curCtrlId);
    } else if (notSelectedValue) {
        showAndHideByClass(notSelectedValue, curCtrlId);
    } else if (defValue) {
        showAndHideByClass(defValue, curCtrlId);
    }
}

function showAndHideByClass(classObj, curCtrlId) {
    var isCtrlVisible = $('#' + curCtrlId).is(':visible');
    var isParentVisible = $('#' + curCtrlId).closest('.myDropdown').length > 0 && $('#' + curCtrlId).closest('.myDropdown').is(':visible');
    if (classObj.classShow && classObj.classShow.length > 0 && (isCtrlVisible || isParentVisible)) {
        for (var i = 0; i < classObj.classShow.length; i++) {
            $('.' + classObj.classShow[i]).parent().css('display', 'block');
        }
    }
    if (classObj.classHide && classObj.classHide.length > 0 && (isCtrlVisible || isParentVisible)) {
        for (var i = 0; i < classObj.classHide.length; i++) {
            $('.' + classObj.classHide[i]).parent().css('display', 'none');
        }
    }
}

function getModualTemplateActionButton(modual) {
    var result = '';

    if (modual && modual.actions && modual.actions.length > 0) {
        result += '<div class="modal-footer">';
        for (var i = 0; i < modual.actions.length; i++) {
            result += getButtonTemplate(modual.actions[i]);
        }
        result += '</div>';
    }

    return result;
}

function getButtonTemplate(action) {
    return '<button onclick="' + action.onClick + '" type="button" class="btn ' + action.class + '" >' + action.title + '</button>';
}

function getStepWizardTemplate(wizard) {
    var result = '';

    if (wizard && wizard.steps && wizard.steps.length > 0) {

        if (!wizard.id)
            wizard.id = uuidv4RemoveDash();

        wizard.steps = wizard.steps.sort(function (a, b) { return a.order > b.order ? 1 : a.order < b.order ? -1 : 0; });

        result += '<div style="' + (wizard.hideBorder ? 'border:none;' : '') + '" class="panelSWizard" id="' + wizard.id + '">';
        result += '<div class="panelSWizardHolderHeader">';

        for (var i = 0; i < wizard.steps.length; i++) {


            var showCondation = '';
            if (wizard.steps[i].showCondation && wizard.steps[i].showCondation.length > 0)
                showCondation = 'showCondation=\'' + JSON.stringify(wizard.steps[i].showCondation) + '\'';
            result += '<span style="' + (wizard.headerTextalign ? 'text-align:' + wizard.headerTextalign + ';' : '') + '" id="step_' + wizard.steps[i].id + '" ' + showCondation + ' class="panelSWizardHolderHeaderItem ' + (i == 0 ? 'panelSWizardHolderHeaderItemActive' : '') + '">';
            if (wizard.steps[i].moveBackToStep) {
                result += '<div class="swMoveBackToStepButton" onclick="moveToStepById(\'stepContent_' + wizard.steps[i].moveBackToStep + '\')" ><i class="fa fa-arrow-right" ></i></div>';
            }
            result += wizard.steps[i].title;
            result += '</span>';
        }
        result += '</div>';
        result += '<div class="panelSWizardHolderContent">';
        for (var i = 0; i < wizard.steps.length; i++) {
            var showCondation = '';
            if (wizard.steps[i].showCondation && wizard.steps[i].showCondation.length > 0)
                showCondation = 'showCondation=\'' + JSON.stringify(wizard.steps[i].showCondation) + '\'';
            result += '<div data-submit-url="' + (wizard.steps[i].submitUrl ? wizard.steps[i].submitUrl : '') + '" id="stepContent_' + wizard.steps[i].id + '" ' + showCondation + ' class="panelSWizardHolderContentItem ' + (i == 0 ? 'panelSWizardHolderContentItemActive' : '') + '">';


            if (wizard.steps[i].panels && wizard.steps[i].panels.length > 0) {
                for (var j = 0; j < wizard.steps[i].panels.length; j++) {
                    result += getPanelTemplate(wizard.steps[i].panels[j]);
                }
            }

            if (!wizard.steps[i].submitUrl) {
                result += '<div class="panelSWizardHolderContentHolderStepButton">';
                if (i > 0)
                    result += '<button class="btn btn-warning btn-sm stepButton buttonBack">بازگشت</button>';
                var isLastStep = (i + 1) >= wizard.steps.length;
                result += '<button class="btn btn-primary btn-sm stepButton ' + (isLastStep ? 'lastStepButton' : 'buttonConfirm') + ' ">' + (isLastStep ? wizard.lastStepButtonTitle : (wizard.fistStepButtonTitle && i == 0 ? wizard.fistStepButtonTitle : 'ادامه')) + '</button>';
                result += '</div>';
            }


            result += '</div>';
        }
        result += '</div>';
    }

    functionsList.push(function () {
        initSWFunctions(this.wizard.id, this.wizard.actionOnLastStep);
        hideStepByRequest(wizard.steps);
    }.bind({ wizard: wizard }));

    return result;
}

function submitThisStep(curThis, targetUrl) {
    var quarySelector = $(curThis).closest('.panelSWizardHolderContentItem');
    if (quarySelector.length > 0) {
        var curUrl = quarySelector.attr('data-submit-url');
        if (targetUrl)
            curUrl = targetUrl;
        if (curUrl) {
            showLoader(quarySelector.find('.myPanel'));
            postForm(curUrl, getFormData(quarySelector), function (res) {
                if (res && res.data) {
                    if (res.data.stepId) {
                        moveToStepById('stepContent_' + res.data.stepId);
                        if (res.data.data) {
                            bindForm(res.data.data, $('#stepContent_' + res.data.stepId))
                        }
                        if (res.data.labels && res.data.labels.length > 0) {
                            changeInputLabels(res.data.labels, res.data.stepId);
                        }
                        if (res.data.countDownId) {
                            if ($('#' + res.data.countDownId).length > 0)
                                $('#' + res.data.countDownId)[0].start();
                        }
                    }
                    if (res.data.hideModal) {
                        $(this).closest('.modal').modal('hide');

                    }
                    if (res.data.userfullname && window['userLoginWeb']) {
                        userLoginWeb(res.data.userfullname);
                        if (window['showLoginUserPanelInMainPage']) {
                            showLoginUserPanelInMainPage();
                        }
                    }
                }
            }.bind(quarySelector), null, function () { hideLoader($(quarySelector.find('.myPanel'))) }.bind(quarySelector));
        }
    }
}

function changeInputLabels(arrLables, targetId) {
    if (arrLables) {
        for (var i = 0; i < arrLables.length; i++) {
            var inputName = arrLables[i].inputName;
            var newLebleText = arrLables[i].labelText;

            $('#stepContent_' + targetId).find('input[name="' + inputName + '"]').closest('.myCtrl').find('label').html(newLebleText);
        }
    }
}

function hideStepByRequest(steps) {
    if (steps && steps.length > 0) {
        var allStepThatHaveUrl = steps.filter(function (curItem) { return curItem.showUrl; });
        for (var i = 0; i < allStepThatHaveUrl.length; i++) {
            var postData = new FormData();
            if (window['exteraModelParams'])
                for (var prop in exteraModelParams)
                    postData.append(prop, exteraModelParams[prop]);
            postForm(allStepThatHaveUrl[i].showUrl, postData, function (res) {
                if (!res) {
                    $('#step_' + this.step.id).attr('dontShow', 'dontShow');
                    $('#stepContent_' + this.step.id).attr('dontShow', 'dontShow');
                } else {
                    bindForm(res, $('#stepContent_' + this.step.id));
                }
            }.bind({ step: allStepThatHaveUrl[i] }));
        }
    }
}

function isShowCondationValid(nextJQObj) {
    var result = true;

    if ($(nextJQObj).length > 0) {

        var attrSC = $(nextJQObj).attr('showCondation');
        if ($(nextJQObj).attr('dontShow'))
            result = false;
        else if (attrSC) {
            var attrSCObj = JSON.parse(attrSC);
            if (attrSCObj && attrSCObj.length > 0) {
                for (var i = 0; i < attrSCObj.length; i++) {
                    var targetCTRL = $('#' + attrSCObj[i].id);
                    if (attrSCObj[i].operator == '!=') {
                        if (!(targetCTRL.val() != attrSCObj[i].value)) {
                            result = false;
                            break;
                        }
                    }
                }
            }
        }
    }

    return result;
}

function initSWFunctions(curId, actionOnLastStep) {
    if ($('#' + curId).length > 0) {
        $('#' + curId)[0].moveNext = function () {
            if (!validateForm($(this).find('.panelSWizardHolderContentItemActive')))
                return;
            var nextStep = $(this).find('.panelSWizardHolderHeaderItemActive').next();
            while (isShowCondationValid(nextStep) == false)
                nextStep = nextStep.next();
            if (nextStep.length > 0) {
                $(this).find('.panelSWizardHolderHeaderItemActive').removeClass('panelSWizardHolderHeaderItemActive');
                nextStep.addClass('panelSWizardHolderHeaderItemActive');
            }
            var nextContent = $(this).find('.panelSWizardHolderContentItemActive').next();
            while (isShowCondationValid(nextContent) == false)
                nextContent = nextContent.next();
            if (nextContent.length > 0) {
                $(this).find('.panelSWizardHolderContentItemActive').removeClass('panelSWizardHolderContentItemActive');
                nextContent.addClass('panelSWizardHolderContentItemActive');
                $(nextContent).find('select').change();
            }
        };
        $('#' + curId)[0].moveToStepById = function (targetId) {
            if ($('#' + targetId).length > 0) {
                var curContent = $('#' + targetId)[0];
                var foundIndex = -1;
                var holderContentItems = $('#' + targetId).closest('.panelSWizardHolderContent');
                if (holderContentItems.length > 0) {
                    holderContentItems.find('.panelSWizardHolderContentItem').each(function (curIndex) {
                        if ($(this)[0] == curContent) {
                            foundIndex = curIndex;
                        }
                    });
                }
                if (foundIndex > -1) {
                    var foundSWCtrl = $('#' + targetId).closest('.panelSWizard');
                    if (foundSWCtrl.length > 0) {
                        $(foundSWCtrl).find('.panelSWizardHolderHeaderItemActive').removeClass('panelSWizardHolderHeaderItemActive');
                        $(foundSWCtrl).find('.panelSWizardHolderHeaderItem').eq(foundIndex).addClass('panelSWizardHolderHeaderItemActive');
                        $(foundSWCtrl).find('.panelSWizardHolderContentItemActive').removeClass('panelSWizardHolderContentItemActive');
                        $(foundSWCtrl).find('.panelSWizardHolderContentItem').eq(foundIndex).addClass('panelSWizardHolderContentItemActive');
                    }
                }
            }
        };
        $('#' + curId)[0].movePrev = function () {
            //if (!validateForm($(this).find('.panelSWizardHolderContentItemActive')))
            //    return;
            var nextStep = $(this).find('.panelSWizardHolderHeaderItemActive').prev();
            while (isShowCondationValid(nextStep) == false)
                nextStep = nextStep.prev();
            if (nextStep.length > 0) {
                $(this).find('.panelSWizardHolderHeaderItemActive').removeClass('panelSWizardHolderHeaderItemActive');
                nextStep.addClass('panelSWizardHolderHeaderItemActive');
            }
            var nextContent = $(this).find('.panelSWizardHolderContentItemActive').prev();
            while (isShowCondationValid(nextContent) == false)
                nextContent = nextContent.prev();
            if (nextContent.length > 0) {
                $(this).find('.panelSWizardHolderContentItemActive').removeClass('panelSWizardHolderContentItemActive');
                nextContent.addClass('panelSWizardHolderContentItemActive');
            }
        };

        initSWMoveNextAndMovePrevButton(curId, actionOnLastStep);
    }
}

function initSWMoveNextAndMovePrevButton(ctrlId, actionOnLastStep) {
    $('#' + ctrlId).find('.buttonBack').click(function () {
        moveBackSW(this);
    });
    $('#' + ctrlId).find('.buttonConfirm').click(function () {
        moveNextSW(this);
    });
    $('#' + ctrlId).find('.lastStepButton').click(function () {
        if (!validateForm($(this).closest('.panelSWizardHolderContentItemActive')))
            return;
        lastButtonActionSW(actionOnLastStep);
    });
}

function moveBackSW(curThis) {
    if ($(curThis).closest('.panelSWizard').length > 0) {
        $(curThis).closest('.panelSWizard')[0].movePrev();
    }
}

function moveToStepById(targetId, destinationInputName, sourceInputName, sourceButtonObj) {
    if ($('#' + targetId).closest('.panelSWizard').length > 0) {
        $('#' + targetId).closest('.panelSWizard')[0].moveToStepById(targetId);

        if (sourceButtonObj) {
            var holderSource = $(sourceButtonObj).closest('.panelSWizardHolderContentItem');
            var sourceValue = holderSource.find('[name="' + sourceInputName + '"]').val();
            $('#' + targetId).find('[name="' + destinationInputName + '"]').val(sourceValue);
            $('#' + targetId).find('[name="' + destinationInputName + '"]').focus();
        }
    }
}

function moveNextSW(curThis) {
    if ($(curThis).closest('.panelSWizard').length > 0) {
        $(curThis).closest('.panelSWizard')[0].moveNext();
    }
}

function lastButtonActionSW(actionOnLastStep) {
    if (actionOnLastStep && actionOnLastStep.length > 0) {
        for (var i = 0; i < actionOnLastStep.length; i++) {
            doPageActions(actionOnLastStep[i]);
        }
    }
}

function doPageActions(actionOnLastStep) {

    if (actionOnLastStep && actionOnLastStep.actionName) {
        if (actionOnLastStep.actionName == 'refreshGrid' && actionOnLastStep.objectId && $('#' + actionOnLastStep.objectId).length > 0) {
            $('#' + actionOnLastStep.objectId)[0].refreshData()
        } else if (actionOnLastStep.actionName == 'submitPage' && actionOnLastStep.objectId && $('#' + actionOnLastStep.objectId).length > 0) {
            var formQuery = $('#' + actionOnLastStep.objectId);
            var postData = getFormData(formQuery);
            if (window['exteraModelParams']) {
                for (var prop in exteraModelParams)
                    postData.append(prop, exteraModelParams[prop]);
            }
            showLoader(formQuery);
            postForm(actionOnLastStep.url, postData, function (res) {
                if (this.actionOnLastStep.detailesUrl && res.isSuccess == true && res.data) {
                    location.href = (res.data.url ? res.data.url : this.actionOnLastStep.detailesUrl) + "?id=" + res.data.id;
                }
            }.bind({ actionOnLastStep: actionOnLastStep }), null, function () { hideLoader(formQuery); });
        } else if (actionOnLastStep.actionName == 'showModal' && actionOnLastStep.objectId && $('#' + actionOnLastStep.objectId).length > 0) {
            $('#' + actionOnLastStep.objectId).modal('show');
        }
    }
}

function getGridTemplate(grid) {
    var result = '';

    if (grid) {
        var gridId = (!grid.id ? uuidv4RemoveDash() : grid.id);
        result += '<div id="' + gridId + '" class="myGridCTRL ' + (grid.class ? grid.class : '') + '"></div>';
        functionsList.push(function () {
            $('#' + gridId).initMyGrid(grid);
        });
    }

    return result;
}

function showModal(targetModal, curElement) {
    clearForm($('#' + targetModal));
    $('#' + targetModal).modal('show');
    $('#' + targetModal)[0].grid = $(curElement).closest('.myGridCTRL')[0];
    if ($(curElement).closest('.modal').length > 0) {
        var pKey = $(curElement).closest('.modal')[0].pKey;
        if (pKey)
            $('#' + targetModal)[0].pKey = pKey;
    }
}

function showEditModal(key, url, modalId, curElement, parentKey) {
    if (url && modalId) {
        var gridSelector = $(curElement).closest('.myGridCTRL');
        showLoader(gridSelector);
        var postData = new FormData();
        postData.append('id', key);
        if (parentKey)
            postData.append('pKey', parentKey);

        postForm(url, postData, function (res) {
            if (res) {
                var holderForm = $('#' + this.modalId);
                clearForm(holderForm);
                bindForm(res, holderForm);
                holderForm.modal('show');
                if (parentKey)
                    $('#' + modalId)[0].pKey = parentKey;
                else
                    $('#' + modalId)[0].pKey = key;
                if ($('#' + modalId).find('.myGridCTRL').length > 0) {
                    $('#' + modalId).find('.myGridCTRL').each(function () {
                        $(this)[0].refreshData();
                    })
                }
            }
        }.bind({ modalId }), null, function () { hideLoader(gridSelector); });
    } else if (modalId) {
        if ($('#' + modalId).length > 0) {
            $('#' + modalId)[0].pKey = key;
            $('#' + modalId).modal('show');
            if ($('#' + modalId).find('.myGridCTRL').length > 0) {
                $('#' + modalId).find('.myGridCTRL').each(function () {
                    $(this)[0].refreshData();
                })
            }
        }
    }
}

function simpleAjax(key, url, curElement) {
    if (url) {
        var gridSelector = $(curElement).closest('.myGridCTRL');
        showLoader(gridSelector);
        var postData = new FormData();
        postData.append('id', key);
        postForm(url, postData, function (res) {
            if (res && res.isSuccess == true) {
                gridSelector[0].refreshData();
            }
        }, null, function () { hideLoader(gridSelector); });
    }
}

function postModalData(curElement, gridId, url) {
    var qSelector = $(curElement).closest('.modal').find('.modal-content');
    var postFormData = getFormData($(qSelector));
    showLoader(qSelector);
    postForm(url, postFormData, function (res) {
        if (res && res.isSuccess == true) {
            closeThisModal(this.curElement);
            if (this.gridId) {
                $('#' + this.gridId)[0].refreshData();
            }
        }
    }.bind({ gridId, curElement }), null, function () { hideLoader(qSelector); });
}

function refreshGrid(gridId, currButtonInsideModal) {
    closeThisModal(currButtonInsideModal);
    $('#' + gridId)[0].refreshData();
}

function postPanel(curElement, url, exteraParameters) {
    var foundPanel = $(curElement).closest('.myPanel');
    if (foundPanel.length > 0) {
        showLoader(foundPanel);
        var postData = getFormData(foundPanel);
        if (exteraParameters) {
            var exParameters = null;
            exParameters = eval(exteraParameters);
            for (var item in exParameters) {
                postData.append(item, exParameters[item]);
            }
        }
        postForm(url, postData, function (res) { }, null, function () { hideLoader(foundPanel) });
    }
}

function closeThisModal(curElement) {
    $(curElement).closest('.modal').modal('hide');
}



function bindPanelByUrl(querySelector) {
    if (querySelector.length > 0) {
        var dataURl = $(querySelector).attr('data-url');
        if (dataURl) {
            var formData = new FormData();
            showLoader(querySelector);
            postForm(dataURl, formData, function (res) {
                bindForm(res, querySelector);
            }.bind({ querySelector }), null, function () { hideLoader(querySelector); });
        }
    }
}

function uuidv4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
function uuidv4RemoveDash() {
    return "id_" + uuidv4().replace(/-/, '');
}

function openUrl(url, parameters) {
    if (parameters) {
        var i = 0;
        for (var item in parameters) {
            if (i == 0) {
                url += '?' + item + '=' + parameters[item];
            }
            else {
                url += '&' + item + '=' + parameters[item];
            }
            i++;
        }
    }

    window.open(url);
}

function loadLangugesTranslate() {
    bindTranslation();
}
function bindTranslation() {
    if (window['langsT'] && langsT.length > 0) {
        for (var i = 0; i < langsT.length; i++) {
            $('[data-lc=' + langsT[i].id + ']').each(function () { $(this).html(langsT[i].des); });
        }
    }
}

function uploadFile(fileName, accepts, url, curButton) {
    if (fileName && accepts, url) {
        var id = 'id_' + uuidv4RemoveDash();
        $('body').append('<div class="holderFUTemp"><input style="display:none" type="file" name="' + fileName + '" id="' + id + '" accept="' + accepts + '" /></div>');
        $('#' + id).change(function () {
            var formObj = $(this).closest('.gridTopActionButtonHolder');
            var formData = getFormData($(this).closest('.holderFUTemp'));
            showLoader(formObj)
            postForm(url, formData, function (res) {
                if (res.isSuccess == true) {
                    $(curButton).closest('.myGridCTRL')[0].refreshData();
                }
            }, function () {
            }, function () {
                hideLoader(formObj)
            })
        });
        $('#' + id).click();
    }
}

function loadJsonConfig(jsonUrl, targetId) {
    var postData = new FormData();
    if (window['exteraModelParams'])
        for (var prop in exteraModelParams)
            postData.append(prop, exteraModelParams[prop]);
    postForm(jsonUrl, postData, function (res) {
        generateForm(res, this.targetId);
        if (!this.targetId) {
            $('.mainLoaderForAdminArea').addClass('mainLoaderForAdminAreaClose');
            setTimeout(function () {
                $('.mainLoaderForAdminArea').css('display', 'none');
            }, 200);
        }
    }.bind({ targetId: targetId }));
}