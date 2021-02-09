forgotCheck = function () {
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

    if (err == 0) {
        $(".loading").show();
        $("#btn_submit").attr('disabled', 'disabled');

        var email = $.trim($("#email").val());
        var action = "send";

        $.ajax({
            type: 'POST',
            url: 'ForgotAJAX',
            data: {
                action: action,
                email: email
            },
            success: function (data, status) {
                if (data == 1) {
                    alert("重設密碼的電郵已發送到您登記的電郵信箱");
                    $("#email").val("");
                    $("#email").focus();
                    $("#btn_submit").removeAttr('disabled');
                    $(".loading").hide();
                }
                else {
                    alert("登記的帳戶不存在，請重新輸入");
                    $("#email").val("");
                    $("#email").focus();
                    $("#btn_submit").removeAttr('disabled');
                    $(".loading").hide();
                }
            },
            error: function (xhr, desc, err) {
                alert("忘記密碼系統發生錯誤, 請請重新提交");
                $("#btn_submit").removeAttr('disabled');
                $(".loading").hide();
            }
        });
    }

    return false;    
}