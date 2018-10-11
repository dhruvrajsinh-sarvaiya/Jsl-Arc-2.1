using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class DemonconfigurationViewModel
    {
        public long Id { get; set; }

        [Required]
        [StringLength(15)]
        public String IPAdd { get; set; }

        [Required]
        public int PortAdd { get; set; }

        [Required]
        [StringLength(200)]
        [DataType(DataType.Url)]
        public string Url { get; set; }
    }
}
