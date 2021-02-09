sharefriendCheck = function (url, successurl) {
    var err = 0;

    if ($.trim($("#lastname").val()) == "") {
        alert("姓別必須輸入");
        $("#lastname").focus();
        $("#lastname").select();
        err = 1;
    }
    else if ($.trim($("#firstname").val()) == "") {
        alert("名字必須輸入");
        $("#firstname").focus();
        $("#firstname").select();
        err = 1;
    }
    else if ($.trim($("#email").val()) == "") {
        alert("電郵必須輸入");
        $("#email").focus();
        $("#email").select();
        err = 1;
    }
    else if (email_validate($.trim($("#email").val())) == false) {
        alert("請輸入正確的電郵地址");
        $("#email").focus();
        $("#email").select();
        err = 1;
    }
    else if ($.trim($("#friendname").val()) == "") {
        alert("朋友名稱必須輸入");
        $("#friendname").focus();
        $("#friendname").select();
        err = 1;
    }
    else if ($.trim($("#friendemail").val()) == "") {
        alert("朋友電郵必須輸入");
        $("#friendemail").focus();
        $("#friendemail").select();
        err = 1;
    }
    else if (email_validate($.trim($("#friendemail").val())) == false) {
        alert("請輸入正確的電郵地址");
        $("#friendemail").focus();
        $("#friendemail").select();
        err = 1;
    }

    if (err == 0) {
        $(".loading").show();
        $("#btn_submit").attr('disabled', 'disabled');

        var lastname = $.trim($("#lastname").val());
        var firstname = $.trim($("#firstname").val());
        var email = $.trim($("#email").val());
        var friendname = $.trim($("#friendname").val());
        var friendemail = $.trim($("#friendemail").val());

        $.ajax({
            type: 'POST',
            url: url,
            data: {
                lastname: lastname,
                firstname: firstname,
                email: email,
                friendname: friendname,
                friendemail: friendemail
            },
            success: function (data, status) {
                if (data == 1) {
                    window.location = successurl;
                }
                else {
                    alert("輸出資料不完整或格式錯誤，請重新輸入");

                    $("#btn_submit").removeAttr('disabled');
                    $(".loading").hide();
                }
            },
            error: function (xhr, desc, err) {
                alert("寄給朋友提交發生錯誤, 請重新提交");
                $("#btn_submit").removeAttr('disabled');
                $(".loading").hide();
            }
        });
    }

    return false;
}