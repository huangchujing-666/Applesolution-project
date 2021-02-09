{success: true, fields: [{name:'type', type:'string'},{name:'count', type:'int'}], url:'../UIContent/modules/portal/portlet/memberDistributionPortlet_data.js', 
series: [{
                type: 'pie',
                categorieField: 'type',
                dataField: 'count',
                name: 'Number of Member'
            }],
xField: '',
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
}
