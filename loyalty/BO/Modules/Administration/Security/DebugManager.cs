using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.Modules.Administration.Security
{
    public static class DebugManager
    {
        public static string _path = @"D:\log";
        public static string _fileName = @"log.txt";

        public static void LogFileWriteln(String logText)
        {
            // Create Directory if not exist
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            // Add log time
            logText = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "] " + logText;

            // Write into log file
            string filePath = Path.Combine(_path, _fileName);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
            {
                file.WriteLine(logText);
            }
        }
    }
}
