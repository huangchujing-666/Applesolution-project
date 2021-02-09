Ext.define('com.palmary.transactiongroup.js.index', {
    
    gridPanel: undefined,
    addTag: function () {
        new com.embraiz.tag().openNewTag('transactionGroup:401', 'Transaction Group:Add', 'com.palmary.transactiongroup.js.insert', 'iconUser16', 'tg:add');
    },
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        // list
        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);
        
        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/transactiongroup",
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json, true);
            },
            scope: this
        });       
    }
});