Ext.define('com.embraiz.barChart.js.index', {

    initTag: function (target_div, data_url, title) {

        var store = Ext.create('Ext.data.Store', {
            fields: ['name', 'value'],//me.json_data.fields,  
            proxy: {
                type: 'ajax',
                url: data_url,
                async: false,
                reader: {
                    type: 'json',
                    root: 'data'
                }
            },
            autoLoad: true
        });



        var chart = Ext.create('Ext.chart.Chart', {
            xtype: 'chart',
            style: 'background:#fff',
            animate: true,
            shadow: true,
            store: store,
            axes: [{
                type: 'Numeric',
                position: 'left',
                fields: ['value'],//
                label: {
                    renderer: Ext.util.Format.numberRenderer('0,0')
                },
                title: 'Number of Hits',//
                grid: true,
                minimum: 0
            }, {
                type: 'Category',
                position: 'bottom',
                fields: ['name'],
                title: 'Month of the Year'//
            }],
            series: [{
                type: 'column',
                axis: 'left',
                highlight: true,
                tips: {
                    trackMouse: true,
                    width: 140,
                    height: 28,
                    renderer: function (storeItem, item) {
                        this.setTitle(storeItem.get('name') + ': ' + storeItem.get('value') + ' $');
                    }
                },
                label: {
                    display: 'insideEnd',
                    'text-anchor': 'middle',
                    field: 'value',//
                    renderer: Ext.util.Format.numberRenderer('0'),
                    orientation: 'vertical',
                    color: '#333'
                },
                xField: 'name',//
                yField: 'value'//
            }]
        });

        Ext.create('widget.panel', {
            width: 300,//
            height: 300,//
            margin: '5',
            title: title,//
            layout: 'fit',
            items: chart,
            renderTo: target_div
        });


    }

});