﻿
$.fn.initMyQuestionItem = function () {
    return this.each(function () {
        $(this).click(function () {
            $(this).toggleClass('yourQuestionItemOpen');
        });
    });
};

$.fn.loadYourQuestionList = function (url, exteraParameters) {
    return this.each(function () {
        var postData = new FormData()
        if (exteraParameters) {
            for (var prop in exteraParameters) {
                postData.append(prop, exteraParameters[prop]);
            }
        }
        postForm(url, postData, function (res) {
            if (res && res.length > 0) {
                var template = getYourQuestionBeginTemplate();

                for (var i = 0; i < res.length; i++) {
                    template += getYourQuestionItemTemplate(res[i]);
                }

                template += getYourQuestionEndTemplate();
                $(this.curThis).html(template);
                $(this.curThis).find('.yourQuestion .yourQuestionItem').initMyQuestionItem();
                $(this.curThis).show();
                addQuestionToJData(res);
            }
            else {
                $(this.curThis).hide();
            }
        }.bind({ curThis: this }), null, null, null, 'GET');
    });
};

function addQuestionToJData(res) {
    if (res && res.length > 0) {
        var result = {
            "@context": "https://schema.org",
            "@type": "FAQPage",
            "mainEntity" : []
        };
        for (var i = 0; i < res.length; i++) {
            if (res[i].q && res[i].a) {
                result.mainEntity.push({
                    "@type": "Question",
                    "name": res[i].q,
                    "acceptedAnswer": {
                        "@type": "Answer",
                        "text": res[i].a
                    }
                });
            }
        }
        $('head').append('<script type="application/ld+json" >' + JSON.stringify(result) +'</script>');
    }
}

function getYourQuestionBeginTemplate() {
    return `
        <section class="yourQuestion ">
            <div class="yourQuestionTitle">پرسش های شما</div>
            <div class="holderyourQuestionItems">
`;
}

function getYourQuestionItemTemplate(q) {
    return `
    <div class="yourQuestionItem">
        <i></i>
        <div class="yourQuestionItemTitle">
            ${q.q}
        </div>
        <div class="yourQuestionItemAnswer">
            ${(q.a + '').replace(/\n/g,'<br />')}
        </div>
    </div>
`;
}

function getYourQuestionEndTemplate() {
    return '</div></section>';
}