using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Infrastructure.DTOClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.Transaction
{
    public class NewTransaction
    {
        BizResponseClass _Resp;
        BizResponseClass _CreateTransactionResp;
        public NewTransaction()
        {

        } 
        
        public BizResponseClass ProcessNewTransaction(NewTransactionRequest Req)
        {
            _Resp = new BizResponseClass();



            return null;
        }
    }
}
