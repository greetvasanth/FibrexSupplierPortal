var isDirtyCheckbox = false;
var isDirtycmp = false;
var isDirtystcmp = false;
var isDirtyselectCountry = false;
var isDirtyselectBusiness = false;
var isDirtyselectEmail = false;
var isDirtyselectFirstName = false;
var isDirtyselectLastName = false;
var isDirtyPostion = false;
var isDirtyMobile = false;
var isDirtyPhone = false;
var isDirtyExtention = false;
var isDirtyTradelicense = false;
var isDirtyIssuingAuth = false;
var isDirtyExpire = false;
var isDirtySelectCountry = false;
var isDirtyAddressLine1 = false;
var isDirtyAddressLine2 = false;
var isDirtyCity = false;
var isDirtpostalCode = false;
var isDirtFax = false;
var isDirtphone = false;
var IsDirtyFileDelete = false;
var submitted = false;

$(document).ready(function () {
    //$('input:text,input:checkbox,input:radio,textarea,select').change(function () {

    $(document).on('change', 'input:checkbox', function (e) {
        //$('input:checkbox').change(function () {
        if (this.checked) {
            isDirtyCheckbox = true;
        }
        else {
            isDirtyCheckbox = false;
        }
    });

    //$('#txtCompanyName').change(function () {
    $(document).on('change', '#txtCompanyName', function (e) {
        if (this.value != '') {
            isDirtycmp = true;
        }
        else {
            isDirtycmp = false;
        }
    });
    //$('#txtCompanyShortName').change(function () {
    $(document).on('change', '#txtCompanyShortName', function (e) {
        if (this.value != '') {
            isDirtystcmp = true;
        }
        else {
            isDirtystcmp = false;
        }
    });
    //$('#ddlCountry').change(function () {
    $(document).on('change', '#ddlCountry', function (e) {
        if (this.value != 'Select') {
            isDirtyselectCountry = true;
        }
        else {
            isDirtyselectCountry = false;
        }
    });
    //$('#ddlBusinessClassficiation').change(function () { 
    $(document).on("change", "#ddlBusinessClassficiation", function (e) {
        if (this.value != 'Select') { 
            isDirtyselectBusiness = true;
        }
        else { 
            isDirtyselectBusiness = false;
        }
    });
    //$('#txtOfficalEmail').change(function () {
    $(document).on('change', '#txtOfficalEmail', function (e) {
        if (this.value != '') {
            isDirtyselectEmail = true;
        }
        else {
            isDirtyselectEmail = false;
        }
    });
    //$('#txtContactFirstName').change(function () {
    $(document).on('change', '#txtContactFirstName', function (e) {
        if (this.value != '') {
            isDirtyselectFirstName = true;
        }
        else {
            isDirtyselectFirstName = false;
        }
    });
    //$('#txtContactLastName').change(function () {
    $(document).on('change', '#txtContactLastName', function (e) {
        if (this.value != '') {
            isDirtyselectLastName = true;
        }
        else {
            isDirtyselectLastName = false;
        }
    });
    //$('#txtPosition').change(function () {
    $(document).on('change', '#txtPosition', function (e) {
        if (this.value != '') {
            isDirtyPostion = true;
        }
        else {
            isDirtyPostion = false;
        }
    });
    //$('#txtMobile').change(function () {
    $(document).on('change', '#txtMobile', function (e) {
        if (this.value != '') {
            isDirtyMobile = true;
        }
        else {
            isDirtyMobile = false;
        }
    });
    //$('#txtPhone').change(function () {
    $(document).on('change', '#txtPhone', function (e) {
        if (this.value != '') {
            isDirtyPhone = true;
        }
        else {
            isDirtyPhone = false;
        }
    });
    //$('#txtExtension').change(function () {
    $(document).on('change', '#txtPhone', function (e) {
        if (this.value != '') {
            isDirtyExtention = true;
        }
        else {
            isDirtyExtention = false;
        }
    });
    //$('#txtIssuingAuthority').change(function () {
    $(document).on('change', '#txtIssuingAuthority', function (e) {
        if (this.value != '') {
            isDirtyIssuingAuth = true;
        }
        else {
            isDirtyIssuingAuth = false;
        }
    });
    //$('#txtTradeLicenseNum').change(function () {
    $(document).on('change', '#txtTradeLicenseNum', function (e) {
        if (this.value != '') {
            isDirtyTradelicense = true;
        }
        else {
            isDirtyTradelicense = false;
        }
    });
    //$('#txtExpireDate').change(function () {
    $(document).on('change', '#txtExpireDate', function (e) {
        if (this.value != '') {
            isDirtyExpire = true;
        }
        else {
            isDirtyExpire = false;
        }
    });
    //$('#ddlAddressLine1Country').change(function () {
    $(document).on('change', '#ddlAddressLine1Country', function (e) {
        if (this.value != 'Select') {
            isDirtySelectCountry = true;
        }
        else {
            isDirtySelectCountry = false;
        }
    });
    //$('#txttabAddress1').change(function () {
    $(document).on('change', '#txttabAddress1', function (e) {
        if (this.value != '') {
            isDirtyAddressLine1 = true;
        }
        else {
            isDirtyAddressLine1 = false;
        }
    });
    //$('#txttabAddress2').change(function () {
    $(document).on('change', '#txttabAddress2', function (e) {
        if (this.value != '') {
            isDirtyAddressLine2 = true;
        }
        else {
            isDirtyAddressLine2 = false;
        }
    });
    //$('#txtAddress1City').change(function () {
    $(document).on('change', '#txtAddress1City', function (e) {
        if (this.value != '') {
            isDirtyCity = true;
        }
        else {
            isDirtyCity = false;
        }
    });

    $(document).on('change', '#txtVATRegistrationNo', function (e) {
        if (this.value != '') {
            isDirtpostalCode = true;
        }
        else {
            isDirtpostalCode = false;
        }
    });
    $(document).on('change', '#txtVATGroupNum', function (e) {
        if (this.value != '') {
            isDirtpostalCode = true;
        }
        else {
            isDirtpostalCode = false;
        }
    });

    //$('#txtAddressPostalCode').change(function () {
    $(document).on('change', '#txtAddressPostalCode', function (e) {
        if (this.value != '') {
            isDirtpostalCode = true;
        }
        else {
            isDirtpostalCode = false;
        }
    });
    //$('#txtAddress1Phone').change(function () {
    $(document).on('change', '#txtAddress1Phone', function (e) {
        if (this.value != '') {
            isDirtphone = true;
        }
        else {
            isDirtphone = false;
        }
    });
    //$('#txtAddress1FaxNum').change(function () {
    $(document).on('change', '#txtAddress1FaxNum', function (e) {
        if (this.value != '') {
            isDirtFax = true;
        }
        else {
            isDirtFax = false;
        }
    });
      
    window.onbeforeunload = function (e) {
        if (((isDirtycmp) || (isDirtyCheckbox) || (isDirtyselectCountry) || (isDirtystcmp) || (isDirtyselectBusiness)
            || (isDirtyselectEmail) || (isDirtyselectFirstName) || (isDirtyselectLastName) || (isDirtyPostion)
            || (isDirtyMobile) || (isDirtyPhone) || (isDirtyExtention) || (isDirtyTradelicense) || (isDirtyExpire)
            || (isDirtySelectCountry) || (isDirtyAddressLine1) || (isDirtyAddressLine2) || (isDirtyCity) || (isDirtpostalCode)
            || (isDirtphone) || (isDirtFax) || (IsDirtyFileDelete)) && !submitted) {

            var message = "Are you sure? You haven\’t saved your changes. Click OK to leave or Cancel to go back and save your changes.", e = e || window.event;
            if (e) {
                e.returnValue = message;
            }
            return message;
        }
    }

    $('#btnSave').click(function () {
        $("form").submit(function () {
            submitted = true;
        });
    });
     
    //$('input:submit').click(function (e) {
    //    isDirtyCheckbox = false;
    //    isDirtycmp = false;
    //    isDirtystcmp = false;
    //    isDirtyselectCountry = false;
    //    isDirtyselectBusiness = false;
    //    isDirtyselectEmail = false;
    //    isDirtyselectFirstName = false;
    //    isDirtyselectLastName = false;
    //    isDirtyPostion = false;
    //    isDirtyMobile = false;
    //    isDirtyPhone = false;
    //    isDirtyExtention = false;
    //    isDirtyTradelicense = false;
    //    isDirtyIssuingAuth = false;
    //    isDirtyExpire = false;
    //    isDirtySelectCountry = false;
    //    isDirtyAddressLine1 = false;
    //    isDirtyAddressLine2 = false;
    //    isDirtyCity = false;
    //    isDirtpostalCode = false;
    //    isDirtFax = false;
    //    isDirtphone = false; 
    //});
});