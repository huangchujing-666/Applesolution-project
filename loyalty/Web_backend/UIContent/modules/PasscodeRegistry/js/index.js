Ext.define('com.palmary.passcoderegistry.js.index', {
    gridPanel: undefined,
    
    initTag: function (tab, url, title, id, itemId) {
        
        // Check user seesion 
        checkSession();

        id = id.substring(id.indexOf(':') + 1, id.length);
        
        transaction_earn_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(transaction_earn_div);
        transaction_earn_grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(transaction_earn_grider_div);

        target_div_pr = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div_pr);
        grider_div_pr = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div_pr);

        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/transactionearn/" + id,
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(transaction_earn_div, transaction_earn_grider_div, data_json, true);
            },
            scope: this
        });

        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/passcodeRegistry/" + id,
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div_pr, grider_div_pr, data_json, true);
            },
            scope: this
        });
    },

    addTag: function (classname, title) {

        new com.embraiz.tag().openNewTag('passcoderegistry:401', 'PasscodeRegistry:Add', 'com.palmary.passcoderegistry.js.insert', 'PasscodeRegistry:Add', 'iconUser16', 'iconUser16', this.abc);
    }
});