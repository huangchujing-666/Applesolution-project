
    {success: true,
    columns: [    
     {header: '', dataIndex: 'img',width:40,sortable:true,renderFun:function(val){return this.colRender(val);}},
     {header: 'Module',dataIndex:'module',width:80},
     {header: 'Value', dataIndex: 'titles',width:134,type:'title'}         
    ],
     fields: ['id','img','module','titles','href','target'],
     url:'../UIContent/modules/portal/portlet/list_history_data.js',
     isPageBar:true,
     isTopBar:false
    }