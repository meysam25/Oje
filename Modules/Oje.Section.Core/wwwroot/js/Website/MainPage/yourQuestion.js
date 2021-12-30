
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