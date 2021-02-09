Ext.define('com.palmary.giftPrivilege.js.insert', {
    gridPanel: undefined,
    initTag: function (tab, url, title, id) {

        // Check user seesion 
        checkSession();

        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);
        var idTemp = 0;
        if (id.toString().split(':').length >= 2) {
            idTemp = id.toString().split(":")[1];
        }
        Ext.Ajax.request({
            url: '../giftLocation/insert/'+idTemp,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.form().editForm(target_div, data_json, null);
            },
            scope: this
        });
    }
});