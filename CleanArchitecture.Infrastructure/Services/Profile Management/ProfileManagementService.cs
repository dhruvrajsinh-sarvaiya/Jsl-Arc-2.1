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
    public class ProfileManagementService : IProfileMaster
    {
        private readonly ICustomRepository<ProfileMaster> _profileRepository;
        private readonly ICustomRepository<Typemaster> _typemasterRepository;
        private readonly ICustomRepository<SubscriptionMaster> _subscriptionRepository;
        public ProfileManagementService(ICustomRepository<ProfileMaster> profileRepository, ICustomRepository<Typemaster> typemasterRepository
            , ICustomRepository<SubscriptionMaster> subscriptionRepository)
        {
            _profileRepository = profileRepository;
            _typemasterRepository = typemasterRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        public List<ProfileMasterViewModel> GetProfileData(int userid)
        {
            try
            {
                var ProfileDataList = _profileRepository.Table.OrderBy(i => i.Level).ToList();
                List<ProfileMasterViewModel> listmodel = new List<ProfileMasterViewModel>();
                if (ProfileDataList != null)
                {
                    foreach (var item in ProfileDataList)
                    {
                        ProfileMasterViewModel model = new ProfileMasterViewModel();
                        model.Description = item.Description;
                        model.LevelName = item.LevelName;
                        model.Price = item.Price;
                        model.TypeId = _typemasterRepository.Table.Where(i => i.Id == item.TypeId).FirstOrDefault().SubType;
                        model.DepositFee = item.DepositFee;
                        model.Withdrawalfee = item.Withdrawalfee;
                        model.Tradingfee = item.Tradingfee;
                        model.WithdrawalLimit = item.WithdrawalLimit;
                        long profileid = 0;
                        profileid = _subscriptionRepository.Table.Where(s => s.UserId == userid && s.ActiveStatus == true).FirstOrDefault().ProfileId;
                        if (item.Id == profileid)
                            model.ActiveStatus = true;
                        else
                            model.ActiveStatus = false;
                        model.ProfileId = item.Id;
                        listmodel.Add(model);
                    }
                }

                return listmodel;
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }

    }
}
