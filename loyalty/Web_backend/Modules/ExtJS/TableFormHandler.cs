using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;

using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Passcode;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;
using Palmary.Loyalty.BO.DataTransferObjects.Service;
using Palmary.Loyalty.BO.DataTransferObjects.Wifi;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.Modules.Gift;
using Palmary.Loyalty.BO.Modules.Product;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.Modules.Service;
using Palmary.Loyalty.BO.Modules.Wifi;

using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.Administration;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.BO.Modules.Administration.Section;

namespace Palmary.Loyalty.Web_backend.Modules.ExtJS
{
    public static class TableFormHandler
    {
        public static string GetFormByModule(MemberObject member)
        {
            //basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData", //Url.Action("ListData", "Common"),
                post_header = "../Table/GridHeader", //Url.Action("GridHeader", "Common"),
                title = "Member Detail",
                icon = "iconRole16",
                post_params = "../Member/Update", // Url.Action("Update"), //Update action
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            // Add row into the table
            if (member.member_id > 0)
            {
                var rowFieldLabelString_no = new ExtJsFieldLabelInput<string>(PayloadKeys.Member_no, member.member_no)
                {
                    fieldLabel = "Member Code",
                    group = "General",
                    readOnly = true
                };
                extTable.AddFieldLabelToRow(rowFieldLabelString_no);
            }
            else
            {
                var rowFieldLabelString_no = new ExtJsFieldLabelInput<string>(PayloadKeys.Member_no, member.member_no)
                {
                    fieldLabel = "Member Code",
                    group = "General",
                };
                extTable.AddFieldLabelToRow(rowFieldLabelString_no);
            }

            var rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Member_level_name, member.member_level_name)
            {
                fieldLabel = "Member Level",
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldLabelString);

            var rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.Salutation, member.salutation.ToString())
            {
                fieldLabel = "Salutation",
                datasource = "../Table/GetListItems/Salutation", //Url.Action("GetListItems/Salutation", "Table"),
                display_value = member.salutation.ToItemName("Salutation")  //Extend.ToItemName("Salutation", member.salutation)
            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.firstname, member.firstname)
            {
                fieldLabel = "First Name",
            };
            extTable.AddFieldLabelToRow(rowFieldLabelString);

            rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.middlename, member.middlename)
            {
                fieldLabel = "Middle Name",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldLabelString);

            rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.lastname, member.lastname)
            {
                fieldLabel = "Last Name",
            };
            extTable.AddFieldLabelToRow(rowFieldLabelString);

           
            rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Email, member.email)
            {
                fieldLabel = "Email",
                regex = "emailRegex"
            };
            extTable.AddFieldLabelToRow(rowFieldLabelString);

            //if (member.member_id > 0)
            //{
            //    rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.fbid, member.fbid)
            //    {
            //        fieldLabel = "Facebook ID",
            //        allowBlank = true
            //    };
            //    extTable.AddFieldLabelToRow(rowFieldLabelString);

            //    rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.fbemail, member.fbemail)
            //    {
            //        fieldLabel = "Facebook Email",
            //        allowBlank = true
            //    };
            //    extTable.AddFieldLabelToRow(rowFieldLabelString);
            //}

            rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.HKID, member.hkid)
            {
                fieldLabel = "HKID",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldLabelString);

            var birth_year = member.birth_year == 0 ? "" : member.birth_year.ToString();
            var birth_month = member.birth_month == 0 ? "" : member.birth_month.ToString();
            var birth_day = member.birth_day == 0 ? "" : member.birth_day.ToString();

            var birth_display = "";
            if (birth_year == "")
                birth_display = birth_month + "-" + birth_day;
            else if (birth_day == "")
                birth_display = birth_year + "-" + birth_month;
            else
                birth_display = birth_year + "-" + birth_month + "-" + birth_day;

            var rowFieldLabelDate = new ExtJsFieldLabelDateTime_threeInput<int>(PayloadKeys.birth_year, birth_year)
            {
                fieldLabel = "Birthday (yyyy-mm-dd)",
                display_value = birth_display,
                datasource = "../Table/GetListItems/MemberLevel",
                allowBlank = true,
                name2 = "birth_month",
                value2 = birth_month,
                name3 = "birth_day",
                value3 = birth_day
            };
            extTable.AddFieldLabelToRow(rowFieldLabelDate);
            
            rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.Gender, member.gender.ToString())
            {
                fieldLabel = "Gender",
                datasource = "../Table/GetListItems/Gender", //Url.Action("GetListItems/Gender", "Table"),
                display_value = member.gender.ToItemName("Gender"),
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Mobile_no, member.mobile_no)
            {
                fieldLabel = "Mobile No",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldLabelString);

            rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.Opt_in, member.opt_in.ToString())
            {
                fieldLabel = "Receive Newsletter",
                datasource = "../Table/GetListItems/YesNo", //Url.Action("GetListItems/YesNo", "Table"),
                display_value = member.opt_in.ToItemName("YesNo")
            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.referrer_member_no, member.referrer_member_no)
            {
                fieldLabel = "Referrer Member Code",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldLabelString);

            if (member.member_id > 0)
            {
                rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.Reg_source, "Email")//member.reg_source.ToString())
                {
                    fieldLabel = "Registration Source",
                    datasource = "../Table/GetListItems/ActionChannel", //Url.Action("GetListItems/ActionChannel", "Table"),
                    display_value = "Staff",//member.reg_source.ToItemName("ActionChannel"),
                    readOnly = true
                };
                extTable.AddFieldLabelToRow(rowFieldSelect);

                rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.Reg_status, "Confirm")//member.reg_status.ToString())
                {
                    fieldLabel = "Registration Status",
                    datasource = "../Table/GetListItems/Status", //Url.Action("GetListItems/Status", "Table"),
                    display_value = "Confirm",//member.reg_status.ToItemName("Status"),
                    readOnly = true
                };
                extTable.AddFieldLabelToRow(rowFieldSelect);

                rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Reg_ip, "::1")//member.reg_ip)
                {
                    fieldLabel = "Registration IP",
                    readOnly = true
                };
                extTable.AddFieldLabelToRow(rowFieldLabelString);

                rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Reg_date, member.crt_date.ToString("yyyy-MM-dd"))
                {
                    fieldLabel = "Registration Date",
                    readOnly = true
                };
                extTable.AddFieldLabelToRow(rowFieldLabelString);

            }

          

            var member_category_name = "";

            if (member.member_category_id > 0)
            {
                var memberCategoryLangManager = new MemberCategoryLangManager();
                var system_code = CommonConstant.SystemCode.undefine;
                var langList = memberCategoryLangManager.GetMemberCategoryLang_ownedList(member.member_category_id, ref system_code);

                var langObject = langList.Where(x => x.lang_id == (int)CommonConstant.LangCode.en).First();

                member_category_name = langObject.name;
            }
            //rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.member_category_id, member.member_category_id.ToString())
            //{
            //    fieldLabel = "Member Category",
            //    datasource = "../Table/GetListItems/MemberCategory", //Url.Action("GetListItems/Status", "Table"),
            //    display_value = member_category_name,
            //    readOnly = false,
            //    //colspan = 2
            //};
            //extTable.AddFieldLabelToRow(rowFieldSelect);

            rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.Status, member.status.ToString())
            {
                fieldLabel = "Status",
                datasource = "../Table/GetListItems/Status", //Url.Action("GetListItems/Status", "Table"),
                display_value = member.status.ToItemName("Status")
            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            // Address
            rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Address1, member.address1)
            {
                fieldLabel = "Unit",
                group = "Address",
                //colspan = 2,
                datasource = "../Table/GetListItems/MemberLevel",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldLabelString);

            rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Address2, member.address2)
            {
                fieldLabel = "Building",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldLabelString);

            rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Address3, member.address3)
            {
                fieldLabel = "Street",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldLabelString);

            rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.District, member.district.ToString())
            {
                fieldLabel = "District",
                datasource = "../Table/GetListItems/District", //Url.Action("GetListItems/District", "Table"),
                display_value = member.district.ToItemName("District"),
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.Region, member.region.ToString())
            {
                fieldLabel = "Region",
                datasource = "../Table/GetListItems/Region", 
                display_value = member.region.ToItemName("Region"),
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            decimal point_current_realized = 0;
            decimal point_current_unrealized = 0;
            decimal point_expiring_2month = 0;
            decimal point_earned = 0;
            decimal point_used = 0;
            decimal point_expired = 0;

            if (member.member_id > 0)
            {
                var transactionManager = new TransactionManager();
                transactionManager.GetPointSummary(member.member_id,
                                                ref point_current_realized, 
                                                ref point_current_unrealized,
                                                ref point_earned, 
                                                ref point_used, 
                                                ref point_expired,
                                                ref point_expiring_2month);   
            }
          
            // Point Summary
            var rowFieldLabelDouble = new ExtJsFieldLabelInput<double>(PayloadKeys.Member.point_current, point_current_realized.ToString())
            {
                fieldLabel = "Available Point",
                group = "Point Summary",
                readOnly = true
            };
           extTable.AddFieldLabelToRow(rowFieldLabelDouble);

           rowFieldLabelDouble = new ExtJsFieldLabelInput<double>(PayloadKeys.Member.point_current_unrealized, point_current_unrealized.ToString())
           {
               fieldLabel = "Unrealized Point",
               readOnly = true
           };
           extTable.AddFieldLabelToRow(rowFieldLabelDouble);

           rowFieldLabelDouble = new ExtJsFieldLabelInput<double>(PayloadKeys.Member.point_earned, point_earned.ToString())
           {
               fieldLabel = "Point Earned",
               readOnly = true
           };
           extTable.AddFieldLabelToRow(rowFieldLabelDouble);

           rowFieldLabelDouble = new ExtJsFieldLabelInput<double>(PayloadKeys.Member.point_used, point_used.ToString())
           {
               fieldLabel = "Point Used",
               readOnly = true
           };
           extTable.AddFieldLabelToRow(rowFieldLabelDouble);

           rowFieldLabelDouble = new ExtJsFieldLabelInput<double>(PayloadKeys.Member.point_expired, point_expired.ToString())
           {
               fieldLabel = "Point Expired",
               readOnly = true
           };
           extTable.AddFieldLabelToRow(rowFieldLabelDouble);

           rowFieldLabelDouble = new ExtJsFieldLabelInput<double>(PayloadKeys.Member.point_expiring_2month, point_expiring_2month.ToString())
           {
               fieldLabel = "Point Expiring in 2 months",
               readOnly = true
           };
           extTable.AddFieldLabelToRow(rowFieldLabelDouble);

            // hidden table data
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Id, member.member_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            if (member.member_id > 0)
            {
                var hiddenLabelString = new ExtJsFieldLabelHidden<string>(PayloadKeys.Member_no, member.member_no.ToString());
                extTable.AddFieldLabelToHiddenRow(hiddenLabelString);
            }
            return extTable.ToJson();
        }

        // Product Edit Form (ExtJS)
        public static string GetFormByModule(sp_GetProductDetailByResult product, IEnumerable<sp_GetProductLang_ownedListResult> product_lang_list)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = "Product Detail",
                icon = "iconRole16",
                post_params = "../Product/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            ExtJsFieldLabelInput<string> field_label_str;
            ExtJsFieldLabelTextarea<string> field_textarea;

            //add row into the table
            var product_no = product.product_no!=null? product.product_no.ToString():"";

            //if (product.product_id>0)
            //{
            var rowFieldLabel_noDuplicate = new ExtJsFieldLabel_inputNoDuplicate<string>(PayloadKeys.No, product_no)
                {
                    fieldLabel = "Product Code",
                    group = "General",
                    check_path = "../Product/CheckDuplicateProductNo",
                    display_value = product_no
                    //readOnly = true
                };
            extTable.AddFieldLabelToRow(rowFieldLabel_noDuplicate);
            //}


            // category
            var category_nameList = new List<string> { };
            var category_valueList = new List<string> { };

            if (product.product_id > 0)
            {
                var linkManger = new ProductCategoryLinkManager();
                var categoryList = linkManger.GetProductCategory_ownedList(product.product_id);

                foreach (var c in categoryList)
                {
                    category_nameList.Add(c.category_name);
                    category_valueList.Add(c.category_id.ToString());
                }
            }

            var field_multiSelect_intList = new ExtJsFieldLabelMultiSelect<string>(PayloadKeys.Product.category, "")
            {
                fieldLabel = "Catergory",
                datasource = "../Table/GetListItems/productcategory",
                display_value = string.Join(",", category_nameList.ToArray()),
                value = category_valueList.ToArray(),
                readOnly = false
            };
            extTable.AddFieldLabelToRow(field_multiSelect_intList);

            var rowFieldLabelInt = new ExtJsFieldLabelInput<int>(PayloadKeys.Product.Consumption_period, product.consumption_period.ToString())
            {
                fieldLabel = "Consumption period (day)"
            };
            extTable.AddFieldLabelToRow(rowFieldLabelInt);

            rowFieldLabelInt = new ExtJsFieldLabelInput<int>(PayloadKeys.Product.Lost_customer_period, product.lost_customer_period.ToString())
            {
                fieldLabel = "Lost Customer period (day)"
            };
            extTable.AddFieldLabelToRow(rowFieldLabelInt);

            var rowFieldLabelfloat = new ExtJsFieldLabelInput<float>(PayloadKeys.Price, product.price.ToString())
            {
                fieldLabel = "Price (HKD)"
            };
            extTable.AddFieldLabelToRow(rowFieldLabelfloat);

            //var rowFieldLabel_double = new ExtJsFieldLabelInput<double>(PayloadKeys.Point, product.point.ToString())
            //{
            //    fieldLabel = "Points"
            //};
            //extTable.AddFieldLabelToRow(rowFieldLabel_double);

            //var fieldLabelUpload = new ExtJsFieldLabelUpload<string>(PayloadKeys.Photo, "/Storage/Product/oenobiol_sp01.jpg")
            //{
            //    upload_url = "Upload/Common",
            //    fieldLabel = "Photo",
            //    allowBlank = false,
            //    upType = "img"
            //};
            //extTable.AddFieldLabelToRow(fieldLabelUpload);

            var rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.Status, product.status_id.ToString())
            {
                fieldLabel = "Status",
                datasource = "../Table/GetListItems/Status", //Url.Action("GetListItems/Status", "Table"),
                display_value = product.status,
                readOnly = false,
                //colspan = 2
            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            // Lang ====================================
            var languageManager = new LanguageManager();
            var systemCode = CommonConstant.SystemCode.undefine;
            var systemLangList = languageManager.GetListAll(true, ref systemCode);
            
            var resultList = product_lang_list.ToList();

            for (int i = 0; i < systemLangList.Count(); i++)
            {
                PayloadKey<string> name = PayloadKeys.varWithLang("name", systemLangList[i].code);
                PayloadKey<string> description = PayloadKeys.varWithLang("description", systemLangList[i].code);

                var name_value = "";
                var desc_value = "";

                if (product.product_id > 0)
                {
                    var lang_item = resultList.Where(x => x.lang_id == systemLangList[i].lang_id).FirstOrDefault();
                    name_value = lang_item.name;
                    desc_value = lang_item.description;
                }

                field_label_str = new ExtJsFieldLabelInput<string>(name, name_value)
                {
                    fieldLabel = "Name",
                    group = systemLangList[i].name
                };
                extTable.AddFieldLabelToRow(field_label_str);

                field_textarea = new ExtJsFieldLabelTextarea<string>(description, desc_value)
                {
                    fieldLabel = "Description",
                };
                extTable.AddFieldLabelToRow(field_textarea);
            }

            // load images
            var images = new List<PhotoField>();

            if (product.product_id > 0)
            {
                var productPhotoManager = new ProductPhotoManager();
                FileHandler _fileHandler = new FileHandler();
             
                var photoList = productPhotoManager.GetList(product.product_id, ref systemCode);

                images = photoList.Select(
                               p =>
                                   new PhotoField
                                   {
                                       id = p.photo_id.ToString(),
                                       src = _fileHandler.GetImagePath(p.file_name, p.file_extension, (string)CommonConstant.Module.product, (int)CommonConstant.ImageSizeType.thumb),
                                       orderedID = p.display_order.ToString()
                                   }
                           ).ToList();
            }

            var field_mutliUpload = new ExtJsFieldLabelMultiUploadDialog<string>(PayloadKeys.Product.photos, "")
            {
                group = "Product Photo",
                fieldLabel = "field_mutliUpload",
                colspan = 2,
                images = images,
                uploadUrl = PathHandler.GetControllerPath("Upload", "ProductPhoto"), //"../ProductPhoto/Upload" 
                removeUrl = "../ProductPhoto/Delete"
            
            };
            extTable.AddFieldLabelToRow(field_mutliUpload);

            //hidden table data
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Id, product.product_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }

        public static string GetFormByModule(sp_GetProductCategoryDetailByResult productCategory, IEnumerable<sp_GetProductCategoryLang_ownedListResult> productCategory_lang_list)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = "Product Category Detail",
                icon = "iconRole16",
                post_params = "../ProductCategory/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            //add row into the table
            ExtJsFieldLabelInput<string> rowFieldLabelString;

            var parentCat_name = "";

            if (productCategory.parent_id > 0)
            {
                var productCategoryLangManager = new ProductCategoryLangManager();
                var sql_result = false;
                var parentCat = productCategoryLangManager.GetProductCategoryLang_ownedList(productCategory.parent_id, ref sql_result);
                foreach (var theCat in parentCat)
                {
                    if (theCat.lang_id == (int)CommonConstant.LangCode.tc)
                        parentCat_name = theCat.name;
                }
            }

            var field_label_string = new ExtJsFieldLabelInput<string>(PayloadKeys.ProductCategory.parent_id_name, productCategory.parent_id.ToString())
            {
                fieldLabel = "Parent Category",
                value = parentCat_name,
                group = "General",
                readOnly = true
            };
            extTable.AddFieldLabelToRow(field_label_string);

            var photoFileName_withPath = "";
            if (!String.IsNullOrWhiteSpace(productCategory.photo_file_name))
            {
                FileHandler _fileHandler = new FileHandler();
                photoFileName_withPath = _fileHandler.GetImagePath(productCategory.photo_file_name, productCategory.photo_file_extension, (string)CommonConstant.Module.productCategory, (int)CommonConstant.ImageSizeType.thumb);
            }

            var field_upload = new ExtJsFieldLabelUpload<string>(PayloadKeys.ProductCategory.photo_file_name, photoFileName_withPath)
            {
                fieldLabel = "Photo",
                upType = "img",  //img or file
                name = "fileData", //Post name
                upload_url = "../ProductCategoryPhoto/Upload",
                allowBlank = false,
                height = 150    // height for img row, should >= img height, if not set this value, the form will not have enough height to show all other data row
            };
            extTable.AddFieldLabelToRow(field_upload);

            var field_label_int = new ExtJsFieldLabelInput<int>(PayloadKeys.ProductCategory.display_order, productCategory.display_order.ToString())
            {
                fieldLabel = "Display Order"
            };
            extTable.AddFieldLabelToRow(field_label_int);

            var field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.ProductCategory.status, productCategory.status_id.ToString())
            {
                fieldLabel = "Status",
                datasource = "../Table/GetListItems/status",
                display_value = productCategory.status,
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_select);

            // Lang Part =========================================
            var languageManager = new LanguageManager();
            var systemCode = CommonConstant.SystemCode.undefine;
            var theLangList = languageManager.GetListAll(true, ref systemCode);
            var resultList = productCategory_lang_list.ToList();
            for (int i = 0; i < theLangList.Count(); i++)
            {
                PayloadKey<string> name = PayloadKeys.varWithLang("name", theLangList[i].code);
                PayloadKey<string> description = PayloadKeys.varWithLang("description", theLangList[i].code);

                var name_value = "";
                var description_value = "";

                if (productCategory.category_id > 0)
                {
                    var lang_item = resultList.Where(x => x.lang_id == theLangList[i].lang_id).FirstOrDefault();
                    name_value = lang_item.name;
                    description_value = lang_item.description;
                }

                var field_label_str = new ExtJsFieldLabelInput<string>(name, name_value)
                {
                    fieldLabel = "Name",
                    group = theLangList[i].name
                };
                extTable.AddFieldLabelToRow(field_label_str);

                var field_textarea = new ExtJsFieldLabelTextarea<string>(description, description_value)
                {
                    fieldLabel = "Description",
                };
                extTable.AddFieldLabelToRow(field_textarea);

            }

            //hidden table data
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.ProductCategory.category_id, productCategory.category_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            var hiddenLabel_p_int = new ExtJsFieldLabelHidden<int>(PayloadKeys.ProductCategory.parent_id, productCategory.parent_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel_p_int);

            hiddenLabel_p_int = new ExtJsFieldLabelHidden<int>(PayloadKeys.ProductCategory.leaf, productCategory.leaf.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel_p_int);

            return extTable.ToJson();
        }

        public static string GetFormByModule(sp_GetPasscodePrefixDetailByResult passcodePrefix)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = "Passcode Prefix Detail",
                icon = "iconRole16",
                post_params = "../PasscodePrefix/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            //add row into the table
            ExtJsFieldLabelInput<string> rowFieldLabelString;

            rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.prefix_value, passcodePrefix.prefix_value)
            {
                fieldLabel = "Prefix Value",
                group = "General"
            };
            extTable.AddFieldLabelToRow(rowFieldLabelString);

            //var rowFieldLabelInt = new ExtJsFieldLabelInput<int>(PayloadKeys.product_id, passcodePrefix.product_id.ToString())
            //{
            //    fieldLabel = "product_id"
            //};
            //extTable.AddFieldLabelToRow(rowFieldLabelInt);

            var field_multiSelect_intList = new ExtJsFieldLabelMultiSelect<int>(PayloadKeys.Product.product_id, "")
            {
                fieldLabel = "Product",
                datasource = "../Table/GetListItems/product", //Url.Action("GetListItems/Status", "Table"),
                //display_value = "",
                //value = location_valueList.ToArray(),
                readOnly = false
            };
            extTable.AddFieldLabelToRow(field_multiSelect_intList);


            rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.passcode_format, "%%%-####-$$$$")
            {
                fieldLabel = "Passcode Format",
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldLabelString);

            //hidden table data
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Id, passcodePrefix.prefix_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }

        public static string GetFormByModule(sp_GetGiftDetailByResult gift, IEnumerable<sp_GetGiftLangDetailByResult> gift_lang_detail)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = gift.gift_id == 0 ? "Create New Gift" : "Edit Gift",
                icon = "iconRole16",
                post_params = "../Gift/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            //add row into the table
            ExtJsFieldLabelInput<string> field_label_str;
            ExtJsFieldLabelInput<int> field_label_int;
            ExtJsFieldLabelInput<double> field_label_double;
            ExtJsFieldLabelTextarea<string> field_textarea;
            ExtJsField_dateRange<DateTime> field_dateRange;
            ExtJsFieldLabelSelect<int> field_select;
            ExtJsFieldLabelSelect<bool> field_select_bool;
            ExtJsFieldLabelMultiSelect<int> field_multiSelect;

            var rowFieldLabel_noDuplicate = new ExtJsFieldLabel_inputNoDuplicate<string>(PayloadKeys.Gift.gift_no, gift.gift_no)
            {
                fieldLabel = "Gift Code",
                group = "General",
                check_path = "../Gift/CheckDuplicateGiftNo",
                display_value = gift.gift_no,
                value = gift.gift_no
                //readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldLabel_noDuplicate);

            // category
            var category_nameList = new List<string> { };
            var category_valueList = new List<string> { };

            if (gift.gift_id > 0)
            {
                var linkManger = new GiftCategoryLinkManager();
                var categoryList = linkManger.GetGiftCategory_ownedList(gift.gift_id);

                foreach (var c in categoryList)
                {
                    category_nameList.Add(c.name);
                    category_valueList.Add(c.category_id.ToString());
                }
            }

            var field_multiSelect_intList = new ExtJsFieldLabelMultiSelect<string>(PayloadKeys.Product.category, "")
            {
                fieldLabel = "Catergory",
                datasource = "../Table/GetListItems/giftcategory",
                display_value = string.Join(",", category_nameList.ToArray()),
                value = category_valueList.ToArray(),
                readOnly = false
            };
            extTable.AddFieldLabelToRow(field_multiSelect_intList);


            field_label_double = new ExtJsFieldLabelInput<double>(PayloadKeys.Gift.point, gift.point.ToString())
            {
                fieldLabel = "Point",
            };
            extTable.AddFieldLabelToRow(field_label_double);
            
            field_label_int = new ExtJsFieldLabelInput<int>(PayloadKeys.Gift.alert_level, gift.alert_level.ToString())
            {
                fieldLabel = "Alert Level",
            };
            extTable.AddFieldLabelToRow(field_label_int);

            field_label_double = new ExtJsFieldLabelInput<double>(PayloadKeys.Gift.cost, gift.cost.ToString())
            {
                fieldLabel = "Cost",
            };
            extTable.AddFieldLabelToRow(field_label_double);

            // location
            var location_nameList = new List<string> { };
            var location_valueList = new List<string> { };

            if (gift.gift_id > 0)
            {
                GiftLocationManager _giftLocationManager = new GiftLocationManager();
                var locationList = _giftLocationManager.GetGiftLocationLists(SessionManager.Current.obj_id, gift.gift_id, 0, 0, "");

                foreach (var location in locationList)
                {
                    location_nameList.Add(location.name);
                    location_valueList.Add(location.location_id.ToString());
                }
            }

            field_multiSelect_intList = new ExtJsFieldLabelMultiSelect<string>(PayloadKeys.Gift.location, "")
            {
                fieldLabel = "Location",
                datasource = "../Table/GetListItems/location", //Url.Action("GetListItems/Status", "Table"),
                display_value = string.Join(",", location_nameList.ToArray()),
                value = location_valueList.ToArray(),
                readOnly = false
            };
            extTable.AddFieldLabelToRow(field_multiSelect_intList);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.Gift.status, gift.status.ToString())
            {
                fieldLabel = "Status",
                datasource = "../Table/GetListItems/status", //Url.Action("GetListItems/Status", "Table"),
                display_value = gift.status.ToItemName("Status"),
                readOnly = false,
            };
            extTable.AddFieldLabelToRow(field_select);

            // Lang ====================================
            var languageManager = new LanguageManager();
            var systemCode = CommonConstant.SystemCode.undefine;
            var theLangList = languageManager.GetListAll(true, ref systemCode);
            // System.Diagnostics.Debug.WriteLine(gift_lang_detail.Count(), "gift_lang_detail!!!!!!!!!!!!");
            //  gift_lang_detail[1].

            var resultList = gift_lang_detail.ToList();

            for (int i = 0; i < theLangList.Count(); i++)
            {
                PayloadKey<string> name = PayloadKeys.varWithLang("name", theLangList[i].code);
                PayloadKey<string> description = PayloadKeys.varWithLang("description", theLangList[i].code);

                var name_value = "";
                var desc_value = "";

                if (gift.gift_id > 0)
                {
                    var lang_item = resultList.Where(x => x.lang_id == theLangList[i].lang_id).FirstOrDefault();
                    name_value = lang_item.name;
                    desc_value = lang_item.description;
                }

                field_label_str = new ExtJsFieldLabelInput<string>(name, name_value)
                {
                    fieldLabel = "Name",
                    group = theLangList[i].name
                };
                extTable.AddFieldLabelToRow(field_label_str);

                field_textarea = new ExtJsFieldLabelTextarea<string>(description, desc_value)
                {
                    fieldLabel = "Description",
                };
                extTable.AddFieldLabelToRow(field_textarea);
            }

            // Group: Display ====================================
            // GiftMemberPrivilege
            var memberPrivilege_nameList = new List<string> { };
            var memberPrivilege_valueList = new List<string> { };

            if (gift.gift_id > 0)
            {
                GiftMemberPrivilegeManager _giftMemberPrivilegeManager = new GiftMemberPrivilegeManager();
                var privilegeList = _giftMemberPrivilegeManager.GetGiftMemberPrivilege_ownedList(gift.gift_id);

                foreach (var privilege in privilegeList)
                {
                    memberPrivilege_nameList.Add(privilege.member_level_name);
                    memberPrivilege_valueList.Add(privilege.member_level_id.ToString());
                }
            }

            field_multiSelect_intList = new ExtJsFieldLabelMultiSelect<string>(PayloadKeys.Gift.member_privilege, "")
            {
                fieldLabel = "Member Privilege",
                datasource = "../Table/GetListItems/memberlevel",
                display_value = string.Join(",", memberPrivilege_nameList.ToArray()),
                value = memberPrivilege_valueList.ToArray(),
                readOnly = false,
                group = "Display and Redeem"
            };
            extTable.AddFieldLabelToRow(field_multiSelect_intList);

            var display_public = gift.display_public ? 1 : 0;
            field_select_bool = new ExtJsFieldLabelSelect<bool>(PayloadKeys.Gift.display_public, display_public.ToString())
            {
                fieldLabel = "Display to Public",
                datasource = "../Table/GetListItems/YesNo", //Url.Action("GetListItems/Status", "Table"),
                display_value = display_public.ToItemName("YesNo"),
                readOnly = false
            };
            extTable.AddFieldLabelToRow(field_select_bool);

            //if (gift.gift_id == 0)
            //{
            //    gift.display_active_date = DateTime.Today;
            //    gift.display_expiry_date = DateTime.Today;
            //    gift.redeem_active_date = DateTime.Today;
            //    gift.redeem_expiry_date = DateTime.Today;
            //    gift.discount_active_date = DateTime.Today;
            //    gift.discount_expiry_date = DateTime.Today;
            //    gift.hot_item_active_date = DateTime.Today;
            //    gift.hot_item_expiry_date = DateTime.Today;
            //}

            //var field_dateTimeRange = new ExtJsField_dateTimeRange<DateTime>(PayloadKeys.Gift.display_date_range, gift.display_active_date.ToString("yyyy-MM-dd HH:mm:ss"))
            //{
            //    fieldLabel = "Display Date Range",
            //    toValue = gift.display_expiry_date.ToString("yyyy-MM-dd HH:mm:ss"),
            //    allowBlank = false
            //};
            //extTable.AddFieldLabelToRow(field_dateTimeRange);

            var field_dateTime = new ExtJsField_dateTime_noSec<DateTime>(PayloadKeys.Gift.display_date_range_from, gift.display_active_date)
            {
                fieldLabel = "Display Date From",
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_dateTime);

            field_dateTime = new ExtJsField_dateTime_noSec<DateTime>(PayloadKeys.Gift.display_date_range_to, gift.display_expiry_date)
            {
                fieldLabel = "Display Date To",
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_dateTime);

            //field_dateTimeRange = new ExtJsField_dateTimeRange<DateTime>(PayloadKeys.Gift.redeem_date_range, gift.redeem_active_date.ToString("yyyy-MM-dd HH:mm:ss"))
            //{
            //    fieldLabel = "Redeem Date Range",
            //    toValue = gift.redeem_expiry_date.ToString("yyyy-MM-dd HH:mm:ss"),
            //    allowBlank = false
            //};
            //extTable.AddFieldLabelToRow(field_dateTimeRange);

            field_dateTime = new ExtJsField_dateTime_noSec<DateTime>(PayloadKeys.Gift.redeem_date_range_from, gift.redeem_active_date)
            {
                fieldLabel = "Redeem Date From",
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_dateTime);

            field_dateTime = new ExtJsField_dateTime_noSec<DateTime>(PayloadKeys.Gift.redeem_date_range_to, gift.redeem_expiry_date)
            {
                fieldLabel = "Redeem Date To",                
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_dateTime);

            // Discount ====================================
            int discount = gift.discount ? 1 : 0;
            field_select_bool = new ExtJsFieldLabelSelect<bool>(PayloadKeys.Gift.discount, discount.ToString())
            {
                fieldLabel = "Discount",
                datasource = "../Table/GetListItems/YesNo", //Url.Action("GetListItems/Status", "Table"),
                display_value = discount.ToItemName("YesNo"),
                readOnly = false,
                group = "Discount"
            };
            extTable.AddFieldLabelToRow(field_select_bool);

            field_label_double = new ExtJsFieldLabelInput<double>(PayloadKeys.Gift.discount_point, gift.discount_point.ToString())
            {
                fieldLabel = "Discount Point",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_label_double);

            var discount_active_date = gift.discount_active_date != null ? gift.discount_active_date.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
            var discount_expiry_date = gift.discount_expiry_date != null ? gift.discount_expiry_date.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
            //var field_dateTimeRange = new ExtJsField_dateTimeRange<DateTime>(PayloadKeys.Gift.discount_date_range, discount_active_date)
            //{
            //    fieldLabel = "Discount Date Range",
            //    toValue = discount_expiry_date,
            //    allowBlank = true
            //};
            //extTable.AddFieldLabelToRow(field_dateTimeRange);

            field_dateTime = new ExtJsField_dateTime_noSec<DateTime>(PayloadKeys.Gift.discount_date_range_from, gift.discount_active_date)
            {
                fieldLabel = "Discount Date From",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_dateTime);

            field_dateTime = new ExtJsField_dateTime_noSec<DateTime>(PayloadKeys.Gift.discount_date_range_to, gift.discount_expiry_date)
            {
                fieldLabel = "Discount Date To",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_dateTime);


            //var field_dateTime = new ExtJsField_dateTime<DateTime>(PayloadKeys.Gift.discount_date_range, discount_active_date)
            //{
            //    fieldLabel = "Discount Date From",
            //   // toValue = discount_expiry_date,
            //   // allowBlank = true
            //};
            //extTable.AddFieldLabelToRow(field_dateTime);


            // Hot Item ====================================
            var hot_item = gift.hot_item ? 1 : 0;
            field_select_bool = new ExtJsFieldLabelSelect<bool>(PayloadKeys.Gift.hotItem, hot_item.ToString())
            {
                fieldLabel = "Hot Item",
                datasource = "../Table/GetListItems/YesNo", //Url.Action("GetListItems/Status", "Table"),
                display_value = hot_item.ToItemName("YesNo"),
                readOnly = false,
                group = "Hot Item"
            };
            extTable.AddFieldLabelToRow(field_select_bool);

            //var hot_item_active_date = gift.discount_active_date != null ? gift.hot_item_active_date.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
            //var hot_item_expiry_date = gift.discount_expiry_date != null ? gift.hot_item_expiry_date.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";

            //var field_dateTimeRange = new ExtJsField_dateTimeRange<DateTime>(PayloadKeys.Gift.hotItem_date_range, hot_item_active_date)
            //{
            //    fieldLabel = "Hot Item Date Range",
            //    toValue = hot_item_expiry_date,
            //    allowBlank = true
            //};
            //extTable.AddFieldLabelToRow(field_dateTimeRange);

            field_dateTime = new ExtJsField_dateTime_noSec<DateTime>(PayloadKeys.Gift.hotItem_date_range_from, gift.hot_item_active_date)
            {
                fieldLabel = "Hot Item Date From",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_dateTime);

            field_dateTime = new ExtJsField_dateTime_noSec<DateTime>(PayloadKeys.Gift.hotItem_date_range_to, gift.hot_item_expiry_date)
            {
                fieldLabel = "Hot Item Date To",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_dateTime);

            // load images
            var images = new List<PhotoField>();

            if (gift.gift_id > 0)
            {
                var giftPhotoManager = new GiftPhotoManager();
                FileHandler _fileHandler = new FileHandler();
                var sql_result = false;
                var giftPhoto = giftPhotoManager.GetGiftPhotoListBy(SessionManager.Current.obj_id, gift.gift_id, ref sql_result);

                images = giftPhoto.Select(
                               p =>
                                   new PhotoField
                                   {
                                       id = p.gift_photo_id.ToString(),
                                       src = _fileHandler.GetImagePath(p.file_name, p.file_extension, (string)CommonConstant.Module.gift, (int)CommonConstant.ImageSizeType.thumb),
                                       orderedID = p.display_order.ToString()
                                   }
                           ).ToList();
            }

            var field_mutliUpload = new ExtJsFieldLabelMultiUploadDialog<string>(PayloadKeys.Gift.photos, "")
            {
                group = "Gift Photo",
                fieldLabel = "field_mutliUpload",
                colspan = 2,
                images = images,
                uploadUrl = "../GiftPhoto/Upload",
                removeUrl = "../GiftPhoto/Delete"
            };
            extTable.AddFieldLabelToRow(field_mutliUpload);

            int current_stock = 0;
            int redeem_count = 0;

            if (gift.gift_id > 0)
            {
                var giftInventoryManager = new GiftInventoryManager();
                giftInventoryManager.GetGiftStockSummery(gift.gift_id, ref current_stock, ref redeem_count);
            }

            // stock summary
            //field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Gift.current_stock, current_stock.ToString())
            //{
            //    fieldLabel = "Current Stock",
            //    group = "Stcok Summary",
            //    readOnly = true
            //};
            //extTable.AddFieldLabelToRow(field_label_str);

            //field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Gift.redeem_count, redeem_count.ToString())
            //{
            //    fieldLabel = "Redemption Count",
            //    readOnly = true
            //};
            //extTable.AddFieldLabelToRow(field_label_str);

            //hidden table data
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Id, gift.gift_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);
            var hiddenLabel_int = new ExtJsFieldLabelHidden<int>(PayloadKeys.Gift.gift_id, gift.gift_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel_int);

            //hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Gift.hotItem_display_order, hot_item_display_order.ToString())
            //{
            //    fieldLabel = "Hot Item Display Order",
            //};
            //extTable.AddFieldLabelToRow(hiddenLabel);

            return extTable.ToJson();
        }

        public static string GetFormByModule(sp_GetLocationDetailResult location, IEnumerable<sp_GetLocationLangDetailResult> location_lang_detail)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = location.location_id == 0 ? "Create New Location" : "Edit Location",
                icon = "iconRole16",
                post_params = "../Location/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            //add row into the table
            ExtJsFieldLabelInput<string> field_label_str;
            ExtJsFieldLabelInput<int> field_label_int;
            ExtJsFieldLabelInput<double> field_label_double;
            ExtJsFieldLabelTextarea<string> field_textarea;
            ExtJsField_dateRange<DateTime> field_dateRange;
            ExtJsFieldLabelSelect<int> field_select;
            ExtJsFieldLabelSelect<bool> field_select_bool;
            ExtJsFieldLabelUpload<string> field_uplaod;

            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Location.location_no, location.location_no)
            {
                fieldLabel = "Location No",
                group = "General"
            };
            extTable.AddFieldLabelToRow(field_label_str);

            field_label_double = new ExtJsFieldLabelInput<double>(PayloadKeys.Location.latitude, location.latitude.ToString())
            {
                fieldLabel = "Latitude"
            };
            extTable.AddFieldLabelToRow(field_label_double);

            field_label_double = new ExtJsFieldLabelInput<double>(PayloadKeys.Location.longitude, location.longitude.ToString())
            {
                fieldLabel = "Longitude"
            };
            extTable.AddFieldLabelToRow(field_label_double);

            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Location.phone, location.phone)
            {
                fieldLabel = "Phone"
            };
            extTable.AddFieldLabelToRow(field_label_str);

            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Location.fax, location.fax)
            {
                fieldLabel = "Fax"
            };
            extTable.AddFieldLabelToRow(field_label_str);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.Location.address_district, location.address_district.ToString())
            {
                fieldLabel = "District",
                datasource = "../Table/GetListItems/District", //Url.Action("GetListItems/District", "Table"),
                display_value = location.address_district.ToItemName("District"),
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_select);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.Location.address_region, location.address_region.ToString())
            {
                fieldLabel = "Region",
                datasource = "../Table/GetListItems/Region",
                display_value = location.address_region.ToItemName("Region"),
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_select);

            field_label_int = new ExtJsFieldLabelInput<int>(PayloadKeys.Location.display_order, location.display_order.ToString())
            {
                fieldLabel = "Display Order"
            };
            extTable.AddFieldLabelToRow(field_label_int);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.Location.status, location.status.ToString())
            {
                fieldLabel = "Status",
                datasource = "../Table/GetListItems/status",
                display_value = location.status.ToItemName("Status"),
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_select);

            var photoFileName_withPath = "";
            if (!String.IsNullOrWhiteSpace(location.photo_file_name))
            {
                FileHandler _fileHandler = new FileHandler();
                photoFileName_withPath = _fileHandler.GetImagePath(location.photo_file_name, location.photo_file_extension, (string)CommonConstant.Module.location, (int)CommonConstant.ImageSizeType.thumb);
            }
            field_uplaod = new ExtJsFieldLabelUpload<string>(PayloadKeys.Location.photo_file_name, photoFileName_withPath)
            {
                fieldLabel = "Photo",
                upType = "img",  //img or file
                name = "fileData", //Post name
                //value = photoFileName_withPath, //location.photo_file_name + location.photo_file_extension,
                upload_url = "../LocationPhoto/Upload",
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_uplaod);

            // Lang Part =========================================
            var languageManager = new LanguageManager();
            var systemCode = CommonConstant.SystemCode.undefine;
            var theLangList = languageManager.GetListAll(true, ref systemCode);
            
            var resultList = location_lang_detail.ToList();
            for (int i = 0; i < theLangList.Count(); i++)
            {
                 PayloadKey<string> name = PayloadKeys.varWithLang("name", theLangList[i].code);
                 PayloadKey<string> description = PayloadKeys.varWithLang("description", theLangList[i].code);
                 PayloadKey<string> operation_info = PayloadKeys.varWithLang("operation_info", theLangList[i].code);
                 PayloadKey<string> address_unit = PayloadKeys.varWithLang("address_unit", theLangList[i].code);
                 PayloadKey<string> address_building = PayloadKeys.varWithLang("address_building", theLangList[i].code);
                 PayloadKey<string> address_street = PayloadKeys.varWithLang("address_street", theLangList[i].code);

                 var name_value = "";
                 var description_value = "";
                 var operation_info_value = "";
                 var address_unit_value = "";
                 var address_building_value = "";
                 var address_street_value = "";

                 if (location.location_id > 0)
                 {
                     var lang_item = resultList.Where(x => x.lang_id == theLangList[i].lang_id).FirstOrDefault();
                     name_value = lang_item.name;
                     description_value = lang_item.description;
                     operation_info_value = lang_item.operation_info;
                     address_unit_value = lang_item.address_unit;
                     address_building_value = lang_item.address_building;
                     address_street_value = lang_item.address_street;
                 }

                 field_label_str = new ExtJsFieldLabelInput<string>(name, name_value)
                 {
                     fieldLabel = "Name",
                     group = theLangList[i].name
                 };
                 extTable.AddFieldLabelToRow(field_label_str);

                 field_textarea = new ExtJsFieldLabelTextarea<string>(description, description_value)
                 {
                     fieldLabel = "Description",
                 };
                 extTable.AddFieldLabelToRow(field_textarea);

                 field_label_str = new ExtJsFieldLabelInput<string>(operation_info, operation_info_value)
                 {
                     fieldLabel = "operation_info",
                   
                 };
                 extTable.AddFieldLabelToRow(field_label_str);

                 field_label_str = new ExtJsFieldLabelInput<string>(address_unit, address_unit_value)
                 {
                     fieldLabel = "address_unit",
                    
                 };
                 extTable.AddFieldLabelToRow(field_label_str);

                 field_label_str = new ExtJsFieldLabelInput<string>(address_building, address_building_value)
                 {
                     fieldLabel = "address_building",
                     
                 };
                 extTable.AddFieldLabelToRow(field_label_str);

                 field_label_str = new ExtJsFieldLabelInput<string>(address_street, address_street_value)
                 {
                     fieldLabel = "address_street",
                    
                 };
                 extTable.AddFieldLabelToRow(field_label_str);
            }

            // hidden table data =================================
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Id, location.location_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            if (location.location_id > 0)
            {
                var hiddenLabel_int = new ExtJsFieldLabelHidden<int>(PayloadKeys.Location.location_id, location.location_id.ToString());
                extTable.AddFieldLabelToHiddenRow(hiddenLabel_int);
            }

            return extTable.ToJson();
        }

        public static string GetFormByModule(sp_GetGiftCategoryDetailResult giftCategory, IEnumerable<sp_GetGiftCategoryLangDetailResult> giftCategory_lang_detail)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = giftCategory.category_id == 0 ? "Create New Category" : "Edit Category",
                icon = "iconRole16",
                post_params = "../GiftCategory/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            //add row into the table
            ExtJsFieldLabelInput<string> field_label_str;
            ExtJsFieldLabelInput<int> field_label_int;
            ExtJsFieldLabelInput<double> field_label_double;
            ExtJsFieldLabelTextarea<string> field_textarea;
            ExtJsField_dateRange<DateTime> field_dateRange;
            ExtJsFieldLabelSelect<int> field_select;
            ExtJsFieldLabelSelect<bool> field_select_bool;
            ExtJsFieldLabelUpload<string> field_upload;

            var parentCat_name = "";

            if (giftCategory.parent_id > 0)
            {
                var giftCategoryLangManager = new GiftCategoryLangManager();
                var sql_result = false;
                var parentCat = giftCategoryLangManager.GetGiftCategoryLangDetail(SessionManager.Current.obj_id, giftCategory.parent_id, ref sql_result);
                foreach (var theCat in parentCat)
                {
                    if (theCat.lang_id == (int)CommonConstant.LangCode.tc)
                        parentCat_name = theCat.name;
                }
            }

            var field_label_string = new ExtJsFieldLabelInput<string>(PayloadKeys.GiftCategory.parent_id_name, giftCategory.parent_id.ToString())
            {
                fieldLabel = "Parent Category",
                value = parentCat_name,
                group = "General",
                readOnly = true
            };
            extTable.AddFieldLabelToRow(field_label_string);

            var photoFileName_withPath = "";
            if (!String.IsNullOrWhiteSpace(giftCategory.photo_file_name))
            {
                FileHandler _fileHandler = new FileHandler();
                photoFileName_withPath = _fileHandler.GetImagePath(giftCategory.photo_file_name, giftCategory.photo_file_extension, (string)CommonConstant.Module.giftCategory, (int)CommonConstant.ImageSizeType.thumb);
            }
            field_upload = new ExtJsFieldLabelUpload<string>(PayloadKeys.GiftCategory.photo_file_name, photoFileName_withPath)
            {
                fieldLabel = "Photo",
                upType = "img",  //img or file
                name = "fileData", //Post name
                upload_url = "../GiftCategoryPhoto/Upload",
                allowBlank = false,
            };
            extTable.AddFieldLabelToRow(field_upload);

            field_label_int = new ExtJsFieldLabelInput<int>(PayloadKeys.GiftCategory.display_order, giftCategory.display_order.ToString())
            {
                fieldLabel = "Display Order"
            };
            extTable.AddFieldLabelToRow(field_label_int);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.GiftCategory.status, giftCategory.status.ToString())
            {
                fieldLabel = "Status",
                datasource = "../Table/GetListItems/status",
                display_value = giftCategory.status.ToItemName("Status"),
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_select);

            // Lang Part =========================================
            var languageManager = new LanguageManager();
            var systemCode = CommonConstant.SystemCode.undefine;
            var theLangList = languageManager.GetListAll(true, ref systemCode);
            var resultList = giftCategory_lang_detail.ToList();
            for (int i = 0; i < theLangList.Count(); i++)
            {
                PayloadKey<string> name = PayloadKeys.varWithLang("name", theLangList[i].code);
                PayloadKey<string> description = PayloadKeys.varWithLang("description", theLangList[i].code);
               
                var name_value = "";
                var description_value = "";

                if (giftCategory.category_id > 0)
                {
                    var lang_item = resultList.Where(x => x.lang_id == theLangList[i].lang_id).FirstOrDefault();
                    name_value = lang_item.name;
                    description_value = lang_item.description;
                }

                field_label_str = new ExtJsFieldLabelInput<string>(name, name_value)
                {
                    fieldLabel = "Name",
                    group = theLangList[i].name
                };
                extTable.AddFieldLabelToRow(field_label_str);

                field_textarea = new ExtJsFieldLabelTextarea<string>(description, description_value)
                {
                    fieldLabel = "Description",
                };
                extTable.AddFieldLabelToRow(field_textarea);
            }

            // hidden table data =================================
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Id, giftCategory.category_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            if (giftCategory.category_id > 0)
            {
                var hiddenLabel_int = new ExtJsFieldLabelHidden<int>(PayloadKeys.GiftCategory.category_id, giftCategory.category_id.ToString());
                extTable.AddFieldLabelToHiddenRow(hiddenLabel_int);
            }

            var hiddenLabel_p_int = new ExtJsFieldLabelHidden<int>(PayloadKeys.GiftCategory.parent_id, giftCategory.parent_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel_p_int);

            hiddenLabel_p_int = new ExtJsFieldLabelHidden<int>(PayloadKeys.GiftCategory.leaf, giftCategory.leaf.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel_p_int);

            return extTable.ToJson();
        }


        public static string GetFormByModule(MemberCategoryObject memberCategory)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = memberCategory.category_id == 0 ? "Create New Category" : "Edit Category",
                icon = "iconRole16",
                post_params = "../MemberCategory/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            //add row into the table
            ExtJsFieldLabelInput<string> field_label_str;
            ExtJsFieldLabelInput<int> field_label_int;
            ExtJsFieldLabelInput<double> field_label_double;
            ExtJsFieldLabelTextarea<string> field_textarea;
            ExtJsField_dateRange<DateTime> field_dateRange;
            ExtJsFieldLabelSelect<int> field_select;
            ExtJsFieldLabelSelect<bool> field_select_bool;
            ExtJsFieldLabelUpload<string> field_upload;

            var parentCat_name = "";

            if (memberCategory.parent_id > 0)
            {
                var memberCategoryLangManaer = new MemberCategoryLangManager();
                
                
                var system_code = CommonConstant.SystemCode.undefine;
                var parent_cat_lang_list = memberCategoryLangManaer.GetMemberCategoryLang_ownedList(memberCategory.parent_id, ref system_code);

                foreach (var l in parent_cat_lang_list)
                {
                    if (l.lang_id == (int)CommonConstant.LangCode.en)
                    {
                        parentCat_name = l.name;
                    }
                }
            }

            var field_label_string = new ExtJsFieldLabelInput<string>(PayloadKeys.MemberCategory.parent_id_name, memberCategory.parent_id.ToString())
            {
                fieldLabel = "Parent Category",
                value = parentCat_name,
                group = "General",
                readOnly = true
            };
            extTable.AddFieldLabelToRow(field_label_string);

            field_label_int = new ExtJsFieldLabelInput<int>(PayloadKeys.MemberCategory.display_order, memberCategory.display_order.ToString())
            {
                fieldLabel = "Display Order"
            };
            extTable.AddFieldLabelToRow(field_label_int);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.MemberCategory.status, memberCategory.status.ToString())
            {
                fieldLabel = "Status",
                datasource = "../Table/GetListItems/status",
                display_value = memberCategory.status.ToItemName("Status"),
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_select);

            // Lang Part =========================================
            var languageManager = new LanguageManager();
            var systemCode = CommonConstant.SystemCode.undefine;
            var theLangList = languageManager.GetListAll(true, ref systemCode);

            for (int i = 0; i < theLangList.Count(); i++)
            {
                PayloadKey<string> name = PayloadKeys.varWithLang("name", theLangList[i].code);
                PayloadKey<string> description = PayloadKeys.varWithLang("description", theLangList[i].code);

                var name_value = "";
                var description_value = "";

                if (memberCategory.category_id > 0)
                {
                    var lang_item = memberCategory.lang_list.Where(x => x.lang_id == theLangList[i].lang_id).FirstOrDefault();
                    name_value = lang_item.name;
                    description_value = lang_item.description;
                }

                field_label_str = new ExtJsFieldLabelInput<string>(name, name_value)
                {
                    fieldLabel = "Name",
                    group = theLangList[i].name
                };
                extTable.AddFieldLabelToRow(field_label_str);

                field_textarea = new ExtJsFieldLabelTextarea<string>(description, description_value)
                {
                    fieldLabel = "Description",
                };
                extTable.AddFieldLabelToRow(field_textarea);

            }

            // hidden table data =================================
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Id, memberCategory.category_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            if (memberCategory.category_id > 0)
            {
                var hiddenLabel_int = new ExtJsFieldLabelHidden<int>(PayloadKeys.MemberCategory.category_id, memberCategory.category_id.ToString());
                extTable.AddFieldLabelToHiddenRow(hiddenLabel_int);
            }

            var hiddenLabel_p_int = new ExtJsFieldLabelHidden<int>(PayloadKeys.MemberCategory.parent_id, memberCategory.parent_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel_p_int);

            hiddenLabel_p_int = new ExtJsFieldLabelHidden<int>(PayloadKeys.MemberCategory.leaf, memberCategory.leaf.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel_p_int);

            return extTable.ToJson();
        }


        // for passcode registry
        public static string GetFormByModule(PasscodeRegistryObject passcodeRegistry)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = passcodeRegistry.registry_id == 0 ? "Create Passcode Registry " : "Unknown Action",
                icon = "iconRole16",
                post_params = "../PasscodeRegistry/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            //add row into the table
            ExtJsFieldLabelInput<string> field_label_str;
            ExtJsFieldLabelInput<int> field_label_int;
            ExtJsFieldLabelInput<double> field_label_double;
            ExtJsFieldLabelTextarea<string> field_textarea;
            ExtJsField_dateRange<DateTime> field_dateRange;
            ExtJsFieldLabelSelect<int> field_select;
            ExtJsFieldLabelSelect<bool> field_select_bool;
            ExtJsFieldLabelUpload<string> field_uplaod;

            // get member detail
            var memberManager = new MemberManager();
            var resultCode = CommonConstant.SystemCode.undefine;
            var theMember = memberManager.GetDetail(passcodeRegistry.member_id, false, ref resultCode);


            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, theMember.member_no)
            {
                fieldLabel = "Member Code",
                value = theMember.member_no,
                readOnly = true,
                colspan = 2,
                group = "General"
            };
            extTable.AddFieldLabelToRow(field_label_str);

            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, theMember.GetFullname())
            {
                fieldLabel = "Member Name",
                value = theMember.GetFullname(),
                readOnly = true,
                colspan = 2
            };
            extTable.AddFieldLabelToRow(field_label_str);

            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.TransactionEarn.pin_value, "")
            {
                fieldLabel = "Passcode",
                colspan = 2
            };
            extTable.AddFieldLabelToRow(field_label_str);

            // hidden table data =================================
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Id, passcodeRegistry.registry_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.TransactionEarn.member_id, passcodeRegistry.member_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            //if (passcodeRegistry.transaction_id > 0)
            //{
            //    var hiddenLabel_int = new ExtJsFieldLabelHidden<int>(PayloadKeys.TransactionEarn.transaction_id, passcodeRegistry.transaction_id.ToString());
            //    extTable.AddFieldLabelToHiddenRow(hiddenLabel_int);
            //}

            return extTable.ToJson();
        }

        public static string GetFormByModule(GiftRedemptionObject giftRedemption)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = giftRedemption.gift_id == 0 ? "Create Gift Redemption" : "Edit Gift Redemption",
                icon = "iconRole16",
                post_params = "../GiftRedemption/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            // hidden table data =================================
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Id, giftRedemption.gift_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            

            hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.GiftRedemption.member_id, giftRedemption.member_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            if (giftRedemption.gift_id > 0)
            {
                var hiddenLabel_int = new ExtJsFieldLabelHidden<int>(PayloadKeys.GiftRedemption.gift_id, giftRedemption.gift_id.ToString());
                extTable.AddFieldLabelToHiddenRow(hiddenLabel_int);
            }

            //add general row into the table
            ExtJsFieldLabelInput<string> field_label_str;
            ExtJsFieldLabelInput<int> field_label_int;
            ExtJsFieldLabelInput<double> field_label_double;
            ExtJsFieldLabelTextarea<string> field_textarea;
            ExtJsField_dateRange<DateTime> field_dateRange;
            ExtJsFieldLabelSelect<int> field_select;
            ExtJsFieldLabelSelect<bool> field_select_bool;
            ExtJsFieldLabelUpload<string> field_uplaod;

            // get member detail
            var memberManager = new MemberManager();
            var resultCode = CommonConstant.SystemCode.undefine;
            var theMember = memberManager.GetDetail(giftRedemption.member_id, false, ref resultCode);


            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, theMember.member_no)
            {
                fieldLabel = "Member Code",
                value = theMember.member_no,
                readOnly = true,
                group = "General"
            };
            extTable.AddFieldLabelToRow(field_label_str);

            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, theMember.GetFullname())
            {
                fieldLabel = "Member Name",
                value = theMember.GetFullname(),
                readOnly = true
            };
            extTable.AddFieldLabelToRow(field_label_str);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.GiftRedemption.gift_id, giftRedemption.gift_id.ToString())
            {
                fieldLabel = "Gift",
                datasource = "../Table/GetListItems/gift/"+giftRedemption.member_id, //Url.Action("GetListItems/Status", "Table"),
                display_value = "",
                readOnly = false
            };
            extTable.AddFieldLabelToRow(field_select);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.GiftRedemption.location_id, giftRedemption.collect_location_id.ToString())
            {
                fieldLabel = "Location",
                datasource = "../Table/GetListItems/location", //Url.Action("GetListItems/Status", "Table"),
                display_value = "",
                readOnly = false
            };
            extTable.AddFieldLabelToRow(field_select);

            field_label_int = new ExtJsFieldLabelInput<int>(PayloadKeys.GiftRedemption.quantity, "")
            {
                fieldLabel = "Quantity",
                colspan = 2
            };
            extTable.AddFieldLabelToRow(field_label_int);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.GiftRedemption.gift_id_2, giftRedemption.gift_id.ToString())
            {
                fieldLabel = "Gift 2",
                datasource = "../Table/GetListItems/gift/" + giftRedemption.member_id, //Url.Action("GetListItems/Status", "Table"),
                display_value = "",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_select);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.GiftRedemption.location_id_2, giftRedemption.collect_location_id.ToString())
            {
                fieldLabel = "Location 2",
                datasource = "../Table/GetListItems/location", //Url.Action("GetListItems/Status", "Table"),
                display_value = "",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_select);

            field_label_int = new ExtJsFieldLabelInput<int>(PayloadKeys.GiftRedemption.quantity_2, "")
            {
                fieldLabel = "Quantity 2",
                allowBlank = true,
                colspan = 2
            };
            extTable.AddFieldLabelToRow(field_label_int);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.GiftRedemption.gift_id_3, giftRedemption.gift_id.ToString())
            {
                fieldLabel = "Gift 3",
                datasource = "../Table/GetListItems/gift/" + giftRedemption.member_id, //Url.Action("GetListItems/Status", "Table"),
                display_value = "",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_select);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.GiftRedemption.location_id_3, giftRedemption.collect_location_id.ToString())
            {
                fieldLabel = "Location 3",
                datasource = "../Table/GetListItems/location", //Url.Action("GetListItems/Status", "Table"),
                display_value = "",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_select);

            field_label_int = new ExtJsFieldLabelInput<int>(PayloadKeys.GiftRedemption.quantity_3, "")
            {
                fieldLabel = "Quantity 3",
                allowBlank = true,
                colspan = 2
            };
            extTable.AddFieldLabelToRow(field_label_int);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.GiftRedemption.gift_id_4, giftRedemption.gift_id.ToString())
            {
                fieldLabel = "Gift 4",
                datasource = "../Table/GetListItems/gift/" + giftRedemption.member_id, //Url.Action("GetListItems/Status", "Table"),
                display_value = "",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_select);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.GiftRedemption.location_id_4, giftRedemption.collect_location_id.ToString())
            {
                fieldLabel = "Location 4",
                datasource = "../Table/GetListItems/location", //Url.Action("GetListItems/Status", "Table"),
                display_value = "",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_select);

            field_label_int = new ExtJsFieldLabelInput<int>(PayloadKeys.GiftRedemption.quantity_4, "")
            {
                fieldLabel = "Quantity 4",
                allowBlank = true,
                colspan = 2
            };
            extTable.AddFieldLabelToRow(field_label_int);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.GiftRedemption.gift_id_5, giftRedemption.gift_id.ToString())
            {
                fieldLabel = "Gift 5",
                datasource = "../Table/GetListItems/gift/" + giftRedemption.member_id, //Url.Action("GetListItems/Status", "Table"),
                display_value = "",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_select);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.GiftRedemption.location_id_5, giftRedemption.collect_location_id.ToString())
            {
                fieldLabel = "Location 5",
                datasource = "../Table/GetListItems/location", //Url.Action("GetListItems/Status", "Table"),
                display_value = "",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_select);

            field_label_int = new ExtJsFieldLabelInput<int>(PayloadKeys.GiftRedemption.quantity_5, "")
            {
                fieldLabel = "Quantity 5",
                allowBlank = true,
                colspan = 2
            };
            extTable.AddFieldLabelToRow(field_label_int);

            return extTable.ToJson();
        }

        public static string GetFormByModule(sp_GetRoleDetailResult role)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = role.role_id == 0 ? "Create Role" : "Edit Role",
                icon = "iconRole16",
                post_params = "../Role/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            //add general row into the table
            ExtJsFieldLabelInput<string> field_label_str;
            ExtJsFieldLabelInput<int> field_label_int;
            ExtJsFieldLabelInput<double> field_label_double;
            ExtJsFieldLabelTextarea<string> field_textarea;
            ExtJsField_dateRange<DateTime> field_dateRange;
            ExtJsFieldLabelSelect<int> field_select;
            ExtJsFieldLabelSelect<bool> field_select_bool;
            ExtJsFieldLabelUpload<string> field_uplaod;


            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Role.name, role.name)
            {
                fieldLabel = "Role",
                group = "General"
            };
            extTable.AddFieldLabelToRow(field_label_str);


            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.Role.status, role.status.ToString())
            {
                fieldLabel = "status",
                datasource = "../Table/GetListItems/status", //Url.Action("GetListItems/Status", "Table"),
                display_value = role.status.ToItemName("Status"),
            };
            extTable.AddFieldLabelToRow(field_select);



            // hidden table data =================================
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Role.role_id,  role.role_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }

        public static string UploadFormPasscodeExcel()
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData", //Url.Action("ListData", "Common"),
                post_header = "../Table/GridHeader", //Url.Action("GridHeader", "Common"),
                title = "Import Passcode from Excel",
                icon = "iconRole16",
                post_params = "../Member/Update", // Url.Action("Update"), //Update action
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            var rowFieldLabelUpload = new ExtJsFieldLabelUpload<string>(PayloadKeys.fbid, "")
            {
                group = "General",

                name = "fileData",
                fieldLabel = "Upload",
                upType = "file",
                upload_url = "../PasscodeExcel/Upload"
            };
            extTable.AddFieldLabelToRow(rowFieldLabelUpload);
            
            return extTable.ToJson();
        }

        public static string GetFormByModule(GiftInventoryObject gift_inventory)
        {

            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = gift_inventory.inventory_id == 0 ? "Create Record" : "Edit Record",
                icon = "iconRole16",
                post_params = "../GiftInventory/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            //add row into the table
            ExtJsFieldLabelInput<string> field_label_str;
            ExtJsFieldLabelInput<int> field_label_int;
            ExtJsFieldLabelInput<double> field_label_double;
            ExtJsFieldLabelTextarea<string> field_textarea;
            ExtJsField_dateRange<DateTime> field_dateRange;
            ExtJsFieldLabelSelect<int> field_select;
            ExtJsFieldLabelSelect<bool> field_select_bool;
            ExtJsFieldLabelMultiSelect<int> field_multiSelect;

            // gift name
            var giftManager = new GiftManager();
            var sql_result = false;
            var theGift = giftManager.GetGiftDetailBy(SessionManager.Current.obj_id, gift_inventory.gift_id, ref sql_result);

            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Gift_inventory.gift_name, theGift.name)
            {
                fieldLabel = "Gift Name",
                group = "General",
                colspan = 2,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(field_label_str);

            field_label_int = new ExtJsFieldLabelInput<int>(PayloadKeys.Gift_inventory.stock_change, gift_inventory.stock_change.ToString())
            {
                fieldLabel = "Stock Change (+/-) (add/deduct)",
                colspan = 2
            };
            extTable.AddFieldLabelToRow(field_label_int);

            field_textarea = new ExtJsFieldLabelTextarea<string>(PayloadKeys.Gift_inventory.remark, gift_inventory.remark)
            {
                fieldLabel = "Remark",
                colspan = 2,
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_textarea);

            // hidden table data =================================
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Gift_inventory.gift_inventory_id, gift_inventory.inventory_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Gift_inventory.gift_id, gift_inventory.gift_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }

        public static string GetFormByModule(ServiceCategoryObject serviceCategory)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = "Service Category Detail",
                icon = "iconRole16",
                post_params = "../ServiceCategory/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            //add row into the table
            
            var parentCat_name = "Root";

            if (serviceCategory.parent_id > 0)
            {
                var serviceCategoryManager = new ServiceCategoryManager();
                var systemCode = CommonConstant.SystemCode.undefine;
                var parentCat = serviceCategoryManager.GetDetail(serviceCategory.parent_id, ref systemCode);
                parentCat_name = parentCat.name;
            }

            var field_label_string = new ExtJsFieldLabelInput<string>(PayloadKeys.ServiceCategory.parent_name, parentCat_name)
            {
                fieldLabel = "Parent Category",
                value = parentCat_name,
                group = "General",
                readOnly = true
            };
            extTable.AddFieldLabelToRow(field_label_string);

            if (serviceCategory.category_id > 0)
            {
                var field_label_id = new ExtJsFieldLabelInput<int>(PayloadKeys.ServiceCategory.category_id, serviceCategory.category_id.ToString())
                {
                    fieldLabel = "Category ID",
                    readOnly = true
                };
                extTable.AddFieldLabelToRow(field_label_id);
            }

            var field_label_int = new ExtJsFieldLabelInput<int>(PayloadKeys.ServiceCategory.display_order, serviceCategory.display_order.ToString())
            {
                fieldLabel = "Display Order"
            };
            extTable.AddFieldLabelToRow(field_label_int);

            var field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.ServiceCategory.status, serviceCategory.status.ToString())
            {
                fieldLabel = "Status",
                datasource = "../Table/GetListItems/status",
                display_value = serviceCategory.status_name,
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_select);

            var field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.ServiceCategory.name, serviceCategory.name)
            {
                fieldLabel = "Name"
            };
            extTable.AddFieldLabelToRow(field_label_str);

            var field_textarea = new ExtJsFieldLabelTextarea<string>(PayloadKeys.ServiceCategory.description, serviceCategory.description)
            {
                fieldLabel = "Description",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_textarea);

            //hidden table data
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.ServiceCategory.category_id, serviceCategory.category_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            var hiddenLabel_p_int = new ExtJsFieldLabelHidden<int>(PayloadKeys.ServiceCategory.parent_id, serviceCategory.parent_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel_p_int);

            hiddenLabel_p_int = new ExtJsFieldLabelHidden<int>(PayloadKeys.ServiceCategory.leaf, serviceCategory.leaf.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel_p_int);

            return extTable.ToJson();
        }

        public static string GetFormByModule(ServicePlanObject servicePlan)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = "Service Category Detail",
                icon = "iconRole16",
                post_params = "../ServicePlan/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            var field_label_string = new ExtJsFieldLabelInput<string>(PayloadKeys.ServicePlan.plan_no, servicePlan.plan_no)
            {
                fieldLabel = "Service Plan No",
                value = servicePlan.plan_no,
                group = "General",
            };
            extTable.AddFieldLabelToRow(field_label_string);

            var field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.ServicePlan.name, servicePlan.name)
            {
                fieldLabel = "Name"
            };
            extTable.AddFieldLabelToRow(field_label_str);

            var field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.ServicePlan.type, servicePlan.type.ToString())
            {
                fieldLabel = "Type",
                datasource = "../Table/GetListItems/servicecategory",
                display_value = servicePlan.type_name,
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_select);

            var field_textarea = new ExtJsFieldLabelTextarea<string>(PayloadKeys.ServicePlan.description, servicePlan.description)
            {
                fieldLabel = "Description",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_textarea);

            var field_label_double = new ExtJsFieldLabelInput<double>(PayloadKeys.ServicePlan.fee, servicePlan.fee.ToString())
            {
                fieldLabel = "Standard Fee",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_label_double);

            var field_label_int = new ExtJsFieldLabelInput<int>(PayloadKeys.ServicePlan.point_expiry_month, servicePlan.point_expiry_month.ToString())
            {
                fieldLabel = "Point Expiry Month",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_label_int);

            field_label_double = new ExtJsFieldLabelInput<double>(PayloadKeys.ServicePlan.ratio_payment, servicePlan.ratio_payment.ToString())
            {
                fieldLabel = "Point Ratio (payment)",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_label_double);

            field_label_double = new ExtJsFieldLabelInput<double>(PayloadKeys.ServicePlan.ratio_point, servicePlan.ratio_point.ToString())
            {
                fieldLabel = "Point Ratio (to point)",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_label_double);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.ServicePlan.status, servicePlan.status.ToString())
            {
                fieldLabel = "Status",
                datasource = "../Table/GetListItems/status",
                display_value = servicePlan.status_name,
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_select);

            //hidden table data
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.ServicePlan.plan_id, servicePlan.plan_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }

        public static string GetFormByModule(MemberCardObject cardObject)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = "Member Card Detail",
                icon = "iconRole16",
                post_params = "../MemberCard/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            var field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.MemberCard.card_no, cardObject.card_no)
            {
                fieldLabel = "Card No",
                value = cardObject.card_no,
                group = "General",
            };
            extTable.AddFieldLabelToRow(field_label_str);

            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.MemberCard.member_no, cardObject.member_no)
            {
                fieldLabel = "Member No",
                value = cardObject.member_no
            };
            extTable.AddFieldLabelToRow(field_label_str);

            var field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.MemberCard.card_type, cardObject.card_type.ToString())
            {
                fieldLabel = "Card Type",
                datasource = "../Table/GetListItems/memberlevel",
                display_value = cardObject.card_type_name,
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_select);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.MemberCard.card_status, cardObject.card_status.ToString())
            {
                fieldLabel = "Card Status",
                datasource = "../Table/GetListItems/MemberCardStatus",
                display_value = cardObject.card_status_name,
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_select);

            var field_dateTime = new ExtJsField_dateTime_noSec<DateTime?>(PayloadKeys.MemberCard.issue_date, cardObject.issue_date)
            {
                fieldLabel = "Issue Date",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_dateTime);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.MemberCard.status, cardObject.status.ToString())
            {
                fieldLabel = "Status",
                datasource = "../Table/GetListItems/status",
                display_value = cardObject.status_name,
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_select);
                      
            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.MemberCard.remark, cardObject.remark)
            {
                fieldLabel = "Remark",
                value = cardObject.remark,
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_label_str);

            //hidden table data
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.MemberCard.card_id, cardObject.card_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }
    }
}