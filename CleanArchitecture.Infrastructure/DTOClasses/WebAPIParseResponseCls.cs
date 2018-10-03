﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ApiModels
{
    public class WebAPIParseResponseCls
    {        
        public decimal Balance { get; set; }
        public int Status { get; set; }
        public string StatusMsg { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMsg { get; set; }
        public string ErrorCode { get; set; }
        public string TrnRefNo { get; set; }
        public string OperatorRefNo { get; set; }             
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
    }

  
}
