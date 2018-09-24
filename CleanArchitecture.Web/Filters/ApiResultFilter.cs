using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanArchitecture.Web.Filters
{
    public class ApiResultFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //_logger.LogWarning("ClassFilter OnResultExecuting");
           // _logger.LogWarning(((string[])((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value)[0]);
            //base.OnResultExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //_logger.LogWarning("ClassFilter OnResultExecuted");

            //string k = ((string[])((Microsoft.AspNetCore.Mvc.ObjectResult)filterContext.Result).Value)[1];
            //_logger.LogWarning(((string[])((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value)[1]);
            //base.OnResultExecuted(context);
        }
    }
}
