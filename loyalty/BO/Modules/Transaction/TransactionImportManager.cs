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
using Palmary.Loyalty.BO.Modules.Service;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.CSV;
using Palmary.Loyalty.BO.DataTransferObjects.Service;
using Palmary.Loyalty.BO.DataTransferObjects.Transaction;

using System.IO;
using CsvHelper;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.DataTransferObjects.Product;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.Modules.Product;

namespace Palmary.Loyalty.BO.Modules.Transaction
{
    public class TransactionImportManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.transactionImport;
        
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;


        // For backend, using BO Session to access
        public TransactionImportManager()
        {
            // access object from session
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public TransactionImportManager(AccessObject accessObject)
        {
            // access object from passing in
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        private CommonConstant.SystemCode Create(
            TransactionImportObject dataObject
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;
    
            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateTransactionImport(
                                    _accessObject.id,
                  _accessObject.type, 
                    dataObject.transaction_type,
                    dataObject.file_name,
                    dataObject.no_of_dataRow,
                    dataObject.no_of_imported,
                    dataObject.no_of_failRecord,
                    dataObject.remark,
                    dataObject.status,
                    
                    ref sql_result
                    );
            
                system_code = (CommonConstant.SystemCode)sql_result.Value;
     
            }else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }

        // Get List with paging, dynamic search, dynamic sorting
        public List<TransactionImportObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<TransactionImportObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from ti in db.transaction_imports
                             
                             join crt_ao in db.v_accessObjects on new { type = ti.crt_by_type, target_id = ti.crt_by } equals new { type = crt_ao.type, target_id = crt_ao.target_id } 

                             join li_tt in db.listing_items on ti.transaction_type equals li_tt.value
                             join l_tt in db.listings on li_tt.list_id equals l_tt.list_id

                             where (
                                ti.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l_tt.code == "TransactionType"
                            )
                             select new TransactionImportObject
                             {
                                 import_id = ti.import_id,
                                 transaction_type = ti.transaction_type,
                                 file_name = ti.file_name,
                                 no_of_dataRow = ti.no_of_dataRow,
                                 no_of_imported = ti.no_of_imported,
                                 no_of_failRecord = ti.no_of_failRecord,
                                 remark = ti.remark,
                                 status = ti.status,
                                 crt_date = ti.crt_date,
                                 crt_by_type = ti.crt_by_type,
                                 crt_by = ti.crt_by,
                                 upd_date = ti.upd_date,
                                 upd_by_type = ti.upd_by_type,
                                 upd_by = ti.upd_by,
                                 record_status = ti.record_status,

                                 // additional info
                                 transaction_type_name = li_tt.name,
                                 crt_by_name = crt_ao.name
                             });

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "crt_date"
                    || sortColumn == "crt_by_name"
                    || sortColumn == "transaction_type_name"
           
                    || sortColumn == "no_of_imported"
           
