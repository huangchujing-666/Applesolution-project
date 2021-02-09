Ext.Loader.setPath('Ext.app', '../UIContent/modules/portal/js');
Ext.Loader.setPath('Chart', '../UIContent');
        

Ext.require('Chart.ux.Highcharts');
Ext.require('Chart.ux.Highcharts.Serie');
Ext.require('Chart.ux.Highcharts.AreaRangeSerie');
Ext.require('Chart.ux.Highcharts.AreaSerie');
Ext.require('Chart.ux.Highcharts.AreaSplineRangeSerie');
Ext.require('Chart.ux.Highcharts.AreaSplineSerie');
Ext.require('Chart.ux.Highcharts.BarSerie');
Ext.require('Chart.ux.Highcharts.BoxPlotSerie');
Ext.require('Chart.ux.Highcharts.BubbleSerie');
Ext.require('Chart.ux.Highcharts.ColumnRangeSerie');
Ext.require('Chart.ux.Highcharts.ColumnSerie');
Ext.require('Chart.ux.Highcharts.ErrorBarSerie');
Ext.require('Chart.ux.Highcharts.FunnelSerie');
Ext.require('Chart.ux.Highcharts.GaugeSerie');
Ext.require('Chart.ux.Highcharts.LineSerie');
Ext.require('Chart.ux.Highcharts.PieSerie');
Ext.require('Chart.ux.Highcharts.RangeSerie');
Ext.require('Chart.ux.Highcharts.ScatterSerie');
Ext.require('Chart.ux.Highcharts.SplineSerie');
Ext.require('Chart.ux.Highcharts.WaterfallSerie');

