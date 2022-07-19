function isScrollShowed() {
    return $(document).height() > $(window).height();
}

function bindForm(res, selector, ignoreChanges) {
    if (res && res.length > 0 && res.constructor == Array && $(selector).length > 0) {
        for (var i = 0; i < res.length; i++) {
            bindingForm(selector, res[i].key, res[i].value, ignoreChanges, res);
        }
    } else if (res && $(selector).length > 0) {
        for (var proerty in res) {
            bindingForm(selector, proerty, res[proerty], ignoreChanges, res);
        }
    }
}

function removeToken(curObj) {
    var qure = $(curObj).closest('.tokenBoxItem');
    if (qure.hasClass('ziroHeight'))
        return;
    qure.addClass('ziroHeight');
    setTimeout(function () {
        $(this).closest('.tokenBoxItem').remove();
    }.bind(curObj), 310)
}

function initTokenBox(curObj) {
    var curItemName = $(curObj).attr('name');
    $(curObj)[0].template = '<span class="tokenBoxItem ziroHeight" ><input name="{{name}}" type="hidden" value="{{value}}" />{{text}}<span class="tokenBoxItemRemoveIcon" onclick="removeToken(this)" ><i class="fa fa-trash" ></i></span></span>';
    $(curObj).find('select').change(function () {
        var selectedText = $(this).find('option:selected').text();
        var selectedValue = $(this).find('option:selected').val();
        if (!selectedText && !selectedValue)
            return;
        if (!selectedValue)
            selectedValue = selectedText;
        $(this).val(null);
        if ($(this).data('select2')) {
            $(this).data('select2').val(['']);
            $(this).data('select2').trigger('change');
        }
        if ($(curObj)[0].isSelected(selectedValue) == false) {
            $(curObj)[0].addNewToken(selectedText, selectedValue, curItemName);
        } else {
            removeToken($('input[value="' + selectedValue + '"]'));
        }
    });

    $(curObj).find('select').focusin(function () {
        $(this)[0].size = 10;
        $(this).addClass('makeBigger')
    });

    $(curObj).find('select').focusout(function () {
        $(this)[0].size = 0;
        $(this).removeClass('makeBigger')
    });

    $(curObj)[0].isSelected = function (value) {
        return $(this).find('input[value="' + value + '"]').length;
    };

    $(curObj)[0].addNewToken = function (text, value, name) {
        if (value) {
            var template = $(this)[0].template;
            template = template.replace('{{text}}', text).replace('{{value}}', value).replace('{{name}}', name);
            $(this).append(template);
            setTimeout(function () {
                $(this).find('.ziroHeight').removeClass('ziroHeight');
            }.bind(this), 50);
        }

    };
}

function updateMappIfNeeded(curQuiry) {
    if (curQuiry.closest('.mapCtrl').length > 0 && curQuiry.closest('.mapCtrl').find('.olMap').length > 0) {
        var mapElement = curQuiry.closest('.mapCtrl').find('.olMap')[0]
        mapElement.updateMapAndZoomPoint();
    }
}

