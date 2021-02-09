Ext.define('com.palmary.gift.js.edit', {
    gridPanel: undefined,
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        tool_div = document.createElement("div");
        //tool_div.style.height = 27;
        id = id.substring(id.indexOf(':') + 1, id.length);
        tool_div.id = "tool_div" + id;
        tab.getEl().dom.lastChild.appendChild(tool_div); //toolbar

        // Build div
        target_div = document.createElement("div");
        target_div.setAttribute("tabindex", "0");
        tab.getEl().dom.lastChild.appendChild(target_div);

        grider_div1 = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div1);

        //grider_div2 = document.createElement("div");
        //tab.getEl().dom.lastChild.appendChild(grider_div2);

        //grider_div3 = document.createElement("div");
        //tab.getEl().dom.lastChild.appendChild(grider_div3);

        // Gift Form
        var form = new com.embraiz.component.form();
        form.viewForm(target_div, '../gift/getModule/' + id);
        form.showToolBar(tool_div, '../gift/toolbarData/' + id, id);

        //// Gift Photo
        //Ext.Ajax.request({
        //    url: '../giftPhoto/GetEditForm/'+id,
        //    success: function (o) {
        //        var data_json = Ext.decode(o.responseText);
        //        new com.embraiz.component.form().editForm(grider_div1, data_json, null);
        //    },
        //    scope: this
        //});
        
        //var dataJosn=new Object();
        ////dataJosn.post_url = '../Common/ListData/giftPhoto/' + id;
        ////dataJosn.post_header = '../giftPhoto/GridHeader/' + id;
        ////new com.embraiz.component.gridSearch().render(target_div, grider_div1, dataJosn, true);

        //dataJosn.post_url = '../Common/ListData/giftPrivilege/' + id;
        //dataJosn.post_header = '../giftPrivilege/GridHeader/' + id;
        //new com.embraiz.component.girdTable().initGrid('../giftPrivilege/GridHeader/' + id, '../Common/ListData/giftPrivilege/' + id, grider_div1);
        ////new com.embraiz.component.girdTable().initGrid(dataJosn.post_header, dataJosn.post_url, target_div);

        //dataJosn.post_url = '../Common/ListData/giftLocation/' + id;
        //dataJosn.post_header = '../giftLocation/GridHeader/' + id;
        //new com.embraiz.component.gridSearch().render(target_div, grider_div3, dataJosn, true);
    }
});