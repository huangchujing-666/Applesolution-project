Ext.define('com.palmary.demo1.js.index', {
    gridPanel: undefined,
    addTag: function () {
        new com.embraiz.tag().openNewTag('demo1:401', 'demo1:Add', 'com.palmary.demo1.js.insert', 'iconUser16', 'user:add');
    },
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        // List (Building Div)
        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);

        // List Data
        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/demo1",
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json, true);
            },
            scope: this
        });
    }
});
