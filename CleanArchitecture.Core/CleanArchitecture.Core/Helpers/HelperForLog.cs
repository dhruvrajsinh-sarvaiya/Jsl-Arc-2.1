using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Helpers
{
    public class HelperForLog
    {
        //Rita 26-10-2018 for all log used in below project of core
        public static void WriteLogIntoFile(string MethodName, string Controllername, string LogData = null, string accessToken = null)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                logger.Info("\nDateTime:" + Helpers.UTC_To_IST() + "\nMethodName:" + MethodName + ", Controllername: " + Controllername + "\nLogData: " + LogData + "\n===================================================================================================================");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        public static void WriteErrorLog(string MethodName, string Controllername, string Error, string accessToken = null)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                logger.Error("\nDateTime:" + Helpers.UTC_To_IST() + "\nMethodName:" + MethodName + ",Controllername: " + Controllername + "\nError: " + Error + "\n===================================================================================================================");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}
