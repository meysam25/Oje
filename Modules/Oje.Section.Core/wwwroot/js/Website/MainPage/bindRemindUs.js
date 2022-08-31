
$.fn.loadAndBindRemindUsSection = function (url) {
    return this.each(function () {
        postForm(url, new FormData(), function (res) {
            var selectQuery = $(this.curThis);
            if (res) {
                selectQuery.find('.reminderMainLogo').attr('data-src', res.mainImage_address).attr('alt', res.title);
                selectQuery.find('.reminderSectionDescriptionTextsTitle').html(res.title);
                selectQuery.find('.reminderSectionDescriptionTextsDescription').html(res.desc);
                selectQuery.css('display', 'block');
                selectQuery.find('img[data-src]').loadImageOnScroll();
            } else {
                selectQuery.css('display', 'none');
            }
        }.bind({ curThis: this }), null, null, null, 'GET');
    });
}