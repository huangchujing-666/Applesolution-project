Ext.define('com.palmary.portlet.pointPortlet', {
    extend: 'com.palmary.app.portalHighChartPanel',
    alias: 'widget.pointPortlet',
    initComponent: function () {
        var me = this;
     
        Ext.Ajax.request({
            url: '../UIContent/modules/portal/portlet/js/pointPortlet_config.js',
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                me.setConfig_data(data_json);
            }
        });
        this.callParent(arguments);
    }
});