using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interfaces;
using CleanArchitecture.Web.Helper;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace CleanArchitecture.Web.Filters
{
    public class ApiResultFilter : ActionFilterAttribute
    {
        private readonly IBasePage _basePage;
        private readonly IConfiguration _configuration;
        public ApiResultFilter(IBasePage basePage, IConfiguration configuration)
        {
            _basePage = basePage;
            _configuration = configuration;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //_logger.LogWarning("ClassFilter OnResultExecuting");
            // _logger.LogWarning(((string[])((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value)[0]);
            //base.OnResultExecuting(context);

            if (!context.ModelState.IsValid)
            {
                //context.Result = new ValidationFailedResult(context.ModelState);
                context.Result = new MyResponse(context.ModelState);
            }

            if (((Microsoft.AspNetCore.Http.Internal.DefaultHttpRequest)((Microsoft.AspNetCore.Http.DefaultHttpContext)context.HttpContext).Request).Path.ToString() != _configuration["ASOSToken"].ToString())
            {
                string ReturnCode = ((Core.ApiModels.BizResponseClass)((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value)?.ErrorCode.ToString();
                if (ReturnCode == "Status500InternalServerError")
                {
                    string ReturnMsg = ((CleanArchitecture.Core.ApiModels.BizResponseClass)((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value).ReturnMsg;
                    HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), context.HttpContext.Request.Path.ToString(), context.HttpContext.Request.Path.ToString(), ReturnMsg);
                    HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), context.HttpContext.Request.Path.ToString(), context.HttpContext.Request.Path.ToString(), ReturnMsg);
                    ((CleanArchitecture.Core.ApiModels.BizResponseClass)((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value).ReturnMsg = "Error occurred.";
                }
            }


            //if (((Microsoft.AspNetCore.Mvc.SignInResult)context.Result).AuthenticationScheme.ToString() != "ASOS")
            //{
            //    string ReturnCode = ((Core.ApiModels.BizResponseClass)((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value)?.ErrorCode.ToString();
            //    if (ReturnCode == "Status500InternalServerError")
            //    {
            //        string ReturnMsg = ((CleanArchitecture.Core.ApiModels.BizResponseClass)((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value).ReturnMsg;
            //        HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), context.HttpContext.Request.Path.ToString(), context.HttpContext.Request.Path.ToString(), ReturnMsg);
            //        HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), context.HttpContext.Request.Path.ToString(), context.HttpContext.Request.Path.ToString(), ReturnMsg);
            //        ((CleanArchitecture.Core.ApiModels.BizResponseClass)((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value).ReturnMsg = "Error occurred.";
            //    }
            //}
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //string k = (string[])((CleanArchitecture.Core.ApiModels.BizResponseClass)((Microsoft.AspNetCore.Mvc.ObjectResult)filterContext.Result).Value);
            //_logger.LogWarning("ClassFilter OnResultExecuted");

            //string k = ((string[])((Microsoft.AspNetCore.Mvc.ObjectResult)filterContext.Result).Value)[1];
            //_logger.LogWarning(((string[])((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value)[1]);
            //base.OnResultExecuted(context);
        }
    }
}
