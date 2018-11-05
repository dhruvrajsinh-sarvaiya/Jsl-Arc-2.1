using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ApiModels.Chat;
using CleanArchitecture.Core.Entities.SignalR;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Services;
using CleanArchitecture.Core.Services.RadisDatabase;
using CleanArchitecture.Infrastructure.Interfaces;
using CleanArchitecture.Web.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CleanArchitecture.Web.Filters
{
    public class ResponseRewindMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IBasePage _basePage;
        private readonly UserManager<ApplicationUser> _userManager;
        private RedisConnectionFactory _fact;

        public ResponseRewindMiddleware(RequestDelegate next, IBasePage basePage, UserManager<ApplicationUser> UserManager, RedisConnectionFactory Factory)
        {
            this.next = next;
            _basePage = basePage;
            _userManager = UserManager;
            _fact = Factory;
        }

        public async Task Invoke(HttpContext context)
        {
            Stream originalBody = context.Response.Body;
            SignalRUserConfiguration User = new SignalRUserConfiguration();
            try
            {
                using (var memStream = new MemoryStream())
                {
                    var newBody = new MemoryStream();
                    var newContent = string.Empty;

                    context.Response.Body = memStream;

                    await next(context);
                    var responseLog = $"RESPONSE Host:{context.Request.Host}, HttpMethod: {context.Request.Method}, Path: {context.Request.Path}";

                    //string ResponsePath = context.Request.Path.ToString();
                    //string[] ResonseDetails = ResponsePath.Split("/");
                    if (context.Request.Path == "/connect/token")
                    {
                        memStream.Seek(0, SeekOrigin.Begin);
                        var content = new StreamReader(memStream).ReadToEnd();
                        memStream.Seek(0, SeekOrigin.Begin);
                        tokanreponsmodel TokenData = JsonConvert.DeserializeObject<tokanreponsmodel>(content);
                        if(TokenData != null && !string.IsNullOrEmpty(TokenData.access_token))
                        {
                            using (var bodyReader = new StreamReader(context.Request.Body))
                            {
                                var bodyAsText = bodyReader.ReadToEnd();
                                if (string.IsNullOrWhiteSpace(bodyAsText) == false)
                                {
                                    User = convertStirngToJson(HttpUtility.UrlDecode(bodyAsText));
                                   // User.username = "user@user.com";
                                }
                                if (User != null && !string.IsNullOrEmpty(User.username))
                                {
                                    var Redis = new RadisServices<ConnetedClientToken>(this._fact);
                                    var Userdata = _userManager.FindByNameAsync(User.username).GetAwaiter().GetResult();
                                    if (Userdata != null && Userdata.Id != 0)
                                    {
                                        Redis.SaveWithOrigionalKey("Tokens:" + Userdata.Id, new ConnetedClientToken { Token = TokenData.access_token });
                                        //string AccessToken = Redis.GetHashData("Tokens:" + Userdata.Id, "Token");
                                    }                                    
                                }
                            }
                        }
                    }

                    memStream.Position = 0;
                    string responseBody = new StreamReader(memStream).ReadToEnd();
                    var erParams = (dynamic)null;
                    if (responseBody.Contains("ReturnCode"))
                        erParams = JsonConvert.DeserializeObject<ErrorParams>(responseBody);

                    responseLog += $", Response : {responseBody}";
                    //if (ResponseDetails?[1] == "api")
                    //{
                    //Uday 05-11-2018 don't write log for graph method
                    if (context.Request.Path.Value.Split("/")[1] != "swagger" && !context.Request.Path.Value.Contains("GetGraphDetail"))
                    {
                        if (erParams?.ReturnCode != 9)
                        {
                            HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), context.Request.Path.ToString(), context.Request.Path.ToString(), responseLog);
                            //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), context.Request.Path.ToString(), context.Request.Path.ToString(), responseLog);
                            //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), context.Request.Path.ToString(), context.Request.Path.ToString(), erParams.ReturnMsg);
                        }
                    }
                    //else
                    //{
                    //    HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), context.Request.Path.ToString(), context.Request.Path.ToString(), responseLog);

                    //}

                    //}
                    //else if (ResponseDetails?[1] != "swagger")
                    //    HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), "", "", responseLog);                    
                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);
                }

            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), "", "", ex.ToString());
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }

        public SignalRUserConfiguration convertStirngToJson(string Data)
        {
            //string str = "clientId=cleanarchitecture&grant_type=password&username=user@user.com&password=P@ssw0rd!&scope=openid profile email offline_access client_id roles phone";
            Data = Data.Replace("=", "\":\"");
            Data = Data.Replace("&", "\",\"");
            Data = "{\"" + Data + "\"}";
            SignalRUserConfiguration obj = JsonConvert.DeserializeObject<SignalRUserConfiguration>(Data);
            return obj;
            //var jsonData = JsonConvert.SerializeObject(obj);
        }
    }

    public class ErrorParams
    {
        public long ReturnCode { get; set; }
        public string ReturnMsg { get; set; }
        public int ErrorCode { get; set; }
    }
}
