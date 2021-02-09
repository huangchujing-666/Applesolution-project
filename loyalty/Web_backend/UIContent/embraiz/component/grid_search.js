Ext.require([
    'Ext.grid.*',
    'Ext.data.*'
]);

//Ext.onReady(function() {
		
		Ext.define('com.embraiz.component.gridSearch', {
			grid_panel:undefined,			
			constructor : function(config){
				config = config || {};
				Ext.apply(this,config);

				this.callParent([config]);
			},
			render:function(target_div, grider_div, json_data, is_search,module,viewId){		
				
				
				 Ext.Ajax.request({
			 		url: json_data.post_header,
			 		params:{viewid:viewId},
			 		async:false,
			 		success: showGrid,
			   	    scope: this
		 		});	
		 		
				 function showGrid(o) {

				     //---set synchronous loading on this one to avoid problems with rendering
				     Ext.apply(Ext.data.Connection.prototype, {
				         async: false
				     });

		 			var gird_info = Ext.decode(o.responseText);		 			
		 			//add button in grid
		 			this.add_hidden=false;
		 			if(gird_info.add_hidden!=null){
		 				this.add_hidden=gird_info.add_hidden;
		 			}	
		 			
		 			this.grid_panel = Ext.create('com.embraiz.component.gird',{
		 				grider_div: grider_div,
		 				json_data: gird_info,
		 				header_str:Ext.encode(gird_info.columns),		 				
		 				grid_url: json_data.post_url,
		 				form_div:target_div
		 			});	
		 			new com.embraiz.roleManagement.js.index().initTag(this.grid_panel.grid_toolbar,module);
		 			var tabs=Ext.getCmp('content-panel');
		 			var currenttab=tabs.getActiveTab();
				    var addMap=new Ext.util.KeyMap(currenttab.id,{
				        key:119,
				        handler:gird_info.add_url
				    });				
				    if(json_data.row!=undefined&&json_data.row!=null&&json_data.row!=""){
				        var forms = new com.embraiz.component.form();

				       

				        forms.editForm(target_div, json_data, this.grid_panel);

				      
				    }

				     //---restore async property to the default value
				    Ext.apply(Ext.data.Connection.prototype, {
				        async: true
				    });
			}	
		}
		});
//});					 