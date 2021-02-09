Ext.define('com.palmary.transactionfield.js.addform', {
    init_pop_up: function (winform, id, itemId, itemName, refreshUrl) {

        // Check user seesion 
        checkSession();

        winform.setHeight(350);
        winform.setWidth(750);

        var _id = id.substring(id.indexOf(':') + 1, id.length);

        Ext.Ajax.request({
            url: '../TransactionField/AddForm/',
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.form().editForm(winform, data_json, null, refreshUrl, 'remarkList:' + itemId, itemName);
            },
            scope: this
        });
    }
});