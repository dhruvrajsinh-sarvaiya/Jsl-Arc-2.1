using CleanArchitecture.Infrastructure.Interfaces;
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
                    responseLog += $", Response : {responseBody}";
                    //if (responseBody.ToLower().Contains("error_description"))
                    //{
                    //    ErrorParams erParams = JsonConvert.DeserializeObject<ErrorParams>(responseBody);
                    //    //if (erParams != null && erParams.error.ToLower() == "invalid_request")
                    //    //{
                    //    //    newBody.Seek(0, SeekOrigin.Begin);
                    //    //    newContent = new StreamReader(newBody).ReadToEnd();

                    //    //    newContent += "{\"ReturnCode\":1,\"ReturnMsg\":\"Invalid Request\",\"ErrorCode\":4000";
                    //    //    await context.Response.WriteAsync(newContent);
                    //    //}
                    //}
                    //_logger.LogTrace(responseBody);
                    if (ResponseDetails?[1] == "api")
                        HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), ResponseDetails?[3], ResponseDetails?[2], responseLog);
                    else
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
        public string error { get; set; }
        public string error_description { get; set; }
    }
}
