using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Core.Entities
{
    public partial class ApplicationUser : IdentityUser<int>
    {       
        public bool IsEnabled { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        [StringLength(250)]
        public string FirstName { get; set; }
        [StringLength(250)]
        public string LastName { get; set; }
        [Phone]
        public string Mobile { get; set; }

        [DataType("decimal(18,2)")]
        public decimal Balance { get; set; }

        [Required]
        [Range(6, Int64.MaxValue)]
        public long OTP { get; set; }

        public ApplicationUserPhotos ProfilePhoto { get; set; }

        [NotMapped]
        public string Name
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
    }
}
