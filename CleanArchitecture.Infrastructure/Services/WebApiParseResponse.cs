using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services
{
    //Common Parsing method Implement Here
    class WebApiParseResponse : IWebApiParseResponse<WebAPIParseResponse>
    {
        public WebAPIParseResponse TransactionParseResponse(string TransactionResponse, long ThirPartyAPIID)
        {
            string test = "test conflictpo";
            throw new NotImplementedException();
        }
    }
}
