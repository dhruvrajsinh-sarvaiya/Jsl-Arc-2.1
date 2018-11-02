using CleanArchitecture.Core.Entities.Log;
using CleanArchitecture.Core.Interfaces.Log;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.AccountViewModels.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.Log
{
    public class LoginHistoryService : ILoginHistory
    {
        private readonly ICustomRepository<LoginHistory> _CustomLoginRepository;
        public LoginHistoryService(ICustomRepository<LoginHistory> customRepository)
        {
            _CustomLoginRepository = customRepository;
        }

        public List<LoginhistoryViewModel> GetLoginHistoryByUserId(long UserId, int pageIndex, int pageSize)
        {
            try
            {
                var LoginHistoryList = _CustomLoginRepository.Table.Where(i => i.UserId == UserId).ToList();
                if (LoginHistoryList == null)
                {
                    return null;
                }

                var LoginHistory = new List<LoginhistoryViewModel>();
                foreach (var item in LoginHistoryList)
                {
                    LoginhistoryViewModel loginhistoryViewModel = new LoginhistoryViewModel()
                    {
                        Id = item.Id,
                        Device = item.Device,
                        IpAddress = item.IpAddress,
                        Location = item.Location,
                        UserId = item.UserId
                    };
                    LoginHistory.Add(loginhistoryViewModel);
                                        
                }

                var total = LoginHistory.Count();
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

                return LoginHistory.Skip(skip).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }

        }

        public long LoginHistory(LoginhistoryViewModel model)
        {
            try
            {
                var LoginHistory = new LoginHistory()
                {
                    UserId = model.UserId,
                    IpAddress = model.IpAddress,
                    Device = model.Device,
                    Location = model.Location,
                    Status = 0,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = model.UserId
                };
                _CustomLoginRepository.Insert(LoginHistory);
                return LoginHistory.Id;
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }

        }
    }
}
