using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class MessagingQueueRes
    {
        public short Status { get; set; }
        public long MobileNo { get; set; }
        public string SMSDate { get; set; }
        public string SMSText { get; set; }
        public string StrStatus { get; set; }
    }

    public class ListMessagingQueueRes : BizResponseClass
    {
       public  List<MessagingQueueRes> MessagingQueueObj { get; set; }
    }
}
