Ext.define('com.palmary.portlet.history', {
    extend: 'com.palmary.app.portalGridPanel',
    alias: 'widget.historyPortlet',
    initComponent: function () {
        var me = this;
        Ext.Ajax.request({
            url: '../UIContent/modules/portal/portlet/history.js',
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                me.setConfig_data(data_json);
            }
        });
        this.callParent(arguments);
    },
    colRender: function (val) {
        return "<img src='" + val + "' />";
    },
    getHistoryHref: function (id, title, module, icon, name) {
        var href = "";
        var showText = title + ":" + name;
        var test = showText;
        if (showText.length > 15) {
            showText = showText.substring(0, 15) + "..";
        }
        var h_url = "com.palmary." + module + ".js.edit";
        if (Ext.ClassManager.isCreated(h_url))
            new com.embraiz.tag().openNewTag(id, test, h_url, icon, icon, icon, test.substring(0, String(test).indexOf(":")));
        else
            new com.embraiz.tag().openNewTag(id, test, 'com.palmary.common.js.edit', icon, icon, icon, test.substring(0, String(test).indexOf(':')));
    }
});