using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

using Palmary.Loyalty.BO.Modules.Utility;

namespace Palmary.Loyalty.BO.Modules.Member
{
    public class MemberCardManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.memberCard;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;


        public MemberCardManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public CommonConstant.SystemCode Create(
            MemberCardObject dataObject
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var checkCode = CommonConstant.SystemCode.undefine;
                var memberManager = new MemberManager();
                //get member_id
                if (dataObject.member_id == 0 && !String.IsNullOrEmpty(dataObject.member_no))
                {
                    var member = memberManager.GetDetail_byMemberNo(dataObject.member_no, ref checkCode);

                    if (member.member_id <= 0)
                        system_code = CommonConstant.SystemCode.err_member_id;
                    else
                        dataObject.member_id = member.member_id;
                }
             
                // check exist current active card
                var current_card = CurrentCard(dataObject.member_id, ref checkCode);

                if (checkCode == CommonConstant.SystemCode.normal && current_card.card_id > 0)
                    system_code = CommonConstant.SystemCode.err_member_card_existusing;

                // check duplicate card no
                var duplicate_card = GetDetail(dataObject.card_no, ref checkCode);
                if (checkCode == CommonConstant.SystemCode.normal && duplicate_card.card_id > 0)
                    system_code = CommonConstant.SystemCode.err_member_cardno_duplicate;

                if (system_code == CommonConstant.SystemCode.undefine)
                {
                    var result = db.sp_CreateMemberCard(
                         _accessObject.id,
                _accessObject.type, 

                        dataObject.member_id,
                        dataObject.card_no,
                        dataObject.card_version,
                        dataObject.card_type,
                        dataObject.card_status,
                        dataObject.issue_date,
                        dataObject.old_card_id,
                        dataObject.remark,
                        dataObject.status,

                        ref sql_result
                        );

                    system_code = (CommonConstant.SystemCode)sql_result.Value;
                }
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }

        public CommonConstant.SystemCode IssueAndReissue(
            MemberCardObject dataObject
        )
        {
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var checkCode = CommonConstant.SystemCode.undefine;
                var memberManager = new MemberManager();
                var member = memberManager.GetDetail(dataObject.member_id, true, ref checkCode);

                //default value
                dataObject.status = (int)CommonConstant.Status.active;
                dataObject.card_status = (int)CommonConstant.MemberCardStatus.waiting_issue;
                dataObject.card_type = member.member_level_id;

                // check exist current active card
                var current_card = CurrentCard(dataObject.member_id, ref checkCode);

                if (current_card.card_id > 0)
                {
                    current_card.card_status = (int)CommonConstant.MemberCardStatus.voided;
                    system_code = Update(current_card);

                    dataObject.old_card_id = current_card.card_id;
                    dataObject.card_version = current_card.card_version + 1;
                    dataObject.card_no = "LT" + member.member_no + "-" + dataObject.card_version;

                    system_code = Create(dataObject);
                }
                else
                {
                    dataObject.old_card_id = 0;
                    dataObject.card_version = 0;
                    dataObject.card_no = "LT" + member.member_no;

                    system_code = Create(dataObject);
                }
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }

        public List<MemberCardObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            var resultList = new List<MemberCardObject>();

