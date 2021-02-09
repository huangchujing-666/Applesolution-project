using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.OleDb;

namespace Palmary.Loyalty.BO.Modules.Utility
{
    public class ExcelManager
    {
        public IEnumerable<DataRow> Read(string filePath, string sheet_name, ref bool result, ref string message)
        {
            IEnumerable<DataRow> data = new List<DataRow>();
            result = false;

            try
            {
                var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=YES\";", filePath);
                var adapter = new OleDbDataAdapter("SELECT * FROM [" + sheet_name +"$]", connectionString);

                var excelDataSet = new DataSet();
                adapter.Fill(excelDataSet, "DataList");

                data = excelDataSet.Tables["DataList"].AsEnumerable();

                result = true;
            }
            catch(Exception e)
            {
                message = e.Message;
            }

            return data;
        }
    }
}
