
    {success: true,
    columns: [    
        {"header":"Action Type","dataIndex":"action_type_name","width":"120","type":null,"renderer":null,"sortable":true,"column":true},
        {"header":"Target Object Type","dataIndex":"target_obj_type_name","width":"150","type":null,"renderer":null,"sortable":true,"column":true},
        {"header":"Target Object Name","dataIndex":"target_obj_name","width":"150","type":"title1","renderer":null,"sortable":true,"column":true},
        {"header":"Log Date","dataIndex":"log_date","width":"700","type":null,"renderer":null,"sortable":true,"column":true},
        {"header":"Action IP","dataIndex":"action_ip","width":"150","type":null,"renderer":null,"sortable":true,"column":true}
    ],
     fields: ["id","href","href1","access_obj_name","action_channel_name","action_type_name","target_obj_type_name","target_obj_name","action_ip","log_date"],
     title: 'Audit Trail',
     pageSize:20,
     delete_url: 'delete_data.jsp',
     url:'../Table/ListData/log',
     add_hidden:true,
     search_text_hidden:true,
     checkbox_hidden:true,
     isPageBar:true,
     isTopBar:false
    }
