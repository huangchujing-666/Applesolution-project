Ext.define('com.palmary.portlet.piePortlet', {
    extend: 'com.palmary.app.portalChartPanel',
    alias: 'widget.piePortlet',
    initComponent: function () {
        var me = this;
        Ext.Ajax.request({
            url: '../UIContent/modules/portal/portlet/pie_portlet_config.js',
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                me.setConfig_data(data_json);
            }
        });
        me.setLegend({ position: 'bottom', itemSpacing: 5 });
        me.setTheme('Base:gradients');
        me.setSeries([{
            type: 'pie',
            field: me.getConfig_data().field,
            showInLegend: true,
            donut: false,
            tips: {
                trackMouse: true,
                width: 140,
                height: 28,
                renderer: function (storeItem, item) {
                    var total = 0;
                    me.items.items[0].store.each(function (rec) {
                        total += rec.get(me.getConfig_data().field);
                    });
                    this.setTitle(storeItem.get(me.getConfig_data().field2) + ': ' + Math.round(storeItem.get(me.getConfig_data().field) / total * 100) + '%');
                }
            },
            highlight: {
                segment: {
                    margin: 20
                }
            },
            label: {
                field: me.getConfig_data().field2,
                display: 'rotate',
                contrast: true,
                font: '18px Arial'
            }
        }]);
        this.callParent(arguments);
    }
});