function bindingForm(selector, key, value, ignoreChanges, res) {
    if (key && key.indexOf('[') > 0 && key.indexOf(']') > 0) {
        var newKey = key.split('[')[0];
        var foundIndex = Number.parseInt(key.split('[')[1].split(']')[0]);
        $(selector).find('div[data-name="' + newKey + '"]').each(function () {
            $(this)[0].addNewRowIfNeeded(foundIndex + 1);
        })
    }

    $(selector).find('video[name="' + key + '"]').each(function () {
        $(this).find('source').attr('src', value);
        $(this).load();
    })
    $(selector).find('textarea[name="' + key + '"]').each(function () {
        if ($(this)[0].ckEditor) {
            $(this)[0].ckEditor.setData((!value ? '' : value));
        } else {
            $(this).val(value);
        }

    });

    $(selector).find('[data-spec-name="' + key + '"]').each(function () {
        if ($(this)[0].addNewRow)
            $(this)[0].addNewRow(value);
    });

    $(selector).find('a[data-name="' + key + '"]').each(function () {
        if (value)
            $(this).html('(دانلود فایل)').attr('href', value);
        else
            $(this).html('').attr('href', '');
    });

    $(selector).find('.tokenBox[name="' + key + '"]').each(function () {

        if (value && value.constructor == Array) {
            var curName = $(this).attr('name');
            var allSource = [];
            $(this).find('option').each(function () {
                allSource.push({ id: $(this).attr('value'), title: $(this).text() });
            });
            for (var i = 0; i < value.length; i++) {
                var foundItem = allSource.filter(function (item) { return item.id == value[i]; });
                if (foundItem && foundItem.length > 0) {
                    $(this)[0].addNewToken(foundItem[0].title, foundItem[0].id, curName);
                } else if (value[i].id && value[i].title) {
                    $(this)[0].addNewToken(value[i].title, value[i].id, curName);
                }
            }
        }
    });
    $(selector).find('section[data-name="' + key + '"]').each(function () {
        $(this).html(value);
    });
    $(selector).find('select[name="' + key + '"]').each(function () {
        $(this)[0].bindValue = value;
        $(this).val(value + '');
        if (!$(this).val()) {
            var foundItemByTextValue = '';

            $(this).find('option').each(function () {
                if (value && $(this).text() && $(this).text().trim() == (value + '').trim()) {
                    foundItemByTextValue = $(this).attr('value');
                }
            });
            $(this).val(foundItemByTextValue);
        }
        if ($(this).data('select2')) {
            if (res[key + '_Title'])
                $(this).append(new Option(res[key + '_Title'], value, true, true)).trigger('change');
        }
        if (!ignoreChanges) {
            setTimeout(function () {
                $(this).change();
            }.bind(this), 100);
        }


    });
    $(selector).find('img[data-name="' + key + '"]').each(function () {
        $(this).attr('src', value);
    })
    $(selector).find('input[name="' + key + '"]').each(function () {
        var type = 'text';
        var curType = $(this).attr('type');
        if (curType) {
            type = curType;
        }
        switch (type) {
            case 'hidden':
            case 'text':
            case 'color':
                if ($(this).closest('.tokenBox').length == 0) {
                    $(this).val(value);
                    if ($(this).closest('.myColorPicker').length > 0) {
                        $(this).closest('.myColorPicker').data('colorPickerByGiro_data').setValue(value)
                    }
                }
                updateMappIfNeeded($(this));
                break;
            case 'checkbox':
                if (value == 'True' || value == true || value == false || value == 'False' || value == '') {
                    var valueBool = false;
                    if (value == 'True' || value == true) {
                        valueBool = true;
                    }
                    $(this).prop('checked', valueBool);
                } else if (value.constructor == Array) {
                    for (var i = 0; i < value.length; i++) {
                        if ($(this).attr('value') == value[i]) {
                            $(this).prop('checked', true);
                        }
                    }
                } else {
                    if (value == $(this).val())
                        $(this).prop('checked', true);
                }
                break;
            case 'radio':
                if ($(this).val() == value)
                    $(this).prop('checked', true);
                else
                    $(this).prop('checked', false);
                if (!ignoreChanges) {
                    setTimeout(function () {
                        $(this).change();
                    }.bind(this), 100);
                }
                break;
        }
    });

    updateCtrlStatus();
    if (window['updateAllSelectByInnerSelect'])
        updateAllSelectByInnerSelect(selector);
}

function updateCtrlStatus() {
    $('input,select').each(function () {
        if ($(this)[0].updateStatus)
            $(this)[0].updateStatus();
    });
}

