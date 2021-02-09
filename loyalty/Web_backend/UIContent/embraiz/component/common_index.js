Ext.define('com.embraiz.common.js.index',{
	module_name:undefined,
    addTag  : function(classname,title,extra){
		this.module_name=classname;
		var icon_string = 'icon'+classname;
		var class_path = 'com.embraiz.common.js.insert';
	    new com.embraiz.tag().openNewTag(classname,title+' Add',class_path,icon_string,icon_string,icon_string,classname,extra);
	},
	grid:function(tab,id,extra){
		var div1;
		if(!tab.nodeName){
			div1 = document.createElement('div');
			tab.getEl().dom.lastChild.appendChild(div1);
		}else{
			div1=tab;
		}
		var module=extra.className;
		var method=extra.method;
		var cfg=extra.cfg;
		var params=extra.params;
		var data=extra.data;
		var grid = new com.embraiz.component.girdTable()
		.initGrid(
				"common/gird_header.jsp?module_name="+module+"&method="+method+"&cfg="+cfg+"",
				"common/list_grid_data.jsp?class="+module+"&method="+data+"&"+params+"="+id, div1);
	},
	winForm:function(className,title,id,method,parentClass,parentId,parentUrl,height,width,extra){
		if(method==undefined||method==''){
			method='dataJson';
		}
		if(parentClass==undefined||parentClass==''){
			parentClass='';
		}
		if(parentUrl==undefined||parentUrl==''){
			parentUrl='com.embraiz.common.js.edit';
		}
		if(height==undefined||height==''){
			height=400;
		}
		if(width==undefined||width==''){
			width=600;
		}
		if(!extra){
			extra={};
		}
		extra.module_name=className;
		extra.method=method;
		extra.id=id;		
		var win = Ext.create('widget.window', {
			title: title,
			closable: true,
			width: width,
			height: height,
			bodyStyle: 'padding: 5px;',
			modal: true,
			autoScroll:true
		});
		Ext.Ajax.request({
			url:'common/insertJson.jsp',
			params:extra,
			async:false,
		    success:function(o){
		        var data_json = Ext.decode(o.responseText);
		        new com.embraiz.component.form().editForm(win, data_json,null,parentUrl,parentId,parentClass);
		    },
		scope:this
		});
		win.show();	 
		
	},
	winFormEdit:function(className,title,id,method,parentClass,parentId,parentUrl,height,width,extra){
		if(method==undefined||method==''){
			method='dataJson';
		}
		if(parentClass==undefined||parentClass==''){
			parentClass='';
		}
		if(parentUrl==undefined||parentUrl==''){
			parentUrl='com.embraiz.common.js.edit';
		}
		if(height==undefined||height==''){
			height=400;
		}
		if(width==undefined||width==''){
			width=600;
		}
		if(!extra){
			extra={};
		}
		extra.module_name=className;
		extra.method=method;
		extra.id=id;		
		var win = Ext.create('widget.window', {
			title: title,
			closable: true,
			width: width,
			height: height,
			bodyStyle: 'padding: 5px;',
			modal: true,
			autoScroll:true
		});
		Ext.Ajax.request({
			url:'common/editJson.jsp',
			params:extra,
			async:false,
		    success:function(o){
		        var data_json = Ext.decode(o.responseText);
		        new com.embraiz.component.form().editForm(win, data_json,null,parentUrl,parentId,parentClass);
		    },
		scope:this
		});
		win.show();	 
		
	},
	initTag : function (tab,url,title, id, module){
		this.module_name=url;
		target_div=document.createElement("div");
		tab.getEl().dom.lastChild.appendChild(target_div);
		form_div=document.createElement("div");
	    form_div.style.margin = "6px";
	    //form_div.style.width=tab.getWidth()-30;
	    tab.getEl().dom.lastChild.appendChild(form_div);
	    grider_div=document.createElement("div");
		tab.getEl().dom.lastChild.appendChild(grider_div);	
	    var me=this;
		Ext.Ajax.request({
			 url:'common/grid_index.jsp?module_name='+url,
		    success:function(o){
		       var data_json=Ext.decode(o.responseText);
		       new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json,true,module);
		      /* var form=Ext.create('Ext.form.FormPanel',{
	             title:'Bookmark',
	             layout:{
	                type:'table',
	                columns:4
	             },
	             height:100,
	             icon:'./icons/book_open.png',
	             renderTo:form_div,
	             items:[{
	                xtype:'combo',
	                fieldLabel:'Type',
	                name:'type',
	                store:Ext.create('Ext.data.Store',{
	                  fields:['id','value'],
	                  proxy: {
			                type: 'ajax',
			                url: 'common/bookmark/list_type.jsp',
			                reader: {
			                    type: 'json',
			                    root: 'data'
			                }
			          },
			          autoLoad:true
	                }),
	                queryMode:'local',
	                displayField:'value',
	                plugins: ['clearbutton'],
	                allowBlank:false,
	                valueField:'id',
	                triggerAction: 'all',
                    forceSelection: true,          
                    emptyText: 'Please Select..',
                    listeners:{
                       'select':function(combo,record,index){
                          var form=combo.up('form').getForm();
                          var list=form.findField('list');
                          list.clearValue(); 
                          if(combo.value==1)
                             list.store.proxy.url = 'common/bookmark/list_bookmark_data.jsp?module_name='+url;
                          if(combo.value==2)
                             list.store.proxy.url = 'common/bookmark/list_search_condition.jsp?module_name='+url;
                          list.store.load();
                        }
                    }
	             },{
	                xtype:'combo',
	                fieldLabel:'List',
	                //id:'list',
	                name:'list',
	                plugins: ['clearbutton'],
	                store:Ext.create('Ext.data.Store',{
	                   fields:['id','value'],
	                   proxy: {
			                type: 'ajax',
			                url: '',
			                reader: {
			                    type: 'json',
			                    root: 'data'
			                }
			            }
	                }),
	                queryMode:'local',
	                displayField:'value',
	                valueField:'id',
	                allowBlank:false,
	                triggerAction: 'all',
                    forceSelection: true,          
                    emptyText: 'Please Select..'
	             }],
				 dockedItems: [{
				    xtype: 'toolbar',
				    dock: 'bottom',
				    ui: 'footer',
				    items: [{ 
				         xtype: 'button', 
				         text: 'Go' ,
				         icon:'./icons/drive_go.png',
				         handler:function(){
				            if(form.getForm().isValid()){
				               var target=Ext.getCmp("content-panel").getActiveTab().getEl().dom;
				               var i=0;if(Ext.isIE){i=1;}
	                           var search_grid=target.childNodes[i].childNodes[3].childNodes[0];
	                           var grid=Ext.getCmp(search_grid.id);
	                           var search=target.childNodes[i].childNodes[1].childNodes[0];
	                           var search_form=Ext.getCmp(search.id);
	                           var store=grid.getStore();
	                           if(form.getForm().findField('type').getValue()==1){
	                              form.submit({
							          url:'common/bookmark/load_data.jsp?type='+form.getForm().findField('type').getValue()+'&module_name='+url,
							          method:'post',
							          success:function(form, action){
							             var data=Ext.decode(action.response.responseText);
							             if(action.result.success==true){
							                 store.on('beforeload',function(){
							                 });
							                 store.load({
							                    params:{filter:action.result.filter},
							                    callback: function(records, options, success){
											       grid.setLoading(false); 
											    }		                    
							                 });
							             }
							          }
							       });
	                           }else{ 
	                              form.submit({
							          url:'common/bookmark/load_data.jsp?type='+form.getForm().findField('type').getValue()+'&module_name='+url,
							          method:'post',
							          success:function(form, action){
							             var data=Ext.decode(action.response.responseText);
							             if(action.result.success==true){
							                 store.on('beforeload',function(){
							                    //grid.setLoading(true);
							                 });
							                 var filter=action.result.filter;
							                 search_form.getForm().reset();
							                 for(var i=0;i<search_form.getForm().getFields().length;i++){
							                     for(var j=0;j<Ext.decode(filter).length;j++){
							                         if(search_form.getForm().getFields().getAt(i).getName()==Ext.decode(filter)[j].property){
							                            if(Ext.decode(filter)[j].value!=null && Ext.decode(filter)[j].value!='' && Ext.decode(filter)[j].value.indexOf(',')!=-1){
							                               var comboval='';
							                               var str=new Array();
							                               str=Ext.decode(filter)[j].value.split(',');
							                               search_form.getForm().getFields().getAt(i).setValue(str);
							                            }else{
							                               search_form.getForm().getFields().getAt(i).setValue(Ext.decode(filter)[j].value);
							                            }
							                         }
							                     }
							                 }  
							                 store.load({
							                    params:{filter:filter},
							                    callback: function(records, options, success){
											       grid.setLoading(false); 
											    }		                    
							                 });
							             }
							          }
							       });
	                           }
				            }  
				         }
				    },{
				       	xtype: 'button', 
				        text: 'Update Existing' ,
				        icon:'./icons/cog_edit.png',
				        handler:function(){
				          var target=Ext.getCmp("content-panel").getActiveTab().getEl().dom;
				          var i=0;if(Ext.isIE){i=1;}
	                      var search_grid=target.childNodes[i].childNodes[3].childNodes[0];
	                      var grid=Ext.getCmp(search_grid.id);
				          if(form.getForm().isValid()){
				             if(form.getForm().findField('type').getValue()==1){
				                me.update_bookmark(form,grid,url);
				             }
				             else{
				                me.update_condition(form,url);
				             } 
				          }
				        }
				    },{
				        xtype:'button',
				        text:'Save New',
				        icon:'./icons/cog_add.png',
				        handler:function(){
				           var target=Ext.getCmp("content-panel").getActiveTab().getEl().dom;
				           var i=0;if(Ext.isIE){i=1;}
	                       var search_grid=target.childNodes[i].childNodes[3].childNodes[0];
	                       var grid=Ext.getCmp(search_grid.id);
				           if(form.getForm().findField('type').getValue()!=null){
				             if(form.getForm().findField('type').getValue()==1){
				                me.bookmark(form,grid,url);
				             }
				             else{
				                me.searchcon(form,url);
				             }
				           }else{
				              Ext.Msg.alert("msg","Please Select Type First!");
				           }
				        }
				    },{
				        xtype:'button',
				        text:'Delete',
				        icon:'./icons/cog_delete.png',
				        handler:function(){
				          if(form.getForm().isValid()){
				        	Ext.MessageBox.confirm('Confirm', 'Are you sure you want to Delete Bookmark or Search Condition?',
						        function(btn) {
						            if (btn == 'yes') {	
						                Ext.Ajax.request({
						                   url:'common/bookmark/delete.jsp',
						                   params:{type:form.getForm().findField('type').getValue(),id:form.getForm().findField('list').getValue()},
						                   waitMsg:'Waiting...',
						                   success:function(o){
						                     var data=Ext.decode(o.responseText);
						                     if(data.success==true){
						                        Ext.Msg.alert("msg","Delete Success");
						                        form.getForm().findField('type').clearValue();
						                        form.getForm().findField('list').clearValue();
						                     }
						                   }
						                });				 				 
						            }else{
						            	this.close();
						            }
						 });
						 }
				        }
				    }
				    ]
				}]
	     });*/
		    },
		    scope:this
		});
	},
	bookmark:function(book,grid,module_name){
	   //var grid=e.up('gridpanel');
	   //var target=Ext.getCmp("content-panel").getActiveTab().getEl().dom;
	   //var book_form=target.childNodes[0].childNodes[2].childNodes[0];
	   //var book=Ext.getCmp(book_form.id);//alert(book_form.id);
	   var store=grid.getStore();
	   var selModel = null;
       var selectedValue = "";
       if (grid.selModel != null && grid.selModel != undefined) {
           selModel = grid.selModel;
           var selectedRecord = selModel.getSelection();
           for (var i = 0; i < selectedRecord.length; i++) {
           	if(i!=0){
           		 selectedValue = selectedValue + "," + selectedRecord[i].get(Ext.ModelManager.getModel(store.model).getFields()[0].name);
           	}else{
           		selectedValue = selectedValue + selectedRecord[i].get(Ext.ModelManager.getModel(store.model).getFields()[0].name);                       
           	}
           }
       }
       if(selectedValue==null || selectedValue==""){
          Ext.Msg.show({
            title: 'Msg',
            msg: 'Please select Record to Save',
            width: 300,
            buttons: Ext.Msg.OK,
            icon: Ext.MessageBox.INFO
        });
       }else{
       var form=Ext.create("Ext.form.Panel",{
           border:false,
           fieldDefaults:{labelWidth:120},
           defaultType:'textfield',
           bodyPadding:20,
           items:[{
              fieldLabel:'Watch List Name',
              name:'watch_list_name',
              allowBlank:false,
              anchor:'100%'
           }]
       });
	   var window=Ext.create("Ext.window.Window",{
	       title:'Watch List',
	       height:150,
	       width:500,
	       layout:'fit',
	       modal:true,
	       plain:true,
	       items:form,
	       buttons:[{
	          text:'Save',
	          handler:function(){
	            if(form.getForm().isValid()){
	            form.submit({
		          url:'common/bookmark/insert_data.jsp',
		          method:'post',
		          waitMsg:'Waiting...',
		          params:{selectId:selectedValue,module_name:module_name,type:book.getForm().findField('type').getValue()},
		          success:function(form, action){
		             var data=Ext.decode(action.response.responseText);
		             if(action.result.success==true && action.result.isPass==true){
		                window.close();
		                book.getForm().findField('type').clearValue();
		                book.getForm().findField('list').clearValue();
		                Ext.Msg.alert("msg","Success");
		             }else{
		                form.reset();
		                Ext.Msg.alert("msg","Watch List Name already exists!");
		             }
		          }
		       });}
	          }
	       }]
	   }).show();
    }
	},//////////////////////////
	searchcon:function(book,module_name){
	   var target=Ext.getCmp("content-panel").getActiveTab().getEl().dom;
	   var i=0;if(Ext.isIE){i=1;}
	   var search=target.childNodes[i].childNodes[1].childNodes[0];
	   //var book_form=target.childNodes[0].childNodes[2].childNodes[0];
	   //var book=Ext.getCmp(book_form.id);
	   var search_form=Ext.getCmp(search.id);
	   var data=Ext.decode('{}');
	   data["json_string"]=new Array();
	   for(var i=0;i<search_form.getForm().getFields().length;i++){ 
	       if(search_form.getForm().getFields().getAt(i).getName()!=undefined && search_form.getForm().getFields().getAt(i).getName()!='change_fields'){
	          var current_field = search_form.getForm().getFields().getAt(i);
	          data.json_string[i]={};
	          data.json_string[i].property=search_form.getForm().getFields().getAt(i).getName();
	          if (current_field.xtype == 'datefield') {
	             data.json_string[i].value=Ext.Date.format(search_form.getForm().getFields().getAt(i).getValue(), 'Y-m-d');
	          }else if (current_field.xtype == 'combofieldbox'||current_field.xtype == 'combobox' || current_field.xtype == 'boxselect' || current_field.xtype == 'comboboxselect') {
	             var value=search_form.getForm().getFields().getAt(i).getValue();
	             var combovalue='';
	             if(typeof (value)=='object' && value!='' && value!=null){
	                for(var m=0;m<value.length;m++){
	                    if(m==0){
	                       combovalue=combovalue+value[m];
	                    }else{
	                       combovalue=combovalue+","+value[m];
	                    }
	                }
	                data.json_string[i].value=combovalue;
	             }else{
	                data.json_string[i].value=(value==null||value==undefined?"":value);
	                data.json_string.splice(i, 1);
	             }
	          }else if (current_field.xtype == 'checkboxgroup') {
	              var check_box = json_data.row[j].items;
                  var checkgroup = current_field.items.items;
                  for (var c = 0; c < checkgroup.length; c++) {
                      if(check_box[c].checked==true)
                         data.json_string[i].value="1";
                      else
                         data.json_string[i].value="0";
                  }
	          }else {
	             data.json_string[i].value=search_form.getForm().getFields().getAt(i).getValue();
	          }
	      }
	   }
	   data.json_string[search_form.getForm().getFields().length]={};
	   data.json_string[search_form.getForm().getFields().length].property="class";
	   data.json_string[search_form.getForm().getFields().length].value=module_name;
	   for(var j=0;j<data.json_string.length;j++){
	      if (data.json_string[j] == undefined || data.json_string[j] == null || data.json_string[j] == "null") {
              data.json_string.splice(j, 1);
              j = 0;
          }
	   }
       var form=Ext.create("Ext.form.Panel",{
           border:false,
           fieldDefaults:{labelWidth:120},
           defaultType:'textfield',
           bodyPadding:20,
           items:[{
              fieldLabel:'Condition Name',
              name:'condition_name',
              allowBlank:false,
              anchor:'100%'
           }]
       });
	   var window=Ext.create("Ext.window.Window",{
	       title:'Search Condition',
	       height:150,
	       width:500,
	       collapsible:true,
	       layout:'fit',
	       modal:true,
	       plain:true,
	       items:form,
	       buttons:[{
	          text:'Save',
	          handler:function(){
	            if(form.getForm().isValid()){
	            form.submit({
		          url:'common/bookmark/insert_data.jsp',
		          method:'post',
		          waitMsg:'Waiting...',
		          params:{json_string:Ext.encode(data.json_string),module_name:module_name,type:book.getForm().findField('type').getValue()},
		          success:function(form, action){
		             var data=Ext.decode(action.response.responseText);
		             if(action.result.success==true && action.result.isPass==true){
		                window.close();
		                book.getForm().findField('type').clearValue();
		                book.getForm().findField('list').clearValue();
		                Ext.Msg.alert("msg","Success");
		             }else{
		                form.reset();
		                Ext.Msg.alert("msg","Condition Name already exists!");
		             }
		          }
		       });}
	          }
	       }]
	   }).show();
	},
	update_bookmark:function(form,grid,module_name){
	   var store=grid.getStore();
	   var selModel = null;
       var selectedValue = "";
       if (grid.selModel != null && grid.selModel != undefined) {
           selModel = grid.selModel;
           var selectedRecord = selModel.getSelection();
           for (var i = 0; i < selectedRecord.length; i++) {
           	if(i!=0){
           		 selectedValue = selectedValue + "," + selectedRecord[i].get(Ext.ModelManager.getModel(store.model).getFields()[0].name);
           	}else{
           		selectedValue = selectedValue + selectedRecord[i].get(Ext.ModelManager.getModel(store.model).getFields()[0].name);                       
           	}
           }
       }
       if(selectedValue==null || selectedValue==""){
          Ext.Msg.show({
            title: 'Msg',
            msg: 'Please select Record to Save',
            width: 300,
            buttons: Ext.Msg.OK,
            icon: Ext.MessageBox.INFO
        });
       }else{
          form.submit({
	          url:'common/bookmark/update_data.jsp',
	          method:'post',
	          waitMsg:'Waiting...',
	          params:{selectId:selectedValue,module_name:module_name},
	          success:function(form, action){
	             var data=Ext.decode(action.response.responseText);
	             if(action.result.success==true){
	                Ext.Msg.alert("msg","Save Success");
	             }
	          }
	       });
    }
	},
	//////////////////////////
	update_condition:function(form,module_name){
	   var target=Ext.getCmp("content-panel").getActiveTab().getEl().dom;
	   var i=0;if(Ext.isIE){i=1;}
	   var search=target.childNodes[i].childNodes[1].childNodes[0];
	   var search_form=Ext.getCmp(search.id);
	   var data=Ext.decode('{}');
	   data["json_string"]=new Array();
	   for(var i=0;i<search_form.getForm().getFields().length;i++){ 
	       if(search_form.getForm().getFields().getAt(i).getName()!=undefined && search_form.getForm().getFields().getAt(i).getName()!='change_fields'){
	          var current_field = search_form.getForm().getFields().getAt(i);
	          data.json_string[i]={};
	          data.json_string[i].property=search_form.getForm().getFields().getAt(i).getName();
	          if (current_field.xtype == 'datefield') {
	             data.json_string[i].value=Ext.Date.format(search_form.getForm().getFields().getAt(i).getValue(), 'Y-m-d');
	          }else if (current_field.xtype == 'combofieldbox'||current_field.xtype == 'combobox' || current_field.xtype == 'boxselect' || current_field.xtype == 'comboboxselect') {
	             var value=search_form.getForm().getFields().getAt(i).getValue();
	             var combovalue='';
	             if(typeof (value)=='object' && value!='' && value!=null){
	                for(var m=0;m<value.length;m++){
	                    if(m==0){
	                       combovalue=combovalue+value[m];
	                    }else{
	                       combovalue=combovalue+","+value[m];
	                    }
	                }
	                data.json_string[i].value=combovalue;
	             }else{
	                data.json_string[i].value=(value==null||value==undefined?"":value);
	                data.json_string.splice(i, 1);
	             }
	          }else if (current_field.xtype == 'checkboxgroup') {
	              var check_box = json_data.row[j].items;
                  var checkgroup = current_field.items.items;
                  for (var c = 0; c < checkgroup.length; c++) {
                      if(check_box[c].checked==true)
                         data.json_string[i].value="1";
                      else
                         data.json_string[i].value="0";
                  }
	          }else {
	             data.json_string[i].value=search_form.getForm().getFields().getAt(i).getValue();
	          }
	      }
	   }
	   data.json_string[search_form.getForm().getFields().length]={};
	   data.json_string[search_form.getForm().getFields().length].property="class";
	   data.json_string[search_form.getForm().getFields().length].value=module_name;
	   for(var j=0;j<data.json_string.length;j++){
	      if (data.json_string[j] == undefined || data.json_string[j] == null || data.json_string[j] == "null") {
              data.json_string.splice(j, 1);
              j = 0;
          }
	   }
	   form.submit({
          url:'common/bookmark/update_data.jsp',
          method:'post',
          waitMsg:'Waiting...',
          params:{json_string:Ext.encode(data.json_string),module_name:module_name},
          success:function(form, action){
             var data=Ext.decode(action.response.responseText);
             if(action.result.success==true){
                Ext.Msg.alert("msg","Save Success");
             }
                
          }
       });
	}
});
