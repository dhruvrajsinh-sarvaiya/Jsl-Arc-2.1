using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp
{
   public class UserConfigurationMasterViewModel : BizBase
    {
        public int UserId { get; set; }

        public string Type { get; set; }

        public string ConfigurationValue { get; set; }

        public bool EnableStatus { get; set; }
    }
}
