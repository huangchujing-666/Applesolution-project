Ext.define('com.palmary.giftinventory.js.index', { 
    gridPanel:undefined,
    addTag: function () {
        
        //new com.embraiz.tag().open_pop_up('', 'Gift Inventory Adjustment', 'com.palmary.giftinventory.js.insert', 'iconHeadOffice', 'iconHeadOffice', 'iconHeadOffice', '', '', '');
	},
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        id = id.substring(id.indexOf(':') + 1, id.length);
        
        //target_div_summary = document.createElement("div");
        //tab.getEl().dom.lastChild.appendChild(target_div_summary);
        //grider_div_summary = document.createElement("div");
        //tab.getEl().dom.lastChild.appendChild(grider_div_summary);
   
        target_div_summary = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div_summary);

        target_div = document.createElement("div");
		tab.getEl().dom.lastChild.appendChild(target_div);
	    grider_div = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div);

	    //Ext.Ajax.request({
	    //    url: "../table/InitWithSearchColumn/giftInventorySummary/" + id,
	    //    async: true,
	    //    success: function (o) {
	    //        var data_json = Ext.decode(o.responseText);
	    //        new com.embraiz.component.gridSearch().render(target_div_summary, grider_div_summary, data_json, true);
	    //    },
	    //    scope: this
	    //});


	    Ext.Ajax.request({
	        url: '../GiftInventory/CoreForm/' + id,
	        success: function (o) {
	            var data_json = Ext.decode(o.responseText);
	            new com.embraiz.component.form().editForm(target_div_summary, data_json, null, '', 'remarkList:' + itemId, '');

	        },
	        scope: this
	    });

	    Ext.Ajax.request({
	        url: "../table/InitWithSearchColumn/giftInventory/" + id,
		 		async:true,
		 		success: function(o){
	    	 var data_json = Ext.decode(o.responseText);
	    	 new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json,true);	 	    	 
	        },
		   	scope: this
	    });
	}
});