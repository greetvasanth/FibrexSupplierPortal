$(document).ready(function () {
    $('#<%= txtnewPassword.ClientID %>').keyup(function () {
        var ucase = new RegExp("[A-Z]+");
        var lcase = new RegExp("[a-z]+");
        var num = new RegExp("[0-9]+");

        if ($('#txtnewPassword').val().length >= 6) {
            alert(0);
            $("#8char").removeClass("glyphicon-remove");
            $("#8char").addClass("glyphicon-ok");
            $("#8char").css("color", "#00A41E");
        } else {
            $("#8char").removeClass("glyphicon-ok");
            $("#8char").addClass("glyphicon-remove");
            $("#8char").css("color", "#FF0004");
        }

        if (ucase.test($('#<%= txtnewPassword.ClientID %>').val())) {
            $("#ucase").removeClass("glyphicon-remove");
            $("#ucase").addClass("glyphicon-ok");
            $("#ucase").css("color", "#00A41E");
        } else {
            $("#ucase").removeClass("glyphicon-ok");
            $("#ucase").addClass("glyphicon-remove");
            $("#ucase").css("color", "#FF0004");
        }

        if (lcase.test($('#<%= txtnewPassword.ClientID %>').val())) {
            $("#lcase").removeClass("glyphicon-remove");
            $("#lcase").addClass("glyphicon-ok");
            $("#lcase").css("color", "#00A41E");
        } else {
            $("#lcase").removeClass("glyphicon-ok");
            $("#lcase").addClass("glyphicon-remove");
            $("#lcase").css("color", "#FF0004");
        }

        if (num.test($('#<%= txtnewPassword.ClientID %>').val())) {
            $("#num").removeClass("glyphicon-remove");
            $("#num").addClass("glyphicon-ok");
            $("#num").css("color", "#00A41E");
        } else {
            $("#num").removeClass("glyphicon-ok");
            $("#num").addClass("glyphicon-remove");
            $("#num").css("color", "#FF0004");
        }

        if ($('#<%= txtnewPassword.ClientID %>').val() == $('#<%= txtnewConfirmPassword.ClientID %>').val()) {
            $("#pwmatch").removeClass("glyphicon-remove");
            $("#pwmatch").addClass("glyphicon-ok");
            $("#pwmatch").css("color", "#00A41E");
        } else {
            $("#pwmatch").removeClass("glyphicon-ok");
            $("#pwmatch").addClass("glyphicon-remove");
            $("#pwmatch").css("color", "#FF0004");
        }
    });
});
$(document).ready(function () {
    $('#<%= btnChangepassword.ClientID %>').click(function (e) {
        if ($('#<%= txtCurrentPassword.ClientID %>').val() == '') {
            $('#<%= txtCurrentPassword.ClientID %>').addClass('boxshow');
        }
        else {
            $('#<%= txtCurrentPassword.ClientID %>').removeClass('boxshow');
        }

        if ($('#<%= txtnewPassword.ClientID %>').val() == '') {
            $('#<%= txtnewPassword.ClientID %>').addClass('boxshow');
        }
        else {
            $('#<%= txtnewPassword.ClientID %>').removeClass('boxshow');
        }

        if ($('#<%= txtnewConfirmPassword.ClientID %>').val() == '') {
            $('#<%= txtnewConfirmPassword.ClientID %>').addClass('boxshow');
        }
        else {
            $('#<%= txtnewConfirmPassword.ClientID %>').removeClass('boxshow');
        }
    });
});