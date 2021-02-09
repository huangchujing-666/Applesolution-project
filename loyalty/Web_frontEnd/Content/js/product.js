getProductPageInfo = function (id, category) {
    hasClick = true;

    if (id != 0 && category != 0) {

        $.ajax({
            type: 'POST',
            url: 'Load',
            data: {
                category: category,
                product: id
            },
            success: function (data, status) {
                $('#productcontent').html(data.Content);
                $('#producttitle').html(data.ProductPageInfo.enname + '<br/>' + data.ProductPageInfo.chiname);
            },
            error: function (xhr, desc, err) {
                alert("產品資料載入發生錯誤, 請重新更新");
            }
        });

    }

    return false;
}