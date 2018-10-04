using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.Enums;
namespace CleanArchitecture.Core.ApiModels
{
    public class BizResponseClass
    {
        public enResponseCode ReturnCode { get; set; }

        public string ReturnMsg { get; set; }

        public enErrorCode ErrorCode { get; set; }

        //public short StatusCode { get; set; }

        //public string StatusMessage { get; set; }

        //public string RefreshToken { get; set; }

        //public string AccessToken { get; set; }

        //public string IDToken { get; set; }
    }
}
