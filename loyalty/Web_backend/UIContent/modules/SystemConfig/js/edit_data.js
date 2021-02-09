Ext.define('com.palmary.systemconfig.js.edit', {
    gridPanel: undefined,
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();


    },
    init_pop_up: function (winform, id, itemsId, itemName, refreshUrl) {

        // Check user seesion 
        checkSession();

        winform.setHeight(350);
        winform.setWidth(750);

        var id = id.substring(id.indexOf(':') + 1, id.length);

        Ext.Ajax.request({
            url: "../SystemConfig/EditPopForm/" + id,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.form().editForm(winform, data_json, null, refreshUrl, 'remarkList:' + itemsId, itemName);
            },
            scope: this
        });
    }
});