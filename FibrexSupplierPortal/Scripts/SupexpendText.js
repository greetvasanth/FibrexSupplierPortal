
$(document).ready(function () {
    var showChar = 50;
    var ellipsestext = "";
    var moretext = "...";
    var lesstext = "...";
    $('.more').each(function () {
        var content = $(this).html();
        if (content.length > 0) {
            if (content.length > showChar) {

                var c = content.substr(0, showChar);
                var h = content.substr(showChar, content.length - showChar); //showChar-1Bfore

                var html = c + '<span class="moreelipses">' + ellipsestext + '</span><span class="morecontent"><span>' + h + '</span>&nbsp;&nbsp;<a href="" class="morelink">' + moretext + '</a></span>';

                $(this).html(html);
            }
        }

    });

    $(".morelink").click(function () {
        if ($(this).hasClass("less")) {
            $(this).removeClass("less");
            $(this).html(moretext);
        } else {
            $(this).addClass("less");
            $(this).html(lesstext);
        }
        $(this).parent().prev().toggle();
        $(this).prev().toggle();
        return false;
    });
});


$(document).ready(function () {
    var showChar1 =11;
    var ellipsestext1 = "";
    var moretext1 = "...";
    var lesstext1 = "...";
    $('.more1').each(function () {
        var content1 = $(this).html();
        if (content1.length > 0) {
            if (content1.length > showChar1) {

                var c1 = content1.substr(0, showChar1);
                var h1 = content1.substr(showChar1, content1.length - showChar1); //showChar-1Bfore

                var html1 = c1 + '<span class="ellipsestext1">' + ellipsestext1 + '</span><span class="morecontent"><span>' + h1 + '</span>&nbsp;&nbsp;<a href="" class="morelink1">' + moretext1 + '</a></span>';

                $(this).html(html1);
            }
        }

    });

    $(".morelink1").click(function () {
        if ($(this).hasClass("less")) {
            $(this).removeClass("less");
            $(this).html(moretext1);
        } else {
            $(this).addClass("less");
            $(this).html(lesstext1);
        }
        $(this).parent().prev().toggle();
        $(this).prev().toggle();
        return false;
    });
});