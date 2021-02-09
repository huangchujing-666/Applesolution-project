Ext.define('com.palmary.membercard.js.edit', {
	gridPanel:undefined,
	initTag : function (tab,url,title,id,itemId){
	
	    // Check user seesion 
	    checkSession();

	    id = id.substring(id.indexOf(':') + 1, id.length);


	    target_div_summary = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div_summary);

	    var form_tool=new com.embraiz.component.form_tool();
	    var tool_div=form_tool.genTool(tab,id);
	    var target_div = form_tool.gen_form_div(tab);

	    // Member Card List
	    target_div_cl = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div_cl);
	    grider_div_cl = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div_cl);
	
	    Ext.Ajax.request({
	        url: '../MemberCard/ViewBasicData/' + id,
	        success: function (o) {
	            var data_json = Ext.decode(o.responseText);
	            new com.embraiz.component.form().editForm(target_div_summary, data_json, null, '', '', '');

	        },
	        scope: this
	    });

        var form = new com.embraiz.component.form();
       // form.viewForm(target_div, '../MemberCard/getModule/' + id);
        form.showToolBar(tool_div, '../MemberCard/toolbarData/' + id, id);

	    // Member Card List
        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/membercard/" + id,
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div_cl, grider_div_cl, data_json, true);
            },
            scope: this
        });
	}
});