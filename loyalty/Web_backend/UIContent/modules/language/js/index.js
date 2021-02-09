Ext.define('com.palmary.language.js.index', {
    gridPanel:undefined,
    //addTag  : function(){
    //    new com.embraiz.tag().openNewTag('user:401', 'User:Add', 'com.embraiz.language.js.insert', 'iconUser16', 'user:add');
	//},
	//copyuser  : function(userId){
	//    new com.embraiz.tag().openNewTag(userId, 'User:Copy', 'com.embraiz.language.js.copyuser', 'iconUser16', 'user:add');
	//},
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

		target_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div);
	    grider_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div);
	    Ext.Ajax.request({
	        url: "../table/InitWithSearchColumn/language",
		 	async:true,
		 	success: function(o){
	    	    var data_json = Ext.decode(o.responseText);
	    	    new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json,true);	 	    	 
	        },
		    scope: this
	    });	  
	}
});
