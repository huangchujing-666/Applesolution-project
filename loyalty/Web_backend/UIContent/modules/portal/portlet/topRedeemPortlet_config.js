{success: true, fields: [
            {
                name: 'gift',
                type: 'string'
            },
            {
                name: 'count',
                type: 'int'
            }
], 
    url:'../UIContent/modules/portal/portlet/topRedeemPortlet_data.js', 
    series: [{
        type: 'bar',
        dataIndex: 'count',
        name: 'Redeem'
    }],
    xField: 'gift',
    chartConfig: {
        chart: {
                type: 'bar'
            /*
            marginRight: 130,
            marginBottom: 120
            */
        },
        title: {
                text: 'Top 5 Gifts Redeemed in Jan 15'
        },
        plotOptions: {
                bar: {
                    dataLabels: {
                            enabled: true
                    },
                    showInLegend: false
                }
        },
        xAxis: {
                category: ['Africa', 'America', 'Asia', 'Europe', 'Oceania'],
                title: {
                text: null
                }
        },
        yAxis: {
                min: 0,
                title: {
                text: 'Quantity',
                align: 'high'
                },
            labels: {
                    overflow: 'justify'
            }
        },
        tooltip: {
                valueSuffix: ' qty'
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
