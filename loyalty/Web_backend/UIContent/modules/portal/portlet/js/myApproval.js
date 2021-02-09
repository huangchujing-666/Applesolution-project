Ext.define('com.palmary.portlet.myApproval', {
    extend: 'com.palmary.app.portalGridPanel',
    alias: 'widget.myApprovalPortlet',
    initComponent: function () {
        var me = this;
        Ext.Ajax.request({
            url: '../UIContent/modules/portal/portlet/my_approval.js',
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                //me.config_data=data_json;
                me.setConfig_data(data_json);
            }
        });
        this.callParent(arguments);
        //me.addSearchField();
        //me.addSearchBtn();
    },
    addSearchBtn: function () {
        var me = this;
        this.dockedItems.items[2].add({
            text: 'Search',
            iconCls: 'iconSearch',
            handler: function () {
                me.searchGrid();
            }
        });
    },
    addSearchField: function () {
        var me = this;
        this.dockedItems.items[2].add({
            xtype: 'textfield',
            fieldLabel: '',
            enableKeyEvents: true,
            name: 'search_field',
            plugins: ['clearbutton'],
            listeners: {
                'keydown': function keyDownSearch(target, e, options) {
                    var raw = e.getKey();
                    if (raw == 13) {
                        me.searchGrid(target);
                    }
                }
            }
        });
    },
    searchGrid: function () {
        var field = this.dockedItems.items[2].items.items[0];
        this.store.clearFilter(true);
        this.store.filter([{
            property: 'approvalType',
            value: ''
        }, {
            property: 'form_name',
            value: field.getValue()
        }]);
        this.store.loadPage(this.store.currentPage);
    }
});