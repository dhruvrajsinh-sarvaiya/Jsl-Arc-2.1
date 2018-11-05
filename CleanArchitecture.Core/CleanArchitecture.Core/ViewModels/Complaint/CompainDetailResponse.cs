using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Complaint
{
    public class CompainDetailResponse  
    {
        public long CompainNumber { get; set; }
        public string SubType { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Complainstatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
