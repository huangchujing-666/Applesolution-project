Ext.onReady(function () {
    Ext.define('com.embraiz.tag', {
    	popUpForm:undefined,
    	winTag:undefined,
    	open_pop_up:function (id, title, url, iconCls, iconClsC, iconClsE, itemId,extra) {
	    	 var win =undefined;
	    	this.popUpForm=win = Ext.create('widget.window', {
					title: title,
					closable: true,
					width: 900,
					height: 400,
//					bodyStyle: 'padding: 5px;',
					modal: true,
					listeners:{ close:function(){Ext.getBody().unmask()}}
			 });	    	 
	    	win.show();	    	
	    	Ext.decode("new " + url + "()").init_pop_up(win,id, itemId,extra);
    	},
    	openWinTag:function(id, title, url, iconCls, iconClsC, iconClsE, itemId,extra){
    	  	var win =undefined;
    	  	this.winTag=win= Ext.create('widget.window', {
					title: title,
					closable: true,
					width: 900,
					height: 400,
//					bodyStyle: 'padding: 5px;',
					modal: true,
					layoutOnTabChange:true ,            
					autoScroll:true,
					listeners:{ close:function(){Ext.getBody().unmask()}},
					html:'<div id="abc"><div><div id="table_div"></div>'
			 });	    	 
	    	win.show();	    	
	    	Ext.decode("new " + url + "()").init_win_up(win,id, itemId,extra);
    	},
    	openNewTag: function (id, title, url, iconCls, iconClsC, iconClsE, itemId, extra) {
    	    
            tab = Ext.getCmp("content-panel");
            ///
           var pre_tab=tab.getActiveTab();
            ///
            
            temp_panel = Ext.getCmp("docs_" + id);
            if (temp_panel == null) {
                currentTagPanel = tab.add({
                    id: "docs_" + id,
                    idValue:id,
                    title: title,
                    iconCls: iconCls,
                    autoScroll: true,
                    closable: true,
                    cls: "inside-tab-panel",
                    urlLink:url,
                    par:itemId,
                    extra:extra,
                    loader: {
                        url: url,
                        loadMask: true
                    }
                });
                
                currentTagPanel.on('beforeclose', function () {
                   
                    for (var i = 0; i < tab.items.getCount(); i++) {
                        if (tab.items.get(i).getId() == tab.getActiveTab().getId()) {
                            if(i-1>=0){
                               if(pre_tab!=null){tab.setActiveTab(pre_tab);}else{
                            tab.items.get(i - 1).show();
                            tab.setActiveTab(i - 1);
                            }
                            }
                            break;
                        }
                    }
                });
                Ext.getBody().mask();
                currentTagPanel.on('activate',function(){
                   setTimeout(function a(){Ext.getBody().unmask()},1500);
                });                
                currentTagPanel.show();
                currentTagPanel.layout.outerCt.dom.style.height='';
                tab.setActiveTab(currentTagPanel);
                var temp_class;
                if(url.indexOf(".")>=0){
                	temp_class  = Ext.decode("new " + url + "()");            
                }else{
                	temp_class = new com.embraiz.common.js.index();
                	temp_class.module_name = url;
                }
                temp_class.initTag(currentTagPanel, url, title, id, itemId,extra, pre_tab);                
            } else {
                currentTagPanel = temp_panel;                
                tab.setActiveTab(currentTagPanel);		      
            }
        },
        tabRefrash : function(url, itemId, itemId1) {

			var currentTab = Ext.getCmp('content-panel').getActiveTab();
			currentTab.close();
			setTimeout(function a(){
			new com.embraiz.tag().openNewTag(currentTab.idValue, currentTab.title, currentTab.urlLink, currentTab.iconCls, '', '', currentTab.par,currentTab.extra);
			},200); 
			
		},
      	previous_tab_refresh:function(pre_tab,url,itemId,itemId1){
      		var tabPanel=Ext.getCmp('content-panel');
      		var currentTab = tabPanel.getActiveTab();
      		 var tabdom=pre_tab.getEl().dom;
    	     size1=tabdom.childNodes.length;
    	  for(var j=size1-1;j>=0;j--){
          var tabDom=tabdom.childNodes[j];	
           size=tabDom.childNodes.length;
           for(var i=size-1;i>=0;i--){	        	
          	var cTab=tabDom.childNodes[i];
              tabDom.removeChild(cTab);
           }
             if(j!=0){
                tabdom.removeChild(tabDom);
             }
          }
    	  tabPanel.setActiveTab(pre_tab);
    	  Ext.decode("new " + url + "()").initTag(pre_tab,'','',itemId,itemId1,null,currentTab);  
    	  tabPanel.setActiveTab(currentTab);
      	},
      	refreshStore:function(){
	     	 var target=Ext.getCmp("content-panel").getActiveTab().getEl().dom;
	         var i=0;if(Ext.isIE){i=1;}
	         if(target.childNodes[i].firstChild.className=='x-clear')
	            target.childNodes[i].removeChild(target.childNodes[i].firstChild);
	         var search_grid=target.childNodes[i].childNodes[2].childNodes[0];
	         var grid=Ext.getCmp(search_grid.id);
	         var store=grid.getStore();
	         store.load();
        }
    });
});