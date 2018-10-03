using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities.User
{
    public class RegisterType : BizBase
    {
        public string Type { get; set; }

        public bool ActiveStatus { get; set; }

        public bool IsDeleted { get; set; }

    }
}
