Ext.define('com.palmary.membersearch.js.list_member', {
    
    gridPanel: undefined,
    addTag: function () {
        
    },
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);
        
        var _id = id.substring(id.indexOf(':') + 1, id.length);

        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/MemberAdvanceSearchListMember/" + _id,
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json, true);
            },
            scope: this
        });       
    }
});