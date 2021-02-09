Ext.define('com.palmary.WifiLocation.js.edit', {
    gridPanel: undefined,
	initTag: function (tab, url, title, id) {

	    // Check user seesion 
	    checkSession();

	    var form_tool = new com.embraiz.component.form_tool();
	    var tool_div = form_tool.genTool(tab, id);
	    var target_div = form_tool.gen_form_div(tab);
	    var _id = id.substring(id.indexOf(':') + 1, id.length);

	    // div, wifi location promote
	    target_div_wp = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div_wp);
	    grider_div_wp = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div_wp);

        // form
	    var form = new com.embraiz.component.form();
	    form.viewForm(target_div, '../WifiLocation/EditView/' + _id);
	    form.showToolBar(tool_div, '../WifiLocation/EditView_toolbarData/' + _id, id);

	    // list, wifi location promote
	    Ext.Ajax.request({
	        url: "../table/InitWithSearchColumn/wifilocationpromote/" + _id,
	        async: false,
	        success: function (o) {
	            var data_json = Ext.decode(o.responseText);
	            new com.embraiz.component.gridSearch().render(target_div_wp, grider_div_wp, data_json, true);
	        },
	        scope: this
	    });
	}
});