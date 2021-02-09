Ext.define('com.palmary.portlet.payment', {
    extend: 'com.palmary.app.portalGridPanel',
    alias: 'widget.paymentPortlet',
    initComponent: function () {
        var me = this;
        Ext.Ajax.request({
            url: '../UIContent/modules/portal/portlet/payment.js',
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                me.setConfig_data(data_json);
            }
        });
        this.callParent(arguments);
    }
});