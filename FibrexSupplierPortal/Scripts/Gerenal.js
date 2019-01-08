function ConfirmDelete(Title) {   
    var x = confirm("Are you sure you want to delete the attachment " + Title + " for this record?");   
    if (x) {
        IsDirtyFileDelete = true;
        isDirty1 = true; 
        return true;
    }
    else {
        return false;
    }
}
function ConfirmPOLineDelete() {
    var x = confirm("Are you sure you want to delete this Record?");
    if (x) {
        isDirty1 = true;
        IsDirtyFileDelete = true; 
        return true;
    }
    else {
        return false;
    }
}
function ConfirmPOPermissionDelete() {
    var x = confirm("Are you sure you want to delete this permission from the purchase order?");
    if (x) {
        isDirty1 = true;
        IsDirtyFileDelete = true;
        return true;
    }
    else {
        return false;
    }
}

function ConfirmSignatureDelete() {
    var x = confirm("Are you sure you want to delete this record?");
    if (x) {
        IsDirtyFileDelete = true;
        isDirty1 = true;
        return true;
    }
    else {
        return false;
    }
}

function ConfirmPOLineUndoDelete() {
    var x = confirm("Are you sure you want to add this Record again?");
    if (x) {
        isDirty1 = true;
        IsDirtyFileDelete = true;
        return true;
    }
    else {
        return false;
    }
}


function ConfirmDeleteSignature() {
    var x = confirm("Are you sure you want to delete this Record?");
    if (x) { 
        return true;
    }
    else {
        return false;
    }
}

var ctrlDown = false;
var ctrlKey = 17, f5Key = 116, rKey = 82;

$(document).keydown(function (e) {
    if (e.keyCode == f5Key) {
        window.location.href = window.location.href;
        e.preventDefault();
    }

    if (e.keyCode == ctrlKey)
        ctrlDown = true;
    if (ctrlDown && (e.keyCode == rKey)) {
        window.location.href = window.location.href;
        //Ctrl + R pressed. Do whatever you want
        //or copy the same code here that you did above
        e.preventDefault();
    }

}).keyup(function (e) {
    if (e.keyCode == ctrlKey)
        ctrlDown = false;
});


function Opendownload(RowIndex) {
    var rowData = RowIndex.parentNode.parentNode;
    var rowIndex = rowData.rowIndex - 1;
    window.location.href = 'FileDownload.ashx?RowIndex=' + rowIndex;
    //window.open('FileDownload.ashx?RowIndex=' + rowIndex);
    // window.open('frmDownloadAttachment?RegID=123&RowIndex=' + rowIndex, '_blank', 'menubar=0,resizable=0,width=350,height=250,toolbars=0');
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function validateFloatKeyPress(el, evt) {
    IsPoUpdate = true;
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = el.value.split('.');
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    //just one dot
    if (number.length > 1 && charCode == 46) {
        return false;
    }
    //get the carat position
    var caratPos = getSelectionStart(el);
    var dotPos = el.value.indexOf(".");
    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 3)) {
        return false;
    }
    return true;
}
 
function getSelectionStart(o) {
    if (o.createTextRange) {
        var r = document.selection.createRange().duplicate()
        r.moveEnd('character', o.value.length)
        if (r.text == '') return o.value.length
        return o.value.lastIndexOf(r.text)
    } else return o.selectionStart
}

function GetSelectedRow(lnk) {
    var row = lnk.parentNode.parentNode;
    var rowIndex = row.rowIndex - 1;
    var Title = row.cells[0].getElementsByTagName("span")[0].innerHTML;
    var answer = ConfirmDelete(Title);
    if (answer)
        return true;
    else
        return false;
}

function ValidateSpace(el, evt) {
    if (el.which == 32)
        return;
}
function Comma(Num) {
    IsPoUpdate = true; 
    Num += '';
    Num = Num.replace(/,/g, '');
    x = Num.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1))
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    return x1 + x2;
}
 
