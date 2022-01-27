
$.fn.initMainPageAboutUs = function (url) {
    return this.each(function () {
        postForm(url, new FormData(), function (res)
        {
            var selectQuery = $(this.curThis);
            if (res) {
                var imagesQuerySelect = selectQuery.find('.aboutUsSectionAbilityIcon img');
                if (imagesQuerySelect.length == 3) {
                    imagesQuerySelect.eq(0).attr('data-src', res.rightFile_address).attr('alt', res.rightFileTitle );
                    imagesQuerySelect.eq(1).attr('data-src', res.centerFile_address).attr('alt', res.centerFileTitle);
                    imagesQuerySelect.eq(2).attr('data-src', res.leftFile_address).attr('alt', res.leftFileTitle);
                }
                var imagesSubTitleQuerySelect = selectQuery.find('.aboutUsSectionAbilityIcon div');
                if (imagesSubTitleQuerySelect.length == 3) {
                    imagesSubTitleQuerySelect.eq(0).html(res.rightFileTitle);
                    imagesSubTitleQuerySelect.eq(1).html(res.centerFileTitle);
                    imagesSubTitleQuerySelect.eq(2).html(res.leftFileTitle);
                }
                selectQuery.find('.aboutUsSectionTitle').html(res.title);
                selectQuery.find('.aboutUsSectionSubTitle').html(res.subTitle);
                selectQuery.find('.aboutUsSectionDescription').html(res.desc);

                selectQuery.css('display', 'block');
                selectQuery.find('img[data-src]').loadImageOnScroll();
            } else {
                selectQuery.css('display', 'none');
            }
        }.bind({ curThis: this }), null, null);
    });
}