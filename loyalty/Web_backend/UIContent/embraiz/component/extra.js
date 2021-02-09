Ext.define('com.embraiz.approvalProcess.js.index',{
	initTag:function(object,id,submitForm,url,appGrid){	
	var sub,app,reject,edit,grid;
	if(!url){
		url='com.embraiz.common.js.edit';
	}
	Ext.Ajax.request({
		url:'common/approvalProcess/approvalRole.jsp',
		params:{
			form:submitForm,
			id:id
		},
		async:false,
		success:function(o){
			var result=Ext.decode(o.responseText);
			edit=result.edit;
			sub=result.sub;
			app=result.app;
			reject=result.reject;
			grid=result.grid;
		},
		failure:function(){
			
		}
	});
	var btnApp=Ext.create('Ext.Button', {
		iconCls:'iconApprove',
	    text: 'Approve',
	    handler: function() {
		var winApp = Ext.create('widget.window', {
			closable: true,
			width: 800,
			height: 400,
			bodyStyle: 'padding: 5px;',
			modal: true
		});
		winApp.setTitle('Approve');
		winApp.show();	
		Ext.Ajax.request({
			url:'common/approvalProcess/centerForm.jsp',
			params:{
				type:'Approve',
				module_name:'ApproveCenter',
				method:'common',
				form:submitForm,
				id:id
			},
			success:function(o){
				var json=Ext.decode(o.responseText);
				new com.embraiz.component.form().editForm(winApp,json,null,url,id,submitForm);
			}
		});
	    }
	});
	var btnReject=Ext.create('Ext.Button', {
		iconCls:'iconReject',
	    text: 'Reject',
	    handler: function() {
		var winRej = Ext.create('widget.window', {
			closable: true,
			width: 800,
			height: 400,
			bodyStyle: 'padding: 5px;',
			modal: true
		});
		winRej.setTitle('Reject');
		winRej.show();	
		Ext.Ajax.request({
			url:'common/approvalProcess/centerForm.jsp',
			params:{
				type:'Reject',
				module_name:'ApproveCenter',
				method:'common',
				form:submitForm,
				id:id
				},
			success:function(o){
				var json=Ext.decode(o.responseText);
				new com.embraiz.component.form().editForm(winRej,json,null,url,id,submitForm);
			}
		});
	    }
	});
	var btnSub=Ext.create('Ext.Button', {
	    text: 'Submit',
	    iconCls:'iconSubmit',
	    handler: function() {
		var winSub = Ext.create('widget.window', {
			closable: true,
			width: 800,
			height: 400,
			bodyStyle: 'padding: 5px;',
			modal: true
		});
		winSub.setTitle('Submit');
		winSub.show();	
			Ext.Ajax.request({
				url:'common/approvalProcess/centerForm.jsp',
				params:{
				type:'Submit',
				module_name:'ApproveCenter',
				method:'common',
				form:submitForm,
				id:id
				},
				success:function(o){
					var json=Ext.decode(o.responseText);
					new com.embraiz.component.form().editForm(winSub,json,null,url,id,submitForm);	
				}
			});
	    }
	});
	if(app){
		object.add(btnApp);
	}
	if(sub){
		object.add(btnSub);
	}
	if(reject){
		object.add(btnReject);
	}
	if(!edit){
		for(var i=0;i<object.items.items.length;i++){
			if(object.items.items[i]&&object.items.items[i].name=='edit'){
				object.items.items[i].hide();
			}
		}
	}
	if(grid){
		var wf=new Ext.ux.embraizWF();
		appGrid.style.padding=5;
		appGrid.style.paddingTop=35;
		wf.renderForm(appGrid,'common/approvalProcess/wf_data.jsp?id='+id+'&form='+submitForm);
	}
	}	
});

