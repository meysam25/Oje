
$.fn.initLoginButton = function (url, modalId) {
    return this.each(function () {
        var newId = uuidv4RemoveDash();
        $('body').append('<div id="' + newId + '" ></div>');
        loadJsonConfig(url, newId);
        $(this).click(function ()
        {
            if ($('#' + modalId).length > 0) {
                $('#' + modalId).modal('show');
            }
        });
    });
}