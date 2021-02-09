Ext.define('com.palmary.Cronjob.js.index', {
    gridPanel: undefined,
    //addTag: function () {
    //    new com.embraiz.tag().openNewTag('user:401', 'User:Add', 'com.palmary.user.js.insert', 'iconUser16', 'user:add');
    //},
    //copyuser: function (userId) {
    //    new com.embraiz.tag().openNewTag(userId, 'User:Copy', 'com.palmary.user.js.copyuser', 'iconUser16', 'user:add');
    //},
    initTag: function (tab, url, title, id, itemId) {

        // Check user seesion 
        checkSession();

        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);
        Ext.Ajax.request({
            url: "../Table/InitWithSearchColumn/cronjob", //"../user/index",
            async: true,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div, grider_div, data_json, true);
            },
            scope: this
        });
    }//,
    //copyUser: function (userId) {
    //    new com.embraiz.user.js.index().copyuser(userId);

    //},
    //resetPwd: function (userId) {
    //    Ext.MessageBox.confirm('Confirm', 'Are you sure you want to Reset password?', function (btn) {
    //        if (btn == 'yes') {
    //            Ext.Ajax.request({
    //                url: "../user/resetPassword",
    //                params: {
    //                    userId: userId
    //                },
    //                success: function (o) {
    //                    Ext.Msg.alert('Success', 'Reset password success!');
    //                },
    //                scope: this
    //            });
    //        } else {
    //            this.close();
    //        }
    //    });
    //}
});
