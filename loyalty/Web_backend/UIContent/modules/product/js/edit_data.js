Ext.define('com.palmary.product.js.edit', {
    gridPanel: undefined,
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        var form_tool = new com.embraiz.component.form_tool();
        var tool_div = form_tool.genTool(tab, id);
        var target_div = form_tool.gen_form_div(tab);
        var _id = id.substring(id.indexOf(':') + 1, id.length);

        // product custom info
        //target_div_custom = document.createElement("div");
        //tab.getEl().dom.lastChild.appendChild(target_div_custom);
        //grider_div_custom = document.createElement("div");
        //tab.getEl().dom.lastChild.appendChild(grider_div_custom);

        // product photo
        grider_div1 = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div1);
        
        var form = new com.embraiz.component.form();
        form.viewForm(target_div, '../product/getModule/' + _id);
        form.showToolBar(tool_div, '../product/ToolbarData/' + _id, id);
        
        // product photo
        //Ext.Ajax.request({
        //    url: '../ProductPhoto/GetEditForm/' + _id,
        //    success: function (o) {
        //        var data_json = Ext.decode(o.responseText);
        //        new com.embraiz.component.form().editForm(grider_div1, data_json, null);
        //    },
        //    scope: this
        //});

        // product custom info
        //Ext.Ajax.request({
        //    url: "../table/InitWithSearchColumn/product_custom_info/"+ _id,
        //    async: true,
        //    success: function (o) {
        //        var data_json = Ext.decode(o.responseText);
        //        new com.embraiz.component.gridSearch().render(target_div_custom, grider_div_custom, data_json, true);
        //    },
        //    scope: this
        //});
    }
});