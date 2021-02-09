{   success: true, 
    fields: [
            {
                name: 'month',
                type: 'string'
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
            },
            {
                name: 'normal_downgrade',
                type: 'int'
            },
            {
                name: 'silver_downgrade',
                type: 'int'
            },
            {
                name: 'gold_downgrade',
                type: 'int'
            }
            //,
            //{
            //    name: 'platinum_downgrade',
            //    type: 'int'
            //}
    ], 
    url: '../UIContent/modules/portal/portlet/js/memberUpgradePortlet_data.js', 
    series: [
         {         
             dataIndex: 'normal_downgrade',
             name: 'Normal(downgrade to)',
             stack: "normal_group"
         },
        {         
            dataIndex: 'silver_downgrade',
            name: 'Silver(downgrade to)',
            stack: "silver_group"
        }, {
            dataIndex: 'gold_downgrade',
            name: 'Gold(downgrade to)',
            stack: "gold_group"
        },
        //{
        //    dataIndex: 'platinum_downgrade',
        //    name: 'Platinum(downgrade)',
        //    stack: "platinum_group"
        //},
        {         
            dataIndex: 'silver',
            name: 'Silver',
            stack: "silver_group"
        }, {
            dataIndex: 'gold',
            name: 'Gold',
            stack: "gold_group"
        },
        {
            dataIndex: 'platinum',
            name: 'Platinum',
            stack: "platinum_group"
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
                text: 'Member Upgrade/Downgrade'
        },
        plotOptions: {
                column: {
                    stacking: 'normal'
                }
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
