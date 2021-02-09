Ext.define('com.palmary.portlet.topRedeemReportPortlet', {
    extend: 'com.palmary.app.portalHighChartPanel',
    alias: 'widget.topRedeemReportPortlet',
    initComponent: function () {
        var me = this;

        Ext.Ajax.request({
            url: '../UIContent/modules/portal/portlet/js/topRedeemReportPortlet_config.js',
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                me.setConfig_data(data_json);
            }
        });
        this.callParent(arguments);
    }
});