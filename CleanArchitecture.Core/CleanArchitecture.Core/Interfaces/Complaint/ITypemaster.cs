using CleanArchitecture.Core.ViewModels.Complaint;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.Complaint
{
   public interface ITypemaster
    {
        List<TypemasterViewModel> GettypeMaster(string Type);
    }
}
