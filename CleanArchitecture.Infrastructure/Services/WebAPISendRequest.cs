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

        public JObject SendJsonRpcAPIRequestAsync(string Url, object[] RequestParameter, string method, WebHeaderCollection headerDictionary, string MethodType = "POST")
        {
            try
            {
                var perso = JsonConvert.DeserializeObject<dynamic>(method);
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Url);

                webRequest.ContentType = "application/json-rpc";
                webRequest.Method = MethodType;

                JObject joe = new JObject();
                joe["jsonrpc"] = "1.0";
                joe["id"] = "1";
                joe["method"] = perso;

                if (RequestParameter != null)
                {
                    if (RequestParameter.Length > 0)
                    {
                        JArray props = new JArray();
                        foreach (var p in RequestParameter)
                        {
                            props.Add(p);
                        }
                        joe.Add(new JProperty("params", props));
                    }
                }

                string s = JsonConvert.SerializeObject(joe);
                // serialize json for the request
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                webRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = webRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                WebResponse webResponse = null;
                try
                {
                    using (webResponse = webRequest.GetResponse())
                    {
                        using (Stream str = webResponse.GetResponseStream())
                        {
                            using (StreamReader sr = new StreamReader(str))
                            {
                                return JsonConvert.DeserializeObject<JObject>(sr.ReadToEnd());
                            }
                        }
                    }
                }
                catch (WebException webex)
                {
                    using (Stream str = webex.Response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(str))
                        {
                            var tempRet = JsonConvert.DeserializeObject<JObject>(sr.ReadToEnd());
                            return tempRet;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
            }
            return new JObject();
        }

    }
}
