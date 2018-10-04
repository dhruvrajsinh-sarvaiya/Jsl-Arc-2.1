using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class ActiveCreditsResponce 
    {
        public long id { get; set; }
        public string currency { get; set; }
        public decimal rate { get; set; }
        public long period { get; set; }
        public string direction { get; set; }
        public String timestamp { get; set; }
        public bool is_live { get; set; }
        public bool is_cancelled { get; set; }
        public decimal original_amount { get; set; }
        public decimal remaining_amount { get; set; }
        public decimal executed_amount { get; set; }
    }
}
