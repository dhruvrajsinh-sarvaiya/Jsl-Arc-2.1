using CleanArchitecture.Core.Entities.Complaint;
using CleanArchitecture.Core.Entities.Profile_Management;
using CleanArchitecture.Core.Interfaces.Profile_Management;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.Profile_Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.Profile_Management
{
    public class SubscriptionMasterService : ISubscriptionMaster
    {
        private readonly ICustomRepository<SubscriptionMaster> _subscriptionRepository;
        private readonly ICustomRepository<ProfileMaster> _profileRepository;

        public SubscriptionMasterService(ICustomRepository<SubscriptionMaster> subscriptionRepository, ICustomRepository<ProfileMaster> profileRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _profileRepository = profileRepository;
        }

        public long AddSubscription(SubscriptionViewModel model)
        {
            try
            {
                var Subscription = new SubscriptionMaster()
                {
                    UserId = model.UserId,
                    ProfileId = _profileRepository.Table.Where(i => i.Level == 1).FirstOrDefault().Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = model.UserId,
                    Status = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddYears(1),
                    ActiveStatus = true
                };
                _subscriptionRepository.Insert(Subscription);
                return Subscription.Id;
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }
    }
}
