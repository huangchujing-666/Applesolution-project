Ext.define('com.palmary.report.js.memberProfileDistribution', {
    
    gridPanel: undefined,
    addTag: function () {
        
    },
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        var me = this;
        var tabpanel = Ext.getCmp('docs_ctagpanel');

        var store = Ext.create('Ext.data.Store', {
            fields: [{
                name: 'id',
                type: 'string'
            }, {
                name: 'value',
                type: 'string'
            }],
            remoteSort: true,
            autoLoad: false,
            id: 'store_member_level',
            proxy: {
                type: 'ajax',
                url: '../Table/GetListItems/reportmemberdemographictype',
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

        store.on('beforeload', function (thiz, action, value) {
            //if (thiz.getCount() == 0 && value && value != '') {
            //    thiz.proxy.extraParams.defaultValue = value;
            //}
        }, true, "");

        var temp_select = Ext.create('com.embraiz.component.ComboBox', {
            fieldLabel: "Main type",
            labelStyle: "labelCss",
            plugins: ['clearbutton'],
            forceSelection: true,
            //sub_component: sub_component,
            name: "main_type",
            emptyText: 'Please Select',
            displayField: 'value',
            valueField: 'id',
            forceSelection: true,
            store: 'store_member_level',
            init_value: "",
            value: "",
            allowBlank: true, //allowBlank,
           // tabIndex: json_data.row[i].tabIndex,
            init_load: 0,
            queryMode: 'local',
            //hidden: json_data.row[i].hidden,
            listeners: {
                render: function () {
                    var temp = this;
                    temp.store.load(function (records, operation, success) {
                        temp.setValue("");
                        temp.init_load == 1;
                    });
                }, beforequery: listener_ComboboxBeforeQuery
            }
        });

        var temp_select2 = Ext.create('com.embraiz.component.ComboBox', {
            fieldLabel: "Sub type",
            labelStyle: "labelCss",
            plugins: ['clearbutton'],
            forceSelection: true,
            //sub_component: sub_component,
            name: "sub_type",
            emptyText: 'Please Select',
            displayField: 'value',
            valueField: 'id',
            forceSelection: true,
            store: 'store_member_level',
            init_value: "",
            value: "",
            allowBlank: true, //allowBlank,
            // tabIndex: json_data.row[i].tabIndex,
            init_load: 0,
            queryMode: 'local',
            //hidden: json_data.row[i].hidden,
            listeners: {
                render: function () {
                    var temp = this;
                    temp.store.load(function (records, operation, success) {
                        temp.setValue("");
                        temp.init_load == 1;
                    });
                }, beforequery: listener_ComboboxBeforeQuery
            }
        });

        // toolbar
        var tb = Ext.create('Ext.toolbar.Toolbar', {
            //  id: 'memberUpgrade_toolbar',
            x: 0,
            y: 1,
            renderTo: tab.getEl(),
            floating: true,
            width: '100%',
            margin: '0 0 0 0',
            items: [
                temp_select, temp_select2,
            {
                text: 'Generate',
                iconCls: 'iconReload',
                handler: function () {
                    me.ReloadFun(tabpanel);
                }
            }
            //, {
            //    text: 'Save Dashboard Layout',
            //    iconCls: 'iconSave',
            //    handler: function () {
            //        me.SaveLayoutFun(true);
            //    }
            //}
            ]
        }).toFront(true);
        // [END] toolbar

        var itemList = [
            {
                items: []
            },
            {
                items: []
            }
        ];

        itemList[0].items.push({
            title: "Member Profile Demographic",
            iconCls: "iconGrid",
            items: Ext.create("com.palmary.portlet.memberProfileDistributionPortlet"),
            linkUrl: "com.palmary.portlet.memberProfileDistributionPortlet",
            linkType: "grid",
            pId: "memberProfileDemographicReport",
            isClose: true,
            isCollapse: false,
            closable: false,
            collapsed: false,
            //tools: this.getTools(),
            y: 0,
            padding: "40px 0px 0px 0px",
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