﻿using CleanArchitecture.Infrastructure.Interfaces;
using CleanArchitecture.Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Web.Filters
{
    public class ResponseRewindMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        private readonly IBasePage _basePage;

        public ResponseRewindMiddleware(RequestDelegate next, IBasePage basePage)
        {
            this.next = next;
            _basePage = basePage;
        }

        public async Task Invoke(HttpContext context)
        {
            Stream originalBody = context.Response.Body;
            try
            {
                using (var memStream = new MemoryStream())
                {
                    var newBody = new MemoryStream();
                    var newContent = string.Empty;

                    context.Response.Body = memStream;

                    await next(context);
                    var responseLog = $"RESPONSE Host:{context.Request.Host}, HttpMethod: {context.Request.Method}, Path: {context.Request.Path}";

                    string ResponsePath = context.Request.Path.ToString();
                    string[] ResponseDetails = ResponsePath.Split("/");

                    memStream.Position = 0;
                    string responseBody = new StreamReader(memStream).ReadToEnd();
                    var erParams = (dynamic)null;
                    if (ResponseDetails?[1] != "swagger")
                        erParams = JsonConvert.DeserializeObject<ErrorParams>(responseBody);

                    responseLog += $", Response : {responseBody}";

                    if (ResponseDetails?[1] == "api")
                    {
                        if (erParams.ReturnCode == 9)
                        {
                            HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), ResponseDetails?[3], ResponseDetails?[2], responseLog);
                            HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), ResponseDetails?[3], ResponseDetails?[2], erParams.ReturnMsg);
                        }
                        else
                            HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), ResponseDetails?[3], ResponseDetails?[2], responseLog);
                    }
                    else if (ResponseDetails?[1] != "swagger")
                        HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), "", "", responseLog);

                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);
                }

            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }
    }

    public class ErrorParams
    {
        public long ReturnCode { get; set; }
        public string ReturnMsg { get; set; }
        public int ErrorCode { get; set; }
    }
}
