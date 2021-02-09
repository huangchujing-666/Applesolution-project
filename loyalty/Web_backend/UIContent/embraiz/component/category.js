Ext.require([
    'Ext.tree.*',
    'Ext.data.*',
    'Ext.tip.*'
]);
Ext.define('com.embraiz.component.category', {
	dataUrl:undefined,
	target_div:undefined,
	form_detail_div:undefined,
	tool_div:undefined,
	form_json_url:undefined,
    initCategory:function(title, dataUrl,form_json_url,move_update_url,add_json_url, delete_json_url){
		var me=this;
		me.dataUrl=dataUrl;		//tree
		me.form_json_url=form_json_url;		//detail form	
		target_div=document.createElement("div");
		target_div.style.margin="5px";
		var form_detail_div=me.form_detail_div=document.createElement("div");
		var currentTab = Ext.getCmp('content-panel').getActiveTab();
		currentTab.getEl().dom.lastChild.appendChild(target_div);				
		currentTab.getEl().dom.lastChild.appendChild(form_detail_div);		
		me.target_div=target_div;
		var store = Ext.create('Ext.data.TreeStore', {      
			proxy: {
				type: 'ajax',
			   // url: 'menuTree.jsp'
				url:dataUrl
			},///////
			 root: {
				text: 'Category Tree',
				id: 'src',
				expanded: true
			},       
			 sorters: [{
				 property: 'leaf',
				 direction: 'ASC'
			 }, {
				property: 'fileName',
				direction: 'ASC'
			 }] 
		});   
		var tree = Ext.create('Ext.tree.Panel', {
			store: store,
			viewConfig: {
				plugins: {
					ptype: 'treeviewdragdrop',
					containerScroll: true
				}
			},
//			scroll:false,
			renderTo: target_div,
			height: 400,       
			title: title,
			useArrows: true,
			dockedItems: [{
				xtype: 'toolbar',
				items: [{
					text: 'Expand All',
					handler: function(){
						tree.getEl().mask('Expanding tree...');
						var toolbar = this.up('toolbar');
						toolbar.disable();                    
						tree.expandAll(function() {
							tree.getEl().unmask();
							toolbar.enable();
						});
					}
				}, {
					text: 'Collapse All',
					handler: function(){
						var toolbar = this.up('toolbar');
						toolbar.disable();                    
						tree.collapseAll(function() {
							toolbar.enable();
						});
					}
				},{
					text: 'Add',
					iconCls:'iconAdd',
					handler: function(){
						 if(tree.selModel.selected.items.length==0){
							  Ext.Msg.alert('Warning','Please select one of parent category!');
						  }else{
							  var id=tree.selModel.selected.items[0].data.id; 
							  //new com.embraiz.tag().open_pop_up(id,'Add product Category','com.embraiz.productCategory.js.index');
							    	var win = Ext.create('widget.window', {
											title: 'Form',
											closable: true,
											width: 900,
											height: 650,
											bodyStyle: 'padding: 5px;',
											modal: true,
											listeners:{ close:function(){Ext.getBody().unmask()}}
									 });	    	 
									win.show();										
									Ext.Ajax.request({
										url:add_json_url+'?parent_id='+id,
										success:function(o){
											var data_json = Ext.decode(o.responseText);
											new com.embraiz.component.form().editForm(win, data_json,null);
										/////重写save
									 	var btn=win.items.items[0].dockedItems.items[1].items.items[0];						
										btn.setHandler(function(thiz,op){
											var form=thiz.ownerCt.ownerCt.form;	
											   if (form.isValid()) {
												form.submit({													
													submitEmptyText: false,
													success: function (form, action) {	
													  win.close();																									
													  Ext.getBody().unmask();
													    Ext.Msg.show({
														title: 'Success',
														msg: 'Add success',
														width: 200,
														buttons: Ext.Msg.OK,
														icon: Ext.MessageBox.INFO
													 });
													  tree.store.load();
													}
												});
											   }
										
										});
									 ////
										},
										scope:this
									});
						  }					
						}},{
						text: 'Remove',
						iconCls:'iconDelete',
						handler: function(){
							 if(tree.selModel.selected.items.length==0){
								  Ext.Msg.alert('Warning','Please select product Category!');
							  }else{
								   Ext.MessageBox.confirm('Confirm', 'Are you sure you want to remove?',
	            			        function(btn) {
	            			            if (btn == 'yes') {	
											  var id=tree.selModel.selected.items[0].data.id; 
													Ext.Ajax.request({
													    url:delete_json_url+'?parent_id='+id,//'../GiftCategory/MultiDelete?parent_id='+id,//
													 success:function(o){
														var data_json = Ext.decode(o.responseText);	
													     //remove后
														 Ext.Msg.show({
														       title: 'Success',
														        msg: 'Remove success',
																width: 200,
																buttons: Ext.Msg.OK,
																icon: Ext.MessageBox.INFO
															 });
														  //
														    if(me.form_detail_div!=undefined){	
			                                                      me.form_detail_div.innerHTML="";		
			                                                   }
														  tree.store.load();										       
													  },
													scope:this
												});
									
								        } else {
	            			                this.close();
										}
	            			         });
							  }
						}
						
				} ]
			}]
		});
		   tree.on('itemmove',function(e,oldParent,newParent,index,eOpts){
			     if(newParent.data.id==oldParent.data.id){
					 
				  }else{//从一个父节点拖到另外一个父节点 server save
				         var newParentId=newParent.data.id;
						 var oldParentId=oldParent.data.id;
						  var id=e.data.id;
				          Ext.Ajax.request({
							url: move_update_url,
							params:{
								  newParentId:newParentId,
								  oldParentId:oldParentId,
								  id:id
								},
							success: function(o) {
								var resText = Ext.decode(o.responseText);
								////
							}
						 });
				  }
		   });
			tree.on('itemclick',function(selModel, record){
			  var id=record.data.id;
			  var form = new com.embraiz.component.form();	
			  if(me.form_detail_div!=undefined){	
				  //me.form_detail_div.remove();	
				  me.form_detail_div.parentNode.removeChild(me.form_detail_div);
				  me.form_detail_div=document.createElement("div");
				  currentTab.getEl().dom.lastChild.appendChild(me.form_detail_div);
			   }
			//  if(me.form_detail_div==undefined){				
				//var currentTab = Ext.getCmp('content-panel').getActiveTab();
					var tool_div=document.createElement("div");	
					me.form_detail_div.appendChild(tool_div);			
					//currentTab.getEl().dom.lastChild.appendChild(tool_div);
					tool_div.style.margin="5px";
				    var target_div=document.createElement("div");
					me.form_detail_div.appendChild(target_div);			
					//currentTab.getEl().dom.lastChild.appendChild(target_div);			   
				    Ext.create('Ext.toolbar.Toolbar', {
						renderTo: tool_div,
						width: '100%',
						height:'30',
						margin: '0 0 0 0',				
						items:[
						{
							xtype    : 'button',
							name     : 'view',
							text     : 'View',
							hidden   : true,
							iconCls  : 'iconView',
							handler  : function (b, e) {								
									 form.viewForm(form.target_div, me.form_json_url+'?id='+id);
									 b.hide();
									 editBtn=b.ownerCt.items.items[1];
									 if (editBtn != undefined) {
										 editBtn.show();
									 }
							   }
					   },{
							xtype    : 'button',
							name     : 'edit',
							text     : 'Edit',
							iconCls  : 'iconRole16',
							handler  : function (b, e) {    						              
									 form.editForm(form.target_div, form.json_data);
									 b.hide();
									 viewtBtn=b.ownerCt.items.items[0];
									 if (viewtBtn != undefined) {
										 viewtBtn.show();
									 }
									 /////重写save
									 	var btn=form.editSimple.dockedItems.items[1].items.items[0];									
										btn.setHandler(function(thiz,op){
											var form=thiz.ownerCt.ownerCt.form;	
											   if (form.isValid()) {
												form.submit({													
													submitEmptyText: false,
													success: function (form, action) {
													 
													    Ext.getBody().unmask();
													    var result = Ext.decode(action.response.responseText);
													    
													    Ext.Msg.show({
														    title: 'Success',
														    msg: result.msg,
														    width: 200,
														    buttons: Ext.Msg.OK,
														    icon: Ext.MessageBox.INFO
													     });
													  tree.store.load();
													},
													failure: function (form, action) {
													    Ext.getBody().unmask();
													    var result = Ext.decode(action.response.responseText);

													    Ext.Msg.show({
													        title: 'Fail',
													        msg: result.msg,
													        width: 200,
													        buttons: Ext.Msg.OK,
													        icon: Ext.MessageBox.INFO
													    });
													}
												});
											   }
										
										});
									 ////
									 
								}
					   }]        	
				 });
			   //			
				form.viewForm(target_div, me.form_json_url+'?id='+id);	
			//  }
		});
    }
});