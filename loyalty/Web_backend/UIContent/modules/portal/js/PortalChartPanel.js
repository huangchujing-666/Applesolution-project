Ext.define('com.palmary.app.portalChartPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.chartportlet',
    config: {
        config_data: '',
        axes: [],
        series: [],
        theme: 'Base',
        legend: {},
        gradients: [],
        background: {},
        insetPadding: 60
    },
    contructor: function (config) {
        var me = this;
        me.initConfig(config);
        this.callParent(arguments);
        return me;
    },
    initComponent: function () {
        var me = this;
        Ext.define('chartModel', {
            extend: 'Ext.data.Model',
            fields: me.getConfig_data().fields
        });
        var chartStore = Ext.create('Ext.data.Store', {
            model: 'chartModel',
            proxy: {
                type: 'ajax',
                url: me.getConfig_data().url,
                reader: {
                    type: 'json',
                    root: 'items'
                }
            },
            autoLoad: true
        });
        Ext.apply(this, {
            layout: 'fit', height: 300,
            items: {
                xtype: 'chart',
                animate: true,
                shadow: true,
                store: chartStore,
                legend: me.getLegend(),
                //insetPadding:me.getConfig_data().isPieChart==true?me.getInsetPadding():undefined,
                axes: me.getAxes(),
                series: me.getSeries(),
                gradients: me.getGradients(),
                theme: me.getConfig_data().isColumnChart == true ? me.getTheme() : undefined,
                background: me.getConfig_data().isColumnChart == true ? me.getBackground() : undefined
                //background:me.getBackground()
            }
        });
        this.callParent(arguments);
    }
});