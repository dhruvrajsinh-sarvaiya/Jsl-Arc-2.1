using CleanArchitecture.Core.ViewModels.Profile_Management;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.Profile_Management
{
    public interface IProfileMaster
    {
        //List<ProfileMasterViewModel> GetIpHistoryListByUserId(long UserId, int pageIndex, int pageSize);
        List<ProfileMasterViewModel> GetProfileData(int userid);
    }
}
