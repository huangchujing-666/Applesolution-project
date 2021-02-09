profileCheck = function () {
    var err = 0;

    /*
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
    else if ($("#age_1:checked").val() != "1" && $("#age_2:checked").val() != "2" && $("#age_3:checked").val() != "3" && $("#age_4:checked").val() != "4" && $("#age_5:checked").val() != "5") {
    alert("會員年齡必須選擇");
    err = 1;
    }
    else 
    else if ($.trim($("#email").val()) == "") {
    alert("會員電郵地址必須輸入");
    $("#email").focus();
    $("#email").select();
    err = 1;
    }
    if ($.trim($("#age").val()) == "") {
        alert("會員出生日期必須輸入");
        $("#age").focus();
        $("#age").select();
        err = 1;
    }
    */
    if ($.trim($("#mobile").val()) == "") {
        alert("會員手提電話號碼必須輸入");
        $("#mobile").focus();
        $("#mobile").select();
        err = 1;
    }
    //else if (email_validate($.trim($("#email").val())) == false) {
    //    alert("請輸入正確的會員電郵地址");
    //    $("#email").focus();
    //    $("#email").select();
    //    err = 1;
    //}
    else if ($.trim($("#address1").val()) == "" && $.trim($("#address2").val()) == "") {
        alert("會員地址必須輸入");
        $("#address1").focus();
        $("#address1").select();
        err = 1;
    }
    else if ($.trim($("#password").val()) != "" || $.trim($("#cpassword").val()) != "") {
        if ($.trim($("#password").val()) == "") {
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
    }
    /*
    else if ($.trim($("#hkid").val()) == "") {
    alert("會員身份證頭4位數字必須輸入");
    $("#hkid").focus();
    $("#hkid").select();
    err = 1;
    }
    */

    if (err == 0) {
        $(".loading").show();
        $("#btn_submit").attr('disabled', 'disabled');
        /*
        var lastname = $.trim($("#lastname").val());
        var firstname = $.trim($("#firstname").val());
        var gender = "";
        gender = $("#gender_male:checked").val() == "M" ? "M" : gender;
        gender = $("#gender_female:checked").val() == "F" ? "F" : gender;
        var age = "";
        age = $("#age_1:checked").val() == "1" ? "1" : age;
        age = $("#age_2:checked").val() == "2" ? "2" : age;
        age = $("#age_3:checked").val() == "3" ? "3" : age;
        age = $("#age_4:checked").val() == "4" ? "4" : age;
        age = $("#age_5:checked").val() == "5" ? "5" : age;
        var age = $.trim($("#age").val());
        */
        var mobile = $.trim($("#mobile").val());
        var email = $.trim($("#email").val());
        var address1 = $.trim($("#address1").val());
        var address2 = $.trim($("#address2").val());
        //        var hkid = $.trim($("#hkid").val());
        var password = $.trim($("#password").val());

        $.ajax({
            type: 'POST',
            url: '../Club/UpdateMember',
            data: {
                mobile: mobile,
                email: email,
                address1: address1,
                address2: address2,
                password: password
            },
            success: function (data, status) {
                var obj = jQuery.parseJSON(data);

                if (obj.result == 1) {
                    window.location = "ProfileChangeSuccess";
                }
                else {
                    if (obj.result == -1) {
                        alert("會員資料不完整或格式錯誤，請重新輸入");
                    }
                    else if (obj.result == -2) {
                        alert("會員登入密碼必須是６個位或以上，包含數字及英文字母");
                    }
                    else if (obj.result == -3) {
                        alert("會員帳戶不存在，系統將會自動登出");
                        window.location = "Logout";
                    }
                    else if (obj.result == -4) {
                        alert("會員登入閒置時間太長, 請重新登入");
                        window.location = "Logout";
                    }

                    $("#btn_submit").removeAttr('disabled');
                    $(".loading").hide();
                }
            },
            error: function (xhr, desc, err) {
                alert("會員更改發生錯誤, 請重新提交新登記");
                $("#btn_submit").removeAttr('disabled');
                $(".loading").hide();
            }
        });
    }

    return false;
}

$().ready(function () {
    $("#mobile").ForceNumericOnly();
    //$("#age").datepicker({ changeMonth: true, changeYear: true, minDate: "-70Y", maxDate: "-18Y", dateFormat: "yy-mm-dd" });
});

