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
        int SetActiveService(long ServiceId);
        int SetInActiveService(long ServiceId);

        IEnumerable<ServiceProviderViewModel> GetAllProvider();
        ServiceProviderViewModel GetPoviderByID(long ID);
        long AddProviderService(ServiceProviderRequest request);
        bool UpdateProviderService(ServiceProviderRequest request);
        bool SetActiveProvider(long id);
        bool SetInActiveProvider(long id);

        IEnumerable<AppTypeViewModel> GetAppType();
        AppTypeViewModel GetAppTypeById(long id);
        long AddAppType(AppTypeRequest request);
        bool UpdateAppType(AppTypeRequest request);
        bool SetActiveAppType(long id);
        bool SetInActiveAppType(long id);

        IEnumerable<ProviderTypeViewModel> GetProviderType();
        ProviderTypeViewModel GetProviderTypeById(long id);
        long AddProviderType(ProviderTypeRequest request);
        bool UpdateProviderType(ProviderTypeRequest request);
        bool SetActiveProviderType(long id);
        bool SetInActiveProviderType(long id);

        ProviderConfigurationViewModel GetProviderConfiguration(long id);
        long AddProviderConfiguration(ProviderConfigurationRequest request);
        bool SetActiveProviderConfiguration(long id);
        bool SetInActiveProviderConfiguration(long id);
        bool UpdateProviderConfiguration(ProviderConfigurationRequest request);

        DemonconfigurationViewModel GetDemonConfiguration(long id);
        long AddDemonConfiguration(DemonConfigurationRequest request);
        bool UpdateDemonConfiguration(DemonConfigurationRequest request);
        bool SetActiveDemonConfig(long id);
        bool SetInActiveDemonConfig(long id);

        IEnumerable<ProviderDetailViewModel> GetProviderDetailList();
        IEnumerable<ProviderDetailGetAllResponce> getProviderDetailsDataList(IEnumerable<ProviderDetailViewModel> dataList);
        ProviderDetailGetAllResponce getProviderDetailDataById(ProviderDetailViewModel viewModel);
        ProviderDetailViewModel GetProviderDetailById(long id);
        long AddProviderDetail(ProviderDetailRequest request);
        bool UpdateProviderDetail(ProviderDetailRequest request);
        bool SetActiveProviderDetail(long id);
        bool SetInActiveProviderDetail(long id);

        long AddProductConfiguration(ProductConfigurationRequest Request);
        long UpdateProductConfiguration(ProductConfigurationRequest Request);
        ProductConfigrationGetInfo GetProductConfiguration(long ProductId);
        List<ProductConfigrationGetInfo> GetAllProductConfiguration();
        int SetActiveProduct(long ProductId);
        int SetInActiveProduct(long ProductId);

        long AddRouteConfiguration(RouteConfigurationRequest Request);
        long UpdateRouteConfiguration(RouteConfigurationRequest Request);
        RouteConfigurationRequest GetRouteConfiguration(long RouteId);
        List<RouteConfigurationRequest> GetAllRouteConfiguration();
        int SetActiveRoute(long RouteId);
        int SetInActiveRoute(long RouteId);

        List<ThirdPartyAPIConfigViewModel> GetAllThirdPartyAPIConfig();
        ThirdPartyAPIConfigViewModel GetThirdPartyAPIConfigById(long Id);
        long AddThirdPartyAPI(ThirdPartyAPIConfigRequest request);
        bool UpdateThirdPartyAPI(ThirdPartyAPIConfigRequest request);
        bool SetActiveThirdPartyAPI(long id);
        bool SetInActiveThirdPartyAPI(long id);

        List<ThirdPartyAPIResponseConfigViewModel> GetAllThirdPartyAPIResponse();
        ThirdPartyAPIResponseConfigViewModel GetThirdPartyAPIResponseById(long id);
        long AddThirdPartyAPIResponse(ThirdPartyAPIResponseConfigViewModel Request);
        bool UpdateThirdPartyAPIResponse(ThirdPartyAPIResponseConfigViewModel Request);
        bool SetActiveThirdPartyAPIResponse(long id);
        bool SetInActiveThirdPartyAPIResponse(long id);

        long AddPairConfiguration(TradePairConfigRequest Request);
        long UpdatePairConfiguration(TradePairConfigRequest Request);
        TradePairConfigRequest GetPairConfiguration(long PairId);
        List<TradePairConfigRequest> GetAllPairConfiguration();
        int SetActivePair(long PairId);
        int SetInActivePair(long PairId);

        List<ServiceTypeMasterInfo> GetAllServiceTypeMaster();
        List<TransactionTypeInfo> GetAllTransactionType();

        List<LimitViewModel> GetAllLimitData();
        LimitViewModel GetLimitById(long id);
        long AddLimitData(LimitRequest Request);
        bool UpdateLimitData(LimitRequest Request);
        bool SetActiveLimit(long id);
        bool SetInActiveLimit(long id);
    }
}
