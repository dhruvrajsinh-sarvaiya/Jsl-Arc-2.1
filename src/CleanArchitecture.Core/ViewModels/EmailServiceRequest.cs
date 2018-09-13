using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels
{
    public class EmailServiceRequest
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(1024, MinimumLength = 5)]
        public string Message { get; set; }
    }
}
