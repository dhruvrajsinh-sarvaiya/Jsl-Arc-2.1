using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Complaint
{
    public class TypemasterViewModel
    {
        public string Type { get; set; }
        public string SubType { get; set; }
        public bool EnableStatus { get; set; }
    }

    public class TypeMasterResponse : BizResponseClass
    {
        public List<TypemasterViewModel> TypeMasterList { get; set; }
    }
}
