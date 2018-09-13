using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;

namespace CleanArchitecture.Core.Entities
{
    public  class CommAPIServiceMaster : BizBase
    {
        public long APID { get; set; }
        public long CommServiceID { get; set; }
        public long SenderID { get; set; }
        public string SMSSendURL { get; set; }
        public string SMSBalURL { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string Balance { get; set; }
    }
}
