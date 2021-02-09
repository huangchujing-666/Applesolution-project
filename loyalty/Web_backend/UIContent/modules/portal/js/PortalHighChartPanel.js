Ext.define('com.palmary.app.portalHighChartPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.highchartportlet',
    config: {
        config_data: '',
        axes: [],
        series: [],
        theme: 'Base',
        xField: '',
        legend: {},
        gradients: [],
        background: {},
        insetPadding: 60
    },
    chartHeight: null,
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

        var chart = Ext.create("Chart.ux.Highcharts", {
            series: me.getConfig_data().series,
            xField: me.getConfig_data().xField,
            store: chartStore,
            chartConfig: me.getConfig_data().chartConfig
        });

        var theHeight = 0;
        
        if (this.chartHeight != undefined)
            theHeight = this.chartHeight;
        else
            theHeight = 300; //default
       
        Ext.apply(this, {
            layout: 'fit', height: theHeight, //width:800,
            items: chart
        });
        this.callParent(arguments);
    }
});