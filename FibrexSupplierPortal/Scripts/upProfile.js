
$(document).ready(function () {
    $("#ContentPlaceHolder1_txtCompanyName").change(function () {
        $('#ContentPlaceHolder1_txtCompanyName').removeClass('boxshow');
        //$('#txtCompanyName').css('border-color','green');
    });
    $("#ContentPlaceHolder1_txtCompanyOwnerName").change(function () {
        $('#ContentPlaceHolder1_txtCompanyOwnerName').removeClass('boxshow');
        //$('#txtCompanyName').css('border-color','green');
    });
    $("#ContentPlaceHolder1_ddlCountry").change(function () {
        $('#ContentPlaceHolder1_ddlCountry').removeClass('boxshow');
        //$('#ddlCountry').css('border-color', 'green');
    });

    $("#ContentPlaceHolder1_ddlBusinessClassficiation").change(function () {
        $('#ContentPlaceHolder1_ddlBusinessClassficiation').removeClass('boxshow');
        //$('#ddlBusinessClassficiation').css('border-color', 'green');
    });

    $("#ContentPlaceHolder1_txtOfficalEmail").change(function () {
        $('#ContentPlaceHolder1_txtOfficalEmail').removeClass('boxshow');
        //$('#txtOfficalEmail').css('border-color', 'green');
    });
    $("#ContentPlaceHolder1_txtContactFirstName").change(function () {
        $('#ContentPlaceHolder1_txtContactFirstName').removeClass('boxshow');
        //$('#txtContactFirstName').css('border-color', 'green');
    });

    $("#ContentPlaceHolder1_txtContactLastName").change(function () {
        $('#ContentPlaceHolder1_txtContactLastName').removeClass('boxshow');
        //$('#txtContactFirstName').css('border-color', 'green');
    });

    $("#ContentPlaceHolder1_txtPosition").change(function () {
        $('#ContentPlaceHolder1_txtPosition').removeClass('boxshow');
        //$('#txtPosition').css('border-color', 'green');
    });
    $("#ContentPlaceHolder1_txtPhone").change(function () {
        $('#ContentPlaceHolder1_txtPhone').removeClass('boxshow');
        //$('#txtPhone').css('border-color', 'green');
    });

    $("#ContentPlaceHolder1_txtMobile").change(function () {
        $('#ContentPlaceHolder1_txtMobile').removeClass('boxshow');
        //$('#txtPhone').css('border-color', 'green');
    });

    //$("#ContentPlaceHolder1_txtTradeLicenseNum").change(function () {
    //    $('#ContentPlaceHolder1_txtTradeLicenseNum').removeClass('boxshow');
    //    //$('#txtTradeLicenseNum').css('border-color', 'green');
    //});

    //$("#ContentPlaceHolder1_txtIssuingAuthority").change(function () {
    //    $('#ContentPlaceHolder1_txtIssuingAuthority').removeClass('boxshow');
    //    //$('#txtIssuingAuthority').css('border-color', 'green');
    //});

    //$("#ContentPlaceHolder1_txtExpireDate").change(function () {
    //    $('#ContentPlaceHolder1_txtExpireDate').removeClass('boxshow');
    //    //$('#txtExpireDate').css('border-color', 'green');
    //});

    $("#ContentPlaceHolder1_txtLineAddress1").change(function () {
        $('#ContentPlaceHolder1_txtLineAddress1').removeClass('boxshow');
        //$('#txtLineAddress1').css('border-color', 'green');
    });

    $("#ContentPlaceHolder1_ddlAddressLine1Country").change(function () {
        $('#ContentPlaceHolder1_ddlAddressLine1Country').removeClass('boxshow');
        //$('#ddlAddressLine1Country').css('border-color', 'green');
    });

    $("#ContentPlaceHolder1_txttabAddress1").change(function () {
        $('#ContentPlaceHolder1_txttabAddress1').removeClass('boxshow');
        //$('#txttabAddress1').css('border-color', 'green');
    });

    $("#ContentPlaceHolder1_txtAddress1City").change(function () {
        $('#ContentPlaceHolder1_txtAddress1City').removeClass('boxshow');
        //$('#txtAddress1City').css('border-color', 'green');
    });

    $("#ContentPlaceHolder1_txtAddressPostalCode").change(function () {
        $('#ContentPlaceHolder1_txtAddressPostalCode').removeClass('boxshow');
        //$('#txtAddressPostalCode').css('border-color', 'green');
    });

    $("#ContentPlaceHolder1_txtAddress1Phone").change(function () {
        $('#ContentPlaceHolder1_txtAddress1Phone').removeClass('boxshow');
        //$('#txtAddressPostalCode').css('border-color', 'green');
    });


    $("#ContentPlaceHolder1_btnSave").click(function (e) {
        var IsValid = true;
        if ($('#ContentPlaceHolder1_txtCompanyName').val() == '') {
            $('#ContentPlaceHolder1_txtCompanyName').addClass('boxshow');
            //IsValid alert("Company name can't be blank");           
            IsValid = false;
        }
        else {
            $('#txtCompanyName').removeClass('boxshow');
        }

        if ($('#ContentPlaceHolder1_txtCompanyOwnerName').val() == '') {
            $('#ContentPlaceHolder1_txtCompanyOwnerName').addClass('boxshow');
            //IsValid alert("Company name can't be blank");           
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtCompanyOwnerName').removeClass('boxshow');
        }

        if ($('#ContentPlaceHolder1_ddlCountry').val() == 'Select') {
            $('#ContentPlaceHolder1_ddlCountry').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_ddlCountry').removeClass('boxshow');
        }

        if ($('#ContentPlaceHolder1_ddlBusinessClassficiation').val() == 'Select') {
            $('#ContentPlaceHolder1_ddlBusinessClassficiation').addClass('boxshow');
            // alert("Please Select Business Classification from the list"); 
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_ddlBusinessClassficiation').removeClass('boxshow');
            //__doPostBack("#ContentPlaceHolder1_ddlBusinessClassficiation", "")
        }
        //ShortName
        if ($('#ContentPlaceHolder1_txtOfficalEmail').val() == '') {
            $('#ContentPlaceHolder1_txtOfficalEmail').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtOfficalEmail').removeClass('boxshow');
        }
        //txtContactFirstName
        if ($('#ContentPlaceHolder1_txtContactFirstName').val() == '') {
            $('#ContentPlaceHolder1_txtContactFirstName').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtContactFirstName').removeClass('boxshow');
        }

        if ($('#ContentPlaceHolder1_txtContactLastName').val() == '') {
            $('#ContentPlaceHolder1_txtContactLastName').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtContactLastName').removeClass('boxshow');
        }
        //txtContactFirstName
        if ($('#ContentPlaceHolder1_txtPosition').val() == '') {
            $('#ContentPlaceHolder1_txtPosition').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtPosition').removeClass('boxshow');
        }

        //txtPhone
        if ($('#ContentPlaceHolder1_txtPhone').val() == '') {
            $('#ContentPlaceHolder1_txtPhone').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtPhone').removeClass('boxshow');
        }

        if ($('#ContentPlaceHolder1_txtMobile').val() == '') {
            $('#ContentPlaceHolder1_txtMobile').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtMobile').removeClass('boxshow');
        }
        ////txtTradeLicenseNum
        //if ($('#ContentPlaceHolder1_txtTradeLicenseNum').val() == '') {
        //    $('#ContentPlaceHolder1_txtTradeLicenseNum').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtTradeLicenseNum').removeClass('boxshow');
        //}

        ////txtIssuingAuthority
        //if ($('#ContentPlaceHolder1_txtIssuingAuthority').val() == '') {
        //    $('#ContentPlaceHolder1_txtIssuingAuthority').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtIssuingAuthority').removeClass('boxshow');
        //}

        ////txtExpireDate
        //if ($('#ContentPlaceHolder1_txtExpireDate').val() == '') {
        //    $('#ContentPlaceHolder1_txtExpireDate').addClass('boxshow');
        //    IsValid = false;
        //}
        //else {
        //    $('#ContentPlaceHolder1_txtExpireDate').removeClass('boxshow');
        //}

        //txtLineAddress1
        if ($('#ContentPlaceHolder1_txtLineAddress1').val() == '') {
            $('#ContentPlaceHolder1_txtLineAddress1').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtLineAddress1').removeClass('boxshow');
        }
        //ddlAddressLine1Country
        if ($('#ContentPlaceHolder1_ddlAddressLine1Country').val() == 'Select') {
            $('#ContentPlaceHolder1_ddlAddressLine1Country').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_ddlAddressLine1Country').removeClass('boxshow');
        }

        //txttabAddress1
        if ($('#ContentPlaceHolder1_txttabAddress1').val() == '') {
            $('#ContentPlaceHolder1_txttabAddress1').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txttabAddress1').removeClass('boxshow');
        }
        //txttabAddress1
        if ($('#ContentPlaceHolder1_txttabAddress1').val() == '') {
            $('#ContentPlaceHolder1_txttabAddress1').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txttabAddress1').removeClass('boxshow');
        }

        //txttabAddress1
        if ($('#ContentPlaceHolder1_txtAddress1City').val() == '') {
            $('#ContentPlaceHolder1_txtAddress1City').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtAddress1City').removeClass('boxshow');
        }
        //txttabAddress1
        if ($('#ContentPlaceHolder1_txtAddressPostalCode').val() == '') {
            $('#ContentPlaceHolder1_txtAddressPostalCode').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtAddressPostalCode').removeClass('boxshow');
        }
        //txttabAddress1
        if ($('#ContentPlaceHolder1_txtAddress1Phone').val() == '') {
            $('#ContentPlaceHolder1_txtAddress1Phone').addClass('boxshow');
            IsValid = false;
        }
        else {
            $('#ContentPlaceHolder1_txtAddress1Phone').removeClass('boxshow');
        }
        if (IsValid == false) {
            alert("Mandatory fields are missing.");
            return false;
        }
    })
});
