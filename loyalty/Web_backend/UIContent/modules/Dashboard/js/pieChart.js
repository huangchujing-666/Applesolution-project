Ext.define('com.embraiz.pieChart.js.index', {

    initTag: function (target_div, data_url, title) {

        /*store1 = Ext.create('Ext.data.JsonStore', {
            fields: ['name', 'data1'],
            data: [
                   {'name':'aa', 'data1':'10'},
                   {'name':'bb', 'data1':'10'},
                   {'name':'cc', 'data1':'10'},
                   {'name':'dd', 'data1':'10'},
                   {'name':'ee', 'data1':'10'}
                   ]
        });*/
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
            animate: true,
            store: store,
            shadow: true,
            legend: {
                position: 'right'
            },
            insetPadding: 20,
            theme: 'Base:gradients',
            series: [{
                type: 'pie',
                field: 'value',//
                showInLegend: true,
                donut: false,
                tips: {
                    trackMouse: true,
                    width: 140,
                    height: 28,
                    renderer: function (storeItem, item) {
                        //calculate percentage.
                        var total = 0;
                        store.each(function (rec) {
                            total += rec.get('value');
                        });
                        this.setTitle(storeItem.get('name') + ': ' + Math.round(storeItem.get('value') / total * 100) + '%');
                    }
                },
                highlight: {
                    segment: {
                        margin: 20
                    }
                },
                label: {
                    field: 'name',
                    display: 'rotate',
                    contrast: true,
                    font: '18px Arial'
                }
            }]
        });


        Ext.create('widget.panel', {
            width: 300,
            height: 300,
            margin: '5',
            title: title,//
            layout: 'fit',
            items: chart,
            renderTo: target_div
        });

    }



});