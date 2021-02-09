Ext.define('com.embraiz.common.js.insertOther',{
	gridPanel:undefined,
	
	init_pop_up : function(winform,id,param,extra){

	var parent_module_name =id.substring(0,id.indexOf(':'));
	if(!extra){
		extra={};
	}
	extra.module_name=param;
	extra.id=id;
	Ext.Ajax.request({
	    url:'common/inserOtherJson.jsp',
	    params:extra,
	    success:function(o){
	        var data_json = Ext.decode(o.responseText);
	        new com.embraiz.component.form().editForm(winform, data_json,null,'com.embraiz.common.js.edit',id,parent_module_name);
	    },
	    scope:this
	});
   }
	
		
})