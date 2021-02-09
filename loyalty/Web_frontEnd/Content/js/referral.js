referralCheck = function() {
    var err = 0;
    var hasinput = 0;

    if (!err && ($.trim($("#friendname1").val()) != "" || $.trim($("#friendemail1").val()) != "")) {
        hasinput = 1;

        if ($.trim($("#friendname1").val()) == "") {
            alert("好友名稱必須輸入");
            $("#friendname1").focus();
            $("#friendname1").select();
            err = 1;
        }
        else if ($.trim($("#friendemail1").val()) == "") {
            alert("好友電郵必須輸入");
            $("#friendemail1").focus();
            $("#friendemail1").select();
            err = 1;
        }
        else if (email_validate($.trim($("#friendemail1").val())) == false) {
            alert("請輸入正確的好友電郵地址");
            $("#friendemail1").focus();
            $("#friendemail1").select();
            err = 1;
        }
    }

    if (!err && ($.trim($("#friendname2").val()) != "" || $.trim($("#friendemail2").val()) != "")) {
        hasinput = 1;

        if ($.trim($("#friendname2").val()) == "") {
            alert("好友名稱必須輸入");
            $("#friendname2").focus();
            $("#friendname2").select();
            err = 1;
        }
        else if ($.trim($("#friendemail2").val()) == "") {
            alert("好友電郵必須輸入");
            $("#friendemail2").focus();
            $("#friendemail2").select();
            err = 1;
        }
        else if (email_validate($.trim($("#friendemail2").val())) == false) {
            alert("請輸入正確的好友電郵地址");
            $("#friendemail2").focus();
            $("#friendemail2").select();
            err = 1;
        }
    }

    if (!err && ($.trim($("#friendname3").val()) != "" || $.trim($("#friendemail3").val()) != "")) {
        hasinput = 1;

        if ($.trim($("#friendname3").val()) == "") {
            alert("好友名稱必須輸入");
            $("#friendname3").focus();
            $("#friendname3").select();
            err = 1;
        }
        else if ($.trim($("#friendemail3").val()) == "") {
            alert("好友電郵必須輸入");
            $("#friendemail3").focus();
            $("#friendemail3").select();
            err = 1;
        }
        else if (email_validate($.trim($("#friendemail3").val())) == false) {
            alert("請輸入正確的好友電郵地址");
            $("#friendemail3").focus();
            $("#friendemail3").select();
            err = 1;
        }
    }

    if (!err && ($.trim($("#friendname4").val()) != "" || $.trim($("#friendemail4").val()) != "")) {
        hasinput = 1;

        if ($.trim($("#friendname4").val()) == "") {
            alert("好友名稱必須輸入");
            $("#friendname4").focus();
            $("#friendname4").select();
            err = 1;
        }
        else if ($.trim($("#friendemail4").val()) == "") {
            alert("好友電郵必須輸入");
            $("#friendemail4").focus();
            $("#friendemail4").select();
            err = 1;
        }
        else if (email_validate($.trim($("#friendemail4").val())) == false) {
            alert("請輸入正確的好友電郵地址");
            $("#friendemail4").focus();
            $("#friendemail4").select();
            err = 1;
        }
    }

    if (hasinput == 0) {
        alert("請輸入要推薦好友的資料");
        err = 1;
    }

    if (err == 0) {
        $(".loading").show();
        $("#btn_submit").attr('disabled', 'disabled');

        var friendname1 = $.trim($("#friendname1").val());
        var friendemail1 = $.trim($("#friendemail1").val());
        var friendname2 = $.trim($("#friendname2").val());
        var friendemail2 = $.trim($("#friendemail2").val());
        var friendname3 = $.trim($("#friendname3").val());
        var friendemail3 = $.trim($("#friendemail3").val());
        var friendname4 = $.trim($("#friendname4").val());
        var friendemail4 = $.trim($("#friendemail4").val());

        $.ajax({
            type: 'POST',
            url: 'ReferralAJAX',
            data: {
                friendname1: friendname1,
                friendemail1: friendemail1,
                friendname2: friendname2,
                friendemail2: friendemail2,
                friendname3: friendname3,
                friendemail3: friendemail3,
                friendname4: friendname4,
                friendemail4: friendemail4
            },
            success: function (data, status) {
                if (data == 1) {
                    window.location = "ReferralSuccess";
                }
                else {
                    if (data == -1) {
                        alert("推薦好友的資料不完整或格式錯誤，請重新輸入");
                    }
                    else if (data == -2) {
                        alert("會員登入閒置時間太長, 請重新登入");
                        window.location = "Logout";
                    }

                    $("#btn_submit").removeAttr('disabled');
                    $(".loading").hide();
                }
            },
            error: function (xhr, desc, err) {
                alert("推薦好友發生錯誤, 請重新提交");
                $("#btn_submit").removeAttr('disabled');
                $(".loading").hide();
            }
        });
    }

    return false;
}

$(function () {
    // Handler for .ready() called.
    $("#friendname1").focus();
});