Ext.define('com.palmary.membercardhistory.js.index', {
    gridPanel:undefined,
    addTag  : function(){
        new com.embraiz.tag().openNewTag('Member Level:401', 'Member Card:Add', 'com.palmary.membercard.js.insert', 'iconUser16', 'user:add');
	},
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

		target_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div);
	    grider_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div);
	    Ext.Ajax.request({
	        url: "../table/InitWithSearchColumn/membercardhistory",
		 	async:true,
		 	success: function(o){
	    	var data_json = Ext.decode(o.responseText);
	    	new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json,true);	 	    	 
	    },
		scope: this
	 	});	  
	}
});