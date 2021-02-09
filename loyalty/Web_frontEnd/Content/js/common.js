PrintPage = function () {
    window.print();
    return false;
}

minusMenu = function (fix) {
    if ($("#mainmenu").css("display") == "none") {
        if (fix) {
            ValidateLogin
            $('.denglu_scroll').css('width', 550);
            $("#rightcontent").removeClass("w976");
            $("#mainmenu").show("fast", function () {
                $("#menucontrolicon").attr("src", $("#menucontrolicon").attr("src").replace("expand", "minus"));
                $('#loginname').focus();
            });
        }
    }
    else {
        if (fix) {
            $("#mainmenu").hide("fast", function () {
                $("#rightcontent").addClass("w976");
                $("#menucontrolicon").attr("src", $("#menucontrolicon").attr("src").replace("minus", "expand"));
                $('#loginname').val('');
                $('#loginpassword').val('');
                $('.denglu_scroll').css('width', 30);
            });
        }

    }
}

ValidateLogin = function (url, loginnameid, loginpasswordid, loginbuttonid) {
    var err = 0;
    var loginname = "";
    var loginpassword = "";
    var loginbutton = "";

    if ($.trim($("#" + loginnameid).val()) == "" || $.trim($("#" + loginnameid).val()) == "登記電郵") {
        alert("請輸入登記電郵");
        $("#" + loginnameid).focus();
        $("#" + loginnameid).select();
        err = 1;
    }
    else if ($.trim($("#" + loginpasswordid).val()) == "") {
        alert("請輸入密碼");
        $("#" + loginpasswordid).focus();
        $("#" + loginpasswordid).select();
        err = 1;
    }

    if (err == 0) {

        loginname = $.trim($("#" + loginnameid).val());
        loginpassword = $.trim($("#" + loginpasswordid).val());
        loginbutton = loginbuttonid

        $("#" + loginbutton).attr('disabled', 'disabled');

        $.ajax({
            type: 'POST',
            url: url + 'Login',
            data: {
                loginname: loginname,
                loginpassword: loginpassword
            },
            success: function (data, status) {
                if (data == 1) {
                    window.location = url + 'Member';
                }
                else {
                    if (data == -1 || data == -2 || data == -5) {
                        alert("會員登入資料不正確,請重新輸入");
                        $("#" + loginnameid).val("");
                        $("#" + loginpasswordid).val("");
                        $("#" + loginnameid).focus();
                    }
                    else if (data == -3) {
                        alert("會員帳戶未啟動");
                        $("#" + loginnameid).val("");
                        $("#" + loginpasswordid).val("");
                    }
                    else if (data == -4) {
                        alert("會員帳戶已被凍結, 請聯絡OENOBIOL");
                        $("#" + loginnameid).val("");
                        $("#" + loginpasswordid).val("");
                    }

                    $("#" + loginbutton).removeAttr('disabled');
                }
            },
            error: function (xhr, desc, err) {
                alert(err);
                alert("會員登入發生錯誤, 請重新提交");
                $("#" + loginbutton).removeAttr('disabled');
            }
        });
    }

    return false;
}

var loginAndSubmitPasscode = function () {

    event.preventDefault(); //avoid traditional submit form

    $(".loading").show();
    $("#btn_submit").attr('disabled', 'disabled');

    var loginnameid = "loginformname";
    var loginpasswordid = "loginformpassword";
    var err = 0;
    if ($.trim($("#" + loginnameid).val()) == "" || $.trim($("#" + loginnameid).val()) == "登記電郵") {
        alert("請輸入登記電郵");
        $("#" + loginnameid).focus();
        $("#" + loginnameid).select();
        err = 1;
    }
    else if ($.trim($("#" + loginpasswordid).val()) == "") {
        alert("請輸入密碼");
        $("#" + loginpasswordid).focus();
        $("#" + loginpasswordid).select();
        err = 1;
    }

    if (err == 0) {

        loginname = $.trim($("#" + loginnameid).val());
        loginpassword = $.trim($("#" + loginpasswordid).val());


        $.ajax({
            type: 'POST',
            url: 'Login',
            data: {
                loginname: loginname,
                loginpassword: loginpassword
            },
            success: function (data, status) {
                if (data == 1) {
                    var code = $.trim($("#passcode").val());
                    var quantity = 1;

                    AjaxPasscode(code, quantity);
                }
                else {
                    if (data == -1 || data == -2 || data == -5) {
                        alert("會員登入資料不正確,請重新輸入");
                        $("#" + loginnameid).val("");
                        $("#" + loginpasswordid).val("");
                        $("#" + loginnameid).focus();
                    }
                    else if (data == -3) {
                        alert("會員帳戶未啟動");
                        $("#" + loginnameid).val("");
                        $("#" + loginpasswordid).val("");
                    }
                    else if (data == -4) {
                        alert("會員帳戶已被凍結, 請聯絡OENOBIOL");
                        $("#" + loginnameid).val("");
                        $("#" + loginpasswordid).val("");
                    }

                    $("#btn_submit").removeAttr('disabled');
                    $(".loading").hide();
                }
            },
            error: function (xhr, desc, err) {
                
                alert("會員登入發生錯誤, 請重新提交, code:"+err);

                $("#btn_submit").removeAttr('disabled');
                $(".loading").hide();
            }
        });
    }
    else {
        $("#btn_submit").removeAttr('disabled');
        $(".loading").hide();
    }
    
    return false;
}

