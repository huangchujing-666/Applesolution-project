Ext.define('com.embraiz.common.js.insert',{
	module_name:undefined,
	initTag : function (tab,url,title,module,module_name,extra){
		target_div=document.createElement("div");
		tab.getEl().dom.lastChild.appendChild(target_div);
		grider_div=document.createElement("div");
		tab.getEl().dom.lastChild.appendChild(grider_div);
		if(!extra){
			extra={};
		}
		extra.module_name=module_name;
		Ext.Ajax.request({
		    url:'common/insertJson.jsp',
			params:extra,
		    success:function(o){
		       var data_json=Ext.decode(o.responseText);
		       new com.embraiz.component.form().editForm(target_div, data_json,null);
		    },
		    scope:this
		});
	}
});
