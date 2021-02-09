Ext.require([
    'Ext.util.History',
    'Ext.tab.Panel'
]);
Ext.onReady(function () {

    // Check Session
    checkSession();

	(Ext.defer(function() {
	var detailEl;
	var activeTabIndex = 0;
    var formFieldFocusIndex = 0;
    var MOVE_MODE_TABS = 1;
    var MOVE_MODE_FORMS = 2;
    var MOVE_MODE_COMB = 3;  
    var MOVE_MODE = MOVE_MODE_TABS;
    //初始化history组件   
    Ext.History.init();   
    //设置截断符   
    var tokenDelimiter = ':';   
	var contentPanel=Ext.create('Ext.tab.Panel', { 
		id: 'content-panel',	
		region: 'center', 
		layout: 'card',
		//deferredRender : true,
		margins: '0 5 5 0',
		activeItem: activeTabIndex,
		border: false,
		items: [],
		//cls: 'template-green', //overwrite css style
		listeners: {   
            'tabchange': function(tabPanel, tab){   
             //tab1切换时修改浏览器hash   
             Ext.History.add(tabPanel.id + tokenDelimiter + tab.id);   
             }   
        }
	});
	
	

	     //获取浏览器hash中#后面的字符串   
         Ext.History.on('change', function(token){   
             if(token){   
             //如果有字符串则进行相应处理  
             //Ext.Msg.alert(token); 
             var parts = token.split(tokenDelimiter);         
             var tabPanel = Ext.getCmp(parts[0]); 
             var tabId;
             if(parts.length==3){
                tabId=parts[1]+":"+parts[2];             
             }else{
                tabId=parts[1]; 
             } 
                tabPanel.show();   
                tabPanel.setActiveTab(tabId);           
             }else{   
             //如果没有字符串则直接默认第一个标签页   
                 contentPanel.setActiveTab(0);   
                 //contentPanel.getItem(0).setActiveTab(0);   
             }   
         }); 
	
	/////////////////////
     var store = Ext.create('Ext.data.TreeStore', {      
        proxy: {
            type: 'ajax',
            url: '../Menu/MenuTree'
        },///////
         root: {
            text: 'Ext JS',
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
    // Go ahead and create the TreePanel now so that we can use it below
     var treePanel = Ext.create('Ext.tree.TreePanel', {
        id: 'ctagpanel',
        title: 'Menu',         
        region:'west',      
        split: true,
        height: 360,
        width:200,
        minSize: 150,
        rootVisible: false,
        //cls: 'template-green', //overwrite css style
     
        store: store          	
    });  
     
     treePanel.on('itemclick', function (selModel, record) {
         // Check Session
         checkSession();

         if (record.get('leaf')) {
         //Ext.Msg.alert(record.raw.url);     
         new com.embraiz.tag().openNewTag(record.getId(), 'List: '+record.get('text'),record.raw.url,record.raw.iconCls,record.raw.iconClsC,record.raw.iconClsE);               
        }
    });
    
    Ext.create('Ext.Viewport', {
        layout: 'border',
        
        items:[
            {
                xtype: 'box',
                el: 'main_top_menu',
                region: 'north',
                html: '',
                height: 75  // 68 +3 shadow + somespace
            },
            treePanel,
            contentPanel,
            {
                xtype: 'box',
                el: 'main_bottom',
                region: 'south',
                html: '',
                height: 22
            }
       ],
        renderTo: Ext.getBody()

    });

    treePanel.getView().focus();

    //Add DashBoard
	tab = Ext.getCmp("content-panel");
	currentTagPanel = tab.add({ id: "docs_ctagpanel", xtype: 'portalpanel', region: 'center', title: "Dashboard", cls: "x-portal treePanel-green", iconCls: "iconDashboard", autoScroll: true, closable: false, border: false });
//	currentTagPanel = tab.add({ id: "docs_ctagpanel", xtype: 'portalpanel', region: 'center', title: "Dashboard", cls: "treePanel-green", iconCls: "iconDashboard", autoScroll: true, closable: false, border: false });
	//	currentTagPanel = tab.add({ id: "docs_ctagpanel", title: "Dashboard", cls: "treePanel-green", iconCls: "iconDashboard", autoScroll: true, closable: false, border: false });
	currentTagPanel.show();
	tab.setActiveTab(currentTagPanel);
//	Ext.History.add("content-panel" + tokenDelimiter + "docs_ctagpanel");

	if (currentTagPanel != null) {
	   // new com.palmary.report.js.memberlevel().initTag(currentTagPanel);
	    new com.palmary.portal.js.index().initTag(currentTagPanel);
	    //new com.palmary.dashboard.js.index().initTag(currentTagPanel);
	}

	//var docs_ctagpanel = Ext.getCmp("docs_ctagpanel");
	//var resever_div = document.createElement("div");
	//docs_ctagpanel.getEl().dom.lastChild.appendChild(resever_div);

	//    //new com.embraiz.common.js.index().grid(resever_div, '', { className: 'EnquirySetup', method: 'gridCfg', data: 'girdCfgJsonReseved', cfg: 'girdHeaderReseved', params: '' });

	//var temp_class = new com.palmary.dashboard.js.index();
	//temp_class.initTag(currentTagPanel, '', '', 1, 1, null);


	//target_div = document.createElement("div");
	//docs_ctagpanel.getEl().dom.lastChild.appendChild(target_div);
	
	//target_div3 = document.createElement("div");
	//docs_ctagpanel.getEl().dom.lastChild.appendChild(target_div3);
        	

	//target_div4 = document.createElement("KitchenSink.view.grid.ArrayGrid");
	//docs_ctagpanel.getEl().dom.lastChild.appendChild(target_div4);

	//Ext.Ajax.request({
	//    url: "../table/InitWithSearchColumn/memberProfile",
	//    async: true,
	//    success: function (o) {
	//        var data_json = Ext.decode(o.responseText);
	//        new com.embraiz.component.gridSearch().render(resever_div, target_div, data_json, true);
	//    },
	//    scope: this
	//});

	
	//tab切换事件		
	new Ext.KeyMap('content-panel', [
	{
       	key: 37,
       	alt:true,
       	handler: function(key, e) { 
         	//contentPanel.getActiveTab().focus();    	
       		activeTabIndex++;
       		if (activeTabIndex > this.items.getCount() - 1) {
       			activeTabIndex = 0;
       		}
       		this.setActiveTab(activeTabIndex);
       		contentPanel.getActiveTab().focus();	
       	},
       	scope: contentPanel,
       	stopEvent: true
   	},
  	{
      	key: 39,
      	alt:true,
      	handler: function(key, e) {
      	    //Ext.getCmp("content-panel").getActiveTab().focus();    	        		
      		activeTabIndex--;
      		if (activeTabIndex < 0) {
      			activeTabIndex = this.items.getCount() - 1;
      		}
      		this.setActiveTab(activeTabIndex);
      		Ext.getCmp("content-panel").getActiveTab().focus();      		
      	},
      	scope: contentPanel,
      	stopEvent: true
  	}
    ]);
    
    //focus on tab
    new Ext.util.KeyMap(document,{
        key:79,//o
        ctrl:true,
        alt:true,
        handler:function(){
           Ext.getCmp("content-panel").getActiveTab().focus();
        }
    });
    //focus to toolbar
    new Ext.util.KeyMap(document,{
        key:80,//p
        ctrl:true,
        alt:true,
        handler:function(){
           var buttons=Ext.DomQuery.select('button[type=button]',Ext.getCmp("content-panel").getActiveTab().getEl().dom.firstChild);
           Ext.each(buttons,function(o,i,buttons){
               if(i==1 || i==0){
                  Ext.get(o).focus(true);
               }
           });
        }
    });
    //focus to tab
    new Ext.util.KeyMap(document,{
	    key:73,//i
	    ctrl:true,
	    alt:true,
	    handler:function(){
	       target_div.focus();
	       Ext.getCmp("content-panel").getActiveTab().focus();
	    }
	});

	
	//Focus on Form
	document.onhelp=function(){return false};   
    window.onhelp=function(){return false}; 
	
	//Focus on search box
	new Ext.util.KeyMap(document,{
	    key:88,
	    alt:true,
	    fn:function(){
	       document.getElementById('generalSearch').value='';
	       Ext.get('generalSearch').focus();    
	    }
	});
	new com.embraiz.keyboard.js().treeKeyboard(treePanel);
	new com.embraiz.keyboard.js().focusToForm(contentPanel,tab);
	new com.embraiz.keyboard.js().closeTAb();
	
	new Ext.util.KeyMap(document,{
	    key:72,//h
	    ctrl:true,
	    alt:true,
	    fn:function(){
	       var currentTab = tab.getActiveTab();		
		   var totalTabs = tab.items.getCount();	
		   for(i=totalTabs-1; i>0; i--){			 
			 if(currentTab.getId()!=tab.items.get(i).getId() ){
				tab.remove(tab.items.get(i));
			}
		   }
	    }
	});

 //new com.embraiz.history().getHistory();
 
	    ////////////

	Ext.fly("change_pwd").on("click", function () {//change password
	    var simple = Ext.create('Ext.form.Panel', {
	        url: '../User/ChangePassword',
	        bodyStyle: 'padding:5px 5px 0',
	        width: 440,
	        fieldDefaults: {
	            msgTarget: 'side',
	            labelWidth: 150
	        },
	        defaultType: 'textfield',
	        defaults: {
	            anchor: '100%'
	        },
	        items: [{
	            fieldLabel: 'Old Password',
	            name: 'oldpwd',
	            inputType: 'password',
	           // cls: 'dateRange_cls',
	            allowBlank: false
	        }, {
	            fieldLabel: 'New Password',
	            name: 'newpwd',
	            inputType: 'password',
	            //  cls: 'dateRange_cls',
	            allowBlank: false
	        }, {
	            fieldLabel: 'Confirm Password',
	            name: 'confirmpwd',
	            inputType: 'password',
	             //  cls: 'dateRange_cls',
	            allowBlank: false
	        }],
	        buttons: [{
	            text: 'Save',
	            handler: function () {
	                var flag = true;
	                var form = this.up('form').getForm();
	                var newpwdValue = form.findField('newpwd').getRawValue();
	                var comfirmpwdValue = form.findField('confirmpwd').getRawValue();
	                if (newpwdValue != comfirmpwdValue) {
	                    flag = false;
	                    Ext.Msg.alert('Warn!', 'Confirm the password and new passwords do not match!');
	                }
	                if (form.isValid() && flag) {
	                    form.submit({
	                        success: function (form, action) {
	                            Ext.Msg.alert('Success', 'Change password success!');
	                            win.close();
	                        },
	                        failure: function (form, action) {
	                            Ext.Msg.alert('Failed', action.result.msg);
	                        }
	                    });
	                }
	            }
	        }, {
	            text: 'Cancel',
	            handler: function () {
	                this.up('form').getForm().reset();
	            }
	        }]
	    });
	    var win = Ext.create('widget.window', {
	        title: 'Change Password',
	        closable: true,
	        width: 460,
	        height: 180,
	        bodyStyle: 'padding: 5px;',
	        modal: true,
	        items: [simple]
	    });
	    win.show();
	});

	Ext.fly("close_all_tab_link").on("click", function () { //close tag	
	    var currentTab = tab.getActiveTab();
	    var totalTabs = tab.items.getCount();
	    for (i = totalTabs - 1; i > 0; i--) {
	        if (currentTab.getId() != tab.items.get(i).getId()) {
	            tab.remove(tab.items.get(i));
	        }
	    }
	});

 var hideMask=function () {
     Ext.get('loading').remove();
     Ext.fly('loading-mask').animate({
         opacity:0,
         remove:true
        // callback: firebugWarning
     });
 };
 Ext.defer(hideMask, 250); 
   },500));
   
});
