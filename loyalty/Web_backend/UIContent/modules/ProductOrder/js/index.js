Ext.define('com.palmary.productorder.js.index', {
    gridPanel: undefined,
    addTag: function () {
        new com.embraiz.tag().openNewTag('ProductOrder:401', 'Product Order:Add', 'com.palmary.productorder.js.insert', 'iconUser16', 'user:add');
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

        if (itemId != undefined)
        {
            select_category_value = parseInt(itemId);
        } else
            select_category_value = null;

        var store = Ext.create('Ext.data.Store', {
            fields: [
                        { name: 'id', type: 'int' },
                        { name: 'value', type: 'string' }
            ],
            autoLoad: true,
            //id: 'theStore',
            proxy: {
                type: 'ajax',
                url: '../Table/GetListItems/productcategory', ///modules/assign/list_role.json',
                reader: {
                    type: 'json',
                    root: 'data'
                }
            }
        });

        var first_load = true;
      
        Ext.create('Ext.panel.Panel', {
            
            grid_panel: undefined,
            id: 'productOrderPanel',
            renderTo: target_div,
            title: 'Product Order',
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
                    id: 'productOrder_search_button',
                    iconCls: '',
                    style: {
                        float: 'left'
                    },
                    handler: function () {
                       
                        var select = Ext.getCmp('productOrder_category_select');
                        var myparams = { category_id: select.value };

                        Ext.Ajax.request({
                            url: '../ProductOrder/GetGridHeader',
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
                                var productOrderPanel = Ext.getCmp('productOrderPanel');
                                productOrderPanel.grid_panel = Ext.create('com.embraiz.component.gird', {
                                    grid_id: 'productOrder_grid',
                                    grider_div: grider_div,
                                    json_data: gird_info,
                                    header_str: Ext.encode(gird_info.columns),
                                    grid_url: '../ProductOrder/ListData',
                                    params: myparams,
                                    form_div: target_div
                                });

                                first_load = false;
                            }
                            else
                            {
                                // not first load
                                var productOrderPanel = Ext.getCmp('productOrderPanel');
                                productOrderPanel.grid_panel.store.load({
                                    url: '../ProductOrder/ListData',
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
	                fieldLabel: 'Product Category',
	                width: 400,
	                id: 'productOrder_category_select',
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
            var myButton = Ext.getCmp('productOrder_search_button');
            myButton.handler.call(myButton.scope, myButton, Ext.EventObject);
        }
        
    }
});