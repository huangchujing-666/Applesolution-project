Ext.define('com.palmary.portlet.memberDistributionReportPortlet', {
    extend: 'com.palmary.app.portalHighChartPanel',
    alias: 'widget.memberDistributionPortlet',
    chartHeight: 400,
    initComponent: function () {
        var me = this;
        
        Ext.Ajax.request({
            url: '../UIContent/modules/portal/portlet/memberDistributionPortlet_config.js',
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                me.setConfig_data(data_json);
            }
        });
        this.callParent(arguments);
    }
});