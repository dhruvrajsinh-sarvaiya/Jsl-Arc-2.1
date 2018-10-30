using CleanArchitecture.Infrastructure.Interfaces;
using CleanArchitecture.Web.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Web.Filters
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        private readonly IBasePage _basePage;
        public RequestResponseLoggingMiddleware(RequestDelegate next, IBasePage basePage)
        {
            _next = next;
            _basePage = basePage;
        }

        public async Task Invoke(HttpContext context)
        {
            var injectedRequestStream = new MemoryStream();

            try
            {
                var requestLog =
                $"REQUEST Host:{context.Request.Host} ,HttpMethod: {context.Request.Method}, Path: {context.Request.Path}";
                //string Path = context.Request.Path.ToString();
                //string[] PathDetails = Path.Split("/");

                var accessToken = await context.Request.HttpContext.GetTokenAsync("access_token");
                using (var bodyReader = new StreamReader(context.Request.Body))
                {
                    var bodyAsText = bodyReader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(bodyAsText) == false)
                    {
                        requestLog += $", Body : {bodyAsText}";
                    }

                    var bytesToWrite = Encoding.UTF8.GetBytes(bodyAsText);
                    injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                    injectedRequestStream.Seek(0, SeekOrigin.Begin);
                    context.Request.Body = injectedRequestStream;
                }

                //_logger.LogTrace(requestLog);
                //if (PathDetails?[1] == "api")
                HelperForLog.WriteLogIntoFile(1, _basePage.UTC_To_IST(), context.Request.Path.ToString(), context.Request.Path.ToString(), requestLog, accessToken);
                //else if (PathDetails?[1] != "swagger")
                // HelperForLog.WriteLogIntoFile(1, _basePage.UTC_To_IST(), "", "", requestLog);
                //_logger.LogInformation(1, requestLog);
                await _next.Invoke(context);

            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), "", "", ex.ToString());
            }
            finally
            {
                injectedRequestStream.Dispose();
            }
        }
    }
}
