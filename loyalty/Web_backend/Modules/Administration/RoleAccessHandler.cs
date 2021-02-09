using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Modules.Administration.Role;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.DB;

namespace Palmary.Loyalty.Web_backend.Modules.Administration
{
    public class RoleAccessHandler
    {
        public string Get_RoleDropDownListData()
        {
            var roleManager = new RoleManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var searchParmList = new List<SearchParmObject>();
            var rowTotal = 0;

            var result = roleManager.GetList(0, 100, searchParmList, "", CommonConstant.SortOrder.asc, ref resultCode, ref rowTotal).ToList();

            var list = new List<ExtJsDataRowRole_DropDownList> { };

            foreach (var x in result)
            {
                if (x.status == CommonConstant.Status.active)
                {
                    list.Add(new ExtJsDataRowRole_DropDownList()
                    {
                        role_Id = x.role_id,
                        role_name = x.name
                    });
                }
            }
            return list.ToJson();
        }

        public string Get_RoleAccessDetail(int user_id, int role_id)
        {
            RoleAccessManager roleaccessManager = new RoleAccessManager();
            var result = roleaccessManager.GetRoleAccessDetail(user_id, role_id).ToList();

            var list = new List<ExtJsDataRowRoleDetail> { };

            var result_parent = from r in result
                                where r.parent == 0
                                select r;

            int counter = 0;
            foreach (var item_parent in result_parent)
            {
                // if role = admin, hide this module (not allow to edit this privilege)
                if (!(role_id == 1 && item_parent.section_id == 1))
                {
                    list.Insert(counter, new ExtJsDataRowRoleDetail
                    {
                        menuId = item_parent.section_id,
                        menuName = item_parent.name,
                        menuLevel = item_parent.leaf + 1,
                        rightR = item_parent.read_status.ToString(),
                        rightU = item_parent.update_status.ToString(),
                        rightD = item_parent.delete_status.ToString(),
                        rightI = item_parent.insert_status.ToString(),
                        rightLog = "",
                        leaf = item_parent.leaf.ToString()
                    });
                    counter++;

                    var result_child = from r in result
                                       where r.parent == item_parent.section_id
                                       select r;

                    foreach (var item in result_child)
                    {
                        list.Insert(counter, new ExtJsDataRowRoleDetail
                        {
                            menuId = item.section_id,
                            menuName = item.name,
                            menuLevel = item.leaf + 1,
                            rightR = item.read_status.ToString(),
                            rightU = item.update_status.ToString(),
                            rightD = item.delete_status.ToString(),
                            rightI = item.insert_status.ToString(),
                            rightLog = "",
                            leaf = item.leaf.ToString()
                        });
                        counter++;
                    }
                }
            }
            return list.ToJson();
        }
    }
}