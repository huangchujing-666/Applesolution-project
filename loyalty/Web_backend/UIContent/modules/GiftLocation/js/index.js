Ext.define('com.palmary.giftlocation.js.index', { 
    gridPanel:undefined,
    addTag  : function(id){
        new com.embraiz.tag().openNewTag('giftLocation:'+id, 'Gift Location:Add', 'com.palmary.giftLocation.js.insert', 'iconUser16', 'Gift Location:add');
	},
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

		target_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div);
	    grider_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div);
	    Ext.Ajax.request({
	        url: "../common/InitWithSearchColumn/giftLocation",
		 	async:true,
		 	success: function(o){
	    	var data_json = Ext.decode(o.responseText);
	    	new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json,true);	 	    	 
	    },
		scope: this
	 	});	  
	}
});
