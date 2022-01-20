
function initBlogPage() {
    initBlogDropdown();
    initBlogCategoryButtons();
    $('.loadMoreButton').click(function () {
        doSearch(true);
    });

    doSearch();
    onBlogScroll();
}

function onBlogScroll() {
    var foundObj = $('#scrollDtectorForBlog');
    if (foundObj.length == 0)
        return;
    var handler = onVisibilityChange(foundObj, function (currVisible) {
        if (currVisible == true) {
            $('.holderBlogItems').addClass('holderBlogItemsActive');
            setTimeout(function () { $('.holderBlogItems').css('max-height', 'unset') }, 600);
        }
    }.bind({ curThis: foundObj }));
    setTimeout(function () { handler(foundObj) }, 50)
    $(window).on('DOMContentLoaded load resize scroll', handler);
}

function initBlogCategoryButtons() {
    var querySelector = $('.holderCats');
    if (querySelector.length == 1) {
        postForm('/Blog/Blog/GetCategories', new FormData(), function (res) {
            var result = '';

            if (res && res.length > 0) {
                for (var i = 0; i < res.length; i++) {
                    result += getBlogCatTemplate(res[i]);
                }
            }

            querySelector.html(result);
            initBlogCategoryButtonInitFunctions(querySelector);
            doSearch();
        }, null, function () { });
    }
}

function getBlogCatTemplate(catItem) {
    var result = '';

    if (catItem && catItem.id && catItem.title) {
        result += '<a data-id="' + catItem.id + '" class="blogCatButton  ' + (catItem.id == Number.parseInt(catId) ? 'mainBtn' : 'secountBtn') + ' ">' + catItem.title + '<input type="hidden" name="catIds" value="' + (catItem.id == Number.parseInt(catId) ? catItem.id : '') + '" /></a>';
    }

    return result;
}

function initBlogCategoryButtonInitFunctions(querySelector) {
    querySelector.find('.blogCatButton').click(function () {
        movePaginationToFist();
        if ($(this).hasClass('secountBtn')) {
            $(this).removeClass('secountBtn').addClass('mainBtn');
            $(this).find('input').val($(this).attr('data-id'));
        } else {
            $(this).removeClass('mainBtn').addClass('secountBtn');
            $(this).find('input').val('');
        }

        doSearch();
    });
}

function movePaginationToFist() {
    mobilePage = 1;
    $('.page-item.active').removeClass('active');
    $('.page-item').find('a[data-value="1"]').closest('.page-item').addClass('active');
}

function showBlogLoaders() {
    showLoader($('#blogContainer .blogSearch'));
    showLoader($('#blogContainer .blogItemsSection'));
}

function hideBlogLoaders() {
    hideLoader($('#blogContainer .blogSearch'));
    hideLoader($('#blogContainer .blogItemsSection'));
}

function getCurrPage(qureSelector) {
    var result = 1;

    result = Number.parseInt(qureSelector.find('.active a').attr('data-value'));

    if (isNaN(result))
        result = 1;

    return result;
}

function filltempArrayForPagination(cPage, pageCount) {
    var pages = [];
    var offset = 2;

    for (var i = 1; i <= pageCount; i++) {
        if (
            i == pageCount ||
            i == 1 ||
            i == cPage ||
            (i > cPage && i <= cPage + offset && i <= pageCount) ||
            (i < cPage && i >= cPage - offset && i >= 1) ||
            (cPage - (offset + 1) <= 0 && i <= (cPage + 1 + offset - (cPage - (offset + 1)))) ||
            ((cPage + offset + 1 > pageCount) && i > cPage - 1 - offset - (cPage + offset + 1 - pageCount))

        )
            pages.push(i);
    }

    if (pages.length >= 2 && pages[1] - pages[0] > 1)
        pages[1] = '...';
    if (pages.length >= 2 && pages[pages.length - 1] - pages[pages.length - 2] > 1)
        pages[pages.length - 2] = '...';

    return pages;
}

function updateBlogPagination(pageCount) {
    var qureSelector = $('.holderPagination');
    if (pageCount > 1) {
        var result = '<ul class="pagination">';
        var cPage = getCurrPage(qureSelector);

        result += '<li class="page-item pMovePrev  ' + (cPage == 1 ? 'disabled' : '') + ' "><a class="page-link" href="#">قبلی</a></li>';

        var tempArr = filltempArrayForPagination(cPage, pageCount);

        for (var i = 0; i < tempArr.length; i++) {
            result += '<li  class="page-item ' + (cPage == tempArr[i] ? 'active' : '') + ' ' + ('...' == tempArr[i] ? 'disabled' : '') + '"><a data-value="' + tempArr[i] + '" class="page-link" href="#">' + tempArr[i] + '</a></li>';
        }

        result += '<li class="page-item pMoveNext ' + (cPage == pageCount ? 'disabled' : '') + '"><a class="page-link" href="#">بعدی</a></li>';
        result += '</ul>';
        qureSelector.html(result);
        initBlogPageClickEvent(qureSelector);
    }
    else {
        qureSelector.html('');
    }
}

