Ext.define('com.palmary.servicepaymentdetail.js.index', {
    gridPanel:undefined,
    addTag: function () {
       // new com.embraiz.tag().openNewTag('Service:401', 'Service:Add', 'com.palmary.service.js.insert', 'iconUser16', 'user:add');
	},
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        // List (Building Div)
		 target_div=document.createElement("div");
	     tab.getEl().dom.lastChild.appendChild(target_div);
	     grider_div=document.createElement("div");
	     tab.getEl().dom.lastChild.appendChild(grider_div);

	     target_div_extra = document.createElement("div");
	     tab.getEl().dom.lastChild.appendChild(target_div_extra);
	     grider_div_extra = document.createElement("div");
	     tab.getEl().dom.lastChild.appendChild(grider_div_extra);

	     var transaction_id = id.substring(id.indexOf(':') + 1, id.length);

        // List
	     Ext.Ajax.request({
	         url: "../table/InitWithSearchColumn/servicepaymentdetail/" + transaction_id,
		 		async:true,
		 		success: function(o){
	    	 var data_json = Ext.decode(o.responseText);
	    	 new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json,true);	 	    	 
	        },
		   	scope: this
	     });


	     Ext.Ajax.request({
	         url: "../table/InitWithSearchColumn/servicepaymentdetailextra/" + transaction_id,
	         async: true,
	         success: function (o) {
	             var data_json = Ext.decode(o.responseText);
	             new com.embraiz.component.gridSearch().render(target_div_extra, grider_div_extra, data_json, true);
	         },
	         scope: this
	     });
	}
});
