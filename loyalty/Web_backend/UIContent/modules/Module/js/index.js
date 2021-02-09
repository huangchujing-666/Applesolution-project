//Ext.onReady(function (){
Ext.define('com.palmary.module.js.index', {
    gridPanel: undefined,
    addTag: function () {
        new com.embraiz.tag().openNewTag('rule:501', 'module:Add', 'com.palmary.module.js.insert', 'iconRole16');
    },
    delRole: function () {
        new com.embraiz.tag().openNewTag('rule:501', 'module:Add', 'com.palmary.module.js.insert', 'iconRole16');
    },
    initTag: function (tab, module, title) {

        // Check user seesion 
        checkSession();

        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);
        Ext.Ajax.request({
            url: '../Module/InitWithSearchColumn/' + module,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.form().editForm(target_div, data_json, null);
            },
            scope: this
        });
    }
});
