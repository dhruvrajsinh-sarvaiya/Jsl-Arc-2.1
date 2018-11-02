using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.ViewModels.Transaction;
using CleanArchitecture.Infrastructure.DTOClasses;
using CleanArchitecture.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services.Transaction
{
    public class CancelOrderProcess : ICancelOrderProcess
    {
        public BizResponse ProcessCancelOrder(CancelOrderRequest Req)
        {
            try
            {
                BizResponse Res = new BizResponse();
                return Res;
            }
            catch(Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
    }
}
