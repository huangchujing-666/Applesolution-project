Ext.define('com.palmary.portlet.linePortlet', {
    extend: 'com.palmary.app.portalChartPanel',
    alias: 'widget.linePortlet',
    initComponent: function () {
        var me = this;
        Ext.Ajax.request({
            url: '../UIContent/modules/portal/portlet/line_portlet_config.js',
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                me.setConfig_data(data_json);
            }
        });
        me.setAxes([{
            type: 'Numeric',
            position: 'left',
            fields: me.getConfig_data().yField,
            title: me.getConfig_data().title1,
            label: {
                font: '11px Arial'
            }
        }, {
            type: 'Numeric',
            position: 'right',
            grid: false,
            fields: me.getConfig_data().yField2,
            title: me.getConfig_data().title2,
            label: {
                font: '11px Arial'
            }
        }]);
        me.setSeries([{
            type: 'line',
            lineWidth: 1,
            showMarkers: false,
            fill: true,
            axis: 'left',
            xField: me.getConfig_data().xField,
            yField: me.getConfig_data().yField,
            style: {
                'stroke-width': 1,
                stroke: 'rgb(148, 174, 10)'
            }
        }, {
            type: 'line',
            lineWidth: 1,
            showMarkers: false,
            axis: 'right',
            xField: me.getConfig_data().xField,
            yField: me.getConfig_data().yField2,
            style: {
                'stroke-width': 1,
                stroke: 'rgb(17, 95, 166)'
            }
        }]);
        me.setLegend({ position: 'bottom' });
        this.callParent(arguments);
    }
});