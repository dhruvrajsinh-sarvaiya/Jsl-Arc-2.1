using CleanArchitecture.Core.ViewModels.AccountViewModels.Log;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.Log
{
    public interface IipHistory
    {
        long AddIpHistory(IpHistoryViewModel model);
        List<IpHistoryViewModel> GetIpHistoryListByUserId(long UserId, int pageIndex, int pageSize);
    }
}