var submitPasscode = function () {

    event.preventDefault(); //avoid traditional submit form

    $(".loading").show();
    $("#btn_submit").attr('disabled', 'disabled');

    var code = $.trim($("#passcode").val());
    var quantity = 1;

    AjaxPasscode(code, quantity);

    return false;
}

var AjaxPasscode = function (code, quantity) {
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
                else {
                    alert("很抱歉，我們未能確認您所輸入的「產品密碼」，請重新輸入，並核對清楚所輸入的密碼及所屬產品名稱無誤。");
                }

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


submitenter = function (myfield, e) {
    var keycode;
    if (window.event) keycode = window.event.keyCode;
    else if (e) keycode = e.which;
    else return true;

    if (keycode == 13) {
        ValidateLogin($('#loginform').attr('action'), 'loginname', 'loginpassword', 'loginsubmit');
        return false;
    }
    else
        return true;
}

focusLoginname = function() {
//    if ($("#loginname").val() == "登記電郵")
//        $("#loginname").val("");
}

blurLoginname = function() {
//    if ($.trim($("#loginname").val()) == "")
//        $("#loginname").val("登記電郵");
}

focusLoginpassword = function() {
//    if ($("#loginpassword").val() == "密碼")
//        $("#loginpassword").val("");
}

blurLoginpassword = function() {
//    if ($.trim($("#loginpassword").val()) == "")
//        $("#loginpassword").val("密碼");
}

/*
	Color Picker Select Function
*/
selectColorPickerItem=function(item, id, color) {
	$("#" + item + "_picker > span").each(function() {
		var child = $(this);

		if (child.attr('id') == item + "_picker_" + id) {
			child.addClass("selected");
		}
		else {
			child.removeClass("selected");
		}
	});

	$("#" + item).val(color);
}

FAQSlideToggle=function(id) {
	$("#faq_content_" + id).slideToggle("slow", function () {
		$("#faq_botton_" + id).attr("src", $("#faq_content_" + id).css("display") == "none" ? $("#faq_botton_" + id).attr("src").replace("down", "up") : $("#faq_botton_" + id).attr("src").replace("up", "down"));
	});
	
	return false;
}

email_validate = function (email_val) {
    var search_str = /^[\w\-\.]+@[\w\-\.]+(\.\w+)+$/;
    if (search_str.test(email_val)) {
        return true;
    } else {
        return false;
    }
}


checkPassword = function (val) {
    var strength = 0;

    // length 8 characters or more
    if (val.length >= 8) {
        strength++;
    }

    // contains characters
    //    if (val.match(/[a-z]+/) || val.match(/[A-Z]+/)) {
    //        strength++;
    //    }

    //    // contains digits
    //    if (val.match(/[0-9]+/)) {
    //        strength++;
    //    }

    // contains digits
    if (val.match(/[~\!@#\$%\^&\*\(\)_\+\-\=\[\]\\\{\}\|;'\:",\.\/<>\?]+/)) {
        strength++;
    }

    return (strength == 2 ? true : false);
}


$().ready(function () {

    jQuery.fn.ForceNumericOnly = function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;
                // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
                return (
                key == 8 ||
                key == 9 ||
                key == 46 ||
                (key >= 37 && key <= 40) ||
                (key >= 48 && key <= 57) ||
                (key >= 96 && key <= 105));
            });
        });
    };

});

function ValidateDate(dtValue) {
    try { $.datepicker.parseDate('yy-mm-dd', dtValue); return true; }
    catch (e) { return false; }
}

function checkdevice() {
    var deviceAgent = navigator.userAgent.toLowerCase();
    var agentID = deviceAgent.match(/(iphone|ipod|ipad)/);
    if (agentID) {
        return false
    }
    return true
}
