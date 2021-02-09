Ext.define('com.palmary.product.js.insert', {
	gridPanel:undefined,
	initTag : function (tab,url,title){
	target_div=document.createElement("div");
	tab.getEl().dom.lastChild.appendChild(target_div);
	grider_div=document.createElement("div");
	tab.getEl().dom.lastChild.appendChild(grider_div);	

	Ext.Ajax.request({
	    url:'../product/insert',
	    success:function(o){
	        var data_json = Ext.decode(o.responseText);
	        new com.embraiz.component.form().editForm(target_div, data_json,null);
	    },
	    scope:this
	});
	}
});
