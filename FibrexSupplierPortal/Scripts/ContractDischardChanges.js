var isDirtySelect = false;
var isDirtycmp = false;
var isDirtystcmp = false;
var isDirtyselectCountry = false;
var isDirtyselectBusiness = false;
var isDirtyselectBuyers = false;
var isDirtyselectPoType = false;
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
var IsDirtyRequistionRef = false;
var IsDirtyQuotationRef = false;
var IsDirtyContractRef = false;
var IsDirtyOriginalPO = false;
var IsDirtyOrderDate = false;
var IsDirtyRequiredDate = false;
var IsDirtyVendorDate = false;
var IsDirtyQuotationDate = false;
var IsDirtyContact1PersonEmail = false;
var IsDirtyContact2PersonEmail = false;
var IsDirtyLessDescription = false;
var IsDirtyLessAmount = false;
var IsDirtyAdditionalDescription = false;
var IsDirtyAdditionalAmount = false;
var IsDirtyFileDelete = false;
var IsPoUpdate = false;
var IsPoAtttachment = false;
var IsPoDelete = false;
var submitted = false;

$(document).ready(function () {
    //$('input:text,input:checkbox,input:radio,textarea,select').change(function () {

    //$(document).on('change', 'select', function (e) {
    //    //$('input:checkbox').change(function () {
    //    if (this.checked) {
    //        isDirtySelect = true;
    //    }
    //    else {
    //        isDirtySelect = false;
    //    }
    //});

    //$//('#ContentPlaceHolder1_txtCompanyName').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtContractType', function (e) {
            if (this.value != '') {
                isDirtycmp = true;
            }
            else {
                isDirtycmp = false;
            }
        });
        //$('#ContentPlaceHolder1_txtCompanyShortName').change(function () {
    $(document).on('change', '#ContentPlaceHolder1_txtOriginalContract', function (e) {
            if (this.value != '') {
                isDirtystcmp = true;
            }
            else {
                isDirtystcmp = false;
            }
        });
    $(document).on('change', '#ContentPlaceHolder1_txtContractStartDate', function (e) {
            if (this.value != '') {
                isDirtystcmp = true;
            }
            else {
                isDirtystcmp = false;
            }
        });
    //
        //$('#ContentPlaceHolder1_ddlCountry').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtOrganization', function (e) {
            if (this.value != '') {
                isDirtyselectCountry = true;
            }
            else {
                isDirtyselectCountry = false;
            }
        });
        //$('#ContentPlaceHolder1_ddlBusinessClassficiation').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtProjectCode', function (e) {
            if (this.value != '') { 
                isDirtyselectBusiness = true;
            }
            else { 
                isDirtyselectBusiness = false;
            }
        }); 
        $(document).on('change', '#ContentPlaceHolder1_txtBuyers', function (e) {
            if (this.value != '') {
                isDirtyselectBuyers = true;
            }
            else {
                isDirtyselectBuyers = false;
            }
        });
        //$('#ContentPlaceHolder1_txtContactFirstName').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtTotalAmount', function (e) {
            if (this.value != '') {
                isDirtyselectPoType = true;
            }
            else {
                isDirtyselectPoType = false;
            }
        });
        $(document).on('change', '#ContentPlaceHolder1_txtContractSubject', function (e) {
            if (this.value != '') {
                IsDirtyRequistionRef = true;
            }
            else {
                IsDirtyRequistionRef = false;
            }
        });
        $(document).on('change', '#ContentPlaceHolder1_txtQuotationRef', function (e) {
            if (this.value != '') {
                IsDirtyQuotationRef = true;
            }
            else {
                IsDirtyQuotationRef = false;
            }
        });

        $(document).on('change', '#ContentPlaceHolder1_txtContactPerson1Mobile', function (e) {
            if (this.value != '') {
                IsDirtyContractRef = true;
            }
            else {
                IsDirtyContractRef = false;
            }
        });
        $(document).on('change', '#ContentPlaceHolder1_txtOrderDate', function (e) {
            if (this.value != '') {
                IsDirtyOrderDate = true;
            }
            else {
                IsDirtyOrderDate = true;
            }
        });
        $(document).on('change', '#ContentPlaceHolder1_txtRequiredDate', function (e) {
            if (this.value != '') {
                IsDirtyRequiredDate = true;
            }
            else {
            IsDirtyRequiredDate = true;
             }
        });
        $(document).on('change', '#ContentPlaceHolder1_txtVendorDate', function (e) {
            if (this.value != '') {
                IsDirtyVendorDate = true;
            }
            else {
            IsDirtyVendorDate = true;
            
             }
        });
        $(document).on('change', '#ContentPlaceHolder1_txtQuotationDate', function (e) {
            if (this.value != '') {
                IsDirtyQuotationDate = true;
            }
            else {
            IsDirtyQuotationDate = true;
             }
        });
    //
        $(document).on('change', '#ContentPlaceHolder1_txtDeliveryDate', function (e) {
            if (this.value != '') {
                IsDirtyOriginalPO = true;
            }
            else {
                IsDirtyOriginalPO = false;
            }
        });
    //
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
        $(document).on('change', '#ContentPlaceHolder1_txtContactPerson1Email', function (e) {
            if (this.value != '') {
                IsDirtyContact1PersonEmail = true;
            }
            else {
                IsDirtyContact1PersonEmail = false;
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
        $(document).on('change', '#ContentPlaceHolder1_txtDelayedDamages', function (e) {
            if (this.value != '') {
                isDirtyAd2AddressLine1 = true;
            }
            else {
                isDirtyAd2AddressLine1 = false;
            }
        });
        //$('#ContentPlaceHolder1_txttabAddress2').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtPerformanceGaurantee', function (e) {
            if (this.value != '') {
                isDirtyAd2AddressLine2 = true;
            }
            else {
                isDirtyAd2AddressLine2 = false;
            }
        });
        //$('#ContentPlaceHolder1_txtAddress1City').change(function () {
        $(document).on('change', '#ContentPlaceHolder1_txtDefectDisabilityPeriods', function (e) {
            if (this.value != '') {
                isDirtyAd2City = true;
            }
            else {
                isDirtyAd2City = false;
            }
        }); 
        $(document).on('change', '#ContentPlaceHolder1_txtPopupFileTitle', function (e) {
            if (this.value != '') {
                IsPoUpdate = true;
            }
            else {
                IsPoUpdate = false;
            }
        });
        $(document).on('change', '#ContentPlaceHolder1_txtPopupFileDescription', function (e) {
            if (this.value != '') {
                IsPoAtttachment = true;
            }
            else {
                IsPoAtttachment = false;
            }
        });
        $(document).on('change', '#txtPopupFileTitle', function (e) {
            if (this.value != '') {
                IsPoUpdate = true;
            }
            else {
                IsPoUpdate = false;
            }
        });
        $(document).on('change', '#txtPopupFileDescription', function (e) {
            if (this.value != '') {
                IsPoAtttachment = true;
            }
            else {
                IsPoAtttachment = false;
            }
        });

        $(document).on('change', '#ContentPlaceHolder1_txtLessDescription', function (e) {
            if (this.value != '') {
                IsDirtyLessDescription = true;
            }
            else {
                IsDirtyLessDescription = false;
            }
        });
        $(document).on('change', '#ContentPlaceHolder1_txtLessAmount', function (e) {
            if (this.value != '') {
                IsDirtyLessAmount = true;
            }
            else {
                IsDirtyLessAmount = false;
            }
        });
        $(document).on('change', '#ContentPlaceHolder1_txtAdditionalChargesDescription', function (e) {
            if (this.value != '') {
                IsDirtyAdditionalDescription = true;
            }
            else {
                IsDirtyAdditionalDescription = false;
            }
        });
        $(document).on('change', '#ContentPlaceHolder1_txtAdditionalChargesAmount', function (e) {
            if (this.value != '') {
                IsDirtyAdditionalAmount = true;
            }
            else {
                IsDirtyAdditionalAmount = false;
            }
        });
    //txtPopupFileDescription

        window.onbeforeunload = function (e) {
            if (((isDirtycmp) || (isDirtySelect) || (isDirtyselectCountry) || (isDirtystcmp) || (isDirtyselectBusiness)
                || (isDirtyselectBuyers) || (isDirtyselectPoType) || (IsDirtyRequistionRef) || (IsDirtyQuotationRef) || (IsDirtyContractRef) || (IsDirtyOriginalPO)
                || (IsDirtyOrderDate) || (IsDirtyRequiredDate) || (IsDirtyVendorDate) || (IsDirtyQuotationDate) || (isDirtyselectLastName) || (isDirtyPostion) || (isDirtyMobile) || (isDirtyPhone) || (isDirtyExtention) || (isDirtyTradelicense)
                || (isDirtyExpire)|| (isDirtySelectCountry) || (isDirtyAddressLine1) || (isDirtyAddressLine2) || (isDirtyCity) || (isDirtpostalCode)
                || (isDirtphone) || (isDirtFax) || (isDirtAddress2Country) || (isDirtyAd2AddressLine1) || (isDirtyAd2AddressLine2)
                || (isDirtyAd2City) || (isDirtyAd2PostalCode) || (isDirtyAd2Phone) || (isDirtyAd2Fax) || (isDirtyddlPaymentMethod)
                || (isDirtyBankName) || (isDirtyddlBankCountry) || (IsDirtyContact1PersonEmail) || (IsDirtyContact2PersonEmail) || (IsDirtyLessDescription)
                || (IsDirtyLessAmount) || (IsDirtyAdditionalDescription) || (IsDirtyAdditionalAmount) || (IsPoAtttachment) || (IsDirtyFileDelete) || (IsPoUpdate) || (IsPoDelete)) && !submitted) {
                // if (isDirty) {
                //var confirmExit = confirm('Are you sure? You haven\’t saved your changes. Click OK to leave or Cancel to go back and save your changes.');
                //if (confirmExit) {IsDirtyContractRef
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
    //}
});