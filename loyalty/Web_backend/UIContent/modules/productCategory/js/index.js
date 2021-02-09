Ext.define('com.palmary.productcategory.js.index', {
    gridPanel: undefined,
    addTag: function () {
        new com.embraiz.tag().openNewTag('ProductCategory:401', 'Product Category:Add', 'com.palmary.productCategory.js.insert', 'iconUser16', 'user:add');
    },
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        var category = new com.embraiz.component.category();
        category.initCategory(
        'Product Category',
        '../ProductCategory/Tree',//tree
        '../ProductCategory/getModule',// view edit detail
        '../ProductCategory/TreeMoveUpdate', //move_update
        '../ProductCategory/Insert'//add
        );

        //target_div = document.createElement("div");
        //tab.getEl().dom.lastChild.appendChild(target_div);
        //grider_div = document.createElement("div");
        //tab.getEl().dom.lastChild.appendChild(grider_div);
        //Ext.Ajax.request({
        //    url: "../table/InitWithSearchColumn/productCategory",
        //    async: true,
        //    success: function (o) {
        //        var data_json = Ext.decode(o.responseText);
        //        new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json, true);
        //    },
        //    scope: this
        //});
    }
});
