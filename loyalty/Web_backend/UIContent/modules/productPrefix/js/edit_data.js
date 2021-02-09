Ext.define('com.palmary.productPrefix.js.edit', {
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

//	var form = new com.embraiz.component.form();
//	form.viewForm(target_div, '../productPrefix/getModule/' + id);
//	form.showToolBar(tool_div, '../productPrefix/toolbarData/' + id, id);

	    new com.embraiz.component.girdTable().initGrid('../ProductPasscode/GridHeader/' + id, '../Common/ListData/ProductPasscode/' + id, grider_div);
	    new com.embraiz.component.gridkeyboard().keyboard([grider_div]);
	}
});