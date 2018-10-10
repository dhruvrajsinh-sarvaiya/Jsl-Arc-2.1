using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ServiceProviderViewModel
    {
        public long Id { get; set; }

        [Required]
        [StringLength(60)]
        public string ProviderName { get; set; }
        
        public short Status { get; set; }
    }
}
