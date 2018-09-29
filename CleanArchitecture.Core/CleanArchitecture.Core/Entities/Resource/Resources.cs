using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Core.Entities.Culture;

namespace CleanArchitecture.Core.Entities.Resource
{
    public partial class Resources
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }        

        public Cultures Culture { get; set; }
    }
}
