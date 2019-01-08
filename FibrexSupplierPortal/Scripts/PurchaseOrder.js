
$(document).ready(function () {
    $("#ContentPlaceHolder1_txtOrganization").change(function () {
        $('#ContentPlaceHolder1_txtOrganization').removeClass('boxshow');
        //$('#txtCompanyName').css('border-color','green');
    });
    $("#ContentPlaceHolder1_txtProjectCode").change(function () {
        $('#ContentPlaceHolder1_txtProjectCode').removeClass('boxshow');
        //$('#txtCompanyName').css('border-color','green');
    });
    $("#ContentPlaceHolder1_txtBuyers").change(function () {
        $('#ContentPlaceHolder1_txtBuyers').removeClass('boxshow');
        //$('#ddlCountry').css('border-color', 'green');
    });

    $("#ContentPlaceHolder1_txtPOType").change(function () {
        $('#ContentPlaceHolder1_txtPOType').removeClass('boxshow');
        //$('#ddlBusinessClassficiation').css('border-color', 'green');
    });

    //$("#ContentPlaceHolder1_txtRequistionRefNum").change(function () {
    //    $('#ContentPlaceHolder1_txtRequistionRefNum').removeClass('boxshow');
    //    //$('#txtOfficalEmail').css('border-color', 'green');
    //});
    //$("#ContentPlaceHolder1_txtQuotationRef").change(function () {
    //    $('#ContentPlaceHolder1_txtQuotationRef').removeClass('boxshow');
    //    //$('#txtContactFirstName').css('border-color', 'green');
    //});

    //$("#ContentPlaceHolder1_txtContractRef").change(function () {
    //    $('#ContentPlaceHolder1_txtContractRef').removeClass('boxshow');
    //    //$('#txtContactFirstName').css('border-color', 'green');
    //});

    //$("#ContentPlaceHolder1_txtOriginalPO").change(function () {
    //    $('#ContentPlaceHolder1_txtOriginalPO').removeClass('boxshow');
    //    //$('#txtPosition').css('border-color', 'green');
    //});
    $("#ContentPlaceHolder1_txtOrderDate").change(function () {
        $('#ContentPlaceHolder1_txtOrderDate').removeClass('boxshow');
        //$('#txtPhone').css('border-color', 'green');
    });

    //$("#ContentPlaceHolder1_txtRequiredDate").change(function () {
    //    $('#ContentPlaceHolder1_txtRequiredDate').removeClass('boxshow'); 
    //});
     

    //$("#ContentPlaceHolder1_txtVendorDate").change(function () {
    //    $('#ContentPlaceHolder1_txtVendorDate').removeClass('boxshow');
    //    //$('#txtLineAddress1').css('border-color', 'green');
    //});

    //$("#ContentPlaceHolder1_txtQuotationDate").change(function () {
    //    $('#ContentPlaceHolder1_txtQuotationDate').removeClass('boxshow');
    //    //$('#ddlAddressLine1Country').css('border-color', 'green');
    //});

    //ContentPlaceHolder1_txtCompanyID
    //$("#ContentPlaceHolder1_txtCompanyName").change(function () {
    //    $('#ContentPlaceHolder1_txtCompanyName').removeClass('boxshow');
    //    //$('#txttabAddress1').css('border-color', 'green');
    //});

    //$("#ContentPlaceHolder1_txtCompanyID").change(function () {
    //    $('#ContentPlaceHolder1_txtCompanyID').removeClass('boxshow');
    //    //$('#txttabAddress1').css('border-color', 'green');
    //});

    //$("#ContentPlaceHolder1_txtContactPerson1Name").change(function () {
    //    $('#ContentPlaceHolder1_txtContactPerson1Name').removeClass('boxshow'); 
    //});

    //$("#ContentPlaceHolder1_txtContactPerson1Position").change(function () {
    //    $('#ContentPlaceHolder1_txtContactPerson1Position').removeClass('boxshow');
    //});
    //$("#ContentPlaceHolder1_txtContactPerson1Mobile").change(function () {
    //    $('#ContentPlaceHolder1_txtContactPerson1Mobile').removeClass('boxshow');
    //}); 
    //$("#ContentPlaceHolder1_txtContactPerson1Phone").change(function () {
    //    $('#ContentPlaceHolder1_txtContactPerson1Phone').removeClass('boxshow');
    //});
    //$("#ContentPlaceHolder1_txtContactPerson1Fax").change(function () {
    //    $('#ContentPlaceHolder1_txtContactPerson1Fax').removeClass('boxshow');
    //});
    //$("#ContentPlaceHolder1_txtContactPerson1Email").change(function () {
    //    $('#ContentPlaceHolder1_txtContactPerson1Email').removeClass('boxshow');
    //});
    //$("#ContentPlaceHolder1_txtShiptoAddress").change(function () {
    //    $('#ContentPlaceHolder1_txtShiptoAddress').removeClass('boxshow');
    //});
    //$("#ContentPlaceHolder1_txtPaymentTerms").change(function () {
    //    $('#ContentPlaceHolder1_txtPaymentTerms').removeClass('boxshow');
    //});
    //$("#ContentPlaceHolder1_txtDeliverContact1Name").change(function () {
    //    $('#ContentPlaceHolder1_txtDeliverContact1Name').removeClass('boxshow');
    //});
    //$("#ContentPlaceHolder1_txtDeliverContact1Position").change(function () {
    //    $('#ContentPlaceHolder1_txtDeliverContact1Position').removeClass('boxshow');
    //});
    //$("#ContentPlaceHolder1_txtDeliverContact1Mobile").change(function () {
    //    $('#ContentPlaceHolder1_txtDeliverContact1Mobile').removeClass('boxshow');
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
            $('#txtOrganization').removeClass('boxshow');
        }

        if ($('#ContentPlaceHolder1_txtProjectCode').val() == '') {
            $('#ContentPlaceHolder1_txtProjectCode').addClass('boxshow');
            //IsValid alert("Company name can't be blank");           
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtProjectCode').removeClass('boxshow');
        }

        if ($('#ContentPlaceHolder1_txtBuyers').val() == '') {
            $('#ContentPlaceHolder1_txtBuyers').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtBuyers').removeClass('boxshow');
        }

        if ($('#ContentPlaceHolder1_txtPOType').val() == '') {
            $('#ContentPlaceHolder1_txtPOType').addClass('boxshow');
            // alert("Please Select Business Classification from the list"); 
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtPOType').removeClass('boxshow');
            //__doPostBack("#ContentPlaceHolder1_txtPOType", "")
        }
        //ShortName
        //if ($('#ContentPlaceHolder1_txtRequistionRefNum').val() == '') {
        //    $('#ContentPlaceHolder1_txtRequistionRefNum').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtRequistionRefNum').removeClass('boxshow');
        //}
        ////txtContactFirstName
        //if ($('#ContentPlaceHolder1_txtQuotationRef').val() == '') {
        //    $('#ContentPlaceHolder1_txtQuotationRef').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtQuotationRef').removeClass('boxshow');
        //}

        //if ($('#ContentPlaceHolder1_txtContractRef').val() == '') {
        //    $('#ContentPlaceHolder1_txtContractRef').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtContractRef').removeClass('boxshow');
        //}
        ////txtContactFirstName
        //if ($('#ContentPlaceHolder1_txtOriginalPO').val() == '') {
        //    $('#ContentPlaceHolder1_txtOriginalPO').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtOriginalPO').removeClass('boxshow');
        //}

        //txtPhone
        if ($('#ContentPlaceHolder1_txtOrderDate').val() == '') {
            $('#ContentPlaceHolder1_txtOrderDate').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtOrderDate').removeClass('boxshow');
        }

        //if ($('#ContentPlaceHolder1_txtRequiredDate').val() == '') {
        //    $('#ContentPlaceHolder1_txtRequiredDate').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtRequiredDate').removeClass('boxshow');
        //} 
        //if ($('#ContentPlaceHolder1_txtVendorDate').val() == '') {
        //    $('#ContentPlaceHolder1_txtVendorDate').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtVendorDate').removeClass('boxshow');
        //}
        //ddlAddressLine1Country
        //if ($('#ContentPlaceHolder1_txtQuotationDate').val() == 'Select') {
        //    $('#ContentPlaceHolder1_txtQuotationDate').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtQuotationDate').removeClass('boxshow');
        //}

        //txttabAddress1
        //if ($('#ContentPlaceHolder1_txtCompanyName').val() == '') {
        //    $('#ContentPlaceHolder1_txtCompanyName').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtCompanyName').removeClass('boxshow');
        //}

        ////ContentPlaceHolder1_txtCompanyID
        //if ($('#ContentPlaceHolder1_txtCompanyID').val() == '') {
        //    $('#ContentPlaceHolder1_txtCompanyID').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtCompanyID').removeClass('boxshow');
        //}

        //if ($('#ContentPlaceHolder1_txtContactPerson1Name').val() == '') {
        //    $('#ContentPlaceHolder1_txtContactPerson1Name').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtContactPerson1Name').removeClass('boxshow');
        //}

        //if ($('#ContentPlaceHolder1_txtContactPerson1Position').val() == '') {
        //    $('#ContentPlaceHolder1_txtContactPerson1Position').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtContactPerson1Position').removeClass('boxshow');
        //}
        //if ($('#ContentPlaceHolder1_txtContactPerson1Mobile').val() == '') {
        //    $('#ContentPlaceHolder1_txtContactPerson1Mobile').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtContactPerson1Mobile').removeClass('boxshow');
        //}
        //if ($('#ContentPlaceHolder1_txtContactPerson1Phone').val() == '') {
        //    $('#ContentPlaceHolder1_txtContactPerson1Phone').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtContactPerson1Phone').removeClass('boxshow');
        //}
        //if ($('#ContentPlaceHolder1_txtContactPerson1Fax').val() == '') {
        //    $('#ContentPlaceHolder1_txtContactPerson1Fax').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtContactPerson1Fax').removeClass('boxshow');
        //}
        //if ($('#ContentPlaceHolder1_txtContactPerson1Email').val() == '') {
        //    $('#ContentPlaceHolder1_txtContactPerson1Email').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtContactPerson1Email').removeClass('boxshow');
        //}

        //if ($('#ContentPlaceHolder1_txtShiptoAddress').val() == '') {
        //    $('#ContentPlaceHolder1_txtShiptoAddress').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtShiptoAddress').removeClass('boxshow');
        //}
        //if ($('#ContentPlaceHolder1_txtPaymentTerms').val() == '') {
        //    $('#ContentPlaceHolder1_txtPaymentTerms').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtPaymentTerms').removeClass('boxshow');
        //}

        //if ($('#ContentPlaceHolder1_txtDeliverContact1Name').val() == '') {
        //    $('#ContentPlaceHolder1_txtDeliverContact1Name').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtDeliverContact1Name').removeClass('boxshow');
        //}
        //if ($('#ContentPlaceHolder1_txtDeliverContact1Position').val() == '') {
        //    $('#ContentPlaceHolder1_txtDeliverContact1Position').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtDeliverContact1Position').removeClass('boxshow');
        //}
        //if ($('#ContentPlaceHolder1_txtDeliverContact1Mobile').val() == '') {
        //    $('#ContentPlaceHolder1_txtDeliverContact1Mobile').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtDeliverContact1Mobile').removeClass('boxshow');
        //} 

        if (IsValid == false) {
            alert("Mandatory fields are missing.");
            return false;
        }
    })
});
