
    {success: true,
    columns: [    
     {header: '', dataIndex: 'img',width:50,sortable:true,renderFun:function(val){return this.colRender(val);}},
     {header: 'Module', dataIndex: 'module',width:204,type:'title'}         
    ],
     fields: ['id','img','module','href','target'],
     url:'../UIContent/modules/portal/portlet/list_quick_link.js',
     isPageBar:false,
     isTopBar:false
    }