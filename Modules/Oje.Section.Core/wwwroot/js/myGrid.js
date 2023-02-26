
$.fn.initMyGrid = function (option) {
    if (!option)
        return;
    if (option.nlevel) {
        option.detailes = JSON.parse(JSON.stringify(option));
        option.detailes.id = option.detailes.id + "1";
    }

    if (option.hide)
        $(this).hide();

    if (option.columns) {
        for (var i = 0; i < option.columns.length; i++) {
            if (option.columns[i].showCondation) {
                if (window[option.columns[i].showCondation]) {
                    option.columns[i].hide = false;
                } else {
                    option.columns[i].hide = true;
                }
            }
        }
    }

    var templateFunctions = {
        getAllColumns: function (option, data) {
            var result = [];

            if (option.columns)
                for (var i = 0; i < option.columns.length; i++)
                    result.push(option.columns[i]);

            if (data && data.columns)
                for (var i = 0; i < data.columns.length; i++)
                    result.push({ field: data.columns[i].id, caption: data.columns[i].title, search: { searchType: 'text' } });

            return result;
        },
        getGridHeaderCellTemplateStyle: function (col) {
            var result = '';

            if (col.width) {
                result += 'width:' + col.width + ';';
            }
            if (col.textAlign) {
                result += 'text-align:' + col.textAlign + ';';
            }

            return result;
        },
        getNoDataTemplate: function (option, data) {
            return '<tr  ><td colspan="' + this.getGridColumnCount(option, data) + '" style="text-align:center;padding:20px;" >اطلاعاتی جهت نمایش یافت نشد</td></tr>';
        },
        hasAction: function (option) {
            var result = false;

            if (option.actions) {
                result = option.actions.delete || option.actions.addNew || option.actions.update || option.actions.cActions;
            }

            return result;
        },
        getGridHeaderCellTemplate: function (option, data) {
            var result = '';
            var allColumns = templateFunctions.getAllColumns(option, data);
            if (option && allColumns && allColumns.constructor == Array) {
                if (option.selectable == true) {
                    result += '<td style="text-align:center;width:30px;" ><input class="headCB" type="checkbox" /></td>';
                }
                if (option.detailes) {
                    result += '<td style="text-align:center;width:30px;" ></td>';
                }
                for (var i = 0; i < allColumns.length; i++) {
                    if (!allColumns[i].hide) {
                        var sortCL = '';
                        if (allColumns[i].sort)
                            sortCL = 'hasSort';
                        if (option.sortFieldStatus == true && allColumns[i].field == option.sortField)
                            sortCL += ' fa fa-sort-alpha-down';
                        else if (option.sortFieldStatus == false && allColumns[i].field == option.sortField)
                            sortCL += ' fa fa-sort-alpha-up';
                        result += '<td class="' + sortCL + '" ' + (allColumns[i].sort ? 'data-field-name="' + allColumns[i].field + '"' : '') +
                            ' style="' + this.getGridHeaderCellTemplateStyle(allColumns[i]) + '" ><span style="display:inline-block;padding-right:5px;" >' + allColumns[i].caption + '</span></td>';
                    }

                }
                if (this.hasAction(option)) {
                    var actionHeaderTemplate = '#';
                    if (option.showAddButton == true) {
                        actionHeaderTemplate = '<span class="gridAddButton icon-android-add-circle"></span>';
                    }
                    result += '<td style="text-align:center;" >' + actionHeaderTemplate + '</td>';
                }
            }

            return result;
        },
        getSearchCTRL: function (searchConfig) {
            var result = '';

            if (searchConfig.search) {
                if (searchConfig.search.searchType == 'text') {
                    result += '<input type="text" readonly onfocus="this.removeAttribute(\'readonly\');" autocomplete="off" value=""  class="form-control" name="' + searchConfig.field + '" />';;
                } else if (searchConfig.search.searchType == 'date') {
                    result += '<input type="text" autocomplete="off" class="form-control myGridPersianDP" name="' + searchConfig.field + '" />';;
                } else if (searchConfig.search.searchType == 'dropdown') {
                    result += '<select data-valueField = "' + searchConfig.search.valueField + '" data-textField="' + searchConfig.search.textField + '" data-url="' + searchConfig.search.url + '" class="form-control" name="' + searchConfig.field + '">/select>';
                } else if (searchConfig.search.searchType == 'persianDateTime') {
                    result += '<input data-jdp value="" autocomplete="off" class="form-control myGridPersianDP" name="' + searchConfig.field + '" />';;
                }
            }

            return result;
        },
        getGridHeaderSearchTemplate: function (option, addActions, isUpdate, data) {
            var result = '';
            var doseHaveSearchFilter = false;
            var allColumns = templateFunctions.getAllColumns(option, data);
            for (var i = 0; i < allColumns.length; i++) {
                if (allColumns[i].search) {
                    doseHaveSearchFilter = true;
                    break;
                }
            }

            if (addActions == true) {
                doseHaveSearchFilter = true;
            }

            if (doseHaveSearchFilter == true) {
                result += '<tr>'
                if (option.selectable == true) {
                    result += '<td style="text-align:center;width:30px;" ></td>';
                }
                if (option.detailes) {
                    result += '<td style="text-align:center;width:30px;" ></td>';
                }
                for (var i = 0; i < allColumns.length; i++) {
                    if (!allColumns[i].hide) {
                        if (allColumns[i].search) {
                            result += '<td>';
                            result += this.getSearchCTRL(allColumns[i]);
                            result += '</td>';
                        } else {
                            result += '<td></td>';
                        }
                    }
                }
                if (this.hasAction(option)) {
                    result += '<td style="text-align:center;" >';
                    if (addActions == true) {
                        result += '<span data-url="' + (isUpdate ? option.actions.update.url : option.actions.addNew.url) + '" title="ذخیره" class="myGridAction saveButton"><i class="icon-save"></i></span>';
                        result += '<span  title="انصراف" class="myGridAction myGridWarning cancelButton"><i class="icon-cancel-circle"></i></span>';
                    }
                    result += '</td>';
                }
                result += '</tr>'
            }

            return result;
        },
        getGridHeaderTemplate: function (option, data) {
            var searchTemplate = this.getGridHeaderSearchTemplate(option, null, null, data);
            return `
                        <thead class="myTableHeader">
                            <tr >
                                `+ this.getGridHeaderCellTemplate(option, data) + `
                            </tr>
                            `+ searchTemplate + `
                        </thead>
                    `
        },
        getGridColumnCount: function (option, data) {
            var result = 0;
            var allColumns = templateFunctions.getAllColumns(option, data);

            if (option && allColumns) {
                for (var i = 0; i < allColumns.length; i++) {
                    if (!allColumns[i].hide)
                        result++;
                }
            }
            if (option.selectable == true)
                result += 1;
            if (option.detailes)
                result += 1;
            if (this.hasAction(option))
                result += 1;

            return result;
        },
        getGridBodyTemplateLoading: function (option, data) {
            return `
                <tbody class="myTableBody">
                    <tr>
                        <td style="text-align:center;" colspan="`+ this.getGridColumnCount(option, data) + `" >در حال بارگزاری ...</td>
                    </tr>
                </tbody>
            `
        },
        getActionTemplates: function (data, option) {
            var result = '';

            if (option && option.actions.delete && option.actions.delete.url) {
                result += '<span title="حذف" data-id="' + data[option.key] + '" data-url="' + option.actions.delete.url + '" class="myGridAction myGridActionDelete"><i class="fa fa-trash" ></i></span>'
            }
            if (option && option.actions.update && option.actions.update.url) {
                result += '<span title="edit" data-id="' + data[option.key] + '" data-url="' + option.actions.update.url + '" class="myGridAction showEditStyle"><i class="fa fa-pen" ></i></span>'
            }
            if (option && option.actions.cActions && option.actions.cActions.length > 0) {
                for (var i = 0; i < option.actions.cActions.length; i++) {
                    if (option.actions.cActions[i].template && typeof (option.actions.cActions[i].template) == 'function') {
                        if (option.actions.cActions[i].ignoreParentButton == true)
                            result += '<span style="display:inline-block" data-index="' + i + '" >' + option.actions.cActions[i].template(data) + '</span>';
                        else
                            result += '<span class="myGridAction myGridCAction" data-index="' + i + '" >' + option.actions.cActions[i].template(data) + '</span>';
                    } else if (option.actions.cActions[i].template && typeof (option.actions.cActions[i].template) == 'string') {
                        var compileStr = 'var data =' + JSON.stringify(data) + ';' + option.actions.cActions[i].template;
                        var compileStrResult = eval(compileStr)
                        if (compileStrResult && option.actions.cActions[i].ignoreParentButton == true)
                            result += '<span style="display:inline-block" data-index="' + i + '" >' + compileStrResult + '</span>';
                        else if (compileStrResult)
                            result += '<span class="myGridAction myGridCAction" data-index="' + i + '" >' + compileStrResult + '</span>';
                    }
                }
            }

            return result;
        },
        getGridRowDataTemplate: function (data, option, datas) {
            var result = '<tr data-row-json=\'' + JSON.stringify(data) + '\' class="rowBindItem">';
            if (option.selectable == true) {
                result += '<td style="text-align:center;width:30px;" ><input name="gridSelectedItems" value="' + (option.key ? data[option.key] : '') + '" type="checkbox" /></td>';
            }
            if (option.detailes) {
                result += '<td style="text-align:center;width:30px;" ><span class="fa fa-plus-square gridExpandButton"></span></td>';
            }
            var allColumns = templateFunctions.getAllColumns(option, datas);
            for (var i = 0; i < allColumns.length; i++) {
                if (!allColumns[i].hide) {
                    var curCellData = data[allColumns[i].field];
                    if (!curCellData)
                        curCellData = '';
                    if (allColumns[i].formatter && option.formatters[allColumns[i].formatter]) {
                        if (typeof (option.formatters[allColumns[i].formatter]) == 'string') {
                            var compileStr = 'var data =' + JSON.stringify(data) + ';' + (option.formatters[allColumns[i].formatter]);
                            curCellData = eval(compileStr);
                        }
                        else
                            curCellData = option.formatters[allColumns[i].formatter](null, data);
                    }
                    result += '<td class="' + (allColumns[i].class ? allColumns[i].class : '') + '" style="' + this.getGridHeaderCellTemplateStyle(allColumns[i]) + '" ><span  class="gridResTitle">' + allColumns[i].caption + ': </span>' + curCellData + '</td>';
                }
            }
            if (this.hasAction(option)) {
                result += '<td class="actionTD" style="text-align:center;" >';
                result += this.getActionTemplates(data, option);
                result += '</td>';
            }
            result += '</tr>'
            return result;
        },
        getColumnConfigButtonTemplate: function (option, data) {
            var newLabelId = 'cb_' + Math.random();
            newLabelId = newLabelId.replace('.', '');
            var result = '<div class="holderColumnConfig">';
            result += '<span class="columnConfigButton">';
            result += '<i class="columnConfigButtonIcon fa fa-columns"></i>';
            result += '</span>';
            result += '<span class="columnMenu">';
            result += '<span class="holderSearchTextBox"><input id="' + newLabelId + '" type="text" placeholder="search" class="form-control columnSearchBox" ><label for="' + newLabelId + '" class="fa fa-search"></label></span>';
            result += '<span class="holdercolumnItem">';
            var allColumns = templateFunctions.getAllColumns(option, data);
            for (var i = 0; i < allColumns.length; i++) {
                var newId = 'cb_' + Math.random();
                newId = newId.replace('.', '');
                result += '<span class="columnMenuItem"><input data-index="' + i + '" id="' + newId + '" type="checkbox" checked="checked" /><label data-lc="' + allColumns[i].lc + '" for="' + newId + '">' + allColumns[i].caption + '</label></span>';
            }
            result += '</span>';
            result += '</span>';
            result += '</div>';
            return result;
        },
        getColumnExcelButtonTemplate: function (option) {
            var result = '<div data-excel-export-url="' + option.exportToExcelUrl + '" class="exportToExcel">';
            result += '<i class="fa fa-file-excel"></i>';
            result += '</div>';
            return result;
        },
        getColumnModalFilterButtonTemplate: function (option) {
            var result = '<div data-modal-filter-id="' + option.exteraFilterModalId + '" class="gridExteraFilterModal">';
            result += '<i class="fa fa-filter"></i>';
            result += '</div>';
            return result;
        },
        hasAnyFilter: function (option, data) {
            var result = false;
            var allColumns = templateFunctions.getAllColumns(option, data);
            for (var i = 0; i < allColumns.length; i++) {
                if (allColumns[i].search) {
                    result = true;
                    break;
                }
            }

            return result;
        },
        getGridTemplate: function (option, data) {
            var doseHaveAnyHeaderAction = false;

            var columnConfigButton = '<div class="topGridAction">';
            if (option.topActions && option.topActions.length > 0) {
                doseHaveAnyHeaderAction = true;
                columnConfigButton += '<div class="gridTopActionButtonHolder">';
                for (var i = 0; i < option.topActions.length; i++) {
                    columnConfigButton += '<button  onclick="' + option.topActions[i].onClick + '" class="gridheaderButton btn btn-primary btn-sm" >' + option.topActions[i].title + '</button>';
                }
                columnConfigButton += '</div>';
            }
            if (option.showColumnConfigButton) {
                columnConfigButton += this.getColumnConfigButtonTemplate(option, data);
                doseHaveAnyHeaderAction = true;
            }
            if (option.exteraFilterModalId) {
                columnConfigButton += this.getColumnModalFilterButtonTemplate(option);
                doseHaveAnyHeaderAction = true;
            }
            if (option.exportToExcelUrl) {
                columnConfigButton += this.getColumnExcelButtonTemplate(option);
                doseHaveAnyHeaderAction = true;
            }
            if (this.hasAnyFilter(option, data)) {
                columnConfigButton += '<span class="myGridSearchButton"><i class="fa fa-search" ></i></span>';
                doseHaveAnyHeaderAction = true;
            }
            columnConfigButton += '<div style="clear:both;" ></div></div>';
            if (doseHaveAnyHeaderAction == false) {
                columnConfigButton = '';
            }
            return `
                        `+ (option.headerTemplate ? option.headerTemplate : '') + columnConfigButton + `
                        <table class="myGrid" >
                            ` + this.getGridHeaderTemplate(option, data) + `
                            ` + this.getGridBodyTemplateLoading(option, data) + `
                        </table>
                    `;
        },
        getFooterTemplate: function (totalNumber, option, currentPage, itemPerPage, data) {
            var result = '<tr><td colspan="' + this.getGridColumnCount(option, data) + '"><div class="row">';

            var pages = Number.parseInt(Math.ceil(totalNumber / itemPerPage));
            if (!pages)
                pages = 0;

            result += '<div class="col-md-9 col-lg-9 col-sm-9 col-xs-12 myGridPagaer" ><div class="gridPageButtonHolder">';

            result += '<button class="moveFirst"><</button>';

            var totalItem = totalNumber;
            var itemPerPage = itemPerPage;
            var centerSize = 6;

            var pages = parseInt(Math.ceil(totalItem / itemPerPage));

            var centerSectionArr = [];

            var startIndex = currentPage;
            var endIndex = currentPage + centerSize - 1;
            startIndex = startIndex - centerSize;
            if (startIndex < 0) {
                endIndex += (startIndex * -1)
                startIndex = 0;
            }
            if (endIndex > pages) {
                startIndex -= (endIndex - pages);
                endIndex = pages
            }
            if (startIndex < 0) {
                startIndex = 0;
            }
            for (var i = startIndex; i < endIndex; i++) {
                centerSectionArr.push({ text: (i + 1), isActive: currentPage == (i + 1) });
            }


            if (centerSectionArr.length > 0 && centerSectionArr[0].text != 1)
                centerSectionArr[0].text = 1;
            if (centerSectionArr.length > 1 && centerSectionArr[1].text - centerSectionArr[0].text > 1)
                centerSectionArr[1].text = '...';
            if (centerSectionArr.length > 0 && centerSectionArr[centerSectionArr.length - 1].text != pages)
                centerSectionArr[centerSectionArr.length - 1].text = pages;
            if (centerSectionArr.length > 1 && centerSectionArr[centerSectionArr.length - 1].text - centerSectionArr[centerSectionArr.length - 2].text > 1)
                centerSectionArr[centerSectionArr.length - 2].text = '...';



            for (var i = 0; i < centerSectionArr.length; i++) {
                result += '<button class="' + (centerSectionArr[i].text == '...' ? 'normalCR' : 'pageItem') + ' ' + (centerSectionArr[i].isActive == true ? 'myActive' : '') + '">' + centerSectionArr[i].text + '</button>';
            }
            result += '<button class="moveNext">></button>';


            result += '</div></div>';

            result += '<div class="col-md-3 col-lg-3 col-sm-3 col-xs-12 " ><div class="myItemPerPage">';
            result += '<span ' + (itemPerPage == 10 ? 'class="activeItemPerPage"' : '') + ' >10</span><span ' + (itemPerPage == 20 ? 'class="activeItemPerPage"' : '') + '>20</span><span ' + (itemPerPage == 50 ? 'class="activeItemPerPage"' : '') + '>50</span><span ' + (itemPerPage == 100 ? 'class="activeItemPerPage"' : '') + '>100</span>';
            result += '</div></div>';

            result += '</div></td></tr>'

            return result;
        }

    }

    return this.each(function () {
        var curElement = $(this)[0];
        curElement.currentPage = 1;
        curElement.skip = 0;
        curElement.take = option.itemPerPage;
        curElement.itemPerPage = option.itemPerPage
        curElement.templateFunctions = templateFunctions;
        curElement.option = option;
        $(this).html(templateFunctions.getGridTemplate(option, curElement.lastData));

        $(this).find('.columnConfigButton').click(function (e) {
            e.stopPropagation();
            $(this).closest('.holderColumnConfig').toggleClass('openConfigMenu');
        });
        $(this).find('.gridExteraFilterModal').click(function () {
            var modalId = $(this).attr('data-modal-filter-id');
            $('#' + modalId).modal('show');
        });
        $(this).find('.exportToExcel').click(function (e) {
            e.stopPropagation();
            var url = $(this).attr('data-excel-export-url');
            if (url) {
                var querySelector = $(this).closest('.myGridCTRL');
                var postData = getFormData(querySelector);
                showLoader(querySelector);
                postForm(url, postData, function (res) {
                    if (res && res.isSuccess == undefined) {
                        const b64toBlob = (b64Data, contentType = '', sliceSize = 2222512) => {
                            const byteCharacters = atob(b64Data);
                            const byteArrays = [];

                            for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
                                const slice = byteCharacters.slice(offset, offset + sliceSize);

                                const byteNumbers = new Array(slice.length);
                                for (let i = 0; i < slice.length; i++) {
                                    byteNumbers[i] = slice.charCodeAt(i);
                                }

                                const byteArray = new Uint8Array(byteNumbers);
                                byteArrays.push(byteArray);
                            }

                            const blob = new Blob(byteArrays, { type: contentType });
                            return blob;
                        }
                        const blob = b64toBlob(res, 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet');
                        const blobUrl = URL.createObjectURL(blob);
                        var printIndex = (Math.random() + '').replace('.', '');
                        $('body').append('<a id="id_' + printIndex + '" style="display:none"  >download.pdf</a>');
                        var aTag = $('#id_' + printIndex);
                        aTag.attr('href', blobUrl);
                        aTag.attr('download', 'download.xlsx');
                        aTag[0].click();
                    }
                }, null, function () {
                    hideLoader(querySelector);
                });
            }
        });
        $(this).find('.columnMenu').click(function (e) { e.stopPropagation(); });
        $('body').click(function () {
            $('.openConfigMenu').removeClass('openConfigMenu');
        });
        $(this).find('.columnSearchBox').keyup(function () {
            var curVal = $(this).val();

            $(this).closest('.columnMenu').find('.holdercolumnItem .columnMenuItem').each(function () {
                var curText = $(this).find('label').text();
                if (curText.indexOf(curVal) == -1) {
                    $(this).css('display', 'none');
                } else {
                    $(this).css('display', 'block');
                }
            });
        });
        $(this).find('.columnMenuItem input[type="checkbox"]').change(function () {
            var curValue = $(this).prop('checked');
            var columnIndex = $(this).attr('data-index');
            var curCTRL = $(this).closest('.myGridCTRL')[0];
            var curOption = curCTRL.option;
            curOption.columns[columnIndex].hide = !curValue;
            curCTRL.option = curOption;
            curCTRL.refreshData();
        });


        curElement.initTopCB = function () {
            $(this).find('.headCB').change(function () {
                var curValue = $(this).prop('checked');
                $(this).closest('.myGridCTRL').find('tbody > tr > td:first-child > input[type="checkbox"]').prop('checked', curValue);
            });
        }
        curElement.initTopCB();


        curElement.initCTRL = function (curObj) {
            $(curObj).find('.myGridPersianDP').each(function () {
                $(document).ready(function () {
                    $(this.cObj).persianDatepicker({
                        formatDate: "YYYY/0M/0D",
                        cellWidth: 38,
                        cellHeight: 30,
                        fontSize: 16
                    });
                }.bind({ cObj: this }));
            });
            $(curObj).find('select[data-url]').each(function () {
                initDropdown(this);
            });
        };

        curElement.initCTRL(this);

        curElement.addNewRow = function () {
            if ($(this)[0].isAddNewShow == true)
                return;
            var theader = $(this).find('.myTableHeader');
            var option = $(this)[0].option;
            if (theader.length > 0) {
                var rowTemplate = templateFunctions.getGridHeaderSearchTemplate(option, true, null, curElement.lastData);
                theader.append(rowTemplate);
                $(this)[0].isAddNewShow = true;
                $(theader).find('.saveButton').click(function () {
                    var url = $(this).attr('data-url');
                    if (url) {
                        showLoader($(this).closest('.myGridCTRL'));
                        var postData = getFormData($(this).closest('tr'));
                        postForm(url, postData, function () {
                            hideLoader($(this).closest('.myGridCTRL'));
                            $(this).closest('.myGridCTRL')[0].refreshData();
                            $(this).closest('.myGridCTRL')[0].isAddNewShow = false;
                            $(this).closest('tr').remove();
                        }.bind(this), null, function () {
                            hideLoader($(this).closest('.myGridCTRL'));
                        }.bind(this));
                    }
                });
                $(theader).find('.cancelButton').click(function () {
                    $(this).closest('.myGridCTRL')[0].isAddNewShow = false;
                    $(this).closest('tr').remove();
                });
                $(this)[0].initCTRL(theader.find('tr:last-child'));
            }
        }

        curElement.showEditStyle = function (curObj) {
            var crTR = $(curObj).closest('tr');
            var rowTemplate = templateFunctions.getGridHeaderSearchTemplate(option, true, true, curElement.lastData);
            crTR.html($(rowTemplate).html());
            $(crTR).closest('.myGridCTRL')[0].initCTRL(crTR);
            var jsData = JSON.parse($(crTR).attr('data-row-json'));
            bindForm(jsData, crTR);
            $(crTR).find('.cancelButton').click(function () {
                var trObj = $(this).closest('tr');
                var option = $(trObj).closest('.myGridCTRL')[0].option;
                var jsData = JSON.parse(trObj.attr('data-row-json'));
                var editRowTemplate = templateFunctions.getGridRowDataTemplate(jsData, option, curElement.lastData);
                trObj.html($(editRowTemplate).html());
                $(trObj).closest('.myGridCTRL')[0].bindActions(trObj);
            });
            $(crTR).find('.saveButton').click(function () {
                var url = $(this).attr('data-url');

                if (url) {
                    var postData = getFormData($(this).closest('tr'));
                    var trObj = $(this).closest('tr');
                    var option = $(trObj).closest('.myGridCTRL')[0].option;
                    var jsData = JSON.parse(trObj.attr('data-row-json'));
                    postData.append(option.key, jsData[option.key]);
                    showLoader($(this).closest('.myGridCTRL'));
                    postForm(url, postData, function (res) {
                        $(this).closest('.myGridCTRL')[0].refreshData();
                    }.bind(this), function () { }.bind(this), function () { hideLoader($(this).closest('.myGridCTRL')); }.bind(this));
                }
            });
        }

        curElement.bindActions = function (curObj) {
            if (!curObj)
                curObj = this;
            $(curObj).find('.myGridAction').each(function () {
                if ($(this).hasClass('myGridActionDelete')) {
                    $(this).click(function () {
                        var url = $(this).attr('data-url');
                        var id = $(this).attr('data-id');
                        var hasUpdateAllGrid = $(this).attr('data-refreshAllGrid');
                        var pKey = $(this).closest('.modal').length > 0 ? $(this).closest('.modal')[0].pKey : '';
                        if (url && id) {
                            confirmDialog(($(this).attr('title') ? $(this).attr('title') : 'حذف'), 'آیا اطمینان دارید ؟', function () {
                                var postData = new FormData();
                                postData.append('id', this.id);
                                if (this.pKey)
                                    postData.append('pKey', this.pKey);
                                showLoader($(this.curThis).closest('.myGridCTRL'))
                                postForm(this.url, postData, function () { $(this).closest('.myGridCTRL')[0].refreshData(); updateDashboardGridCountIfExist(); }.bind(this.curThis), null, function ()
                                {
                                    hideLoader($(this).closest('.myGridCTRL'));
                                    if (hasUpdateAllGrid) {
                                        refreshAllGrid();
                                    }
                                }.bind(this.curThis));
                            }.bind({ url, id, curThis: this, pKey: pKey }));
                        }
                    });
                } else if ($(this).hasClass('showEditStyle')) {
                    $(this).click(function () {
                        $(this).closest('.myGridCTRL')[0].showEditStyle(this);
                    });
                }
            });
            $(curObj).find('.gridExpandButton').click(function () {
                $(curObj)[0].expandDetailes(this);
            });
        }

        curElement.expandDetailes = function (currButton) {

            if ($(currButton).hasClass('fa-plus-square')) {
                this.closeAllExpandButton(currButton);
                $(currButton).removeClass('fa-plus-square');
                $(currButton).addClass('fa-minus-square');
                var curTr = $(currButton).closest('tr');
                var optionX = $(this)[0].option
                curTr.after('<tr class="holderDetailesGrid"><td colspan="' + templateFunctions.getGridColumnCount(optionX, curElement.lastData) + '" ><div id="' + optionX.detailes.id + '" class="myGridCTRL gridDetailes"></div></td></tr>');
                var currRowData = JSON.parse(curTr.attr('data-row-json'));
                optionX.detailes.ds = currRowData[optionX.detailesClientSchema];
                optionX.detailes.exteraParameters = { pKey: currRowData[optionX.key] };
                curTr.next().find('.myGridCTRL').initMyGrid(optionX.detailes);
            } else {
                $(currButton).removeClass('fa-minus-square');
                $(currButton).addClass('fa-plus-square');
                var curTr = $(currButton).closest('tr');
                curTr.next().remove();
            }
        }

        curElement.closeAllExpandButton = function (currButton) {
            $(currButton).closest('.myTableBody').find('> tr > td > .fa-minus-square').each(function () {
                if ($(this)[0] != $(currButton)[0]) {
                    $(this).click();
                }
            });
        }

        curElement.bindData = function (res) {
            var option = this.option;
            var tHead = $(this).find('thead');
            var totalSchema = option.schema.total;
            var dataSchema = option.schema.data;
            var totalItems = res[totalSchema];
            var data = res[dataSchema];
            var holderItems = $(this).find('.myTableBody');
            var resultRows = '';
            var cElement = $(this)[0];
            var headerTemplate = templateFunctions.getGridHeaderTemplate(option, res);
            tHead.html($(headerTemplate).html());
            cElement.initCTRL(tHead);
            cElement.initTopCB();
            holderItems.html('');
            cElement.curItems = data;
            cElement.curTotalItems = totalItems;
            if (option.isClient) {
                if (data && data.length > 0 && data.constructor == Array) {
                    for (var i = this.skip; i < data.length && i < (this.skip + this.take); i++) {
                        resultRows += templateFunctions.getGridRowDataTemplate(data[i], option, res);
                    }
                    holderItems.html(resultRows);
                    $(this)[0].bindActions();
                }
            } else {
                if (data && data.length > 0 && data.constructor == Array) {
                    for (var i = 0; i < data.length; i++) {
                        resultRows += templateFunctions.getGridRowDataTemplate(data[i], option, res);
                    }
                    holderItems.html(resultRows);
                    $(this)[0].bindActions();
                }
            }

            if (data && data.length == 0 && data.constructor == Array) {
                holderItems.html(templateFunctions.getNoDataTemplate(option, res));
            }

            $(this).find('tbody').append(templateFunctions.getFooterTemplate(totalItems, option, cElement.currentPage, cElement.itemPerPage, res));

            $(this).find('.myItemPerPage span').click(function () {
                var gridCTRL = $(this).closest('.myGridCTRL');
                gridCTRL[0].option.itemPerPage = parseInt($(this).text());
                gridCTRL[0].take = parseInt($(this).text());
                gridCTRL[0].itemPerPage = parseInt($(this).text());
                gridCTRL[0].refreshData();
            });

            $(this).find('.pageItem').click(function () {
                var gridCTRL = $(this).closest('.myGridCTRL');
                var curPage = gridCTRL[0].currentPage;
                var targetPage = Number.parseInt($(this).text());
                if (curPage != targetPage) {
                    gridCTRL[0].currentPage = targetPage;
                    gridCTRL[0].skip = (targetPage - 1) * gridCTRL[0].take;
                    gridCTRL[0].refreshData();
                }
            });

            $(this).find('.moveFirst').click(function (e) {

                e.preventDefault();
                e.stopPropagation();
                var gridCTRL = $(this).closest('.myGridCTRL');
                var curPage = gridCTRL[0].currentPage;
                var targetPage = curPage - 1;
                if (targetPage <= 0) {
                    return false;
                }
                if (curPage != targetPage) {
                    gridCTRL[0].currentPage = targetPage;
                    gridCTRL[0].skip = (targetPage - 1) * gridCTRL[0].take;
                    gridCTRL[0].refreshData();
                }

                return false;
            });

            $(this).find('.moveNext').click(function () {
                var gridCTRL = $(this).closest('.myGridCTRL');
                var curPage = gridCTRL[0].currentPage;
                var targetPage = curPage + 1;
                var pages = Number.parseInt(Math.ceil(totalItems / gridCTRL[0].itemPerPage))
                if (targetPage > pages) {
                    return;
                }
                if (curPage != targetPage) {
                    gridCTRL[0].currentPage = targetPage;
                    gridCTRL[0].skip = (targetPage - 1) * gridCTRL[0].take;
                    gridCTRL[0].refreshData();
                }
            });

            var filterValues = $(this)[0].filtersValue;
            if (filterValues) {
                var filterJsonData = {};
                for (var key of filterValues.keys()) {
                    filterJsonData[key] = filterValues.get(key);
                }
                bindForm(filterJsonData, $(this).find('thead'), true)
            }

            //$(this).find('.myTableHeader input[type="text"], .myTableHeader select').change(function() {
            //    $(this).closest('.myGridCTRL')[0].refreshData();
            //});
            $(this).find('.myGridSearchButton').unbind('click').click(function () { $(this).closest('.myGridCTRL')[0].refreshData(); });

            $(this).find('.gridAddButton').click(function () {
                $(this).closest('.myGridCTRL')[0].addNewRow();
            });

            //bind cAction functions
            $(this).find('.myGridCAction').click(function () {
                var dataIndex = $(this).attr('data-index');
                var jsData = JSON.parse($(this).closest('tr').attr('data-row-json'));
                var cActions = $(this).closest('.myGridCTRL')[0].option.actions.cActions;
                if (cActions[dataIndex].whatTodDo) {
                    cActions[dataIndex].whatTodDo(jsData);
                }

            });

            //fix stepWizard height problem
            var foundParentSW = $(this).closest('.stepWizardagent');
            if (foundParentSW.length > 0) {
                foundParentSW[0].moveToIndex(foundParentSW[0].lastIndex, true);
            }

            // bind sort function
            $(this).find('.hasSort').click(function () {
                var findOption = $(this).closest('.myGridCTRL')[0].option;
                var currentSortField = findOption.sortField;
                var currentSortFieldStatus = findOption.sortFieldStatus;
                var currentField = $(this).attr('data-field-name');

                if (!currentSortFieldStatus && currentSortFieldStatus != false) {
                    currentSortFieldStatus = false;
                }
                else if (currentSortFieldStatus == true) {
                    currentSortFieldStatus = null;
                }
                else {
                    currentSortFieldStatus = true;
                }

                if (currentField != currentSortField) currentSortFieldStatus = false;

                currentSortField = currentField;

                findOption.sortField = currentSortField;
                findOption.sortFieldStatus = currentSortFieldStatus;

                $(this).closest('.myGridCTRL')[0].refreshData();
            });

            bindTranslation();
        }
        function setFiltersAndSorts(data, option, filters) {

            var cloneData = data.data.slice();
            for (var key of filters.keys()) {
                var filterValue = filters.get(key);
                if (filterValue) {
                    cloneData = cloneData.filter(function (item) {
                        return item[this.key].indexOf(this.value) > -1;
                    }.bind({ key: key, value: filterValue }));
                }
            }

            if (option.sortField && (option.sortFieldStatus == true || option.sortFieldStatus == false)) {
                cloneData = cloneData.sort(function (a, b) {
                    var valueA = a[this.field] + '';
                    var valueB = b[this.field] + '';
                    valueA = valueA.toUpperCase();
                    valueB = valueB.toUpperCase();

                    if (valueA > valueB)
                        return this.isAsc == true ? 1 : -1;
                    if (valueA < valueB)
                        return this.isAsc == true ? -1 : 1;

                    return 0;
                }.bind({ field: option.sortField, isAsc: option.sortFieldStatus }));
            }

            return { data: cloneData, total: cloneData.length };
        }
        curElement.setFiltersAndSorts = setFiltersAndSorts;
        curElement.refreshData = function () {
            var option = $(this)[0].option;
            if (!option.isClient) {
                var url = option.url;
                var postData = getFormData($(this));
                postData.append('skip', this.skip);
                postData.append('take', this.take);
                if (option.sortFieldStatus == true || option.sortFieldStatus == false) {
                    postData.append('sortField', option.sortField);
                    postData.append('sortFieldIsAsc', option.sortFieldStatus);
                }

                $(this)[0].filtersValue = postData;
                if (option.exteraSearchIds) {
                    for (var i = 0; i < option.exteraSearchIds.length; i++) {
                        var exteraPostData = getFormData($('#' + option.exteraSearchIds[i]));
                        for (var pair of exteraPostData.entries()) {
                            postData.append(pair[0], pair[1]);
                        }
                    }
                }
                if ($(this).closest('.modal').length > 0) {
                    var pkey = $(this).closest('.modal')[0].pKey
                    if (pkey) {
                        postData.append('pKey', pkey);
                    }
                }
                if (option.exteraParameters) {
                    for (var item in option.exteraParameters) {
                        postData.append(item, option.exteraParameters[item]);
                    }
                }
                if (url) {
                    showLoader($(this));
                    postForm(url, postData,
                        function (res) {
                            if (res.isSuccess != false) {
                                $(this)[0].bindData(res);
                                $(this)[0].lastData = res;
                            }
                        }.bind(this), function () {
                        }.bind(this), function () {
                            hideLoader($(this));
                        }.bind(this));
                }
            } else {
                var postData = getFormData($(this));
                $(this)[0].filtersValue = postData;
                $(this)[0].bindData(setFiltersAndSorts(option.ds, option, postData));
            }

        };

        curElement.refreshData();
    });
}