function bindDropdown(curObj, data, textField, valueField) {
    $(curObj).html('');
    var bValue = $(curObj)[0].bindValue;
    var hasSelect2 = $(curObj).attr('data-s2');
    var select2Opening = $(curObj).attr('data-s2-on-opening');
    var select2Closing = $(curObj).attr('data-s2-on-closeing');
    for (var i = 0; i < data.length; i++) {
        var isSelected = '';
        if (data[i][valueField] + '' == bValue || data[i][textField] + '' == bValue) {
            isSelected = 'selected=selected';
        }
        var curRowTemplate = '<option ' + isSelected + ' value="' + data[i][valueField] + '" ';

        for (var prop in data[i]) {
            curRowTemplate += ' data-' + prop + '="' + data[i][prop] + '" ';
        }

        curRowTemplate += '>';
        curRowTemplate += data[i][textField] + '</option>';
        $(curObj).append(curRowTemplate);
    }
    if (hasSelect2) {
        $(curObj).select2({

        });
        if (select2Opening) {
            $(curObj).on('select2:opening', function () { window[this.evName](); }.bind({ evName: select2Opening }));
        }

        if (select2Closing) {
            $(curObj).on('select2:closing', function () { window[this.evName](); }.bind({ evName: select2Closing }));
        }
    } else {
        if ($(curObj).closest('.myDropdown').length > 0) {
            $(curObj).closest('.myDropdown')[0].updateItemFromSelect();
        }
    }
}

var allRequestQ = [];
var allRequestRes = [];
var allPendingCTRLs = [];

function isExistInRequestQ(url) {
    return allRequestQ.filter(function (item) { return item.url == url }).length > 0;
}

function fillResponse(url, data) {
    var foundDatas = allRequestRes.filter(function (item) { return item.url == url; });
    if (foundDatas.length == 1) {
        foundDatas[0].data = data;
    }
    else {
        var temp = { data: data, url: url };
        allRequestRes.push(temp);
    }
}

function bindingPendingCTRLs(url) {
    var allUrlPendingCTRLs = allPendingCTRLs.filter(function (item) { return item.url.toLowerCase() == url.toLowerCase(); });
    var foundCData = allRequestRes.filter(function (item) { return item.url == url });
    if (foundCData.length > 0) {
        for (var i = 0; i < allUrlPendingCTRLs.length; i++) {
            $(allUrlPendingCTRLs[i].curObj)[0].resData = foundCData[0].data;
            bindDropdown(allUrlPendingCTRLs[i].curObj, foundCData[0].data, allUrlPendingCTRLs[i].textField, allUrlPendingCTRLs[i].valueField);
        }
    }
}

function initDropdown(curObj, dontUseCache, parentValue) {
    var url = $(curObj).attr('data-url');
    var textField = $(curObj).attr('data-textfield');
    var valueField = $(curObj).attr('data-valuefield');
    var hasAttrBindFormUrl = $(curObj).attr('bindFormUrl');
    if (url && textField && valueField) {
        var postData = new FormData();
        if ($(curObj).closest('.modal').length > 0 && $(curObj).closest('.modal')[0].pKey)
            postData.append('pKey', $(curObj).closest('.modal')[0].pKey);
        if (dontUseCache == true || isExistInRequestQ(url) == false) {
            allRequestQ.push({ url: url });
            if (parentValue)
                postData.append('id', parentValue);
            if (url.indexOf("/Core/BaseData") == -1 && window['exteraModelParams'])
                for (var prop in exteraModelParams)
                    postData.append(prop, exteraModelParams[prop]);
            postForm(url, postData, function (res) {
                $(this.curObj)[0].resData = res;
                fillResponse(this.url, res);
                bindDropdown(this.curObj, $(this.curObj)[0].resData, this.textField, this.valueField);
                bindingPendingCTRLs(this.url);
                if (hasAttrBindFormUrl)
                    $(this.curObj).change();

            }.bind({ curObj: curObj, url: url, textField: textField, valueField: valueField }));
        } else {
            var foundData = allRequestRes.filter(function (item) { return item.url == url });
            if (foundData && foundData.length > 0) {
                $(curObj)[0].resData = foundData[0].data;
                bindDropdown(curObj, foundData[0].data, textField, valueField);
                if (hasAttrBindFormUrl)
                    $(curObj).change();
            } else {
                allPendingCTRLs.push({ curObj: curObj, textField: textField, valueField: valueField, url: url });
            }
        }
    }
}

