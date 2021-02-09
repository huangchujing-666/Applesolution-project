Ext.define('com.embraiz.listconfig.js.index',{ 
    gridPanel:undefined,
    addTag  : function(){
    new com.embraiz.tag().openNewTag('listconfig:401','List Config:Add','com.embraiz.listconfig.js.insert','iconListConfig','List Config:add');
	},
    initTag:function (tab,url,title,id,itemId){
		var rightI;
		var rightU;
		var rightE;
		Ext.Ajax.request({
	    	   url:'../ListConfig/Index',
	       		success:function(o){
	    	   		var result=Ext.decode(o.responseText);
	    	   		rightI=result.rightI;
	    	   		rightE=result.rightE;
	    	   		rightU=result.rightU;//获取到它是"textfield":"displayfield";
					var form_tool=new com.embraiz.component.form_tool();
					var target_div=form_tool.gen_form_div(tab);	
					target_div.style.margin = "5px";
					var grider_div=form_tool.gen_form_div(tab);
					grider_div.style.margin = "5px";
					var comid=null;
			 		var states = Ext.create('Ext.data.Store', {
			 			fields: [{name:'value',type:'string'}, {name:'name',type:'string'}],
			 			proxy: {
			 			type: 'ajax',
			 			url : '../listConfig/ListData',
			 			reader: {
			 				type: 'json',
			 				root:'data'           
			 				},
			 				sorters: {
			 					property: 'value',
			 					direction: 'DESC'
			 				}
			 			},
			 			autoLoad:true
			 		});
			 		var combo=Ext.create('Ext.form.ComboBox', {
						    fieldLabel: 'List Item',
						    store: states,
						    queryMode: 'local',
						    width:400,
						    forceSelection:true,
						    displayField: 'name',
						    margins: '20 5 0 0',
						    emptyText: 'Please Select',
						    valueField: 'value',
						    listeners:{
							    change:function(){
			 						comid=this.getValue(),
				 					gridstate.load({
							 			    params:{
				 								itemid:this.getValue(),
							 			        start:0,    
							 			        limit: 100
							 			    }
							 			});					
				 				},
			 					focus:function(){
				 					states.load();
				 				}
			 				}
					       });
				        var panel = Ext.create("Ext.form.Panel", {
				            title: 'Item Detail',
				            height: 100,
				            iconCls: 'iconListConfig',
				            renderTo: target_div,
							layout: {
							    type: 'hbox',
							    pack: 'center'
							},
				            items:[combo,
				            	{
				                    xtype: 'button',
				                    text: 'Add',
				                    width:50,
				                    iconCls: 'iconAdd',
				                    height:21,
				                    margins: '20 5 0 0',
				                    handler:function(){
						            	Ext.Msg.prompt('Name', 'Please enter your name:', function(btn, text){
						            	    if (btn == 'ok'){
						            	       Ext.Ajax.request({
						            	    	   url:'../listConfig/insertData',
						            	    	   params:{
						            	    	   		itemname:text
						            	       },
						            	       		success:function(o){
						            	    	   		var result=Ext.decode(o.responseText);
						            	    	   		combo.store.load();
						            	    	   		combo.setRawValue(result.itemname);
						            	    	   		combo.setValue(result.itemid);
						            	    	   		combo.onFocus();
						            	    	   		
						            	       }
						            	       })
						            	    }
						            	});
			
				            }
				            },{
					            	xtype: 'button',
				                    text: 'Edit',
				                    width:50,
				                    iconCls: 'iconRole16',
				                    height:21,
				                    margins: '20 5 0 0',
				                    handler:function(){
				            			if(combo.getValue()==null){
				            				Ext.Msg.alert('Warming', 'Please select a value!');
				            			}else{
							            	Ext.Msg.prompt('Change Name', 'Please enter your new name:', function(btn, text){
							            	    if (btn == 'ok'){	
							            	       Ext.Ajax.request({
							            	    	   url:'../listConfig/insertData',
							            	    	   params:{
							            	    	   		itemid:combo.getValue(),
							            	    	   		itemname:text
							            	       },
							            	       		success:function(o){
							            	    	   		var result=Ext.decode(o.responseText);
							            	    	   		combo.store.load();
							            	    	   		combo.setRawValue(result.itemname);
							            	    	   		combo.setValue(result.itemid);
							            	    	   		combo.onFocus();
							            	       }
							            	       })
							            	    }
							            	});
				            			}
				            }
				            }]
				        });
				 		var gridstate = Ext.create('Ext.data.Store', {
				 			pageSize:100,
				 			fields: [
				 			         {name:'id',type:'string'},
				 			         {name:'name',type:'string'}, 
				 			         {name:'namecn',type:'string'},
				 			         {name:'sorting',type:'string'}
				 			         ],
				 			proxy: {
				 			type: 'ajax',
				 			url : '../listConfig/listItemData',
				 			reader: {
				 				type: 'json',
				 				root:'data' ,
				 				totalProperty:'totalCount'
				 				},
				 				sorters: {
				 					property: 'value',
				 					direction: 'DESC'
				 				}
				 			},
				 			autoLoad:true
				 		});
				 		gridstate.on('beforeload', function(thiz,options) {
				 			thiz.proxy.extraParams.itemid = combo.getValue();
				 			});
				 		var gridpanel=Ext.create('Ext.grid.Panel', {
				 		    title: 'Item List',
				 		    store: gridstate,
				 		    columns: [
								{header: 'ID',  dataIndex: 'id',field:rightU,width:200},
				 		        {header: 'Name',  dataIndex: 'name', field: rightU,width:355},
				 		        {header: 'Name(CN)',  dataIndex: 'namecn', field: rightU,width:300},
				 		        {header: 'Sorting', dataIndex: 'sorting',field: rightU,width:200}
				 		    ],
				 		    selType: 'cellmodel',
				 		    plugins: [
				 		        Ext.create('Ext.grid.plugin.CellEditing', {
				 		            clicksToEdit: 1
				 		        })
				 		    ],
				 		    dockedItems: [
				 		    {
					 		    xtype: 'toolbar',
					 		    dock: 'top',
					 		    items: [
					 		          {   xtype: 'button', 
					 		        	  text: 'Row',
					 		        	  iconCls: 'iconAdd',
					 		        	  handler:function(){ 
					 		        	  			 var size=gridstate.data.getCount();
					 		        	  			 if(combo.getValue()!=null&&combo.getValue()!=""){
								 		        	 gridstate.insert(size,"","","");
								 		        	 gridstate.totalCount+=1;
					 		        	  			 }
					 		          }
					 		        }
				 		    ]
					 		},
				 		    {
				 		        xtype: 'pagingtoolbar',
				 		        store: gridstate,
				 		        dock: 'bottom',
				                displayMsg: 'Displaying topics {0} - {1} of {2}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;',
				                emptyMsg: "No topics to display",
				                items: ['-'],
				 		        displayInfo: true
				 		    }],
				 		    height:500,
				 		    renderTo: grider_div
				 		});
				 		gridpanel.on('edit', onEdit, this); 
				 	    function onEdit(o,ed){
				 	    	var index=ed.colIdx;
				 	    	var oldvalue=ed.originalValue;
				 	    	var id=ed.record.data.id;
				 	    	var value=ed.value;
				 	    	var name=ed.field;
				 	    	if(id==""&&value==""){
				 	    	}else{
				 	    		if(oldvalue!=value){
				 	    		Ext.Ajax.request({
				 	    			url:'../listConfig/insertListData',
				 	    			params:{
				 	    			id:id,
				 	    			value:value,
				 	    			name:name,
				 	    			item:combo.getValue()
				 	    		},
				 	    		success:function(o){
				 	    			var result=Ext.decode(o.responseText);
				 	    			if(result.flag==1){
				 	    				ed.record.set("id",result.listid);
				 	    			}		
				 	    		}
				 	    		});
				 	    		}
				 	    	}
				 	    } 
				 	   if(rightE){
				 	    	panel.items.items[2].destroy();
				 	   }
				 	   if(rightI){
				 	    	panel.items.items[1].destroy();
				 	   }
				 	   
			}
		})
	   }
})