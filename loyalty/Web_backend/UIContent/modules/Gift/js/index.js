Ext.define('com.palmary.gift.js.index', { 
    gridPanel:undefined,
    addTag  : function(){
        new com.embraiz.tag().openNewTag('Gift:401', 'Gift:Add', 'com.palmary.gift.js.insert', 'iconUser16', 'user:add');
	},
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        // Gift List (Building Div)
		 target_div=document.createElement("div");
	     tab.getEl().dom.lastChild.appendChild(target_div);
	     grider_div=document.createElement("div");
	     tab.getEl().dom.lastChild.appendChild(grider_div);

        // Gift List
	     Ext.Ajax.request({
	         url: "../table/InitWithSearchColumn/gift",
		 		async:true,
		 		success: function(o){
	    	 var data_json = Ext.decode(o.responseText);
	    	 new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json,true);	 	    	 
	        },
		   	scope: this
	 		});	  
	   }
});
