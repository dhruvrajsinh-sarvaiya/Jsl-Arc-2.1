using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.Configuration
{
    public interface ITransactionConfigService
    {
        long AddServiceConfiguration(ServiceConfigurationRequest Request);
        long UpdateServiceConfiguration(ServiceConfigurationRequest Request);
        ServiceConfigurationRequest GetServiceConfiguration(long ServiceId);
        List<ServiceConfigurationRequest> GetAllServiceConfiguration();
        int SetActiveService(int ServiceId);
        int SetInActiveService(int ServiceId);

        IEnumerable<ServiceProviderViewModel> GetAllProvider();
        ServiceProviderViewModel GetPoviderByID(long ID);
        bool AddProviderService(ServiceProviderRequest request);

        IEnumerable<AppTypeViewModel> GetAppType();
        AppTypeViewModel GetAppTypeById(long id);

        IEnumerable<ProviderTypeViewModel> GetProviderType();
        ProviderTypeViewModel GetProviderTypeById(long id);
    }
}
