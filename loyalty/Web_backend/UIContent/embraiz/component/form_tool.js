Ext.define('com.embraiz.component.form_tool',{
	gen_form_div:function (tab){
		target_div = document.createElement("div");
        target_div.setAttribute("tabindex", "0");
        tab.getEl().dom.lastChild.appendChild(target_div);
        return target_div;
	},
	genTool:function (tab,id){
		tool_div = document.createElement("div");
//		tool_div.style.height=27;
        id = id.substring(id.indexOf(':') + 1, id.length);
        tool_div.id = "tool_div" + id;
        tab.getEl().dom.lastChild.appendChild(tool_div);
        return tool_div;
	}
});