//Ext.define('com.palmary.core.imageUploadForm', {
//    init_pop_up: function (winform, id, itemId, itemName, refreshUrl) {

//        // Check user seesion 
//        checkSession();

//        winform.setHeight(350);
//        winform.setWidth(750);

//        var _id = id.substring(id.indexOf(':') + 1, id.length);

//        Ext.Ajax.request({
//            url: '../TransactionImport/GenerateForm/' + _id,
//            success: function (o) {
//                var data_json = Ext.decode(o.responseText);
//                new com.embraiz.component.form().editForm(winform, data_json, null, 'com.palmary.transactionimport.js.index', 'remarkList:' + itemId, itemName);
//            },
//            scope: this
//        });
//    }
//});

Ext.define('com.palmary.core.imageUploadForm', {
    extend: 'Ext.grid.Panel',
    requires: [
        'Ext.ux.multiupload.Upload'
    ],
    viewConfig: {
        markDirty: false
    },
    store: {
        fields: ['id', 'name', 'size', 'status', 'progress']
    },
    initComponent: function () {
        var me = this;

        me.addEvents('fileuploadcomplete');
        me.addEvents('filequeuedatacomplete');
        me.tbar = [{
            xtype: 'uploader',
            uploadConfig: this.uploadConfig,
            listeners:
            {
                'fileadded': function (source, file) {
                    this.up('grid').store.add({
                        id: file.fileIndex,
                        name: file.fileName,
                        size: file.fileSize,
                        status: 'waiting...',
                        progress: 0
                    });
                },
                'uploadstart': function (source, file) {
                    var grid = this.up('grid');
                    var record = grid.store.getById(file.fileIndex);

                    if (record) {
                        record.set('status', 'uploading...');
                    }
                },
                'uploadprogress': function (source, file) {
                    var grid = this.up('grid');
                    var record = grid.store.getById(file.fileIndex);
                    if (record) {
                        var p = Math.round(file.fileProgress / file.fileSize * 100);
                        record.set('progress', p);
                    }
                },
                'uploaddatacomplete': function (source, file) {
                    var grid = this.up('grid');
                    var record = grid.store.getById(file.fileIndex);
                    if (record) {
                        record.set('status', 'completed');
                    }
                    me.fireEvent('fileuploadcomplete', file.data);
                },
                'queuedatacomplete': function (source, data) {
                    Ext.Msg.show({
                        title: 'Info',
                        msg: 'Queue upload end. ' + data.files + ' file(s) uploaded.',
                        buttons: Ext.Msg.OK,
                        icon: Ext.Msg.INFO
                    });
                    me.fireEvent('filequeuedatacomplete', source, data);

                },
                'uploaderror': function (src, data) {
                    var msg = 'ErrorType: ' + data.errorType;

                    switch (data.errorType) {
                        case 'FileSize':
                            msg = 'This file is too big: ' + Ext.util.Format.fileSize(data.fileSize) +
                            '. The maximum upload size is ' + Ext.util.Format.fileSize(data.maxFileSize) + '.';
                            break;

                        case 'QueueLength':
                            msg = 'Queue length is too long: ' + data.queueLength +
                            '. The maximum queue length is ' + data.maxQueueLength + '.';
                            break;
                    }

                    Ext.Msg.show({
                        title: 'Upload Error',
                        msg: msg,
                        buttons: Ext.Msg.OK,
                        icon: Ext.Msg.ERROR
                    });
                }
            }
        }];

        me.columns = [
            {
                header: 'Id',
                dataIndex: 'id',
                width: 75,
                renderer: function (v) { return v + 1; }
            },
            { header: 'Name', dataIndex: 'name', width: 150 },
            { header: 'Size', dataIndex: 'size', renderer: Ext.util.Format.fileSize },
            { header: 'Status', dataIndex: 'status' },
            {
                header: 'Progress',
                dataIndex: 'progress',
                renderer: function (v) { return v + '%'; }
            }
        ];

        me.callParent(arguments);
    }
});
