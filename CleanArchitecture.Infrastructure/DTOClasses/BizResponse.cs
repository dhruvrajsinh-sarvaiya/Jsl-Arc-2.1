using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.Enums;

namespace CleanArchitecture.Infrastructure.DTOClasses
{
    public class BizResponse
    {
        public enResponseCode ReturnCode { get; set; }
        public enErrorCode ErrorCode { get; set; }
        public string ReturnMsg { get; set; }
    }
}