Ext.define('com.embraiz.roleManagement.js.index',{
	initTag:function(object,module){
	 
	/* var s=roleStore.rStore
	
	 var toolbar=object;
	 var len=toolbar.items.items.length
	
	 for(var i=0;i<len;i++){
	   var btn = toolbar.items.items[i];
	   var menu = btn.menu;
	   if(menu==null||menu==''||menu==undefined){
		 var btnName = btn.name;
		 if(btnName!=null&&btnName!=''&&btnName!=undefined){ 
		   
		   var module='';
		   var butType='';
		   if(btnName.indexOf(':')!=-1){
			   module=  btnName.substring(0,btnName.indexOf(':'));
			   butType= btnName.substring(btnName.indexOf(':')+1,btnName.length);		    
		   }
		   
		   s.each(function(model){
				 
			   if(model.get('module')==module){
					
					if('edit'==butType&&'1'!=model.get('rightU')){
						btn.hide();
					}
					
					if('remove'==butType&&'1'!=model.get('rightD')){
						btn.hide();
					}
					
					if('add'==butType&&'1'!=model.get('rightI')){
						btn.hide();
					}
					if('exprot'==butType&&'1'!=model.get('rightE')){
						btn.hide();
					}
				}
			   
			});
		   
		 }
		   
	   }else{
		   for(var j=0;j<btn.menu.items.items.length;j++ ){
		      var menuBtn=btn.menu.items.items[j];
		      
		      var btnName = menuBtn.name;
		      
		      if(btnName!=null&&btnName!=''&&btnName!=undefined){ 
		      
		      var module='';
		      var butType='';
		      if(btnName.indexOf(':')!=-1){
			     module=  btnName.substring(0,btnName.indexOf(':'));
			     butType= btnName.substring(btnName.indexOf(':')+1,btnName.length);
			    
		       }
		      
		      s.each(function(model){
					 
				   if(model.get('module')==module){
						
						if('edit'==butType&&'1'!=model.get('rightU')){
							menuBtn.hide();
						}
						
						if('remove'==butType&&'1'!=model.get('rightD')){
							menuBtn.hide();
						}
						
						if('add'==butType&&'1'!=model.get('rightI')){
							menuBtn.hide();
						}
						if('exprot'==butType&&'1'!=model.get('rightE')){
							menuBtn.hide();
						}
					}
				   
				});
		      
		     } 
		      
		   }   
	    }
	 
	 } 	*/
	},
	
	checkToolbar:function(id,url){
		var rnum=false;
		Ext.Ajax.request({
			//url:'modules/lead/checkPower.jsp',
			url:url,
			method: 'post',
		    async : false,
			params:{
				id:id
			},
			success:function(o){
			var result=Ext.decode(o.responseText);
			if(result.exist){		
				rnum=true;
			}
		}
		});
		return rnum;
		
	}
});
Ext.define('com.embraiz.exporter.js.index',{
	initTag:function(grid,toolbar,store,form_div,module){
		var form=Ext.get(form_div);
		form=Ext.getCmp(form.dom.lastChild.id);
		var data=form.getValues();
		Ext.getBody().mask("wait");
		Ext.Ajax.request({
			url:'../Table/Export/'+module,
			params:data,
			success:function(o){
				Ext.getBody().unmask();
				var result = Ext.decode(o.responseText);

				if (result.success == true) {
				    window.open(result.fileDownloadPath);
				    //window.open('common/fileDownload.jsp?fileName=' + result.fileName + '&del=true');
				}
				else {
				    Ext.getBody().unmask();
				    Ext.Msg.alert('Error', 'Export Error!');
				}
				
			},
			failure:function(){
				Ext.getBody().unmask();
				Ext.Msg.alert('Error','Export Error!');
			}
		});
		
	}
});
Ext.define('com.embraiz.deal.js.index',{
	moduleTitle:function(module,id){
		var title='';
		Ext.Ajax.request({//查出title值
			url:'common/moduleTitle.jsp',
			async:false,
			params:{
				module_name:module,
				id:id
			},
			success:function(o){
				var resault=Ext.decode(o.responseText);
				title=resault.title;
			}
		})
		return title;
	}
	
});
