
function closeAllDropdownInPage() {
    $('.myDropdown').each(function () {
        if ($(this)[0].closeMDD) {
            $(this)[0].closeMDD();
        }
    });
}

function updateAllSelectByInnerSelect(querySelector) {
    $(querySelector).find('.myDropdown').each(function () {
        if ($(this)[0].updateItemFromSelect) {
            $(this)[0].updateItemFromSelect();
        }
    });
}

$(document).ready(
    function () {
        $('body').click(function () { closeAllDropdownInPage(); });
    }
);

$.fn.initMyDropdown = function () {

    return this.each(function () {
        var curElement = $(this)[0];
        curElement.openMDD = function () {
            closeAllDropdownInPage();
            makeCtrlFocused($(this).find('select'));
            if ($(this)[0].actionTimeoutInterval)
                clearTimeout($(this)[0].actionTimeoutInterval);
            var itemId = $(this).find('.myDropdownItems').attr('id');
            var itemHtml = '<div id="' + itemId + '" class="' + $(this).find('.myDropdownItems').attr('class') + '">' + $(this).find('.myDropdownItems').clone().html() + '</div>';
            $(this).find('.myDropdownItems').remove();
            $('body').append(itemHtml);
            $('#' + itemId).width($(this).width() + 11);
            $('#' + itemId).css('top', $(this).offset().top + $(this).height() + 1);
            $('#' + itemId).css('left', $(this).offset().left);
            $(this).addClass('myDropdownMakeVisibleItems');
            $('#' + itemId).css('display', 'block').css('opacity', '1').css('z-index', '2005');
            $(this)[0].actionTimeoutInterval = setTimeout(function () { $(this).addClass('myDroppdownShowItem'); }.bind(this), 10);
            $(this)[0].bindSelectItemEventMMD();
        };
        curElement.closeMDD = function () {
            var curId = $(this).attr('id');
            if (!$(this).find('select').val())
                makeCtrlBlure($(this).find('select'));
            if ($(this)[0].actionTimeoutInterval)
                clearTimeout($(this)[0].actionTimeoutInterval);
            if ($('#' + curId + '_HItems').length == 0)
                return;
            $(this).removeClass('myDroppdownShowItem');
            $(this)[0].actionTimeoutInterval = setTimeout(function () { $(this).removeClass('myDropdownMakeVisibleItems'); }.bind(this), 300);
            var curId = $(this).attr('id');
            var html = '<div id="' + curId + '_HItems' + '" class="' + $('#' + curId + '_HItems').attr('class') + '">' + $('#' + curId + '_HItems').clone().html() + '</div>';
            $('#' + curId + '_HItems').remove();
            $(this).append(html);
            $(this)[0].updateItemFromSelect();
            $(this)[0].bindSelectItemEventMMD();
        };
        curElement.updateItemFromSelect = function () {
            var qureSelect = $(this).find('select option');
            if (qureSelect.length > 0) {
                var containerItems = $(this).find('.myDropdownItems');
                if (containerItems.length > 0) {
                    containerItems.html('');
                    $(this).find('select option').each(function () {
                        var isSelected = $(this).is(':selected');
                        containerItems.append('<div class="myDropdownItem ' + (isSelected ? 'myDropdownItemSelected' : '') + '">' + $(this).html() + '</div>');
                        if (isSelected) {
                            $(this).closest('.myDropdown')[0].setTextMMD($(this).html());
                        }
                    });
                    $(this)[0].bindSelectItemEventMMD();
                }
            }
            if (!$(this).find('select').val()) {
                makeCtrlBlure($(this).find('select'));
                $(this)[0].setTextMMD();
            }
            else
                makeCtrlFocused($(this).find('select'));
        }
        curElement.getSelectedIndex = function () {
            var result = -1;
            var curId = $(this).attr('id');
            $('#' + curId + '_HItems').find('.myDropdownItem').each(function (curIndex) {
                if ($(this).hasClass('myDropdownItemSelected')) {
                    result = curIndex;
                }
            });

            return result;
        };
        curElement.selectOptionByIndex = function (targetIndex) {
            if (targetIndex > -1) {
                var selectQuery = $(this).find('option:eq(' + targetIndex + ')');
                if (selectQuery.length > 0) {
                    $(this).find('select').val(selectQuery.val());
                    $(this).find('select').change();
                }
            }
        }
        curElement.setTextMMD = function () {
            var curId = $(this).attr('id');
            $(this).find('.myDropdownText span').html($('#' + curId + '_HItems').find('.myDropdownItemSelected').html());
        };
        curElement.bindSelectItemEventMMD = function () {
            var curId = $(this).attr('id');
            $('#' + curId + '_HItems').find('.myDropdownItem').click(function (e) {
                $(this).closest('.myDropdownItems').find('.myDropdownItemSelected').removeClass('myDropdownItemSelected');
                $(this).addClass('myDropdownItemSelected');
                $('#' + curId)[0].setTextMMD();
                var curIndex = $('#' + curId)[0].getSelectedIndex();
                if (curIndex > -1) {
                    $('#' + curId)[0].selectOptionByIndex(curIndex);
                }
                $('#' + curId)[0].closeMDD();
                e.stopPropagation();
            });
        };
        curElement.bindSelectItemEventMMD();
        $(curElement).closest('.myCtrl').find('label').click(function (e) {
            $(this).closest('.myDropdown')[0].openMDD();

            if (!window['isEditModeActive']) {
                e.preventDefault();
                e.stopPropagation();

                return false;
            }
        });

        $(this).click(function (e) {
            if ($(this).hasClass('myDropdownMakeVisibleItems')) {
                $(this)[0].closeMDD();
            } else {
                $(this)[0].openMDD();
            }
            if (!window['isEditModeActive'])
                e.stopPropagation();
        });
    });


}