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
//Poline
var isDirtyLIneNUm = false;
var isDirtytxtDItemCode = false;

var submitted = false;

$(document).ready(function () {
    //$('input:text,input:checkbox,input:radio,textarea,select').change(function () {

    //$('#ContentPlaceHolder1_txtCompanyName').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtDTotalTax', function (e) {
        if (this.value != '') {
            isDirtytxtDItemCode = true;
        }
        else {
            isDirtytxtDItemCode = false;
        }
    });
    $(document).on('change', '#ContentPlaceHolder1_txtDDTAXCODE', function (e) {
        if (this.value != '') {
            isDirtytxtDItemCode = true;
        }
        else {
            isDirtytxtDItemCode = false;
        }
    });
    $(document).on('change', '#ContentPlaceHolder1_txtDTP', function (e) {
        if (this.value != '') {
            isDirtytxtDItemCode = true;
        }
        else {
            isDirtytxtDItemCode = false;
        }
    });
    $(document).on('change', '#ContentPlaceHolder1_txtDUP', function (e) {
        if (this.value != '') {
            isDirtytxtDItemCode = true;
        }
        else {
            isDirtytxtDItemCode = false;
        }
    });
    $(document).on('change', '#ContentPlaceHolder1_txtDUOM', function (e) {
        if (this.value != '') {
            isDirtytxtDItemCode = true;
        }
        else {
            isDirtytxtDItemCode = false;
        }
    });
    $(document).on('change', '#ContentPlaceHolder1_txtDQty', function (e) {
        if (this.value != '') {
            isDirtytxtDItemCode = true;
        }
        else {
            isDirtytxtDItemCode = false;
        }
    }); 
    $(document).on('change', '#ContentPlaceHolder1_txtDRequestedBy', function (e) {
        if (this.value != '') {
            isDirtytxtDItemCode = true;
        }
        else {
            isDirtytxtDItemCode = false;
        }
    });
    $(document).on('change', '#ContentPlaceHolder1_txtDCatalogCode', function (e) {
        if (this.value != '') {
            isDirtytxtDItemCode = true;
        }
        else {
            isDirtytxtDItemCode = false;
        }
    });
    $(document).on('change', '#ContentPlaceHolder1_txtDBrand', function (e) {
        if (this.value != '') {
            isDirtytxtDItemCode = true;
        }
        else {
            isDirtytxtDItemCode = false;
        }
    });

    $(document).on('change', '#ContentPlaceHolder1_txtDModel', function (e) {
        if (this.value != '') {
            isDirtytxtDItemCode = true;
        }
        else {
            isDirtytxtDItemCode = false;
        }
    });

    $(document).on('change', '#ContentPlaceHolder1_txtDRemarks', function (e) {
        if (this.value != '') {
            isDirtytxtDItemCode = true;
        }
        else {
            isDirtytxtDItemCode = false;
        }
    });
    
    $(document).on('change', '#ContentPlaceHolder1_txtDCostCode', function (e) {
        if (this.value != '') {
            isDirtytxtDItemCode = true;
        }
        else {
            isDirtytxtDItemCode = false;
        }
    });
    $(document).on('change', '#ContentPlaceHolder1_txtDItemCode', function (e) {
        if (this.value != '') {
            isDirtytxtDItemCode = true;
        }
        else {
            isDirtytxtDItemCode = false;
        }
    });

    $(document).on('change', '#ContentPlaceHolder1_txtDPOLineNum', function (e) {
        if (this.value != '') {
            isDirtyLIneNUm = true;
        }
        else {
            isDirtyLIneNUm = false;
        }
    });

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
    $(document).on('change', '#ContentPlaceHolder1_txtTemplateName', function (e) {
        if (this.value != '') {
            isDirtycmp = true;
        }
        else {
            isDirtycmp = false;
        }
    });
        //$('#ContentPlaceHolder1_txtCompanyShortName').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtTemplateDescription', function (e) {
        if (this.value != '') {
            isDirtystcmp = true;
        }
        else {
            isDirtystcmp = false;
        }
    });
        //$('#ContentPlaceHolder1_ddlCountry').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtOrganization', function (e) {
        if (this.value != 'Select') {
            isDirtyselectCountry = true;
        }
        else {
            isDirtyselectCountry = false;
        }
    });
        //$('#ContentPlaceHolder1_ddlBusinessClassficiation').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtProjectCode', function (e) {
        if (this.value != 'Select') { 
            isDirtyselectBusiness = true;
        }
        else { 
            isDirtyselectBusiness = false;
        }
    }); 
    $(document).on('change', '#ContentPlaceHolder1_txtBuyers', function (e) {
        if (this.value != '') {
            isDirtyselectEmail = true;
        }
        else {
            isDirtyselectEmail = false;
        }
    });
    //$('#ContentPlaceHolder1_txtContactFirstName').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtPOType', function (e) {
        if (this.value != '') {
            isDirtyselectFirstName = true;
        }
        else {
            isDirtyselectFirstName = false;
        }
    });
    //$('#ContentPlaceHolder1_txtContactLastName').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtCompanyID', function (e) {
        if (this.value != '') {
            isDirtyselectLastName = true;
        }
        else {
            isDirtyselectLastName = false;
        }
    });
    //$('#ContentPlaceHolder1_txtPosition').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtCompanyName', function (e) {
        if (this.value != '') {
            isDirtyPostion = true;
        }
        else {
            isDirtyPostion = false;
        }
    });
    //$('#ContentPlaceHolder1_txtMobile').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtCompanyAddress', function (e) {
        if (this.value != '') {
            isDirtyMobile = true;
        }
        else {
            isDirtyMobile = false;
        }
    });
    //$('#ContentPlaceHolder1_txtPhone').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtContactPerson1Name', function (e) {
        if (this.value != '') {
            isDirtyPhone = true;
        }
        else {
            isDirtyPhone = false;
        }
    });
        //$('#ContentPlaceHolder1_txtExtension').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtContactPerson1Position', function (e) {
        if (this.value != '') {
            isDirtyExtention = true;
        }
        else {
            isDirtyExtention = false;
        }
    });
    //$('#ContentPlaceHolder1_txtIssuingAuthority').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtContactPerson1Mobile', function (e) {
        if (this.value != '') {
            isDirtyIssuingAuth = true;
        }
        else {
            isDirtyIssuingAuth = false;
        }
    });
    //$('#ContentPlaceHolder1_txtTradeLicenseNum').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtContactPerson1Phone', function (e) {
        if (this.value != '') {
            isDirtyTradelicense = true;
        }
        else {
            isDirtyTradelicense = false;
        }
    });
    //$('#ContentPlaceHolder1_txtExpireDate').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtContactPerson1Fax', function (e) {
        if (this.value != '') {
            isDirtyExpire = true;
        }
        else {
            isDirtyExpire = false;
        }
    });
    //$('#ContentPlaceHolder1_ddlAddressLine1Country').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtContactPerson2Name', function (e) {
        if (this.value != 'Select') {
            isDirtySelectCountry = true;
        }
        else {
            isDirtySelectCountry = false;
        }
    });
    //$('#ContentPlaceHolder1_txttabAddress1').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtContactPerson2Position', function (e) {
        if (this.value != '') {
            isDirtyAddressLine1 = true;
        }
        else {
            isDirtyAddressLine1 = false;
        }
    });
    //$('#ContentPlaceHolder1_txttabAddress2').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtContactPerson2Mobile', function (e) {
        if (this.value != '') {
            isDirtyAddressLine2 = true;
        }
        else {
            isDirtyAddressLine2 = false;
        }
    });
    //$('#ContentPlaceHolder1_txtAddress1City').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtContactPerson2Phone', function (e) {
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

        $(document).on('change', '#ContentPlaceHolder1_txtContactPerson2Fax', function (e) {
        if (this.value != '') {
            isDirtphone = true;
        }
        else {
            isDirtphone = false;
        }
    });
    //$('#ContentPlaceHolder1_txtAddress1FaxNum').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtShiptoAddress', function (e) {
        if (this.value != '') {
            isDirtFax = true;
        }
        else {
            isDirtFax = false;
        }
        });

    //Address 2

    //$('#ContentPlaceHolder1_ddlAddressLine1Country').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtPaymentTerms', function (e) {
            if (this.value != '') {
                isDirtAddress2Country = true;
            }
            else {
                isDirtAddress2Country = false;
            }
        });


    //$('#ContentPlaceHolder1_txttabAddress1').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtDeliverContact1Name', function (e) {
            if (this.value != '') {
                isDirtyAd2AddressLine1 = true;
            }
            else {
                isDirtyAd2AddressLine1 = false;
            }
        });
    //$('#ContentPlaceHolder1_txttabAddress2').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtDeliverContact1Position', function (e) {
            if (this.value != '') {
                isDirtyAd2AddressLine2 = true;
            }
            else {
                isDirtyAd2AddressLine2 = false;
            }
        });
    //$('#ContentPlaceHolder1_txtAddress1City').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtDeliverContact1Mobile', function (e) {
            if (this.value != '') {
                isDirtyAd2City = true;
            }
            else {
                isDirtyAd2City = false;
            }
        });
    //$('#ContentPlaceHolder1_txtAddressPostalCode').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtDeliverContact1Phone', function (e) {
            if (this.value != '') {
                isDirtyAd2PostalCode = true;
            }
            else {
                isDirtyAd2PostalCode = false;
            }
        });
    //$('#ContentPlaceHolder1_txtAddress1Phone').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtDeliverContact2Name', function (e) {
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


        $(document).on('change', '#ContentPlaceHolder1_txtDeliverContact2Position', function (e) {
            if (this.value != '') {
                isDirtyddlPaymentMethod = true;
            }
            else {
                isDirtyddlPaymentMethod = false;
            }
        });

        $(document).on('change', '#ContentPlaceHolder1_txtDeliverContact2Mobile', function (e) {
            if (this.value != '') {
                isDirtyddlBankCountry = true;
            }
            else {
                isDirtyddlBankCountry = false;
            }
        });
        
        $(document).on('change', '#ContentPlaceHolder1_txtDeliverContact2Phone', function (e) {
            if (this.value != '') {
                isDirtyBankName = true;
            }
            else {
                isDirtyBankName = false;
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
                || (isDirtyBankName) || (isDirtyddlBankCountry) || (isDirtyLIneNUm) || (isDirtytxtDItemCode)) && !submitted) {
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