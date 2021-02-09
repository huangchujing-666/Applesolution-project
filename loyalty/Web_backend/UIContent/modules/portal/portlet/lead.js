
    {success: true,
    columns: [    
    {header: 'Lead Name', dataIndex: 'lead_name',width:200,type:'title'},
    {header:'Date',dataIndex:'date',width:120}       
    ],
     fields: ['id','lead_name','date','href'],
     title: 'Latest Lead',
     pageSize:20,
     delete_url: 'delete_data.jsp',
     url:'../UIContent/modules/portal/portlet/list_lead_data.js',
     add_hidden:true,
     search_text_hidden:true,
     checkbox_hidden:true,
     isPageBar:true,
     isTopBar:false
    }