using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class ActiveCreditsResponce
    {
        public int id { get; set; }
        public string currency { get; set; }
        public string rate { get; set; }
        public int period { get; set; }
        public string direction { get; set; }
        public string timestamp { get; set; }
        public bool is_live { get; set; }
        public bool is_cancelled { get; set; }
        public string original_amount { get; set; }
        public string remaining_amount { get; set; }
        public string executed_amount { get; set; }
    }
}
