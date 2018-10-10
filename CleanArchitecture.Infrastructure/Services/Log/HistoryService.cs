using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.Log;
using CleanArchitecture.Core.Interfaces.Log;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.AccountViewModels.Log;

namespace CleanArchitecture.Infrastructure.Services.Log
{
    public class HistoryService : IHistoryService
    {       
        private readonly ICustomRepository<HistoryMaster> _historyMasterRepository;

        public HistoryService(ICustomRepository<HistoryMaster> historyMasterRepository)
        {
            _historyMasterRepository = historyMasterRepository;
        }

        public async Task<long> AddHistory(HistoryViewModel model)
        {
            var currentHistory = new HistoryMaster
            {
                UserId = model.UserId,
                HistoryTypeId = model.HistoryTypeId,
                ServiceUrl=model.ServiceUrl,
                IpId = model.IpId,
                DeviceId = model.DeviceId,
                Mode = model.Mode,
                HostName = model.HostName,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = model.UserId,
                Status = 0,

            };
            _historyMasterRepository.Insert(currentHistory);
            //_dbContext.SaveChanges();

            return currentHistory.Id;
        }
    }
}
