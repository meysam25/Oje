
function showLoginUserPanelInMainPage() {
    postForm('/Account/Dashboard/GetLoginUserSideMenu', new FormData(), function (res) {
        if (res && res.length > 0) {
            bindCharts();
            var htmlShortCut = '';
            var curCount = getItemCount(res);
            for (var i = 0; i < res.length; i++) {
                var curMod = res[i];
                if (curCount > 24)
                    htmlShortCut += '<span class="WebMod"><i class="fa ' + curMod.icon + '"></i>' + curMod.title + '<span class="WebModActions">';
                if (curMod.actions && curMod.actions.length > 0) {
                    for (var j = 0; j < curMod.actions.length; j++) {
                        htmlShortCut += getSectionTemplate(curMod.actions[j]);
                    }
                }
                if (curCount > 24)
                    htmlShortCut += '</span></span>';
            }

            $('#adminPanel').find('.allQuckAccessItemsHolder').html(htmlShortCut);
            $('#adminPanel').css('display', 'block');
        }
    });
}

function getItemCount(items) {
    var result = 0;
    for (var i = 0; i < items.length; i++) {
        result += items[i].actions.length;
    }

    return result;
}

function getSectionTemplate(item) {
    return `<a href="${item.url}"><i class="fa ${item.icon}"></i>${item.title}</a>`;
}

function bindCharts() {
    loadJsonConfig('/ProposalFilledFormChartReports/ProposalFilledFormMonth/GetJsonConfig', $('#hiChartMonthly'));
    loadJsonConfig('/ProposalFilledFormChartReports/ProposalFilledFormYear/GetJsonConfig', $('#hiChartYear'));
    loadJsonConfig('/ProposalFilledFormChartReports/ProposalFilledFormWeek/GetJsonConfig', $('#hiChartWeekly'));
}

showLoginUserPanelInMainPage();