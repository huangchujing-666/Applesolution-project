registerCheck = function () {
    var err = 0;

    if ($.trim($("#lastname").val()) == "") {
        alert("會員姓氏必須輸入");
        $("#lastname").focus();
        $("#lastname").select();
        err = 1;
    }
    else if ($.trim($("#firstname").val()) == "") {
        alert("會員名字必須輸入");
        $("#firstname").focus();
        $("#firstname").select();
        err = 1;
    }
    else if ($("#gender_male:checked").val() != "M" && $("#gender_female:checked").val() != "F") {
        alert("會員性別必須選擇");
        err = 1;
    }
    else if ($.trim($("#age").val()) != "" && !ValidateDate($.trim($("#age").val()))) {
        alert("會員出生日期必須是YYYY-MM-DD");
        err = 1;
    }
    else if ($.trim($("#mobile").val()) == "") {
        alert("會員手提電話號碼必須輸入");
        $("#mobile").focus();
        $("#mobile").select();
        err = 1;
    }
    else if ($.trim($("#email").val()) == "") {
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
    else if ($.trim($("#address1").val()) == "" && $.trim($("#address2").val()) == "") {
        alert("會員地址必須輸入");
        $("#address1").focus();
        $("#address1").select();
        err = 1;
    }
    else if ($.trim($("#hkid").val()) == "") {
        alert("會員身份證頭4位數字必須輸入");
        $("#hkid").focus();
        $("#hkid").select();
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
    //else if ($.trim($("#product").val()) == "") {
    //    alert("請選擇所屬產品");
    //    err = 1;
    //}
    //else if ($.trim($("#code1").val()).length != 2 || $.trim($("#code2").val()).length != 4 || $.trim($("#code3").val()).length != 4) {
    //    alert("請輸入所購產品內的「產品密碼」");
    //    err = 1;
    //}
    else if ($("#referral_yes:checked").val() == "Y" && $.trim($("#referreremail").val()) == "") {
        alert("請輸入介紹人的登記電郵");
        $("#referreremail").focus();
        $("#referreremail").select();
        err = 1;
    }
    else if ($("#referral_yes:checked").val() == "Y" && $.trim($("#referreremail").val()) != "" && email_validate($.trim($("#referreremail").val())) == false) {
        alert("請輸入介紹人正確的登記電郵地址");
        $("#referreremail").focus();
        $("#referreremail").select();
        err = 1;
    }
    else if ($.trim($("#email").val()) == $.trim($("#referreremail").val())) {
        alert("介紹人的登記電郵地址不能和會員的電郵地址一樣");
        $("#referreremail").focus();
        $("#referreremail").select();
        err = 1;
    }
    else if ($("#accept_terms:checked").val() != "Y") {
        alert("請閱讀並同意登記會員之條款");
        err = 1;
    }

    if (err == 0) {
        $(".loading").show();
        $("#btn_submit").attr('disabled', 'disabled');

        var lastname = $.trim($("#lastname").val());
        var firstname = $.trim($("#firstname").val());
        var gender = "";
        gender = $("#gender_male:checked").val() == "M" ? "M" : gender;
        gender = $("#gender_female:checked").val() == "F" ? "F" : gender;
        var age = $.trim($("#age").val());
        var mobile = $.trim($("#mobile").val());
        var email = $.trim($("#email").val());
        var address1 = $.trim($("#address1").val());
        var address2 = $.trim($("#address2").val());
        var hkid = $.trim($("#hkid").val());
        var password = $.trim($("#password").val());
        var product = $.trim($("#product").val());
        var code = $.trim($("#code1").val()) + "-" + $.trim($("#code2").val()) + "-" + $.trim($("#code3").val());
        var referreremail = $("#referral_yes:checked").val() == "Y" ? $.trim($("#referreremail").val()) : "";

        $.ajax({
            type: 'POST',
            url: '../Club/Register',
            data: {
                lastname: lastname,
                firstname: firstname,
                gender: gender,
                age: age,
                mobile: mobile,
                email: email,
                address1: address1,
                address2: address2,
                hkid: hkid,
                password: password,
                product: product,
                code: code,
                referreremail: referreremail
            },
            success: function (data, status) {

                var obj = jQuery.parseJSON(data);

                if (obj.result == 1) {
                    window.location = "RegisterSuccess";
                }
                else {
                    if (obj.result == -1) {
                        alert("登記資料不完整或格式錯誤，請重新輸入");
                    }
                    else if (obj.result == -2) {
                        alert("會員登入密碼必須是６個位或以上，包含數字及英文字母");
                    }
                    else if (obj.result == -3) {
                        alert("輸入的「產品密碼」不正確");
                    }
                    else if (obj.result == -4) {
                        alert("輸入的「產品密碼」不屬於選擇的產品");
                    }
                    else if (obj.result == -5) {
                        alert("輸入的「產品密碼」已有用戶登記");
                    }
                    else if (obj.result == -6) {
                        alert("輸入的電郵地址已有用戶登記");
                    }

                    $("#btn_submit").removeAttr('disabled');
                    $(".loading").hide();
                }
            },
            error: function (xhr, desc, err) {
                alert("會員登記發生錯誤, 請重新提交新登記");
                $("#btn_submit").removeAttr('disabled');
                $(".loading").hide();
            }
        });
    }

    return false;
}

$().ready(function () {
    $("#hkid").ForceNumericOnly();
    $("#mobile").ForceNumericOnly();
    $("#age").datepicker({ changeMonth: true, changeYear: true, minDate: "-70Y", maxDate: "-18Y", dateFormat: "yy-mm-dd" });

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
