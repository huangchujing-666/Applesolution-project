Ext.define('com.palmary.remindertemplate.js.index', {
    gridPanel: undefined,
    addTag: function () {
        
        new com.embraiz.tag().openNewTag('ReminderTemplate:Add', 'ReminderTemplate:Add', 'com.palmary.remindertemplate.js.insert', 'iconUser16', 'remindertemplate:add');
    },
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        // List (Building Div)
		target_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div);
	    grider_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div);

        // List
	    Ext.Ajax.request({
	        url: "../table/InitWithSearchColumn/ReminderTemplate",
		 		async:true,
		 		success: function(o){
	    	 var data_json = Ext.decode(o.responseText);
	    	 new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json,true);	 	    	 
	        },
		   	scope: this
	 	});
	}
});