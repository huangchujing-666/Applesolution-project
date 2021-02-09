Ext.define('com.palmary.ProductOrder.js.edit', {
    init_pop_up: function (winform, id, itemsId, itemName, refreshUrl) {

        // Check user seesion 
        checkSession();

        winform.setHeight(250);
        winform.setWidth(750);

        var pid = id.substring(id.indexOf(':') + 1, id.length);
        var cid = itemsId;
        
        Ext.Ajax.request({
            url: '../ProductOrder/EditForm/' + pid,
            params: { category_id: cid },
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.form().editForm(winform, data_json, null, refreshUrl, pid, cid);
            },
            scope: this
        });
    }
});