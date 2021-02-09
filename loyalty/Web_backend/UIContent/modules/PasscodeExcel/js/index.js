Ext.define('com.palmary.passcodeexcel.js.index', {
    gridPanel: undefined,
    addTag: function () {
        new com.embraiz.tag().openNewTag('passcode:401', 'Product:Add', 'com.palmary.passcodeexcel.js.insert', 'iconUser16', 'product:add');
    },
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        // passcodeexcel
        passcodeexcel_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(passcodeexcel_div);

        // Passcode Usage
        target_div_usage = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div_usage);
        grider_div_usage = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div_usage);

        // passcodegenerate
        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);

        // passcodeexcel
        Ext.Ajax.request({
            url: '../Table/Init/passcodeexcel',
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.form().editForm(passcodeexcel_div, data_json, null);
            },
            scope: this
        });

        // Passcode Usage
        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/passcodeusagesummary",
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div_usage, grider_div_usage, data_json, true);
            },
            scope: this
        });

        // passcodegenerate
        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/passcodegenerate",
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json, true);
            },
            scope: this
        });  
    }
});
