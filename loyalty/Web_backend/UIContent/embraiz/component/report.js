Ext.Loader.setConfig({
    enabled: true
});
Ext.require(['Ext.grid.*', 'Ext.toolbar.Paging', 'Ext.util.*', 'Ext.data.*', 'Ext.state.*', 'Ext.form.*', 'Ext.selection.CellModel', 'Ext.ux.CheckColumn', 'Ext.selection.CheckboxModel', 'Ext.tab.*', 'Ext.window.*', 'Ext.tip.*', 'Ext.layout.container.Border']);
Ext.define('com.embraiz.component.report', {
    reportJson: undefined,
    simplefield: undefined,
    header_data: undefined,
    DataJson: undefined,
    reportId: undefined,
    reportName: undefined,
    simpleCombo: undefined,
    reportFilter: undefined,
    report_grid: undefined,
    field_grid: undefined,
    view_grid: undefined,
    editForm: undefined,
    value_json: undefined,
    fieldStore: undefined,
    viewStore: undefined,
    comStore: undefined,
    comStore: undefined,
    reportStore: undefined,
    viewReport: function (DataJson, reportId, reportName, valueurl) {
        this.DataJson = DataJson;
        this.reportId = reportId;
        Ext.Ajax.request({
            url: valueurl,
            success: this.reponseFun,
            scope: this
        });
    },
    reponseFun: function (o) {
        var me = this;
        var repID = this.reportId;
        var value_json = Ext.decode(o.responseText);
        this.initReport(this.DataJson, me.reportId, me.reportName);
        simplefield.setValue(value_json.data);
        comStore.proxy.url = this.DataJson.db_view_link + '?name' + Ext.decode(value_json.data).table[0].display_name + '';
        comStore.load();
        simpleCombo.setValue(Ext.decode(value_json.data).table[0].display_name);
        //simpleCombo.setDisabled(true);
        fieldStore.proxy.url = this.DataJson.field_link + '?reportId=' + repID;
        fieldStore.load();
        view_grid.getStore().proxy.url = me.DataJson.view_url + '?reportId=' + repID + '';
        view_grid.getStore().load();
    },
    initReport: function (DataJson, reportId, reportName) {
        var me = this;
        this.DataJson = DataJson;
        this.reportId = reportId;
        this.reportName = reportName;
        simplefield = Ext.create('Ext.form.field.TextArea', {
            name: 'json_detail',
            height: 80,
            anchor: '-0'
        });
        Ext.define('fieldModel', {
            extend: 'Ext.data.Model',
            fields: [{
                name: 'fieldname',
                mapping: 'fieldname'
            }, {
                name: 'fieldtype',
                mapping: 'fieldtype'
            }]
        });
        fieldStore = Ext.create('Ext.data.Store', {
            model: 'fieldModel',
            proxy: {
                type: 'ajax',
                url: '',
                reader: {
                    type: 'json',
                    root: 'field'
                }
            }
        });
        Ext.define('viewModel', {
            extend: 'Ext.data.Model',
            fields: [{
                name: 'name',
                type: 'string'
            }, {
                name: 'display_name',
                type: 'string'
            }, {
                name: 'output',
                type: 'bool'
            }, {
                name: 'group_by',
                type: 'string'
            }, {
                name: 'order_by',
                type: 'string'
            }, {
                name: 'sign',
                type: 'string'
            }, {
                name: 'condition',
                type: 'string'
            }, {
                name: 'fieldtype',
                type: 'string'
            }]
        });
        viewStore = Ext.create('Ext.data.Store', {
            model: 'viewModel',
            autoDestroy: true,
            proxy: {
                type: 'ajax',
                url: '',
                reader: {
                    type: 'json',
                    root: 'field'
                }
            }
        });
        var viewColumn = [{
            header: "name",
            width: 100,
            sortable: true,
            dataIndex: 'name',
            sortable: true
        }, {
            header: "display_name",
            width: 100,
            sortable: true,
            dataIndex: 'display_name',
            field: {
                xtype: 'textfield',
                allowBlank: false,
                name: 'display_name'
            }
        }, {
            xtype: 'checkcolumn',
            header: "output",
            width: 60,
            dataIndex: 'output'
        }, {
            header: "group_by",
            width: 80,
            sortable: true,
            dataIndex: 'group_by',
            sortable: true,
            field: {
                xtype: 'combobox',
                typeAhead: true,
                triggerAction: 'all',
                selectOnTab: true,
                store: [
                    ['Group By', 'Group By'],
                    ['Sum', 'Sum'],
                    ['Avg', 'Avg'],
                    ['Min', 'Min'],
                    ['Max', 'Max'],
                    ['Count', 'Count']
                ],
                lazyRender: true,
                listClass: 'x-combo-list-small'
            }
        }, {
            header: "order_by",
            width: 80,
            sortable: true,
            dataIndex: 'order_by',
            sortable: true,
            field: {
                xtype: 'combobox',
                typeAhead: true,
                triggerAction: 'all',
                selectOnTab: true,
                store: [
                    ['ASC', 'ASC'],
                    ['DESC', 'DESC']
                ],
                lazyRender: true,
                listClass: 'x-combo-list-small'
            }
        }, {
            header: 'sign',
            width: 60,
            sortable: true,
            dataIndex: 'sign',
            field: {
                xtype: 'combobox',
                typeAhead: true,
                triggerAction: 'all',
                selectOnTab: true,
                store: [
                    ['=', '='],
                    ['in', 'in'],
                    ['<>', '<>'],
                    ['like', 'like'],
                    ['>', '>'],
                    ['<', '<'],
                    ['>=', '>='],
                    ['<=', '<=']
                ],
                lazyRender: true,
                listClass: 'x-combo-list-small'
            }
        }, {
            header: 'condition',
            width: 100,
            sortable: true,
            dataIndex: 'condition',
            field: {
                xtype: 'textfield',
                allowBlank: false,
                name: 'condition'
            }
        }];
        var cellEditing = Ext.create('Ext.grid.plugin.CellEditing', {
            clicksToEdit: 1
        });
        Ext.ClassManager.setAlias('Ext.selection.CheckboxModel', 'selection.checkboxmodel');
        view_grid = Ext.create('Ext.grid.Panel', {
            columnLines: true,
            frame: true,
            iconCls: 'iconGrid',
            multiSelect: true,
            selModel: {
                selType: 'checkboxmodel' //复选框选择模式Ext.selection.CheckboxModel   
            },
            autoScroll: true,
            height: 385,
            store: viewStore,
            columns: viewColumn,
            disableSelection: false,
            stripeRows: true,
            selType: 'cellmodel',
            loadMask: true,
            anchor: '-0',
            margins: '0 2 0 0',
            title: 'View Table',
            plugins: [cellEditing],
            dockedItems: [{
                dock: 'top',
                xtype: 'toolbar',
                items: [{
                    text: 'Delete From View',
                    iconCls: 'iconDelete',
                    handler: function () {
                        var record = view_grid.getSelectionModel().getSelection();
                        if (record.length == 0) {
                            Ext.Msg.alert("msg", "Please Select a Record");
                        } else {
                            for (var i = 0; i < record.length; i++) {
                                var name = record[i].get('name');
                                var type = record[i].get('fieldtype');
                                var r = Ext.ModelManager.create({
                                    fieldname: name,
                                    fieldtype: type
                                }, 'fieldModel');
                                fieldStore.insert(0, r);
                            }
                            view_grid.getStore().remove(record);
                        }
                    }
                }]
            }]
        });
        var detailsPanel = new Ext.Panel({
            bodyStyle: 'padding-bottom:15px;background:#eee;',
            border: false,
            layout: 'anchor',
            items: [simplefield, view_grid]
        });
        var fieldColumn = [{
            id: 'fieldname',
            width: 80,
            header: "Field Name",
            sortable: true,
            dataIndex: 'fieldname'
        }, {
            header: "Field Type",
            width: 80,
            sortable: true,
            dataIndex: 'fieldtype'
        }];
        var fieldModel = Ext.create('Ext.selection.CheckboxModel', {
            checkOnly: true
        });
        field_grid = Ext.create('Ext.grid.Panel', {
            viewConfig: {
                stripeRows: false,
                forceFit: false
            },
            listeners: {
                itemdblclick: {
                    fn: function (view, record, item, index) {
                        var name = record.store.getAt(index).get('fieldname');
                        var type = record.store.getAt(index).get('fieldtype');
                        var r = Ext.ModelManager.create({
                            name: name,
                            display_name: '',
                            output: true,
                            group_by: '',
                            order_by: '',
                            sign: '',
                            condition: '',
                            fieldtype: type
                        }, 'viewModel');
                        var flag = false;
                        for (var i = 0; i < viewStore.getCount(); i++) {
                            if (viewStore.getAt(i).get('name') == name) {
                                flag = true;
                            }
                        }
                        if (!flag) {
                            viewStore.insert(0, r);
                            record.store.removeAt(index);
                        }
                        view_grid.getView().refresh();
                        Ext.getCmp('pagingtoolbar').store = viewStore;
                        cellEditing.startEditByPosition({
                            row: 0,
                            column: 0
                        });
                        view_grid.getView().refresh();
                        Ext.getCmp('pagingtoolbar').store = viewStore;
                    }
                }
            },
            frame: true,
            title: 'Field Table',
            selModel: fieldModel,
            trackMouseOver: false,
            height: 495,
            iconCls: 'iconGrid',
            store: fieldStore,
            autoScroll: true,
            columns: fieldColumn,
            anchor: '-0',
            margins: '0 2 0 0',
            dockedItems: [{
                dock: 'top',
                xtype: 'toolbar',
                items: [{
                    text: 'Add To View',
                    iconCls: 'iconInsurance',
                    handler: function () {
                        var record = field_grid.getSelectionModel().getSelection();
                        if (record.length == 0) {
                            Ext.MessageBox.show({
                                title: 'Msg',
                                buttons: Ext.Msg.OK,
                                msg: 'Pelese Select the Record First!',
                                icon: Ext.MessageBox.INFO
                            });
                        } else {
                            var list = new Array();
                            for (var i = 0; i < record.length; i++) {
                                var name = record[i].get('fieldname');
                                var fieldtype = record[i].get('fieldtype');
                                var r = Ext.ModelManager.create({
                                    name: name,
                                    display_name: '',
                                    output: true,
                                    group_by: '',
                                    order_by: '',
                                    sign: '',
                                    condition: '',
                                    fieldtype: fieldtype
                                }, 'viewModel');
                                list.push(r);
                            }
                            fieldStore.remove(record);
                            field_grid.getView().refresh();
                            var field_data = '[';
                            for (var j = 0; j < list.length; j++) {
                                if (j != 0) {
                                    field_data = field_data + "," + Ext.encode(list[j].data);
                                } else {
                                    field_data = field_data + Ext.encode(list[j].data);
                                }
                            }
                            field_data = field_data + "]";
                            Ext.Ajax.request({
                                url: me.DataJson.view_url,
                                params: {
                                    viewjson: field_data
                                },
                                success: function (o) {
                                    var data_jsons = Ext.decode(o.responseText);
                                    view_grid.getStore().loadData(data_jsons.field, true);
                                }
                            });
                        }
                    }
                }]
            }]
        });
        Ext.define('comboStore', {
            extend: 'Ext.data.Model',
            fields: [{
                type: 'string',
                name: 'name'
            }, {
                type: 'string',
                name: 'display_name'
            }]
        });
        comStore = Ext.create('Ext.data.Store', {
            model: 'comboStore',
            proxy: {
                type: 'ajax',
                url: this.DataJson.db_view_link,
                reader: {
                    type: 'json',
                    root: 'view'
                },
                fields: ['name', 'display_name']
            }
            //,autoLoad: true
        });
        filterStore = Ext.create('Ext.data.Store', {
            model: 'comboStore',
            proxy: {
                type: 'ajax',
                url: me.DataJson.report_filter_url,
                reader: {
                    type: 'json',
                    root: 'view'
                },
                fields: ['name', 'display_name']
            }
        });
        simpleCombo = Ext.create('Ext.form.ComboBox', {
            xtype: 'combo',
            fieldLabel: 'Select a table',
            displayField: 'name',
            name: 'select_table_combo',
            width: 300,
            store: comStore,
            typeAhead: true,
            mode: 'local',
            triggerAction: 'all',
            emptyText: 'Please Select..',
            selectOnFocus: true,
            listeners: {
                "select": function (combo, record, index) {
                    simplefield.setValue('');
                    viewStore.removeAll();
                    if (this.report_grid != null) {
                        detailsPanel.remove(this.report_grid);
                    }
                    fieldStore.proxy.url = me.DataJson.all_field_url + '?view=' + combo.value;
                    fieldStore.load();
                    reportFilter.clearValue();
                    filterStore.proxy.url = me.DataJson.report_filter_url + '?viewName=' + combo.value;
                    filterStore.load();
                }
            }
        });
        reportFilter = Ext.create('Ext.form.ComboBox', {
            xtype: 'combo',
            fieldLabel: 'Report Filter',
            displayField: 'name',
            valueField: 'display_name',
            name: 'report_filter',
            width: 300,
            store: filterStore,
            typeAhead: true,
            mode: 'local',
            triggerAction: 'all',
            emptyText: 'Please Select..',
            selectOnFocus: true,
            listConfig: {
                minWidth: 70,
                maxHeight: 300,
                shadow: 'sides',
                loadMask: false
            }
        });
        var toolbar = Ext.create('Ext.toolbar.Toolbar', {
            region: 'north',
            items: [simpleCombo, reportFilter,
            {
                text: 'Preview',
                icon: 'icons/zoom.png',
                handler: function () {
                    if (this.report_grid != null) {
                        this.report_grid.getStore().removeAll();
                    }
                    if (simpleCombo.getValue() == "" || simpleCombo.getValue() == null) {
                        Ext.Msg.alert("Msg", "Please select the data table!");
                    } else if (view_grid.getStore().getCount() == 0) {
                        simplefield.setValue("");
                        Ext.Msg.alert("Msg", "Please select the field");
                    } else {
                        me.generateReport(view_grid.getStore(), simpleCombo, simplefield, me.DataJson,me,null);
                        Ext.getCmp('content').setActiveTab(Ext.getCmp('scbb'));
                    }
                }
            }, {
                text: 'Save',
                id: 'addreport',
                icon: 'icons/table_save.png',
                handler: function () {
                    editForm = Ext.create('Ext.form.Panel', {
                        bodyPadding: 10,
                        width: 350,
                        border: false,
                        frame: true,
                        xtype: 'filedset',
                        url: 'ecAssetStatus/update',
                        layout: 'anchor',
                        defaults: {
                            anchor: '100%'
                        },
                        defaultType: 'textfield',
                        items: [{
                            fieldLabel: 'Report Name',
                            name: 'name',
                            id: 'report_name',
                            emptyText: 'Report Name',
                            value: me.reportName,
                            allowBlank: false
                        }],
                        buttons: [{
                            text: 'Save',
                            buttonAlign: 'center',
                            formBind: true,
                            disabled: true,
                            icon: 'icons/table_save.png',
                            handler: function () {
                                var form = this.up('form').getForm();
                                var field_data = '[';
                                for (var jj = 0; jj < fieldStore.getCount(); jj++) {
                                    if (jj != 0) {
                                        field_data = field_data + "," + Ext.encode(fieldStore.data.items[jj].data);
                                    } else {
                                        field_data = field_data + Ext.encode(fieldStore.data.items[jj].data);
                                    }
                                }
                                field_data = field_data + "]";
                                var view_data = '[';
                                for (var m = 0; m < viewStore.getCount(); m++) {
                                    if (m != 0) {
                                        view_data = view_data + "," + Ext.encode(viewStore.data.items[m].data);
                                    } else {
                                        view_data = view_data + Ext.encode(viewStore.data.items[m].data);
                                    }
                                }
                                view_data = view_data + "]";
                                var val = simplefield.getValue();
                                if (val != '') {
                                    var name = form.findField('name').getValue();
                                    var url = '';
                                    if (me.reportId != null) {
                                        url = me.DataJson.update_url + '?reportId=' + me.reportId + '';
                                    } else {
                                        url = me.DataJson.add_url;
                                    }
                                    Ext.Ajax.request({
                                        url: url,
                                        params: {
                                            reportname: name,
                                            reportjson: val,
                                            fieldjson: field_data,
                                            viewjson: view_data,
                                            filter: reportFilter.getValue()
                                        },
                                        success: function (o) {
                                            rewin.close();
                                            //new com.embraiz.tag().tabRefrash('com.embraiz.repview.js.index', '17', '');
                                        }
                                    });
                                    win.close();
                                } else {
                                    Ext.Msg.alert("Msg", "Please select the data table!");
                                }
                            }
                        }]
                    });
                    var win = Ext.create('Ext.window.Window', {
                        title: 'Create Report',
                        hight: 500,
                        width: 400,
                        modal: true,
                        hidden: true,
                        layout: 'fit',
                        closable: true,
                        items: editForm
                    });
                    win.show();
                }
            }]
        });
        var rewin = Ext.create('Ext.window.Window', {
            title: 'Report Builder',
            id: 'rewin',
            closable: true,
            width: 900,
            height: 600,
            layout: 'border',
            maximizable: true,
            modal: true,
            bodyStyle: 'padding: 5px;',
            items: [{
                region: 'north',
                items: toolbar
            }, {
                region: 'center',
                xtype: 'tabpanel',
                id:'content',
                items: [{
                    title: 'Design',
                    xtype: 'panel',
                    layout: 'column',
                    iconCls:"iconDesign",
                    border: false,
                    items: [{
                        padding: '1,1,3',
                        id: 'dbsy',
                        xtype: 'panel',
                        title: 'JSON Details',
                        width: '74%',
                        border: false,
                        height: 510,
                        items: [detailsPanel]
                    }, {
                        padding: '1,1,3',
                        width: '26%',
                        xtype: 'panel',
                        border: false,
                        id: 'wdyx',
                        height: 600 * (0.85),
                        items: field_grid
                    }]
                }, {
                    title: 'Preview',
                    xtype: 'panel',
                    iconCls:'iconPreview',
                    border: false,
                    id: 'scbb'
                }]
            }],
            listeners: {
                'resize': function (window, e) {
                    if (window.getHeight() == 600) {
                        field_grid.setHeight(495);
                        view_grid.setHeight(385);                    
                        if(Ext.getCmp('scbb').items.length>0)
                           Ext.getCmp('scbb').items[0].setHeight(500);
                        Ext.getCmp('wdyx').setHeight(600 * 0.85);
                        Ext.getCmp('dbsy').setHeight(510);
                    } else {
                        field_grid.setHeight(window.getHeight() * 0.845);
                        view_grid.setHeight(window.getHeight() * 0.68);
                        if(Ext.getCmp('scbb').items.length>0)
                           Ext.getCmp('scbb').items[0].setHeight(window.getHeight()*0.85);
                        Ext.getCmp('wdyx').setHeight(window.getHeight() * 0.85);
                        Ext.getCmp('dbsy').setHeight(window.getHeight() * 0.85);
                    }
                }
            }
        });
        rewin.show();
        rewin.on('close', function () {
            rewin = null;
        });
    },
    previewReport: function (json_data, dataJson,me,grider_div) {
        var field_arr = new Array();
        var header_arr = new Array();
        var parame_arr = new Array();
        var grid_header = new Array();
        for (var i = 0; i < json_data.column.length; i++) {
            field_arr[i] = {};
            header_arr[i] = {};
            if (json_data.column[i].output == 1) {
                field_arr[i].name = json_data.column[i].name;
                field_arr[i].mapping = json_data.column[i].name;
                if (json_data.column[i].display_name != '' && json_data.column[i].display_name != null) {
                    header_arr[i].header = json_data.column[i].display_name.replace("*", ".");
                } else {
                    header_arr[i].header = json_data.column[i].name.replace("*", ".");
                }
                header_arr[i].sortable = true;
                header_arr[i].dataIndex = json_data.column[i].name;
            }
        }
        for (var i = 0; i < field_arr.length; i++) {
            if (Ext.encode(field_arr[i]) != "{}") {
                parame_arr.push(field_arr[i]);
            }
        }
        for (var i = 0; i < header_arr.length; i++) {
            if (Ext.encode(header_arr[i]) != "{}") {
                grid_header.push(header_arr[i]);
            }
        }
        var column = grid_header;
        header_data = grid_header;
        Ext.define('content_type_model', {
            extend: 'Ext.data.Model',
            proxy: {
                type: 'ajax',
                url: dataJson.report_url,
                getMethod: function (request) {
                    return 'POST';
                },
                reader: {
                    type: 'json',
                    root: 'report',
                    totalProperty: 'totalCount'
                }
            },
            fields: parame_arr
        });
        reportStore = Ext.create('Ext.data.Store', {
            model: 'content_type_model',
            pageSize: 20
        });
        reportStore.on('beforeload', function (store, options) {
            var new_params = {
                json_data: Ext.encode(json_data),
                field_column: Ext.encode(parame_arr)
            };
            Ext.apply(store.proxy.extraParams, new_params);
        });
        reportStore.load({
            params: {
                start: 0,
                limit: 20
            }
        });
        pagebar= Ext.create('Ext.PagingToolbar', {
                store: reportStore, 
                pageSize: 20,
                dock: 'bottom',
                emptyMsg: 'No topics to display',
                displayInfo: true,
                displayMsg: 'Displaying topics {0} - {1} of {2}',
                beforePageText: 'First',
                afterPageText: 'Page / Page {0}'
        });
        if (grider_div==null?Ext.getCmp('scbb').items.length ==0:true) { 
            report_grid = Ext.create('Ext.grid.Panel', {
                viewConfig: {
                    loadMask: true
                },
                frame: true,
                iconCls: 'iconGrid',
                store: reportStore,
                columns: column,
                height:500,
                width:'100%',
                autoScroll:true,
                anchor: '-0',
                margins: '0 2 0 0',
                title: 'Report',
                dockedItems: [pagebar, {
                    dock: 'top',
                    xtype: 'toolbar',
                    items: [{
                        text: 'Export All',
                        tooltip: 'Export Excel',
                        iconCls: 'iconExcel',
                        handler: function () {
                            var tempForm = document.createElement("form");
					        tempForm.id = "tempForm1";
					        tempForm.method = "post";
					        tempForm.action = dataJson.export_link;
					        tempForm.target = '';
					        var hideInput1 = document.createElement("input");
					        hideInput1.type = "hidden";
					        hideInput1.name = "header";
					        hideInput1.value = Ext.encode(header_data);
					        tempForm.appendChild(hideInput1);
					        var hideInput2 = document.createElement("input");
					        hideInput2.type = "hidden";
					        hideInput2.name = "data";
					        hideInput2.value = Ext.encode(json_data);
					        tempForm.appendChild(hideInput2);
					        document.body.appendChild(tempForm);
					        tempForm.submit();
					        window.open('', 'height=400, width=400, top=0, left=0, toolbar=yes, menubar=yes, scrollbars=yes, resizable=yes,location=yes, status=yes');
                        }
                    }]
                }]
            });
            if(grider_div!=null)
               report_grid.render(grider_div);
            else
               Ext.getCmp('scbb').add(report_grid);
        } else {
            pagebar.bind(reportStore);
            report_grid.reconfigure(reportStore, grid_header);
            //report_grid.render();
            report_grid.getView().refresh();
            reportStore.load();
            header_data = column;
        }
    },
    generateReport: function (viewStore, simpleCombo, simplefield, datajson,me,grider_div) {
        var json_data = '';
        json_data = Ext.decode('{table:[]}');
        var groupby_arr = new Array();
        for (var j = 0; j < viewStore.getCount(); j++) {
            if (viewStore.getAt(j).get('name') != '') {
                json_data["column"] = new Array();
            }
            if (viewStore.getAt(j).get('sign') != null && viewStore.getAt(j).get('condition') != '') {
                json_data["filters"] = new Array();
            }
            if (viewStore.getAt(j).get('group_by') != '') {
                groupby_arr.push(viewStore.getAt(j).get('group_by'));
            }
            if (viewStore.getAt(j).get('order_by') != '') {
                json_data["order_by"] = new Array();
            }
        }
        if (containsKey('Group By', groupby_arr)) {
            json_data["group_by"] = new Array();
        }
        json_data.table[0] = {};
        json_data.table[0].display_name = simpleCombo.getValue();
        json_data.table[0].name = simpleCombo.getValue();
        for (var i = 0; i < viewStore.getCount(); i++) {
            if (json_data["column"]) {
                json_data.column[i] = {};
                if (viewStore.getAt(i).get('display_name') != '') {
                    json_data.column[i].display_name = viewStore.getAt(i).get('display_name');
                }
                if (viewStore.getAt(i).get('name') != '') {
                    json_data.column[i].name = viewStore.getAt(i).get('name');
                    if (viewStore.getAt(i).get('name').indexOf('.') != -1) {
                        json_data.column[i].name = viewStore.getAt(i).get('name').replace(".", "*");
                    }
                }
                if (viewStore.getAt(i).get('output') != null) {
                    if (viewStore.getAt(i).get('output') == true) {
                        json_data.column[i].output = 1;
                    } else {
                        json_data.column[i].output = 0;
                    };
                }
            }
            if (json_data["filters"]) {
                if (viewStore.getAt(i).get('sign') != null && viewStore.getAt(i).get('condition') != '') {
                    json_data.filters[i] = {};
                    json_data.filters[i].name = viewStore.getAt(i).get('name');
                    json_data.filters[i].sign = viewStore.getAt(i).get('sign');
                    json_data.filters[i].condition = viewStore.getAt(i).get('condition');
                    display_string(json_data, json_data.filters);
                }
                display_string(json_data, json_data.filters);
            }
            if (json_data["group_by"]) {
                if (viewStore.getAt(i).get('group_by') != '') {
                    json_data.group_by[i] = {};
                    if (viewStore.getAt(i).get('group_by') == 'Group By') {
                        json_data.group_by[i].name = viewStore.getAt(i).get('name');
                    } else {
                        json_data.column[i].name = viewStore.getAt(i).get('group_by') + '(' + viewStore.getAt(i).get('name') + ')';
                    }
                    display_string(json_data, json_data.group_by);
                    display_array(json_data, viewStore);
                }
            } else {
                if (viewStore.getAt(i).get('group_by') != '') {
                    json_data.column[i].name = viewStore.getAt(i).get('group_by') + '(' + viewStore.getAt(i).get('name') + ')';
                    display_array(json_data, viewStore);
                }
            }
            if (json_data["order_by"]) {
                if (viewStore.getAt(i).get('order_by') != '') {
                    json_data.order_by[i] = {};
                    if (viewStore.getAt(i).get('order_by') == 'ASC') {
                        json_data.order_by[i].name = viewStore.getAt(i).get('name');
                    } else {
                        json_data.order_by[i].name = viewStore.getAt(i).get('name') + ' ' + viewStore.getAt(i).get('order_by');
                    }
                    display_string(json_data, json_data.order_by);
                }
            }
        }
        if (json_data["group_by"]) {
            display_string(json_data, json_data.group_by);
        }
        if (json_data["order_by"]) {
            display_string(json_data, json_data.order_by);
        }
        simplefield.setValue(Ext.encode(json_data).replace("*", "."));
        this.previewReport(json_data, datajson,me);
        function display_string(json_data, json_array) {
            for (var m = 0; m < json_array.length; m++) {
                if (json_array[m] == undefined || json_array[m].name == null || json_array[m] == null || json_array[m] == "null") {
                    json_array.splice(m, 1);
                    m = 0;
                }
            }
        }
        function containsKey(name, array) {
            var type = typeof name;
            if (type == 'string' || type == 'number') {
                for (var i in array) {
                    if (array[i] == name) {
                        return true;
                    }
                }
            }
            return false;
        }
        function display_array(json_data, viewStore) {
            var arr = new Array();
            for (var item = 0; item < viewStore.getCount(); item++) {
                arr.push(viewStore.getAt(item).get('group_by'));
            }
            if (!containsKey('Group By', arr)) {
                delete json_data.group_by;
            }
        }
    }
});