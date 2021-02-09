Ext.define('com.palmary.portlet.auditTrail', {
    extend: 'com.palmary.app.portalGridPanel',
    alias: 'widget.auditTrailPortlet',
    initComponent: function () {
        var me = this;
        Ext.Ajax.request({
            url: '../UIContent/modules/portal/portlet/auditTrail.js',
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
    }
});