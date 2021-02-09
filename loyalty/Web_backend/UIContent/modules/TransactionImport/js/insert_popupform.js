Ext.define('com.palmary.transactionimport.js.insert_popupform', {
    init_pop_up: function (winform, id, itemId, itemName, refreshUrl) {

        // Check user seesion 
        checkSession();

        winform.setHeight(350);
        winform.setWidth(750);

        var _id = id.substring(id.indexOf(':') + 1, id.length);

        Ext.Ajax.request({
            url: '../TransactionImport/GenerateForm/' + _id,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.form().editForm(winform, data_json, null, 'com.palmary.transactionimport.js.index', 'remarkList:' + itemId, itemName);
            },
            scope: this
        });
    }
});