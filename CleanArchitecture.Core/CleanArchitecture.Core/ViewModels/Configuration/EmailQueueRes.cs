using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class EmailQueueRes
    {
        public short Status { get; set; }
        public string RecepientEmail { get; set; }
        public string  EmailDate { get; set; }
        public string Body { get; set; }
        public string StrStatus { get; set; }
        public string Subject { get; set; }
        public string Attachment { get; set; }
        public string EmailType { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
    }
    public class ListEmailQueueRes : BizResponseClass
    {
        public List<EmailQueueRes> EmailQueueObj { get; set; }
    }
}
