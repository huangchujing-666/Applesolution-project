Ext.define('com.palmary.TransactionDetailRedemption.js.index', {
    gridPanel: undefined,
    addTag: function () {
        //new com.embraiz.tag().openNewTag('location:401', 'Location:Add', 'com.palmary.giftredemption.js.insert', 'iconUser16', 'user:add');
    },
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        //member_id = id.substring(id.indexOf(':') + 1, id.length);

        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);

        var transaction_id = id.substring(id.indexOf(':') + 1, id.length);

        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/transactiondetailredemption/" + transaction_id,
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json, true);
            },
            scope: this
        });
    }
});