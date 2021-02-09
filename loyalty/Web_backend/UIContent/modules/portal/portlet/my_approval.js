
    {success: true,
    columns: [    
    {header: 'Form Name', dataIndex: 'form_name',width:100,type:'title',sortable:true},
    {header: 'Submitted By', dataIndex: 'createByUser',width:100,hidden:true},  
    {header: 'Date', dataIndex: 'submitDate',width:150}          
    ],
     fields: ['id','form_name','createByUser','submitDate','href','target'],
     title: 'Approval List',
     pageSize:20,
     delete_url: 'delete_data.jsp',
     url:'../UIContent/modules/approvalList/list_data.js',
     add_hidden:true,
     search_text_hidden:true,
     checkbox_hidden:true,
     isPageBar:true,
     isTopBar:false
    }