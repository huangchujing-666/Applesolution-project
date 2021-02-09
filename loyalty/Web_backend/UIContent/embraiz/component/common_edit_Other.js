Ext.define('com.embraiz.common.js.editOther',{
	gridPanel:undefined,
	
	init_pop_up : function(winform,id,param){

//	var parent_module_name =id.substring(0,id.indexOf(':'));
	
	Ext.Ajax.request({
	    url:'common/editOtherJson.jsp?module_name='+param,
	    params:{
		    id:id
		},
	    success:function(o){
	        var data_json = Ext.decode(o.responseText);
	        new com.embraiz.component.form().editForm(winform, data_json,null,'',id,param);
	    },
	    scope:this
	});
   }
	
		
})