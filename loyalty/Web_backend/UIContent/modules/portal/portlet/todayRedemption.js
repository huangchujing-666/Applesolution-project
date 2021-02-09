
    {success: true,
    columns: [    
    {"header":"Reference Code","dataIndex":"ref_no","width":"80","type":"title","renderer":null,"sortable":true,"column":true},
    {"header":"Member Name","dataIndex":"member_name","width":"100","type":null,"renderer":null,"sortable":true,"column":true},
    {"header":"Gift Name","dataIndex":"gift_name","width":"100","type":null,"renderer":null,"sortable":true,"column":true},
    {"header":"Quantity","dataIndex":"quantity","width":"80","type":null,"renderer":null,"sortable":true,"column":true},
    {"header":"Redeem Date","dataIndex":"crt_date","width":"100","type":null,"renderer":null,"sortable":true,"column":true}
    ],
     fields: ["id","href","href1","ref_no","member_no","member_name","gift_no","gift_name","quantity","quantity_larger","quantity_lower","point_used","point_used_larger","point_used_lower","location","redemption_status_name","collect_date","collect_date_larger","collect_date_lower","crt_date","crt_date_larger","crt_date_lower"],
     title: 'Today Redemption',
     pageSize:20,
     delete_url: 'delete_data.jsp',
     url:'../UIContent/modules/portal/portlet/list_todayRedemption_data.js',
     add_hidden:true,
     search_text_hidden:true,
     checkbox_hidden:true,
     isPageBar:true,
     isTopBar:false
    }
