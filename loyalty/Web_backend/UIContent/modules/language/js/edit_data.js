Ext.define('com.embraiz.language.js.edit', {
	gridPanel:undefined,
	initTag : function (tab,url,title,id,itemId){
	
	    // Check user seesion 
	    checkSession();

	    var form_tool=new com.embraiz.component.form_tool();
	    var tool_div=form_tool.genTool(tab,id);
	    var target_div=form_tool.gen_form_div(tab);	
	    id=id.substring(id.indexOf(':')+1,id.length);
	
        var form = new com.embraiz.component.form();
        form.viewForm(target_div, '../language/getModule/'+ id);
        form.showToolBar(tool_div, '../language/toolbarData/' + id, id);	
	}
});