Ext.define('com.palmary.productXXXXXXXX.js.edit', {
	gridPanel:undefined,
	initTag : function (tab,url,title,id,itemId){

	    tool_div = document.createElement("div");
	    tool_div.style.height = 27;
	    id = id.substring(id.indexOf(':') + 1, id.length);
	    tool_div.id = "tool_div" + id;
	    tab.getEl().dom.lastChild.appendChild(tool_div); //toolbar

	    target_div = document.createElement("div");
	    target_div.setAttribute("tabindex", "0");
	    tab.getEl().dom.lastChild.appendChild(target_div);

	    grider_div = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div);
	    grider_div1 = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div1);
	    grider_div2 = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div2);
	    grider_div3 = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div3);

	    var form = new com.embraiz.component.form();
	    form.viewForm(target_div, '../product/getModule/' + id);
	    form.showToolBar(tool_div, '../product/toolbarData/' + id, id);	

	    Ext.Ajax.request({
	        url: '../ProductPhoto/GetEditForm/' + id,
	        success: function (o) {
	            var data_json = Ext.decode(o.responseText);
	            new com.embraiz.component.form().editForm(grider_div1, data_json, null);
	        },
	        scope: this
	    });

	    var dataJosn = new Object();

	    dataJosn.post_url = '../Common/ListData/ProductPrefix/' + id;
	    dataJosn.post_header = '../ProductPrefix/GridHeader/' + id;
	    new com.embraiz.component.gridSearch().render(target_div, grider_div2, dataJosn, true);
	

	
	}
});