using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SharedKernel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    //For All type of Web Request
    public class WebAPISendRequest : IWebApiSendRequest
    {
        public readonly ILogger<WebAPISendRequest> _log;

        public WebAPISendRequest(ILogger<WebAPISendRequest> log)
        {
            _log = log;
        }

        public string  SendAPIRequestAsync(string Url, string Request, string ContentType,int Timeout= 180000, WebHeaderCollection   headerDictionary = null, string MethodType = "POST")
        {
            string responseFromServer = "";
            try
            {               
                object ResponseObj = new object();
           //     ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
           //| SecurityProtocolType.Tls11
           //| SecurityProtocolType.Tls12
           //| SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
                httpWebRequest.ContentType = ContentType;
                
                httpWebRequest.Method = MethodType.ToUpper();
                httpWebRequest.KeepAlive = false;
                httpWebRequest.Timeout = Timeout;

                httpWebRequest.Headers = headerDictionary;

                _log.LogInformation(System.Reflection.MethodBase.GetCurrentMethod().Name, Url, Request);

                if (Request != null)
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(Request);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
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
            
            return responseFromServer;
        }

        public Task<string> SendRequestAsync(string Url, string Request="", string MethodType = "GET", string ContentType="application/json", WebHeaderCollection Headers = null, int Timeout = 9000)
        {
            string responseFromServer = "";
            try
            {
                object ResponseObj = new object();
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);

                httpWebRequest.Method = MethodType.ToUpper();
                if(Headers != null)
                {
                    httpWebRequest.Headers = Headers;
                }                
                httpWebRequest.KeepAlive = false;
                httpWebRequest.Timeout = Timeout;
                httpWebRequest.ContentType = ContentType;
                //if (!string.IsNullOrEmpty(Headers))
                //{
                // httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", Headers));
                // }

                _log.LogInformation(System.Reflection.MethodBase.GetCurrentMethod().Name, Url, MethodType);

                if(Request != "")
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(Request);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
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
                return Task.FromResult(responseFromServer);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }            
        }

        public async Task<string> SendTCPSocketRequestAsync(string HostName, string Port, string request)
        {
            string responseFromServer = "";

            int read;
            byte[] buffer1 = new byte[2048];
            bool IsSocketCallDone = false;

            try
            {
                if (string.IsNullOrEmpty(HostName) || string.IsNullOrEmpty(Port))
                {
                    responseFromServer = "Configuration Not Found";
                    return responseFromServer;
                }
                IPAddress ipAddress = IPAddress.Parse(HostName);
                System.Net.Sockets.TcpClient client = new TcpClient();
                await client.ConnectAsync(ipAddress, Convert.ToInt32(Port));
                client.ReceiveTimeout = 61000;
                client.SendTimeout = 61000;
                NetworkStream networkStream = client.GetStream();
                StreamWriter writer = new StreamWriter(networkStream, Encoding.UTF8);

                writer.AutoFlush = true;
                await writer.WriteLineAsync(request);

                read = networkStream.Read(buffer1, 0, buffer1.Length);
                IsSocketCallDone = true;
                byte[] data = new byte[read];
                Array.Copy(buffer1, data, read);
                responseFromServer = Encoding.UTF8.GetString(data);

                networkStream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }

            return responseFromServer;
        }

        public string SendJsonRpcAPIRequestAsync(string Url,string RequestStr,string UserName,string Password)
        {
            try
            {
                string WSResponse = "";
                try
                {
                    var authInfo = UserName + ":" + Password;
                    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                    var myReqrpc = WebRequest.Create(Url);
                    myReqrpc.Headers.Add("Authorization", "Basic " + authInfo);
                    myReqrpc.Method = "Post";

                    var sw = new StreamWriter(myReqrpc.GetRequestStream());
                    sw.Write(RequestStr);
                    sw.Close();

                    WebResponse response;
                    response = myReqrpc.GetResponse();

                    StreamReader StreamReader = new StreamReader(response.GetResponseStream());
                    WSResponse = StreamReader.ReadToEnd();
                    StreamReader.Close();
                    response.Close();

                    return WSResponse;
                }
                catch(WebException webex)
                {
                    WebResponse errResp = webex.Response;
                    Stream respStream = errResp.GetResponseStream();
                    StreamReader reader = new StreamReader(respStream);
                    string Text = reader.ReadToEnd();                   
                    if (Text.ToLower().Contains("code"))
                    {
                        WSResponse = Text;
                    }
                    webex = null;

                    return WSResponse;
                }
                
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
    }
}
