Ext.define('com.palmary.app.portalTextPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.textportlet',
    config: {
        config_data: ''
    },
    constructor: function (config) {
        var me = this;
        me.initConfig(config);
        this.callParent(arguments);
        return me;
    },
    initComponent: function () {
        var me = this;
        Ext.apply(this, {
            html: '<div class="portlet-content">' + me.config_data + '</div>'
        });
        this.callParent(arguments);
    }
});