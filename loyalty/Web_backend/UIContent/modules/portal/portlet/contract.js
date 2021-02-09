
    {success: true,
    columns: [    
    {header: 'Contract No', dataIndex: 'contract_no',width:100,type:'title'},
    {header: 'Quotation No',dataIndex:'quotation_no',width:100,type:'title1'},
    {header: 'Start Date',dataIndex:'start_date',width:100},
    {header: 'End Date',dataIndex:'end_date',width:100}       
    ],
     fields: ['id','contract_no','quotation_no','start_date','end_date','href','href1'],
     title: 'Near end Contract',
     pageSize:20,
     delete_url: 'delete_data.jsp',
     url:'../UIContent/modules/portal/portlet/list_contract_data.js',
     add_hidden:true,
     search_text_hidden:true,
     checkbox_hidden:true,
     isPageBar:true,
     isTopBar:false
    }