function getFileInputImage(inputQuiry) {
    if (inputQuiry && inputQuiry.closest('.myFileUpload').length > 0 && inputQuiry.closest('.myFileUpload').find('.holderUploadImage img').length > 0)
        return inputQuiry.closest('.myFileUpload').find('.holderUploadImage img')[0];

    return null;
}

function compressImageAndAddToFormData(file, img, targetImage) {
    var canvas = document.createElement('canvas');
    var fileName = file.name.split('.')[0];
    var width = img.width;
    var height = img.height;
    var maxWith = 1900;
    var maxHeight = 1900;

    if (width > maxWith) {
        height = Math.round(height *= maxWith / width);
        width = maxWith;
    } else if (height > maxHeight) {
        width = Math.round(width *= maxHeight / height);
        height = maxHeight;
    }

    canvas.width = width;
    canvas.height = height;
    var ctx = canvas.getContext('2d');
    ctx.drawImage(img, 0, 0, width, height);
    canvas.toBlob(function (blob) {
        var f2 = new File([blob], fileName + ".jpg", { type: "image/jpg" });
        targetImage.compressUploadFile = f2;
    }, 'image/jpeg', 0.9);
}

function getFormData(selector) {
    var postData = new FormData();

    if ($(selector).closest('.modal').length > 0) {
        var pKey = $(selector).closest('.modal')[0].pKey;
        if (pKey)
            postData.append('pKey', pKey);
    }

    $(selector).find('textarea').each(function () {
        var curName = $(this).attr('name');
        if (curName) {
            if ($(this)[0].ckEditor) {
                postData.append(curName, $(this)[0].ckEditor.getData());
            } else {
                postData.append(curName, $(this).val());
            }
        }
    });

    $(selector).find('select').each(function () {
        var curName = $(this).attr('name');
        var s2Obj = $(this).data('select2');
        if (s2Obj) {
            var curValue = s2Obj.val();
            if (curName) {
                postData.append(curName, curValue);
            }
        } else {
            var curValue = $(this).find('option:selected').val();
            var curValueText = $(this).find('option:selected').text();
            if (curName && curValue) {
                postData.append(curName, curValue);
                postData.append(curName + '_Title', curValueText);
            }
        }

    });

    $(selector).find('input').each(function () {
        var type = 'text';
        var name = $(this).attr('name');
        var curCTRLType = $(this).attr('type');
        if (curCTRLType) {
            type = curCTRLType;
        }
        type = type.toLowerCase();
        if (name) {
            switch (type) {
                case 'password':
                case 'text':
                case 'hidden':
                case 'color':
                    postData.append(name, $(this).val())
                    break;
                case 'file':
                    var curFileObj = $(this)[0].files;
                    if (curFileObj && curFileObj.length > 0) {
                        var targetImage = getFileInputImage($(this));
                        if ($(this).attr('data-compressImage') == 'true' && (/image/i).test(curFileObj[0].type) && targetImage) {
                            if (targetImage.compressUploadFile) {
                                postData.append(name, targetImage.compressUploadFile)
                            } else
                                postData.append(name, $(this)[0].files[0])
                        } else
                            postData.append(name, $(this)[0].files[0])
                    } else {
                        var targetImage = getFileInputImage($(this));
                        if (targetImage.compressUploadFile && targetImage.useWithouFile) {
                            postData.append(name, targetImage.compressUploadFile)
                        }
                    }
                    break;
                case 'checkbox':
                    if ($(this).prop('checked')) {
                        postData.append(name, $(this).val())
                    }
                    break;
                case 'radio':
                    if ($(this).prop('checked')) {
                        postData.append(name, $(this).val());
                    }
                    break;
            }
        }

    });

    return postData;
}

