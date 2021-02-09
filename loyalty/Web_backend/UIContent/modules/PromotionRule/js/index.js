Ext.define('com.palmary.promotionrule.js.index', {
    gridPanel: undefined,
    addTag: function () {
        
        new com.embraiz.tag().openNewTag('promotionrule:401', 'PromotionRule:Add', 'com.palmary.promotionrule.js.insert', 'iconUser16', 'promotionrule:add');
    },
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        // Rule List (Building Div)
		target_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div);
	    grider_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div);

        // Rule List
	    Ext.Ajax.request({
	         url: "../table/InitWithSearchColumn/promotionrule",
		 		async:true,
		 		success: function(o){
	    	 var data_json = Ext.decode(o.responseText);
	    	 new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json,true);	 	    	 
	        },
		   	scope: this
	 	});	  
	}
});
