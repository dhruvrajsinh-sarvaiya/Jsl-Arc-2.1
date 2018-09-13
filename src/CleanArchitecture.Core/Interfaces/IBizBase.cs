﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IBizBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         int Id { get; set; }

         DateTime CreatedDate { get; set; }

         DateTime? UpdatedDate { get; set; }

         long CreatedBy { get; set; }

         long? UpdatedBy { get; set; }

        int Status { get; set; }
    }
}