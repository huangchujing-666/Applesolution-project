Ext.define('com.palmary.giftLocation.js.edit', {
	gridPanel:undefined,
	initTag : function (tab,url,title,id,itemId){

	    // Check user seesion 
	    checkSession();

	    tool_div = document.createElement("div");
	    tool_div.style.height = 27;
	    id = id.substring(id.indexOf(':') + 1, id.length);
	    tool_div.id = "tool_div" + id;
	    tab.getEl().dom.lastChild.appendChild(tool_div);
	    target_div = document.createElement("div");
	    target_div.setAttribute("tabindex", "0");
	    tab.getEl().dom.lastChild.appendChild(target_div);
	    grider_div = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div);	
	
        var form = new com.embraiz.component.form();
        form.viewForm(target_div, '../giftLocation/getModule/' + id);
        form.showToolBar(tool_div, '../giftLocation/toolbarData/' + id, id);

        var form1 = new com.embraiz.component.form();
        form1.viewForm(grider_div, '../location/getModule/' + itemId);
        //form.showToolBar(tool_div, '../giftLocation/toolbarData/' + id, id);
	    
	}
});