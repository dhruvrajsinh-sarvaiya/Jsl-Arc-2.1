using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ProviderTypeViewModel
    {
        public long Id { get; set; }

        [Required]
        [StringLength(20)]
        public string ServiveProType { get; set; }

        public short Status { get; set; }
    }
}
