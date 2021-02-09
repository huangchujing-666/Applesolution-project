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
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.DataTransferObjects.CSV;

using System.IO;
using CsvHelper;

namespace Palmary.Loyalty.BO.Modules.Member
{
    public class MemberImportManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.memberImport;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        public MemberImportManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        private CommonConstant.SystemCode Create(
             MemberImportObject dataObject
         )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateMemberImport(
                   _accessObject.id,
                _accessObject.type, 

                    dataObject.file_name,
                    dataObject.no_of_dataRow,
                    dataObject.no_of_imported,
                    dataObject.no_of_failRecord,
                    dataObject.remark,
                    dataObject.status,

                    ref sql_result
                    );

                system_code = (CommonConstant.SystemCode)sql_result.Value;
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }

        public List<MemberImportObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<MemberImportObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from mi in db.member_imports
                             join ao in db.v_accessObjects on mi.crt_by equals ao.target_id

                             where (
                                mi.record_status != (int)CommonConstant.RecordStatus.deleted
                                && ao.type == CommonConstant.ObjectType.user
                            )
                             select new MemberImportObject
                             {
                                 import_id = mi.import_id,
                                 file_name = mi.file_name,
                                 no_of_dataRow = mi.no_of_dataRow,
                                 no_of_imported = mi.no_of_imported,
                                 no_of_failRecord = mi.no_of_failRecord,
                                 remark = mi.remark,
                                 status = mi.status,
                                 crt_date = mi.crt_date,
                                 crt_by_type = mi.crt_by_type,
                                 crt_by = mi.crt_by,
                                 upd_date = mi.upd_date,
                                 upd_by_type = mi.upd_by_type,
                                 upd_by = mi.upd_by,
                                 record_status = mi.record_status,

                                 //-- additional info
                                 crt_by_name = ao.name
                             });

                // dynamic sort
                Func<MemberImportObject, Object> orderByFunc = null;
                if (sortColumn == "crt_date")
                    orderByFunc = x => x.crt_date;
                else if (sortColumn == "crt_by_name")
                    orderByFunc = x => x.crt_by_name;
                else if (sortColumn == "import_file")
                    orderByFunc = x => x.file_name;
                else if (sortColumn == "no_of_dataRow")
                    orderByFunc = x => x.no_of_dataRow;
                else if (sortColumn == "no_of_imported")
                    orderByFunc = x => x.no_of_imported;
                else if (sortColumn == "no_of_failRecord")
                    orderByFunc = x => x.no_of_failRecord;
                else if (sortColumn == "remark")
                    orderByFunc = x => x.remark;
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
                resultList = new List<MemberImportObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public CommonConstant.SystemCode Import(string filePath, int importType)
        {
            // Read Excel
            var result = false;
            var import_message = "";

            var excelManager = new ExcelManager();
            var dataRows = excelManager.Read(filePath, "Sheet1", ref result, ref import_message);

            var excel_row_no = 1;
            var no_of_imported = 0;
            var no_of_failed = 0;

            // init csv writer
            var writeDirectory = Path.GetDirectoryName(filePath);
            var writeFileName_noExt = Path.GetFileNameWithoutExtension(filePath);
            var writeFullPath = Path.Combine(writeDirectory, writeFileName_noExt + ".csv");

            var streamWriter = new StreamWriter(writeFullPath, false, Encoding.UTF8);
            var csvWriter = new CsvWriter(streamWriter);

            // write header
            csvWriter.WriteRecord(new MemberImportCSVReportHeaderObject());

            // Check Excel Column
            #region Check Excel Column
            var validColumn = false;
            if (dataRows == null || dataRows.Count() == 0)
            {
                if (string.IsNullOrEmpty(import_message))
                    import_message = "Invalid Sheet";
            }
            else
            {
                var firstRow = dataRows.First();

                if (firstRow.Table.Columns.Contains("Member Code")
                    && firstRow.Table.Columns.Contains("Salutation")
                    && firstRow.Table.Columns.Contains("First Name")
                    && firstRow.Table.Columns.Contains("Middle Name")
                    && firstRow.Table.Columns.Contains("Last Name")
                    && firstRow.Table.Columns.Contains("Email")
                    && firstRow.Table.Columns.Contains("Birthday")
                    && firstRow.Table.Columns.Contains("Gender")
                    && firstRow.Table.Columns.Contains("HKID")
                    && firstRow.Table.Columns.Contains("Mobile no")
                    && firstRow.Table.Columns.Contains("Opt in")
                    && firstRow.Table.Columns.Contains("Referrer Member Code")
                 )
                {
                    validColumn = true;
                }
                else
                {
                    import_message = "Invalid Excel Column";
                }
            }

            if (!String.IsNullOrEmpty(import_message))
            {
                no_of_failed = dataRows.Count() == 0 ? 1 : dataRows.Count();
                var csvReportRow = new MemberImportCSVReportObject()
                {
                    excel_row_no = excel_row_no,
                    fail_reason = import_message
                };

                csvWriter.WriteRecord(csvReportRow);
            }
            #endregion

            if (validColumn)
            {
                var memberManger = new MemberManager();
                
                foreach (var x in dataRows)
                {
                    excel_row_no++;
                    var row_error_message = "";

                    MemberObject member;

                    var csvReportRow = new MemberImportCSVReportObject() 
                    {
                        excel_row_no = excel_row_no
                    };
                
                    // Try to get data from Excel data set
                    #region Read parameters
                    try
                    {
                        var systemCode = CommonConstant.SystemCode.undefine;
                        var member_no = x["Member Code"].ToString();

                        if (importType == (int)CommonConstant.MemberImportType.insert)
                        {
                            member = new MemberObject();
                            member.member_no = member_no;
                        }
                        else
                            member = memberManger.GetDetail_byMemberNo(member_no, ref systemCode);

                        csvReportRow.member_no = member.member_no;
                        
                        var salutation = x["Salutation"].ToString().ToUpper();
                        if (salutation == "MR")
                            member.salutation = 1;
                        else if (salutation == "MISS")
                            member.salutation = 2;
                        else if (salutation == "MRS")
                            member.salutation = 3;
                        else if (salutation == "DR")
                            member.salutation = 4;
                        else if (salutation == "PROF")
                            member.salutation = 5;
                        else
                            member.salutation = 1;

                        member.firstname = x["First Name"].ToString();
                        member.middlename = x["Middle Name"].ToString();
                        member.lastname = x["Last Name"].ToString();
                        member.email = x["Email"].ToString();

                        var birthday = (DateTime)x["Birthday"];
                        member.birth_year = birthday.Year;
                        member.birth_month = birthday.Month;
                        member.birth_day = birthday.Day;

                        var gender = x["Gender"].ToString();

                        if (gender == "M")
                            member.gender = 1;
                        else
                            member.gender = 2;

                        member.hkid = x["HKID"].ToString();
                        member.mobile_no = x["Mobile no"].ToString();
                        member.referrer_member_no = x["Referrer Member Code"].ToString();
                        member.status = CommonConstant.Status.active;

                        var optIn = x["Opt in"].ToString().ToUpper();
                        if (optIn == "T")
                            member.opt_in = 1;
                        else
                            member.opt_in = 0;
                        
                        // default
                        member.reg_ip = SessionManager.Current.obj_ip;
                        member.activate_key = "";
                        member.hash_key = "";
                        member.member_level_id = 1;
                        member.member_category_id = 0;
                        member.member_category_id = 0;

                        // check referrer member no
                        member.referrer = 0;
                        var inputValid = false;
                        if (!String.IsNullOrEmpty(member.referrer_member_no))
                        {
                            var referrer = memberManger.GetDetail_byMemberNo(member.referrer_member_no, ref systemCode);
                            if (referrer.member_id > 0)
                            {
                                inputValid = true;
                                member.referrer = referrer.member_id;
                            }
                            else
                            {
                                inputValid = false;
                                row_error_message = "Referrer Member Code is invalid";
                            }

                        }
                        else
                            inputValid = true;

                        if (inputValid)
                        {
                            var create_remark = "";
                            systemCode = CommonConstant.SystemCode.undefine;
                            var new_member_id = 0;

                            if (importType == (int)CommonConstant.MemberImportType.insert)
                                systemCode = memberManger.Create(member, ref create_remark, ref new_member_id);
                            else if (importType == (int)CommonConstant.MemberImportType.update)
                                systemCode = memberManger.Update_directCore(member);

                            if (systemCode == CommonConstant.SystemCode.normal)
                                no_of_imported++;
                            else
                                row_error_message = create_remark;
                        }
                    }
                    catch (Exception ex)
                    {
                        row_error_message = ex.Message;
                     
                    }
                    #endregion

                    if (!String.IsNullOrEmpty(row_error_message))
                    {
                        if (String.IsNullOrEmpty(import_message)) // cache the first error
                            import_message = row_error_message;

                        csvReportRow.fail_reason = row_error_message;
                        csvWriter.WriteRecord(csvReportRow);
                        no_of_failed++;
                    }
                }
            }

            // release writer
            csvWriter.Dispose();
            
            var remark = "";
            if (!String.IsNullOrEmpty(import_message))
                remark = "One of error: " + import_message;

            // log
            var importObj = new MemberImportObject()
            {
                file_name = Path.GetFileName(filePath),
                no_of_dataRow = dataRows.Count(),
                no_of_imported = no_of_imported,
                no_of_failRecord = no_of_failed,
                remark = remark,
                status = CommonConstant.Status.active
            };

            Create(importObj);

            if (no_of_failed == 0)
                return CommonConstant.SystemCode.normal;
            else
                return CommonConstant.SystemCode.record_invalid;
        }
    }
}