$.fn.initMyGridClient = function (inputOption) {
    return this.each(function () {
        var option =
        {
            columns: [],
            isClient: true,
            itemPerPage: 10,
            schema: {
                data: 'data',
                total: 'total'
            },
            showColumnConfigButton: true,
            selectable: inputOption.selectable
        };
        for (var item in inputOption) {
            option[item] = inputOption[item];
        }

        $(this).find('thead tr th').each(function () {
            if ($(this).attr('data-identifier')) {
                option.key = $(this).attr('data-column-id');
            }
            var pItem = {
                field: $(this).attr('data-column-id'),
                caption: $(this).text(),
                hide: $(this).attr('data-visible') == 'false' ? true : false,
                formatter: $(this).attr('data-formatter')
            };
            if ($(this).attr('data-column-search') == 'true') {
                pItem.search = {
                    searchType: 'text'
                }
            }
            if ($(this).attr('data-sort') == 'true') {
                pItem.sort = true;
            }

            option.columns.push(pItem);
        });

        var ds = [];
        $(this).find('tbody tr').each(function () {
            ds.push({});
            $(this).find('td').each(function (index) {
                ds[ds.length - 1][option.columns[index].field] = $(this).text();
            });
        });
        option.ds = { data: ds, total: ds.length };

        var randId = "grid_" + Math.random();
        randId = randId.replace('.', '');
        $(this).replaceWith('<div id="' + randId + '" class="myGridCTRL"></div>');
        $('#' + randId).initMyGrid(option);
    });
};

