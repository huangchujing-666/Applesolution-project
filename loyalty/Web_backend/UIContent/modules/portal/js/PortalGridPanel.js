Ext.define('com.palmary.app.portalGridPanel', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.gridportlet',
    //config_data:undefined,
    //title:undefined,
    config: {
        config_data: ''
    },
    constructor: function (config) {
        var me = this;
        me.initConfig(config);
        this.callParent(arguments);
        return me;
    },
    //    applyConfig_data:function(config_data){
    //       console.log("refresh");
    //    },
    initComponent: function () {
        var me = this;
        var store = Ext.create('Ext.data.Store', {
            fields: me.config_data.fields,
            proxy: {
                type: 'ajax',
                url: me.config_data.url,
                reader: {
                    type: 'json',
                    root: 'items',
                    totalProperty: 'totalCount'
                }
            },
            autoLoad: true,
            pageSize: 10
        });
        var pageBar = Ext.create('Ext.PagingToolbar', {
            store: store,
            pageSize: 10,
            dock: 'bottom',
            emptyMsg: 'No data&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;',
            displayInfo: true,
            displayMsg: 'Total {2}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'
        });
        function renderTopic(value, p, record) {
   		 	 return Ext.String.format('<span onclick="outline(this)" class="gird_link" >{0}<input type="hidden" value="{1}"/></span>', value, formatString(record.data.href), record.data.tooltip, record.data.target);
   	    }
   	 	function renderTopic1(value, p, record) {
      		 return Ext.String.format('<span onclick="outline(this)" class="gird_link" >{0}<input type="hidden" value="{1}"/></span>', value, formatString(record.data.href1), record.data.tooltip1, record.data.target1);
      	}
   	 	function renderTopic2(value, p, record) {
      		 return Ext.String.format('<span onclick="outline(this)" class="gird_link" >{0}<input type="hidden" value="{1}"/></span>', value, formatString(record.data.href2), record.data.tooltip2, record.data.target2);
      	}
         for (var i = 0; i < me.config_data.columns.length; i++) {
             if(me.config_data.columns[i].type!=undefined){            	 	
	             if (me.config_data.columns[i].type == 'title') {
	                 me.config_data.columns[i].renderer= renderTopic;
	             }else if (me.config_data.columns[i].type == 'title1') {
	                 me.config_data.columns[i].renderer= renderTopic1;
	             }else if (me.config_data.columns[i].type == 'title2') {
	                 me.config_data.columns[i].renderer= renderTopic2;
	                 
	             }
             }
             if(me.config_data.columns[i].renderFun!=undefined && me.config_data.columns[i].renderFun!=''){
                me.config_data.columns[i].renderer=me.config_data.columns[i].renderFun;
             }
        } 
        var topBar = Ext.create('Ext.toolbar.Toolbar', {
            xtype: 'toolbar',
            dock: 'top'
        });
        Ext.apply(this, {
            store: store,
            stripeRows: true,
            columnLines: true,
            title: me.title,
            autoHeight: true,
            columns: me.config_data.columns,
            dockedItems: [me.config_data.isPageBar == true ? pageBar : {}, me.config_data.isTopBar == true ? topBar : {}]
        });
        if (me.config_data.isPageBar) {
            pageBar.bind(store);
            pageBar.moveFirst();
        }
        //this.reconfigure(store, me.config_data.columns);
me.on('afterlayout',function(thiz,layout,eOpts){
           	//修复4.2 grid render size不够导致scrollbar显示不出来问题
        	if(thiz.view.el.getHeight()>thiz.view.lastBox.height){
        		thiz.body.setHeight(thiz.view.getHeight());
            	thiz.el.setHeight(thiz.lastBox.height+18);
            	var parent=thiz.ownerCt;
            	parent.body.setHeight(thiz.el.getHeight());
            	parent.el.setHeight(parent.lastBox.height+18);
            	thiz.dockedItems.get(1).el.setTop(thiz.dockedItems.get(1).lastBox.y+18);
        	}
    		})
        this.callParent(arguments);
    }
});