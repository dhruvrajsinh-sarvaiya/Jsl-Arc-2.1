using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.Log
{
    public class IpMasterGetViewModel
    {
        public string IpAliasName { get; set; }
        public string IpAddress { get; set; }
        public short Status { get; set; }
        public bool IsEnable { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
