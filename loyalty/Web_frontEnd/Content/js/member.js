beautyCodeCheck = function () {

    var err = 0;

    //if ($.trim($("#product").val()) == "") {
    //    alert("請選擇所屬產品");
    //    err = 1;
    //}
    //else if ($.trim($("#code1").val()).length != 2 || $.trim($("#code2").val()).length != 4 || $.trim($("#code3").val()).length != 4) {
    //    alert("請輸入所購產品內的「產品密碼」");
    //    err = 1;
    //}

    if (err == 0) {
        $(".loading").show();
        $("#btn_submit").attr('disabled', 'disabled');

        var code = $.trim($("#code1").val()) + "-" + $.trim($("#code2").val()) + "-" + $.trim($("#code3").val());
        var quantity = $.trim($("#quantity").val());

        $.ajax({
            type: 'POST',
            url: 'RegisterPasscode',
            data: {
                pin_value: code,
                quantity: quantity
            },
            success: function (data, status) {
                var obj = jQuery.parseJSON(data);
                if (obj.resultCode == 100) {
                    window.location = "PasscodeSuccess";
                }
                else {
                    if (obj.resultCode == 303001)
                        alert("很抱歉，我們未能確認您所輸入的「產品密碼」，請重新輸入，並核對清楚所輸入的密碼及所屬產品名稱無誤。");
                    else if (obj.resultCode == 303002)
                        alert("「產品數目」不正確, 請重新輸入");
                    //else if (obj.resultCode == -4) {
                    //    alert("很抱歉，所輸入的「產品密碼」已經被登記了，請核對清楚所輸入的密碼正確無誤。");
                    //}
                    //else if (obj.resultCode == -5) {
                    //    alert("會員登入閒置時間太長, 請重新登入");
                    //    window.location = "Logout";
                    //}
                    else {
                        alert("很抱歉，我們未能確認您所輸入的「產品密碼」，請重新輸入，並核對清楚所輸入的密碼及所屬產品名稱無誤。");
                    }
                    //                    if (data == -1) {
                    //                        alert("輸入的「產品密碼」資料不正確");
                    //                    }
                    //                    else if (data == -2) {
                    //                        alert("輸入的「產品密碼」不正確");
                    //                    }
                    //                    else if (data == -3) {
                    //                        alert("輸入的「產品密碼」不屬於選擇的產品");
                    //                    }
                    //                    else if (data == -4) {
                    //                        alert("輸入的「產品密碼」已有用戶登記");
                    //                    }

                    $("#btn_submit").removeAttr('disabled');
                    $(".loading").hide();
                }
            },
            error: function (xhr, desc, err) {
                alert("「產品密碼」登記發生錯誤, 請重新提交");
                $("#btn_submit").removeAttr('disabled');
                $(".loading").hide();
            }
        });
    }

    return false;
}



$().ready(function () {
    $('#code1').keyup(function (e) {
        var key = e.charCode || e.keyCode || 0;
        if ($(this).val().length == 3 && key != 8 && key != 9 && key != 16) {
            $('#code2').focus();
            $('#code2').select();
        }
    });

    $('#code2').keyup(function (e) {
        var key = e.charCode || e.keyCode || 0;
        if ($(this).val().length == 4 && key != 8 && key != 9 && key != 16) {
            $('#code3').focus();
            $('#code3').select();
        }
    });
});
