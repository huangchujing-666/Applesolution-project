using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Palmary.Loyalty.Web_backend.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/

        public ActionResult Index()
        {
            return View();
        }


        public string json_grid_index_quick_link(){

            var result = @"
            {
            post_url:'../Dashboard/json_list_data_quick_link',
            post_header:'../Dashboard/json_grid_header_quick_link'
            }";

            return result;
        }

        public string json_grid_header_quick_link()
        {

            var result = @"
           {success: true,
            columns: [ 
            {header: '',dataIndex: 'status',width:'50',type:'img', imgHeight:'16', imgWidth:'16'},
             {header: 'Role Name', dataIndex: 'name',width:'100',type:'title'}

            ],
             fields: ['id','name','status','href','target'],
            title: 'Quick Link',
             pageSize:8,
            add_url:'new com.embraiz.role.js.index().addTag',
            add_hidden:true,
            search_text_hidden:true,
            delete_url:'modules/role/del.jsp',
            delete_hidden:true,
            checkbox_hidden:true,
            displayMsg:1
            }";

            return result;
        }

        public string json_list_data_quick_link()
        {

            var result = @"
           {""totalCount"":"""",""items"":[
            {""id"":8,""name"":""ADMIN"",""status"":""../UIContent/icons/obj/coreUserGroup/16.png"",""href"":""new com.embraiz.tag().openNewTag('Role:254','Role:ADMIN','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','8')"",""target"":""_self""},
            {""id"":1,""name"":""adminstartor"",""status"":""../UIContent/icons/obj/coreUserGroup/16.png"",""href"":""new com.embraiz.tag().openNewTag('Role:1','Role:adminstartor','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','1')"",""target"":""_self""},
            {""id"":4,""name"":""CIC"",""status"":""../UIContent/icons/obj/coreUserGroup/16.png"",""href"":""new com.embraiz.tag().openNewTag('Role:4','Role:CIC','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','4')"",""target"":""_self""},
            {""id"":6,""name"":""Contact"",""status"":""../UIContent/icons/obj/coreUserGroup/16.png"",""href"":""new com.embraiz.tag().openNewTag('Role:6','Role:Contact','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','6')"",""target"":""_self""},
            {""id"":7,""name"":""Owner"",""status"":""../UIContent/icons/obj/coreUserGroup/16.png"",""href"":""new com.embraiz.tag().openNewTag('Role:7','Role:Owner','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','7')"",""target"":""_self""},
            {""id"":2,""name"":""Staff"",""status"":""../UIContent/icons/obj/coreUserGroup/16.png"",""href"":""new com.embraiz.tag().openNewTag('Role:2','Role:Staff','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','2')"",""target"":""_self""},
            {""id"":5,""name"":""Student"",""status"":""../UIContent/icons/obj/coreUserGroup/16.png"",""href"":""new com.embraiz.tag().openNewTag('Role:5','Role:Student','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','5')"",""target"":""_self""},
            {""id"":3,""name"":""Tutor"",""status"":""../UIContent/icons/obj/coreUserGroup/16.png"",""href"":""new com.embraiz.tag().openNewTag('Role:3','Role:Tutor','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','3')"",""target"":""_self""}

            ]}";

            return result;
        }

        public string json_list_data_pie()
        {

            var result = @"
            {""data"": [
			   {'name':'aa', 'value':'10'},
			   {'name':'bb', 'value':'20'},
			   {'name':'cc', 'value':'38'},
			   {'name':'dd', 'value':'10'},
			   {'name':'ee', 'value':'11'}
			   ]
            }
            ";

            return result;
        }

        public string json_grid_index()
        {

            var result = @"
            
            {
            post_url:'../Dashboard/json_list_data',
            post_header:'../Dashboard/json_grid_header'
            }
            ";

            return result;
        }

        public string json_list_data()
        {

            var result = @"
            
            {""totalCount"":"""",""items"":[
            {""id"":8,""name"":""ADMIN"",""status"":1,""href"":""new com.embraiz.tag().openNewTag('Role:254','Role:ADMIN','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','8')"",""target"":""_self""},
            {""id"":1,""name"":""adminstartor"",""status"":1,""href"":""new com.embraiz.tag().openNewTag('Role:1','Role:adminstartor','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','1')"",""target"":""_self""},
            {""id"":4,""name"":""CIC"",""status"":1,""href"":""new com.embraiz.tag().openNewTag('Role:4','Role:CIC','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','4')"",""target"":""_self""},
            {""id"":6,""name"":""Contact"",""status"":1,""href"":""new com.embraiz.tag().openNewTag('Role:6','Role:Contact','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','6')"",""target"":""_self""},
            {""id"":7,""name"":""Owner"",""status"":1,""href"":""new com.embraiz.tag().openNewTag('Role:7','Role:Owner','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','7')"",""target"":""_self""},
            {""id"":2,""name"":""Staff"",""status"":1,""href"":""new com.embraiz.tag().openNewTag('Role:2','Role:Staff','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','2')"",""target"":""_self""},
            {""id"":5,""name"":""Student"",""status"":1,""href"":""new com.embraiz.tag().openNewTag('Role:5','Role:Student','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','5')"",""target"":""_self""},
            {""id"":3,""name"":""Tutor"",""status"":1,""href"":""new com.embraiz.tag().openNewTag('Role:3','Role:Tutor','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','3')"",""target"":""_self""},
            {""id"":9,""name"":""Tutor"",""status"":1,""href"":""new com.embraiz.tag().openNewTag('Role:3','Role:Tutor','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','3')"",""target"":""_self""},
            {""id"":10,""name"":""Tutor"",""status"":1,""href"":""new com.embraiz.tag().openNewTag('Role:3','Role:Tutor','com.embraiz.role.js.edit','iconRole16','iconRole16','iconRole16','3')"",""target"":""_self""}
            ]}
            ";

            return result;
        }

        public string json_grid_header()
        {

            var result = @"
            
            {success: true,
            columns: [ 
             {header: 'Role Name', dataIndex: 'name',width:'100',type:'title'},
            {header: 'Status', dataIndex: 'status',width:'100',renderer:function(val){if(val=='1'){return 'Active';}else {return 'InActive';}}}
            ],
             fields: ['id','name','status','href','target'],
            title: 'Gift',
             pageSize:8,
            add_url:'new com.embraiz.role.js.index().addTag',
            add_hidden:true,
            search_text_hidden:true,
            delete_url:'modules/role/del.jsp',
            delete_hidden:true,
            checkbox_hidden:true,
            displayMsg:1
            }
            ";

            return result;
        }

    }
}
