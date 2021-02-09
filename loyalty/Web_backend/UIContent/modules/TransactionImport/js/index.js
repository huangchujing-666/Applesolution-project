Ext.define('com.palmary.transactionimport.js.index', {
    gridPanel: undefined,
    addTag: function () {
        new com.embraiz.tag().open_pop_up('MI:' + 0, 'Member Import', 'com.palmary.transactionimport.js.insert_popupform', 'iconRole16', 'iconRole16', 'iconRole16', '0');
    },
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);
        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/transactionimport",
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json, true);
            },
            scope: this
        });
    }

});
