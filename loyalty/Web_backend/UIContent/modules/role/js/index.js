//Ext.onReady(function (){
Ext.define('com.palmary.role.js.index', {
    gridPanel: undefined,
    addTag: function () {
        new com.embraiz.tag().openNewTag('role:501', 'Role:Add', 'com.palmary.role.js.insert', 'iconRole16');
    },
    //delRole: function () {
    //    new com.embraiz.tag().openNewTag('role:501', 'Role:Add', 'com.embraiz.role.js.insert', 'iconRole16');
    //},
    initTag: function (tab, url, title, itemId) {

        // Check user seesion 
        checkSession();

        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);

        Ext.Ajax.request({
            url: '../Table/InitWithSearchColumn/Role',
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json, true);
            },
            scope: this
        });
    }
});
