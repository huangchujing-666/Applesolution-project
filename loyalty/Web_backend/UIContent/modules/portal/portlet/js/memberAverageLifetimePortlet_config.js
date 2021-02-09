{   success: true, 
    fields: [
            {
                name: 'month',
                type: 'string'
            },
            {
                name: 'normal',
                type: 'int'
            },
            {
                name: 'silver',
                type: 'int'
            },
            {
                name: 'gold',
                type: 'int'
            },
            {
                name: 'platinum',
                type: 'int'
            }
            //,
            //{
            //    name: 'platinum_downgrade',
            //    type: 'int'
            //}
    ], 
    url: '../UIContent/modules/portal/portlet/js/memberAverageLifetimePortlet_data.js', 
    series: [
         {         
             dataIndex: 'normal',
             name: 'Normal'
         },
        {         
            dataIndex: 'silver',
            name: 'Silver'
        }, {
            dataIndex: 'gold',
            name: 'Gold'
        },
        {
            dataIndex: 'platinum',
            name: 'Platinum'
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
                text: 'Member Average Lifetime'
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
                //min: 0,
                title: {
                    text: 'Average Lifetime(Day)',
               // align: 'high'
                }
            //labels: {
            //        overflow: 'justify'
            //}
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
