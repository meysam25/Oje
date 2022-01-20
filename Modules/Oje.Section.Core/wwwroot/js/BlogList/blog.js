
var isLikeProcess = false;
$('.articleContentLikeButton').click(function () {
    if (isLikeProcess)
        return;
    $(this).toggleClass('articleContentLikeButtonActive');
    var pfd = new FormData();
    pfd.append('action', 'likeOrDisLike');
    postForm(location.href, pfd, function (res) {
        if (!isNaN(res)) {
            $('#likeCount span').html(res);
        }
    }, null, function () { isLikeProcess = false; });
});

loadJsonConfig('/Blog/Blog/GetAddReviewJsonConfig', $('.articleUouViewSectioninputsHolder'));

function submitNewReviewForBlog(curButton) {
    var panelQuery = $(curButton).closest('.myPanel');
    if (panelQuery.length > 0) {
        var postData = getFormData(panelQuery);
        showLoader(panelQuery);
        postData.append('action', 'newReview');
        postForm(location.href, postData, function (res) {
            if (res && res.isSuccess == true) {
                clearForm(panelQuery);
                updateComments();
            }
        }, null, function () {
            hideLoader(panelQuery);
        });
    }
}

function updateComments() {
    var pfd = new FormData();
    pfd.append('action', 'commentList');
    postForm(location.href, pfd, function (res) {
        var html = '';

        if (res && res.length > 0)
            for (var i = 0; i < res.length; i++) {
                html += bindCommentTemplate(res[i]);
            }

        $('.otherViews').html(html);
    })
}

function bindCommentTemplate(item) {
    var result = '';

    if (item) {
        var confirmStr = '';
        if (!item.ic)
            confirmStr = '(تایید نشده)';
        var des = item.des.replace(/\n/g, '<br />')
        result = `
        <div class="othersViewItem">
            <div class="otherViewHeader">
                <div>
                    <i class="fa fa-user"></i>
                    <span>${item.fn} ${confirmStr}</span>
                </div>
                <div>
                    <i class="fa fa-clock">${item.cd}</i>
                </div>
            </div>
            <div class="othersViewContent">
                ${des}
            </div>
        </div>
`;
    }

    return result;
}

updateComments();