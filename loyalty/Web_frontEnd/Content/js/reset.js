resetCheck = function () {
    var err = 0;

    if ($.trim($("#email").val()) == "") {
        alert("會員電郵地址必須輸入");
        $("#email").focus();
        $("#email").select();
        err = 1;
    }
    else if (email_validate($.trim($("#email").val())) == false) {
        alert("請輸入正確的會員電郵地址");
        $("#email").focus();
        $("#email").select();
        err = 1;
    }
    else if ($.trim($("#password").val()) == "") {
        alert("會員登入密碼必須輸入");
        $("#password").focus();
        $("#password").select();
        err = 1;
    }
    else if ($.trim($("#password").val()) != $.trim($("#cpassword").val())) {
        alert("會員登入密碼和確認密碼不相同");
        $("#password").focus();
        $("#password").select();
        err = 1;
    }
    else if (checkPassword($.trim($("#password").val())) == false) {
        alert("會員登入密碼必須是８個位或以上，包括最少一個符號，如@#$%^");
        $("#password").focus();
        $("#password").select();
        err = 1;
    }

    if (err == 0) {
        $(".loading").show();
        $("#btn_submit").attr('disabled', 'disabled');

        var email = $.trim($("#email").val());
        var password = $.trim($("#password").val());
        var key = $.trim($("#key").val());
        var action = "reset";

        $.ajax({
            type: 'POST',
            url: 'ResetAJAX',
            data: {
                action: action,
                email: email,
                password: password,
                key: key
            },
            success: function (data, status) {
                if (data == 1) {
                    window.location = "ResetSuccess";
                }
                else if (data == -1) {
                    alert("輸入的資料不正確，請重新輸入");
                    $("#email").val("");
                    $("#password").val("");
                    $("#cpassword").val("");
                    $("#email").focus();
                    $("#btn_submit").removeAttr('disabled');
                }
                else if (data == -2) {
                    alert("會員登入密碼必須是６個位或以上，包含數字及英文字母");
                    $("#password").val("");
                    $("#cpassword").val("");
                    $("#password").focus();
                    $("#btn_submit").removeAttr('disabled');
                }
                else if (data == -3) {
                    alert("重設密碼連結已經失效。");
                    $("#email").val("");
                    $("#password").val("");
                    $("#cpassword").val("");
                    $("#email").focus();
                    $("#btn_submit").removeAttr('disabled');
                }
                else if (data == -4) {
                    alert("會員帳戶資料不正確。");
                    $("#email").val("");
                    $("#password").val("");
                    $("#cpassword").val("");
                    $("#email").focus();
                    $("#btn_submit").removeAttr('disabled');
                }
                else if (data == -5) {
                    alert("重設密碼連結已經失效。");
                    $("#email").val("");
                    $("#password").val("");
                    $("#cpassword").val("");
                    $("#email").focus();
                    $("#btn_submit").removeAttr('disabled');
                    $(".loading").hide();
                }
            },
            error: function (xhr, desc, err) {
                alert("重設密碼系統發生錯誤, 請請重新提交");
                $("#btn_submit").removeAttr('disabled');
                $(".loading").hide();
            }
        });
    }

    return false;
}

