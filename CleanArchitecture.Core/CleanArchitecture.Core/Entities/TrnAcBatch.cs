using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.Helpers;


namespace CleanArchitecture.Core.Entities
{
    public class TrnAcBatch : BizBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public new long Id { get; set; }
       
        public TrnAcBatch()
        {

        }
        public TrnAcBatch(DateTime d1)
        {
            CreatedBy = 900;
            CreatedDate = d1;
            UpdatedBy = 900;
            Status = 1;            
            Id = Helpers.Helpers.GenerateBatch();
        }
    }
}
