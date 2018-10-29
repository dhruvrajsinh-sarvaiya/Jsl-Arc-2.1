using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.ManageViewModels.UserChangeLog
{
  public  class UserChangeLogViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Type   { get; set; }
        public string Oldvalue { get; set; }
        public string Newvalue { get; set; }
       
    }
}
