using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class AppTypeViewModel
    {
        public long Id { get; set; }

        [Required]
        [StringLength(20)]
        public String AppTypeName { get; set; }
    }
}
