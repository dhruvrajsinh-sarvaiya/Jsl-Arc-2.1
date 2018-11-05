using CleanArchitecture.Core.ViewModels.KYC;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.KYC
{
    public interface IKYCLevelMaster
    {
        List<KYCLevelViewModel> GetKYCLevelData();
    }
}
