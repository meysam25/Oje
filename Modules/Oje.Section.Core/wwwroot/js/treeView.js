

$.fn.initTreeView = function () {
    return this.each(function () {
        $(this)[0].openButton = function (curObj) {
            if ($(curObj).hasClass('fa-plus-square')) {
                $(curObj).removeClass('fa-plus-square');
                $(curObj).addClass('fa-minus-square');
            }

            $(curObj).closest('.treeViewItem').addClass('treeViewItemShowSubItem');
        };
        $(this)[0].openButtonAll = function () {
            var curThis = this;
            $(this).find('.treeViewPMIcon').each(function () {
                $(curThis)[0].openButton(this);
            });
        };
        $(this)[0].closeButton = function (curObj) {
            $(curObj).removeClass('fa-minus-square');
            $(curObj).addClass('fa-plus-square');
            $(curObj).closest('.treeViewItem').removeClass('treeViewItemShowSubItem');
        }
        $(this)[0].bindData = function (data, textField, valueField, dataChildFeild, dataSelected) {
            var result = '';
            if (data && data.length > 0) {
                result += '<span class="treeViewItems">'
                for (var i = 0; i < data.length; i++) {
                    result += $(this)[0].getItemTemplate(data[i], textField, valueField, dataChildFeild, 1, dataSelected);
                }
                result += '</span>';
            }
            $(this).html(result);
        };
        $(this)[0].getItemTemplate = function (dataItem, textField, valueField, dataChildFeild, index, dataSelected) {
            var result = '';
            var arrChilds = dataItem[dataChildFeild];
            if (dataItem && dataItem[textField] && dataItem[valueField]) {
                result += '<span class="treeViewItem">'
                var itemValue = uuidv4RemoveDash();
                var isSelected = dataItem[dataSelected];
                var isChekcedStr = '';
                if (isSelected) {
                    isChekcedStr = 'checked="checked"';
                    console.log(isChekcedStr);
                }
                result += '<span class="treeViewItemTitle"><i class="' + (arrChilds && arrChilds.length > 0 ? 'fa fa-plus-square' : '') + ' treeViewPMIcon" ></i><input ' + isChekcedStr + ' name="a_' + index + '" value="' +
                    dataItem[valueField] + '" type="checkbox" id="' + itemValue + '" /><label data-lc="' + dataItem['lc'] +'" for="' + itemValue + '" >' + dataItem[textField] + '</label></span>';
                if (arrChilds && arrChilds.length > 0) {
                    result += '<span class="treeViewItems">';
                    for (var i = 0; i < arrChilds.length; i++) {
                        result += $(this)[0].getItemTemplate(arrChilds[i], textField, valueField, dataChildFeild, index + 1, dataSelected);
                    }
                    result += '</span>'
                }
                result += '</span>'
            }

            return result;
        }
        $(this)[0].loadItems = function () {
            var dataUrl = $(this).attr('data-url');
            var dataTextFeild = $(this).attr('data-textfield');
            var dataValueFeild = $(this).attr('data-valuefield');
            var dataChildFeild = $(this).attr('data-childfeild');
            var databindsrcparameters = $(this).attr('data-bindsrcparameters');
            var dataSelected = $(this).attr('data-selected');
            if (dataUrl && dataTextFeild && dataValueFeild) {
                var postFormData = new FormData();
                if (databindsrcparameters) {
                    postFormData = eval(databindsrcparameters);
                }
                postForm(dataUrl, postFormData, function (res) {
                    if (res) {
                        $(this)[0].bindData(res, dataTextFeild, dataValueFeild, dataChildFeild, dataSelected);
                        $(this)[0].initCTRLs();
                        $(this)[0].openButtonAll();
                        loadLangugesTranslate();
                    }
                }.bind(this));
            }
        }
        $(this)[0].initCTRLs = function () {
            $(this).find('.fa-plus-square').click(function () {
                if ($(this).hasClass('fa-plus-square')) {
                    $(this).closest('.treeView')[0].openButton(this);
                }
                else {
                    $(this).closest('.treeView')[0].closeButton(this);
                }
            });
            $(this).find('input[type=checkbox]').change(function () {
                $(this).closest('.treeView')[0].openButton($(this).prev());
                var isChecked = $(this).is(':checked');
                $(this).closest('.treeViewItem').find('.treeViewItems input[type=checkbox]').prop('checked', isChecked).change();
            });
        }
        $(this)[0].loadItems();
    });
};

