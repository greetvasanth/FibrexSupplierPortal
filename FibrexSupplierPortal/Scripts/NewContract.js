
$(document).ready(function () {
    $("#ContentPlaceHolder1_txtOrganization").change(function () {
        $('#ContentPlaceHolder1_txtOrganization').removeClass('boxshow');
        //$('#txtCompanyName').css('border-color','green');
    });
    $("#ContentPlaceHolder1_txtContractSubject").change(function () {
        $('#ContentPlaceHolder1_txtContractSubject').removeClass('boxshow');
        //$('#txtCompanyName').css('border-color','green');
    });
    $("#ContentPlaceHolder1_txtBuyers").change(function () {
        $('#ContentPlaceHolder1_txtBuyers').removeClass('boxshow');
        //$('#ddlCountry').css('border-color', 'green');
    });

    $("#ContentPlaceHolder1_txtContractType").change(function () {
        $('#ContentPlaceHolder1_txtContractType').removeClass('boxshow'); 
    });
    //
    $("#ContentPlaceHolder1_txtPOCurrency").change(function () {
        $('#ContentPlaceHolder1_txtPOCurrency').removeClass('boxshow');
    });

    $("#ContentPlaceHolder1_txtTotalAmount").change(function () {
        $('#ContentPlaceHolder1_txtTotalAmount').removeClass('boxshow');
    });
    $("#ContentPlaceHolder1_txtOriginalContract").change(function () {
        $('#ContentPlaceHolder1_txtOriginalContract').removeClass('boxshow');
        //$('#txtOfficalEmail').css('border-color', 'green');
    });
    $("#ContentPlaceHolder1_txtContractStartDate").change(function () {
        $('#ContentPlaceHolder1_txtContractStartDate').removeClass('boxshow');
        //$('#txtContactFirstName').css('border-color', 'green');
    });
    //$("#ContentPlaceHolder1_txtQuotationRef").change(function () {
    //    $('#ContentPlaceHolder1_txtQuotationRef').removeClass('boxshow');
    //    //$('#txtContactFirstName').css('border-color', 'green');
    //});
    //$("#ContentPlaceHolder1_txtQuotationRef").change(function () {
    //    $('#ContentPlaceHolder1_txtQuotationRef').removeClass('boxshow');
    //    //$('#txtContactFirstName').css('border-color', 'green');
    //});
    $("#ContentPlaceHolder1_txtQuotationDate").change(function () {
        $('#ContentPlaceHolder1_txtQuotationDate').removeClass('boxshow');
        //$('#txtPhone').css('border-color', 'green');
    });

    $("#ContentPlaceHolder1_txtRequiredDate").change(function () {
        $('#ContentPlaceHolder1_txtRequiredDate').removeClass('boxshow'); 
    });
     
     
    //ContentPlaceHolder1_txtCompanyID
    $("#ContentPlaceHolder1_txtCompanyName").change(function () {
        $('#ContentPlaceHolder1_txtCompanyName').removeClass('boxshow');
        //$('#txttabAddress1').css('border-color', 'green');
    });

    $("#ContentPlaceHolder1_txtCompanyID").change(function () {
        $('#ContentPlaceHolder1_txtCompanyID').removeClass('boxshow');
        //$('#txttabAddress1').css('border-color', 'green');
    });
    $("#ContentPlaceHolder1_txtContactPerson1Name").change(function () {
        $('#ContentPlaceHolder1_txtContactPerson1Name').removeClass('boxshow'); 
    });

    $("#ContentPlaceHolder1_txtContactPerson1Position").change(function () {
        $('#ContentPlaceHolder1_txtContactPerson1Position').removeClass('boxshow');
    });
    $("#ContentPlaceHolder1_txtContactPerson1Phone").change(function () {
        $('#ContentPlaceHolder1_txtContactPerson1Phone').removeClass('boxshow');
    });
    $("#ContentPlaceHolder1_txtDeliveryDate").change(function () {
        $('#ContentPlaceHolder1_txtDeliveryDate').removeClass('boxshow');
    });
    
    $("#ContentPlaceHolder1_txtPaymentTerms").change(function () {
        $('#ContentPlaceHolder1_txtPaymentTerms').removeClass('boxshow');
    });
    //$("#ContentPlaceHolder1_txtDelayedDamages").change(function () {
    //    $('#ContentPlaceHolder1_txtDelayedDamages').removeClass('boxshow');
    //});
    //$("#ContentPlaceHolder1_txtPerformanceGaurantee").change(function () {
    //    $('#ContentPlaceHolder1_txtPerformanceGaurantee').removeClass('boxshow');
    //});
    //$("#ContentPlaceHolder1_txtDefectDisabilityPeriods").change(function () {
    //    $('#ContentPlaceHolder1_txtDefectDisabilityPeriods').removeClass('boxshow');
    //});

    // ContentPlaceHolder1_txtContactPerson1Email
    $("#ContentPlaceHolder1_btnSave").click(function (e) {
        var IsValid = true;
        if ($('#ContentPlaceHolder1_txtOrganization').val() == '') {
            $('#ContentPlaceHolder1_txtOrganization').addClass('boxshow');
            //IsValid alert("Company name can't be blank");           
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtOrganization').removeClass('boxshow');
        }

        if ($('#ContentPlaceHolder1_txtContractSubject').val() == '') {
            $('#ContentPlaceHolder1_txtContractSubject').addClass('boxshow');
            //IsValid alert("Company name can't be blank");           
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtContractSubject').removeClass('boxshow');
        }

        if ($('#ContentPlaceHolder1_txtBuyers').val() == '') {
            $('#ContentPlaceHolder1_txtBuyers').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtBuyers').removeClass('boxshow');
        }

        if ($('#ContentPlaceHolder1_txtContractType').val() == '') {
            $('#ContentPlaceHolder1_txtContractType').addClass('boxshow');
            // alert("Please Select Business Classification from the list"); 
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtContractType').removeClass('boxshow');
            //__doPostBack("#ContentPlaceHolder1_txtContractType", "")
        }

        if ($('#ContentPlaceHolder1_txtPOCurrency').val() == '') {
            $('#ContentPlaceHolder1_txtPOCurrency').addClass('boxshow');
            // alert("Please Select Business Classification from the list"); 
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtPOCurrency').removeClass('boxshow');
            //__doPostBack("#ContentPlaceHolder1_txtContractType", "")
        }


        if ($('#ContentPlaceHolder1_txtTotalAmount').val() == '0.00') {
            $('#ContentPlaceHolder1_txtTotalAmount').addClass('boxshow');
            // alert("Please Select Business Classification from the list"); 
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtTotalAmount').removeClass('boxshow');
            //__doPostBack("#ContentPlaceHolder1_txtContractType", "")
        }

        if ($('#ContentPlaceHolder1_txtOriginalContract').val() == '') {
            $('#ContentPlaceHolder1_txtOriginalContract').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtOriginalContract').removeClass('boxshow');
        }
        ////txtContactFirstName
        if ($('#ContentPlaceHolder1_txtContractStartDate').val() == '') {
            $('#ContentPlaceHolder1_txtContractStartDate').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtContractStartDate').removeClass('boxshow');
        }

        //if ($('#ContentPlaceHolder1_txtQuotationRef').val() == '') {
        //    $('#ContentPlaceHolder1_txtQuotationRef').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtQuotationRef').removeClass('boxshow');
        //}

        if ($('#ContentPlaceHolder1_txtQuotationDate').val() == '') {
            $('#ContentPlaceHolder1_txtQuotationDate').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtQuotationDate').removeClass('boxshow');
        }
        //txtPhone
        if ($('#ContentPlaceHolder1_txtOrderDate').val() == '') {
            $('#ContentPlaceHolder1_txtOrderDate').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtOrderDate').removeClass('boxshow');
        }

        if ($('#ContentPlaceHolder1_txtRequiredDate').val() == '') {
            $('#ContentPlaceHolder1_txtRequiredDate').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtRequiredDate').removeClass('boxshow');
        }  
        //txttabAddress1
        if ($('#ContentPlaceHolder1_txtCompanyName').val() == '') {
            $('#ContentPlaceHolder1_txtCompanyName').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtCompanyName').removeClass('boxshow');
        }

        //ContentPlaceHolder1_txtCompanyID
        if ($('#ContentPlaceHolder1_txtCompanyID').val() == '') {
            $('#ContentPlaceHolder1_txtCompanyID').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtCompanyID').removeClass('boxshow');
        }

        if ($('#ContentPlaceHolder1_txtContactPerson1Name').val() == '') {
            $('#ContentPlaceHolder1_txtContactPerson1Name').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtContactPerson1Name').removeClass('boxshow');
        }

        if ($('#ContentPlaceHolder1_txtContactPerson1Position').val() == '') {
            $('#ContentPlaceHolder1_txtContactPerson1Position').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtContactPerson1Position').removeClass('boxshow');
        }
        if ($('#ContentPlaceHolder1_txtContactPerson1Phone').val() == '') {
            $('#ContentPlaceHolder1_txtContactPerson1Phone').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtContactPerson1Phone').removeClass('boxshow');
        }
        if ($('#ContentPlaceHolder1_txtPaymentTerms').val() == '') {
            $('#ContentPlaceHolder1_txtPaymentTerms').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtPaymentTerms').removeClass('boxshow');
        }

        if ($('#ContentPlaceHolder1_txtDeliveryDate').val() == '') {
            $('#ContentPlaceHolder1_txtDeliveryDate').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtDeliveryDate').removeClass('boxshow');
        }

        //if ($('#ContentPlaceHolder1_txtDelayedDamages').val() == '') {
        //    $('#ContentPlaceHolder1_txtDelayedDamages').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtDelayedDamages').removeClass('boxshow');
        //}
        //if ($('#ContentPlaceHolder1_txtPerformanceGaurantee').val() == '') {
        //    $('#ContentPlaceHolder1_txtPerformanceGaurantee').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtPerformanceGaurantee').removeClass('boxshow');
        //}
        //if ($('#ContentPlaceHolder1_txtDefectDisabilityPeriods').val() == '') {
        //    $('#ContentPlaceHolder1_txtDefectDisabilityPeriods').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtDefectDisabilityPeriods').removeClass('boxshow');
        //}


        if (IsValid == false) {
            alert("Mandatory fields are missing.");
            return false;
        }
    })
});
