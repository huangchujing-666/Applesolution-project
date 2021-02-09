Ext.require(['Ext.grid.*', 'Ext.data.*']);

Ext.onReady(function () {

    Ext.define('com.embraiz.component.cellEditGridSearch', {
        editGrid_panel: undefined,
        target_div: undefined,
        grider_div: undefined,
        json_data: undefined,
        is_search: undefined,
        constructor: function (config) {
            config = config || {};
            Ext.apply(this, config);

            this.callParent([config]);
        },
        render: function (target_div, grider_div, json_data, is_search, json_data_url) {

            this.target_div = target_div;
            this.grider_div = grider_div;
            this.json_data = json_data;
            this.is_search = is_search;

            target_div.style.margin = "5px";
            grider_div.style.margin = "5px";

            Ext.Ajax.request({
                url: json_data.post_header,
                success: showGrid,
                scope: this
            });

            function showGrid(o) {
                var gird_info = Ext.decode(o.responseText);

                this.add_hidden = false;
                if (gird_info.add_hidden != null) {
                    this.add_hidden = gird_info.add_hidden;
                }

               var gird=this.editGrid_panel = Ext.create('com.embraiz.component.editGrid', {
                    grider_div: grider_div,	
                    json_data_url: json_data_url

                });
                if (json_data.row != undefined && json_data.row != null && json_data.row != "") {
                    var forms = new com.embraiz.component.form();
                    forms.editForm(target_div, json_data, gird);
                }
            }
        }
    });
});