using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.SharedKernel;


namespace CleanArchitecture.Core.Entities
{
    public class TrnAcBatch : BizBase
    {
        public TrnAcBatch()
        {

        }
        public TrnAcBatch(DateTime d1)
        {
            CreatedBy = 900;
            CreatedDate = d1;
            UpdatedBy = 900;
            Status = 1;
        }
    }
}
