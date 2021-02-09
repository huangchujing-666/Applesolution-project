Ext.define('com.palmary.memberfield.js.index', {
    
    gridPanel: undefined,
    addTag: function () {
        new com.embraiz.tag().openNewTag('memberProfile:401', 'Member Profile:Add', 'com.palmary.memberProfile.js.insert', 'iconUser16', 'user:add');
    },
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        target_div_summary = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div_summary);

        var form_tool = new com.embraiz.component.form_tool();
        var tool_div = form_tool.genTool(tab, id);
        var target_div = form_tool.gen_form_div(tab);

        // list
        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);
        
        Ext.Ajax.request({
            url: '../MemberField/ViewBasicData/',
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.form().editForm(target_div_summary, data_json, null, '', '', '');

            },
            scope: this
        });

        var form = new com.embraiz.component.form();
        // form.viewForm(target_div, '../MemberCard/getModule/' + id);
        form.showToolBar(tool_div, '../MemberField/toolbarData/');

        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/memberfield",
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json, true);
            },
            scope: this
        });       
    }
});