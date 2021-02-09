Ext.define('com.palmary.logdetail.js.index', {
    gridPanel: undefined,
    addTag: function () {

    },
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        var log_id = id.substring(id.indexOf(':') + 1, id.length);

        // summary form
        target_div_summary = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div_summary);

        // detail form
        target_div_detail = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div_detail);


        var form_summary = new com.embraiz.component.form();
        form_summary.viewForm(target_div_summary, '../Log/SummaryViewForm/' + log_id);

        var form_detail = new com.embraiz.component.form();
        form_detail.viewForm(target_div_detail, '../Log/DetailViewForm/' + log_id);
    },
    init_pop_up: function (winform, id, itemId, itemName, refreshUrl) {

        // Check user seesion 
        checkSession();

        //winform.setHeight(250);
        //  winform.setWidth(750);

        var log_id = id.substring(id.indexOf(':') + 1, id.length);

        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);

        Ext.Ajax.request({
            url: '../Log/ViewForm/' + log_id,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.form().editForm(winform, data_json, null, refreshUrl, 'remarkList:' + itemId, itemName);
            },
            scope: this
        });
    }
});