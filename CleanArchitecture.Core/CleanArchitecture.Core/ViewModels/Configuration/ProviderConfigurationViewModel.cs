using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ProviderConfigurationViewModel
    {
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string AppKey { get; set; }

        [Required]
        [StringLength(50)]
        public string APIKey { get; set; }

        [Required]
        [StringLength(50)]
        public string SecretKey { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }
    }
}
