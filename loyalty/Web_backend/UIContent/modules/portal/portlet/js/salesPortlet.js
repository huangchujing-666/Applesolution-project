Ext.define('com.palmary.portlet.salesPortlet', {
    extend: 'com.palmary.app.portalHighChartPanel',
    alias: 'widget.salesPortlet',
    initComponent: function () {
        var me = this;

        Ext.Ajax.request({
            url: '../UIContent/modules/portal/portlet/salesPortlet_config.js',
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                me.setConfig_data(data_json);
            }
        });
        this.callParent(arguments);
    }
});