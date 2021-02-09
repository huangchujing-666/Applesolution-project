
    {success: true,
    columns: [    
    {header: 'Company Name', dataIndex: 'company_name',width:200,type:'title'},
    {header: 'Total Amount',dataIndex:'total_amount',width:100,type:'title1'},
    {header:'Date' ,dataIndex:'date'}      
    ],
     fields: ['date','payment_id','company_name','total_amount','href','href1'],
     title: 'Latest Payment',
     pageSize:20,
     delete_url: 'delete_data.jsp',
     url:'../UIContent/modules/portal/portlet/list_payment_data.js',
     add_hidden:true,
     search_text_hidden:true,
     checkbox_hidden:true,
     isPageBar:true,
     isTopBar:false
    }