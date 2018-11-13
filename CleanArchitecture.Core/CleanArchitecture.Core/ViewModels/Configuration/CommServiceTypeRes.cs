using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class CommServiceTypeRes: BizResponseClass
    {
       public List<CommServiceTypeMaster> Response { get; set; }
    }
}
