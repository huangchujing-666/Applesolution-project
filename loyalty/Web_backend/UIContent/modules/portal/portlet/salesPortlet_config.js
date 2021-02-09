{success: true, fields: 
[
            {
                name: 'month',
                type: 'string'
            },
            //{
            //    name: 'Prepaid',
            //    type: 'int'
            //},
            //{
            //    name: 'Addon',
            //    type: 'int'
            //},
            //{
            //    name: 'Product',
            //    type: 'int'
            //},
            {
                name: 'no_of_member',
                type: 'int'
            }
], 
    url:'../UIContent/modules/portal/portlet/memberGrowth_data.js', 
series: [{                
                dataIndex: 'no_of_member',
                name: 'no of member'
}
            //, {
            //    dataIndex: 'Prepaid',
            //    name: 'Prepaid'
            //},{
            //    dataIndex: 'Addon',
            //    name: 'Addon'
            //}, {
            //    dataIndex: 'Product',
            //    name: 'Product'
            //}
],
xField: 'month',
chartConfig: {
                chart: {
                    /*
                    marginRight: 130,
                    marginBottom: 120
                    */
                },
                title: {
                    text: 'Member Growth'
                },
                xAxis: {
                    title: {
                        text: null
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Amount',
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
