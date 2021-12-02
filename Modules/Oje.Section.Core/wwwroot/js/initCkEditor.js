$('.ckEditor').each(function () {
    ClassicEditor
        .create($(this)[0], {
            language: 'fa',
            cloudServices: {
                tokenUrl: '/Core/BaseData/GenerateToken',
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