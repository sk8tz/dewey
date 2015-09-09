using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Dewey.Net.Log
{
    public static class Logger
    {
        //private const string _errorLog = @"C:\Tmp\axial\error.log";
        //private const string _infoLog = @"C:\Tmp\axial\info.log";

        public static void Error(Exception ex, string customMessage = null)
        {
#if DEBUG
            if (customMessage != null) {
                Console.WriteLine(customMessage);
            }

            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            if (ex.InnerException != null) {
                Console.WriteLine(ex.InnerException.Message);
                Console.WriteLine(ex.InnerException.StackTrace);
            }

            throw ex;
#else
            //if (customMessage != null) {
            //    File.AppendAllText(_errorLog, customMessage);
            //    File.AppendAllText(_errorLog, "\r\n");
            //}

            //File.AppendAllText(_errorLog, ex.Message);
            //File.AppendAllText(_errorLog, "\r\n");
            //File.AppendAllText(@_errorLog, ex.StackTrace);
            //File.AppendAllText(_errorLog, "\r\n");
            //if (ex.InnerException != null) {
            //    File.AppendAllText(_errorLog, ex.InnerException.Message);
            //    File.AppendAllText(_errorLog, "\r\n");
            //    File.AppendAllText(_errorLog, ex.InnerException.StackTrace);
            //    File.AppendAllText(_errorLog, "\r\n");
            //}
            throw ex;
#endif
        }

        public static void Info(string message)
        {
#if DEBUG
            Console.WriteLine(message);
#else
            //File.AppendAllText(_errorLog, message);
            //File.AppendAllText(_errorLog, "\r\n");
#endif
        }
    }
}