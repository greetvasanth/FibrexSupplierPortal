
$(document).ready(function () {
    $("#ContentPlaceHolder1_txtTemplateName").change(function () {
        $('#ContentPlaceHolder1_txtTemplateName').removeClass('boxshow');
        //$('#txtCompanyName').css('border-color','green');
    });
    //$("#ContentPlaceHolder1_txtTemplateDescription").change(function () {
    //    $('#ContentPlaceHolder1_txtTemplateDescription').removeClass('boxshow');
    //    //$('#txtCompanyName').css('border-color','green');
    //});
    $("#ContentPlaceHolder1_txtOrganization").change(function () {
        $('#ContentPlaceHolder1_txtOrganization').removeClass('boxshow');
        //$('#txtCompanyName').css('border-color','green');
    });
    $("#ContentPlaceHolder1_txtProjectCode").change(function () {
        $('#ContentPlaceHolder1_txtProjectCode').removeClass('boxshow');
        //$('#txtCompanyName').css('border-color','green');
    });
    //$("#ContentPlaceHolder1_txtBuyers").change(function () {
    //    $('#ContentPlaceHolder1_txtBuyers').removeClass('boxshow');
    //    //$('#ddlCountry').css('border-color', 'green');
    //});

    //$("#ContentPlaceHolder1_txtPOType").change(function () {
    //    $('#ContentPlaceHolder1_txtPOType').removeClass('boxshow');
    //    //$('#ddlBusinessClassficiation').css('border-color', 'green');
    //});
     
    //$("#ContentPlaceHolder1_txtOrderDate").change(function () {
    //    $('#ContentPlaceHolder1_txtOrderDate').removeClass('boxshow');
    //    //$('#txtPhone').css('border-color', 'green');
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
    //$("#ContentPlaceHolder1_txtCompanyName").change(function () {
    //    $('#ContentPlaceHolder1_txtCompanyName').removeClass('boxshow');
    //    //$('#txttabAddress1').css('border-color', 'green');
    //});
    //$("#ContentPlaceHolder1_txtCompanyID").change(function () {
    //    $('#ContentPlaceHolder1_txtCompanyID').removeClass('boxshow');
    //    //$('#txttabAddress1').css('border-color', 'green');
    //});
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

        //if ($('#ContentPlaceHolder1_txtTemplateDescription').val() == '') {
        //    $('#ContentPlaceHolder1_txtTemplateDescription').addClass('boxshow');
        //    //IsValid alert("Company name can't be blank");           
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtTemplateDescription').removeClass('boxshow');
        //}

        if ($('#ContentPlaceHolder1_txtTemplateName').val() == '') {
            $('#ContentPlaceHolder1_txtTemplateName').addClass('boxshow');
            //IsValid alert("Company name can't be blank");           
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtTemplateName').removeClass('boxshow');
        }

        if ($('#ContentPlaceHolder1_txtProjectCode').val() == '') {
            $('#ContentPlaceHolder1_txtProjectCode').addClass('boxshow');
            //IsValid alert("Company name can't be blank");           
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtProjectCode').removeClass('boxshow');
        }

        //if ($('#ContentPlaceHolder1_txtBuyers').val() == '') {
        //    $('#ContentPlaceHolder1_txtBuyers').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtBuyers').removeClass('boxshow');
        //}

        //if ($('#ContentPlaceHolder1_txtPOType').val() == '') {
        //    $('#ContentPlaceHolder1_txtPOType').addClass('boxshow');
        //    // alert("Please Select Business Classification from the list"); 
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtPOType').removeClass('boxshow');
        //    //__doPostBack("#ContentPlaceHolder1_txtPOType", "")
        //} 
        //txttabAddress1
        //if ($('#ContentPlaceHolder1_txtCompanyName').val() == '') {
        //    $('#ContentPlaceHolder1_txtCompanyName').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtCompanyName').removeClass('boxshow');
        //}

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