            if (_privilege.read_status == 1)
            {
                var query = (from mcard in db.member_cards
                             join m in db.member_profiles on mcard.member_id equals m.member_id

                             //card type
                             join ml in db.member_levels on mcard.card_type equals ml.level_id
                             
                             //card status
                             join li_cs in db.listing_items on mcard.card_status equals li_cs.value
                             join l_cs in db.listings on li_cs.list_id equals l_cs.list_id

                             where (
                                mcard.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l_cs.code == "MemberCardStatus"
                             
                            )
                             select new MemberCardObject
                             {
                                 card_id = mcard.card_id,
                                 member_id = mcard.member_id,
                                 card_no = mcard.card_no,
                                 card_version = mcard.card_version,
                                 card_type = mcard.card_type,
                                 card_status = mcard.card_status,
                                 issue_date = mcard.issue_date,
                                 old_card_id = mcard.old_card_id,
                                 remark = mcard.remark,
                                 status = mcard.status,
                                 crt_date = mcard.crt_date,
                                 crt_by_type = mcard.crt_by_type,
                                 crt_by = mcard.crt_by,
                                 upd_date = mcard.upd_date,
                                 upd_by_type = mcard.upd_by_type,
                                 upd_by = mcard.upd_by,
                                 record_status = mcard.record_status,

                                 // additional
                                 member_no = m.member_no,
                                 card_type_name = ml.name,
                                 card_status_name = li_cs.name
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (f.property == "member_no")
                    {
                        query = query.Where(x => x.member_no.Contains(f.value));
                    }
                    else if (f.property == "status")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.status == int.Parse(f.value));
                    }
                }

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "member_no"
                    || sortColumn == "card_no"
                    || sortColumn == "card_type_name"
                    || sortColumn == "card_status_name"
                    || sortColumn == "issue_date"
                    || sortColumn == "crt_date"
                    || sortColumn == "remark")

                    orderByColumn = sortColumn;
                else
                { //default
                    sortOrder = CommonConstant.SortOrder.desc;
                    orderByColumn = "crt_date";
                }

                // row total
                totalRow = query.Count();

                if (sortOrder == CommonConstant.SortOrder.desc)
                    resultList = query.OrderBy(orderByColumn, true).Skip(rowIndexStart).Take(rowLimit).ToList();
                else
                    resultList = query.OrderBy(orderByColumn, false).Skip(rowIndexStart).Take(rowLimit).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<MemberCardObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public List<MemberCardObject> GetListByMember(int member_id, int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            var resultList = new List<MemberCardObject>();

            if (_privilege.read_status == 1)
            {
                var query = (from mcard in db.member_cards
                             join m in db.member_profiles on mcard.member_id equals m.member_id

                             //card type
                             join ml in db.member_levels on mcard.card_type equals ml.level_id

                             //card status
                             join li_cs in db.listing_items on mcard.card_status equals li_cs.value
                             join l_cs in db.listings on li_cs.list_id equals l_cs.list_id

                             where (
                                mcard.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l_cs.code == "MemberCardStatus"
                                && m.member_id == member_id
                            )
                             select new MemberCardObject
                             {
                                 card_id = mcard.card_id,
                                 member_id = mcard.member_id,
                                 card_no = mcard.card_no,
                                 card_version = mcard.card_version,
                                 card_type = mcard.card_type,
                                 card_status = mcard.card_status,
                                 issue_date = mcard.issue_date,
                                 old_card_id = mcard.old_card_id,
                                 remark = mcard.remark,
                                 status = mcard.status,
                                 crt_date = mcard.crt_date,
                                 crt_by_type = mcard.crt_by_type,
                                 crt_by = mcard.crt_by,
                                 upd_date = mcard.upd_date,
                                 upd_by_type = mcard.upd_by_type,
                                 upd_by = mcard.upd_by,
                                 record_status = mcard.record_status,

                                 // additional
                                 member_no = m.member_no,
                                 card_type_name = ml.name,
                                 card_status_name = li_cs.name
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (f.property == "member_no")
                    {
                        query = query.Where(x => x.member_no.Contains(f.value));
                    }
                    else if (f.property == "status")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.status == int.Parse(f.value));
                    }
                }

                // dynamic sort
                Func<MemberCardObject, Object> orderByFunc = null;
                if (sortColumn == "member_no")
                    orderByFunc = x => x.member_no;
                else
                {
                    sortOrder = CommonConstant.SortOrder.desc;
                    orderByFunc = x => x.crt_date;
                }

                // row total
                totalRow = query.Count();

                if (sortOrder == CommonConstant.SortOrder.desc)
                    resultList = query.OrderByDescending(orderByFunc).Skip(rowIndexStart).Take(rowLimit).ToList();
                else
                    resultList = query.OrderBy(orderByFunc).Skip(rowIndexStart).Take(rowLimit).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<MemberCardObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public MemberCardObject GetDetail(int card_id, ref CommonConstant.SystemCode systemCode)
        {
            var resultObj = new MemberCardObject();

            
            var query = (from mcard in db.member_cards
                         join m in db.member_profiles on mcard.member_id equals m.member_id

                         //card type
                         join ml in db.member_levels on mcard.card_type equals ml.level_id

                         //card status
                         join li_cs in db.listing_items on mcard.card_status equals li_cs.value
                         join l_cs in db.listings on li_cs.list_id equals l_cs.list_id

                         where (
                            mcard.record_status != (int)CommonConstant.RecordStatus.deleted
                            && mcard.card_id == card_id
                            && l_cs.code == "MemberCardStatus"
                         )
                        select new MemberCardObject
                        {
                            card_id = mcard.card_id,
                            member_id = mcard.member_id,
                            card_no = mcard.card_no,
                            card_version = mcard.card_version,
                            card_type = mcard.card_type,
                            card_status = mcard.card_status,
                            issue_date = mcard.issue_date,
                            old_card_id = mcard.old_card_id,
                            remark = mcard.remark,
                            status = mcard.status,
                            crt_date = mcard.crt_date,
                            crt_by_type = mcard.crt_by_type,
                            crt_by = mcard.crt_by,
                            upd_date = mcard.upd_date,
                            upd_by_type = mcard.upd_by_type,
                            upd_by = mcard.upd_by,
                            record_status = mcard.record_status,

                            // additional
                            member_no = m.member_no,
                            card_type_name = ml.name,
                            card_status_name = li_cs.name

                        });

                
            resultObj = query.FirstOrDefault();

            if (resultObj != null)
                systemCode = CommonConstant.SystemCode.normal;
            else
                systemCode = CommonConstant.SystemCode.record_invalid;

            return resultObj;
        }

        public MemberCardObject GetDetail(string card_no, ref CommonConstant.SystemCode systemCode)
        {
            var resultObj = new MemberCardObject();


            var query = (from mcard in db.member_cards
                         join m in db.member_profiles on mcard.member_id equals m.member_id

                         where (
                         mcard.record_status != (int)CommonConstant.RecordStatus.deleted
                         && mcard.card_no == card_no
                     )
                         select new MemberCardObject
                         {
                             card_id = mcard.card_id,
                             member_id = mcard.member_id,
                             card_no = mcard.card_no,
                             card_version = mcard.card_version,
                             card_type = mcard.card_type,
                             card_status = mcard.card_status,
                             issue_date = mcard.issue_date,
                             old_card_id = mcard.old_card_id,
                             remark = mcard.remark,
                             status = mcard.status,
                             crt_date = mcard.crt_date,
                             crt_by_type = mcard.crt_by_type,
                             crt_by = mcard.crt_by,
                             upd_date = mcard.upd_date,
                             upd_by_type = mcard.upd_by_type,
                             upd_by = mcard.upd_by,
                             record_status = mcard.record_status,

                             // additional
                             member_no = m.member_no

                         });


            resultObj = query.FirstOrDefault();

            if (resultObj != null)
                systemCode = CommonConstant.SystemCode.normal;
            else
                systemCode = CommonConstant.SystemCode.record_invalid;

            return resultObj;
        }

        public MemberCardObject CurrentCard(int member_id, ref CommonConstant.SystemCode systemCode)
        {
            var resultObj = new MemberCardObject();


            var query = (from mcard in db.member_cards
                         join m in db.member_profiles on mcard.member_id equals m.member_id

                         //card type
                         join ml in db.member_levels on mcard.card_type equals ml.level_id

                         //card status
                         join li_cs in db.listing_items on mcard.card_status equals li_cs.value
                         join l_cs in db.listings on li_cs.list_id equals l_cs.list_id

                         where (
                             mcard.record_status != (int)CommonConstant.RecordStatus.deleted
                             && mcard.member_id == member_id
                             && mcard.card_status != (int)CommonConstant.MemberCardStatus.voided
                            && l_cs.code == "MemberCardStatus"
                        )
                         select new MemberCardObject
                         {
                             card_id = mcard.card_id,
                             member_id = mcard.member_id,
                             card_no = mcard.card_no,
                             card_version = mcard.card_version,
                             card_type = mcard.card_type,
                             card_status = mcard.card_status,
                             issue_date = mcard.issue_date,
                             old_card_id = mcard.old_card_id,
                             remark = mcard.remark,
                             status = mcard.status,
                             crt_date = mcard.crt_date,
                             crt_by_type = mcard.crt_by_type,
                             crt_by = mcard.crt_by,
                             upd_date = mcard.upd_date,
                             upd_by_type = mcard.upd_by_type,
                             upd_by = mcard.upd_by,
                             record_status = mcard.record_status,

                             // additional
                             member_no = m.member_no,
                             card_type_name = ml.name,
                             card_status_name = li_cs.name
                         });
            if (query.Count() > 0)
            {
                resultObj = query.FirstOrDefault();
                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultObj = new MemberCardObject();
                systemCode = CommonConstant.SystemCode.record_invalid;
            }

            return resultObj;
        }

        public CommonConstant.SystemCode Update(MemberCardObject dataObject)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                var result = db.sp_UpdateMemberCard(
                     _accessObject.id,
                _accessObject.type, 

                    dataObject.card_id,
                    dataObject.member_id,
                    dataObject.card_no,
                    dataObject.card_version,
                    dataObject.card_type,
                    dataObject.card_status,
                    dataObject.issue_date,
                    dataObject.old_card_id,
                    dataObject.remark,
                    dataObject.status,
                   
                    ref sql_result);

                system_code = (CommonConstant.SystemCode)sql_result.Value;
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }
            //
            return system_code;
        }
    }
}
