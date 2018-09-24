using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Core.Entities
{
    public partial class ApplicationUserPhotos : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public int ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
