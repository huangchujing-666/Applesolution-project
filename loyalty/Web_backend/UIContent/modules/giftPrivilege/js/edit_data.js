Ext.define('com.palmary.giftPrivilege.js.edit', {
	gridPanel:undefined,
	initTag : function (tab,url,title,id,itemId){

	    // Check user seesion 
	    checkSession();

	    var toolDiv = document.createElement("div");
	    toolDiv.style.height = 27;
	    var _id = id.substring(id.indexOf(':') + 1, id.length);
	    toolDiv.id = "tool_div" + id;
	    tab.getEl().dom.lastChild.appendChild(toolDiv);
	    var targetDiv = document.createElement("div");
	    targetDiv.setAttribute("tabindex", "0");
	    tab.getEl().dom.lastChild.appendChild(targetDiv);
	    var griderDiv = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(griderDiv);	
	
        var form = new com.embraiz.component.form();
        form.viewForm(targetDiv, '../giftPrivilege/getModule/' + _id);
        form.showToolBar(toolDiv, '../giftPrivilege/toolbarData/' + _id, id);

        var form1 = new com.embraiz.component.form();
        form1.viewForm(griderDiv, '../memberLevel/getModule/' + itemId);
        //form.showToolBar(tool_div, '../giftLocation/toolbarData/' + id, id);
	}
});