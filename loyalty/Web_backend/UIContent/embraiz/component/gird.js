Ext.require(['Ext.grid.*', 'Ext.data.*', 'Ext.tip.*']);
Ext.define('Ext.grid.column.Columnlink', {
    extend: 'Ext.grid.column.Action',
    alias: 'widget.columnlink',
    constructor: function (config) {
        var me = this, cfg = Ext.apply({}, config), items = cfg.items
				|| [me], l = items.length, i, item;
        delete cfg.items;
        me.callParent([cfg]);
        me.items = items;
        me.renderer = function (v, meta, datastore) {
            v = Ext.isFunction(cfg.renderer) ? cfg.renderer.apply(this,
					arguments) || '' : '';
            meta.tdCls += ' ' + Ext.baseCSSPrefix + 'action-col-cell';
            for (i = 0; i < l; i++) {
                item = items[i];
                item.disable = Ext.Function.bind(me.disableAction, me, [i]);
                item.enable = Ext.Function.bind(me.enableAction, me, [i]);
                v += '<span class="gird_link '
						+ Ext.baseCSSPrefix
						+ 'action-col-icon '
						+ Ext.baseCSSPrefix
						+ 'action-col-'
						+ String(i)
						+ ' '
						+ (item.disabled ? Ext.baseCSSPrefix + 'item-disabled'
								: ' ')
						+ (item.iconCls || '')
						+ ' '
						+ (Ext.isFunction(item.getClass) ? item.getClass.apply(
								item.scope || me.scope || me, arguments)
								: (me.iconCls || '')) + '">'
						+ datastore.get(item.dataIndex);
                +'</span>';
            }
            return v;
        };
    }
});
function formatString(str) {
    var data = '';
    for (var i = 0; i < str.length; i++) {
        var c = str.charAt(i);
        switch (c) {
            case '\'':
                data += "&#39;";
                break;
            case '\"':
                data += "&quot;";
                break;
            case '\\':
                data += "&#92;&#92;";
                break;
            default:
                data += c;
        }
    }
    str = data;
    return str;
}
function outline(op) {
    var str = op.children[0].value;
    if (str.indexOf('\'') >= 0) {
        var left = str.substring(0, str.indexOf("(\'") + 1);
        var mid = str.substring(str.indexOf("(\'") + 2, str.indexOf("')"));
        var right = str.substring(str.indexOf("')") + 1);
        var spmid = mid.split("','");
        for (var i = 0; i < spmid.length; i++) {
            spmid[i] = "'" + formatString(spmid[i]) + "',";
            left += spmid[i];
        }
        left = left.substring(0, left.length - 1);
        str = left + right;
    }
    eval(str);
}
Ext.define('com.embraiz.component.gird', {
    grid_toolbar: undefined,
    gird: undefined,
    store: undefined,
    grider_div: undefined,
    json_data: undefined,
    grid_url: undefined,
    header_str: undefined,
    delete_flag: true,
    form_id: undefined,
    params: undefined,
    bbar: undefined,
    dataIndex: undefined,
    add_hidden: false,
    checkbox_hidden: false,
    delete_url: undefined,
    selModel: undefined,
    delete_hidden: false,
    form_div: undefined,
    displayMsg: undefined,
    constructor: function (config) {
        Ext.tip.QuickTipManager.init();
        config = config || {};
        Ext.apply(this, config);
        this.callParent([config]);
        var me = this;
        var form_div = this.form_div;
        
        var store = me.store = Ext.create('Ext.data.Store', {
            fields: me.json_data.fields,
            pageSize: me.json_data.pageSize,
            filterOnLoad: false,
            remoteSort: true,
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: me.grid_url,
      
                reader: {
                    type: 'json',
                    root: 'items',
                    totalProperty: 'totalCount'
                }
            }
        });

        store.on('beforeload', function (thiz, op) {
            if (op.sorters[0] != undefined) {
                var name = op.sorters[0].property;
                for (var i = 0; i < name.length; i++) {
                    if ((name.charCodeAt(i) >= 65)
                            && (name.charCodeAt(i) <= 90)) {
                        name = name.substring(0, i)
                                + '_'
                                + name.substring(i, i + 1)
                                        .toLowerCase()
                                + name.substring(i + 1,
                                        name.length);
                    }
                }
                op.sorters[0].property = name;
            }
        });

        //$.ajax({
        //    url: "http://localhost:50450/table/ListData/memberprofile?_dc=1435226382187&page=1&start=0&limit=20", //me.grid_url,
        //    dataType: "json",
        //    type: "GET",
        //    async: false,
        //}).done(function (o) {
        //    console.log(o);
        //    store.loadData(o.items);
        //});

        // check box model
        var tempUrl = me.json_data.delete_url;
        var selModel = null;
        if (me.json_data.delete_hidden != undefined
                && me.json_data.delete_hidden != '') {
            me.delete_hidden = me.json_data.delete_hidden;
        }
        if (me.json_data.displayMsg != undefined) {
            me.displayMsg = me.json_data.displayMsg;
        }
        if (me.json_data.checkbox_hidden != true) {
            me.selModel = selModel = Ext
                    .create('Ext.selection.CheckboxModel', {
                        checkOnly: true,
                        listeners: {
                            selectionChange: function (
                                    sm, selections) {
                                grid
                                        .down(
                                                '#removeButton')
                                        .setDisabled(
                                                selections.length == 0
                                                        || tempUrl == null
                                                        || tempUrl == undefined);
                            },
                            select: function (sm,
                                    selections) {
                                grid
                                        .down(
                                                '#removeButton')
                                        .setDisabled(
                                                selections.length == 0
                                                        || tempUrl == null
                                                        || tempUrl == undefined);
                            }
                        }
                    });
        }
        var displayMsg = me.displayMsg == 1 ? 'Total {2}&nbsp;&nbsp;' : 'Displaying data {0} - {1} of {2}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;';

        // paging bar on the bottom
        me.bbar = Ext.create('com.embraiz.component.extPaging', {
            store: me.store,
            displayInfo: true,
            //displayMsg : 'Displaying data {0} - {1} of {2}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;',
            displayMsg: displayMsg,
            emptyMsg: "No data to display",
            url: me.grid_url,
            params: me.params,
            items: ['-'],
            inputItemWidth: 60
        });
        // generate grid's columns
        var form_columns = [];
        for (i = 0; i < me.json_data.columns.length; i++) {
            var temp_element = {};
            temp_element.header = me.json_data.columns[i].header;
            temp_element.dataIndex = me.json_data.columns[i].dataIndex;
            var gridWidth = me.json_data.columns[i].width;
            if (gridWidth != undefined) {
                if (!isNaN(gridWidth + '')) {
                    gridWidth = parseInt(gridWidth + '');
                    temp_element.width = gridWidth;
                }
            }
            temp_element.type = me.json_data.columns[i].type;
            temp_element.flex = me.json_data.columns[i].flex;
            temp_element.renderer = me.json_data.columns[i].renderer;
            temp_element.hidden = me.json_data.columns[i].hidden
                    || me.json_data.columns[i].hidden;
            temp_element.columnorder = i;
            if (me.json_data.columns[i].sortable != undefined) {
                temp_element.sortable = me.json_data.columns[i].sortable;
            } else {
                temp_element.sortable = false;
            }
            if (me.json_data.columns[i].type == "img") {
                var imgWidth = me.json_data.columns[i].imgWidth != undefined ? me.json_data.columns[i].imgWidth : '40';
                var imgHeight = me.json_data.columns[i].imgHeight != undefined ? me.json_data.columns[i].imgHeight : '40';
                temp_element.renderer = function renderTopic1(value, p, record) {

                    var largeValue = value.replace("_thumb", "_large"); // show large iamge if have it
                    return Ext.String.format('<a href ="{0}" target="_blank"><img src="{1}" style="height:{2}px;width:{3}px;"/></a>', largeValue, largeValue, imgHeight, imgWidth);
                };

            }
            if (me.json_data.columns[i].type == "title") {
                temp_element.renderer = function renderTopic1(
                        value, p, record) {
                    return Ext.String
                            .format(
                                    '<span onclick="outline(this)" class="gird_link" >{0}<input type="hidden" value="{1}"/></span>',
                                    value,
                                    formatString(record.data.href),
                                    record.data.tooltip,
                                    record.data.target);
                };
            } else if (me.json_data.columns[i].type == "title1") {
                temp_element.renderer = function renderTopic1(
                        value, p, record) {
                    return Ext.String
                            .format(
                                    '<span onclick="outline(this)" class="gird_link" >{0}<input type="hidden" value="{1}"/></span>',
                                    value,
                                    formatString(record.data.href1),
                                    record.data.tooltip1,
                                    record.data.target1);
                };
            } else if (me.json_data.columns[i].type == "title2") {
                temp_element.renderer = function renderTopic2(
                        value, p, record) {
                    return Ext.String
                            .format(
                                    '<span onclick="outline(this)" class="gird_link" >{0}<input type="hidden" value="{1}"/></span>',
                                    value,
                                    formatString(record.data.href2),
                                    record.data.tooltip2,
                                    record.data.target2);
                };
            } else if (me.json_data.columns[i].type == 'currency') {
                temp_element.renderer = function currency(
                        value, p, record) {
                    return value = Ext.util.Format.currency(
                            value, '$', 2);
                }
            }
            form_columns[i] = temp_element;
        }
        // set the add button
        if (me.json_data.add_hidden != null
                && me.json_data.add_hidden != undefined) {
            me.add_hidden = me.json_data.add_hidden;
        }
        if (me.json_data.checkbox_hidden != null
                && me.json_data.checkbox_hidden != undefined) {
            me.checkbox_hidden = me.json_data.checkbox_hidden;
        }
        if (me.json_data.search_text_hidden != null
                && me.json_data.search_text_hidden != undefined) {
            me.search_text_hidden = me.json_data.search_text_hidden;
        }
        if (me.json_data.search_button_hidden != null
                && me.json_data.search_button_hidden != undefined) {
            me.search_button_hidden = me.json_data.search_button_hidden;
        }
        this.grid_toolbar = grid_toolbar = Ext.create(
                'Ext.toolbar.Toolbar', {
                    xtype: 'toolbar'
                });
        if (!me.checkbox_hidden) {
            var name = '';
            var module = me.json_data.module;
            if (module == null || module == ''
                    || module == undefined) {
                name = 'remove';
            } else {
                name = module + ':remove';
            }
            grid_toolbar.add({
                itemId: 'removeButton',
                text: 'Remove',
                name: name,
                tooltip: 'Remove the selected item',
                iconCls: 'iconDelete',
                // hidden: me.checkbox_hidden,
                hidden: me.delete_hidden,
                disabled: true,
                handler: function () {
                    var tempUrl = me.json_data.delete_url;
                    Ext.MessageBox
                            .confirm(
                                    'Confirm',
                                    'Are you sure you want to delete?',
                                    function (btn) {
                                        if (btn == 'yes') {
                                            var selectedRecord = selModel
                                                    .getSelection();
                                            var selectedValue = "0";
                                            for (var i = 0; i < selectedRecord.length; i++) {
                                                selectedValue = selectedValue
                                                        + ","
                                                        + selectedRecord[i]
                                                                .get('id');
                                            }
                                            // call delete function here
                                            Ext.Ajax.request({
                                                url: tempUrl,
                                                async: false,
                                                params: {
                                                    id: selectedValue
                                                },
                                                callback: function (
                                                        o,
                                                        s,
                                                        r) {
                                                    eval('var text=' + r.responseText);
                                                    if (text.flag
                                                            || text.flag == undefined) {
                                                        Ext.Msg.show({
                                                            title: 'Success',
                                                            msg: text.msg || '',
                                                            width: 200,
                                                            buttons: Ext.Msg.OK,
                                                            icon: Ext.MessageBox.INFO
                                                        });
                                                        store.loadPage(store.currentPage);

                                                    } else {
                                                        Ext.Msg.show({
                                                            title: 'Warning',
                                                            msg: text.msg || '',
                                                            width: 200,
                                                            buttons: Ext.Msg.OK,
                                                            icon: Ext.MessageBox.INFO
                                                        });
                                                    }
                                                }
                                            });
                                        } else {
                                           // no need, beacause IE will close browser tag
                                           // this.close();
                                        }
                                    });
                }
            });
        }
        if (!me.add_hidden) {
            var name = '';
            var text = '';
            var module = me.json_data.module;
            var add_text = me.json_data.add_text;
            if (module == null || module == ''
                    || module == undefined) {
                name = 'add';
            } else {
                name = module + ':add';
            }
            if (add_text == null || add_text == ''
                    || add_text == undefined) {
                text = 'Add';
            } else {
                text = add_text;
            }
            grid_toolbar.add({
                itemId: 'addButton',
                text: text,
                name: name,
                tooltip: 'Add item',
                iconCls: 'iconAdd',
                handler: function () {

                    //---set synchronous loading on this one to avoid problems with rendering
                    //Ext.apply(Ext.data.Connection.prototype, {
                    //    async: false
                    //});

                    if (typeof me.json_data.add_url != "function") {
                        if (me.json_data.add_url
                                .substr(
                                        me.json_data.add_url.length - 1,
                                        me.json_data.add_url.length) != ")") {
                            me.json_data.add_url = me.json_data.add_url
                                    + "()";
                        }
                        eval(me.json_data.add_url);
                    } else {
                        me.json_data.add_url.call();
                    }

                    //---restore async property to the default value
                    //Ext.apply(Ext.data.Connection.prototype, {
                    //    async: true
                    //});
                }
            });
        }
        if (me.json_data.myConfig == true) {
            var name = '';
            var text = '';
            var module = me.json_data.module;
            grid_toolbar.add({
                text: 'Config',
                name: 'gridConfig',
                iconCls: 'iconConfig',
                handler: function () {
                    new com.embraiz.common.js.gridConfig()
                            .initTag(me.json_data.module,
                                    'grid');
                }
            });
        }
        if (me.json_data.export_hidden != undefined
                && !me.json_data.export_hidden) {
            var name = '';
            var module = me.json_data.module;
            if (module == null || module == ''
                    || module == undefined) {
                name = 'exprot';
            } else {
                name = module + ':exprot';
            }
            grid_toolbar.add({
                itemId: 'Exprot',
                text: 'Export Record',
                name: name,
                tooltip: 'Export',
                iconCls: 'iconRcAction',
                handler: function () {
                    if (me.json_data.export_url != undefined
                            && me.json_data.export_url != '') {
                        if (typeof me.json_data.export_url != "function") {
                            if (me.json_data.export_url
                                    .substr(
                                            me.json_data.export_url.length - 1,
                                            me.json_data.export_url.length) != ")") {
                                me.json_data.export_url = me.json_data.export_url
                                        + "()";
                            }
                            eval(me.json_data.export_url);
                        } else {
                            me.json_data.export_url
                                    .call();
                        }
                    } else {
                        new com.embraiz.exporter.js.index()
                                .initTag(
                                        grid,
                                        grid_toolbar,
                                        store,
                                        form_div,
                                        me.json_data.module);
                    }
                    scope: this
                }
            });
        }
        if (!me.search_text_hidden) {
            grid_toolbar.add({
                xtype: 'textfield',
                name: 'searchField',
                id: 'gridSearchText',
                hidden: me.search_text_hidden,
                hideLabel: true,
                width: 200,
                enableKeyEvents: true,
                listeners: {
                    'keydown': function keyDownSearch(target,
                            e, options) {
                        var raw = e.getKey();
                        if (raw == 13) {
                            me.searchinGrid(target);
                        }
                    }
                }
            });
            grid_toolbar.add({
                xtype: 'button',
                text: 'Search',
                iconCls: 'iconSearch',
                handler: function search() {
                    me.searchinGrid(this);
                },
                hidden: me.search_text_hidden
            });
        }
        if (me.json_data.config_link != null
                && me.json_data.config_refresh_url != null
                && me.json_data.config_gird_header_url != null) {
            var thisJsonData = this.json_data;
            grid_toolbar.add({
                itemId: 'configButton',
                text: 'Config',
                name: 'config',
                tooltip: 'Config',
                iconCls: 'iconConfig',
                json_data: me.json_data,
                handler: function () {
                    Ext.Ajax.request({
                        url: me.json_data.config_gird_header_url,
                        async: false,
                        success: function (o) {
                            this.json_data = Ext
                                    .decode(o.responseText);
                        },
                        scope: this
                    });
                    var itemcheckbox = [];
                    var json_data = this.json_data;
                    for (i = 0; i < json_data.columns.length; i++) {
                        var cur_item = {};
                        cur_item.boxLabel = json_data.columns[i].header;
                        cur_item.name = json_data.columns[i].dataIndex;
                        cur_item.inputValue = json_data.columns[i].dataIndex;
                        for (j = 0; j < thisJsonData.columns.length; j++) {
                            if (json_data.columns[i].dataIndex == thisJsonData.columns[j].dataIndex) {
                                cur_item.checked = true;
                            }
                        }
                        itemcheckbox.push(cur_item);
                    }
                    var win = Ext.create('widget.window', {
                        title: 'Field config',
                        closable: true,
                        modal: true,
                        items: {
                            xtype: 'form',
                            width: 600,
                            height: 400,
                            bodyPadding: 10,
                            method: 'POST',
                            url: this.json_data.config_link,
                            items: [{
                                xtype: 'checkboxgroup',
                                columns: 3,
                                vertical: true,
                                items: itemcheckbox
                            }, {
                                xtype: 'hidden',
                                name: 'fields',
                                value: ''
                            }, {
                                xtype: 'hidden',
                                name: 'header',
                                value: me.json_data.config_gird_header_url
                            }, {
                                xtype: 'button',
                                text: 'save',
                                iconCls: 'iconSave',
                                minWidth: 60,
                                cls: 'formbuttoncls',
                                handler: function () {
                                    var form = this
                                            .up(
                                                    'form')
                                            .getForm();
                                    var filedCheck = '';
                                    for (var j = 0; j <= i; j++) {
                                        if (form
                                                .getFields().items[j].xtype == "checkboxfield") {
                                            if (form
                                                    .getFields().items[j].checked == true) {
                                                filedCheck = filedCheck
                                                        + form
                                                                .getFields().items[j].inputValue
                                                        + ",";
                                            }
                                        }
                                    }
                                    // });
                                    if (filedCheck != '') {
                                        filedCheck = filedCheck
                                                .substring(
                                                        0,
                                                        filedCheck.length - 1);
                                    }
                                    form
                                            .findField(
                                                    "fields")
                                            .setRawValue(
                                                    filedCheck);
                                    if (form
                                            .isValid()) {
                                        form
                                                .submit({
                                                    success: function (
                                                            form,
                                                            action) {
                                                     
                                                        win
                                                                .close();
                                                        new com.embraiz.tag()
                                                                .tabRefrash(
                                                                        me.json_data.config_refresh_url,
                                                                        0,
                                                                        0);
                                                    }
                                                });
                                    }
                                }
                            }, {
                                xtype: 'button',
                                text: 'reset',
                                minWidth: 60,
                                iconCls: 'iconReset',
                                cls: 'formbuttoncls',
                                handler: function () {
                                    this
                                            .up(
                                                    'form')
                                            .getForm()
                                            .reset();
                                }
                            }]
                        }
                    });
                    win.show();
                }
            });
        }
        if (me.json_data.export_link != null) {
            grid_toolbar.add({
                itemId: 'excel',
                text: 'Export',
                name: 'exportExcel',
                tooltip: 'Export excel',
                iconCls: 'iconExcel',
                handler: function () {
                    // ///////////////////
                    var exportWin = Ext.create('widget.window', {
                        title: 'Excel Report',
                        closable: true,
                        modal: true,
                        items: {
                            xtype: 'form',
                            width: 500,
                            height: 200,
                            bodyPadding: 10,
                            method: 'POST',
                            url: me.json_data.export_link,
                            items: [{
                                fieldLabel: 'Number of rows',
                                xtype: 'textfield',
                                name: 'count',
                                value: '500'
                            }, {
                                xtype: 'hidden',
                                name: 'post_header',
                                value: ''
                            }, {
                                xtype: 'hidden',
                                name: 'post_params',
                                value: ''
                            }, {
                                xtype: 'button',
                                text: 'save',
                                iconCls: 'iconSave',
                                minWidth: 60,
                                cls: 'formbuttoncls',
                                handler: function () {
                                    var form = this
                                            .up(
                                                    'form')
                                            .getForm();
                                    var count = form
                                            .findField("count");
                                    var header = me.header_str;
                                    exportWin
                                            .close();
                                    window
                                            .open(me.json_data.export_link
                                                    + "?post_header="
                                                    + header
                                                    + "&filter="
                                                    + Ext
                                                            .encode(me.params)
                                                    + "&limit=" + count.getValue());
                                }
                            }, {
                                xtype: 'button',
                                text: 'reset',
                                minWidth: 60,
                                iconCls: 'iconReset',
                                cls: 'formbuttoncls',
                                handler: function () {
                                    this
                                            .up(
                                                    'form')
                                            .getForm()
                                            .reset();
                                }
                            }]
                        }
                    });
                    exportWin.show();
                    // ///////////////////
                }
            });
        }
        if (me.json_data.button_items != null
                && me.json_data.button_items != '') {
            var json_data = me.json_data;
            for (var a = 0; a < me.json_data.button_items.length; a++) {
                grid_toolbar.add({
                    xtype: json_data.button_items[a].xtype,
                    text: json_data.button_items[a].text,
                    iconCls: json_data.button_items[a].iconCls,
                    name: json_data.button_items[a].name,
                    fun: json_data.button_items[a].handler,
                    handler: function (btn, e, op) {
                        var ids = me.getCheckIds();
                        // /
                        var selModel = null;
                        var Selected = "";
                        if (me.selModel != null
                                && me.selModel != undefined) {
                            selModel = me.selModel;
                            var selectedRecord = selModel
                                    .getSelection();
                            for (var i = 0; i < selectedRecord.length; i++) {
                                if (selectedRecord[i]
                                        .get('color') != undefined
                                        && selectedRecord[i]
                                                .get('color') != null)
                                    ;
                                if (selectedRecord[i]
                                        .get('color') == 'red') {
                                    Selected = "red";
                                } else if (selectedRecord[i]
                                        .get('color') == 'blue') {
                                    Selected = "blue";
                                } else {
                                    Selected = "";
                                }
                            }
                        }
                        // //
                        var fun = btn.fun + "('" + ids
                                + "','" + Selected
                                + "',btn)";
                        if (btn.fun.substring(
                                btn.fun.length - 1,
                                btn.fun.length) == ')') {
                            fun = btn.fun;
                        }
                        eval(fun)
                    }
                });
            }
        }
        var grid = me.gird = Ext.create('Ext.grid.Panel', {
            title: me.json_data.title,
            iconCls: 'iconGrid',
            selModel: selModel,
            store: me.store,
            columns: form_columns,
            trackMouseOver: false,
            autoHeight: true,
            tooltip: 'test',
            width: '100%',
            padding:7,
            dockedItems: [grid_toolbar],
            bbar: me.bbar,
            renderTo: me.grider_div,
            viewConfig: {
                getRowClass: function (record, rowIndex,
                        rowParams, store) {
                    var color = record.get("color");
                    if (color != null) {
                        if (color == 'red') {
                            return 'gird_bk_red';
                        } else if (color == 'blue') {
                            return 'gird_bk_blue';
                        }
                    }
                    return '';
                },
               enableTextSelection: true
            }
        });
        grid.on('afterlayout',function(thiz,layout,eOpts){
        	//修复4.2 grid render size不??致scrollbar?示不出???               	
        	if(thiz.view.el.getHeight()>thiz.view.lastBox.height){
        		thiz.body.setHeight(thiz.view.getHeight());
            	thiz.el.setHeight(thiz.lastBox.height+18);
            	thiz.dockedItems.last().el.setTop(thiz.dockedItems.last().lastBox.y+18);
        	}
		})
        new com.embraiz.keyboard.js().gridKeyboard(grid);
        this.store.loadPage(1);
    },
    getSelectionIds: function () {
        var selectedRecord = this.gird.selModel.getSelection();
        var selectedValue = "0";
        for (var i = 0; i < selectedRecord.length; i++) {
            selectedValue = selectedValue + ","
                    + selectedRecord[i].get('id');
        }
        Ext.Msg.alert(selectedValue);
    },
    refresh: function () {
        this.store.loadPage(this.store.currentPage);
    },
    search: function (post_data) {
        this.store.currentPage = 1;
        if (post_data != null && post_data.params != null) {
            this.params = post_data.params;
            this.store.clearFilter(true);
            this.store.filter(post_data.params);
            this.store.loadPage(this.store.currentPage);
        } else {
            this.store.load({
                url: post_data.post_url
            });
        }
        // this.store.loadData(post_data.items,false);
    },
    searchinGrid: function (e) {
        var panel = e.up('gridpanel');
        this.store.currentPage = 1;
        var gridSearch = panel.dockedItems.items[1].items.items[0]
                .getValue();
        this.store.clearFilter(true);
        this.store.filter([{
            property: 'gridSearchKeyword',
            value: gridSearch
        }]);
        this.store.loadPage(this.store.currentPage);
    },
    getCheckIds: function () {
        var selModel = null;
        var selectedValue = "";
        if (this.selModel != null && this.selModel != undefined) {
            selModel = this.selModel;
            var selectedRecord = selModel.getSelection();
            for (var i = 0; i < selectedRecord.length; i++) {
                if (i != 0) {
                    selectedValue = selectedValue + ","
                            + selectedRecord[i].get('id');
                } else {
                    selectedValue = selectedValue
                            + selectedRecord[i].get('id');
                }
            }
        }
        return selectedValue;
    }
})
