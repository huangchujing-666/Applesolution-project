Ext.define('com.palmary.pieChart.js.index', {

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
        /*
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
        */
        var store = Ext.create('Ext.data.JsonStore', {
            fields: [
            {
                name : 'type',
                type : 'string'
            }, 
            {
                name : 'count',
                type : 'int'
            }
            ],

            data: [
                { "type": "Normal", "count": 7500 },
                { "type": "No.1", "count": 2700 },
                { "type": "Platinum", "count": 3500 }
            ]

        });

        var chart = Ext.create("Chart.ux.Highcharts", {
            series: [{
                type: 'pie',
                categorieField: 'type',
                dataField: 'count',
                name: 'Number of Member'
            }],
            store: store,
            chartConfig: {
                chart: {
/*
                    marginRight: 130,
                    marginBottom: 120
*/
                },
                title: {
                    text: 'Member Distribution'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            distance: -30,
                            enabled: true,
                            color: '#FFFFFF',
                            connectorColor: '#FFFFFF',
                            format: '<b>{point.percentage:.1f} %'
                        },
/*
                        dataLabels: {
                            enabled: false
                        },
*/
                        showInLegend: true
                    }
                },
/*
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'top',
                    x: -10,
                    y: 100,
                    borderWidth: 0
                },
*/
                credits: {
                    text: 'TrueLoyalty',
                    href: '#'
                }
            }
        });

        Ext.create('widget.panel', {
            width: 300,
            height: 300,
            margin: '5',
            title: title, //
            layout: 'fit',
            items: chart,
            renderTo: target_div
        });

    }



});