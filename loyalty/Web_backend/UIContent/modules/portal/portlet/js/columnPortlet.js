Ext.define('portletColors', {
    config: {
        colors: [
            'url(#v-1)',
            'url(#v-2)',
            'url(#v-3)',
            'url(#v-4)',
            'url(#v-5)'
      ],
        baseColor: "#eee"
    },
    constructor: function (config) {
        var me = this;
        this.initConfig(config);
        return me;
    }

});
Ext.define('Ext.chart.theme.Fancy', {
    extend: 'Ext.chart.theme.Base',
    mixins: { colors: 'portletColors' },
    constructor: function (config) {
        this.callParent([Ext.apply({
            axis: {
                fill: this.getBaseColor(),
                stroke: this.getBaseColor()
            },
            axisLabelLeft: {
                fill: this.getBaseColor()
            },
            axisLabelBottom: {
                fill: this.getBaseColor()
            },
            axisTitleLeft: {
                fill: this.getBaseColor()
            },
            axisTitleBottom: {
                fill: this.getBaseColor()
            },
            colors: this.getColors()
        }, config)]);
    }
});
Ext.define('com.palmary.portlet.columnPortlet', {
    extend: 'com.palmary.app.portalChartPanel',
    alias: 'widget.columnPortlet',
    mixins: { colors: 'portletColors' },
    initComponent: function () {
        var me = this;
        Ext.Ajax.request({
            url: '../UIContent/modules/portal/portlet/column_portlet_config.js',
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                me.setConfig_data(data_json);
            }
        });
        me.setTheme('Fancy');
        me.setBackground({
            fill: 'rgb(17, 17, 17)'
        });
        me.setGradients([
	       {
	           'id': 'v-1',
	           'angle': 0,
	           stops: {
	               0: {
	                   color: 'rgb(212, 40, 40)'
	               },
	               100: {
	                   color: 'rgb(117, 14, 14)'
	               }
	           }
	       },
	       {
	           'id': 'v-2',
	           'angle': 0,
	           stops: {
	               0: {
	                   color: 'rgb(180, 216, 42)'
	               },
	               100: {
	                   color: 'rgb(94, 114, 13)'
	               }
	           }
	       },
	       {
	           'id': 'v-3',
	           'angle': 0,
	           stops: {
	               0: {
	                   color: 'rgb(43, 221, 115)'
	               },
	               100: {
	                   color: 'rgb(14, 117, 56)'
	               }
	           }
	       },
	       {
	           'id': 'v-4',
	           'angle': 0,
	           stops: {
	               0: {
	                   color: 'rgb(45, 117, 226)'
	               },
	               100: {
	                   color: 'rgb(14, 56, 117)'
	               }
	           }
	       },
	       {
	           'id': 'v-5',
	           'angle': 0,
	           stops: {
	               0: {
	                   color: 'rgb(187, 45, 222)'
	               },
	               100: {
	                   color: 'rgb(85, 10, 103)'
	               }
	           }
	       }]);
        me.setAxes([{
            type: 'Numeric',
            position: 'left',
            fields: me.getConfig_data().yField,
            minimum: 0,
            maximum: 100,
            label: {
                renderer: Ext.util.Format.numberRenderer('0,0')
            },
            title: me.getConfig_data().title1,
            grid: {
                odd: {
                    stroke: '#555'
                },
                even: {
                    stroke: '#555'
                }
            }
        }, {
            type: 'Category',
            position: 'bottom',
            fields: me.getConfig_data().xField,
            title: me.getConfig_data().title2
        }]);
        me.setSeries([{
            type: 'column',
            axis: 'left',
            highlight: true,
            label: {
                display: 'insideEnd',
                'text-anchor': 'middle',
                field: me.getConfig_data().yField,
                orientation: 'horizontal',
                fill: '#fff',
                font: '17px Arial'
            },
            renderer: function (sprite, storeItem, barAttr, i, store) {
                barAttr.fill = me.getColors()[i % me.getColors().length];
                return barAttr;
            },
            style: {
                opacity: 0.95
            },
            xField: me.getConfig_data().xField,
            yField: me.getConfig_data().yField,
            title: me.getConfig_data().legendTitle
        }]);
        me.setLegend({ position: 'bottom', itemSpacing: 5 });
        this.callParent(arguments);
    }
});