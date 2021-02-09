Ext.define('com.palmary.portlet.company', {
    extend: 'com.palmary.app.portalGridPanel',
    alias: 'widget.companyPortlet',
    initComponent: function () {
        var me = this;
        Ext.Ajax.request({
            url: '../UIContent/modules/portal/portlet/company.js',
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                me.setConfig_data(data_json);
            }
        });
        this.callParent(arguments);
    }
});