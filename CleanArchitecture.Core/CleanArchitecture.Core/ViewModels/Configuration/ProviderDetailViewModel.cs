using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ProviderDetailViewModel
    {
        public long Id { get; set; }

        [Required]
        public long ServiceProID { get; set; }

        [Required]
        public long ProTypeID { get; set; }

        [Required]
        public long AppTypeID { get; set; }

        [Required]
        public long TrnTypeID { get; set; }

        [Required]
        public long LimitID { get; set; }

        public long DemonConfigID { get; set; }

        [Required]
        public long ServiceProConfigID { get; set; }

        public long ThirPartyAPIID { get; set; }
    }
}
