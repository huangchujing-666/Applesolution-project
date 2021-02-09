contactusCheck = function (url, successurl) {
    var err = 0;

    if ($("#type_customer:checked").val() != "customer" && $("#type_care:checked").val() != "care") {
        alert("請選擇閣下是消費者或專業護理人員");
        err = 1;
    }
    else if ($.trim($("#lastname").val()) == "") {
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
    else if ($.trim($("#phone").val()) == "") {
        alert("電話必須輸入");
        $("#phone").focus();
        $("#phone").select();
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
    else if ($.trim($("#content").val()) == "") {
        alert("內容必須輸入");
        $("#content").focus();
        $("#content").select();
        err = 1;
    }

    if (err == 0) {
        $(".loading").show();

        $("#btn_submit").attr('disabled', 'disabled');

        var type = $('input[name=type]:checked').val();
        var lastname = $.trim($("#lastname").val());
        var firstname = $.trim($("#firstname").val());
        var phone = $.trim($("#phone").val());
        var email = $.trim($("#email").val());
        var content = $.trim($("#content").val());
        var address1 = $.trim($("#address1").val());
        var address2 = $.trim($("#address2").val());

        $.ajax({
            type: 'POST',
            url: url,
            data: {
                type: type,
                lastname: lastname,
                firstname: firstname,
                phone: phone,
                email: email,
                address1: address1,
                address2: address2,
                content: content
            },
            success: function (data, status) {
                if (data == 1) {
                    window.location = successurl;
                }
                else {
                    alert("聯絡資料不完整或格式錯誤，請重新輸入");

                    $("#btn_submit").removeAttr('disabled');
                    $(".loading").hide();
                }
            },
            error: function (xhr, desc, err) {
                alert("聯絡我們資料提交發生錯誤, 請重新提交");
                $("#btn_submit").removeAttr('disabled');
                $(".loading").hide();
            }
        });
    }

    return false;
}