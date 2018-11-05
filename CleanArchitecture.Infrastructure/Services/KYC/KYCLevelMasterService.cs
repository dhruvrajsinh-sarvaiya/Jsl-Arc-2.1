using CleanArchitecture.Core.Entities.KYC;
using CleanArchitecture.Core.Interfaces.KYC;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.KYC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.KYC
{
    public class KYCLevelMasterService : IKYCLevelMaster
    {
        private readonly ICustomRepository<KYCLevelMaster> _kyclevelRepository;

        public KYCLevelMasterService(ICustomRepository<KYCLevelMaster> kyclevelRepository)
        {
            _kyclevelRepository = kyclevelRepository;
        }

        public List<KYCLevelViewModel> GetKYCLevelData()
        {
            try
            {
                var KYCLevelDataList = _kyclevelRepository.Table.OrderBy(i => i.Level).ToList();
                List<KYCLevelViewModel> listmodel = new List<KYCLevelViewModel>();
                if (KYCLevelDataList != null)
                {
                    foreach (var item in KYCLevelDataList)
                    {
                        KYCLevelViewModel model = new KYCLevelViewModel();
                        model.KYCName = item.KYCName;
                        model.Level = item.Level;
                        model.EnableStatus = item.EnableStatus;
                        model.IsDelete = item.IsDelete;
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
