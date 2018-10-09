using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Configuration
{
    public class ServiceProConfiguration : BizBase
    {
        [Required]
        public long ServiceProID { get; set; }

        public string AppKey { get; set; }
        public string APIKey { get; set; }
        public string SecretKey { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}   
