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
var isDirtAddress2Country = false;
var isDirtyAd2AddressLine1 = false;
var isDirtyAd2AddressLine2 = false;
var isDirtyAd2City = false;
var isDirtyAd2PostalCode = false;
var isDirtyAd2Phone = false;
var isDirtyAd2Fax = false;
var isDirtyddlPaymentMethod = false;
var isDirtyddlBankCountry = false;
var isDirtyBankName = false;
var isDirtyBankAddress = false;
var IsDirtyAccountNumber = false;
var IsDirtyAccountName = false;
var IsDirtyIBAN = false;
var IsDirtyFileTitle = false;
var IsDirtyFileDescription = false;
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
    //$('#ContentPlaceHolder1_txtCompanyName').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtCompanyName', function (e) {
        if (this.value != '') {
            isDirtycmp = true;
        }
        else {
            isDirtycmp = false;
        }
    });
        //$('#ContentPlaceHolder1_txtCompanyShortName').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtCompanyShortName', function (e) {
        if (this.value != '') {
            isDirtystcmp = true;
        }
        else {
            isDirtystcmp = false;
        }
    });
        //$('#ContentPlaceHolder1_ddlCountry').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_ddlCountry', function (e) {
        if (this.value != 'Select') {
            isDirtyselectCountry = true;
        }
        else {
            isDirtyselectCountry = false;
        }
    });
        //$('#ContentPlaceHolder1_ddlBusinessClassficiation').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_ddlBusinessClassficiation', function (e) {
        if (this.value != 'Select') { 
            isDirtyselectBusiness = true;
        }
        else { 
            isDirtyselectBusiness = false;
        }
    });
    //$('#ContentPlaceHolder1_txtOfficalEmail').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtOfficalEmail', function (e) {
        if (this.value != '') {
            isDirtyselectEmail = true;
        }
        else {
            isDirtyselectEmail = false;
        }
    });
    //$('#ContentPlaceHolder1_txtContactFirstName').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtContactFirstName', function (e) {
        if (this.value != '') {
            isDirtyselectFirstName = true;
        }
        else {
            isDirtyselectFirstName = false;
        }
    });
    //$('#ContentPlaceHolder1_txtContactLastName').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtContactLastName', function (e) {
        if (this.value != '') {
            isDirtyselectLastName = true;
        }
        else {
            isDirtyselectLastName = false;
        }
    });
    //$('#ContentPlaceHolder1_txtPosition').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtPosition', function (e) {
        if (this.value != '') {
            isDirtyPostion = true;
        }
        else {
            isDirtyPostion = false;
        }
    });
    //$('#ContentPlaceHolder1_txtMobile').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtMobile', function (e) {
        if (this.value != '') {
            isDirtyMobile = true;
        }
        else {
            isDirtyMobile = false;
        }
    });
    //$('#ContentPlaceHolder1_txtPhone').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtPhone', function (e) {
        if (this.value != '') {
            isDirtyPhone = true;
        }
        else {
            isDirtyPhone = false;
        }
    });
        //$('#ContentPlaceHolder1_txtExtension').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtExtension', function (e) {
        if (this.value != '') {
            isDirtyExtention = true;
        }
        else {
            isDirtyExtention = false;
        }
    });
    //$('#ContentPlaceHolder1_txtIssuingAuthority').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtIssuingAuthority', function (e) {
        if (this.value != '') {
            isDirtyIssuingAuth = true;
        }
        else {
            isDirtyIssuingAuth = false;
        }
    });
    //$('#ContentPlaceHolder1_txtTradeLicenseNum').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtTradeLicenseNum', function (e) {
        if (this.value != '') {
            isDirtyTradelicense = true;
        }
        else {
            isDirtyTradelicense = false;
        }
    });
    //$('#ContentPlaceHolder1_txtExpireDate').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtExpireDate', function (e) {
        if (this.value != '') {
            isDirtyExpire = true;
        }
        else {
            isDirtyExpire = false;
        }
    });
    //$('#ContentPlaceHolder1_ddlAddressLine1Country').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_ddlAddressLine1Country', function (e) {
        if (this.value != 'Select') {
            isDirtySelectCountry = true;
        }
        else {
            isDirtySelectCountry = false;
        }
    });
    //$('#ContentPlaceHolder1_txttabAddress1').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txttabAddress1', function (e) {
        if (this.value != '') {
            isDirtyAddressLine1 = true;
        }
        else {
            isDirtyAddressLine1 = false;
        }
    });
    //$('#ContentPlaceHolder1_txttabAddress2').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txttabAddress2', function (e) {
        if (this.value != '') {
            isDirtyAddressLine2 = true;
        }
        else {
            isDirtyAddressLine2 = false;
        }
    });
    //$('#ContentPlaceHolder1_txtAddress1City').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtAddress1City', function (e) {
        if (this.value != '') {
            isDirtyCity = true;
        }
        else {
            isDirtyCity = false;
        }
    });
    //$('#ContentPlaceHolder1_txtAddressPostalCode').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtAddressPostalCode', function (e) {
        if (this.value != '') {
            isDirtpostalCode = true;
        }
        else {
            isDirtpostalCode = false;
        }
    });
    //$('#ContentPlaceHolder1_txtAddress1Phone').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtAddress1Phone', function (e) {
        if (this.value != '') {
            isDirtphone = true;
        }
        else {
            isDirtphone = false;
        }
    });
    //$('#ContentPlaceHolder1_txtAddress1FaxNum').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtAddress1FaxNum', function (e) {
        if (this.value != '') {
            isDirtFax = true;
        }
        else {
            isDirtFax = false;
        }
        });

    //Address 2

    //$('#ContentPlaceHolder1_ddlAddressLine1Country').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_ddlAddressCountry2', function (e) {
            if (this.value != 'Select') {
                isDirtAddress2Country = true;
            }
            else {
                isDirtAddress2Country = false;
            }
        });


    //$('#ContentPlaceHolder1_txttabAddress1').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtAddress2AddressLine1', function (e) {
            if (this.value != '') {
                isDirtyAd2AddressLine1 = true;
            }
            else {
                isDirtyAd2AddressLine1 = false;
            }
        });
    //$('#ContentPlaceHolder1_txttabAddress2').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtAddress2AddressLine2', function (e) {
            if (this.value != '') {
                isDirtyAd2AddressLine2 = true;
            }
            else {
                isDirtyAd2AddressLine2 = false;
            }
        });
    //$('#ContentPlaceHolder1_txtAddress1City').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtAddress2City', function (e) {
            if (this.value != '') {
                isDirtyAd2City = true;
            }
            else {
                isDirtyAd2City = false;
            }
        });
    //$('#ContentPlaceHolder1_txtAddressPostalCode').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtAddress2PostalCode', function (e) {
            if (this.value != '') {
                isDirtyAd2PostalCode = true;
            }
            else {
                isDirtyAd2PostalCode = false;
            }
        });
    //$('#ContentPlaceHolder1_txtAddress1Phone').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtAddress2PhoneNum', function (e) {
            if (this.value != '') {
                isDirtphone = true;
            }
            else {
                isDirtphone = false;
            }
        });
    //$('#ContentPlaceHolder1_txtAddress1FaxNum').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtAddress2FaxNum', function (e) {
            if (this.value != '') {
                isDirtyAd2Fax = true;
            }
            else {
                isDirtyAd2Fax = false;
            }
        });


        $(document).on('change', '#ContentPlaceHolder1_ddlPaymentMethod', function (e) {
            if (this.value != '') {
                isDirtyddlPaymentMethod = true;
            }
            else {
                isDirtyddlPaymentMethod = false;
            }
        });

        $(document).on('change', '#ContentPlaceHolder1_ddlBankCountry', function (e) {
            if (this.value != '') {
                isDirtyddlBankCountry = true;
            }
            else {
                isDirtyddlBankCountry = false;
            }
        });
        
        $(document).on('change', '#ContentPlaceHolder1_txtBankName', function (e) {
            if (this.value != '') {
                isDirtyBankName = true;
            }
            else {
                isDirtyBankName = false;
            }
        });

        $(document).on('change', '#ContentPlaceHolder1_txtBankAddress', function (e) {
            if (this.value != '') {
                isDirtyBankAddress = true;
            }
            else {
                isDirtyBankAddress = false;
            }
        });
    
        $(document).on('change', '#ContentPlaceHolder1_txtVATRegistrationNo', function (e) {
            if (this.value != '') {
                isDirtpostalCode = true;
            }
            else {
                isDirtpostalCode = false;
            }
        });
        $(document).on('change', '#ContentPlaceHolder1_txtVATGroupNum', function (e) {
            if (this.value != '') {
                isDirtpostalCode = true;
            }
            else {
                isDirtpostalCode = false;
            }
        });

        $(document).on('change', '#ContentPlaceHolder1_txtAccountNumber', function (e) {
            if (this.value != '') {
                IsDirtyAccountNumber = true;
            }
            else {
                IsDirtyAccountNumber = false;
            }
        });
        
        $(document).on('change', '#ContentPlaceHolder1_txtAccountName', function (e) {
            if (this.value != '') {
                IsDirtyAccountName = true;
            }
            else {
                IsDirtyAccountName = false;
            }
        });
        $(document).on('change', '#ContentPlaceHolder1_txtBankIBan', function (e) {
            if (this.value != '') {
                IsDirtyIBAN = true;
            }
            else {
                IsDirtyIBAN = false;
            }
        });
        $(document).on('change', '#ContentPlaceHolder1_txtPopupFileDescription', function (e) {
            if (this.value != '') {
                IsDirtyFileDescription = true;
            }
            else {
                IsDirtyFileDescription = false;
            }
        });

        $(document).on('change', '#ContentPlaceHolder1_txtPopupFileTitle', function (e) {
            if (this.value != '') {
                IsDirtyFileTitle = true;
            }
            else {
                IsDirtyFileTitle = false;
            }
        });
       
    //
    window.onbeforeunload = function (e) {
            if (((isDirtycmp) || (isDirtyCheckbox) || (isDirtyselectCountry) || (isDirtystcmp) || (isDirtyselectBusiness)
                || (isDirtyselectEmail) || (isDirtyselectFirstName) || (isDirtyselectLastName) || (isDirtyPostion)
                || (isDirtyMobile) || (isDirtyPhone) || (isDirtyExtention) || (isDirtyTradelicense) || (isDirtyExpire)
                || (isDirtySelectCountry) || (isDirtyAddressLine1) || (isDirtyAddressLine2) || (isDirtyCity) || (isDirtpostalCode)
                || (isDirtphone) || (isDirtFax) || (isDirtAddress2Country) || (isDirtyAd2AddressLine1) || (isDirtyAd2AddressLine2)
                || (isDirtyAd2City) || (isDirtyAd2PostalCode) || (isDirtyAd2Phone) || (isDirtyAd2Fax) || (isDirtyddlPaymentMethod)
                || (isDirtyBankName) || (isDirtyddlBankCountry) || (isDirtyBankAddress) || (IsDirtyAccountNumber) || (IsDirtyAccountName)
                || (IsDirtyIBAN) || (isDirtyIssuingAuth) || (IsDirtyFileDescription) || (IsDirtyFileTitle) || (IsDirtyFileDelete)) && !submitted) {
                // if (isDirty) {
                //var confirmExit = confirm('Are you sure? You haven\’t saved your changes. Click OK to leave or Cancel to go back and save your changes.');
                //if (confirmExit) {
                //    return true;
                //}
                //else {
                //    return false;
                //}
                var message = "Are you sure? You haven\’t saved your changes. Click OK to leave or Cancel to go back and save your changes.", e = e || window.event;
                if (e) {
                    e.returnValue = message;
                }
                return message;
            } 
        }
    // });    
    $('#ContentPlaceHolder1_btnSave').click(function () {
        $("form").submit(function () {
            submitted = true;
        });
    });

    $('#ContentPlaceHolder1_btnSave1').click(function () {
        $("form").submit(function () {
            submitted = true;
        });
    });
  
});