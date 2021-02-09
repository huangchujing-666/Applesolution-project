using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.Modules.Product;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Database;
using System.Data.Objects;

using System.Data.OleDb;
using System.Data;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

namespace Palmary.Loyalty.BO.Modules.Passcode
{
    public class PasscodeImportManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.passcodeImport;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

         // For backend, using BO Session to access
        public PasscodeImportManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }



        public bool Import(int user_id, DataSet excelDataSet, ref string result_message)
        {
            var generate_id = 0;
            var status = 1;
            var data = excelDataSet.Tables["DataList"].AsEnumerable();
            var sql_remark = "";
            var resultFlag = false;

            System.Diagnostics.Debug.WriteLine(data.Count(),"data.Count()");

            if (data.Count() <= 0)
            {
                result_message = "Invalid row numbers";
            }
            else
            {
                // ** Create generate job, return generate_id **/
                PasscodeGenerateManager _passcodeGenerateManager = new PasscodeGenerateManager();
                var addFlag = _passcodeGenerateManager.Create(
                    user_id,
                    data.Count(), //noToGenerate
                    0,
                    ref generate_id,
                    ref sql_remark);

                if (addFlag)
                {
                    ProductManager _productManager = new ProductManager();
                    PasscodeManager _passcodeManager = new PasscodeManager();

                    var generateCompleteCounter = 0;
                    var insertErrorCounter = 0;
                    var excel_row_no = 1; // row 1 for header

                    foreach (var dataRow in data)
                    {
                        excel_row_no++;

                        var pin_value = "";
                        var product_no = "";
                        var row_error_message = "";
                        var current_row_invalid = false;
                        long new_passcode_id = 0;

                        DateTime active_date = default(DateTime);
                        DateTime expiry_date = default(DateTime);

                        var product_id = 0;
                        var vaildParmeters = true;
                        List<string> invaildParmeterList = new List<string>();

                        // Try to get data from Excel data set
                        #region Check and gather parameters
                        try
                        {
                            SystemConfigManager configManager = new SystemConfigManager();
                            var get_sql_remark = "";
                            var point_mode = configManager.GetSystemConfigValue(CommonConstant.ConfigName.point_mode, ref get_sql_remark);

                            if (!dataRow.Table.Columns.Contains("passcode_pin_value"))
                            {
                                vaildParmeters = false;
                                invaildParmeterList.Add("passcode_pin_value");
                            }
                            else
                                pin_value = dataRow["passcode_pin_value"].ToString();

                            if (!dataRow.Table.Columns.Contains("product_no"))
                            {
                                vaildParmeters = false;
                                invaildParmeterList.Add("product_no");
                            }
                            else
                                product_no = dataRow["product_no"].ToString();

                            if (!dataRow.Table.Columns.Contains("active_date"))
                            {
                                vaildParmeters = false;
                                invaildParmeterList.Add("active_date");
                            }
                            else
                                active_date = (DateTime)dataRow["active_date"];

                            var passcode_expiry_month = int.Parse(configManager.GetSystemConfigValue(CommonConstant.ConfigName.passcode_expiry_month, ref get_sql_remark));
                            expiry_date = active_date.AddMonths(passcode_expiry_month);
                        }
                        catch (Exception ex)
                        {
                            current_row_invalid = true;
                            vaildParmeters = false;
                            
                            resultFlag = false;
                            row_error_message = "Fatal error of getting parameters from Excel.";

                            System.Diagnostics.Debug.WriteLine(ex.ToString(), "get data error");
                        }
                        #endregion

                        // import passcode into database
                        if (vaildParmeters)
                        {
                            try
                            {
                                // get product_id
                                var getFlag = _productManager.GetProductID(user_id, product_no, ref product_id);
                                var product_sql_result = false;
                                sp_GetProductDetailByResult theProduct = _productManager.GetProductDetailBy(SessionManager.Current.obj_id, null, product_no, ref product_sql_result);

                                resultFlag = _passcodeManager.Create(
                                    user_id,
                                    pin_value,
                                    generate_id,
                                    0, //passcode_prefix_id, import from excel no need
                                    new DateTime(int.Parse(active_date.ToString("yyyy")), int.Parse(active_date.ToString("MM")), int.Parse(active_date.ToString("dd"))), //active_date,
                                    expiry_date, //new DateTime(int.Parse(expiry_date.ToString("yyyy")), int.Parse(expiry_date.ToString("MM")), int.Parse(expiry_date.ToString("dd"))), //expiry_date,
                                    product_id,
                                    theProduct.point,

                                    status,
                                    null, //void_date,
                                    null, //void_reason,

                                    ref new_passcode_id,
                                    ref sql_remark
                                );

                                generateCompleteCounter++;
                            }
                            catch (Exception ex)
                            {
                                current_row_invalid = true;
                                resultFlag = false;

                                if (ex.ToString().Contains("duplicate key"))
                                    row_error_message = "Please ensure no duplicate key.";
                                else
                                    row_error_message = "Excel Content Error";  //ex.Message
                            }
                        }
                        else
                        {
                            current_row_invalid = true;
                            row_error_message += "InvaildParmeters: " + string.Join(", ", invaildParmeterList.ToArray());
                        }

                        // add invalid row counter
                        if (current_row_invalid)
                            insertErrorCounter++;
                        
                        var generate_status = 0; // Processing

                        if (generateCompleteCounter == data.Count())
                        {
                            generate_status = 1; // Success
                        }
                        else if (insertErrorCounter == data.Count())
                        {
                            generate_status = -1; // Fail
                            result_message = "Fail to import all records. Please read fail report.";
                        }
                        else if ((insertErrorCounter + generateCompleteCounter) == data.Count())
                        {
                            generate_status = 2; // Complete with fail
                            result_message = "Fail to import some records. Please read fail report.";
                        }

                        // **Update Passcode Generate Job**
                        var updateFlag = _passcodeGenerateManager.Update(
                            user_id,
                            generate_id,             // generate_id,
                            null,                    // noToGenerate, no need to update this attribute
                            generateCompleteCounter, // generateCompleteCounter,
                            insertErrorCounter,      // insertErrorCounter,
                            result_message,          // error_messgae,
                            generate_status,         // generate_status,
                        
                        ref sql_remark);
                        
                        System.Diagnostics.Debug.WriteLine(insertErrorCounter, "insertErrorCounter AFTER");
                        System.Diagnostics.Debug.WriteLine(data.Count(), "data.Count()");
                        System.Diagnostics.Debug.WriteLine(generateCompleteCounter, "generateCompleteCounter");

                        if (!resultFlag)
                        {
                            var sql_remark_failImport = "";
                            CreateFailImportRecord(user_id, generate_id, excel_row_no, new_passcode_id, row_error_message, ref sql_remark_failImport);
                        }

                        
                    } // [END] for loop
                }
            }
            return resultFlag;
        }

    public bool CreateFailImportRecord(
        int user_id,

        int generate_id,
        int excel_row_no,
        long passcode_id,
        string error_message,
    
        ref string sql_remark
     )
        {
            int? get_sql_result = 0;
            
            var result = db.sp_CreatePasscode_failImport(
                _accessObject.id, 
                _accessObject.type, 
                generate_id,
                excel_row_no,
                passcode_id,
                error_message,

                ref get_sql_result, ref sql_remark);
    
            return get_sql_result == 1 ? true : false;
        }
    }
}