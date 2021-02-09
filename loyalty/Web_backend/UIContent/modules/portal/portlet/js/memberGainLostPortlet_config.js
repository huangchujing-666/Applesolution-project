{   success: true, 
    fields: [
            {
                name: 'month',
                type: 'string'
            },
            
            {
                name: 'gain',
                type: 'int'
            },
            {
                name: 'lost',
                type: 'int'
            }
    ], 
    url: '../UIContent/modules/portal/portlet/js/memberGainLostPortlet_data.js', 
    series: [
         {         
             dataIndex: 'gain',
             name: 'Gain'
         },
        {         
            dataIndex: 'lost',
            name: 'Lost'
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
            text: 'Member Gain Lost Report'
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
                    text: 'No of member'
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
