{   success: true, 
    fields: [
        {name:'gender', type:'string'},
        {name:'type', type:'string'},
        {name:'count', type:'int'}
    ], 
    url:'../UIContent/modules/portal/portlet/js/memberProfileDistributionPortlet_data.js', 
    series: [
        {
            type: 'pie',
            categorieField: 'gender',
            dataField: 'count', 
            size: '60%',
            name: 'Number of Member',
            dataLabels: {
                formatter: function () {
                    return this.y > 5 ? this.point.name : null;
                },
                color: 'white',
                distance: -30
            },
            totalDataField: true
        },{
            type: 'pie',
            categorieField: 'type',
            dataField: 'count', 
            name: 'Number of Member',
            size: '80%',
            innerSize: '60%',
            dataLabels: {
            formatter: function () {
                // display only if larger than 1
                return this.y > 1 ? '<b>' + this.point.name + ':</b> ' + this.y + '%'  : null;
            }
    }
        }],
    xField: '',
    chartConfig: {
        chart: {
                type: 'pie'
            /*
            marginRight: 130,
            marginBottom: 120
            */
        },
        title: {
                text: 'Member Profile Demographic'
        },
        plotOptions: {
                pie: {
                    shadow: false,
                    center: ['50%', '50%']
        
                    //pie: {
                    //    allowPointSelect: true,
                    //    cursor: 'pointer',
                    //    dataLabels: {
                    //        distance: -30,
                    //        enabled: true,
                    //        color: '#FFFFFF',
                    //        connectorColor: '#FFFFFF',
                    //        format: '<b>{point.percentage:.1f} %'
                    //    },
                    /*
                    dataLabels: {
                    enabled: false
                    },
                    */
                    // showInLegend: true
                }
        },
        tooltip: {
                valueSuffix: '%'
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
