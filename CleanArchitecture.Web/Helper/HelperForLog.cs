using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Web.Helper
{
    public class HelperForLog
    {
        //vsolanki 17-10-2018
        public static void WriteLogIntoFile(byte CheckReq_Res, DateTime Date, string MethodName, string Controllername, string Req_Res = null, string accessToken = null)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                if (CheckReq_Res == 1)
                {
                    logger.Info("\nDate:" + Date + "\nMethodName:" + MethodName + ", Controllername: " + Controllername + "\nAccessToken: " + accessToken + "\nRequest: " + Req_Res + "\n===================================================================================================================");
                }
                else //if(CheckReq_Res==2)
                {
                    logger.Info("\nDate:" + Date + "\nMethodName:" + MethodName + ", Controllername: " + Controllername + "\nResponse: " + Req_Res + "\n===================================================================================================================");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        public static void WriteErrorLog(DateTime Date, string MethodName, string Controllername, string Error)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                logger.Error("\nDate:" + Date + "\nMethodName:" + MethodName + ",Controllername: " + Controllername + "\nError: " + Error + "\n===================================================================================================================");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}
