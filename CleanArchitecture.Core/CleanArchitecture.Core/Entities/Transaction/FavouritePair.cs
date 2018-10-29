using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Entities.Transaction
{
    public class FavouritePair : BizBase
    {
        public long UserId { get; set; }
        public long PairId { get; set; }
    }
}
