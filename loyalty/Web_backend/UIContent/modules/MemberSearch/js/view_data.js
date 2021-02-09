Ext.define('com.palmary.membersearch.js.view', {
    gridPanel: undefined,
    initTag: function (tab, url, title, id) {

        // Check user seesion 
        checkSession();

        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);

        id = id.substring(id.indexOf(':') + 1, id.length);

        Ext.Ajax.request({
            url: '../MemberAdvanceSearch/ViewDetail/' + id,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.form().editForm(target_div, data_json, null, '', '', '');

            },
            scope: this
        });
    }
});