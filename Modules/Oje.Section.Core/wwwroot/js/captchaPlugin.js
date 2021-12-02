$.fn.initCaptcha = function () {
    return this.each(function () {
        $(this).attr('data-captcha', 'true');
        $(this).append('<img  />');
        $(this).append('<input type="text" name="sCode" placeholder="کد امنیتی" autocomplete="off" />');
        $(this).append('<input type="hidden" name="sCodeGuid" placeholder="کد امنیتی" />');
        $(this).append('<span class="fa fa-sync-alt" ></span>');
        $(this)[0].refreshCode = function () {
            var postData = new FormData();
            var curThis = this;
            postForm('/Core/BaseData/GenerateCaptch', postData, function (res) {
                if (res)
                {
                    $(curThis).find('input[type="hidden"]').val(res);
                    $(curThis).find('input[type="text"]').val('');
                    $(curThis).find('img').attr('src', '/Core/BaseData/GetCaptchaImage/' + res);
                }
            });
        }
        $(this)[0].refreshCode();
        $(this).find('span').click(function () { $(this).closest('[data-captcha]')[0].refreshCode() });
    });
}