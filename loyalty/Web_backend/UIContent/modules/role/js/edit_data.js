Ext.define('com.palmary.role.js.edit', {
    gridPanel: undefined,
    initTag: function (tab, url, title, id) {

        // Check user seesion 
        checkSession();

        var form_tool = new com.embraiz.component.form_tool();
        var tool_div = form_tool.genTool(tab, id);
        var target_div = form_tool.gen_form_div(tab);
        var _id = id.substring(id.indexOf(':') + 1, id.length);

        //tab.getEl().dom.lastChild.appendChild(tool_div);
        //target_div = document.createElement("div");
        //target_div.setAttribute("tabindex", "0"); 
        //tab.getEl().dom.lastChild.appendChild(target_div);
        //grider_div = document.createElement("div");
        //tab.getEl().dom.lastChild.appendChild(grider_div);

        var form = new com.embraiz.component.form();
        form.viewForm(target_div, '../role/EditViewForm/' + _id);
        form.showToolBar(tool_div, '../role/EditView_toolbarData/' + _id, id);

        //new com.embraiz.component.girdTable().initGrid('../user/GridHeader?id=' + id, '../user/ListData?id=' + id, grider_div);
        //new com.embraiz.component.gridkeyboard().keyboard([grider_div]);
    }
});