{success: true, fields: 
[
            {
                name: 'month',
                type: 'string'
            },
            {
                name: 'Prepaid',
                type: 'int'
            },
            {
                name: 'Addon',
                type: 'int'
            },
            {
                name: 'Product',
                type: 'int'
            },
            {
                name: 'Service',
                type: 'int'
            }
], 
    url:'../UIContent/modules/portal/portlet/js/salesReportPortlet_data.js', 
series: [{                
                dataIndex: 'Service',
                name: 'Service'
            }, {
                dataIndex: 'Prepaid',
                name: 'Prepaid'
            },{
                dataIndex: 'Addon',
                name: 'Addon'
            }, {
                dataIndex: 'Product',
                name: 'Product'
            }],
xField: 'month',
chartConfig: {
                chart: {
                    /*
                    marginRight: 130,
                    marginBottom: 120
                    */
                },
                title: {
                    text: 'Sales Statistic Report'
                },
                xAxis: {
                    title: {
                        text: null
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Sales Amount',
                        align: 'high'
                    },
                    labels: {
                        overflow: 'justify'
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                tooltip: {
                    valueSuffix: ''
                },
                plotOptions: {
                    line: {
                        connectNulls: false
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
}
