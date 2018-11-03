using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.Log
{
   public class LoginhistoryViewModel
    {
        public long Id { get; set; }
        public int UserId { get; set; }

        public string IpAddress { get; set; }
        public string Device { get; set; }
        public string Location { get; set; }
    }

    public class LoginHistoryDataViewModel
    {
        public string IpAddress { get; set; }
        public string Device { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
    }


    public class LoginHistoryResponse : BizResponseClass
    {
        public int TotalRow { get; set; }
        public List<LoginHistoryDataViewModel> LoginHistoryList { get; set; }
    }
}
