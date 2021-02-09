using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.Security;

namespace Palmary.Loyalty.BO.Modules.Administration.Role
{
    public class RolePrivilegeManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private static string _module = CommonConstant.Module.rolePrivilege;

        private AccessManager _accessManager;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public RolePrivilegeManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }


        public bool Create(

            int object_type,
            int object_id,
            int section_id,
            int read_status,
            int insert_status,
            int update_status,
            int delete_status,
            int status,
            
            ref string sql_remark
        )
        {
            // Access Checking
            AccessManager accessManager = new AccessManager();
            var privilege = accessManager.AccessModule(_module);
            
            var create_result = false;

            if (privilege.insert_status == 1)
            {
                int? get_sql_result = 0;
                
                var result = db.sp_CreateRolePrivilege(
                  _accessObject.id, 
                   _accessObject.type,

                    object_type,
                    object_id,
                    section_id,
                    read_status,
                    insert_status,
                    update_status,
                    delete_status,
                    status,

                    ref get_sql_result, ref sql_remark);

                create_result = (int)get_sql_result.Value == 1 ? true : false;
            }
            else
            {
                sql_remark = "No Access";
            }
            return create_result;
        }

        public bool Delete(
            int access_object_id,
            int object_type,
            int object_id,

            ref string sql_remark
        )
        {
            int? get_sql_result = 0;
            
            var result = db.sp_DeleteRolePrivilege(
                access_object_id,

                object_type,
                object_id,

                ref get_sql_result, ref sql_remark);

            return (get_sql_result == 1 ? true : false);
        }

        // Manager Object (DAO)  UPDATE
        public bool Update(

            int object_type,
            int object_id,
            int section_id,
            int read_status,
            int insert_status,
            int update_status,
            int delete_status,
            int status,
           
            ref string sql_remark
        )
        {
            int? get_sql_result = 0;
            
            var result = db.sp_UpdatePrivilege(
                _accessObject.id, 
                _accessObject.type, 

                object_type,
                object_id,
                section_id,
                read_status,
                insert_status,
                update_status,
                delete_status,
                status,

                ref get_sql_result, ref sql_remark);

            return (get_sql_result == 1 ? true : false);
        }


        public bool Update(List<RolePrivilegeObject> privilegeList, ref string sql_remark)
        {
            // Access Checking
            AccessManager accessManager = new AccessManager();
            var privilege = accessManager.AccessModule(_module);

            var sql_result = false;

            if (privilege.update_status == 1)
            {
                foreach (var p in privilegeList)
                {
                    var savedPrivilege = GetPrivilege_detail(p.object_id, p.section_id, ref sql_result);

                    if (sql_result && savedPrivilege.privilege_id > 0) // Exist
                    {
                        sql_result = Update(
                            p.object_type,
                            p.object_id,
                            p.section_id,
                            p.read_status,
                            p.insert_status,
                            p.update_status,
                            p.delete_status,
                            p.status,
                            ref sql_remark
                        );
                    }
                    else if (!sql_result && savedPrivilege.privilege_id == 0) // Not Exist
                    {
                        sql_result = Create(
                            p.object_type,
                            p.object_id,
                            p.section_id,
                            p.read_status,
                            p.insert_status,
                            p.update_status,
                            p.delete_status,
                            p.status,
                            ref sql_remark
                        );


                    }
                }
            }

            return sql_result;
        }

        public sp_GetPrivilege_detailResult GetPrivilege_detail(int target_id, int section_id, ref bool sql_result)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetPrivilege_detail(SessionManager.Current.obj_id, target_id, section_id, ref get_sql_result, ref sql_remark);

            // convert from IEnumerable List to Generic List
            var result_list = result.ToList();

            sql_result = false;
            if (result_list.Count() > 0)
                sql_result = true;

            return result_list.FirstOrDefault() ?? new sp_GetPrivilege_detailResult();
        }

    }
}
