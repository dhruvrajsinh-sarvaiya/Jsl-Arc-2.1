using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ApiModels
{
    public class BizResponseClass
    {
        public byte ReturnCode { get; set; }

        public string ReturnMsg { get; set; }

        public int ErrorCode { get; set; }

        //public string RefreshToken { get; set; }

        //public string AccessToken { get; set; }

        //public string IDToken { get; set; }
    }
}
