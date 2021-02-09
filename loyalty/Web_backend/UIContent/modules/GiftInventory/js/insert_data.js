Ext.define('com.palmary.giftinventory.js.insert', {
    gridPanel: undefined,
    init_pop_up: function (winform, id, itemsId, itemName, refreshUrl) {
        
        // Check user seesion 
        checkSession();

        winform.setHeight(350);
        winform.setWidth(750);

        var _id = id.substring(id.indexOf(':') + 1, id.length);

        Ext.Ajax.request({
            url: '../GiftInventory/Insert/' + _id,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.form().editForm(winform, data_json, null, refreshUrl, 'remarkList:' + itemsId, itemName);
            },
            scope: this
        });
    }
});