function initBlogPageClickEvent(holderQuery) {
    holderQuery.find('.page-item a').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        if ($(this).closest('.page-item').hasClass('disabled'))
            return;


        if ($(this).closest('.page-item').hasClass('pMovePrev')) {
            var targetObj = $(this).closest('.pagination').find('.active').prev();
            $(this).closest('.pagination').find('.active').removeClass('active');
            targetObj.addClass('active');
        }
        else if ($(this).closest('.page-item').hasClass('pMoveNext')) {
            var targetObj = $(this).closest('.pagination').find('.active').next();
            $(this).closest('.pagination').find('.active').removeClass('active');
            targetObj.addClass('active');
        }
        else {
            $(this).closest('.pagination').find('.active').removeClass('active');
            $(this).closest('.page-item').addClass('active');
        }
        doSearch();
        return false;
    });
}

function getBlogItemTemplate(blogItem) {
    return `
    <div class="BlogItem">
        <a href="${blogItem.url}" title="${blogItem.title}" class="BlogItemHolderImage">
            <i class="BlogItemCat">${blogItem.catTitle}</i>
            <span></span>
            <img width="285" height="200" alt="${blogItem.title}" data-src="${blogItem.src}" />
            <i class="blogItemFav">${blogItem.fCount}</i>
            <i class="blogItemCom">${blogItem.mCount}</i>
        </a>
        <div class="BlogItemHolderTitle">
            <a title="${blogItem.title}" href="${blogItem.url}">${blogItem.title}</a>
        </div>
        <div class="BlogItemDescription">
            ${blogItem.summery}
        </div>
        <div class="blogOwnerAndTime">
            <div>${blogItem.createUserFullname}</div><div>
                <img alt="تاریخ ایجاد" width="16" height="16" src="/Modules/Assets/Blogs/clock.svg" />
                ${blogItem.createDate}
            </div>
        </div>
    </div>
`;
}

var mobilePage = 1;

function doSearch(mobileV) {
    var postParams = getFormData($('#blogContainer'));
    if (!mobileV)
        postParams.append('page', getCurrPage($('.holderPagination')));
    else {
        mobilePage++;
        postParams.append('page', mobilePage);
    }

    if (window['keyWordId'])
        postParams.append('keyWordId', keyWordId);

    showBlogLoaders();
    postForm('/Blog/Blog/Search', postParams, function (res) {
        if (res && res.data && res.data.length > 0) {
            var result = '';

            for (var i = 0; i < res.data.length; i++) {
                result += getBlogItemTemplate(res.data[i]);
            }

            $('.blogSearchSummery').find('strong').html(res.total);
            if (!mobileV)
                $('.holderBlogItems').html(result);
            else
                $('.holderBlogItems').append(result);
            $('.holderBlogItems [data-src]').loadImageOnScroll();
            updateBlogPagination(res.pageCount);
            if (res.pageCount <= mobilePage)
                $('.loadMoreButton').css('display', 'none');
            else
                $('.loadMoreButton').css('display', 'block');
        } else {
            if (!mobileV)
                $('.holderBlogItems').html('<div style="line-height:200px;" >اطلاعاتی یافت نشد</div>');
        }
    }, null, function () { hideBlogLoaders(); });
}

function initBlogDropdown() {
    $('#holderDropdownType').html(getDropdownCTRLTemplate({
        "parentCL": "col-xl-4 col-lg-4 col-md-3 col-sm-6 col-xs-12",
        "name": "typeId",
        "type": "dropDown",
        "textfield": "title",
        "valuefield": "id",
        "dataurl": "/Core/BaseData/Get/BlogTypes",
        "label": "نوع"
    }));
    $('#holderDropdownType').find('select').change(function () {
        movePaginationToFist();
        doSearch();
    });

    $('#holderDropdownSort').html(getDropdownCTRLTemplate({
        "parentCL": "col-xl-4 col-lg-4 col-md-3 col-sm-6 col-xs-12",
        "name": "sortID",
        "type": "dropDown",
        "textfield": "title",
        "valuefield": "id",
        "dataurl": "/Core/BaseData/Get/BlogSortTypes",
        "label": "مرتب سازی"
    }));
    $('#holderDropdownSort').find('select').change(function () {
        movePaginationToFist();
        doSearch();
    });

    executeArrFunctions();
}

initBlogPage();