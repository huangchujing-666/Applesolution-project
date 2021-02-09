Ext.define('com.palmary.systemconfig.js.index', {
    gridPanel: undefined,
    //addTag: function () {
    //    new com.embraiz.tag().openNewTag('systemconfig:401', 'Product:Add', 'com.palmary.passcode.js.insert', 'iconUser16', 'product:add');
    //},
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        // SystemConfig List (Building Div)
        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);

        // SystemConfig List
        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/systemconfig/",
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json, true);
            },
            scope: this
        });

    }

});
