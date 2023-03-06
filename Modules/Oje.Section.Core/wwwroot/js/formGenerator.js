
var functionsList = [];

function generateForm(res, targetId, canBeAppened, url) {
    var result = '';
    if (res && res.panels && res.panels.length > 0) {
        result += '<div class="row">';
        for (var i = 0; i < res.panels.length; i++) {
            result += getPanelTemplate(res.panels[i]);
        }
        result += '</div>';
    }
    if (targetId) {
        if (typeof targetId === 'string' || targetId instanceof String) {
            if (canBeAppened)
                $('#' + targetId).append(result);
            else
                $('#' + targetId).html(result);
            $('#' + targetId).attr('data-json-config-url', url);
            executeArrFunctions();
        }
        else {
            if (canBeAppened)
                $(targetId).append(result);
            else
                $(targetId).html(result);
            $(targetId).attr('data-json-config-url', url);
            executeArrFunctions();
        }

    } else {
        if (canBeAppened)
            $('.MainHolder').append(result);
        else
            $('.MainHolder').html(result);
        $('.MainHolder').attr('data-json-config-url', url);
        executeArrFunctions();
    }
}

function getInputTemplate(ctrl) {
    var result = '';
    if (ctrl) {
        if (ctrl.showCondation && !window[ctrl.showCondation])
            return result;

        if (ctrl.onChange)
            ctrl.onChange = ctrl.onChange.replace(/&#39;/g, '\'');
        if (!ctrl.id)
            ctrl.id = uuidv4RemoveDash();
        if (ctrl.type != 'hidden')
            result += '<div class="' + ctrl.parentCL + '">';
        switch (ctrl.type) {
            case 'hidden':
                result += '<input id="' + ctrl.id + '" autocomplete="off" type="hidden" name="' + ctrl.name + '" ' + (ctrl.dfaultValue ? 'value="' + ctrl.dfaultValue + '"' : '') + ' />';
                addCtrlToObj(ctrl);
                break;
            case 'text':
            case 'number':
            case 'password':
            case 'persianDateTime':
            case 'color':
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
            case 'TabContent':
            case 'TabContentDynamicContent':
            case 'TabContentHorizontal':
                result += getTabContentTemplate(ctrl);
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
            case 'searchButtons':
                result += getSearchButtonsTemplate(ctrl);
                break;
            case 'countDownButton':
                result += getCountDownButtonTemplate(ctrl);
                break;
            case 'multiSelectLine':
                result += getMultiSelectLineTemplate(ctrl);
                break;
            case 'multiSelectImg':
                result += getMultiSelectImgTemplate(ctrl);
                break;
            case 'cTemplate':
                result += getCTemplate(ctrl);
                break;
            case 'template':
                result += getTemplate(ctrl);
                break;
            case 'label':
                result += getLableTemplate(ctrl);
                break;
            case 'map':
                result += getMapTemplate(ctrl);
                break;
            case 'ppfDesigner':
                result += getDesignerTemplate(ctrl);
                break;
            case 'empty':
                result += getEmptyCtrlTemplate(ctrl);
            default:
                break;
        }
        if (ctrl.type != 'hidden')
            result += '</div>';
    }

    return result;
}

function getEmptyCtrlTemplate(ctrl) {
    var result = '';

    result += '<div class="myCtrl " ><div style="width:100%;text-align:center;" id="' + ctrl.id + '" >' + (window['isEditModeActive'] != undefined ? 'سطر خالی' : '') + '</div></div>';
    addCtrlToObj(ctrl);

    return result;
}

var isDisinerTryToLoaded = false;
function getDesignerTemplate(ctrl) {
    var result = '<div class="myFormDesigner" data-baseUrl="' + ctrl.baseUrl + '" id="' + ctrl.id + '" ></div>';
    if (window['initDisigner']) {
        initDisigner(ctrl);
    } else {
        if (!window['initDisigner'] && !isDisinerTryToLoaded) {
            isDisinerTryToLoaded = true;
            loadJS("/Modules/Core/js/fd.min.js.gz");
            loadCSS("/Modules/Core/css/fd.min.css.gz");
            ctrl.initInterval = setInterval(function () {
                if (window['initDisigner']) {
                    clearInterval(this.curCtrl.initInterval);
                    initDisigner(this.curCtrl);
                }
            }.bind({ curCtrl: ctrl }), 300);
        }
    }
    return result;
}

function getLableTemplate(ctrl) {
    var result = '';
    result += '<div class="myCtrl form-group ' + (ctrl.class ? ctrl.class : '') + '">';
    if (ctrl.label)
        result += '<label ' + (ctrl.ltr ? 'dir:ltr' : '') + ' ' + (ctrl.id ? 'id="' + ctrl.id + '"' : '') + ' style="position: relative;font-weight:bold;cursor:default;color:' + (ctrl.color ? ctrl.color : 'black') + ';" >' + ctrl.label + '</label>';
    result += '</div>';
    addCtrlToObj(ctrl);
    return result;
}


$.fn.addStatusBarToElement = function (onClose, onMax, onMin, option, cButtons) {
    var getToobarTemplate = function () {
        var htmlResult = '<div class="myPanelToolbar" style=""><i style="' + (option && option.close == 'hide' ? 'display:none;' : '') + '"  class="fa fa-times toolbarCloseButton"></i><i class="fa fa-arrows toolBarMaxMin"></i>';
        if (cButtons && cButtons.length > 0) {
            for (var i = 0; i < cButtons.length; i++) {
                var cButton = cButtons[i];
                htmlResult += '<i class="fa ' + cButton.icon + ' "></i>';
            }
        }
        return htmlResult + '</div>';
    };

    this.each(function () {
        $(this).addClass('holderMyToolbar');
        $(this).append(getToobarTemplate(option));
        $(this).find('.toolbarCloseButton').click(function () {
            if (onClose)
                onClose();
            $(this).closest('.holderMyToolbar').remove();
        });
        if (cButtons && cButtons.length > 0) {
            for (var i = 0; i < cButtons.length; i++) {
                var cButton = cButtons[i];
                if (cButton.icon && cButton.onClick) {
                    $(this).find('.' + cButton.icon).click(function () {
                        if (this.onClick)
                            this.onClick();
                    }.bind({ onClick: cButton.onClick }));
                }
            }
        }
        $(this).find('.toolBarMaxMin').click(function () {
            if ($(this).hasClass('fa-arrows')) {
                if (onMax) {
                    var onMaxResult = onMax();
                    if (onMaxResult)
                        return;
                }
                $(this).removeClass('fa-arrows').addClass('fa-compress-arrows-alt');
                $(this).closest('.holderMyToolbar').addClass('myToolbarMakeMaximum');
            } else {
                if (onMin)
                    onMin();
                $(this).removeClass('fa-compress-arrows-alt').addClass('fa-arrows');
                $(this).closest('.holderMyToolbar').removeClass('myToolbarMakeMaximum');
            }
        });
    });
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

function getPanelTemplate(panel, isInsideModal) {
    var result = '';
    if (panel) {
        if (!panel.id)
            panel.id = uuidv4RemoveDash();
        result += '<div id="' + panel.id + '" ' + (panel.loadUrl ? 'data-url="' + panel.loadUrl + '"' : '') + '  class="myPanel ' + (panel.class ? panel.class : 'col-md-12 col-sm-12 col-xs-12 col-lg-12 col-xl-12') + '" >';
        if (panel.title)
            result += '<div id="' + panel.id + 'Title" class="myPanelTitle" style="padding:10px;padding-right:0px;">' + panel.title + '</div>';

        if (panel.ctrls) {
            result += '<div style="padding-top:7px;" class="row">';
            for (var i = 0; i < panel.ctrls.length; i++) {
                result += getInputTemplate(panel.ctrls[i]);
            }
            result += '</div>';
        }

        if (panel.type == 'TabCtrl')
            result += getTabCtrlTemplate(panel);

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
                result += getGridTemplate(panel.grids[i], isInsideModal);
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

        if (panel.actions && panel.actions.length > 0) {
            result += '<div style="text-align:right;padding-top:15px;">'
            for (var i = 0; i < panel.actions.length; i++) {
                result += getButtonTemplate(panel.actions[i]);
            }
            result += '</div>';
        }

        if (panel.ePanels && panel.ePanels.length > 0) {
            result += '<div class="row">';
            result += '<div class="col-md-12 col-sm-12 col-xl-12 col-lg-12 col-xs-12 ">';
            result += '<div class="expandablePanelGroup">'
            for (var i = 0; i < panel.ePanels.length; i++) {
                result += getEPanelsTemplate(panel.ePanels[i]);
            }
            result += '</div>';
            result += '</div>';
            result += '</div>';
        }

        if (panel && panel.panels && panel.panels.length > 0) {
            result += '<div class="row">';
            for (var i = 0; i < panel.panels.length; i++) {
                result += getPanelTemplate(panel.panels[i]);
            }
            result += '</div>';
        }

        functionsList.push(function () {
            bindPanelByUrl($('#' + this.panelId));
        }.bind({ panelId: panel.id }));
        result += '</div>';

        functionsList.push(function () {
            $('#' + this.panel.id)[0].panel = this.panel;
        }.bind({ panel: panel }));

        if (panel.autoScrollToHtml)
            functionsList.push(function () {
                if ($(this.targetElement) && $(this.targetElement).length > 0)
                    $('html').animate({ scrollTop: $(this.targetElement).offset().top }, 1000);
            }.bind({ targetElement: panel.autoScrollToHtml }));
    }
    return result;
}

function getTabCtrlTemplate(panel) {
    var result = '';

    if (panel && panel.ctrls && panel.ctrls.length > 0) {
        var newId = uuidv4RemoveDash();
        result += '<div id="' + newId + '" class="inquirySectionTab"><div class="inquirySectionTabHeaderItems">';
        for (var i = 0; i < panel.ctrls.length; i++) {
            result += '<div data-json-url="' + panel.ctrls[i].url + '" class="inquirySectionTabHeaderItem">';
            result += '<div class="inquirySectionTabHeaderItemInner ">';
            result += '<i class="fa ' + panel.ctrls[i].icon + ' imageIcon"></i>';
            result += '<div>' + panel.ctrls[i].label + '</div>';
            result += '</div>';
            result += '</div>';
        }
        result += '</div>';

        result += '<div class="inquirySectionTabBodyItems">';
        for (var i = 0; i < panel.ctrls.length; i++) {
            result += '<div class="inquirySectionTabBodyItem "></div>';
        }
        result += '</div>';

        result += '</div>';

        functionsList.push(function () {
            $('#' + this.id).initInquiryTab();
        }.bind({ id: newId }));
    }

    return result;
}

function getEPanelsTemplate(ePanel) {
    var result = '';
    if (ePanel) {
        if (!ePanel.id)
            ePanel.id = uuidv4RemoveDash();

        result += '<div id="' + ePanel.id + '" class="expandablePanel">';
        result += '<div class="expandablePanelHeader"><div class="expandablePanelHeaderTitle">' + ePanel.title + '</div><span class="expandablePanelHeaderArrow"></span></div>';
        result += '<div class="expandablePanelBody">';

        if (ePanel.ctrls && ePanel.ctrls.length > 0) {
            result += '<div class="row">';
            for (var i = 0; i < ePanel.ctrls.length; i++) {
                result += getInputTemplate(ePanel.ctrls[i]);
            }
            result += '</div>';
        }

        result += '</div>';
        result += '</div>';
    }

    functionsList.push(function () { $('#' + this.ePanel.id).initExpanablePanel(); }.bind({ ePanel: ePanel }));

    return result;
}


function getChartNotificationAttributes(notificationTriger) {
    var result = '';

    if (notificationTriger && notificationTriger.length > 0)
        for (var i = 0; i < notificationTriger.length; i++) {
            result += ' data-notify-' + notificationTriger[i];
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
        result += '<div ' + getChartNotificationAttributes(chart.notificationTriger) + ' id="' + chart.id + '" class="chartHolderDiv" >';

        result += '</div>';


        functionsList.push(function () {
            var curObj = $('#' + this.id)[0];
            curObj.refreshChart = function () {
                $('#' + this.id).html('');
                var postData = new FormData();
                if (chart.filterId) {
                    postData = getFormData($('#' + chart.filterId));
                }
                postForm(chart.url, postData, function (res) {
                    this.config[this.dataSchmea] = res;
                    initChartUntilSerciptLoaded(this.id, this.config);
                }.bind(this));
            }.bind(this);
            curObj.refreshChart();
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
        '<div class="modal-dialog modal-dialog-centered  ' + (modual.class ? modual.class : '') + '" role="document">' +
        '<div class="modal-content">' +
        '<div class="modal-header">' +
        '<h5 class="modal-title">' + modual.title + '</h5>' +
        '<button type="button" class="close" data-dismiss="modal" aria-label="Close">' +
        '<span aria-hidden="true">&times;</span>' +
        '</button>' +
        '</div>' +
        '<div class="modal-body">' +
        (modual.modelBody ? modual.modelBody : getModualTemplateCTRL(modual)) +
        '</div>' +
        getModualTemplateActionButton(modual) +
        '</div>' +
        '</div>' +
        '</div>';

    functionsList.push(function () {
        $('#' + modalId).keypress(function (e) {
            if (e.keyCode == 13) {
                if (e.target && $(e.target)[0].nodeName == 'TEXTAREA')
                    return;
                $(this).find('button.btn-primary').each(function () {
                    if ($(this).is(':visible')) {
                        $(this).click();
                    }
                });
            }
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

    if (modual) {
        result += '<div class="row">';

        if (modual && modual.panels && modual.panels.length > 0) {
            for (var i = 0; i < modual.panels.length; i++) {
                result += getPanelTemplate(modual.panels[i], true);
            }
        }
        if (modual && modual.ctrls) {
            for (var i = 0; i < modual.ctrls.length; i++) {
                result += getInputTemplate(modual.ctrls[i]);
            }
        }
        result += '</div>';
    }

    return result;
}

function copyTextToClipboard(str) {
    const el = document.createElement('textarea');
    el.value = str;
    el.setAttribute('readonly', '');
    el.style.position = 'absolute';
    el.style.left = '-9999px';
    document.body.appendChild(el);
    el.select();
    document.execCommand('copy');
    document.body.removeChild(el);
}

function updateTemplateText(ctrl) {
    var selectQuiry = $('#' + ctrl.id);
    if (selectQuiry.length > 0 && ctrl.fieldMaps && ctrl.fieldMaps.length > 0) {
        var curElement = selectQuiry[0];
        if (curElement.templateStr) {
            var newTemplate = `${curElement.templateStr}`;

            for (var i = 0; i < ctrl.fieldMaps.length; i++) {
                var curValue = '';

                for (var j = 0; j < ctrl.fieldMaps[i].sourceCtrl.length; j++) {
                    if (!curValue) {
                        if ($('input[name="' + ctrl.fieldMaps[i].sourceCtrl[j] + '"]').length > 0)
                            curValue = $('input[name="' + ctrl.fieldMaps[i].sourceCtrl[j] + '"]').val();
                        else {
                            curValue = $('select[name="' + ctrl.fieldMaps[i].sourceCtrl[j] + '"]').find('option:selected').text();
                        }
                    }
                }

                newTemplate = newTemplate.replace('{{' + ctrl.fieldMaps[i].targetTemplate + '}}', curValue).replace('{{' + ctrl.fieldMaps[i].targetTemplate + '}}', curValue).replace('{{' + ctrl.fieldMaps[i].targetTemplate + '}}', curValue).replace('{{' + ctrl.fieldMaps[i].targetTemplate + '}}', curValue)
                    .replace('{{' + ctrl.fieldMaps[i].targetTemplate + '}}', curValue).replace('{{' + ctrl.fieldMaps[i].targetTemplate + '}}', curValue).replace('{{' + ctrl.fieldMaps[i].targetTemplate + '}}', curValue).replace('{{' + ctrl.fieldMaps[i].targetTemplate + '}}', curValue);
            }
            selectQuiry.html(newTemplate);
        }
    }
}

function updateCtemplateCtrlTemplate(curThis) {
    var curUrl = $('#' + curThis.ctrl.id).attr('data-url');
    if (curUrl) {
        var postFormData = new FormData();
        appendAllQueryStringToForm(postFormData);
        if (window['exteraModelParams']) {
            for (var item in exteraModelParams) {
                postFormData.append(item, exteraModelParams[item]);
            }
        }
        if (curThis.ctrl.exteraModelParams) {
            for (var i = 0; i < curThis.ctrl.exteraModelParams.length; i++) {
                var curValue = $('input[name=' + curThis.ctrl.exteraModelParams[i] + ']').val();
                if (!curValue)
                    curValue = $('select[name=' + curThis.ctrl.exteraModelParams[i] + ']').find('option:selected').attr('value');
                if (curValue)
                    postFormData.append(curThis.ctrl.exteraModelParams[i], curValue);
            }
        }
        postForm(curUrl, postFormData, function (res) {
            $('#' + this.id)[0].templateStr = res;
            $('#' + this.id).html(res);
        }.bind({ id: curThis.ctrl.id }), function () { $('#' + this.id).html('<div style="line-height:40px;" >اطلاعاتی یافت نشد</div>'); }.bind({ id: curThis.ctrl.id }));
    }
}

function getTemplate(ctrl) {
    var result = '';

    if (ctrl) {
        result += '<div class="myCtrl fdgHtmlTemplate" id="' + ctrl.id + '" >' + ctrl.html + '</div>';
        addCtrlToObj(ctrl);
    }

    return result;
}

function getCTemplate(ctrl) {
    var result = '';

    if (ctrl) {
        result += '<div class="myCtrl " data-url="' + ctrl.dataurl + '" id="' + ctrl.id + '" ></div>';
        addCtrlToObj(ctrl);
        functionsList.push(function () {
            $('#' + this.ctrl.id)[0].ctrl = ctrl;
            updateCtemplateCtrlTemplate(this);
            if (this.ctrl.fieldMaps && this.ctrl.fieldMaps.length > 0) {
                for (var i = 0; i < this.ctrl.fieldMaps.length; i++) {
                    var curMap = this.ctrl.fieldMaps[i];
                    if (curMap.sourceCtrl && curMap.sourceCtrl.length > 0) {
                        for (var j = 0; j < curMap.sourceCtrl.length; j++) {
                            $('input[name="' + curMap.sourceCtrl[j] + '"],select[name="' + curMap.sourceCtrl[j] + '"]').change(function () { updateTemplateText(this.ctrl) }.bind({ ctrl: this.ctrl }));
                        }
                    }
                }
            }
        }.bind({ ctrl: ctrl }));
    }

    return result;
}

function getMapTemplate(ctrl) {
    var result = '';

    if (ctrl) {
        if (!ctrl.id)
            ctrl.id = uuidv4RemoveDash();
        result += '<div style="padding:0px;position:relative;z-index:2;" class="myCtrl mapCtrl form-check ' + (ctrl.class ? ctrl.class : '') + '">';
        if (ctrl.label)
            result += '<label>' + ctrl.label + '</label>';
        if (ctrl.names && ctrl.names.lat)
            result += '<input id="' + ctrl.id + '_lat" name="' + ctrl.names.lat + '" type="hidden" value="' + (ctrl.values && ctrl.values.lat ? ctrl.values.lat : '') + '" />';
        if (ctrl.names && ctrl.names.lon)
            result += '<input id="' + ctrl.id + '_lon" name="' + ctrl.names.lon + '" type="hidden" value="' + (ctrl.values && ctrl.values.lon ? ctrl.values.lon : '') + '" />';
        if (ctrl.names && ctrl.names.zoom)
            result += '<input id="' + ctrl.id + '_zoom" name="' + ctrl.names.zoom + '" type="hidden" value="' + (ctrl.values && ctrl.values.zoom ? ctrl.values.zoom : '') + '" />';

        result += '<div id="' + ctrl.id + '" style="width:' + ctrl.width + ';height:' + ctrl.height + ';margin-bottom:17px;" ></div>';
        result += '</div>';
        addCtrlToObj(ctrl);
        addOpenLayerIfNotExist();
        functionsList.push(function () {
            initMap(this.ctrl);
        }.bind({ ctrl: ctrl }));
    }

    return result;
}

function initMap(ctrl) {
    var quirySelector = $('#' + ctrl.id);
    if (quirySelector.length > 0) {
        quirySelector[0].tryToInit = 0;
        quirySelector[0].updateMapAndZoomPoint = function () {
            var curId = $(this).attr('id');
            var lat = $('#' + curId + '_lat').val();
            var lon = $('#' + curId + '_lon').val();
            var zoom = $('#' + curId + '_zoom').val();
            var fromProjection = new OpenLayers.Projection("EPSG:4326");
            var toProjection = new OpenLayers.Projection("EPSG:900913");
            var curMarkers = $('#' + ctrl.id)[0].markers
            var map = $('#' + curId)[0].map;
            if (map && lat && lon && zoom && curMarkers) {
                var position = new OpenLayers.LonLat(lon, lat).transform(fromProjection, toProjection);
                var size = new OpenLayers.Size(21, 25);
                var offset = new OpenLayers.Pixel(-(size.w / 2), -size.h);
                var icon = new OpenLayers.Icon('/Modules/Images/marker.png', size, offset);
                for (var i = 0; i < curMarkers.markers.length; i++) {
                    curMarkers.markers[i].erase()
                }
                curMarkers.addMarker(new OpenLayers.Marker(position, icon));
                map.setCenter(position, zoom);
            }
        }
        quirySelector[0].isInit = false;
        quirySelector[0].initInterval = setInterval(function () {
            var curTry = this.curThis.tryToInit;
            if (isNaN(curTry))
                curTry = 0;
            if (curTry >= 100 || this.curThis.isInit) {
                clearInterval(this.curThis.initInterval);
                return;
            }
            curTry++;
            this.curThis.tryToInit = curTry;
            if (window['OpenLayers']) {
                initMapInner(this.ctrl);
                this.curThis.isInit = true;
            }
        }.bind({ curThis: quirySelector[0], ctrl: ctrl }), 100);
    }
}

function updateMapMarkers(dataSource, mapId) {
    if (dataSource && mapId) {
        var fromProjection = new OpenLayers.Projection("EPSG:4326"); // Transform from WGS 1984
        var toProjection = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Projection
        if ($('#' + mapId).length > 0) {
            var markers = $('#' + mapId)[0].markers;
            if (markers) {
                for (var i = 0; i < dataSource.length; i++) {
                    var mapLat = dataSource[i].mapLat;
                    var mapLng = dataSource[i].mapLng;
                    var id = dataSource[i].id;
                    if (id && mapLat && mapLng) {
                        var size = new OpenLayers.Size(21, 25);
                        var offset = new OpenLayers.Pixel(-(size.w / 2), -size.h);
                        var icon = new OpenLayers.Icon('/Modules/Images/markerAgent.png', size, offset);
                        var position = new OpenLayers.LonLat(mapLng, mapLat).transform(fromProjection, toProjection);

                        var newMarketer = new OpenLayers.Marker(position, icon);
                        newMarketer.myId = id;
                        markers.addMarker(newMarketer);
                    }
                }
            }
        }
    }
}

function getSelect2ValueAndRemoveAllMapItemsExcept(selectTwoCtrlId, mapCtrlId) {
    if (selectTwoCtrlId && mapCtrlId) {
        var s2Query = $('#' + selectTwoCtrlId);
        var mapQuiry = $('#' + mapCtrlId);
        if (s2Query.length > 0 && mapQuiry.length > 0) {
            var fromProjection = new OpenLayers.Projection("EPSG:4326"); // Transform from WGS 1984
            var toProjection = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Projection
            var selectValue = s2Query.find('option:selected').val();
            if (selectValue) {
                var s2Obj = s2Query.data('select2');
                if (s2Obj) {
                    var allData = s2Obj.data();
                    if (allData && allData.length > 0) {
                        var foundSelectedItem = allData.filter(function (item) { return item.id == selectValue });
                        if (foundSelectedItem && foundSelectedItem.length > 0) {
                            foundSelectedItem = foundSelectedItem[0];
                            var markers = mapQuiry[0].markers;
                            var map = mapQuiry[0].map;
                            if (markers && map) {
                                for (var i = 0; i < markers.markers.length; i++) {
                                    if (markers.markers[i].myId) {
                                        markers.markers[i].erase();
                                    }
                                }

                                markers.markers.splice(1);

                                var size = new OpenLayers.Size(21, 25);
                                var offset = new OpenLayers.Pixel(-(size.w / 2), -size.h);
                                var icon = new OpenLayers.Icon('/Modules/Images/markerAgentSelected.png', size, offset);
                                var position = new OpenLayers.LonLat(foundSelectedItem.mapLng, foundSelectedItem.mapLat).transform(fromProjection, toProjection);

                                var newMarketer = new OpenLayers.Marker(position, icon);
                                newMarketer.myId = foundSelectedItem.id;
                                markers.addMarker(newMarketer);
                                map.setCenter(position, 15);
                            }
                        }
                    }
                }
            }

        }
    }
}

function initMapInner(ctrl) {
    var map = null;
    var markers = null;
    OpenLayers.ImgPath = "/Modules/Assets/MapIcons/";
    OpenLayers.Control.Click = OpenLayers.Class(OpenLayers.Control, {
        defaultHandlerOptions: {
            'single': true,
            'double': false,
            'pixelTolerance': 0,
            'stopSingle': false,
            'stopDouble': false
        },

        initialize: function (options) {
            this.handlerOptions = OpenLayers.Util.extend(
                {}, this.defaultHandlerOptions
            );
            OpenLayers.Control.prototype.initialize.apply(
                this, arguments
            );
            this.handler = new OpenLayers.Handler.Click(
                this, {
                'click': this.trigger
            }, this.handlerOptions
            );
        },

        trigger: function (e) {
            if (!this.ctrl.readonly) {
                var lonlat = map.getLonLatFromPixel(e.xy);
                var fromProjection = new OpenLayers.Projection("EPSG:4326"); // Transform from WGS 1984
                var toProjection = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Projection
                var position = new OpenLayers.LonLat(lonlat.lon, lonlat.lat).transform(toProjection, fromProjection);
                $('#' + ctrl.id + '_lat').val(position.lat);
                $('#' + ctrl.id + '_lon').val(position.lon);
                $('#' + ctrl.id + '_zoom').val(map.zoom);
                if (ctrl.updateAnotherMapLocation) {
                    $('#' + ctrl.updateAnotherMapLocation + '_lat').val(position.lat);
                    $('#' + ctrl.updateAnotherMapLocation + '_lon').val(position.lon);
                    $('#' + ctrl.updateAnotherMapLocation + '_zoom').val(map.zoom);
                }
                var size = new OpenLayers.Size(21, 25);
                var offset = new OpenLayers.Pixel(-(size.w / 2), -size.h);
                var icon = new OpenLayers.Icon('/Modules/Images/marker.png', size, offset);
                for (var i = 0; i < markers.markers.length; i++) {
                    markers.markers[i].erase()
                }
                markers.markers.splice(0);
                markers.addMarker(new OpenLayers.Marker(lonlat, icon));
            }
        }.bind({ ctrl: ctrl })
    });
    map = new OpenLayers.Map(ctrl.id, {
        controls: [
            new OpenLayers.Control.Navigation(),
            new OpenLayers.Control.PanZoom(),
            new OpenLayers.Control.Attribution()
        ],
        theme: null
    });
    map.addLayer(new OpenLayers.Layer.OSM(
        "OpenStreetMap",
        [
            '//a.tile.openstreetmap.org/${z}/${x}/${y}.png',
            '//b.tile.openstreetmap.org/${z}/${x}/${y}.png',
            '//c.tile.openstreetmap.org/${z}/${x}/${y}.png'
        ],
        null));
    var fromProjection = new OpenLayers.Projection("EPSG:4326");   // Transform from WGS 1984
    var toProjection = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Projection
    var position = new OpenLayers.LonLat(51.351869, 35.722938).transform(fromProjection, toProjection);
    var zoom = 15;
    markers = new OpenLayers.Layer.Markers("Markers");
    map.addLayer(markers);
    map.addLayer(new OpenLayers.Layer.OSM());
    map.setCenter(position, zoom);
    var click = new OpenLayers.Control.Click();
    map.addControl(click);
    click.activate();
    $('#' + ctrl.id)[0].map = map;
    $('#' + ctrl.id)[0].markers = markers;
    $('#' + ctrl.id)[0].updateMapAndZoomPoint();
    $('#' + ctrl.id)[0].ctrl = ctrl;
}

var isTryingLoadMapJs = false;

function addOpenLayerIfNotExist() {
    if (!window['OpenLayers'] && !isTryingLoadMapJs) {
        loadJS("/Modules/Core/js/op.min.js.gz");
        isTryingLoadMapJs = true;
    }
}

function getMultiSelectImgTemplate(ctrl) {
    var result = '';

    if (ctrl) {
        result += '<div data-name="' + ctrl.name + '" id="' + ctrl.id + '" class="multiSelectImage myCtrl">';
        result += '<div class="multiSelectImageTitle">' + ctrl.label + '</div>';
        result += '<div class="multiSelectImageBody" ></div>';
        result += '</div>';

        addCtrlToObj(ctrl);

        if (ctrl.dataurl) {
            functionsList.push(function () {
                postForm(this.ctrl.dataurl, new FormData(), function (res) {
                    var resTemplate = '';

                    if (res && res.length > 0) {
                        for (var i = 0; i < res.length; i++) {
                            resTemplate += getMultiSelectImgItemTemplate(res[i], this.ctrl.textfield, this.ctrl.valuefield, this.ctrl.imgfield, this.ctrl.onChange);
                        }
                    }

                    $('#' + this.ctrl.id).find('.multiSelectImageBody').html(resTemplate);
                }.bind({ ctrl: this.ctrl }))
            }.bind({ ctrl: ctrl }));
        }
    }

    return result;
}

function getMultiSelectImgItemTemplate(objItem, textField, valueField, imgField, onChange) {
    var result = '';

    if (objItem && textField && valueField && imgField && objItem[textField] && objItem[valueField] && objItem[imgField]) {
        result += '<div onClick="multiSelectImgClick(this)" data-onChange="' + (onChange ? onChange : '') + '" class="multiSelectImageBodyItem" data-id="' + objItem[valueField] + '" ><img alt="' + objItem[textField] + '" class="multiSelectImageBodyItemImg" width="32" height="32" src="' + objItem[imgField] + '" /><span class="multiSelectImageBodyItemTitle" >' + objItem[textField] + '</span></div>';
    }

    return result;
}

function multiSelectImgClick(curElement) {
    if (curElement) {
        var sQuery = $(curElement);
        var onChange = sQuery.attr('data-onChange');
        var curId = sQuery.attr('data-id');
        var curName = sQuery.closest('.multiSelectImage').attr('data-name');
        if (sQuery.hasClass('multiSelectImageBodyItemActive')) {
            sQuery.removeClass('multiSelectImageBodyItemActive');
            sQuery.find('input[type=hidden]').remove();
        } else {
            sQuery.addClass('multiSelectImageBodyItemActive');
            sQuery.append('<input type="hidden" value="' + curId + '" name="' + curName + '" />');
        }
        if (onChange)
            eval(onChange);
    }
}

function getMultiSelectLineTemplate(ctrl) {
    var result = '';

    if (ctrl && ctrl.name && ctrl.dataurl) {
        result += '<div data-name="' + ctrl.name + '" id="' + ctrl.id + '" class="multiSelectLine myCtrl">';
        result += '<div class="multiSelectLineTitle">' + ctrl.label + '</div>';
        result += '<div class="multiSelectLineBody"></div>'
        result += '</div>';

        addCtrlToObj(ctrl);

        functionsList.push(function () {
            postForm(this.ctrl.dataurl, new FormData(), function (res) {
                var resTemplate = '';
                if (res && res.length > 0) {
                    for (var i = 0; i < res.length; i++) {
                        resTemplate += getMultiSelectLineItemTemplate(res[i], this.ctrl.textfield, this.ctrl.valuefield);
                    }
                }
                var sQ = $('#' + this.ctrl.id);
                sQ.find('.multiSelectLineBody').html(resTemplate);
                sQ.initMultiSelectLine(this.ctrl.onChange);
            }.bind({ ctrl: this.ctrl }));
        }.bind({ ctrl: ctrl }));
    }



    return result;
}

function getMultiSelectLineItemTemplate(itemObj, titleSchema, valueSchema) {
    var result = '';

    if (itemObj && titleSchema && valueSchema && itemObj[titleSchema] && itemObj[valueSchema]) {
        result += '<div data-id="' + itemObj[valueSchema] + '" class="multiSelectLineBodyItem"><div class="multiSelectLineBodyItemTitle">' + itemObj[titleSchema] + '</div><div class="multiSelectLineBodyItemSelectIndicator"></div></div>';
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

function getSearchButtonsTemplate(ctrl) {
    var result = '';

    if (ctrl && ctrl.dataurl && ctrl.valuefield && ctrl.textfield && ctrl.name) {
        result += '<div class="myCtrl" ><div class="row" id="' + ctrl.id + '" ></div></div>';
        addCtrlToObj(ctrl);
        functionsList.push(function () {
            postForm(this.ctrl.dataurl, new FormData(), function (res) {
                if (res && res.length > 0) {
                    var templateButton = '';
                    for (var i = 0; i < res.length; i++) {
                        templateButton += '<div class="' + this.ctrl.itemClass + '" >';
                        templateButton += '<button style="margin-bottom:10px;" data-button-name="' + this.ctrl.name + '" data-button-value="' + res[i][this.ctrl.valuefield] + '" onclick="' + this.ctrl.onClick + '" type="button" class="btn btn-primary btn-block" >' + res[i][this.ctrl.textfield] + '</button>';
                        templateButton += '</div>';
                    }
                    $('#' + this.ctrl.id).html(templateButton);
                }
            }.bind({ ctrl: this.ctrl }));
        }.bind({ ctrl: ctrl }));
    }

    return result;
}

function getButtonTemplateWidthLabel(ctrl) {
    var result = '';

    result += '<div class="myCtrl form-group ' + (ctrl.showHideClass ? ctrl.showHideClass : '') + '" >'
    result += getButtonTemplate(ctrl);
    result += '</div>';
    addCtrlToObj(ctrl);

    return result;
}

function getPlaqueTemplate(ctrl) {
    var result = '';
    if (ctrl) {
        //if (ctrl.label) {
        //    result += '<label style="display:block;"  >' + ctrl.label + (ctrl.isRequired ? '<span style="color:red" >*</span>' : '') + '</label>';
        //}
        result += '<div id="' + (ctrl.id ? ctrl.id : '') + '" class="myCtrl plaqueCtrl form-group ' + (ctrl.class ? ctrl.class : '') + '" >';
        result += '<div class="plaqueRightPart" >';
        result += '<div class="plaqueRightPartRight">';
        result += '<input type="text" placeholder="55" maxlength="2" name="' + ctrl.name + '_1" />';
        result += '</div>';
        result += '<div class="plaqueRightPartLeft">';
        result += '<input type="text" placeholder="555" maxlength="3" name="' + ctrl.name + '_2" />';
        result += getPlaqueDropdownItem(ctrl);
        result += '<input type="text" placeholder="55" maxlength="2" name="' + ctrl.name + '_4" />';
        result += '</div>';
        result += '</div>';
        result += '<div class="plaqueLeftPart" >';
        result += '<img alt="پرچم ایران" width="19" height="11" src="/Modules/Images/iran.png" />';
        result += '<div>I.R</div>';
        result += '<div>Iran</div>';
        result += '</div>';
        result += '</div>';
        addCtrlToObj(ctrl);
    }
    return result;
}

function getPlaqueDropdownItem(ctrl) {
    var result = '';

    result += getDropdownCTRLTemplate(
        {
            "id": uuidv4RemoveDash(),
            "type": "dropDown",
            "textfield": "title",
            "valuefield": "title",
            "name": ctrl.name + '_3',
            "itemsClass": "makeFAP",
            "values": [
                { title: "الف" },
                { title: "ب" },
                { title: "پ" },
                { title: "ت" },
                { title: "ج" },
                { title: "د" },
                { title: "ر" },
                { title: "ژ" },
                { title: "س" },
                { title: "ص" },
                { title: "ط" },
                { title: "ع" },
                { title: "ق" },
                { title: "ک" },
                { title: "ل" },
                { title: "م" },
                { title: "ن" },
                { title: "و" },
                { title: "ه" },
                { title: "ی" }
            ]
        });

    return result;
}

function getShortcutButtonTemplate(ctrl) {
    var result = '';

    if (ctrl.url && ctrl.label) {
        result += '<a style="margin-bottom:10px;" class="btn btn-primary btn-block" href="' + ctrl.url + '" >' + ctrl.label + '</a>';
    }

    return result;
}

function createHolderIfNotExist(curObj) {
    if ($(window).width() <= 600) {
        var foundRow = $(curObj).closest('.col-xs-12');
        if (foundRow.length == 0)
            return '';

        var id = uuidv4RemoveDash();

        if (foundRow.find('.holderTabContentDiv').length == 0) {
            foundRow.append('<div id="' + id + '" class="col-md-12 col-sm-12 col-lg-12 col-xl-12 col-xs-12 holderTabContentDiv" ></div>');
        } else {
            id = foundRow.find('.holderTabContentDiv').attr('id');
        }

        return id;
    }
    else {
        var foundRow = $(curObj).closest('.row');
        if (foundRow.length == 0)
            return '';

        var id = uuidv4RemoveDash();

        if (foundRow.find('.holderTabContentDiv').length == 0) {
            foundRow.append('<div id="' + id + '" class="col-md-12 col-sm-12 col-lg-12 col-xl-12 col-xs-12 holderTabContentDiv" ></div>');
        } else {
            id = foundRow.find('.holderTabContentDiv').attr('id');
        }

        return id;
    }
}

function getGridUrlFromConfig(res) {
    if (res && res.panels) {
        for (var i = 0; i < res.panels.length; i++) {
            if (res.panels[i].grids && res.panels[i].grids.length > 0) {
                return res.panels[i].grids && res.panels[i].grids[0].url;
            }
        }
    }
    return null;
}

function addUpdateNotificationCount(buttonId, total) {
    var buttonQuery = $('#' + buttonId);
    if (buttonQuery.length > 0) {
        if (buttonQuery.find('.tabButtonExistCount').length == 0) {
            buttonQuery.append('<span class="tabButtonExistCount" >' + total + '</span>');
        }
        else {
            buttonQuery.find('.tabButtonExistCount').html(total);
        }
    }
}

function addCountNotificationToButtonIfHasGrid(buttonId) {
    var configUrl = $('#' + buttonId).attr('href');
    if (configUrl) {
        postForm(configUrl, new FormData(), function (res) {
            var foundGridUrl = getGridUrlFromConfig(res);
            if (foundGridUrl) {
                var formData = new FormData();
                formData.append('skip', 0);
                formData.append('take', 1);
                postForm(foundGridUrl, formData, function (gridResult) {
                    if (gridResult)
                        addUpdateNotificationCount(this.buttonId, gridResult.total)
                }.bind({ buttonId: this.buttonId }));
            }
        }.bind({ buttonId: buttonId }), null, null);
    }
}

function getTextURl(configUrl) {
    if (configUrl) {
        var arrParts = configUrl.split('/');
        arrParts.pop();
        return arrParts.join('/') + '/GetTitle';
    }
}

function loadTabContentDynamicContentText(buttonId) {
    var configUrl = $('#' + buttonId).attr('href');
    if (configUrl) {
        var textUrl = getTextURl(configUrl);
        if (textUrl) {
            postForm(textUrl, new FormData(), function (res) {
                addUpdateNotificationCount(this.buttonId, res);
            }.bind({ buttonId: buttonId }), null, null);
        }
    }
}

function getTabButtonNotifyUpdateAttribute(nTypes) {
    var result = '';

    if (nTypes && nTypes.length > 0) {
        for (var i = 0; i < nTypes.length; i++)
            result += ' data-notify-' + nTypes[i];
    }

    return result;
}

function getTabContentTemplate(ctrl) {
    var result = '';

    if (ctrl.url && ctrl.label) {
        if (!ctrl.color)
            ctrl.color = 'orange';
        result += '<div id="' + ctrl.id + '_holder" class="tabButtonBoxHolder ' + (ctrl.type == 'TabContentHorizontal' ? 'MakeHorizoneTC' : '') +'">';
        result += '<span  style="' + (ctrl.color ? 'background-color:' + ctrl.color + ';' : '') + '" class="tabButtonIcon fa ' + ctrl.icon + '" ><span style="' + (ctrl.color ? 'border-color:' + ctrl.color + ';' : '') + '" ></span></span>';
        result += '<a ' + getTabButtonNotifyUpdateAttribute(ctrl.nTypes) + ' id="' + ctrl.id + '" style="margin-bottom:15px;position:relative;' + (ctrl.color ? 'border-top:4px solid ' + ctrl.color + ';' : '') + '" class="tabButtonBox" href="' + ctrl.url + '" ><span class="buttonTitle">' + ctrl.label + '</span></a>';
        result += '</div>';
        functionsList.push(function () {
            if (ctrl.type == 'TabContent' || ctrl.type == 'TabContentHorizontal')
                addCountNotificationToButtonIfHasGrid(this.ctrl.id);
            else if (ctrl.type == 'TabContentDynamicContent')
                loadTabContentDynamicContentText(this.ctrl.id);
            $('#' + this.ctrl.id + '_holder').click(function () {
                $(this).find('a').click();
            });
            $('#' + this.ctrl.id).click(function (e) {
                e.stopPropagation();
                e.preventDefault();

                var curUrl = $(this).attr('href');
                var targetDivId = createHolderIfNotExist(this);
                if (targetDivId) {
                    loadJsonConfig(curUrl, targetDivId, function () {
                        $(document.documentElement, document.body).animate({
                            scrollTop: $('#' + targetDivId).offset().top - 300
                        }, 1000);
                    });
                }

                return false;
            });
        }.bind({ ctrl: ctrl }));
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

        result += '<div class="col-md-12 col-sm-12 col-xs-12 col-lg-12" style="text-align:center;" >' + ('<button style="margin:3px;' + (ctrl.hideAddButton ? 'display:none;' : '') + '" class="btn btn-primary btn-sm addNewRowForMultiRowCtrl">' + (ctrl.addTitle ? ctrl.addTitle : 'افزودن') + '</button>') +
            '<button style="margin:3px;display:none;" class="btn btn-danger btn-sm deleteNewRowForMultiRowCtrl">' + (ctrl.deleteTitle ? ctrl.deleteTitle : 'حذف اخرین سطر') + '</button>' +
            '</div>';
        result += '</div>';
        result += '</div>';

        functionsList.push(function () {
            initMoultiRowInputButton(this.ctrl);
            initInternalFunctions(this.ctrl);
        }.bind({ ctrl: ctrl }));
        addCtrlToObj(ctrl);
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

function addNewRowForMultiCtrl(curButton) {
    if (curButton) {
        var sQuiry = $(curButton).closest('.panelSWizardHolderContentItem');
        var isStepWizard = sQuiry.length > 0 && sQuiry.parent().closest('.panelSWizardHolderContentItem').length > 0;
        if (isStepWizard) {
            closeCurSWRow(curButton);
        }
        var foundParentButton = $(curButton).closest('.MultiRowInputRow');
        if (foundParentButton.length > 0) {
            var addButtonTitle = foundParentButton.parent().find('> div > .addNewRowForMultiRowCtrl').text();
            if (!(foundParentButton.next().length > 0 && foundParentButton.next().hasClass('MultiRowInputRow'))) {
                if (!foundParentButton.next().hasClass('addNewRowForMRI')) {
                    foundParentButton.after('<div class="row addNewRowForMRI" ><div class="col-md-4 col-sm-6 col-xs-12 col-lg-3 "><button onclick="addNewRowForMRCTrl2(this)" class="btn btn-primary btn-block" >' + addButtonTitle +'</button></div><div class="col-md-4 col-sm-6 col-xs-12 col-lg-3 "><button onclick="moveToNextStepForSW(this)" class="btn btn-primary btn-block" >عدم افزودن و مرحله بعد</button></div></div>');
                }
            //    foundParentButton.parent().find('.addNewRowForMultiRowCtrl').click();
            }
        }
    }
}

function addNewRowForMRCTrl2(curButton) {
    if ($(curButton).closest('.addNewRowForMRI').length > 0) {
        $(curButton).closest('.addNewRowForMRI').next().find('.addNewRowForMultiRowCtrl').click();
        $(curButton).closest('.addNewRowForMRI').remove();
    }
}

function closeCurSWRow(curButton) {
    var title = $(curButton).closest('.panelSWizard').closest('.myPanel').find('.myPanelTitle:eq(0)').text();
    var curRowQuiry = $(curButton).closest('.MultiRowInputRow');
    if (curRowQuiry.length > 0) {
        var curIndex = getElementIndex(curRowQuiry[0], 'MultiRowInputRow');
        curRowQuiry.addClass('hideMultiRowItem');
        curRowQuiry.append('<div class="multiRowItemCover" ><button onclick="editThisRow(this)" class="editMRButton btn btn-warning btn-sm" >ویرایش</button><span>' + curIndex + ' - ' + title + '</span></div>');
    }
}

function getElementIndex(curElement, curClass) {
    var result = -1;

    if (curElement) {
        var parentEleemnt = $(curElement).parent();
        parentEleemnt.find('.' + curClass).each(function (index)
        {
            if ($(this)[0] == curElement) {
                result = index + 1;
            }
        })
    }

    return result;
}

function editThisRow(curThis) {
    $(curThis).closest('.hideMultiRowItem').removeClass('hideMultiRowItem');
    $(curThis).closest('.multiRowItemCover').remove();
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
                $(this).closest('.myCtrl').find('> div > .MultiRowInputRow:last').after(newRowHtml);
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
        result += '<div class="form-check form-switch myCtrl ' + (ctrl.class ? ctrl.class : '') + '">';
        result += '<input ' + (ctrl.dfaultValue ? 'checked' : '') + ' ' + getCtrlValidationAttribute(ctrl) + ' ' + ' class="form-check-input " name="' + ctrl.name + '" type="checkbox" value="' + (ctrl.defValue ? ctrl.defValue : 'true') + '" id="' + ctrl.id + '" />';
        result += '<label style="padding-right:20px;" class="form-check-label" for="' + ctrl.id + '">' + ctrl.label + '</label>';
        result += '</div>';
        addCtrlToObj(ctrl);
        if (ctrl.fileDownloadConfig) {
            functionsList.push(function () {
                if (this.ctrl.fileDownloadConfig.url && this.ctrl.fileDownloadConfig.text && this.ctrl.id) {
                    var formData = new FormData();
                    appendAllQueryStringToForm(formData);
                    if (window['exteraModelParams'])
                        for (var prop in exteraModelParams) {
                            var isExist = false;
                            for (const key of formData.keys()) {
                                if (prop == key) {
                                    isExist = true;
                                    break;
                                }
                            }
                            if (!isExist)
                                formData.append(prop, exteraModelParams[prop]);
                        }
                    postForm(this.ctrl.fileDownloadConfig.url, formData, function (res) {
                        if (res) {
                            var foundLabel = $('#' + this.ctrl.id).closest('.myCtrl').find('label');
                            if (foundLabel.length > 0 && foundLabel.text()) {
                                var aTag = '<a target="_blank" href="' + res + '" title="' + this.ctrl.fileDownloadConfig.text + '" >' + this.ctrl.fileDownloadConfig.text + '</a>';
                                foundLabel.html(foundLabel.text().replace(this.ctrl.fileDownloadConfig.text, aTag));
                            }
                        }
                    }.bind({ ctrl: this.ctrl }))
                }
            }.bind({ ctrl: ctrl }));
        }
    }

    return result;
}

function getRadioButtonTemplate(ctrl) {
    var result = '';
    if (ctrl) {
        result += '<div class="myCtrl form-group ' + (ctrl.class ? ctrl.class : '') + '"' + '>';
        if (ctrl.label)
            result += '<label ' + (ctrl.id ? 'id="' + ctrl.id + '"' : '') + ' style="position: relative;display:block" >' + ctrl.label + (ctrl.isRequired ? '<span style="color:red" >*</span>' : '') + '</label>';
        if (ctrl.values && ctrl.values.length > 0) {
            var idList = [];
            for (var i = 0; i < ctrl.values.length; i++) {
                var id = ctrl.values[i].oId ? ctrl.values[i].oId : uuidv4RemoveDash();
                idList.push(id);
                result += '<div class="form-check form-check-inline">';
                result += '<input ' + (i == 0 ? 'checked="checked"' : '') + ' class="form-check-input" name="' + ctrl.name + '" id="' + id + '" type="radio" value="' + ctrl.values[i][ctrl.valuefield] + '" />';
                result += '<label  style="position: relative" class="form-check-label" for="' + id + '" >' + ctrl.values[i][ctrl.textfield] + '</label>';
                result += '</div>';
            }
            ctrl.idList = idList;
        }
        result += '</div>';
        addCtrlToObj(ctrl);
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
                            if (!$(this)[0].isAutoChangeFProcessing)
                                callInputChangeEventInSameRange($(this).attr('id'));
                        });
                        showAndHideCtrl('', this.showHideCondation, this.id);

                    }
                }

            }.bind({ id: ctrl.id, showHideCondation: ctrl.showHideCondation, idList: ctrl.idList }));
        }
    }
    return result;
}


function callInputChangeEventInSameRange(ctrlId) {
    var selectQuiry = $('#' + ctrlId);
    if (selectQuiry.length > 0) {
        if (selectQuiry.closest('.panelSWizardHolderContentItem').length > 0) {
            selectQuiry[0].isAutoChangeFProcessing = true;
            selectQuiry.closest('.panelSWizardHolderContentItem').find('input,select').change();
        } else if (selectQuiry.closest('.myPanel').length > 0) {
            selectQuiry[0].isAutoChangeFProcessing = true;
            selectQuiry.closest('.myPanel').find('input,select').change();
        }
        selectQuiry[0].isAutoChangeFProcessing = false;
    }
}

function getDynamicCtrlsTemplate(ctrl) {
    var result = '';

    if (ctrl && ctrl.id && ctrl.dataurl) {
        result += '<div class="" id="' + ctrl.id + '" ></div>';
    }
    addCtrlToObj(ctrl);
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
                htmlResult += '<div class="row">';
                for (var i = 0; i < res.panels.length; i++) {
                    htmlResult += getPanelTemplate(res.panels[i]);
                }
                htmlResult += '</div>';
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
                            compressImage: true,
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
    addCtrlToObj(ctrl);
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

function getCurrUrlParameters() {
    var result = '';
    var allParams = new URLSearchParams(window.location.search);
    var isFirst = true;
    for (var pair of allParams.entries()) {
        if (isFirst == true) {
            result += '?' + pair[0] + "=" + pair[1];
            isFirst = false;
        }
        else
            result += '&' + pair[0] + "=" + pair[1];
    }
    return result;
}

function getDynamicFileUploadCtrlTemplate(ctrl) {
    var result = '';

    if (ctrl.parentCL && ctrl.url && ctrl.schema) {
        result += '<div class="row myCtrl" ><div class="col-md-12 col-sm-12 col-xs-12 col-lg-12" id="' + ctrl.id + '"></div></div>';
        functionsList.push(function () {
            if (this.ctrl.url) {
                var formData = new FormData();
                appendAllQueryStringToForm(formData);
                if (window['exteraModelParams'])
                    for (var prop in exteraModelParams) {
                        var isExist = false;
                        for (const key of formData.keys()) {
                            if (prop == key) {
                                isExist = true;
                                break;
                            }
                        }
                        if (!isExist)
                            formData.append(prop, exteraModelParams[prop]);
                    }
                postForm(this.ctrl.url, formData, function (res) {
                    if (res && res.length > 0) {
                        if (this.ctrl.schema) {
                            var template = '<div class="row">';
                            for (var i = 0; i < res.length; i++) {
                                template += '<div class="' + this.ctrl.css + '">';
                                template += getFileCTRLTemplate({
                                    compressImage: true,
                                    hideImagePreview: this.ctrl.hideImagePreview,
                                    label: res[i][this.ctrl.schema.title],
                                    type: 'file',
                                    name: res[i][this.ctrl.schema.name].replace(/ /g, ''),
                                    acceptEx: this.ctrl.acceptEx,
                                    isRequired: res[i][this.ctrl.schema.isRequired],
                                    sampleUrl: res[i][this.ctrl.schema.sampleUrl]
                                });
                                template += '</div>';
                            }
                            template += '</div>';
                            $('#' + this.ctrl.id).html(template);
                        }
                        executeArrFunctions();
                    }
                }.bind({ ctrl: this.ctrl }));
            }
        }.bind({ ctrl: ctrl }));
        addCtrlToObj(ctrl);
    }

    return result;
}

function getDateTimeMinMaxValueValidation(ctrl) {
    var result = '';

    if (ctrl.type == 'persianDateTime') {
        if (ctrl.minDateValidation)
            result += ' data-jdp-min-date="' + getLastDayFromToday(ctrl.minDateValidation) + '" ';
        if (ctrl.maxDateValidation)
            result += ' data-jdp-max-date="' + getLastDayFromToday(ctrl.maxDateValidation) + '" ';
    }

    return result;
}

function hasNumberValidation(validations) {
    if (validations && validations.length > 0)
        for (var i = 0; i < validations.length; i++)
            if (validations[i].reg == '^[0-9]*$')
                return true;

    return false;
}

function addCtrlToObj(ctrl) {
    functionsList.push(function () {
        var sQuiry = $('#' + this.ctrl.id);
        if (sQuiry.length > 0 && (sQuiry.hasClass('myCtrl') || sQuiry.closest('.myCtrl').length > 0)) {
            if (sQuiry.hasClass('myCtrl'))
                sQuiry[0].ctrl = this.ctrl;
            else
                sQuiry.closest('.myCtrl')[0].ctrl = this.ctrl;
        }
    }.bind({ ctrl: ctrl }));
}

function getTextBoxTemplate(ctrl) {
    if (!ctrl.id)
        ctrl.id = uuidv4RemoveDash();
    var result = '';
    result += '<div class="myCtrl form-group ' + (ctrl.class ? ctrl.class : '') + '">';
    if (ctrl.label) {
        result += '<label for="' + ctrl.id + '"  >' + ctrl.label + (ctrl.isRequired ? '<span style="color:red" >*</span>' : '') + '</label>';
    }
    result += '<input  style="' + (ctrl.fontSize ? ('font-size:' + ctrl.fontSize) : '') + '" ' + (getOs() == 'iOS' ? '' : 'onfocus="this.removeAttribute(\'readonly\');" readonly="readonly"') + '' + (ctrl.ltr ? 'dir="ltr"' : '') + ' ' + getDateTimeMinMaxValueValidation(ctrl) + ' autocomplete="off" ' + (ctrl.maxLengh ? 'maxlength="' + ctrl.maxLengh + '"' : '') + ' ' + (ctrl.type == "persianDateTime" ? 'data-jdp ' : ' ') + getCtrlValidationAttribute(ctrl) + ' ' + (ctrl.disabled ? 'disabled="disabled"' : '') + ' ' + (ctrl.ph ? 'placeholder="' + ctrl.ph + '"' : '') + ' ' + (ctrl.id ? 'id="' + ctrl.id + '"' : '') + '" ' + (ctrl.dfaultValue ? 'value="' + ctrl.dfaultValue + '"' : ctrl.yearFromKnow !== undefined ? 'value="' + getLastYearFromToday(ctrl.yearFromKnow) + '"' : '') + ' name="' + ctrl.name + '" class="form-control" />';
    if (ctrl.help)
        result += '<i onclick="return showHelpModal(this)" class="fa fa-question HelpButton inputHelpButton" data-help=\'' + ctrl.help + '\' ></i>';
    result += '</div>';

    if (ctrl.nationalCodeValidation || hasNumberValidation(ctrl.validations))
        if (!ctrl.nationalCodeValidation)
            ctrl.type = 'number';

    addCtrlToObj(ctrl);
    functionsList.push(function () {
        if (this.toUpperCase) {
            $('#' + this.id).blur(function () {
                $(this).val($(this).val().toUpperCase());
            });
        }
        $('#' + this.id).change(function (e) {
            if (e.originalEvent) {
                validateForm($(this).closest('div'));
            }
        });
        setTimeout(function () {
            $('#' + this.id).attr('type', (this.type == 'persianDateTime' ? 'text' : this.type));
        }.bind(this), 1);
        inputNewLabelEventHandler(this.id);

    }.bind({ id: ctrl.id, type: ctrl.type, toUpperCase: ctrl.toUpperCase }));

    if (ctrl.mask) {
        functionsList.push(function () {
            $('#' + this.ctrl.id).mask(this.ctrl.mask)
        }.bind({ ctrl: ctrl }));
    }

    if (ctrl.onChange) {
        functionsList.push(function () {
            $('#' + this.ctrl.id).change(function () {
                eval(this.ctrl.onChange);
            }.bind({ ctrl: this.ctrl }));
        }.bind({ ctrl: ctrl }));
    }

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
    if (ctrl.holderStatusId) {
        functionsList.push(function () {
            var holderId = this.ctrl.holderStatusId;
            $('#' + this.ctrl.id).change(function () {
                var label = getCtrlLabel(this);
                var value = $(this).val();
                updateHolderStatusValue(holderId, label, value, $(this).attr('id'));
            });
        }.bind({ ctrl: ctrl }));
    }
    if (ctrl.seperator) {
        functionsList.push(function () {
            $('#' + this.id).seleperator();
        }.bind({ id: ctrl.id }));
    }
    if (ctrl.type == "persianDateTime") {
        functionsList.push(function () {
            setTimeout(function () {
                var curQuiery = $('#' + this.id);
                var jdtOption = {
                    formatDate: "YYYY/0M/0D",
                    cellWidth: 38,
                    cellHeight: 30,
                    fontSize: 16
                };

                if (this.ctrl.minDateValidation)
                    jdtOption.startDate = curQuiery.attr('data-jdp-min-date');
                if (this.ctrl.maxDateValidation)
                    jdtOption.endDate = curQuiery.attr('data-jdp-max-date');

                if (curQuiery.val()) {
                    jdtOption.selectedDate = curQuiery.val();
                }

                curQuiery.persianDatepicker(jdtOption);
            }.bind({ id: this.id, ctrl: ctrl }), 100);
        }.bind({ id: ctrl.id }));
    }
    if (ctrl.multiPlay) {
        functionsList.push(function () {
            for (var i = 0; i < this.ctrl.multiPlay.length; i++) {
                if ($('#' + this.ctrl.multiPlay[i]).length > 0) {
                    if (!$('#' + this.ctrl.multiPlay[i])[0].multiPlayObj)
                        $('#' + this.ctrl.multiPlay[i])[0].multiPlayObj = [];
                    $('#' + this.ctrl.multiPlay[i])[0].multiPlayObj.push(this.ctrl);
                    $('#' + this.ctrl.multiPlay[i]).change(function () {
                        if ($(this)[0].multiPlayObj && $(this)[0].multiPlayObj.length > 0)
                            for (var m = 0; m < $(this)[0].multiPlayObj.length; m++)
                                doCalceForMultiplay($(this)[0].multiPlayObj[m]);
                    });
                }
            }
        }.bind({ ctrl: ctrl }));
    }

    if (ctrl.sumCalculator) {
        functionsList.push(function () {
            for (var i = 0; i < this.ctrl.sumCalculator.length; i++) {
                if ($('#' + this.ctrl.sumCalculator[i]).length > 0) {
                    if (!$('#' + this.ctrl.sumCalculator[i])[0].sumCalculatorObj)
                        $('#' + this.ctrl.sumCalculator[i])[0].sumCalculatorObj = [];
                    $('#' + this.ctrl.sumCalculator[i])[0].sumCalculatorObj.push(this.ctrl);
                    $('#' + this.ctrl.sumCalculator[i]).change(function () {
                        if ($(this)[0].sumCalculatorObj && $(this)[0].sumCalculatorObj.length > 0)
                            for (var m = 0; m < $(this)[0].sumCalculatorObj.length; m++)
                                doCalceForSum($(this)[0].sumCalculatorObj[m]);
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

function showHelpModal(curButton) {
    var curHtml = $(curButton).attr('data-help');
    var curLaabel = $(curButton).closest('.myCtrl').find('label').text();
    if (curHtml) {
        var modalId = 'help' + uuidv4RemoveDash();
        $('body').append(getModualTemplate({
            id: modalId,
            class: 'modal-md',
            title: 'راهنما :' + curLaabel,
            modelBody: curHtml,
            actions: [
                {
                    title: 'بستن',
                    class: 'btn-secondary',
                    onClick: 'closeThisModal(this)'
                }
            ]
        }));
        $('#' + modalId).modal('show');
    }
}

function doCalceForSum(sumCalculator) {
    if (sumCalculator && sumCalculator.sumCalculator) {
        var result = 0;
        for (var i = 0; i < sumCalculator.sumCalculator.length; i++) {
            try {
                if ($('#' + sumCalculator.sumCalculator[i]).val().replace(/,/, ''))
                    result = result + Number.parseInt($('#' + sumCalculator.sumCalculator[i]).val().replace(/,/, ''));
            }
            catch (e) {

            }
        }
        if (isNaN(result))
            result = 0;
        $('#' + sumCalculator.id).val(postCommanInNumberPlugin(result));
    }
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
        if ($('#' + multiPlayObj.id).attr('type') == 'text')
            $('#' + multiPlayObj.id).val(postCommanInNumberPlugin(result));
        else
            $('#' + multiPlayObj.id).val(result);
    }
}
var isLoadCkEditorTrying = false;
function getCkEditorTemplate(ctrl) {
    var result = '';

    result += getTextAreaTemplate(ctrl);
    if (result) {
        functionsList.push(function () {
            if (isLoadCkEditorTrying == false) {
                loadJS('/Modules/Core/js/ce.js.gz?v=1');
                isLoadCkEditorTrying = true;
            } else if (window['initAllCkEditors']) {
                initAllCkEditors();
            }

        });
        addCtrlToObj(ctrl);
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
    $('#' + curId).keyup(function () {
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

function loadCSS(src) {
    var script = document.createElement("link");
    script.setAttribute("rel", "stylesheet");
    script.setAttribute("href", src);
    document.getElementsByTagName("head")[0].appendChild(script);
}

function getTextAreaTemplate(ctrl) {
    var result = '';
    result += '<div class="myCtrl form-group ' + ctrl.class + ' ' + (ctrl.type == 'ck' ? 'makeLabelBGSilver' : '') + '">';
    if (ctrl.label) {
        result += '<label for="' + ctrl.id + '" >' + ctrl.label + (ctrl.isRequired ? '<span style="color:red" >*</span>' : '') + '</label>';
    }
    var id = (ctrl.id ? ctrl.id : uuidv4RemoveDash());
    if (!ctrl.id)
        ctrl.id = id;
    result += '<textarea ' + (ctrl.ltr ? 'dir="ltr"' : '') + ' id="' + ctrl.id + '" autocomplete="off" ' + getCtrlValidationAttribute(ctrl) + ' ' + (ctrl.ph ? 'placeholder="' + ctrl.ph + '"' : '') + ' ' + (ctrl.id ? 'id="' + ctrl.id + '"' : '') + ' type="' + ctrl.type + '" name="' + ctrl.name + '" class="form-control ' + (ctrl.type == 'ck' ? 'ckEditor' : '') + '" >' + (ctrl.dfaultValue ? ctrl.dfaultValue : '') + '</textarea>';
    result += '</div>';

    functionsList.push(function () {
        inputNewLabelEventHandler(this.id);
    }.bind({ id: ctrl.id }));
    addCtrlToObj(ctrl);

    return result;
}

function showCopModal(imageQuiry, min, max) {
    var modalId = imageQuiry.attr('id') + '_cropModal';
    var cropperModalId = modalId + '_cpCtrl';
    var localModalObj = {
        id: modalId,
        class: 'modal-xl',
        title: 'ویرایش تصویر',
        modelBody: '<div id="' + cropperModalId + '" class="cImageCtrl"></div>',
        actions: [
            {
                title: 'بستن',
                onClick: 'closeThisModal(this)',
                class: 'btn-secondary'
            },
            {
                title: 'ذخیره',
                onClick: 'saveCropImage(\'' + imageQuiry.attr('id') + '\', this)',
                class: 'btn-primary'
            }
        ]
    };

    if ($('#' + modalId).length == 0) {
        $('body').append(getModualTemplate(localModalObj));
        executeArrFunctions();
    }
    $('#' + cropperModalId).initImageCrapper(imageQuiry.attr('src'), min, max);
    $('#' + modalId).modal('show');
}

function saveCropImage(imgId, curThis) {
    if ($('#tempCanvas').length > 0 && $('#' + imgId).length > 0) {
        var canvas = $('#tempCanvas')[0];
        var targetImage = $('#' + imgId)[0];
        var fileName = 'cropImage';
        canvas.toBlob(function (blob) {
            var f2 = new File([blob], fileName + ".jpg", { type: "image/jpg" });
            targetImage.compressUploadFile = f2;
            targetImage.useWithouFile = true;
            var urlCreator = window.URL || window.webkitURL;
            var imageUrl = urlCreator.createObjectURL(blob);
            targetImage.src = imageUrl;
        }, 'image/jpeg', 0.8);
        closeThisModal(curThis);
    }
}


function getFileCTRLTemplate(ctrl) {
    var result = '';

    if (!ctrl.id)
        ctrl.id = uuidv4RemoveDash();

    result += '<div class="myCtrl form-group myFileUpload">';

    result += '<div ' + (ctrl.id ? 'id="' + ctrl.id + '"' : '') + ' style="' + (ctrl.hideImagePreview ? 'display:none;' : '') + '" class="holderUploadImage">';
    result += '<img data-name="' + ctrl.name + '_address" id="img_' + ctrl.id + '" src="' + (ctrl.sampleUrl ? ctrl.sampleUrl : '/Modules/Images/unknown.svg') + '" />';
    result += '</div>';

    if (ctrl.label) {
        result += '<label class="btn btn-secondary btn-block" style="margin-bottom:0px;text-align:center;" for="file_' + ctrl.id + '" >' + ctrl.label + (ctrl.isRequired ? '<span style="color:red" >*</span>' : '') + '</label>';
    }
    result += '<input ' + (ctrl.compressImage ? 'data-compressImage="true"' : '') + ' id="file_' + ctrl.id + '" ' + getCtrlValidationAttribute(ctrl) + ' ' + (ctrl.acceptEx ? 'accept="' + ctrl.acceptEx + '"' : '') + ' type="' + ctrl.type + '" name="' + ctrl.name + '" class="form-control" />';
    result += '</div>';

    addCtrlToObj(ctrl);
    functionsList.push(function () {
        $('#file_' + this.id).change(function () {
            var curValue = $(this).val();
            if (curValue) {
                if ($(this).closest('.myFileUpload').length > 0) {
                    $(this).closest('.myFileUpload').find('label')[0].prevTitle = $(this).closest('.myFileUpload').find('label').html();
                    $(this).closest('.myFileUpload').find('label').html($(this)[0].files[0].name);
                    $(this).closest('.myFileUpload').find('label').removeClass('btn-secondary').addClass('btn-success');
                }
            } else {
                if ($(this).closest('.myFileUpload').length > 0) {
                    $(this).closest('.myFileUpload').find('label').html($(this).closest('.myFileUpload').find('label')[0].prevTitle);
                    $(this).closest('.myFileUpload').find('label').removeClass('btn-success').addClass('btn-secondary');
                }
            }
        });
    }.bind({ id: ctrl.id }));

    functionsList.push(function () {
        var imgId = 'img_' + this.id;
        var fileId = 'file_' + this.id;
        if ($('#' + imgId).length == 0 || $('#' + fileId).length == 0)
            return;

        $('#' + fileId).change(function () {
            readFileFromInput(this, imgId);
        });
        var allCButtons = [];
        if (this.ctrl.cropper)
            allCButtons.push({ icon: 'fa-crop', onClick: function () { showCopModal($('#' + imgId), this.ctrl.cropperValidation ? this.ctrl.cropperValidation.min : null, this.ctrl.cropperValidation ? this.ctrl.cropperValidation.max : null) }.bind({ ctrl: this.ctrl }) });
        if (this.ctrl.deleteButton)
            allCButtons.push({ icon: 'fa-trash', onClick: function () { postForm(this.ctrl.deleteButton, new FormData(), function () { }); }.bind({ ctrl: this.ctrl }) });

        $('#' + fileId).closest('.myFileUpload').find('.holderUploadImage').addStatusBarToElement(null, null, null, { close: 'hide' }, allCButtons);

    }.bind({ id: ctrl.id, ctrl: ctrl }));

    return result;
}

function readFileFromInput(fileInput, imgId) {
    if (fileInput.files && fileInput.files[0] && (/image/i).test(fileInput.files[0].type)) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#' + imgId).attr('src', e.target.result);
            var img = new Image();
            img.src = e.target.result;
            img.onload = function () {
                compressImageAndAddToFormData(fileInput.files[0], img, $('#' + imgId)[0]);
            }
        }
        reader.readAsDataURL(fileInput.files[0]);
    }
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

    if (ctrl.type == 'dropDown')
        ctrl.type = 'tokenBox';
    if (ctrl.type == 'dropDown2')
        ctrl.type = 'tokenBox2';

    functionsList.push(function () { initTokenBox($('#' + this.id).closest('.tokenBox')); }.bind({ id: ctrl.id }));

    return result;
}

function setSelect2LabelAria(curId) {
    var sQuery = $('#' + curId).closest('.myCtrl');
    if (sQuery.length > 0) {
        var curLabel = sQuery.find('label').text();
        var curLabelId = sQuery.find('label').attr('id');
        if (curLabel) {
            sQuery.find('[aria-labelledby]').attr('aria-label', curLabel).attr('aria-labelledby', curLabelId);
            sQuery.find('[role="textbox"]').attr('aria-label', curLabel).attr('aria-labelledby', curLabelId);
        }
    }
}

function updateHolderStatusValue(holderId, label, value, uId) {
    var qureSelector = $('#' + holderId);
    var isCurCtrlVisible = $('#' + uId).closest('.myCtrl').is(':visible');
    var resultValue = (value && isCurCtrlVisible ? (value) : '');
    var targetStatusId = 'status_' + uId;
    if (qureSelector.length > 0) {
        if (qureSelector.find('#' + targetStatusId).length > 0) {
            $('#' + targetStatusId).text(resultValue);
        } else {
            if (resultValue) {
                qureSelector.append('<span class="ctrlStatusIndicator" id="' + targetStatusId + '" >' + resultValue + '</span>');
            }
        }
    }
}

function getCtrlLabel(curThis) {
    return ($(curThis).closest('.myCtrl').find('label').text() + '').replace('*', '');
}

function refreshInquiryGrid(curId) {
    if (curId) {
        var curObj = $('#' + curId);
        if (curObj.length > 0) {
            var modalContainer = $(curObj).closest('.modal-body');
            if (modalContainer.length > 0) {
                var foundGrid = modalContainer.find('.myGridCTRL');
                if (foundGrid.length > 0)
                    foundGrid[0].refreshData();
            }
        }
    }
}

function getDropdownCTRLTemplate(ctrl) {
    var result = '';

    if (!ctrl.id)
        ctrl.id = uuidv4RemoveDash();

    var labelId = uuidv4RemoveDash();

    result += '<div id="myCtrlDivHolder' + ctrl.id + '" class="myCtrl ' + (ctrl.type == 'dropDown' && !ctrl.moveNameToParent ? 'myDropdown myDropdownHeight' : '') + ' form-group ' + (ctrl.class ? ctrl.class : '') + '"' + (ctrl.moveNameToParent ? ' name="' + ctrl.name + '"' : '') + '>';
    if (ctrl.label) {
        result += '<label id="' + labelId + '" for="' + ctrl.id + '" >' + ctrl.label + (ctrl.isRequired ? '<span style="color:red" >*</span>' : '') + '</label>';
    }
    result += '<select ' + (ctrl.ignoreChangeOnBinding ? 'data-ignore-change-onBinding="true"' : '') + ' ' + (ctrl.reInitOnShowModal ? 'reInitOnShowModal="true" ' : '') + (ctrl.ignoreOnChange ? 'ignoreOnChange="ignoreOnChange" ' : '') + (ctrl.disabled ? 'disabled="disabled"' : '') + ' ' + getCtrlValidationAttribute(ctrl) + ' ' + (ctrl.bindFormUrl ? ('bindFormUrl=' + ctrl.bindFormUrl) : '') + ' style="width: 100%" ' + (ctrl.dataS2 ? 'data-s2="true"' : '') + '  id="' + ctrl.id + '"  data-valuefield="' + ctrl.valuefield + '" data-textfield="' + ctrl.textfield + '" data-url2="' + (ctrl.dataurl ? ctrl.dataurl : '') + '" data-url="' + (ctrl.dataurl ? ctrl.dataurl : '') + '" ' + (!ctrl.moveNameToParent ? 'name="' + ctrl.name + '"' : '') + ' class="form-control" >';
    if (ctrl.values && ctrl.values.length > 0) {
        for (var i = 0; i < ctrl.values.length; i++) {
            result += '<option ' + (ctrl.dfaultValue && ctrl.values[i][ctrl.valuefield] == ctrl.dfaultValue ? 'selected="selected"' : '') + ' value=\'' + ctrl.values[i][ctrl.valuefield] + '\' >' + ctrl.values[i][ctrl.textfield] + '</option>';
        }
    }
    result += '</select>';
    if (ctrl.type == 'dropDown' && !ctrl.moveNameToParent) {
        result += '<div class="myDropdownText myDropdownHeight" ><span class="myDropdownHeight"></span><i ></i></div>';
        result += '<div id="myCtrlDivHolder' + ctrl.id + '_HItems" class="myDropdownItems ' + (ctrl.itemsClass ? ctrl.itemsClass : '') + '"></div>';
    }
    result += '</div>';

    addCtrlToObj(ctrl);
    if (ctrl.holderStatusId) {
        functionsList.push(function () {
            var holderId = this.ctrl.holderStatusId;
            $('#' + this.ctrl.id).change(function () {
                var label = getCtrlLabel(this);
                var value = $(this).find('option:selected').text();
                updateHolderStatusValue(holderId, label, value, $(this).attr('id'));
            });
        }.bind({ ctrl: ctrl }));
    }

    if (ctrl.type == 'dropDown')
        functionsList.push(function () {
            if (this.ctrl.exteraParameterIds) {
                $('#' + this.id)[0].exteraParameterIds = this.ctrl.exteraParameterIds;
            }
            if (!this.moveNameToParent)
                $('#' + this.id).closest('.myDropdown').initMyDropdown();
            initDropdown($('#' + this.id));
            if (this.onChange) {
                $('#' + this.id).change(function () {
                    var tempOnChange = this.sOnChange;
                    if (tempOnChange.indexOf('{{currentIdHolder}}') > -1)
                        tempOnChange = tempOnChange.replace('{{currentIdHolder}}', this.id);
                    eval(tempOnChange);
                }.bind({ sOnChange: this.onChange, id: this.id }));
            }
        }.bind({ id: ctrl.id, moveNameToParent: ctrl.moveNameToParent, onChange: ctrl.onChange, ctrl: ctrl }));
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
                    processResults: function (data) {
                        if (this.ctrl && this.ctrl.updateMapMarker && data) {
                            updateMapMarkers(data.results, this.ctrl.updateMapMarker);
                        }
                        return data;
                    }.bind({ ctrl: ctrl }),
                    data: function (params) {
                        addExteraParameterFromCtrls(this.exteraParameterIds, this.exParam, this.exteraParameterIdsEndWith, this.elementId);
                        this.exParam.search = params.term;
                        this.exParam.page = params.page || 1;
                        return this.exParam;
                    }.bind({ exParam: exteraSelect2Parameters, exteraParameterIds: this.exteraParameterIds, exteraParameterIdsEndWith: this.ctrl.exteraParameterIdsEndWith, elementId: this.ctrl.id })
                }
            };
            if (this.ctrl.disableCondationStyleOnly) {

                select2Option.templateResult = function (item) {
                    if (!item[this.tProp]) {
                        return $('<span style="color:silver;" >' + item.text + '</span>');
                    }
                    return item.text;
                }.bind({ tProp: this.ctrl.disableCondationStyleOnly });
            }
            $('#' + this.id).select2(select2Option);
            setSelect2LabelAria(this.id);
            if (this.exteraParameterIds) {
                for (var i = 0; i < this.exteraParameterIds.length; i++) {
                    $('#' + this.exteraParameterIds[i]).change(function (e) {
                        if (!$('#' + this.curId)[0].startBinding) {
                            var s2Obj = $('#' + this.id).data('select2');
                            if (s2Obj) {
                                s2Obj.val(['']);
                                s2Obj.trigger('change');
                            }
                        }
                    }.bind({ id: this.id, curId: this.exteraParameterIds[i] }));
                }
            }
            if (this.ctrl.showModalCondation) {
                $('#' + this.id).change(function () {
                    showModalOnDynamiCondation(this.ctrl.id, this.ctrl.showModalCondation);
                }.bind({ ctrl: this.ctrl }));
            }
            if (this.onChange) {
                $('#' + this.id).change(function () {
                    var tempOnChange = this.sOnChange;
                    if (tempOnChange.indexOf('{{currentIdHolder}}') > -1)
                        tempOnChange = tempOnChange.replace('{{currentIdHolder}}', this.id);
                    eval(tempOnChange);
                }.bind({ sOnChange: this.onChange, id: this.id }));
            }
            $('#' + this.id).closest('.myCtrl').find('label').click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                var s2Obj = $(this).closest('.myCtrl').find('select').data('select2');
                if (s2Obj) {
                    s2Obj.open();
                }
                return false;
            });
        }.bind({ id: ctrl.id, dataurl: ctrl.dataurl, exteraParameterIds: ctrl.exteraParameterIds, onChange: ctrl.onChange, ctrl: ctrl }));
        functionsList.push(function () {
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
        }.bind({ id: ctrl.id }));
    }

    initDropDownExteraFunctions(ctrl);

    return result;
}

function showModalOnDynamiCondation(ctrlId, showModalCondation) {
    var s2Obj = $('#' + ctrlId).data('select2');
    if (s2Obj) {
        var selectDataResult = $('#' + ctrlId).select2('data');
        if (selectDataResult && selectDataResult.length > 0) {
            if (!selectDataResult[0][showModalCondation['propName']]) {
                var mId = uuidv4RemoveDash();
                var newModalHtml =
                    getModualTemplate({
                        id: mId,
                        title: showModalCondation.title,
                        modelBody: showModalCondation.message
                    });
                $('body').append(newModalHtml);
                $('#' + mId).modal('show');
            }
        }
    }
}

function addExteraParameterFromCtrls(exteraParameterIds, exteraSelect2Parameters, exteraParameterIdsEndWith, elementId) {
    if (exteraParameterIds && exteraParameterIds.length > 0) {
        for (var i = 0; i < exteraParameterIds.length; i++) {
            var qureSelect = $('#' + exteraParameterIds[i]);
            if (exteraParameterIdsEndWith && elementId)
                qureSelect = $('#' + elementId).closest('.MultiRowInputRow').find('[name$=' + exteraParameterIds[i]);
            if (qureSelect.length > 0) {
                if (qureSelect.closest('.tokenBox').length > 0) {
                    exteraSelect2Parameters[qureSelect.closest('.tokenBox').attr('name')] = '';
                    qureSelect.closest('.tokenBox').find('input[type="hidden"]').each(function () {
                        exteraSelect2Parameters[$(this).attr('name')] += $(this).val() + ',';
                    });
                }
                else {
                    if (exteraParameterIdsEndWith)
                        exteraSelect2Parameters[exteraParameterIds[i]] = qureSelect.val();
                    else
                        exteraSelect2Parameters[qureSelect.attr('name')] = qureSelect.val();
                }
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
                if (!$(this)[0].isAutoChangeFProcessing)
                    callInputChangeEventInSameRange($(this).attr('id'));
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

function reinitDropdown(ddId) {
    if ($('#' + ddId).length > 0) {
        $('#' + ddId)[0].resData = null;
        initDropdown($('#' + ddId));
    }
}

function showAndHideCtrl(curValue, showHideCondation, curCtrlId) {
    var defValue = showHideCondation.filter(function (curItem) { return curItem.isDefault == true; });
    if (defValue.length > 0)
        defValue = defValue[0];
    else
        defValue = null;
    if (defValue == null) {
        defValue = showHideCondation.filter(function (curItem) { return curItem.isDefault == true; });
        if (defValue.length == 0)
            defValue = null;
        else
            defValue = defValue[0];
    }
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
    var curCtrl = $('#' + curCtrlId);
    var curHolder = curCtrl.closest('.myPanel');
    if (curCtrl.closest('.panelSWizardHolderContent').length > 0)
        curHolder = curCtrl.closest('.panelSWizardHolderContent');
    var isCtrlVisible = $('#' + curCtrlId).is(':visible');
    var isParentVisible = $('#' + curCtrlId).closest('.myDropdown').length > 0 && $('#' + curCtrlId).closest('.myDropdown').is(':visible');
    if (classObj.classShow && classObj.classShow.length > 0 && (isCtrlVisible || isParentVisible)) {
        for (var i = 0; i < classObj.classShow.length; i++) {
            curHolder.find('.' + classObj.classShow[i]).parent().css('display', 'block');
        }
    }
    if (classObj.classHide && classObj.classHide.length > 0 && (isCtrlVisible || isParentVisible)) {
        for (var i = 0; i < classObj.classHide.length; i++) {
            curHolder.find('.' + classObj.classHide[i]).parent().css('display', 'none');
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
    return '<button ' + (action.id ? 'id="' + action.id + '"' : '') + ' onclick="' + action.onClick + '" type="button" class="btn ' + action.class + '" >' + (action.title ? action.title : action.label) + '</button>';
}

function getStepWizardTemplate(wizard) {
    var result = '';

    if (wizard && wizard.steps && wizard.steps.length > 0) {

        if (!wizard.id)
            wizard.id = uuidv4RemoveDash();

        for (var ii = 0; ii < wizard.steps.length; ii++)
            if (!wizard.steps[ii].id)
                wizard.steps[ii].id = uuidv4RemoveDash();

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
            if (i > 0 && wizard.moveBackButtonToTop)
                result += '<button class="btn btn-warning btn-sm stepButton buttonBack moveToTopSW"><i class="fa fa-chevron-right"></i>بازگشت</button>';
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
                result += '<div class="row">';
                for (var j = 0; j < wizard.steps[i].panels.length; j++) {
                    result += getPanelTemplate(wizard.steps[i].panels[j]);
                }
                result += '</div>';
            }

            if (!wizard.steps[i].submitUrl) {
                result += '<div class="panelSWizardHolderContentHolderStepButton">';
                if (i > 0) {
                    if (!wizard.moveBackButtonToTop)
                        result += '<button class="btn btn-warning btn-sm stepButton buttonBack"><i class="fa fa-chevron-right"></i>بازگشت</button>';

                }
                var isLastStep = (i + 1) >= wizard.steps.length;
                if (!wizard.steps[i].hideMoveNextButton)
                    result += '<button class="btn btn-primary btn-sm stepButton ' + (isLastStep ? 'lastStepButton' : 'buttonConfirm') + ' ">' + (isLastStep ? wizard.lastStepButtonTitle : (wizard.fistStepButtonTitle && i == 0 ? wizard.fistStepButtonTitle : 'ادامه<i class="fa fa-chevron-left"></i>')) + '</button>';
                result += '</div>';
            }


            result += '</div>';
        }

        result += '</div>';
    }

    functionsList.push(function () {
        initSWFunctions(this.wizard.id, this.wizard.actionOnLastStep);
        hideStepByRequest(wizard.steps);
        $('#' + this.wizard.id)[0].wizard = this.wizard;
    }.bind({ wizard: wizard }));

    return result;
}

function convertFormDataToJson(formData) {
    var formDataObj = {};
    if (!formData || formData.constructor != (new FormData()).constructor)
        return formDataObj;
    formData.forEach(function (value, key) {
        if (key.indexOf('[') != -1 && key.indexOf(']') != -1) {
            var propNameLeft = key.split('[')[0];
            var propNameRight = key.split('].')[1];
            var curIndex = Number.parseInt(key.split('[')[1].split(']')[0]);
            if (propNameLeft && propNameRight) {
                if (!formDataObj[propNameLeft])
                    formDataObj[propNameLeft] = [];

                if (formDataObj[propNameLeft].length <= curIndex) {
                    var newObj = {};
                    newObj[propNameRight] = value;
                    formDataObj[propNameLeft].push(newObj);
                } else {
                    var newObj = formDataObj[propNameLeft][curIndex];
                    newObj[propNameRight] = value;
                }
            }
        }
        else {
            if (!formDataObj[key])
                formDataObj[key] = value;
            else {
                if (formDataObj[key].constructor != Array) {
                    var prevValue = formDataObj[key];
                    formDataObj[key] = [];
                    formDataObj[key].push(prevValue);
                }
                formDataObj[key].push(value);
            }
        }
    });

    return formDataObj;
}

function convertToPerisanDate(curDate) {
    try {
        var result = curDate.toLocaleDateString('fa-IR').replace(/([۰-۹])/g, token => String.fromCharCode(token.charCodeAt(0) - 1728));
        var partSplit = result.split('/');
        result = '';
        for (var i = 0; i < partSplit.length; i++) {
            if (partSplit[i].length == 1)
                result += ('0' + partSplit[i] + (i < 2 ? '/' : ''));
            else
                result += (partSplit[i] + (i < 2 ? '/' : ''));
        }
        return result;
    }
    catch {
        return '';
    }
}

function getLastYearFromToday(yearCount) {
    var date = new Date();
    date.setFullYear(date.getFullYear() + yearCount);
    return convertToPerisanDate(date);
}

function getLastDayFromToday(dayCount) {
    var date = new Date();
    date.setDate(date.getDate() + dayCount);
    return convertToPerisanDate(date);
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
                    if (res.data.userfullname) {
                        if (window['userLoginWeb'])
                            userLoginWeb(res.data.userfullname, res.data.isUser);
                        if (window['showLoginUserPanelInMainPage'])
                            showLoginUserPanelInMainPage();
                        if (window['whatToDoAfterUserLogin']) {
                            if (whatToDoAfterUserLogin && whatToDoAfterUserLogin.length > 0) {
                                for (var i = 0; i < whatToDoAfterUserLogin.length; i++) {
                                    whatToDoAfterUserLogin[i].curFun(res.data.userfullname);
                                }
                            }
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

function moveToNextStepForSW(curItem) {
    if (curItem) {
        if ($(curItem).closest('.panelSWizard').length > 0) {
            $(curItem).closest('.panelSWizard')[0].moveNext();
        }
    }
}

function refreshMapIfExist(quirySelector) {
    if ($(quirySelector).find('.olMap').length > 0 && $(quirySelector).find('.olMap')[0].ctrl) {
        $(quirySelector).find('.olMap').html('');
        initMapInner($(quirySelector).find('.olMap')[0].ctrl);
    }
}

function initSWFunctions(curId, actionOnLastStep) {
    if ($('#' + curId).length > 0) {
        $('#' + curId)[0].moveNext = function (ignoreDDChanges) {
            if (!validateForm($(this).find('>.panelSWizardHolderContent>.panelSWizardHolderContentItemActive')))
                return;
            var nextStep = $(this).find('>.panelSWizardHolderHeader>.panelSWizardHolderHeaderItemActive').next();

            while (isShowCondationValid(nextStep) == false)
                nextStep = nextStep.next();
            if (nextStep.length > 0) {
                $(this).find('>.panelSWizardHolderHeader>.panelSWizardHolderHeaderItemActive').removeClass('panelSWizardHolderHeaderItemActive');
                nextStep.addClass('panelSWizardHolderHeaderItemActive');
            }
            var nextContent = $(this).find('>.panelSWizardHolderContent>.panelSWizardHolderContentItemActive').next();
            while (isShowCondationValid(nextContent) == false)
                nextContent = nextContent.next();
            if (nextContent.length > 0) {
                $(this).find('>.panelSWizardHolderContent>.panelSWizardHolderContentItemActive').removeClass('panelSWizardHolderContentItemActive');
                nextContent.addClass('panelSWizardHolderContentItemActive');
                if (!ignoreDDChanges)
                    $(nextContent).find('select:not([ignoreOnChange])').change();
                refreshMapIfExist($(nextContent));
                $([document.documentElement, document.body]).animate({
                    scrollTop: $(nextContent).offset().top - 300
                }, 500);
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
            var nextStep = $(this).find('>.panelSWizardHolderHeader>.panelSWizardHolderHeaderItemActive').prev();
            while (isShowCondationValid(nextStep) == false)
                nextStep = nextStep.prev();
            if (nextStep.length > 0) {
                $(this).find('>.panelSWizardHolderHeader>.panelSWizardHolderHeaderItemActive').removeClass('panelSWizardHolderHeaderItemActive');
                nextStep.addClass('panelSWizardHolderHeaderItemActive');
            }
            var nextContent = $(this).find('>.panelSWizardHolderContent>.panelSWizardHolderContentItemActive').prev();
            while (isShowCondationValid(nextContent) == false)
                nextContent = nextContent.prev();
            if (nextContent.length > 0) {
                $(this).find('>.panelSWizardHolderContent>.panelSWizardHolderContentItemActive').removeClass('panelSWizardHolderContentItemActive');
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
            if ($('#' + actionOnLastStep.objectId).length > 0 && $('#' + actionOnLastStep.objectId)[0].refreshData)
                $('#' + actionOnLastStep.objectId)[0].refreshData();
            else if ($('#' + actionOnLastStep.objectId)[0].tempGridConfig) {
                $('#' + actionOnLastStep.objectId).initMyGrid($('#' + actionOnLastStep.objectId)[0].tempGridConfig);
            }

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
        } else if (actionOnLastStep.actionName == 'showPrintPreview' && actionOnLastStep.objectId && $('#' + actionOnLastStep.objectId).length > 0 && actionOnLastStep.modalId && $('#' + actionOnLastStep.modalId).length > 0 && actionOnLastStep.url) {
            var postData = getFormData($('#' + actionOnLastStep.objectId));
            if (window['exteraModelParams']) {
                for (var prop in exteraModelParams)
                    postData.append(prop, exteraModelParams[prop]);
            }
            showLoader($('#' + actionOnLastStep.objectId));
            postForm(actionOnLastStep.url, postData, function (res) {
                if (res && res.isSuccess != false) {
                    $('#' + actionOnLastStep.modalId).find('.modal-body').html(res);
                    $('#' + actionOnLastStep.modalId).modal('show');
                }
            }, null, function () { hideLoader($('#' + actionOnLastStep.objectId)) });
        } else if (actionOnLastStep.actionName == 'postDataCCMRG' && actionOnLastStep.objectId && $('#' + actionOnLastStep.objectId).length > 0 && actionOnLastStep.url) {
            var formQuery = $('#' + actionOnLastStep.objectId);
            var jsonCofnigUrlQuiry = formQuery.closest('[data-json-config-url]');
            var closestUrl = jsonCofnigUrlQuiry.length > 0 ? jsonCofnigUrlQuiry.attr('data-json-config-url') : location.href;
            var targetUrl = actionOnLastStep.url;
            if (targetUrl.indexOf('****') > -1) {
                var allCurUrl = closestUrl.split('/');
                if (allCurUrl.length > 2)
                    targetUrl = targetUrl.replace('****', allCurUrl[allCurUrl.length - 2]);
            }
            
            var postData = getFormData(formQuery);
            if (window['exteraModelParams']) {
                for (var prop in exteraModelParams)
                    postData.append(prop, exteraModelParams[prop]);
            }
            showLoader(formQuery);
            postForm(targetUrl, postData, function (res) {
                if (res.isSuccess == true) {
                    if (this.actionOnLastStep.gridId) {
                        $('#' + this.actionOnLastStep.gridId)[0].refreshData();
                    }
                    $('#' + this.actionOnLastStep.objectId).closest('.modal').modal('hide');
                }
            }.bind({ actionOnLastStep: actionOnLastStep }), null, function () { hideLoader(formQuery); });
        }
    }
}

function getGridNotificationRefreshAttributes(arrNotifics) {
    var result = '';

    if (arrNotifics && arrNotifics.length > 0) {
        for (var i = 0; i < arrNotifics.length; i++)
            result += ' data-notify-' + arrNotifics[i];
    }

    return result;
}

function getGridTemplate(grid, isInsideModal) {
    var result = '';
    if (grid) {
        var gridId = (!grid.id ? uuidv4RemoveDash() : grid.id);
        result += '<div ' + getGridNotificationRefreshAttributes(grid.notificationTriger) + '  id="' + gridId + '" class="myGridCTRL ' + (grid.class ? grid.class : '') + '"></div>';
        functionsList.push(function () {
            if (!isInsideModal)
                $('#' + gridId).initMyGrid(grid);
            else {
                $('#' + gridId)[0].tempGridConfig = grid;
            }
        });
    }

    return result;
}

function getOs() {
    var userAgent = navigator.userAgent || navigator.vendor || window.opera;
    if (/windows phone/i.test(userAgent))
        return "Windows Phone";
    if (/android/i.test(userAgent))
        return "Android";
    if (/iPad|iPhone|iPod/.test(userAgent) && !window.MSStream)
        return "iOS";

    return "unknown";
}

function reinitCtrls(targetModal) {
    var sQuiry = $('#' + targetModal);
    sQuiry.find('select[reinitonshowmodal]').each(function () {
        $(this)[0].resData = null;
        initDropdown($('#' + $(this).attr('id')), true);
    });

}

function showModal(targetModal, curElement) {
    clearForm($('#' + targetModal));
    $('#' + targetModal)[0].pKey = null;
    $('#' + targetModal).modal('show');
    $('#' + targetModal)[0].grid = $(curElement).closest('.myGridCTRL')[0];
    if ($(curElement).closest('.myGridCTRL').length > 0)
        $('#' + targetModal)[0].gridOwnerId = $(curElement).closest('.myGridCTRL').attr('id');
    if ($(curElement).closest('.modal').length > 0) {
        var pKey = $(curElement).closest('.modal')[0].pKey;
        if (pKey)
            $('#' + targetModal)[0].pKey = pKey;
    }
    if ($(curElement).closest('.gridDetailes').length > 0) {
        var option = $(curElement).closest('.gridDetailes')[0].option;
        if (option && option.exteraParameters && option.exteraParameters.pKey) {
            var pKey = option.exteraParameters.pKey;
            if (pKey)
                $('#' + targetModal)[0].pKey = pKey;
        }
    }
    if ($('#' + targetModal).find('.olMap').length > 0 && $('#' + targetModal).find('.olMap')[0].ctrl) {
        $('#' + targetModal).find('.olMap').html('');
        initMapInner($('#' + targetModal).find('.olMap')[0].ctrl);
    }
    reinitCtrls(targetModal);
}

function initGridIfNotAlreadyInited(curThis) {
    if ($(curThis).length > 0 && !$(curThis)[0].refreshData && $(curThis)[0].tempGridConfig) {
        $(curThis).initMyGrid($(curThis)[0].tempGridConfig);
    }
}

function showEditModal(key, url, modalId, curElement, parentKey, ignoreChange, setParentKeyImeditly, itsHtml, useGridClientData) {
    if (url && modalId) {
        var gridSelector = $(curElement).closest('.myGridCTRL');
        showLoader(gridSelector);
        var postData = new FormData();
        postData.append('id', key);
        if (parentKey)
            postData.append('pKey', parentKey);
        var gridId = gridSelector.attr('id');

        if (setParentKeyImeditly && parentKey)
            $('#' + modalId)[0].pKey = parentKey;

        postForm(url, postData, function (res) {
            if (res) {
                var holderForm = $('#' + this.modalId);
                if (itsHtml) {
                    holderForm.find('.modal-body').html(res);
                    holderForm.modal('show');
                }
                else {
                    clearForm(holderForm);
                    holderForm.modal('show');
                    if ($('#' + modalId).find('.olMap').length > 0 && $('#' + modalId).find('.olMap')[0].ctrl) {
                        $('#' + modalId).find('.olMap').html('');
                        initMapInner($('#' + modalId).find('.olMap')[0].ctrl);
                    }
                    bindForm(res, holderForm, ignoreChange);
                    if (parentKey)
                        $('#' + modalId)[0].pKey = parentKey;
                    else
                        $('#' + modalId)[0].pKey = key;
                    $('#' + modalId)[0].gridOwnerId = gridId;
                    if ($('#' + modalId).find('.myGridCTRL').length > 0) {
                        $('#' + modalId).find('.myGridCTRL').each(function () {
                            if ($(this)[0].refreshData)
                                $(this)[0].refreshData();
                            else
                                initGridIfNotAlreadyInited(this);
                        })
                    }
                }
            }
        }.bind({ modalId }), null, function () { hideLoader(gridSelector); });
    } else if (modalId) {
        if ($('#' + modalId).length > 0) {
            $('#' + modalId)[0].pKey = key;
            if (useGridClientData) {
                var foundTr = $(curElement).closest('[data-row-json]');
                if (foundTr.length > 0) {
                    var jsObj = JSON.parse(foundTr.attr('data-row-json'));
                    bindForm(jsObj, $('#' + modalId));
                }
            }
            $('#' + modalId).modal('show');
            if ($('#' + modalId).find('.myGridCTRL').length > 0) {
                $('#' + modalId).find('.myGridCTRL').each(function () {
                    if ($(this)[0].refreshData)
                        $(this)[0].refreshData();
                    else
                        initGridIfNotAlreadyInited(this);
                })
            }
        }
    }

    reinitCtrls(modalId);
}

function simpleAjax(key, url, curElement) {
    if (url) {
        var gridSelector = $(curElement).closest('.myGridCTRL');
        showLoader(gridSelector);
        var postData = new FormData();
        postData.append('id', key);
        postForm(url, postData, function (res) {
            if (res && res.isSuccess == true) {
                if (gridSelector[0].refreshData)
                    gridSelector[0].refreshData();
            }
        }, null, function () { hideLoader(gridSelector); });
    }
}

function setFocusToFirstVisbleText(qSelector) {
    if (qSelector) {
        qSelector.find('input:visible:eq(0)').focus();
        var modalQuiry = $(qSelector).closest('.modal');
        modalQuiry.animate({ scrollTop: modalQuiry.find('.modal-body').height() }, 300);
    }
}

function updateDashboardGridCountIfExist() {
    $('.tabButtonBox').each(function () {
        if ($(this).find('.tabButtonExistCount').length > 0) {
            addCountNotificationToButtonIfHasGrid($(this).attr('id'));
        }
    });
}

function postButtonAndMakeDisable(curElement, holderInputs, gridId, url, ignoreCloseModal, successUrl, onSuccessFunction, showLoaderId) {
    var curButtonQuiry = $(curElement);
    if (curButtonQuiry.attr('disabled'))
        return;

    $(curElement).attr('disabled', 'disabled');
    postModalData(holderInputs, gridId, url, ignoreCloseModal, successUrl, onSuccessFunction, showLoaderId, function () { $(curElement).removeAttr('disabled') })
}

function postModalData(curElement, gridId, url, ignoreCloseModal, successUrl, onSuccessFunction, showLoaderId, onFinished, exteraModalId) {
    var qSelector = $(curElement).closest('.modal').find('.modal-content');
    if (qSelector.length == 0)
        qSelector = $(curElement);
    if (!gridId && $(curElement).closest('.modal')[0] && $(curElement).closest('.modal')[0].gridOwnerId)
        gridId = $(curElement).closest('.modal')[0].gridOwnerId;
    var postFormData = getFormData($(qSelector));
    if (window['exteraModelParams']) {
        for (var prop in exteraModelParams)
            postFormData.append(prop, exteraModelParams[prop]);
    }
    showLoader(qSelector);
    if (showLoaderId)
        showLoader($('#' + showLoaderId))
    postForm(url, postFormData, function (res) {
        if (res && res.isSuccess == true) {
            if (exteraModalId && $('#' + exteraModalId).length > 0)
                $('#' + exteraModalId).modal('hide');
            updateDashboardGridCountIfExist();
            if (onSuccessFunction)
                onSuccessFunction(res);
            if (successUrl)
                location.href = successUrl;
            if (!this.ignoreCloseModal) {
                closeThisModal(this.curElement);
            } else {
                clearForm(qSelector);
                setFocusToFirstVisbleText(qSelector);
            }
            if (this.gridId == 'allGrid') {
                refreshAllGrid();
            } else if (this.gridId && $('#' + this.gridId).length > 0 && $('#' + this.gridId)[0].refreshData) {
                $('#' + this.gridId)[0].refreshData();
            }
        }
    }.bind({ gridId, curElement, ignoreCloseModal: ignoreCloseModal }), null, function () {
        hideLoader(qSelector);
        if (showLoaderId)
            hideLoader($('#' + showLoaderId));
        if (onFinished)
            onFinished();
    });
}

function refreshGrid(gridId, currButtonInsideModal) {
    closeThisModal(currButtonInsideModal);
    if ($('#' + gridId).length > 0 && $('#' + gridId)[0].refreshData)
        $('#' + gridId)[0].refreshData();
}

function refreshGridAndShow(gridId, curButton) {
    var sQuiry = $('#' + gridId);
    $(curButton).closest('.myCtrl').find('input[type=hidden]').remove();
    $(curButton).append('<input type="hidden" name="' + $(curButton).attr('data-button-name') + '" value="' + $(curButton).attr('data-button-value') +'" />')
    if (sQuiry.length > 0 && sQuiry[0].refreshData) {
        sQuiry[0].refreshData();
        sQuiry.show();
    }
}

function postPanel(curElement, url, exteraParameters, clearPanelAfter) {
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
        postForm(url, postData, function (res) {
            if (this.clearPanelAfter && res.isSuccess) {
                clearForm(this.foundPanel);
            }
        }.bind({ foundPanel: foundPanel, clearPanelAfter: clearPanelAfter }), null, function () { hideLoader(foundPanel) });
    }
}

function closeThisModal(curElement) {
    if (curElement) {
        $(curElement).closest('.modal').modal('hide');
    }
}

function hasValidValueProperty(input, res) {
    if (res && res.length > 0) {
        for (var i = 0; i < res.length; i++) {
            if (res[i].key == input)
                if (res[i].value && res[i].value != '0')
                    return true;
        }
    }
    return false;
}

function postBindSW(curElement, url) {
    if (curElement && url && $(curElement).length > 0) {
        var targetObj = $(curElement).closest('.panelSWizard');
        if (targetObj.length > 0) {
            showLoader(targetObj);
            postForm(url, getFormData(targetObj), function (res) {
                if (res && res.isSuccess == undefined) {
                    if (
                        hasValidValueProperty('vehicleTypeId', res),
                        hasValidValueProperty('carTypeId', res),
                        hasValidValueProperty('brandId', res),
                        hasValidValueProperty('specId', res)

                    ) {
                        bindForm(res, targetObj, true);
                        targetObj[0].moveNext(true);
                    }
                    else {
                        $.toast({
                            heading: 'خطا',
                            position: 'bottom-right',
                            textAlign: 'right',
                            text: 'پاسخ مناسب یافت نشد',
                            showHideTransition: 'slide',
                            icon: 'error'
                        });
                    }

                }
            }, null, function () {
                hideLoader(targetObj);
            });
        }
    }
}

function bindPanelByUrl(querySelector) {
    if (querySelector.length > 0) {
        var dataURl = $(querySelector).attr('data-url');
        if (dataURl) {
            var formData = new FormData();
            showLoader(querySelector);
            postForm(dataURl, formData, function (res) {
                bindForm(res, querySelector, true);
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

function bindContentByUrl(url, arrInputNames, contentId) {
    if (contentId && arrInputNames && url && arrInputNames.length > 0 && $('#' + contentId).length > 0) {
        var postData = new FormData();
        var hasValueCount = 0;
        for (var i = 0; i < arrInputNames.length; i++) {
            var curName = arrInputNames[i];
            if ($('input[name=' + curName + ']').length > 0) {
                if ($('input[name=' + curName + ']').val())
                    hasValueCount++;
                postData.append(curName, $('input[name=' + curName + ']').val());
            } else if ($('select[name=' + curName + ']').length > 0) {
                var curValue = $('select[name=' + curName + ']').find('option:selected').attr('value');
                if (curValue)
                    hasValueCount++;
                postData.append(curName, curValue);
            }
        }
        if (hasValueCount == arrInputNames.length)
            postForm(url, postData, function (res) {
                if (res && res.isSuccess == undefined) {
                    bindForm(res, $('#' + contentId), true);
                }
            });
    }
}

function updateMapFromDropdown(dropdownId, mapId) {
    var ddSelector = $('#' + dropdownId);
    var mapSelector = $('#' + mapId);
    if (ddSelector.length > 0 && mapSelector.length > 0) {
        var ddOption = ddSelector.find('option:selected');
        var mapzoom = ddOption.attr('data-mapzoom');
        var maplon = ddOption.attr('data-maplon');
        var maplat = ddOption.attr('data-maplat');
        if (mapzoom && maplon && maplat && mapzoom != 'null' && maplon != 'null' && maplat != 'null') {
            var holderMap = mapSelector.parent();
            clearForm(holderMap);
            if ($(holderMap).find('.olMap').length > 0 && $(holderMap).find('.olMap')[0].ctrl) {
                $(holderMap).find('.olMap').html('');
                initMapInner($(holderMap).find('.olMap')[0].ctrl);
            }
            bindForm({ mapLatRecivePlace_lat: maplat, mapLonRecivePlace_lon: maplon, mapZoomRecivePlace_zoom: mapzoom }, holderMap);
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
                    if ($(curButton).closest('.myGridCTRL').length > 0 && $(curButton).closest('.myGridCTRL')[0].refreshData)
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

function needToBeLoginFirst(curButton) {
    if (window['isUserLogin'] != undefined) {
        if (!isUserLogin) {
            $('.holderRigAndLogUser .secountBtn').click();
            whatToDoAfterUserLogin.push({
                curFun: function () {
                    location.href = $(this.curButton).attr('href');
                }.bind({ curButton: curButton })
            });
            return false;
        }
    }
    return true;
}

function loadJsonConfig(jsonUrl, targetId, whatToDoAfterFinished, rType) {
    if (!rType)
        rType = 'POST';
    var postData = new FormData();
    if (window['exteraModelParams'])
        for (var prop in exteraModelParams)
            postData.append(prop, exteraModelParams[prop]);
    var holderQuiry = null;
    if (typeof targetId === 'string' || targetId instanceof String) {
        holderQuiry = $('#' + targetId);
    } else {
        holderQuiry = $(targetId);
    }
    showLoader(holderQuiry);
    postForm(jsonUrl, postData, function (res) {
        generateForm(res, this.targetId, null, jsonUrl);
        if (!this.targetId) {
            $('.mainLoaderForAdminArea').addClass('mainLoaderForAdminAreaClose');
            setTimeout(function () {
                $('.mainLoaderForAdminArea').css('display', 'none');
            }, 200);
        }

        if (this.whatToDoAfterFinished) {
            this.whatToDoAfterFinished();
        }
    }.bind({ targetId: targetId, whatToDoAfterFinished: whatToDoAfterFinished }), null, function () { hideLoader(this); }.bind(holderQuiry) , null, rType);
}

function refreshAllGrid() {
    $('.myGridCTRL').each(function ()
    {
        var curElement = $(this)[0];
        if (curElement && curElement.refreshData) {
            curElement.refreshData();
        }
    });
}