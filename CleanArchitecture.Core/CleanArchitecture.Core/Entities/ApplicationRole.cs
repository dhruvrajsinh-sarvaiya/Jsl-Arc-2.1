using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Core.Entities
{
    public partial class ApplicationRole : IdentityRole<int>
    {    

        [StringLength(250)]
        public string Description { get; set; }
    }
}
