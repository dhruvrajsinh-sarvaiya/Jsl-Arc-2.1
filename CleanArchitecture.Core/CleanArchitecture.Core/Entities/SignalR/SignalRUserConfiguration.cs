using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Entities.SignalR
{
    public class SignalRUserConfiguration
    {
        public string ClientId { get; set; }
        public string grant_type { get; set; }
        public string password { get; set; }
        public string Scope { get; set; }
        public string username { get; set; }
    }
}
