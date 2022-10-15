
function initDisigner(ctrl) {
    if (ctrl) {
        var quirySelector = $('#' + ctrl.id);
        if (quirySelector.length > 0) {
            var template = generateDesigner(ctrl);
            quirySelector.html(template);

            initDisignerEventAndFunction(ctrl, quirySelector);
        }
    }
}

function generateDesigner(ctrl) {
    var result = '';

    result += `

<div class="formDesigner" >
    <div class="formDesignerPlateTopMenu" >${getTopMenuActions(ctrl)}</div>
    <div class="formDesignerPlateHolders" >
        <div class="formDesignerCtrlHolders" >${getActiveCtrls(ctrl)}</div>
        <div class="formDesignerPlate" >
            <div id="${ctrl.id}_renderPlace" ></div>
            <div class="formDesignerCtrlProperties" ></div>
        </div>
    </div>
</div>

`;

    return result;
}

function getActiveCtrls() {
    var result = '';

    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemTextBox" ><img src="/Modules/Images/textbox.png" alt="تکس باکس" /><span title ="تکس باکس" >تکس باکس</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemTextBoxArea" ><img src="/Modules/Images/textarea.png" alt="تکس اریا" /><span title ="تکس اریا" >تکس اریا</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemTextBox" ><img src="/Modules/Images/radio.png" alt="ردیو باکس" /><span title ="ردیو باکس" >ردیو باکس</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemTextBox" ><img src="/Modules/Images/checkbox.png" alt="چک باکس" /><span title ="چک باکس" >چک باکس</span></div>';
    result += '<div class="formDesignerCtrlItem formDesignerCtrlItemTextBox" ><img src="/Modules/Images/fileupload.png" alt="فایل" /><span title ="فایل" >فایل</span></div>';

    return result;
}

function getTopMenuActions() {
    var result = '';

    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuNew fa fa-plus-circle" style="color:red;" title="جدید" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuExport fa fa-download" title="خروجی" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuImport fa fa-upload" title="ورودی" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuUndo fa fa-undo" title="قبلی" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuRedo fa fa-redo" title="بعدی" ></div>';
    result += '<div class="formDesignerPlateTopMenuItem formDesignerPlateTopMenuInfo fa fa-info" title="خصوصیات" ></div>';

    return result;
}

function initDisignerEventAndFunction(ctrl, quirySelector) {
    var curObj = quirySelector[0];
    curObj.ctrl = ctrl;
    initReNewFunction(curObj);
    curObj.reNew();
    quirySelector.find('.formDesignerPlateTopMenuNew').click(function () { $(this).closest('.myFormDesigner')[0].reNew(); })
}

function initReNewFunction(curObj) {
    curObj.reNew = function () {
        var curCtrl = this.ctrl;
        if (curCtrl && curCtrl.baseConfig) {
            $('#' + curCtrl.id + '_renderPlace').html('');
            generateForm(JSON.parse(JSON.stringify(curCtrl.baseConfig)), curCtrl.id + '_renderPlace')
        }
    };
}