var allCacheURLs = [
    '/core/basedata/get', '/getjsonconfig', '/TopMenu/GetTopMenu', '/Home/GetAboutUsMainPage', '/Home/GetReminderConfig', '/Reminder/GetMainPageDescription',
    '/Home/GetOurPrideMainPage', '/Home/GetFooterDescrption', '/Home/GetFooterInfor', '/GetCompanyList', '/GetCreateDateList', '/Blog/Blog/GetMainBlog', '/Home/GetLoginModalConfig',
    '/Home/GetFooterExteraLink', '/Home/GetFooterExteraLinkGroup', '/Home/GetFooterSambole', '/Question/YourQuestion/GetList'
];

function isValidURL(url) {
    var foundURl = allCacheURLs.filter(function (item) { return url.toLowerCase().indexOf(item.toLowerCase()) > -1; })
    return foundURl && foundURl.length > 0
}

function cacheLocalstorage(url, data) {
    var localStorage = window.localStorage;
    var foundCookie = getCookie('lang');
    if (localStorage) {
        if (url && isValidURL(url)) {
            localStorage.setItem((url + '_' + foundCookie), JSON.stringify({ url: url, data: data, sDate: new Date() }));
        }
    }
}

function getDataFromLocalStorageCache(url) {
    var foundCookie = getCookie('lang');
    var localStorage = window.localStorage;
    if (localStorage) {
        if (url && isValidURL(url)) {
            var resData = localStorage.getItem((url + '_' + foundCookie));
            if (resData) {
                resData = JSON.parse(resData);
                resData.sDate = new Date(resData.sDate);
                if (resData.sDate.getDay() == new Date().getDay()) {
                    return resData.data;
                }
            }
        }
    }
    return null;
}


