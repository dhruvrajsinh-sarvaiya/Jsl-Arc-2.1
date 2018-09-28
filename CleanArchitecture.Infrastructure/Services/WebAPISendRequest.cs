using CleanArchitecture.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    //For All type of Web Request
    class WebAPISendRequest : IWebApiSendRequest
    {
        public Task<string> SendAPIRequestAsync(string Url, string Request, string MethodType = "POST")
        {
            string Response = "";
            return Task.FromResult(Response);
        }       
    }
}
