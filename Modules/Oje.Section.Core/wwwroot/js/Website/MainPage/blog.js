
$.fn.bindBlog = function (url) {
    if (!url)
        return null;

    function getFirstItemHtml(item) {
        return `
            <div class="blogSectionMainBlog">
                <div>
                    <a title="${item.title}" href="${item.url}" class="blogSectionMainBlogImg"><i></i><img alt="${item.title}" width="600" height="400" data-src="${item.img1}" ></a>
                </div>
                <div class="blogSectionMainBlogTitleAndDes">
                    <a href="${item.url}" title="${item.title}" class="blogSectionMainBlogTitle">${item.title}</a>
                    <a href="${item.url}" title="${item.title}" class="blogSectionMainBlogDescription">
                        ${item.desc}
                    </a>
                </div>
            </div>
`;
    }

    function getOthersItemHtml(item) {
        return `
            <div class="blogSectionSubBlogItem">
                <div ><a title="${item.title}" class="blogSectionSubBlogItemImg" href="${item.url}" ><i></i><img alt="${item.title}" width="135" height="100" data-src="${item.img2}"></a></div>
                <div class="blogSectionSubBlogItemTitleAndDes">
                    <a href="${item.url}" title="${item.title}" class="blogSectionSubBlogItemTitle">${item.title}</a>
                    <a href="${item.url}" title="${item.title}" class="blogSectionSubBlogItemDescription">
                        ${item.desc}
                    </a>
                </div>
            </div>
`;
    }

    return this.each(function () {
        postForm(url, new FormData(), function (res) {
            if (res && res.length > 0) {
                var result = `<section class="blogSection ">
                                <div style="text-align:center;padding-bottom:20px;padding-top:15px;"><div class="ourPrideSectionTitle"><span>اخبار و مقالات</span></div></div>
                                <div class="blogSectionEntity">`;

                for (var i = 0; i < res.length; i++) {
                    if (i == 0)
                        result += getFirstItemHtml(res[i]);
                    else {
                        if (i == 1)
                            result += '<div class="blogSectionSubBlog">';
                        result += getOthersItemHtml(res[i]);
                    }
                }
                result += '</div>';

                result += `
                                </div>
                                <div class="blogSectionHolderButton"><a class="mainBtn" href="/Blogs">همه اخبار و مقالات</a></div>
                            </section>`;
                $(this.curThis).css('display','block').addClass('myContainerEffect1Active').html(result);
                $(this.curThis).find('img[data-src]').loadImageOnScroll();
            }
        }.bind({ curThis: this }), null, function () {

        });

    });
};
