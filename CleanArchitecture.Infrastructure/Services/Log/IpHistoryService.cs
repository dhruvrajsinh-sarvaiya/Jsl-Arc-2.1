using CleanArchitecture.Core.Entities.Log;
using CleanArchitecture.Core.Interfaces.Log;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.AccountViewModels.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services.Log
{
    public class IpHistoryService : IipHistory
    {
        private readonly ICustomRepository<IpHistory> _ipHistoryRepository;

        public IpHistoryService(ICustomRepository<IpHistory> ipHistoryRepository)
        {
            _ipHistoryRepository = ipHistoryRepository;
        }

        public long AddIpHistory(IpHistoryViewModel model)
        {
            try
            {
                var IpHistory = new IpHistory()
                {
                    UserId = model.UserId,
                    IpAddress = model.IpAddress,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = model.UserId,
                    Status = 0,
                    Location = model.Location
                };
                _ipHistoryRepository.Insert(IpHistory);
                return IpHistory.Id;
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }

        public List<IpHistoryViewModel> GetIpHistoryListByUserId(long UserId, int pageIndex, int pageSize)
        {
            try
            {
                var IpHistoryList = _ipHistoryRepository.Table.Where(i => i.UserId == UserId).ToList();
                if (IpHistoryList == null)
                {
                    return null;
                }

                var IpList = new List<IpHistoryViewModel>();
                foreach (var item in IpHistoryList)
                {
                    IpHistoryViewModel model = new IpHistoryViewModel();
                    model.IpAddress = item.IpAddress;
                    model.Location = item.Location;
                    model.CreatedDate = item.CreatedDate;
                    IpList.Add(model);
                }
                //return IpList;

                var total = IpList.Count();
                //var pageSize = 10; // set your page size, which is number of records per page

                //var page = 1; // set current page number, must be >= 1
                if (pageIndex == 0)
                {
                    pageIndex = 1;
                }

                if (pageSize == 0)
                {
                    pageSize = 10;
                }

                var skip = pageSize * (pageIndex - 1);

                //var canPage = skip < total;

                //if (canPage) // do what you wish if you can page no further
                //    return null;

                return IpList.Skip(skip).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }
    }
}
