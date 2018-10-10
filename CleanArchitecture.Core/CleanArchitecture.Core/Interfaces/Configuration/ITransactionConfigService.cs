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
        long AddProviderService(ServiceProviderRequest request);
        bool UpdateProviderService(ServiceProviderRequest request);

        IEnumerable<AppTypeViewModel> GetAppType();
        AppTypeViewModel GetAppTypeById(long id);
        long AddAppType(AppTypeRequest request);
        bool UpdateAppType(AppTypeRequest request);

        IEnumerable<ProviderTypeViewModel> GetProviderType();
        ProviderTypeViewModel GetProviderTypeById(long id);
        long AddProviderType(ProviderTypeRequest request);
        bool UpdateProviderType(ProviderTypeRequest request);
    }
}
