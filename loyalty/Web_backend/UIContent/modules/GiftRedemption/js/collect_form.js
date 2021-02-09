Ext.define('com.palmary.giftredemption.collect.js.form', {
    gridPanel: undefined,
    addTag: function () {
        new com.embraiz.tag().openNewTag('location:401', 'Location:Add', 'com.palmary.giftredemption.js.insert', 'iconUser16', 'user:add');
    },
    init_pop_up: function (winform, id, itemsId, itemName, refreshUrl) {

        // Check user seesion 
        checkSession();

        winform.setHeight(350);
        winform.setWidth(750);

        var redemption_id = id.substring(id.indexOf(':') + 1, id.length);

        Ext.Ajax.request({
            url: "../GiftRedemption/Collect/" + redemption_id,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.form().editForm(winform, data_json, null, refreshUrl, 'remarkList:' + itemsId, itemName);
            },
            scope: this
        });
       
    }
});