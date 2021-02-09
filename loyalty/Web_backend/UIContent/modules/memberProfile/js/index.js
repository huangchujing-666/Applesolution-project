Ext.define('com.palmary.memberprofile.js.index', {
    
    gridPanel: undefined,
    addTag: function () {
        new com.embraiz.tag().openNewTag('memberProfile:401', 'Member Profile:Add', 'com.palmary.memberProfile.js.insert', 'iconUser16', 'user:add');
    },
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);

        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/member",
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json, true);
            },
            scope: this
        });       
    }
});