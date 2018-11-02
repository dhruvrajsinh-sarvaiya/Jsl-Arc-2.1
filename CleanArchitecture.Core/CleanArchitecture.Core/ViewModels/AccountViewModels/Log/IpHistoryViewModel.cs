using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.Log
{
    public class IpHistoryViewModel
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public string IpAddress { get; set; }
        public string Location { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