                    || sortColumn == "remark"
                    )
                    orderByColumn = sortColumn;
                else if (sortColumn == "import_file")
                    orderByColumn = "file_name";
                else if (sortColumn == "no_of_data_row")
                    orderByColumn = "no_of_dataRow";
                else if (sortColumn == "no_of_fail_record")
                    orderByColumn = "no_of_failRecord";
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
                resultList = new List<TransactionImportObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }
            return resultList;
        }

        public CommonConstant.SystemCode Import(int transactionType, string filePath)
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

            var validColumn = false;

            // Purchase Product
            if (transactionType == (int)CommonConstant.TransactionType.purchase_product)
            {
                // write header
                csvWriter.WriteRecord(new ProductPurchaseImportCSVReportObject.Header());

                // Check Excel Column
                #region Check Excel Column

                if (dataRows == null || dataRows.Count() == 0)
                {
                    if (string.IsNullOrEmpty(import_message))
                        import_message = "Invalid Sheet";
                }
                else
                {
                    var firstRow = dataRows.First();

                    if (firstRow.Table.Columns.Contains("Purchase Date")
                        && firstRow.Table.Columns.Contains("Member Code")
                        && firstRow.Table.Columns.Contains("Product Code")
                        && firstRow.Table.Columns.Contains("Total Amount")
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
                    var csvReportRow = new ProductPurchaseImportCSVReportObject.Row()
                    {
                        excel_row_no = excel_row_no,
                        fail_reason = import_message
                    };
                    csvWriter.WriteRecord(csvReportRow);
                }
                #endregion

                if (validColumn)
                {
                    var productPurchaseManager = new ProductPurchaseManager(_accessObject);

                    foreach (var x in dataRows)
                    {
                        excel_row_no++;
                        var row_error_message = "";

                        var paymentObject = new ProductPurchaseImportCSVReportObject();
                        var csvReportRow = new ProductPurchaseImportCSVReportObject.Row()
                        {
                            excel_row_no = excel_row_no
                        };

                        // Try to get data from Excel data set and process it
                        #region Read parameters
                        var try_get = "";
                        try
                        {
                            DateTime purchaseDate;
                            string memberNo;
                            string productNo;
                            double totalAmount;

                            try_get = "Purchase Date";
                            csvReportRow.purchase_date = (DateTime)x[try_get];
                            purchaseDate = (DateTime)x[try_get];

                            try_get = "Member Code";
                            csvReportRow.member_no = x[try_get].ToString();
                            memberNo = x[try_get].ToString();

                            try_get = "Product Code";
                            csvReportRow.product_no = x[try_get].ToString();
                            productNo = x[try_get].ToString();

                            try_get = "Total Amount";
                            csvReportRow.total_amount = (double)x[try_get];
                            totalAmount = (double)x[try_get];

                            
                            try_get = "";  // pass and clear

                            //var create_remark = "";

                            var systemCode = productPurchaseManager.Purchase(memberNo, productNo, totalAmount, purchaseDate);
                            if (systemCode == CommonConstant.SystemCode.normal)
                                no_of_imported++;
                            else if (systemCode == CommonConstant.SystemCode.err_member_not_exist)
                                row_error_message = "Member not exist";
                            else if (systemCode == CommonConstant.SystemCode.err_product_not_exist)
                                row_error_message = "Product not exist";
                            else
                                row_error_message = "Data invalid";

                        }
                        catch (Exception ex)
                        {
                            if (!string.IsNullOrEmpty(try_get))
                                row_error_message = try_get + ": " + ex.Message;
                            else
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

                // create import record
                var importObj = new TransactionImportObject()
                {
                    transaction_type = transactionType,
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

            else if (transactionType == (int)CommonConstant.TransactionType.postpaidservice)
            {
                // write header
                csvWriter.WriteRecord(new ServicePaymentImportCSVReportObject.Header());

                // Check Excel Column
                #region Check Excel Column

                if (dataRows == null || dataRows.Count() == 0)
                {
                    if (string.IsNullOrEmpty(import_message))
                        import_message = "Invalid Sheet";
                }
                else
                {
                    var firstRow = dataRows.First();

                    if (firstRow.Table.Columns.Contains("Invoice No")
                        && firstRow.Table.Columns.Contains("Member Code")
                        && firstRow.Table.Columns.Contains("Member Service No")
                        && firstRow.Table.Columns.Contains("Service Plan No")
                        && firstRow.Table.Columns.Contains("Service Start Date")
                        && firstRow.Table.Columns.Contains("Service End Date")
                        && firstRow.Table.Columns.Contains("Amount")
                        && firstRow.Table.Columns.Contains("Payment Status")
                        && firstRow.Table.Columns.Contains("Paid Amount")
                        && firstRow.Table.Columns.Contains("Payment Method")
                        && firstRow.Table.Columns.Contains("Payment Date")
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
                    var csvReportRow = new ServicePaymentImportCSVReportObject.Row()
                    {
                        excel_row_no = excel_row_no,
                        fail_reason = import_message
                    };
                    csvWriter.WriteRecord(csvReportRow);
                }
            #endregion

                if (validColumn)
                {
                    var servicePaymentManager = new ServicePaymentManager();

                    foreach (var x in dataRows)
                    {
                        excel_row_no++;
                        var row_error_message = "";

                        var paymentObject = new ServicePaymentObject();
                        var csvReportRow = new ServicePaymentImportCSVReportObject.Row()
                        {
                            excel_row_no = excel_row_no
                        };

                        // Try to get data from Excel data set and process it
                        #region Read parameters
                        var try_get = "";
                        try
                        {
                            var servicePaymentObject = new ServicePaymentObject();

                            try_get = "Invoice No";
                            csvReportRow.invoice_no = x["Invoice No"].ToString();
                            servicePaymentObject.invoice_no = x["Invoice No"].ToString();
                            
                            try_get = "Member Code";
                            csvReportRow.member_no = x["Member Code"].ToString();
                            servicePaymentObject.member_no = x["Member Code"].ToString();
                            
                            try_get = "Member Service No";
                            csvReportRow.member_service_no = x["Member Service No"].ToString();
                            servicePaymentObject.member_service_no = x["Member Service No"].ToString();

                            try_get = "Service Plan No";
                            csvReportRow.service_plan_no = x["Service Plan No"].ToString();
                            servicePaymentObject.service_plan_no = x["Service Plan No"].ToString();

                            try_get = "Service Start Date";
                            if (string.IsNullOrEmpty(x["Service Start Date"].ToString()))
                                servicePaymentObject.service_start_date = null;
                            else
                                servicePaymentObject.service_start_date = (DateTime)x["Service Start Date"];

                            try_get = "Service End Date";
                            if (string.IsNullOrEmpty(x["Service End Date"].ToString()))
                                servicePaymentObject.service_end_date = null;
                            else
                                servicePaymentObject.service_end_date = (DateTime)x["Service End Date"];
                            
                            try_get = "Amount";
                            servicePaymentObject.amount = (double)x["Amount"];
                            try_get = "Payment Status";
                            servicePaymentObject.status = int.Parse(x["Payment Status"].ToString());
                            try_get = "Paid Amount";
                            servicePaymentObject.paid_amount = (double)x["Paid Amount"];
                            try_get = "Payment Method";
                            servicePaymentObject.payment_method = int.Parse(x["Payment Method"].ToString());
                            try_get = "Payment Date";
                            servicePaymentObject.payment_date = (DateTime)x["Payment Date"];
                            
                            try_get = "";  // pass and clear

                            var create_remark = "";

                            var systemCode = servicePaymentManager.MakePayment(transactionType, servicePaymentObject, ref create_remark);
                            if (systemCode == CommonConstant.SystemCode.normal)
                                no_of_imported++;
                            else
                                row_error_message = create_remark;
                        }
                        catch (Exception ex)
                        {
                            if (!string.IsNullOrEmpty(try_get))
                                row_error_message = try_get + ": "+ ex.Message;
                            else
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

                // create import record
                var importObj = new TransactionImportObject()
                {
                    transaction_type = transactionType,
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
            
            // Process Point Adjustment Upload File -------------------
        
            else if (transactionType == (int)CommonConstant.TransactionType.point_adjustment)
            {
                // write header
                csvWriter.WriteRecord(new ServicePaymentImportCSVReportObject.Header());

                // Check Excel Column
                #region Check Excel Column
                if (dataRows == null || dataRows.Count() == 0)
                {
                    if (string.IsNullOrEmpty(import_message))
                        import_message = "Invalid Sheet";
                }
                else
                {
                    var firstRow = dataRows.First();

                    if (firstRow.Table.Columns.Contains("Member Code")
                        && firstRow.Table.Columns.Contains("Adjust Point")
                        && firstRow.Table.Columns.Contains("Adjust Reason")
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
                    var csvReportRow = new ServicePaymentImportCSVReportObject.Row()
                    {
                        excel_row_no = excel_row_no,
                        fail_reason = import_message
                    };

                    csvWriter.WriteRecord(csvReportRow);
                }
            #endregion

                if (validColumn)
                {
                    var pointAdjustManager = new PointAdjustManager();

                    foreach (var x in dataRows)
                    {
                        excel_row_no++;
                        var row_error_message = "";

                        var paymentObject = new ServicePaymentObject();
                        var csvReportRow = new PointAdjustmentImportCSVReportObject.Row()
                        {
                            excel_row_no = excel_row_no
                        };

                        // Try to get data from Excel data set
                        #region Read parameters
                        var try_get = "";
                        try
                        {
                            var servicePaymentObject = new ServicePaymentObject();

                            try_get = "Member Code";
                            csvReportRow.member_no = x["Member Code"].ToString();

                            try_get = "Adjust Point";
                            csvReportRow.adjust_point = (double)x["Adjust Point"];

                            try_get = "Adjust Reason";
                            csvReportRow.adjust_reason = x["Adjust Reason"].ToString();

                            MemberObject resultObject;

                            // TODO:: Add Row Data Format Validation

                            var query = (from m in db.member_profiles
                                         join so in db.system_objects on m.member_id equals so.object_id
                                         where (
                                            m.record_status != (int)CommonConstant.RecordStatus.deleted
                                            && m.member_no == csvReportRow.member_no
                                        )
                                         select new MemberObject
                                         {
                                             member_id = m.member_id,
                                             member_no = m.member_no,
                                             password = m.password,
                                             email = m.email,
                                             fbid = m.fbid,
                                             fbemail = m.fbemail,
                                             mobile_no = m.mobile_no,
                                             salutation = m.salutation,
                                             firstname = m.firstname,
                                             middlename = m.middlename,
                                             lastname = m.lastname,
                                             fullname = m.fullname, 
                                             birth_year = m.birth_year,
                                             birth_month = m.birth_month,
                                             birth_day = m.birth_day,
                                             gender = m.gender,
                                             hkid = m.hkid,
                                             address1 = m.address1,
                                             address2 = m.address2,
                                             address3 = m.address3,
                                             district = m.district,
                                             region = m.region,
                                             reg_source = m.reg_source,
                                             referrer = m.referrer,
                                             reg_status = m.reg_status,
                                             reg_ip = m.reg_ip,
                                             activate_key = m.activate_key,
                                             hash_key = m.hash_key,
                                             session = m.session,
                                             status = m.status,
                                             opt_in = m.opt_in,
                                             member_level_id = m.member_level_id,
                                             member_category_id = m.member_category_id,
                                             available_point = m.available_point,
                                             crt_date = m.crt_date,
                                             upd_date = m.upd_date,
                                             crt_by_type = m.crt_by_type,
                                             crt_by = m.crt_by,
                                             upd_by_type = m.upd_by_type,
                                             upd_by = m.upd_by,
                                             record_status = m.record_status
                                         });

                            resultObject = query.FirstOrDefault();

                            try_get = "";  // pass and clear

                            var create_remark = "";

                            var systemCode = pointAdjustManager.Adjust(0, resultObject.member_id, csvReportRow.adjust_point, csvReportRow.adjust_reason);

                            if (systemCode == CommonConstant.SystemCode.normal)
                                no_of_imported++;
                            else
                                row_error_message = create_remark;
                        }
                        catch (Exception ex)
                        {
                            if (!string.IsNullOrEmpty(try_get))
                                row_error_message = try_get + ": "+ ex.Message;
                            else
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

                // create import record
                var importObj = new TransactionImportObject()
                {
                    transaction_type = transactionType,
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


            
            return CommonConstant.SystemCode.record_invalid;
        }
    }
}
