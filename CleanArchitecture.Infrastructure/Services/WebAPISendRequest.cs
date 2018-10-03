using CleanArchitecture.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    //For All type of Web Request
    public class WebAPISendRequest<T> : IWebApiSendRequest
    {
        public readonly ILogger<T> _log;
        public WebAPISendRequest(ILogger<T> log)
        {
            _log = log;
        }
        public Task<string> SendAPIRequestAsync(string Url, string Request, string ContentType,int Timeout, string MethodType = "POST")
        {
            string responseFromServer = "";
            try
            {               
                object ResponseObj = new object();
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
                httpWebRequest.ContentType = ContentType;

                httpWebRequest.Method = MethodType.ToUpper();
                httpWebRequest.KeepAlive = false;
                httpWebRequest.Timeout = Timeout;

                _log.LogInformation(System.Reflection.MethodBase.GetCurrentMethod().Name, Url, Request);              
               
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(Request);
                    streamWriter.Flush();
                    streamWriter.Close();
                }           

                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                using (StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    responseFromServer = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();

                }
                httpWebResponse.Close();
                _log.LogInformation(System.Reflection.MethodBase.GetCurrentMethod().Name, responseFromServer);                
            }
            catch (Exception ex)
            {                
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
            }
            
            return Task.FromResult(responseFromServer);
        }

        public Task<string> SendRequestAsync(string Url, string MethodType = "GET")
        {
            string responseFromServer = "";
            try
            {
                object ResponseObj = new object();
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);

                httpWebRequest.Method = MethodType.ToUpper();

                _log.LogInformation(System.Reflection.MethodBase.GetCurrentMethod().Name, Url, MethodType);

                //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                //{
                //    streamWriter.Write(Request);
                //    streamWriter.Flush();
                //    streamWriter.Close();
                //}

                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                using (StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    responseFromServer = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                }
                httpWebResponse.Close();
                _log.LogInformation(System.Reflection.MethodBase.GetCurrentMethod().Name, responseFromServer);
                return Task.FromResult(responseFromServer);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }            
        }
    }
}
