Ext.define('com.embraiz.assign.js.index', {

    initTag: function (tab, url, title) {
        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        target_div.style.margin = "5px";
        assign_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(assign_div);
        assign_div.style.margin = "5px";

        var roleStore = Ext.create('Ext.data.Store', {
            fields: [
						{ name: 'role_Id', type: 'int' },
						{ name: 'role_name', type: 'string' }
            ],
            autoLoad: true,
            id: roleStore,
            proxy: {
                type: 'ajax',
                url: '../Table/Init/UserRole', ///modules/assign/list_role.json',
                reader: {
                    type: 'json',
                    root: 'data'
                }
            }
        });

        // var form = Ext.create('Ext.form.Panel', {	
        Ext.create('Ext.panel.Panel', {
            renderTo: target_div,
            title: 'Assign Right To Role',
            autoHeight: true,
            bodyStyle: 'padding:5px 5px 0',
            defaults: {
                layout: 'Anchor',
                labelWidth: 50
            },
            items: [
                {
                    xtype: 'combo',
                    fieldLabel: 'Role',
                    width: 300,
                    id: 'rolecombo',
                    store: roleStore,
                    queryMode: 'local',
                    emptyText: 'Please Selected',
                    displayField: 'role_name',
                    valueField: 'role_Id',
                    renderTo: target_div,
                    listeners: {
                        select: function (combo, record, index) {
                            
                            Ext.Ajax.request({
                                url: '../Table/ListData/RoleAccessDetail', //'modules/assign/list_role_menu.json',
                                success: showData,
                                waitTitle: 'Status',
                                waitMsg: 'Process...',
                                params: {
                                    'roleId': record[0].data.role_Id
                                }
                            });

                            // if checked 'Check all', it should be unchecked after changing role 
                            this.ownerCt.items.items[2].setValue(false);

                        }
                    }
                }, {
                    xtype: 'button',
                    text: 'Save',
                    handler: function () {
                        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "Please wait..." });
                        var roleId = Ext.getCmp("rolecombo").getValue();
                        var checkRight = Ext.query("input[name*=right]");
                        var menuId = Ext.query("input[name*=menuId]");
                        var checkRightValue = [];
                        var checkRightName = [];
                        var menu_id = [];
                        for (var j = 0; j < menuId.length; j++) {
                            menu_id[j] = menuId[j].value;
                        }
                        for (var i = 0; i < checkRight.length; i++) {
                            if (checkRight[i].checked) {
                                checkRightValue[i] = 1;
                            } else {
                                checkRightValue[i] = 0;
                            }
                            checkRightName[i] = checkRight[i].name;
                        }
                        Ext.Ajax.request({
                            url: "../RolePrivilege/Update", //"modules/assign/save.jsp",
                            params: {
                                roleId: roleId,
                                menuId: menu_id,
                                checkRightValue: checkRightValue,
                                checkRightName: checkRightName
                            },
                            success: function (o) {
                                var strJson = o.responseText;

                                var obj = Ext.JSON.decode(strJson);

                                if (obj.result) {
                                    Ext.Msg.alert('Update', 'Success');
                                } else {
                                    Ext.Msg.alert('Update', 'Fail: ' + obj.msg);
                                }
                                myMask.hide();
                            }, failure: function (f, a) {
                                Ext.Msg.alert('Status', 'Fail');
                            },
                            scope: this
                        });
                    }
                }, {
                    xtype: 'checkbox',
                    boxLabel: 'Select All',
                    name: 'checkAll',
                    inputValue: '1',
                    id: 'checkAll',
                    handler: function () {
                        var tabs = tab.body;
                        var abc = Ext.query("input[class*=assign_checkbox]");
                        for (var i = 0; i < abc.length; i++) {

                            if (abc[i].name.indexOf('rightLog') == -1) {
                                abc[i].checked = this.checked;
                            }
                        }
                    }
                }]
        });
        //Focus on Form
        document.onhelp = function () { return false };
        window.onhelp = function () { return false };
        new Ext.util.KeyMap(document, {
            key: 112,
            fn: function () {
                var activeTab = Ext.getCmp("content-panel").getActiveTab();
                activeTab.focus();
                var curTabElement = null;
                var curExtCtl = null;
                for (var i = 1; i < Ext.getCmp("content-panel").items.getCount() ; i++) {
                    curTabElement = Ext.getCmp("content-panel").items.get(i);
                    if (curTabElement.getId() == activeTab.getId()) {
                        curExtCtl = Ext.getCmp("content-panel").items.get(i);
                        break;
                    }
                }
                if (curExtCtl == null) {
                    return;
                }
                var divs = curExtCtl.getEl().dom.lastChild;
                if (divs != null && divs != undefined) {
                    var curExtForm = Ext.DomQuery.select('input[type=text],button[type=button],input[type=button]', target_div);
                    Ext.each(curExtForm, function (o, i, curExtForm) {
                        if (i == 0) {
                            Ext.get(o).focus(true);
                        }
                        var config = [{
                            key: 37,
                            fn: function () {
                                if (i > 0 && i < curExtForm.length) {
                                    curExtForm[i - 1].focus(true);
                                    return false;
                                }
                            }
                        }, {
                            key: 39,
                            fn: function () {
                                if (i < curExtForm.length - 1) {
                                    curExtForm[i + 1].focus(true);
                                }
                            }
                        }];
                        new Ext.util.KeyMap(Ext.get(o).id, config);
                    });
                }
            }
        });
        /////
        var tplCase = new Ext.XTemplate(
                   '<form class="myForm" action="modules/assign/save.jsp" id="form123" name="myForm" method="post"><table class="gridTable" cellspacing="0">',
                   '<tr><th width="120">Module</th><th width="130">&nbsp;</th><th width="120">&nbsp;</th><th width="280">&nbsp;</th></tr>',
                   '<tpl for="items"><tr>',
                   '<tpl if="menuLevel==1">',
                   '<td>{menuName}&nbsp;</td>',
                   '</tpl>',
                   '<tpl if="menuLevel!=1">',
                   '<td>&nbsp;</td>',
                   '</tpl>',
                   '<tpl if="menuLevel==2">',
                   '<td>{menuName}&nbsp;</td>',
                   '</tpl>',
                   '<tpl if="menuLevel!=2">',
                   '<td>&nbsp;</td>',
                   '</tpl>',
                   '<tpl if="menuLevel==3">',
                   '<td>{menuName}&nbsp;<div align="right"><input type="checkbox" onchange="changeRight(this,{menuId})"/>&nbsp;Select All</div></td>',
                   '</tpl>',
                   '<tpl if="menuLevel==2">',
                   '<td>&nbsp;<div align="right"><input type="checkbox" onchange="changeRight(this,{menuId})"/>&nbsp;Select All</div></td>',
                   '</tpl>',
                   '<tpl if="menuLevel==1">',
                   '<td>&nbsp;</td>',
                   '</tpl>',
                   '<td>',
                   '<tpl if="leaf==1">',
                   '<input type="hidden" name="menuId" value="{menuId}"/>',
                   '<input class="assign_checkbox" type="checkbox" name="rightR{menuId}" ',
                   '<tpl if="this.isChecked(rightR)">',
                   ' checked ',
                   '</tpl>',
                   'value="1"/>&nbsp;&nbsp;Read &nbsp;&nbsp;',
                   '<input class="assign_checkbox" type="checkbox" name="rightI{menuId}"  ',
                   '<tpl if="this.isChecked(rightI)">',
                   ' checked ',
                   '</tpl>',
                   'value="1"/>&nbsp;&nbsp;Insert &nbsp;&nbsp;',
                   '<input class="assign_checkbox" type="checkbox" name="rightU{menuId}"  ',
                   '<tpl if="this.isChecked(rightU)">',
                   ' checked ',
                   '</tpl>',
                   'value="1"/>&nbsp;&nbsp;Update &nbsp;&nbsp;',
                   '<input class="assign_checkbox" type="checkbox" name="rightD{menuId}"  ',
                   '<tpl if="this.isChecked(rightD)">',
                   ' checked ',
                   '</tpl>',
                   'value="1"/>&nbsp;&nbsp;Delete &nbsp;&nbsp;',

                   //'<tpl if="menuName==\'User\'">',
                   //'&nbsp;&nbsp;<input class="assign_checkbox" type="checkbox" name="rightLog{menuId}"',				
                   //'<tpl if="this.isChecked(rightLog)">',
                   //' checked ',
                   //'</tpl>',				
                   //'value="1"/>&nbsp;&nbsp;History Log',
                   //'</tpl>',
                   '</tpl>',

                   '<tpl if="leaf==0">',
                   '<input type="hidden" name="menuId" value="{menuId}"/><input class="assign_checkbox" type="checkbox" name="rightR{menuId}" ',
                   '<tpl if="this.isChecked(rightR)">',
                   ' checked ',
                   '</tpl>',
                   'value="1"/>&nbsp;&nbsp;Read&nbsp;&nbsp;',
                   '</tpl>',
                   '</td></tr>',
                   '</tpl>',
                   '</table></form>', {
                       isChecked: function (name) {
                           return name == '1';
                       }
                   }
           );

        function showData(o) {
            var data = Ext.decode(o.responseText);
            tplCase.overwrite(assign_div, data);
        }

        //grid focus
        new Ext.util.KeyMap(Ext.getCmp('content-panel').getActiveTab().id, {
            key: 120,
            fn: function () {
                var table = Ext.DomQuery.select('gridTable', assign_div);
                if (table != null) {
                    var allcheck = Ext.DomQuery.select('input[type=checkbox]', assign_div);
                    Ext.each(allcheck, function (o, i, allcheck) {
                        if (i == 0) {
                            Ext.get(o).focus(true);
                            new Ext.KeyMap(Ext.get(o).id, {
                                key: 13,
                                handler: function () {
                                    if (Ext.get(o).checked == true) {
                                        Ext.get(o).checked = false;
                                    } else {
                                        Ext.get(o).checked = true;
                                    }
                                }
                            });
                        }
                        var config = [{
                            key: 37,
                            fn: function () {
                                if (i > 0 && i < allcheck.length) {
                                    allcheck[i - 1].focus(true);
                                    new Ext.KeyMap(allcheck[i - 1].id, {
                                        key: 13,
                                        handler: function () {
                                            if (allcheck[i - 1].checked == true) {
                                                allcheck[i - 1].checked = false;
                                            } else {
                                                allcheck[i - 1].checked = true;
                                            }
                                        }
                                    });
                                    return false;
                                }
                            }
                        }, {
                            key: 39,
                            fn: function () {
                                if (i < allcheck.length - 1) {
                                    allcheck[i + 1].focus(true);
                                    new Ext.KeyMap(allcheck[i + 1].id, {
                                        key: 13,
                                        handler: function () {
                                            if (allcheck[i + 1].checked == true) {
                                                allcheck[i + 1].checked = false;
                                            }
                                            else {
                                                allcheck[i + 1].checked = true;
                                            }
                                        }
                                    });
                                }
                            }
                        }];
                        new Ext.util.KeyMap(Ext.get(o).id, config);
                    });
                }
            }
        });

    }
});

function changeRight(controlBtn, id) {

    var obj = document.getElementsByTagName("input");
    for (var i = 0; i < obj.length; i++) {

        if (obj[i].type == "checkbox" && obj[i].name == 'rightR' + id) {
            obj[i].checked = controlBtn.checked;
        }
        if (obj[i].type == "checkbox" && obj[i].name == 'rightI' + id) {
            obj[i].checked = controlBtn.checked;
        }
        if (obj[i].type == "checkbox" && obj[i].name == 'rightU' + id) {
            obj[i].checked = controlBtn.checked;
        }
        if (obj[i].type == "checkbox" && obj[i].name == 'rightD' + id) {
            obj[i].checked = controlBtn.checked;
        }
    }
}