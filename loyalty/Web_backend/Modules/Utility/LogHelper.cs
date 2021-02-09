using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;

namespace Palmary.Loyalty.Web_backend.Modules.Utility
{
    public static class LogHelper
    {
        public static void WriteLine(string filename, string message)
        {
            var dirPath = PathHandler.GetStorage_serverFullPath("Log");
            var logFilePath = PathHandler.GetStorage_serverFullPath("Log", filename);

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            System.IO.StreamWriter logfile = new System.IO.StreamWriter(logFilePath, true);
            logfile.WriteLine(DateTime.Now + " " + message);
            logfile.Close();
        }
    }
}