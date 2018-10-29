using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Entities.UserChangeLog
{
    public class UserLogChange : BizBase
    {
        public long UserId { get; set; }
        public string Type { get; set; }
        public string Oldvalue { get; set; }
        public string Newvalue { get; set; }
    }
}
