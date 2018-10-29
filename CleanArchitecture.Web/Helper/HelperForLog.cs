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
                    if (accessToken != null)
                        logger.Info(Environment.NewLine + "\nDate:" + Date + ",MethodName:" + MethodName + ", Controllername: " + Controllername + Environment.NewLine + "AccessToken: " + accessToken + Environment.NewLine + "Request: " + Req_Res + Environment.NewLine + "===================================================================================================================");
                    else
                        logger.Info(Environment.NewLine + "\nDate:" + Date + ",MethodName:" + MethodName + ", Controllername: " + Controllername + Environment.NewLine + "Request: " + Req_Res + Environment.NewLine + "===================================================================================================================");
                }
                else //if(CheckReq_Res==2)
                {
                    logger.Info(Environment.NewLine + "Date:" + Date + ", MethodName:" + MethodName + ", Controllername: " + Controllername + Environment.NewLine + "Response: " + Req_Res + Environment.NewLine + "===================================================================================================================");
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
                logger.Error(Environment.NewLine + "Date:" + Date + ", MethodName:" + MethodName + ",Controllername: " + Controllername + "\nError: " + Error + Environment.NewLine + "===================================================================================================================");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}
