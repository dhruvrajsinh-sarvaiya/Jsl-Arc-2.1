using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ApiModels
{
    public class CommunicationProviderList
    {
        public string  SenderID { get; set; }
        public string  SMSSendURL { get; set; }
        public int  Priority { get; set; }
        public string  SMSBalURL { get; set; }
        public long  RequestID { get; set; }
        public string  RequestFormat { get; set; }
        public string  contentType { get; set; }
        public string  MethodType { get; set; }
        public string  ServiceName { get; set; }
        public string  UserID { get; set; }
        public string  Password { get; set; }
        public decimal  Balance { get; set; }
    }
}
