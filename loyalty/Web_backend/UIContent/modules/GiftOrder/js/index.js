Ext.define('com.palmary.giftorder.js.index', {
    gridPanel: undefined,
    addTag: function () {
        
    },
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        target_div.style.margin = "5px";

        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);
        grider_div.style.margin = "5px";

        var select_category_value;

        if (itemId != undefined) {
            select_category_value = parseInt(itemId);
        } else
            select_category_value = null;

        var store = Ext.create('Ext.data.Store', {
            fields: [
                        { name: 'id', type: 'int' },
                        { name: 'value', type: 'string' }
            ],
            autoLoad: true,
           // id: 'theStore',
            proxy: {
                type: 'ajax',
                url: '../GiftOrder/GetListItems', 
                reader: {
                    type: 'json',
                    root: 'data'
                }
            }
        });

        var first_load = true;

        Ext.create('Ext.panel.Panel', {

            grid_panel: undefined,
            id: 'giftOrderPanel',
            renderTo: target_div,
            title: 'Gift Order',
            height: 100,
            bodyStyle: 'padding:5px 5px 0',
            defaults: {
                layout: 'Anchor',
                labelWidth: 150
            },
            buttonAlign: 'left',
            buttons: [
                Ext.create('Ext.Button', {
                    text: 'Search',
                    id: 'giftOrder_search_button',
                    iconCls: '',
                    style: {
                        float: 'left'
                    },
                    handler: function () {

                        var select = Ext.getCmp('gift_order_category_select');
                        var myparams = { category_id: select.value };

                        Ext.Ajax.request({
                            url: '../GiftOrder/GetGridHeader',
                            async: false,
                            success: showGrid,
                            method: 'POST',
                            scope: this
                        });

                        function showGrid(o) {

                            var gird_info = Ext.decode(o.responseText);
                            //add button in grid
                            //this.add_hidden = true;
                            //if (gird_info.add_hidden != null) {
                            //    this.add_hidden = gird_info.add_hidden;
                            //}

                            if (first_load == true) {
                                // first load
                                var giftOrderPanel = Ext.getCmp('giftOrderPanel');
                                giftOrderPanel.grid_panel = Ext.create('com.embraiz.component.gird', {
                                    grid_id: 'giftOrderPanel_grid',
                                    grider_div: grider_div,
                                    json_data: gird_info,
                                    header_str: Ext.encode(gird_info.columns),
                                    grid_url: '../GiftOrder/ListData',
                                    params: myparams,
                                    form_div: target_div
                                });

                                first_load = false;
                            }
                            else {
                                // not first load
                                var giftOrderPanel = Ext.getCmp('giftOrderPanel');
                                giftOrderPanel.grid_panel.store.load({
                                    url: '../GiftOrder/ListData',
                                    params: myparams
                                });
                            }
                        };
                    }
                })
            ],
            items: [
	            {
	                xtype: 'combo',
	                fieldLabel: 'Gift Category',
	                width: 400,
	                id: 'gift_order_category_select',
	                store: store,
	                queryMode: 'local',
	                emptyText: 'Please Selected',
	                displayField: 'value',
	                valueField: 'id',
	                renderTo: target_div,
	                value: select_category_value,
	                listeners: {
	                }
	            }
            ]
        });

        if (itemId != undefined) { //simulate button click after updating display_order
            var myButton = Ext.getCmp('giftOrder_search_button');
            myButton.handler.call(myButton.scope, myButton, Ext.EventObject);
        }
    }
});