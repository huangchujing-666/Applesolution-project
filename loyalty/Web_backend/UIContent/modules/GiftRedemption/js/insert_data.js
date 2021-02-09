Ext.define('com.palmary.giftredemption.js.insert', {
    gridPanel: undefined,

    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

	    member_id = id.substring(id.indexOf(':') + 1, id.length);

	    target_div = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div);
	    grider_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div);	

	    Ext.Ajax.request({
	        url: '../giftredemption/insert/' + member_id,
	        success:function(o){
	            var data_json = Ext.decode(o.responseText);
	            new com.embraiz.component.form().editForm(target_div, data_json,null);
	        },
	        scope:this
	    });
    },

    init_pop_up: function (winform, id, itemId, itemName, refreshUrl) {
        // Check user seesion 
        checkSession();

        winform.setHeight(550);
        winform.setWidth(750);

        member_id = id.substring(id.indexOf(':') + 1, id.length);

        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);

        Ext.Ajax.request({
            url: '../giftredemption/insert/' + member_id,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.form().editForm(winform, data_json, null, refreshUrl, 'remarkList:' + itemId, itemName); 
            },
            scope: this
        });
    }
});

