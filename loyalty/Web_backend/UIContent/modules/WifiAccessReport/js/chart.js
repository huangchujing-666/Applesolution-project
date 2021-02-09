Ext.define('com.palmary.wifiaccessreport.js.chart', {
    extend: 'com.palmary.app.portalHighChartPanel',
    alias: 'widget.salesPortlet',
    initComponent: function () {
        var me = this;

        Ext.Ajax.request({
            url: '../WifiLocation/ChartConfig', //'../WifiLocation/ChartConfig', //'../UIContent/modules/portal/portlet/salesPortlet_config.js',
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                me.setConfig_data(data_json);
            }
        });
        this.callParent(arguments);
    }
});