using CleanArchitecture.Core.Entities.Complaint;
using CleanArchitecture.Core.ViewModels.Complaint;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.Complaint
{
   public interface ICompainTrail
    {
        long AddCompainTrail(CompainTrailVirewModel compainTrail);
    }
}
