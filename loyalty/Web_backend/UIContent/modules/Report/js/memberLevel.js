Ext.define('com.palmary.report.js.memberlevel', {
    
    gridPanel: undefined,
    addTag: function () {
        
    },
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        var me = this;
        var tabpanel = Ext.getCmp('docs_ctagpanel');

        var itemList = [
            {
                items: []
            },
            {
                items: []
            }
        ];

        itemList[0].items.push({
            title: "Member Distribution",
            iconCls: "iconGrid",
            items: Ext.create("com.palmary.portlet.memberDistributionReportPortlet"),
            linkUrl: "com.palmary.portlet.memberDistributionReportPortlet",
            linkType: "grid",
            pId: "memberLevelReport",
            isClose: true,
            isCollapse: false,
            closable: false,
            collapsed: false,
            //tools: this.getTools(),
            y: 0,
           // width: 400,
            listeners: {
                //'close': function (panel, obj) {
                //    //Ext.bind(me.onPortletClose, me,[panel])
                //    me.onPortletClose(panel, Ext.getCmp('menu-' + panel.pId), Ext.getCmp('db_portlet_add'));
                //},
                //'afterrender': function (panel, obj) {
                //    me.refreshMenu(Ext.getCmp('menu-' + panel.pId), Ext.getCmp('db_portlet_add'), panel);
                //},
                //'collapse': function (panel, obj) {
                //    me.Pcollapse(panel);
                //},
                //'expand': function (panel, obj) {
                //    me.Pexpand(panel);
                //}
            }
        });

        var container = Ext.create('Ext.container.Container', {
            anchor: '100%',
            //id: 'myForm',
            layout: {
                type: 'table',
                columns: 1,
                tableAttrs: {
                    style: {
                        width: '100%',
                        margin: '0px 5px 5px 0px'
                    }
                },
                tdAttrs: {
                    style: {
                        width: '50%'
                    }
                }
            },
            items: itemList
           
        });

        tab.add(container);
    }
});