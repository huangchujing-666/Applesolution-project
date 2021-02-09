var redeemID;
var redeemGiftName;
var redeemPoint;

function redeemGift(id, gift, point) {
    redeemID = id;
    redeemGiftName = gift;
    redeemPoint = point;

    return false;
}

function RedeemMessageBox(giftID, giftName) {
    //BootstrapDialog.confirm('Hi Apple, are you sure?', function (result) {
    //    if (result) {
    //        alert('Yup.');
    //    } else {
    //        alert('Nope.');
    //    }
    //});
    
    var test = BootstrapDialog.show({
        title: '換購禮品 ',
        message: "是否確定換領禮品: " + giftName + "?",
        type: BootstrapDialog.TYPE_DEFAULT,
        animate: true,
        buttons: [{
            label: '是',
            action: function (dialogRef) {
                
                dialogRef.close();
                $.ajax({
                            type: 'POST',
                            url: '../Club/Redeem',
                            data: {
                                id: giftID
                            },
                            success: function (data, status) {
                                var obj = jQuery.parseJSON(data);

                                if (obj.result == 1) {
                                    window.location = 'Gift_Redeem_Success';
                                }
                                else {
                                    if (obj.result == -1) {
                                        alert("未有選擇要換購的禮品。");
                                    }
                                    else if (obj.result == -2) {
                                        alert("抱歉，您未有足夠分數換購此禮品。");
                                    }
                                    else if (obj.result == -3) {
                                        alert("選擇的禮品已經換罄。");
                                    }
                                    else if (obj.result == -4) {
                                        alert("會員登入閒置時間太長, 請重新登入");
                                        window.location = "Logout";
                                    }
                                    else if (obj.result == -5) {
                                        alert(obj.msg);
                                    }

                                    $("#btn_submit").removeAttr('disabled');
                                }
                            },
                            error: function (xhr, desc, err) {
                                alert("換購禮品提交發生錯誤, 請重新提交");
                                $("#btn_submit").removeAttr('disabled');
                            }
                        });

            }
        }, {
            label: '否',
            action: function (dialogRef) {
                
                dialogRef.close();
            }
        }],
        onshown: function (dialogRef) {
            // fix for conflict with Eco template
            // reset z-index
            $(".modal-backdrop.fade.in").css("z-index", "0");
        }
    });
    
    //test.updateZIndex();
}

$().ready(function () {
   
    //$(".confirm").easyconfirm({ locale: { title: '換購禮品', text: '是否確定要換領本禮品?', button: ['否', '是']} });

    //$(".confirm").click(function () {
    //    $.ajax({
    //        type: 'POST',
    //        url: '../Club/Redeem',
    //        data: {
    //            id: redeemID
    //        },
    //        success: function (data, status) {
    //            var obj = jQuery.parseJSON(data);

    //            if (obj.result == 1) {
    //                window.location = 'Gift_Redeem_Success';
    //            }
    //            else {
    //                if (obj.result == -1) {
    //                    alert("未有選擇要換購的禮品。");
    //                }
    //                else if (obj.result == -2) {
    //                    alert("抱歉，您未有足夠分數換購此禮品。");
    //                }
    //                else if (obj.result == -3) {
    //                    alert("選擇的禮品已經換罄。");
    //                }
    //                else if (obj.result == -4) {
    //                    alert("會員登入閒置時間太長, 請重新登入");
    //                    window.location = "Logout";
    //                }
    //                else if (obj.result == -5) {
    //                    alert(obj.msg);
    //                }

    //                $("#btn_submit").removeAttr('disabled');
    //            }
    //        },
    //        error: function (xhr, desc, err) {
    //            alert("換購禮品提交發生錯誤, 請重新提交");
    //            $("#btn_submit").removeAttr('disabled');
    //        }
    //    });
    //});

    //    $("#page_top").ForceNumericOnly();
    //    $("#page_bottom").ForceNumericOnly();
});

