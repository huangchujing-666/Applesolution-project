Ext.define('com.palmary.membercategory.js.index', { 
    gridPanel:undefined,
    addTag  : function(){
        new com.embraiz.tag().openNewTag('MemberCategory:401', 'Member Category:Add', 'com.palmary.memberCategory.js.insert', 'iconUser16', 'user:add');
	},
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        var category = new com.embraiz.component.category();
        category.initCategory(
        'Member Category', // title
        '../MemberCategory/Tree',//tree
        '../MemberCategory/getModule',// view edit detail
        '../MemberCategory/TreeMoveUpdate', //move_update
        '../MemberCategory/Insert', //add
         '../MemberCategory/Delete' //delete
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
