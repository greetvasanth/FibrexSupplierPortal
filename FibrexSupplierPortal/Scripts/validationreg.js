$(document).ready(function () {

    $("#txtCompanyName").change(function () {
        $('#txtCompanyName').removeClass('boxshow');
        //$('#txtCompanyName').css('border-color','green');
    });
    $("#txtCompanyOwnerName").change(function () {
        $('#txtCompanyOwnerName').removeClass('boxshow');
        //$('#txtCompanyName').css('border-color','green');
    });
    $("#ddlCountry").change(function () {
        $('#ddlCountry').removeClass('boxshow');
        //$('#ddlCountry').css('border-color', 'green');
    });

    //$("#ddlBusinessClassficiation").change(function () {
    $(document).on("change", "#ddlBusinessClassficiation", function (e) {
        $('#ddlBusinessClassficiation').removeClass('boxshow');
        //$('#ddlBusinessClassficiation').css('border-color', 'green');
    });

    $("#txtOfficalEmail").change(function () {
        $('#txtOfficalEmail').removeClass('boxshow');
        //$('#txtOfficalEmail').css('border-color', 'green');
    });
    $("#txtContactFirstName").change(function () {
        $('#txtContactFirstName').removeClass('boxshow');
        //$('#txtContactPerson').css('border-color', 'green');
    });
    $("#txtContactLastName").change(function () {
        $('#txtContactLastName').removeClass('boxshow');
        //$('#txtContactPerson').css('border-color', 'green');
    });
    $("#txtPosition").change(function () {
        $('#txtPosition').removeClass('boxshow');
        //$('#txtPosition').css('border-color', 'green');
    });
    $("#txtPhone").change(function () {
        $('#txtPhone').removeClass('boxshow');
        //$('#txtPhone').css('border-color', 'green');
    });
/*
    $("#txtTradeLicenseNum").change(function () {
        $('#txtTradeLicenseNum').removeClass('boxshow');
        //$('#txtTradeLicenseNum').css('border-color', 'green');
    });

    $("#txtIssuingAuthority").change(function () {
        $('#txtIssuingAuthority').removeClass('boxshow');
        //$('#txtIssuingAuthority').css('border-color', 'green');
    });

    $("#txtExpireDate").change(function () {
        $('#txtExpireDate').removeClass('boxshow');
        //$('#txtExpireDate').css('border-color', 'green');
    });

    $("#txtLineAddress1").change(function () {
        $('#txtLineAddress1').removeClass('boxshow');
        //$('#txtLineAddress1').css('border-color', 'green');
    });
    */
    $("#ddlAddressLine1Country").change(function () {
        $('#ddlAddressLine1Country').removeClass('boxshow');
        //$('#ddlAddressLine1Country').css('border-color', 'green');
    });

    $("#txttabAddress1").change(function () {
        $('#txttabAddress1').removeClass('boxshow');
        //$('#txttabAddress1').css('border-color', 'green');
    });

    $("#txtAddress1City").change(function () {
        $('#txtAddress1City').removeClass('boxshow');
        //$('#txtAddress1City').css('border-color', 'green');
    });

    $("#txtAddressPostalCode").change(function () {
        $('#txtAddressPostalCode').removeClass('boxshow');
        //$('#txtAddressPostalCode').css('border-color', 'green');
    });

    $("#txtAddress1Phone").change(function () {
        $('#txtAddress1Phone').removeClass('boxshow');
        //$('#txtAddressPostalCode').css('border-color', 'green');
    });
    $("#txtMobile").change(function () {
        $('#txtMobile').removeClass('boxshow');
        //$('#txtAddressPostalCode').css('border-color', 'green');
    });

    $("#txtCaptcha").change(function () {
        $('#txtCaptcha').removeClass('boxshow');
        //$('#txtAddressPostalCode').css('border-color', 'green');
    });

    $('#btnSave').click(function (e) {
        var IsValid = true;
        if ($('#txtCompanyName').val() == '') {
            $('#txtCompanyName').addClass('boxshow');
            //IsValid alert("Company name can't be blank");           
            IsValid = false;
        }
        else {
            $('#txtCompanyName').removeClass('boxshow');
        }

        if ($('#txtCompanyOwnerName').val() == '') {
            $('#txtCompanyOwnerName').addClass('boxshow');
            //IsValid alert("Company name can't be blank");           
            IsValid = false;
        }
        else {
            $('#txtCompanyOwnerName').removeClass('boxshow');
        }
        //
        if ($('#ddlCountry').val() == 'Select') {
            $('#ddlCountry').addClass('boxshow');
            //alert("Please Select country from the list");
            IsValid = false;
        }
        else {
            $('#ddlCountry').removeClass('boxshow');
        }


        //Business
        if ($('#ddlBusinessClassficiation').val() == 'Select') {
            $('#ddlBusinessClassficiation').addClass('boxshow');
            // alert("Please Select Business Classification from the list"); 
            IsValid = false;
        }
        else {
            $('#ddlBusinessClassficiation').removeClass('boxshow');
            //__doPostBack("#ddlBusinessClassficiation", "")
        }
        //ShortName
        if ($('#txtOfficalEmail').val() == '') {
            $('#txtOfficalEmail').addClass('boxshow');
            // alert("Official Email can't blank"); 
            IsValid = false;
        }
        else {
            $('#txtOfficalEmail').removeClass('boxshow');
        }
        //txtContactPerson
        if ($('#txtContactFirstName').val() == '') {
            $('#txtContactFirstName').addClass('boxshow');
            //alert("Contact Person can't blank"); 
            IsValid = false;
        }
        else {
            $('#txtContactFirstName').removeClass('boxshow');
        }

        if ($('#txtContactLastName').val() == '') {
            $('#txtContactLastName').addClass('boxshow');
            //alert("Contact Person can't blank"); 
            IsValid = false;
        }
        else {
            $('#txtContactLastName').removeClass('boxshow');
        }

        //txtContactPerson
        if ($('#txtPosition').val() == '') {
            $('#txtPosition').addClass('boxshow');
            /// alert("Position can't blank"); 
            IsValid = false;
        }
        else {
            $('#txtPosition').removeClass('boxshow');
        }

        //txtPhone
        if ($('#txtPhone').val() == '') {
            $('#txtPhone').addClass('boxshow');
            // alert("Phone can't blank");  
            IsValid = false;
        }
        else {
            $('#txtPhone').removeClass('boxshow');
        }
        if ($('#txtMobile').val() == '') {
            $('#txtMobile').addClass('boxshow');
            // alert("Phone can't blank");  
            IsValid = false;
        }
        else {
            $('#txtMobile').removeClass('boxshow');
        }
        ////txtTradeLicenseNum
        //if ($('#txtTradeLicenseNum').val() == '') {
        //    $('#txtTradeLicenseNum').addClass('boxshow');
        //    // alert("Trade License can't blank"); 
        //    IsValid = false;
        //}
        //else {
        //    $('#txtTradeLicenseNum').removeClass('boxshow');
        //}

        ////txtIssuingAuthority
        //if ($('#txtIssuingAuthority').val() == '') {
        //    $('#txtIssuingAuthority').addClass('boxshow');
        //    // alert("Issuing Authority can't blank"); 
        //    IsValid = false;
        //}
        //else {
        //    $('#txtIssuingAuthority').removeClass('boxshow');
        //}

        ////txtExpireDate
        //if ($('#txtExpireDate').val() == '__-___-____') {
        //    $('#txtExpireDate').addClass('boxshow');
        //    // alert("Expire Date can't blank"); 
        //    IsValid = false;
        //}
        //else {
        //    $('#txtExpireDate').removeClass('boxshow');
        //}

        //txtLineAddress1
        if ($('#txtLineAddress1').val() == '') {
            $('#txtLineAddress1').addClass('boxshow');
            //  alert("Line Address 1 can't blank"); 
            IsValid = false;
        }
        else {
            $('#txtLineAddress1').removeClass('boxshow');
        }
        //ddlAddressLine1Country
        if ($('#ddlAddressLine1Country').val() == 'Select') {
            $('#ddlAddressLine1Country').addClass('boxshow');
            //alert("Address Country can't blank"); 
            IsValid = false;
        }
        else {
            $('#ddlAddressLine1Country').removeClass('boxshow');
        }

        //txttabAddress1
        if ($('#txttabAddress1').val() == '') {
            $('#txttabAddress1').addClass('boxshow');
            ///alert("Address can't blank"); 
            IsValid = false;
        }
        else {
            $('#txttabAddress1').removeClass('boxshow');
        }

        //txttabAddress1
        if ($('#txtAddress1City').val() == '') {
            $('#txtAddress1City').addClass('boxshow');
            //alert("City can't blank"); 
            IsValid = false;
        }
        else {
            $('#txtAddress1City').removeClass('boxshow');
        }
        //txttabAddress1
        if ($('#txtAddressPostalCode').val() == '') {
            $('#txtAddressPostalCode').addClass('boxshow');
            //alert("Postal code can't blank"); 
            IsValid = false;
        }
        else {
            $('#txtAddressPostalCode').removeClass('boxshow');
        }
        //txttabAddress1
        if ($('#txtAddress1Phone').val() == '') {
            $('#txtAddress1Phone').addClass('boxshow');
            //alert("Address Phone can't blank"); 
            IsValid = false;
        }
        else {
            $('#txtAddress1Phone').removeClass('boxshow');
        }

        if ($('#txtCaptcha').val() == '') {
            $('#txtCaptcha').addClass('boxshow');
            //alert("Address Phone can't blank"); 
            IsValid = false;
        }
        else {
            $('#txtCaptcha').removeClass('boxshow');
        }
      
         
        if (IsValid == false) {
            alert("Mandatory fields are missing.");
            return false;
        } 
    })
});
