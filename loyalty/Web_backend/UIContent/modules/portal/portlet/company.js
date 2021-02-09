
    {success: true,
    columns: [    
    {header: 'Company Name', dataIndex: 'company_name',width:200,type:'title'} ,
    {header:'Date',dataIndex:'date',width:120}     
    ],
     fields: ['id','company_name','date','href'],
     title: 'Latest Company',
     pageSize:20,
     delete_url: 'delete_data.jsp',
     url:'../UIContent/modules/portal/portlet/list_company_data.js',
     add_hidden:true,
     search_text_hidden:true,
     checkbox_hidden:true,
     isPageBar:true,
     isTopBar:false
    }