function delete_cookie(name) {
    document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function postForm(url, postData, success, error, completeEvent, ignoreAutoToast) {
    var foundDataFromCache = getDataFromLocalStorageCache(url);
    if (foundDataFromCache) {
        if (success)
            success(foundDataFromCache);
        if (completeEvent)
            completeEvent();
    } else {
        $.ajax({
            url: url,
            data: postData,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (res) {
                cacheLocalstorage(this.url, res);
                if (success) {
                    success(res);
                    if (res) {
                        if (res.isSuccess == true || res.isSuccess == false) {
                            if (!ignoreAutoToast)
                                $.toast({
                                    heading: res.isSuccess == false ? 'خطا' : 'موفقیت',
                                    text: res.message,
                                    textAlign: 'right',
                                    position: 'bottom-right',
                                    showHideTransition: 'slide',
                                    icon: res.isSuccess == false ? 'error' : 'success'
                                })
                            if (res.errorCode == 5) {
                                if ($('[data-captcha]').length > 0 && $('[data-captcha]')[0].refreshCode) {
                                    $('[data-captcha]')[0].refreshCode();
                                }
                            }
                        }
                    }
                    if (res && res.fileData && res.fileName) {
                        var blob = new Blob([res.fileData])
                        const link = document.createElement('a');
                        link.href = URL.createObjectURL(blob);
                        link.download = res.fileName;
                        document.body.append(link);
                        link.click();
                        link.remove();
                        window.addEventListener('focus', e => URL.revokeObjectURL(link.href), { once: true });
                    }
                }
                if (res && res.isSuccess && res.data && res.data.action) {
                    if (res.data.action == 'redirect' && res.data.url)
                        location.href = res.data.url;

                }
            }.bind({ url: url }),
            error: function (xhr) {

                if (error) {
                    error();
                }
                if (xhr.status == 403) {
                    $.toast({
                        heading: 'عدم دسترسی',
                        position: 'bottom-right',
                        textAlign: 'right',
                        text: 'دسترسی شما به این بخش محدود می باشد',
                        showHideTransition: 'slide',
                        icon: 'error'
                    });
                    return;
                }

                $.toast({
                    heading: 'خطا',
                    position: 'bottom-right',
                    textAlign: 'right',
                    text: 'خطای غیر قابل پیشبینی لطفا با بخش ادمین تماس حاصل کنید',
                    showHideTransition: 'slide',
                    icon: 'error'
                });
            },
            complete: function () {
                if (completeEvent) {
                    completeEvent();
                }
            },
        });
    }

}

function showLoader(selector, setRelative) {
    if ($(selector).find('.loaderPanel').length == 0) {
        if (!setRelative)
            $(selector).css('position', 'relative');
        $(selector).append('<div class="loaderPanel"><div class="loader"></div></div>');
    }
}

function hideLoader(selector) {
    $(selector).find('.loaderPanel').remove()
}

function clearForm(selector) {
    $(selector).find('select').each(function () {
        var s2Obj = $(this).data('select2');
        if (s2Obj) {
            s2Obj.val(['']);
            s2Obj.trigger('change');
        }
    });
    $(selector).find('img[data-name]').attr('src', '/Modules/Images/unknown.svg');
    $(selector).find('input[type="text"]').not('[data-no-clear]').val('');
    $(selector).find('input[type="hidden"]').not('[data-no-clear]').val('');
    $(selector).find('input[type="color"]').not('[data-no-clear]').val('');
    $(selector).find('input[type="radio"]').prop('checked', false);
    $(selector).find('input[type="checkbox"]').prop('checked', false);
    $(selector).find('input[type="file"]').not('[data-no-clear]').val(null);
    $(selector).find('input[type="file"]').not('[data-no-clear]').change();
    $(selector).find('select').val('');
    $(selector).find('a[data-name]').html('').removeAttr('href');
    $(selector).find('select').each(function () {
        $(this)[0].bindValue = null;
    });
    $(selector).find('.tokenBox').find('.tokenBoxItem').remove();
    $(selector).find('textarea').val('');
    $(selector).find('textarea').each(function () {
        if ($(this)[0].ckEditor) {
            $(this)[0].ckEditor.setData('');
        }
    });

    updateCtrlStatus();
    $(selector).find('.myDropdown').each(function () {
        if ($(this)[0].updateItemFromSelect)
            $(this)[0].updateItemFromSelect();
    });
}

function expandGridThisRow(curElement) {
    if (curElement) {
        closeAllGridDetailes(curElement);
        var currentWith = $(window).width();
        if (currentWith <= 650) {
            if ($(curElement).prev().hasClass('floatGridDetiles')) {
                $(curElement).html('جزییات بیشتر<i ></i>');
                $(curElement).removeClass('gridDetialseMakeLessShow');
                $(curElement).prev().remove();
                $(curElement).closest('tr').after('<tr></tr>');
                $(curElement).closest('tr').find('.gridExpandButton').click();
            }
            else {
                $(curElement).html('جزییات کمتر<i ></i>');
                $(curElement).addClass('gridDetialseMakeLessShow');
                $(curElement).closest('tr').find('.gridExpandButton').click();
                var templateDetailes = '<div class="floatGridDetiles">' + $(curElement).closest('tr').next().find('.gridDetailes').clone().html() + '</div>';
                $(curElement).before(templateDetailes);
                $(curElement).closest('tr').next().remove();
            }
        }
        else {
            $(curElement).closest('tr').find('.gridExpandButton').click();
        }
    }
}

function closeAllGridDetailes(curElement) {
    var currentWith = $(window).width();
    if (currentWith <= 650) {
        $(curElement).closest('table.myGrid').find('>tbody.myTableBody>tr').each(function () {
            if ($(this).find('>td>span.gridExpandButton.fa-minus-square').length > 0) {
                if ($(this).find('.btn-roundMoreGridDetailes')[0] != curElement)
                    $(this).find('.btn-roundMoreGridDetailes').click();
            }
        });
    }
}

function closeThisDetailes(curElement) {
    if (curElement) {
        $(curElement).closest('.holderDetailesGrid').prev().find('.gridExpandButton').click();
    }
}

function confirmDialog(title, message, posetiveEvent, negativeEvent) {
    var newId = "confirmDialog_" + Math.random();
    newId = newId.replace('.', '');
    var template = `
        <div id="`+ newId + `" class='confirmDialogBG'>
            <div class='dialogBox'>
                <div class="dialogBoxTitle" >`+ title + `</div>
                <div class="dialogBoxBody">
                    <p>`+ message + `</p>
                </div>
                <div class="confirmDialogAction">
                    <button class='btn btn-success btn-sm'>تایید</button>
                    <button class='btn btn-primary btn-sm' >انصراف</button>
                </div>
            </div>
        </div>
    `;
    $('body').append(template);
    $('#' + newId).find('.btn-success').click(function () {
        if (posetiveEvent) {
            posetiveEvent();
        }
        $(this).closest('.confirmDialogBG').remove();
    });
    $('#' + newId).find('.btn-primary').click(function () {
        if (negativeEvent) {
            negativeEvent();
        }
        $(this).closest('.confirmDialogBG').remove();
    });
}

function convertFormDataToJsonOject(formData) {
    var result = {};

    for (var item of formData) {
        result[item[0]] = item[1];
    }

    return result;
}

function openNewLink(holderParametersId, link, checkUrl, makePost) {
    if (link && holderParametersId && $('#' + holderParametersId).length > 0 && checkUrl) {
        var formData = getFormData($('#' + holderParametersId));
        showLoader($('#' + holderParametersId));
        postForm(checkUrl, formData, function (res) {
            if (res && res.isSuccess) {
                if (!makePost) {
                    var qureString = '';
                    for (var pair of this.formData.entries()) {
                        if (qureString == '' && this.link.indexOf('?') == -1) {
                            qureString = '?' + pair[0] + '=' + pair[1];
                        } else {
                            qureString = qureString + '&' + pair[0] + '=' + pair[1];
                        }
                    }
                    location.href = this.link + qureString;
                } else {
                    postPage(link, this.formData);
                }

            }
        }.bind({ checkUrl: checkUrl, formData: formData, link: link }), null, function () { hideLoader($('#' + holderParametersId)); });
    } else if (link && holderParametersId && $('#' + holderParametersId).length > 0) {
        var formData = getFormData($('#' + holderParametersId));
        var qureString = '';
        for (var pair of formData.entries()) {
            if (qureString == '' && link.indexOf('?') == -1) {
                qureString = '?' + pair[0] + '=' + pair[1];
            } else {
                qureString = qureString + '&' + pair[0] + '=' + pair[1];
            }
        }
        location.href = link + qureString;
    }
}



function postPage(link, formData) {
    var formId = uuidv4RemoveDash();
    var tempResult = '<form id="' + formId + '" action="' + link + '" style="display:none;" method="post">';

    for (var pair of formData.entries()) {
        tempResult += '<input type="hidden" value="' + pair[1] + '" name="' + pair[0] + '" />';
    }

    tempResult += '</form>';

    $('body').append(tempResult);
    $('#' + formId).submit();
}

$.fn.modal = function (action) {
    function closeModal(curThis) {
        setTimeout(function () {
            $(curThis).css('display', 'none');
            if (!$('.modal').is(':visible')) {
                $('body').removeClass('modal-open').find('.modal-backdrop').remove();
            }
        }, 100);
        $(curThis).removeClass('show');
        $('body').find('.modal-backdrop:last-child').removeClass('show')
    }
    function openModal(curThis) {
        $(curThis).css('display', 'block');
        setTimeout(function () {
            $(curThis).addClass('show');
        }, 100);
        $(curThis).click(function () {
            $(this).modal('hide');
        });
        $(curThis).find('.modal-dialog').click(function (e) { e.stopPropagation(); if (window['closeAllDropdownInPage']) closeAllDropdownInPage(); });
        $('body').addClass('modal-open').append('<div class="modal-backdrop fade show"></div>');
        setTimeout(function () { $(curThis).find('input:visible:eq(0)').focus(); $(curThis).find('input:visible:eq(0)').click(); }, 200);
    }
    function bindCloseButton(curThis) {
        $(curThis).find('[data-dismiss]').unbind().click(function () {
            $(this).closest('.modal').modal('hide');
        });
    }
    return this.each(function () {
        var curThis = this;
        if (action == 'hide') {
            closeModal(curThis);
        } else {
            openModal(curThis);
            bindCloseButton(curThis);
        }
    });
};