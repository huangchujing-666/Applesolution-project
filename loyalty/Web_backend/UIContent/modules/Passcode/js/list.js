Ext.define('com.palmary.passcode.js.list', {
    gridPanel: undefined,
    addTag: function () {
        new com.embraiz.tag().openNewTag('product:401', 'Product:Add', 'com.palmary.passcode.js.insert', 'iconUser16', 'product:add');
    },
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        var _id = id.substring(id.indexOf(':') + 1, id.length);

        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);

        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);

        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/passcode/" + _id,
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json, true);
            },
            scope: this
        });
    }
});