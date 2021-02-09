Ext.Loader.setConfig({
    enabled: true
});
Ext.Loader.setPath('Ext.ux', '../UIContent/ux');

Ext.require([
    'Ext.grid.*',
    'Ext.data.*',
    'Ext.util.*',
    'Ext.state.*',
    'Ext.form.*',
    'Ext.ux.CheckColumn'
]);
Ext.define('com.embraiz.component.roweditgird', {
	     store: undefined,
	     grid_data_url:undefined,
	     grid_header_url:undefined,
	     grider_div: undefined,
	     json_data:undefined,
	     rowIndex:0,
	     edit_grid:undefined,
	     extra:undefined,
	     formatDate: function 	(value){
        return value ? Ext.Date.dateFormat(value, 'M-d-Y') : '';
      },
	     constructor: function (config) {
            Ext.tip.QuickTipManager.init();
            config = config || {};
            Ext.apply(this, config);
            this.callParent([config]);            
           },
      render:function(grid_header_url,grid_data_url,grider_div,extras){    //extra  lesson
      	   grider_div.style.margin="5px";
      	   var isEdit=false;
            var me = this;
           var extra=me.extra=extras;
            me.grid_header_url=grid_header_url;
            me.grid_data_url=grid_data_url;
            me.grider_div=grider_div;
             Ext.Ajax.request({
			 		   url: grid_header_url,
			 		   async:false,
			 		   success:showGrid,
			   	      scope: this
		 	  	    });
		 	  	
		 	  	function showGrid(o){
		 	  		var grid_info=me.json_data= Ext.decode(o.responseText);	
		 	  		
		 	  		 Ext.define('editModel', {
			        extend: 'Ext.data.Model',
			        fields: me.json_data.fields
				    });	 	  	
            var store = me.store = Ext.create('Ext.data.Store', {
               // fields: me.json_data.fields,
                model: 'editModel',
                pageSize: me.json_data.pageSize,
                filterOnLoad: false,
                remoteSort: true,                
                proxy: {
                    type: 'ajax',
                    url: grid_data_url,
                    async:false,
                    reader: {
                        type: 'json',
                        root: 'items',
                        totalProperty: 'totalCount'
                    }

                }
            });
            store.on('beforeload',function(thiz,op){
            	if(op.sorters[0]!=undefined){
            	var name=op.sorters[0].property;
            	for(var i=0;i<name.length;i++){
            		if((name.charCodeAt(i)>=65)&&(name.charCodeAt(i)<=90)){
            			name=name.substring(0,i)+'_'+name.substring(i,i+1).toLowerCase()+name.substring(i+1,name.length);
            		}           		
            	}
            	op.sorters[0].property=name;
            	}	
            	
            });
            store.on('load',function(thiz,op){
            	var width=10;
            	for(var i=0;i<grid.columns.length;i++){
            		width+=grid.columns[i].getWidth();
            	}
            	if(Ext.isIE){
            		if(width>grid.getWidth()){
            			var len=75+thiz.data.length*23;
             			len+=grid_toolbar.items.length==0?0:27;
             			var height=len+23;
             			grid.setHeight(height);
            		}
            	}
            })
           var rowEditing = Ext.create('Ext.grid.plugin.RowEditing', {
            clicksToMoveEditor: 1,
            clicksToEdit:2,
            autoCancel: false     
          });
           var bbar = Ext.create('com.embraiz.component.extPaging', {
                    store: me.store,
                    displayInfo: true,
                    displayMsg: 'Displaying topics {0} - {1} of {2}',
                    emptyMsg: "No topics to display",
                    url: me.grid_data_url,
                    params: me.params,
                    items: ['-']
                });
           var grid_toolbar = Ext.create('Ext.toolbar.Toolbar', {
                xtype: 'toolbar'       
            });
            if(!grid_info.add_hidden){
	              grid_toolbar.add({
	                  text: 'Add',
			              iconCls: 'iconAdd',
			               handler : function() {
			            	     if(extra==null||extra==undefined ||extra==''){
			            	    	 rowEditing.cancelEdit();	
						                var r = Ext.create('editModel',{
						                //	grid_info.addfields
						                });			
						                store.insert(0, r);
						                rowEditing.startEdit(0, 0);
			            	     }else{
				            	        if(store.data.items.length>=parseInt(extra)){
				            	        	Ext.Msg.alert('Warning','The course number data is not greater than lesson!');
				            	        }else{
							                rowEditing.cancelEdit();	
							                var r = Ext.create('editModel',{
							                //	grid_info.addfields
							                });			
							                store.insert(0, r);
							                rowEditing.startEdit(0, 0);
						               }
			                       }
			                }
			          });
            }
            if(!grid_info.delete_hidden){
            	 grid_toolbar.add({  
            	 	   itemId:'remove',          	 	    
				            text: 'Remove',
				            iconCls: 'iconRemove',
				            handler: function() {
				            	Ext.MessageBox.confirm('Confirm', 'Are you sure you want to delete?',
							        function(btn) {
							            if (btn == 'yes') {
							            	 var sm = grid.getSelectionModel();				                
							                var ids=sm.selected.items[0].data.id;
							                 rowEditing.cancelEdit();				               
							                   Ext.Ajax.request({        	
											             url: grid_info.delete_url,
											             params: {
											             	id:ids
											             	},
											             success: function(o){ 
												             	var result=Ext.decode(o.responseText);	
												             	 store.remove(sm.getSelection());
												                if (store.getCount() > 0) {
												                    sm.select(0);
												                }									             
												               	Ext.Msg.alert('Status', 'Delete successfully.');												             
											             } 
										           });
											      } else {
											       this.close();
											      }
											 });
				               				                
				               
				               
				            },
				            disabled: true
            	 	});
            	 	new Ext.util.KeyMap(document,{
            	 	   key:120,
            	 	   fn:function(){
            	 	      if(grid.down('#remove').disabled==false){
            	 	      Ext.MessageBox.confirm('Confirm', 'Are you sure you want to delete?',
					        function(btn) {
					            if (btn == 'yes') {
					            	 var sm = grid.getSelectionModel();				                
					                var ids=sm.selected.items[0].data.id;
					                 rowEditing.cancelEdit();				               
					                   Ext.Ajax.request({        	
									             url: grid_info.delete_url,
									             params: {
									             	id:ids
									             	},
									             success: function(o){ 
										             	var result=Ext.decode(o.responseText);	
										             	 store.remove(sm.getSelection());
										                if (store.getCount() > 0) {
										                    sm.select(0);
										                }									             
										               	Ext.Msg.alert('Status', 'Delete successfully.');												             
									             } 
								           });
									      } else {
									       this.close();
									      }
									 });
						}
            	 	   }
            	 	});
            	 	
            	}
            if(grid_info.toolData!=''&&grid_info.toolData!=undefined&&grid_info.toolData!=null){
            	for(var tool=0;tool<grid_info.toolData.length;tool++){
            		   var hidden=false;
            		  if (grid_info.toolData[tool].hidden != undefined) {
                          hidden = grid_info.toolData[tool].hidden;
                      } else {
                          hidden = false;
                      }
            	 grid_toolbar.add({ 
                         xtype: grid_info.toolData[tool].xtype,
                         text: grid_info.toolData[tool].text,
                         iconCls: grid_info.toolData[tool].iconUrl,
                         name: grid_info.toolData[tool].name,
                         hidden: hidden,
                         handler: grid_info.toolData[tool].href,
                         itemId: grid_info.toolData[tool].itemId
         	 	       });
            	}
            }
       var  form_columns=[];
        for (i = 0; i < grid_info.columns.length; i++) {
                    var temp_element = {};
                    var columns_config = grid_info.columns[i];
                    temp_element.header = columns_config.header;
                    temp_element.dataIndex = columns_config.dataIndex;
                    temp_element.width = columns_config.width;
                    temp_element.type = columns_config.type;
                    temp_element.flex = columns_config.flex;

                    temp_element.hidden = columns_config.hidden || columns_config.hidden;
                    if (me.json_data.columns[i].sortable != undefined) {
                        temp_element.sortable = me.json_data.columns[i].sortable;
                    } else {
                        temp_element.sortable = false;
                    }
                    if (columns_config.formula != undefined) {
                        temp_element.renderer = function (value, meta, record) {
                            var forumla_arr = columns_config.formula.split("*");
                            if (forumla_arr.length = 2) {
                                var a = record.get(forumla_arr[0]) * record.get(forumla_arr[1]);
                                return a;
                            } else {
                                forumla_arr = columns_config.formula.split("+");
                                if (forumla_arr.length = 2) {
                                    var a = record.get(forumla_arr[0]) + record.get(forumla_arr[1]);
                                    return a;
                                }
                            }

                        }
                    }                  
                    if(columns_config.type == 'datefield'){
                    	 temp_element.xtype='datecolumn';
                    	 temp_element.format='Y-m-d';
	                    	temp_element.editor = {
	                                xtype: 'datefield',
	                                name:columns_config.dataIndex,
	                                allowBlank: columns_config.allowBlank==undefined?true:columns_config.allowBlank,
					                format: 'Y-m-d',
					                readOnly:columns_config.readOnly==undefined?false:columns_config.readOnly
	                      };
                    }else if(columns_config.type == 'checkbox'){
                    		  temp_element.xtype= 'checkcolumn';						            
							            temp_element.editor={
							                xtype: 'checkbox',
							                name:columns_config.dataIndex,
							                readOnly:columns_config.readOnly==undefined?false:columns_config.readOnly
							             };
                    }else if(columns_config.type == 'currency'){
              		  temp_element.xtype= 'numbercolumn';
              		  temp_element.format= '$0,0.00';
              		  temp_element.editor={
		                xtype: 'numberfield',
		                name:columns_config.dataIndex,
		                readOnly:columns_config.readOnly==undefined?false:columns_config.readOnly
              		  };
			            
                    }else if(columns_config.type == 'numberfield'){
                  		  temp_element.editor=Ext.create('Ext.form.field.Number',{    
                  			name:columns_config.dataIndex,
    		                readOnly:columns_config.readOnly==undefined?false:columns_config.readOnly
                  		  });
                  		if(columns_config.handler!=undefined&&columns_config.handler!=''){
                    	    var eventName='change';
                    	    if(columns_config.eventName){
                    		   eventName=columns_config.eventName;
                    	    }
	                      	temp_element.editor.addListener(eventName,columns_config.handler);
	                   	} 
                    }else if(columns_config.type == 'timefield'){
    			            temp_element.editor={
    			                xtype: 'timefield',
    			                format:'H:i',
				                readOnly:columns_config.readOnly==undefined?false:columns_config.readOnly
    			             };
    			            temp_element.renderer=function(value,metadata,record,rowIndex,colIndex){
    			            	if(value instanceof Date){
    			            		value=Ext.Date.format(value,'H:i');
    			            	}
    			            	return value;
    			            }
                    }else if(columns_config.type == 'combobox'){
                    			    var cellfields = [{
		                                 name: 'id',
		                                 type: 'int'
				                        }, {
				                            name: 'value',
				                            type: 'string'
				                        }];
				                        if (columns_config.fields != undefined) {
				                            cellfields = columns_config.fields;
				                        }
										cellStore = Ext.create('Ext.data.Store', {
				                            fields: cellfields,
				                            autoLoad: true,
				                            proxy: {
				                                type: 'ajax',
				                                url: columns_config.url,
				                                reader: {
				                                    type: 'json',
				                                    root: 'data'
				                                },
				                                sorters: {
				                                    property: 'value',
				                                    direction: 'ASC'
				                                }
				                            }
				                        });
				                        //cellStore.load();
				                        cellStore.on('load', function () {
				                            // solve delay of db data load problem
				                            grid.view.refresh();
				                        });
				                        temp_element.editor = Ext.create('Ext.form.field.ComboBox',{ 
				                        	allowBlank: columns_config.allowBlank==undefined?true:columns_config.allowBlank,	
					                        name:columns_config.dataIndex,
					                        forceSelection:true,
					                        selectOnFocus:true,
					                        plugins: ['clearbutton'],
					                        queryMode: 'local',
					                        displayField: 'value',
					                        valueField: 'id',
					                        store: cellStore,
					                        listeners:{beforequery:listener_ComboboxBeforeQuery},
							                readOnly:columns_config.readOnly==undefined?false:columns_config.readOnly
				                      });
				                        function listener_ComboboxBeforeQuery(e) {
				                        	var combo = e.combo;
				                        	try {
				                        		var value = e.query;
				                        		combo.lastQuery = value;
				                        		combo.store.filterBy(function(record, id) {
				                        			var text = record.get(combo.displayField);
				                        			return (text.toLowerCase().indexOf(value.toLowerCase()) != -1); // ignore upper / lower case.
				                        		});
				                        		combo.onLoad();
				                        		combo.expand();
				                        		return false;
				                        	} catch (e) {
				                        		combo.selectedIndex = -1;
				                        		combo.store.clearFilter();
				                        		combo.onLoad();
				                        		combo.expand();
				                        		return false;
				                        	}
				                        }
				                        if(columns_config.handler!=undefined&&columns_config.handler!=''){
				                        	   var eventName='select';
				                        	   if(columns_config.eventName){
				                        		   eventName=columns_config.eventName;
				                        	   }
					                      	   temp_element.editor.addListener(eventName,columns_config.handler);
					                      	}
				                       temp_element.renderer=function(value,metadata,record,rowIndex,colIndex){
				                    	    var combox=this.columns[colIndex].editor;
				                    	    if(combox==undefined){
				                    	    	combox=this.columns[colIndex].field;
				                    	    }
			                    	    	var index=combox.store.find(combox.valueField,value);
				                    	    var record=combox.store.getAt(index);
				                    	    var displayText = "";
					                        if (record == null) {
					                    	    displayText = value;
				                    	    } else {
				                    		    displayText = record.data.value;
				                    	    }				                    	           
				                    	    return displayText;
				                    	 }  	    				                       
				                    }else if(columns_config.type == 'textfield'){
				                        temp_element.editor = Ext.create('Ext.form.field.Text',{ 
					                    	allowBlank: columns_config.allowBlank==undefined?true:columns_config.allowBlank,
					                        xtype: columns_config.type,
					                        vtype: columns_config.vtype,
					                        typeAhead: columns_config.typeAhead,
					                        name:columns_config.dataIndex,
					                        triggerAction: columns_config.triggerAction,
					                        selectOnTab: columns_config.selectOnTab,
							                readOnly:columns_config.readOnly==undefined?false:columns_config.readOnly	                       
					                      });
				                      if(columns_config.handler!=undefined&&columns_config.handler!=''){
				                    	    var eventName='change';
			                        	    if(columns_config.eventName){
			                        		   eventName=columns_config.eventName;
			                        	    }
					                      	temp_element.editor.addListener(eventName,columns_config.handler);
					                   	}
				                    }else{
				                      temp_element.editor = { 
				                    	allowBlank: columns_config.allowBlank==undefined?true:columns_config.allowBlank,
				                        xtype: columns_config.type,
				                        vtype: columns_config.vtype,
				                        name:columns_config.dataIndex,
				                        typeAhead: columns_config.typeAhead,
				                        triggerAction: columns_config.triggerAction,
				                        selectOnTab: columns_config.selectOnTab,
						                readOnly:columns_config.readOnly==undefined?false:columns_config.readOnly	                       
				                      };			                      
                    }
                    form_columns[i] = temp_element;
                }
            ///////
        var grid =this.edit_grid= Ext.create('Ext.grid.Panel', {
					        store: store,
					        columns: form_columns,
					        renderTo: grider_div,
					        width: '100%',					       
					        title: grid_info.title,
					        frame: true,
					        tbar: grid_toolbar,
					        minHeight:200,
					        plugins: [rowEditing],
					        bbar: bbar,
					        listeners: {
					            'selectionchange': function(view, records) {
					                var remove=grid.down('#remove');
					                if(remove){
					                	remove.setDisabled(!records.length);
					                }
					            },
					            beforerender:function(thiz,op){
					            	if(grid_info.render!=undefined&&grid_info.render!=''){
					            		eval(grid_info.render);
					            	}
					            }
					        }
         });
      store.loadPage(1);    
      grid.on('edit', function(editor,e,options){//add/update
      	var fields=grid_info.fields;
	      if(fields.length>0){
	      	var params='{';
	      	for(i=0;i<fields.length;i++){
	      		 params=params+"'"+fields[i]+"':'"+e.record.data[fields[i]]+"',";
	      		}
	      		 params=params.substring(0,params.length-1)+"}";
	      		if(grid_info.update_url){
	      		   Ext.Ajax.request({        	
		             url: grid_info.update_url,
		             params:e.record.data, //Ext.decode(params),
		             success: function(o){ 
		      			  	var result=Ext.decode(o.responseText);	
			             	if(result.ispass!=false){
				             	if(result.id_value!=null && result.id_value!=undefined){//add
				                	e.record.set("id",result.id_value);
				                	Ext.Msg.alert('Status', result.msg,function(){
				                	   grid.getView().focus();
	                                   grid.getSelectionModel().selectRange(e.rowIdx,e.rowIdx);
				                	});
				              }else{//update
				               	Ext.Msg.alert('Status', result.msg,function(){
				                	  grid.getView().focus();
	                                  grid.getSelectionModel().selectRange(e.rowIdx,e.rowIdx);
				                	});
				              }
			              }else{
			            	 Ext.Msg.alert('Status', result.msg,function(){
				                	grid.getView().focus();
	                                grid.getSelectionModel().selectRange(e.rowIdx,e.rowIdx);   
				                	});
			            	}
		             } 
	           }); 
	      	}
          }          	
      },this);           
      }
      }  
           
     	
	})