function initAllCkEditors() {
    $('.ckEditor').each(function () {
        if ($(this).closest('.myCtrl').find('.ck-editor').length > 0)
            return;
        ClassicEditor
            .create($(this)[0], {
                language: 'fa',
                ckfinder: {
                    uploadUrl: '/Core/BaseData/UploadFile/'
                }
            })
            .then(editor => {
                $(this)[0].ckEditor = editor;
            })
            .catch(err => {
                console.error(err.stack);
            });
    });
}
initAllCkEditors();