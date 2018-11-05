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

        public long AddMultiSubscription(int UserId, long ProfileId)
        {
            var Subscription = new SubscriptionMaster()
            {
                UserId = UserId,
                ProfileId = ProfileId,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = UserId,
                Status = 0,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddYears(1),
                ActiveStatus = true
            };
            _subscriptionRepository.Insert(Subscription);
            return Subscription.Id;
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

        public bool GetSubscriptionData(int UserId, long ProfileId)
        {
            try
            {
                bool Status = false;
                var Subscription = _subscriptionRepository.Table.Where(i => i.ProfileId == ProfileId && i.UserId == UserId && i.ActiveStatus == true).FirstOrDefault();
                if (Subscription == null)
                    Status = true;
                else
                    Status = false;
                if (Status)
                {
                    int Level = _profileRepository.Table.Where(i => i.Id == ProfileId).FirstOrDefault().Level;

                    var SubscriptionData = (from pf in _profileRepository.Table
                                            join ss in _subscriptionRepository.Table on pf.Id equals ss.ProfileId
                                            where ss.UserId == UserId
                                            select new { ProfileId = pf.Id, ProlfileLevel = pf.Level }).OrderBy(pf => pf.ProlfileLevel).ToList();
                    if (SubscriptionData != null)
                    {
                        foreach (var item in SubscriptionData)
                        {
                            if (item.ProlfileLevel > Level)
                            {
                                Status = false;
                                break;
                            }
                            else
                                Status = true;
                        }
                    }
                    return Status;
                }
                else
                {
                    return Status;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }
    }
}
