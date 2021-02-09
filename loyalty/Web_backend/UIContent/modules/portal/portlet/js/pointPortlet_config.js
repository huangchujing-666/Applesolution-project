{   success: true, 
    fields: [
            {
                name: 'month',
                type: 'string'
            },
            
            {
                name: 'register',
                type: 'int'
            },
            {
                name: 'use',
                type: 'int'
            },
            {
                name: 'expire',
                type: 'int'
            }
    ], 
    url: '../UIContent/modules/portal/portlet/js/pointPortlet_data.js', 
    series: [
         {         
             dataIndex: 'register',
             name: 'Register'
         },
        {         
            dataIndex: 'use',
            name: 'Use'
        }, {
            dataIndex: 'expire',
            name: 'Expire'
        }
    ],
    xField: 'month',
   
    chartConfig: {
        chart: {
                type: 'column'
            /*
            marginRight: 130,
            marginBottom: 120
            */
        },
        title: {
                text: 'Point Report'
        },
        plotOptions: {
                //column: {
                //    stacking: 'normal'
                //}
        },
        //plotOptions: {
         
        //        bar: {
        //            dataLabels: {
        //                    enabled: true
        //            },
        //            showInLegend: false
        //        }
        //},
        //xAxis: {
        //        category: ['Africa', 'America', 'Asia', 'Europe', 'Oceania'],
        //        title: {
        //        text: null
        //        }
        //},
        yAxis: {
            title: {
                    text: 'Point'
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
