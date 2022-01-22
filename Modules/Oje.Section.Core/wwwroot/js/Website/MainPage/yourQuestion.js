
$.fn.initMyQuestionItem = function ()
{
    return this.each(function ()
    {
        $(this).click(function ()
        {
            $(this).toggleClass('yourQuestionItemOpen');
        });
    });
};

$.fn.loadYourQuestionList = function ()
{
    return this.each(function ()
    {
        postForm('/Question/YourQuestion/GetList', new FormData(), function (res)
        {
            if (res && res.length > 0) {
                var template = getYourQuestionBeginTemplate();

                for (var i = 0; i < res.length; i++) {
                    template += getYourQuestionItemTemplate(res[i]);
                }

                template += getYourQuestionEndTemplate();
                $(this.curThis).html(template);
                $(this.curThis).find('.yourQuestion .yourQuestionItem').initMyQuestionItem();
            }
            else {
                $(this.curThis).css('display', 'none');
            }
        }.bind({curThis: this}));
    });
};

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
            ${q.a}
        </div>
    </div>
`;
}

function getYourQuestionEndTemplate() {
    return '</div></section>';
}