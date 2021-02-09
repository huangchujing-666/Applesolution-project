Ext.Loader.setConfig({
    enabled: true,
    disableCaching: true, // For debug only
    paths: {
        'Chart': '../UIContent'
    }
});

Ext.require('Chart.ux.Highcharts');
Ext.require('Chart.ux.Highcharts.Serie');
Ext.require('Chart.ux.Highcharts.AreaRangeSerie');
Ext.require('Chart.ux.Highcharts.AreaSerie');
Ext.require('Chart.ux.Highcharts.AreaSplineRangeSerie');
Ext.require('Chart.ux.Highcharts.AreaSplineSerie');
Ext.require('Chart.ux.Highcharts.BarSerie');
Ext.require('Chart.ux.Highcharts.BoxPlotSerie');
Ext.require('Chart.ux.Highcharts.BubbleSerie');
Ext.require('Chart.ux.Highcharts.ColumnRangeSerie');
Ext.require('Chart.ux.Highcharts.ColumnSerie');
Ext.require('Chart.ux.Highcharts.ErrorBarSerie');
Ext.require('Chart.ux.Highcharts.FunnelSerie');
Ext.require('Chart.ux.Highcharts.GaugeSerie');
Ext.require('Chart.ux.Highcharts.LineSerie');
Ext.require('Chart.ux.Highcharts.PieSerie');
Ext.require('Chart.ux.Highcharts.RangeSerie');
Ext.require('Chart.ux.Highcharts.ScatterSerie');
Ext.require('Chart.ux.Highcharts.SplineSerie');
Ext.require('Chart.ux.Highcharts.WaterfallSerie');

Ext.define('com.palmary.dashboard.js.index', {
    initTag: function (tab) {

        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        target_div.style.margin = "5px";

        /*data list quick link*/
        target_div5 = document.createElement("div");
        //target_div5.style.cssFloat="left";
        target_div5.className = 'sub_div';
        target_div5.style.width = "300";
        target_div5.style.height = "300";
        target_div.appendChild(target_div5);
        Ext.Ajax.request({
            url: '../Dashboard/json_grid_index_quick_link',
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div5, target_div5, data_json, true);
            },
            scope: this
        });


        /*pie chart*/
        target_div1 = document.createElement("div");
        target_div1.className = 'sub_div';
        target_div.appendChild(target_div1);
        new com.embraiz.pieChart.js.index().initTag(target_div1, '../Dashboard/json_list_data_pie', 'Pie Chart');

        /* bar chart*/
        target_div2 = document.createElement("div");
        target_div2.className = 'sub_div';
        target_div.appendChild(target_div2);
        new com.embraiz.barChart.js.index().initTag(target_div2, '../Dashboard/json_list_data_pie', 'Bar Chart');




        /*data list*/
        target_div3 = document.createElement("div");
        target_div3.className = 'sub_div';
        target_div3.style.width = "300";
        target_div3.style.height = "300";
        target_div.appendChild(target_div3);
        Ext.Ajax.request({
            url: '../Dashboard/json_grid_index',
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div3, target_div3, data_json, true);
            },
            scope: this
        });


        /*data list*/
        target_div4 = document.createElement("div");
        target_div4.className = 'sub_div';
        target_div4.style.width = "300";
        target_div4.style.height = "300";
        target_div.appendChild(target_div4);
        Ext.Ajax.request({
            url: '../Dashboard/json_grid_index',
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div4, target_div4, data_json, true);
            },
            scope: this
        });


        /*highchart pie chart*/
        target_div6 = document.createElement("div");
        target_div6.className = 'sub_div';
        target_div.appendChild(target_div6);
        new com.palmary.pieChart.js.index().initTag(target_div6, '../Dashboard/json_list_data_pie', 'Member Distributionn');



    }
});