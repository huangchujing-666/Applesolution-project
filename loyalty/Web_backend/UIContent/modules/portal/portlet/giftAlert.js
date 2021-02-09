
    {success: true,
    columns: [    
    {"header":"Gift Code","dataIndex":"gift_no","width":"80","type":"title","renderer":null,"sortable":true,"column":true},
    {"header":"Name","dataIndex":"name","width":"160","type":null,"renderer":null,"sortable":true,"column":true},
    {"header":"Current Stock","dataIndex":"stock","width":"80","type":null,"renderer":null,"sortable":true,"column":true},
    {"header":"Alert Level","dataIndex":"alert_level","width":"80","type":null,"renderer":null,"sortable":true,"column":true}
    ],
     fields: ["id","href","gift_id","photo","gift_no","name","stock","alert_level"],
     title: 'Gift without enough Stock',
     pageSize:20,
     delete_url: 'delete_data.jsp',
     url:'../UIContent/modules/portal/portlet/list_giftAlert_data.js',
     add_hidden:true,
     search_text_hidden:true,
     checkbox_hidden:true,
     isPageBar:true,
     isTopBar:false
    }
