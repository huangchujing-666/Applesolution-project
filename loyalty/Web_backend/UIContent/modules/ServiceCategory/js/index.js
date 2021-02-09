Ext.define('com.palmary.servicecategory.js.index', { 
    gridPanel:undefined,
    addTag  : function(){
        new com.embraiz.tag().openNewTag('GiftCategory:401', 'Gift Category:Add', 'com.palmary.giftCategory.js.insert', 'iconUser16', 'user:add');
	},
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();


        var category = new com.embraiz.component.category();
        category.initCategory(
        'Service Category', //title
        '../ServiceCategory/Tree',//tree
        '../ServiceCategory/getModule',// view edit detail
        '../ServiceCategory/TreeMoveUpdate', //move_update
        '../ServiceCategory/Insert'//add
        );

		//target_div=document.createElement("div");
	    //tab.getEl().dom.lastChild.appendChild(target_div);
	    //grider_div=document.createElement("div");
	    //tab.getEl().dom.lastChild.appendChild(grider_div);
	    //Ext.Ajax.request({
	    //    url: "../table/InitWithSearchColumn/giftCategory",
		// 	async:true,
		// 	success: function(o){
	    //	var data_json = Ext.decode(o.responseText);
	    //	new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json,true);	 	    	 
	    //},
		//scope: this
	 	//});	  
	}
});
