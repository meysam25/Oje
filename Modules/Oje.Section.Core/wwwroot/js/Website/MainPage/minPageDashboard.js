
function showLoginUserPanelInMainPage() {
    if (location.pathname == "/" || location.pathname.toLowerCase() == "/home/index")
        postForm('/Account/Dashboard/GetLoginUserSideMenu', new FormData(), function (res) {
            if (res && res.length > 0) {
                bindCharts();
                var htmlShortCut = '';
                for (var i = 0; i < res.length; i++) {
                    var curMod = res[i];
                    htmlShortCut += getSectionParentTemplate(curMod);
                }

                $('#adminPanel').find('.allQuckAccessItemsHolder').html(htmlShortCut);
                $('#adminPanel').css('display', 'block');
                initShortCutItemClick();
                $('#adminPanel .quickAccessSection').addStatusBarToElement(null, function () { location.href = "/Account/Dashboard/Index"; return true; }, null);
                hideOtherExteraSections();
            }
        });
}
function hideOtherExteraSections() {
    $('#holderAboutUs').css('display', 'none');
    $('#ourPrideDiv').css('display', 'none');
}
function disableFloatingFooter() {
    if ($('#floatingFooter').length > 0)
        $('#floatingFooter').removeClass('floatingFooterSectionMakeFloat').addClass('makeMyContainer100')[0].disableFloating = true;

}
function initShortCutItemClick() {
    $('#adminPanel').find('.allQuckAccessItemsHolder').find('.WebMod').click(function (e) { $(this).toggleClass('WebModActive'); e.stopPropagation(); });
    $('#adminPanel .WebModActions').click(function (e) { e.stopPropagation(); })
    //$('body').click(function () { $('#adminPanel').find('.allQuckAccessItemsHolder').find('.WebMod').removeClass('WebModActive') });
    $('#adminPanel .WebModActions').find('.fa-times').click(function () {
        $(this).closest('.WebModActive').removeClass('WebModActive');
    });
}

function getSectionParentTemplate(curMod) {
    var result = '';

    result += '<' + (curMod.url ? 'a href="' + curMod.url + '"' : 'span') + ' class="WebMod"><i class="fa ' + curMod.icon + '"></i>' + curMod.title;

    if (curMod.childs && curMod.childs.length > 0) {
        result += '<span class="WebModActions"><i class="fa fa-times closeButtonQA" ></i>'
        for (var i = 0; i < curMod.childs.length; i++) {
            result += getSectionParentTemplate(curMod.childs[i]);
        }
        result += '</span>';
    }

    result += '</' + (curMod.url ? 'a' : 'span') + '>';

    return result;
}

function bindCharts() {
    loadJsonConfig('/ProposalFilledFormChartReports/ProposalFilledFormMonth/GetJsonConfig', $('#hiChartMonthly'));
    loadJsonConfig('/ProposalFilledFormChartReports/ProposalFilledFormYear/GetJsonConfig', $('#hiChartYear'));
    loadJsonConfig('/ProposalFilledFormChartReports/ProposalFilledFormWeek/GetJsonConfig', $('#hiChartWeekly'));
}