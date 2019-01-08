$(document).ready(function () {
      
    $("#txtLineAddress1").change(function () {
        $('#txtLineAddress1').removeClass('boxshow');
        //$('#txtLineAddress1').css('border-color', 'green');
    });

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


    $('#btnAddressSave').click(function (e) {
        var isValid = true;  
        //txtLineAddress1
        if ($('#txtLineAddress1').val() == '') {
            $('#txtLineAddress1').addClass('boxshow');
        }
        else {
            $('#txtLineAddress1').removeClass('boxshow');
        }
        //ddlAddressLine1Country
        if ($('#ddlAddressLine1Country').val() == 'Select') {
            $('#ddlAddressLine1Country').addClass('boxshow');
        }
        else {
            $('#ddlAddressLine1Country').removeClass('boxshow');
        }

        //txttabAddress1
        if ($('#txttabAddress1').val() == '') {
            $('#txttabAddress1').addClass('boxshow');
        }
        else {
            $('#txttabAddress1').removeClass('boxshow');
        }
        //txttabAddress1
        if ($('#txttabAddress1').val() == '') {
            $('#txttabAddress1').addClass('boxshow');
        }
        else {
            $('#txttabAddress1').removeClass('boxshow');
        }

        //txttabAddress1
        if ($('#txtAddress1City').val() == '') {
            $('#txtAddress1City').addClass('boxshow');
        }
        else {
            $('#txtAddress1City').removeClass('boxshow');
        }
        //txttabAddress1
        if ($('#txtAddressPostalCode').val() == '') {
            $('#txtAddressPostalCode').addClass('boxshow');
        }
        else {
            $('#txtAddressPostalCode').removeClass('boxshow');
        }
        //txttabAddress1
        if ($('#txtAddress1PostalCode').val() == '') {
            $('#txtAddress1PostalCode').addClass('boxshow');
        }
        else {
            $('#txtAddress1PostalCode').removeClass('boxshow');
        }
    })
});