//Ext.require([
//    'Ext.tip.QuickTipManager',
//    'Ext.menu.*'
//]);
Ext.define('com.palmary.portal.js.index', {
    //生成工具栏
    getTools: function () {
        return [{
            xtype: 'tool',
            type: 'gear',
            handler: function (e, target, panelHeader, tool) {
                var portlet = panelHeader.ownerCt;
                portlet.setLoading('Loading...');
                if (portlet.items.items.length > 0 && portlet.items.items[0].store != undefined)
                    portlet.items.items[0].store.load();
                Ext.defer(function () {
                    portlet.setLoading(false);
                }, 2000);
            }
        }];
    },
    //判断下拉Menu是否有子节点
    hasChild: function (data_json, id) {
        var f = false;
        for (var i = 0; i < data_json.items.length; i++) {
            if (data_json.items[i].parentId == id) {
                f = true;
                break;
            }
        }
        return f;
    },
    //下拉Menu按钮点击事件
    menuItemFun: function (btn, menu, tabpanel) {
        var me = this;
        var index = 0, min = 0, len = [];
        for (var i = 0; i < tabpanel.items.items.length; i++) {
            len[i] = tabpanel.items.items[i].items.items.length;
        }
        min = Ext.Array.min(len);
        for (var j = 0; j < len.length; j++) {
            if (len[j] == min) {
                index = j;
                break;
            }
        }
        var isClose = false;
        if (btn.isClose != null && btn.isClose == 1)
            isClose = true;
        if (btn.linkUrl != null && btn.linkUrl != '' && btn.linkUrl != undefined) {
            var pid = btn.id.split('-')[1];
            tabpanel.items.items[index].add({
                title: btn.text,
                iconCls: btn.iconCls,
                y: 30,
                tools: this.getTools(),
                closable: isClose,
                items: Ext.create(btn.linkUrl),
                linkUrl: btn.linkUrl,
                linkType: btn.btnType,
                isClose: btn.isClose,
                isCollopse: false,
                collapsed: false,
                pId: pid,
                listeners: {
                    'close': function (panel, obj) {
                        //Ext.bind(me.onPortletClose, me, [panel,btn,menu])
                        me.onPortletClose(panel, btn, menu);
                    },
                    'afterrender': function (panel, obj) {
                        Ext.bind(me.refreshMenu, me, [btn, menu, panel])
                    },
                    'collapse': function (panel, obj) {
                        me.Pcollapse(panel);
                    },
                    'expand': function (panel, obj) {
                        me.Pexpand(panel);
                    }
                }
            });
            me.SaveLayoutFun(false);
        }
        //tabpanel.items.items[index].doLayout();
        //tabpanel.superclass.doLayout.call(this);
    },
    //点击按钮后刷新下拉Menu
    refreshMenu: function (btn, thiz, panel) {
        if (btn != undefined) {
            if (btn.level == 1) {
                //thiz.menu.remove(btn);
                btn.hide();
            } else {
                //btn.ownerCt.remove(btn);
                var hideCount = 0;
                Ext.each(btn.ownerCt.items.items, function (o, i) {
                    if (o.hidden == true)
                        hideCount++;
                });
                if (btn.ownerCt.items.items.length == (1 + hideCount)) {
                    btn.ownerCt.ownerItem.hide();
                    btn.hide();
                } else {
                    btn.hide();
                }
            } 
        }
    },
    Pcollapse: function (panel) {
        panel.isCollapse = true;
    },
    Pexpand: function (panel) {
        panel.isCollapse = false;
    },
    initTag: function (tab) {
   
        var me = this;
        var tabpanel = Ext.getCmp('docs_ctagpanel');
        var itemList = [{
                items: []
            }, {
                items: []
            }
        ];

        // toolbar
        var tb = Ext.create('Ext.toolbar.Toolbar', {
            id: 'dashboard_portal',
            x: 0,
            y: 1,
            renderTo: tab.getEl(),
            floating: true,
            width: '100%',
            margin: '0 0 0 0',
            items: [
                //{
                //text: 'Add',
                //iconCls: 'iconAdd',
                //id: 'db_portlet_add',
                //menu: {},
                //listeners: {
                //    'afterrender': function (thiz, e, obj) {
                //        Ext.Ajax.request({ // load all portlets
                //            url: '../UIContent/modules/portal/list_all_portlet.js',
                //            async: false,
                //            success: function (o) {
                //                var data_json = Ext.decode(o.responseText);
                //                for (var i = 0; i < data_json.items.length; i++) {
                //                    if (data_json.items[i].level == 1) {
                //                        thiz.menu.add({
                //                            id: 'menu-' + data_json.items[i].id,
                //                            text: data_json.items[i].title,
                //                            iconCls: data_json.items[i].icon,
                //                            linkUrl: data_json.items[i].url,
                //                            isClose: data_json.items[i].isDelete,
                //                            level: data_json.items[i].level,
                //                            parentId: data_json.items[i].parentId,
                //                            btnType: data_json.items[i].portletType,
                //                            menu: me.hasChild(data_json, data_json.items[i].id) == true ? {} : undefined,
                //                            handler: function (btn) {
                //                                me.menuItemFun(btn, thiz, tabpanel);
                //                            }
                //                        });
                //                    } else {
                //                        Ext.getCmp('menu-' + data_json.items[i].parentId).menu.add({
                //                            id: 'menu-' + data_json.items[i].id,
                //                            text: data_json.items[i].title,
                //                            menu: me.hasChild(data_json, data_json.items[i].id) == true ? {} : undefined,
                //                            iconCls: data_json.items[i].icon,
                //                            linkUrl: data_json.items[i].url,
                //                            isClose: data_json.items[i].isDelete,
                //                            level: data_json.items[i].level,
                //                            parentId: data_json.items[i].parentId,
                //                            btnType: data_json.items[i].portletType,
                //                            handler: function (btn) {
                //                                me.menuItemFun(btn, thiz, tabpanel);
                //                            }
                //                        });
                //                    }
                //                }
                //            }
                //        });
                //    }
                //}
                //},
            {
                text: 'Reload',
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

        // portlets
        Ext.Ajax.request({
            url: '../UIContent/modules/portal/show_dashboard_data.js',
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                if (data_json.data != null && data_json.data != '')
                    itemList = me.getLayoutFun(itemList, data_json);
            }
        });
        tabpanel.add(itemList);
    },
    //获取portlet的布局
    getLayoutFun: function (itemList, data_json) {
        var me = this;
        var dataList = data_json.data;
        Ext.Array.sort(dataList, function (a, b) {
            return a.y - b.y;
        });
        for (var i = 0; i < dataList.length; i++) {
            var isClose = false;
            if (dataList[i].isClose != null && dataList[i].isClose == 1)
                isClose = true;
            var isExpand = true;
            if (dataList[i].isCollapse != null && dataList[i].isCollapse == true)
                isExpand = false;
            //	          for(var j=0;j<dataList[i].y;j++){
            //	              if(j==dataList[i].y-1){
            if (dataList[i].url != null && dataList[i].url != '' && dataList[i].url != 'undefined') {
                itemList[dataList[i].x - 1].items.push({
                    title: dataList[i].title,
                    iconCls: dataList[i].iconCls,
                    items: Ext.create(dataList[i].url),
                    linkUrl: dataList[i].url,
                    linkType: dataList[i].type,
                    pId: dataList[i].pId,
                    isClose: dataList[i].isClose,
                    isCollapse: dataList[i].isCollapse,
                    closable: isClose,
                    collapsed: isExpand == true ? false : true,
                    tools: this.getTools(),
                    y: 30,
                    listeners: {
                        'close': function (panel, obj) {
                            //Ext.bind(me.onPortletClose, me,[panel])
                            me.onPortletClose(panel, Ext.getCmp('menu-' + panel.pId), Ext.getCmp('db_portlet_add'));
                        },
                        'afterrender': function (panel, obj) {
                            me.refreshMenu(Ext.getCmp('menu-' + panel.pId), Ext.getCmp('db_portlet_add'), panel);
                        },
                        'collapse': function (panel, obj) {
                            me.Pcollapse(panel);
                        },
                        'expand': function (panel, obj) {
                            me.Pexpand(panel);
                        }
                    }
                });
            }
        }
        return itemList;
    },
    //关闭portlet处理
    onPortletClose: function (panel, btn, menu) {
        //panel.destroy();
        panel.ownerCt.remove(panel, true)
        this.SaveLayoutFun(false);
        if (btn.parentMenu) {
            if (btn.parentMenu.parentItem)
                btn.parentMenu.parentItem.show();
        }
        Ext.getCmp(btn.id).show();
    },
    //portlet布局保存函数
    SaveLayoutFun: function (f) {
        var portalPanel = Ext.getCmp('docs_ctagpanel');
        var layout_json = { data: [] };
        for (var i = 0; i < portalPanel.items.items.length; i++) {
            for (var j = 0; j < portalPanel.items.items[i].items.items.length; j++) {
                var sub_portlet = {};
                sub_portlet.title = portalPanel.items.items[i].items.items[j].title;
                sub_portlet.iconCls = portalPanel.items.items[i].items.items[j].iconCls;
                sub_portlet.isClose = portalPanel.items.items[i].items.items[j].isClose;
                sub_portlet.url = portalPanel.items.items[i].items.items[j].linkUrl;
                sub_portlet.type = portalPanel.items.items[i].items.items[j].linkType;
                sub_portlet.pId = portalPanel.items.items[i].items.items[j].pId;
                sub_portlet.isCollapse = portalPanel.items.items[i].items.items[j].collapsed == false ? false : true;
                sub_portlet.y = j;
                sub_portlet.x = i + 1;
                layout_json.data.push(sub_portlet);
            }
        }
        Ext.Ajax.request({
            url: '/ctm_service/portal/update_dashboard.aspx',
            async: false,
            params: { layout_json: Ext.encode(layout_json) },
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                if (f)
                    Ext.Msg.alert("Msg", "Save Success!");
            }
        });
    },
    //刷新整个Dashboard
    ReloadFun: function (tabpanel) {
        var myMask = new Ext.LoadMask(tabpanel.el.dom, { msg: "Please wait..." });
        myMask.show();
        var me = this;
        var itemList = [{
            items: []
        }, {
            items: []
        }];
        var portalPanel = tabpanel;
        for (var i = 0; i < portalPanel.items.items.length; i++) {
            for (var j = 0; j < portalPanel.items.items[i].items.items.length; j++) {
                var portlet = portalPanel.items.items[i].items.items[j];
                portlet.ownerCt.removeAll();
            }
            portalPanel.removeAll();
        }
        Ext.Ajax.request({
            url: '../UIContent/modules/portal/show_dashboard_data.js',
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                if (data_json.data != null && data_json.data != '')
                    itemList = me.getLayoutFun(itemList, data_json);
            }
        });
        portalPanel.add(itemList);
        myMask.destroy();
        //me.SaveLayoutFun(false);
    }
});