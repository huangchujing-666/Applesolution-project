Ext.define('com.palmary.wifiaccessreport.js.index', { 
    gridPanel:undefined,
    addTag: function () {
    
        new com.embraiz.tag().openNewTag('location:401', 'Location:Add', 'com.palmary.WifiLocation.js.insert', 'iconUser16', 'user:add');
	},
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        // chart_div
        var chart_div = document.createElement("div");
        //chart_div.style.backgroundColor = "red";
        tab.getEl().dom.lastChild.appendChild(chart_div);
     
        // target_div
		target_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div);
	    grider_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div);

	    target_div2 = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div2);
	    grider_div2 = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div2);

	    var wifiAccessChart = Ext.create('com.palmary.wifiaccessreport.js.chart', {
	        title: 'Wifi Access Chart',
	        iconCls: 'iconGrid',
	        linkUrl: 'com.palmary.portlet.salesPortlet',
	        linkType: 'grid',
	        pId: "6",
	        //  isClose: true,
	        closable: false,

	        padding: '5px 7px 5px 7px',

	        urlLink: 'com.palmary.portlet.salesPortlet'
	    });

	    wifiAccessChart.render(chart_div);

	    Ext.Ajax.request(
            {
                url: "../table/InitWithSearchColumn/WifiAccessReportByLocation",
		 	    async:true,
		 	    success: function(o){
	    	    var data_json = Ext.decode(o.responseText);
	    	    new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json,true);	 	    	 
	        },
		    scope: this
            });

	    Ext.Ajax.request(
            {
                url: "../table/InitWithSearchColumn/WifiAccessReportByMemberLevel",
                async: true,
                success: function (o) {
                    var data_json = Ext.decode(o.responseText);
                    new com.embraiz.component.gridSearch().render(target_div2, grider_div2, data_json, true);
                },
                scope: this
            });

	    var itemList = [{
	        items: []
	    }, {
	        items: []
	    }
	    ];
        
	   
    }

});
