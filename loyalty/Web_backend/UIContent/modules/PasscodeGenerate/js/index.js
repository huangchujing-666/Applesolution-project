Ext.define('com.palmary.passcodegenerate.js.index', {
    gridPanel: undefined,
    addTag: function () {
        new com.embraiz.tag().openNewTag('passcode:401', 'Product:Add', 'com.palmary.passcode.js.insert', 'iconUser16', 'product:add');
    },
    initTag: function (tab, url, title, id, itemId) {
       
        // Check user seesion 
        checkSession();

        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);

        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);

        // Passcode Prefix
        grider_div2 = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div2);
        grider_div3 = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div3);

        // Passcode Generate
        var grider_div_pg1 = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div_pg1);
        var grider_div_pg2 = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div_pg2);

        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/passcodeformat",
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json, true);
            },
            scope: this
        });

        // Passcode Prefix
        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/passcodeprefix",
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(grider_div2, grider_div3, data_json, true);
            },
            scope: this
        });

        // Passcode Generate
        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/passcodegenerate",
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(grider_div_pg1, grider_div_pg2, data_json, true);
            },
            scope: this
        });
    }

});
