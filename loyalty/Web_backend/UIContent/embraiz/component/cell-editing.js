/*

This file is part of Ext JS 4

Copyright (c) 2011 Sencha Inc

Contact:  http://www.sencha.com/contact

GNU General Public License Usage
This file may be used under the terms of the GNU General Public License version 3.0 as published by the Free Software Foundation and appearing in the file LICENSE included in the packaging of this file.  Please review the following information to ensure the GNU General Public License version 3.0 requirements will be met: http://www.gnu.org/copyleft/gpl.html.

If you are unsure which license is appropriate for your use, please contact the sales department at http://www.sencha.com/contact.

*/
Ext.Loader.setConfig({
    enabled: true
});


Ext.require(['Ext.selection.CellModel', 'Ext.grid.*', 'Ext.data.*', 'Ext.util.*', 'Ext.state.*', 'Ext.form.*', 'Ext.ux.CheckColumn']);

Ext.onReady(function () {
    Ext.define('com.embraiz.component.editGrid', {
    	gird:undefined,
        grider_div: undefined,
        json_data: undefined,
        json_data_url: undefined,
        bbar: undefined,
        selModel:undefined,
        store:undefined,
        params:undefined,
        formatDate: function (value) {
            return value ? Ext.Date.dateFormat(value, 'M d, Y') : '';
        },
        constructor: function (config) {
            Ext.tip.QuickTipManager.init();
            config = config || {};
            Ext.apply(this, config);
            this.callParent([config]);
            var me = this;
            me.grider_div.style.margin = "5px";

            Ext.Ajax.request({
                url: me.json_data_url,
                async: false,
                success: function (o) {
                    var grid_data = Ext.decode(o.responseText);
                    me.json_data = grid_data;
                }
            });

            Ext.Ajax.request({
                url: me.json_data.post_header,
                success: showGrid
            });
            var dt = new Date();

            function showGrid(o) {
                var grid_info = Ext.decode(o.responseText);

                var model = Ext.define('Plant_' + dt, {
                    extend: 'Ext.data.Model',
                    fields: grid_info.fields
                });
                var store = me.store = Ext.create('Ext.data.Store', {
                    model: 'Plant_' + dt,
                    autoDestroy: true,
                    remoteSort: true,
                    autoLoad: true,
                    pageSize: grid_info.pageSize,
                    proxy: {
                        type: 'ajax',                        
                        url: me.json_data.post_url,                      
                        reader: {
                            type: 'json',
                            root: 'items',
                            totalProperty: 'totalCount'
                        }
                    }
                });

                me.bbar = Ext.create('com.embraiz.component.extPaging', {
                    store: me.store,
                    displayInfo: true,
                    displayMsg: 'Displaying topics {0} - {1} of {2}',
                    emptyMsg: "No topics to display",
                    url: me.json_data.post_url,
                    params: me.params,
                    items: ['-']
                });
                var checkboxEvent = 0;
                //check box model
                var tempUrl = grid_info.delete_url;

                var selModel = null;
                if (grid_info.checkbox_hidden != true) {
                    checkboxEvent = 1;
                    me.selModel=selModel = Ext.create('Ext.selection.CheckboxModel', {
                        checkOnly: true,
                        listeners: {
                            selectionChange: function (sm, selections) {
                                if (grid.down('#editRemoveButton') != null) {
                                    grid.down('#editRemoveButton').setDisabled(selections.length == 0 || tempUrl == null || tempUrl == undefined);
                                }
                            },
                            select: function (sm, selections) {
                                if (grid.down('#editRemoveButton') != null) {
                                    grid.down('#editRemoveButton').setDisabled(selections.length == 0 || tempUrl == null || tempUrl == undefined);
                                }
                            }
                        }
                    });
                }
                var cellEditing = Ext.create('com.embraiz.component.CellEditing', {
                    clicksToEdit: 1,
                    checkbox: checkboxEvent
                });

                //generate grid's columns
                var form_columns = [];

                for (i = 0; i < grid_info.columns.length; i++) {
                    var temp_element = {};
                    var columns_config = grid_info.columns[i];
                    temp_element.header = columns_config.header;
                    temp_element.dataIndex = columns_config.dataIndex;
                    temp_element.width = columns_config.width;
                    temp_element.type = columns_config.type;
                    temp_element.flex = columns_config.flex;
                    temp_element.hidden = columns_config.hidden || columns_config.hidden;
                    if (columns_config.formula != undefined) {

                        temp_element.renderer = function (value, meta, record) {

                            var forumla_arr = columns_config.formula.split("*");
                            if (forumla_arr.length = 2) {
                                var a = record.get(forumla_arr[0]) * record.get(forumla_arr[1]);
                                return a;
                            } else {
                                forumla_arr = columns_config.formula.split("+");
                                if (forumla_arr.length = 2) {
                                    var a = record.get(forumla_arr[0]) + record.get(forumla_arr[1]);
                                    return a;
                                }
                            }

                        }
                    }
                    var cellStore = null;
                    if (columns_config.type == 'combobox') {
                        var cellfields = [{
                            name: 'id',
                            type: 'string'
                        }, {
                            name: 'value',
                            type: 'string'
                        }];
                        if (columns_config.fields != undefined) {
                            cellfields = columns_config.fields;
                        }
                        cellStore = Ext.create('Ext.data.Store', {
                            fields: cellfields,
                            autoLoad: true,
                            proxy: {
                                type: 'ajax',
                                url: columns_config.url,
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

                  /*  temp_element.field = {
                        allowBlank: columns_config.allowBlank,
                        xtype: columns_config.type,
                        typeAhead: columns_config.typeAhead,
                        triggerAction: columns_config.triggerAction,
                        selectOnTab: columns_config.selectOnTab,
                        queryMode: 'local',
                        displayField: 'value',
                        valueField: 'id',
                        store: cellStore,
                        select: function () {
                            var we = me;
                            me.store.data.items[0].data.email = this.store.data.items[0].data.email;
                            //alert(this.store.data.items[0].data.email);
                        }
                    };*/
                     
                    cellStore.load();
                    temp_element.field = Ext.create('Ext.form.field.ComboBox',{ 
                    	allowBlank: columns_config.allowBlank==undefined?true:columns_config.allowBlank,	
                        name:columns_config.dataIndex,
                        forceSelection:true,
                        selectOnFocus:true,                       
                        queryMode: 'local',
                        displayField: 'value',
                        valueField: 'id',
                        store: cellStore,
                        listeners:{beforequery:listener_ComboboxBeforeQuery}
                  });
                    function listener_ComboboxBeforeQuery(e) {
                    	var combo = e.combo;
                    	try {
                    		var value = e.query;
                    		combo.lastQuery = value;
                    		combo.store.filterBy(function(record, id) {
                    			var text = record.get(combo.displayField);
                    			return (text.toLowerCase().indexOf(value.toLowerCase()) != -1); // ignore upper / lower case.
                    		});
                    		combo.onLoad();
                    		combo.expand();
                    		return false;
                    	} catch (e) {
                    		combo.selectedIndex = -1;
                    		combo.store.clearFilter();
                    		combo.onLoad();
                    		combo.expand();
                    		return false;
                    	}
                    }
                    if(columns_config.handler!=undefined&&columns_config.handler!=''){
                    	   var eventName='select';
                    	   if(columns_config.eventName){
                    		   eventName=columns_config.eventName;
                    	   }
                      	   temp_element.editor.addListener(eventName,columns_config.handler);
                      	}
                   temp_element.renderer=function(value,metadata,record,rowIndex,colIndex){
                	    var combox=this.columns[colIndex].editor;
                	    if(combox==undefined){
                	    	combox=this.columns[colIndex].field;
                	    }
            	    	var index=combox.store.find(combox.valueField,value);
                	    var record=combox.store.getAt(index);
                	    var displayText = "";
                        if (record == null) {
                    	    displayText = value;
                	    } else {
                		    displayText = record.data.value;
                	    }				                    	           
                	    return displayText;
                	 } 
                    }else if(columns_config.type == 'datefield'){
                   	 temp_element.xtype='datecolumn';
                	 temp_element.format='Y-m-d';
                    	temp_element.field = {
                                xtype: 'datefield',
                                allowBlank: columns_config.allowBlank==undefined?true:columns_config.allowBlank,
				                format: 'Y-m-d',
				                readOnly:columns_config.readOnly==undefined?false:columns_config.readOnly
                      };
	                }else if(columns_config.type == 'checkbox'){
	                		  temp_element.xtype= 'checkcolumn';						            
							      temp_element.field={
							                xtype: 'checkbox',
							                readOnly:columns_config.readOnly==undefined?false:columns_config.readOnly
							             };
	                }else if(columns_config.type == 'currency'){
	          		  temp_element.xtype= 'numbercolumn';
	          		  temp_element.format= '$0,0.00';
	          		  temp_element.field={
		                xtype: 'numberfield',
		                readOnly:columns_config.readOnly==undefined?false:columns_config.readOnly
	          		  };
			            
	                }else if(columns_config.type == 'numberfield'){
	              		  temp_element.field=Ext.create('Ext.form.field.Number',{    		                
			                readOnly:columns_config.readOnly==undefined?false:columns_config.readOnly
	              		  });
	              		if(columns_config.handler!=undefined&&columns_config.handler!=''){
	                	    var eventName='change';
	                	    if(columns_config.eventName){
	                		   eventName=columns_config.eventName;
	                	    }
	                      	temp_element.field.addListener(eventName,columns_config.handler);
	                   	} 
	                }else if(columns_config.type == 'timefield'){
				            temp_element.field={
				                xtype: 'timefield',
				                format:'H:i',
				                readOnly:columns_config.readOnly==undefined?false:columns_config.readOnly
				             };
				            temp_element.renderer=function(value,metadata,record,rowIndex,colIndex){
				            	if(value instanceof Date){
				            		value=Ext.Date.format(value,'H:i');
				            	}
				            	return value;
				            }              
                    	
                    }else{
                    	temp_element.field = {
                                allowBlank: columns_config.allowBlank,
                                xtype: columns_config.type,
                                typeAhead: columns_config.typeAhead,
                                triggerAction: columns_config.triggerAction,
                                selectOnTab: columns_config.selectOnTab,
                                queryMode: 'local',
                                displayField: 'value',
                                valueField: 'id',
                                store: cellStore,
                                select: function () {
                                    var we = me;
                                    me.store.data.items[0].data.email = this.store.data.items[0].data.email;
                                    //alert(this.store.data.items[0].data.email);
                                }
                            };
                    }
                    form_columns[i] = temp_element;
                }

                //gird_toolbar
                var grid_toolbar = Ext.create('Ext.toolbar.Toolbar', {
                    xtype: 'toolbar'
                });
                if (!grid_info.checkbox_hidden && !grid_info.delete_hidden) {
                    grid_toolbar.add({
                        itemId: 'editRemoveButton',
                        text: 'Remove',
                        tooltip: 'Remove the selected item',
                        iconCls: 'iconDelete',
                        // hidden: grid_info.checkbox_hidden,
                        disabled: true,
                        handler: function () {
                            var tempUrl = grid_info.delete_url;
                            Ext.MessageBox.confirm('Confirm', 'Are you sure you want to delete?', function (btn) {
                                if (btn == 'yes') {
                                    var selectedRecord = selModel.getSelection();
                                    var selectedValue = "0";
                                    for (var i = 0; i < selectedRecord.length; i++) {
                                        selectedValue = selectedValue + "," + selectedRecord[i].get('id');
                                    }
                                    //call delete function here
                                    Ext.Ajax.request({
                                        url: tempUrl,
                                        params: {
                                            id: selectedValue
                                        },
                                        callback: function (o, s, r) {
                                            eval('var text=' + r.responseText);

                                            if (text.flag == "1") {
                                                Ext.Msg.alert('Success', 'Success');
                                                store.loadPage(store.currentPage);

                                            } else {
                                                Ext.Msg.alert('Success', Fail);
                                            }
                                        }

                                    });

                                } else {
                                    this.close();
                                }
                            });
                        }
                    });
                }
                ///
                if (!grid_info.add_hidden) {
                    grid_toolbar.add({
                        text: grid_info.add_button_text,
                        iconCls: 'iconAdd',
                        //hidden: grid_info.add_hidden,
                        handler: function () {
                            // Create a record instance through the ModelManager
                            var r = Ext.ModelManager.create(grid_info.add_row, 'Plant_' + dt);
                            store.insert(0, r);
                            cellEditing.startEditByPosition({
                                row: 0,
                                column: checkboxEvent
                            });
                            // if(grid.selModel!=null){                
                            grid.selModel.deselect(0, true, true);
                            //	}
                        }
                    });
                }
                if (!grid_info.save_hidden) {
                    grid_toolbar.add({
                        text: 'Save',
                        iconCls: 'iconForm',
                        // hidden: grid_info.save_hidden,
                        handler: function () {
                            var data = grid.store;
                            if (!cellEditing.validateEdit()) {
                                return;
                            }

                            var returnParam = "{item:" + data.getCount() + ",";
                            for (var i = 0; i < data.getCount(); i++) {
                                var record = data.getAt(i);
                                var field_name = grid_info.fields;
                                var columns = grid_info.columns;
                                for (var j = 0; j < columns.length; j++) {
                                    if (record.data['id'] == "" && columns[j].allowBlank == false) {
                            
                                        var columnHeader = grid.headerCt.getHeaderAtIndex(j);
                                        var context = cellEditing.getEditingContext(data.getAt(i), columnHeader);
                                        if (context.value == "") {
                                            Ext.MessageBox.alert('Alert', 'This field is required!');
                                            cellEditing.setActiveColumn(columnHeader);
                                            grid.getView().getEl(columnHeader).focus();
                                            cellEditing.startEdit(data.getAt(i), columnHeader);
                                            return;
                                        }
                                    }
                                }
                                for (var j = 0; j < field_name.length; j++) {
                                    returnParam = returnParam + field_name[j] + i + ":'" + record.data[field_name[j]] + "',";
                                }
                            }
                            returnParam = returnParam.substring(0, (returnParam.length - 1)) + "}";
                            var temp = Ext.decode(returnParam);
                            Ext.Ajax.request({
                                url: grid_info.save_url,
                                method: 'POST',
                                params: Ext.decode(returnParam),
                                callback: function (o, s, r) {
                                    eval('var text=' + r.responseText);
                                    if (text.flag == "1") {
                                        Ext.Msg.alert('Success', 'Success');
                                        store.loadPage(store.currentPage);

                                    } else {
                                        Ext.Msg.alert('Success', Fail);
                                    }
                                }

                            });
                        }
                    });
                }
                if (grid_info.button_items != null && grid_info.button_items != '') {
                    var json_data = grid_info;
                    for (var a = 0; a < grid_info.button_items.length; a++) {
                        grid_toolbar.add({
                            xtype: json_data.button_items[a].xtype,
                            text: json_data.button_items[a].text,
                            iconCls: json_data.button_items[a].iconCls,
                            name: json_data.button_items[a].handler,
                            handler: function (btn, e, op) {
                                var value='';
                                var ids = me.getCheckIds();
                                var data=grid.store;
                                var values = me.getCheckValues();/*
                                for (var i = 0; i < data.getCount(); i++) {
                                    var record = data.getAt(i);
          
                                    if (i != 0) {
				                        value = value + "," + record.data['payment'];
				                    } else {
				                        value = value + record.data['payment'];
				                    }
                                }*/                               
                          							   
                                Ext.decode(btn.name + "('" + ids + "')")
                            }
                        });
                    }
                }
                //grid
                var grid =me.gird= Ext.create('Ext.grid.Panel', {
                    store: me.store,
                    title: grid_info.title,
                    bbar: me.bbar,
                    iconCls: 'iconGridEdit',
                    selModel: selModel,
                    columns: form_columns,
                    stripeRows: true,
                    enableColumnHide: false,
                    renderTo: me.grider_div,
                   // height: 300,
                    frame: true,
                    tbar: grid_toolbar,
                    plugins: [cellEditing]
                });
            }
        },
        search: function (post_data) {
        	var me=this;
        	me.store.currentPage = 1;
            if (post_data != null && post_data.params != null) {
//            	me.params = post_data.params;
//            	me.store.clearFilter(true);
//            	me.store.filter(post_data.params);
//            	me.store.loadPage(1);    
            	me.store.load({
                    url: post_data.post_url,
                    params:post_data.params
                });               
            } else {
            	me.store.load({
                    url: post_data.post_url
                });
            }
        },
        getCheckIds: function () {
            var selModel = null;
            var selectedValue = "";
            if (this.selModel != null && this.selModel != undefined) {
                selModel = this.selModel;
                var selectedRecord = selModel.getSelection();
                for (var i = 0; i < selectedRecord.length; i++) {
                    if (i != 0) {
                        selectedValue = selectedValue + "," + selectedRecord[i].get('id');
                    } else {
                        selectedValue = selectedValue + selectedRecord[i].get('id');
                    }
                }
            }
            return selectedValue;
        },
        getCheckValues: function () {
           var selModel = null;
           var selectedValue = "";
           if (this.selModel != null && this.selModel != undefined) {
                selModel = this.selModel;
                var selectedRecord = selModel.getSelection();
                for (var i = 0; i < selectedRecord.length; i++) {
                    if (i != 0) {
                        selectedValue = selectedValue + "," + selectedRecord[i].data['payment'];
                    } else {
                        selectedValue = selectedValue + selectedRecord[i].data['payment'];
                    }
                }
            }
           return selectedValue;
        }
    })



});