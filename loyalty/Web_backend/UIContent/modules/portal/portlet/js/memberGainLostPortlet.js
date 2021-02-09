Ext.define('com.palmary.portlet.memberGainLostPortlet', {
    extend: 'com.palmary.app.portalHighChartPanel',
    alias: 'widget.memberGainLostPortlet',
    initComponent: function () {
        var me = this;
     
        Ext.Ajax.request({
            url: '../UIContent/modules/portal/portlet/js/memberGainLostPortlet_config.js',
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                me.setConfig_data(data_json);
            }
        });
        this.callParent(arguments);
    }
});