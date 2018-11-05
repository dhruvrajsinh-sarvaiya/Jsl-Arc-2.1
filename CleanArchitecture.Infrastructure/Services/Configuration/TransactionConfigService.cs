using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Communication;
using CleanArchitecture.Core.Entities.Configuration;
using CleanArchitecture.Core.Entities.Transaction;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Configuration;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.Configuration;
using CleanArchitecture.Core.ViewModels.WalletConfiguration;
using CleanArchitecture.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace CleanArchitecture.Infrastructure.Services.Configuration
{
    public class TransactionConfigService : ITransactionConfigService
    {
        private readonly ICommonRepository<ServiceMaster> _serviceMasterRepository;
        private readonly ICommonRepository<ServiceDetail> _serviceDetailRepository;
        private readonly ICommonRepository<ServiceStastics> _serviceStasticsRepository;
        private readonly ICommonRepository<ServiceProviderMaster> _ServiceProviderMaster;
        private readonly ICommonRepository<AppType> _ApptypeRepository;
        private readonly ICommonRepository<ServiceProviderType> _ProviderTypeRepository;
        private readonly ICommonRepository<ServiceProConfiguration> _ProviderConfiguration;
        private readonly ICommonRepository<DemonConfiguration> _DemonRepository;
        private readonly ICommonRepository<ServiceProviderDetail> _ProDetailRepository;
        private readonly ILogger<TransactionConfigService> _logger;
        private readonly ICommonRepository<ProductConfiguration> _productConfigRepository;
        private readonly ICommonRepository<CountryMaster> _countryMasterRepository;
        private readonly ICommonRepository<RouteConfiguration> _routeConfigRepository;
        private readonly ICommonRepository<ThirdPartyAPIConfiguration> _thirdPartyAPIRepository;
        private readonly ICommonRepository<ThirdPartyAPIResponseConfiguration> _thirdPartyAPIResRepository;
        private readonly ICommonRepository<TradePairMaster> _tradePairMasterRepository;
        private readonly ICommonRepository<TradePairDetail> _tradePairDetailRepository;
        private readonly ICommonRepository<Limits> _limitRepository;
        private readonly ICommonRepository<ServiceTypeMapping> _serviceTypeMapping;
        private readonly ICommonRepository<WalletTypeMaster> _walletTypeService;
        private readonly IWalletService _walletService;
        private readonly ICommonRepository<TradePairStastics> _tradePairStastics;
        private readonly ICommonRepository<Market> _marketRepository;
        private readonly IFrontTrnRepository _frontTrnRepository;

        public TransactionConfigService(
            ICommonRepository<ServiceMaster> serviceMasterRepository,
            ICommonRepository<ServiceDetail> serviceDetailRepository,
            ICommonRepository<ServiceStastics> serviceStasticsRepository,
            ILogger<TransactionConfigService> logger,
            ICommonRepository<ServiceProviderMaster> ServiceProviderMaster,
            ICommonRepository<AppType> ApptypeRepository,
            ICommonRepository<ServiceProviderType> ProviderTypeRepository,
            ICommonRepository<ServiceProConfiguration> ProviderConfiguration,
            ICommonRepository<DemonConfiguration> DemonRepository,
            ICommonRepository<ServiceProviderDetail> ProDetailRepository,
            ICommonRepository<ProductConfiguration> productConfigRepository,
            ICommonRepository<CountryMaster> countryMasterRepository,
            ICommonRepository<RouteConfiguration> routeConfigRepository,
            ICommonRepository<ThirdPartyAPIConfiguration> thirdPartyAPIRepository,
            ICommonRepository<ThirdPartyAPIResponseConfiguration> thirdPartyAPIResRepository,
            ICommonRepository<TradePairMaster> tradePairMasterRepository,
            ICommonRepository<TradePairDetail> tradePairDetailRepository,
            ICommonRepository<Limits> limitRepository,
            ICommonRepository<ServiceTypeMapping> serviceTypeMapping,
            ICommonRepository<WalletTypeMaster> walletTypeService,
            IWalletService walletService,
            ICommonRepository<TradePairStastics> tradePairStastics,
            ICommonRepository<Market> marketRepository,
            IFrontTrnRepository frontTrnRepository)
          {
            _serviceMasterRepository = serviceMasterRepository;
            _serviceDetailRepository = serviceDetailRepository;
            _serviceStasticsRepository = serviceStasticsRepository;
            _ServiceProviderMaster = ServiceProviderMaster;
            _ApptypeRepository = ApptypeRepository;
            _ProviderTypeRepository = ProviderTypeRepository;
            _ProviderConfiguration = ProviderConfiguration;
            _DemonRepository = DemonRepository;
            _ProDetailRepository = ProDetailRepository;
            _logger = logger;
            _productConfigRepository = productConfigRepository;
            _countryMasterRepository = countryMasterRepository;
            _routeConfigRepository = routeConfigRepository;
            _thirdPartyAPIRepository = thirdPartyAPIRepository;
            _thirdPartyAPIResRepository = thirdPartyAPIResRepository;
            _tradePairMasterRepository = tradePairMasterRepository;
            _tradePairDetailRepository = tradePairDetailRepository;
            _limitRepository = limitRepository;
            _serviceTypeMapping = serviceTypeMapping;
            _walletTypeService = walletTypeService;
            _walletService = walletService;
            _tradePairStastics = tradePairStastics;
            _marketRepository = marketRepository;
            _frontTrnRepository = frontTrnRepository;
        }

        #region Service
        public long AddServiceConfiguration(ServiceConfigurationRequest Request)
        {
            try
            {
                ServiceMaster serviceMaster = new ServiceMaster()
                {
                    Name = Request.Name,
                    SMSCode = Request.SMSCode,
                    ServiceType = Request.Type,
                    Status = Convert.ToInt16(ServiceStatus.Active),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = null
                };
                var newServiceMaster = _serviceMasterRepository.Add(serviceMaster);

                ServiceDetailJsonData serviceDetailJsonData = new ServiceDetailJsonData()
                {
                    ImageUrl = Request.ImageUrl,
                    TotalSupply = Request.TotalSupply,
                    MaxSupply = Request.MaxSupply,
                    ProofType = Request.ProofType,
                    Community = Request.Community,
                    Explorer = Request.Explorer,
                    EncryptionAlgorithm = Request.EncryptionAlgorithm,
                    WebsiteUrl = Request.WebsiteUrl,
                    WhitePaperPath = Request.WebsiteUrl,
                    Introduction = Request.Introduction,

                };

                ServiceDetail serviceDetail = new ServiceDetail()
                {
                    ServiceId = newServiceMaster.Id,
                    ServiceDetailJson = JsonConvert.SerializeObject(serviceDetailJsonData)
                };
                var newServiceDetail = _serviceDetailRepository.Add(serviceDetail);

                ServiceStastics serviceStastics = new ServiceStastics()
                {
                    ServiceId = newServiceMaster.Id,
                    IssueDate = Request.IssueDate,
                    IssuePrice = Request.IssuePrice,
                    MaxSupply = Request.MaxSupply,
                    CirculatingSupply = Request.CirculatingSupply,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = null
                };
                var newServiceStastics = _serviceStasticsRepository.Add(serviceStastics);
               
                var depositSerMapping = new ServiceTypeMapping
                {
                    ServiceId = newServiceMaster.Id,
                    TrnType = Convert.ToInt16(enTrnType.Deposit),
                    Status = Request.IsDeposit,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = null
                };
                var newdepositSerMapping = _serviceTypeMapping.Add(depositSerMapping);     
               
                var withdrawSerMapping = new ServiceTypeMapping
                {
                    ServiceId = newServiceMaster.Id,
                    TrnType = Convert.ToInt16(enTrnType.Withdraw),
                    Status = Request.IsWithdraw,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = null
                };
                var newwithdrawSerMapping = _serviceTypeMapping.Add(withdrawSerMapping);             
               
                var tranSerMapping = new ServiceTypeMapping
                {
                    ServiceId = newServiceMaster.Id,
                    TrnType = Convert.ToInt16(enTrnType.Transaction),
                    Status = Request.IsTransaction,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = null
                };
                var newtranSerMapping = _serviceTypeMapping.Add(tranSerMapping);

                //Add Default WalletType Master
                var walletTypeMaster = new WalletTypeMaster
                {
                    WalletTypeName = Request.SMSCode,
                    Discription = Request.SMSCode,
                    Status = Convert.ToInt16(ServiceStatus.Active),
                    IsDepositionAllow = Request.IsDeposit,
                    IsWithdrawalAllow = Request.IsWithdraw,
                    IsTransactionWallet = Request.IsTransaction,
                    CreatedBy = 1,
                    CreatedDate = DateTime.UtcNow
                };
                var newwalletTypeMaster = _walletTypeService.Add(walletTypeMaster);

                //Update WalletTypeId In ServiceMaster
                newServiceMaster.WalletTypeID = newwalletTypeMaster.Id;
                _serviceMasterRepository.Update(newServiceMaster);

                ////Add Into WalletMaster For Default Organization
                int[] AllowTrnType = new int[3] { Convert.ToInt16(enTrnType.Deposit), Convert.ToInt16(enTrnType.Withdraw), Convert.ToInt16(enTrnType.Transaction) };
                var walletMaster = _walletService.InsertIntoWalletMaster(" Default Org " + Request.SMSCode, Request.SMSCode,1, AllowTrnType, 1, null, 1);

                //Add BaseCurrency In MarketEntity
                if (Request.IsBaseCurrency == 1)
                {
                    var marketViewModel = new MarketViewModel { CurrencyName = Request.SMSCode, isBaseCurrency = 1, ServiceID = newServiceMaster.Id };
                    AddMarketData(marketViewModel);
                }

                return newServiceMaster.Id;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public long UpdateServiceConfiguration(ServiceConfigurationRequest Request)
        {
            try
            {
                var serviceMaster = _serviceMasterRepository.GetActiveById(Request.ServiceId);
                if (serviceMaster != null)
                {
                    serviceMaster.Name = Request.Name;
                    serviceMaster.SMSCode = Request.SMSCode;
                    serviceMaster.ServiceType = Request.Type;
                    serviceMaster.UpdatedBy = 1;
                    serviceMaster.UpdatedDate = DateTime.UtcNow;

                    _serviceMasterRepository.Update(serviceMaster);

                    ServiceDetailJsonData serviceDetailJsonData = new ServiceDetailJsonData()
                    {
                        ImageUrl = Request.ImageUrl,
                        TotalSupply = Request.TotalSupply,
                        MaxSupply = Request.MaxSupply,
                        ProofType = Request.ProofType,
                        Community = Request.Community,
                        Explorer = Request.Explorer,
                        EncryptionAlgorithm = Request.EncryptionAlgorithm,
                        WebsiteUrl = Request.WebsiteUrl,
                        WhitePaperPath = Request.WebsiteUrl,
                        Introduction = Request.Introduction
                    };

                    var serviceDetail = _serviceDetailRepository.GetSingle(service => service.ServiceId == Request.ServiceId);
                    serviceDetail.ServiceDetailJson = JsonConvert.SerializeObject(serviceDetailJsonData);
                    _serviceDetailRepository.Update(serviceDetail);

                    var serviceStastics = _serviceStasticsRepository.GetSingle(service => service.ServiceId == Request.ServiceId);
                    serviceStastics.IssueDate = Request.IssueDate;
                    serviceStastics.IssuePrice = Request.IssuePrice;
                    serviceStastics.MaxSupply = Request.MaxSupply;
                    serviceStastics.CirculatingSupply = Request.CirculatingSupply;
                    serviceStastics.UpdatedDate = DateTime.UtcNow;
                    serviceStastics.UpdatedBy = 1;
                    _serviceStasticsRepository.Update(serviceStastics);

                    var depositSerMapping = _serviceTypeMapping.GetSingle(x => x.ServiceId == Request.ServiceId && x.TrnType == Convert.ToInt16(enTrnType.Deposit));
                    if(depositSerMapping != null)
                    {
                        depositSerMapping.Status = Request.IsDeposit;
                        depositSerMapping.UpdatedBy = 1;
                        depositSerMapping.UpdatedDate = DateTime.UtcNow;
                        _serviceTypeMapping.Update(depositSerMapping);
                    }

                    var withdrawSerMapping = _serviceTypeMapping.GetSingle(x => x.ServiceId == Request.ServiceId && x.TrnType == Convert.ToInt16(enTrnType.Withdraw));
                    if (withdrawSerMapping != null)
                    {
                        withdrawSerMapping.Status = Request.IsWithdraw;
                        withdrawSerMapping.UpdatedBy = 1;
                        withdrawSerMapping.UpdatedDate = DateTime.UtcNow;
                        _serviceTypeMapping.Update(withdrawSerMapping);
                    }

                    var tranSerMapping = _serviceTypeMapping.GetSingle(x => x.ServiceId == Request.ServiceId && x.TrnType == Convert.ToInt16(enTrnType.Transaction));
                    if (tranSerMapping != null)
                    {
                        tranSerMapping.Status = Request.IsTransaction;
                        tranSerMapping.UpdatedBy = 1;
                        tranSerMapping.UpdatedDate = DateTime.UtcNow;
                        _serviceTypeMapping.Update(tranSerMapping);
                    }

                    var walletType = _walletTypeService.GetById(serviceMaster.WalletTypeID);
                    if(walletType != null)
                    {
                        walletType.IsDepositionAllow = Request.IsDeposit;
                        walletType.IsWithdrawalAllow = Request.IsWithdraw;
                        walletType.IsTransactionWallet = Request.IsTransaction;
                        tranSerMapping.UpdatedBy = 1;
                        tranSerMapping.UpdatedDate = DateTime.UtcNow;

                        _walletTypeService.Update(walletType);
                    }

                    return Request.ServiceId;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public List<ServiceConfigurationRequest> GetAllServiceConfiguration()
        {
            List<ServiceConfigurationRequest> responsedata;
            try
            {
                responsedata = new List<ServiceConfigurationRequest>();
                var ServiceData = _frontTrnRepository.GetAllServiceConfiguration();
                //var serviceMaster = _serviceMasterRepository.List();
                if (ServiceData != null && ServiceData.Count > 0)
                {
                    foreach (var service in ServiceData)
                    {
                        ServiceConfigurationRequest response = new ServiceConfigurationRequest();
                        response.ServiceId = service.ServiceId;
                        response.Name = service.ServiceName;
                        response.SMSCode = service.SMSCode;
                        response.Type = service.ServiceType;

                        //var serviceDetail = _serviceDetailRepository.GetSingle(ser => ser.ServiceId == service.Id);
                        var serviceDetailJson = JsonConvert.DeserializeObject<ServiceDetailJsonData>(service.ServiceDetailJson);

                        response.ImageUrl = serviceDetailJson.ImageUrl;
                        response.TotalSupply = serviceDetailJson.TotalSupply;
                        response.MaxSupply = serviceDetailJson.MaxSupply;
                        response.ProofType = serviceDetailJson.ProofType;
                        response.EncryptionAlgorithm = serviceDetailJson.EncryptionAlgorithm;
                        response.WebsiteUrl = serviceDetailJson.WebsiteUrl;
                        response.Explorer = serviceDetailJson.Explorer;
                        response.Community = serviceDetailJson.Community;
                        response.WhitePaperPath = serviceDetailJson.WhitePaperPath;
                        response.Introduction = serviceDetailJson.Introduction;

                        //var serviceStastics = _serviceStasticsRepository.GetSingle(ser => ser.ServiceId == service.Id);
                        response.CirculatingSupply = service.CirculatingSupply;
                        response.IssueDate = service.IssueDate;
                        response.IssuePrice = service.IssuePrice;

                        //var depositSerMapping = _serviceTypeMapping.GetSingle(x => x.ServiceId == service.Id && x.TrnType == Convert.ToInt16(enTrnType.Deposit));
                        //if (depositSerMapping != null)
                        //{
                        //    response.IsDeposit = depositSerMapping.Status;
                        //}

                        //var withdrawSerMapping = _serviceTypeMapping.GetSingle(x => x.ServiceId == service.Id && x.TrnType == Convert.ToInt16(enTrnType.Withdraw));
                        //if (withdrawSerMapping != null)
                        //{
                        //    response.IsWithdraw = withdrawSerMapping.Status;
                        //}

                        //var tranSerMapping = _serviceTypeMapping.GetSingle(x => x.ServiceId == service.Id && x.TrnType == Convert.ToInt16(enTrnType.Transaction));
                        //if (tranSerMapping != null)
                        //{
                        //    response.IsTransaction = tranSerMapping.Status;
                        //}
                        response.IsDeposit = service.DepositBit;
                        response.IsWithdraw = service.WithdrawBit;
                        response.IsTransaction = service.TransactionBit;

                        responsedata.Add(response);
                    }
                    return responsedata;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public ServiceConfigurationRequest GetServiceConfiguration(long ServiceId)
        {
            ServiceConfigurationRequest responsedata;
            try
            {
                responsedata = new ServiceConfigurationRequest();
                var serviceMaster = _serviceMasterRepository.GetActiveById(ServiceId);
                if (serviceMaster != null)
                {
                    responsedata.ServiceId = serviceMaster.Id;
                    responsedata.Name = serviceMaster.Name;
                    responsedata.SMSCode = serviceMaster.SMSCode;
                    responsedata.Type = serviceMaster.ServiceType;

                    var serviceDetail = _serviceDetailRepository.GetSingle(service => service.ServiceId == ServiceId);
                    var serviceDetailJson = JsonConvert.DeserializeObject<ServiceDetailJsonData>(serviceDetail.ServiceDetailJson);

                    responsedata.ImageUrl = serviceDetailJson.ImageUrl;
                    responsedata.TotalSupply = serviceDetailJson.TotalSupply;
                    responsedata.MaxSupply = serviceDetailJson.MaxSupply;
                    responsedata.ProofType = serviceDetailJson.ProofType;
                    responsedata.EncryptionAlgorithm = serviceDetailJson.EncryptionAlgorithm;
                    responsedata.WebsiteUrl = serviceDetailJson.WebsiteUrl;
                    responsedata.Explorer = serviceDetailJson.Explorer;
                    responsedata.Community = serviceDetailJson.Community;
                    responsedata.WhitePaperPath = serviceDetailJson.WhitePaperPath;
                    responsedata.Introduction = serviceDetailJson.Introduction;

                    var serviceStastics = _serviceStasticsRepository.GetSingle(service => service.ServiceId == ServiceId);
                    responsedata.CirculatingSupply = serviceStastics.CirculatingSupply;
                    responsedata.IssueDate = serviceStastics.IssueDate;
                    responsedata.IssuePrice = serviceStastics.IssuePrice;

                    var depositSerMapping = _serviceTypeMapping.GetSingle(x => x.ServiceId == ServiceId && x.TrnType == Convert.ToInt16(enTrnType.Deposit));
                    if (depositSerMapping != null)
                    {
                        responsedata.IsDeposit = depositSerMapping.Status;
                    }

                    var withdrawSerMapping = _serviceTypeMapping.GetSingle(x => x.ServiceId == ServiceId && x.TrnType == Convert.ToInt16(enTrnType.Withdraw));
                    if (withdrawSerMapping != null)
                    {
                        responsedata.IsWithdraw = withdrawSerMapping.Status;
                    }

                    var tranSerMapping = _serviceTypeMapping.GetSingle(x => x.ServiceId == ServiceId && x.TrnType == Convert.ToInt16(enTrnType.Transaction));
                    if (tranSerMapping != null)
                    {
                        responsedata.IsTransaction = tranSerMapping.Status;
                    }

                    return responsedata;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public int SetActiveService(long ServiceId)
        {
            try
            {
                var servicedata = _serviceMasterRepository.GetById(ServiceId);
                if (servicedata != null)
                {
                    servicedata.SetActiveService();
                    _serviceMasterRepository.Update(servicedata);
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public int SetInActiveService(long ServiceId)
        {
            try
            {
                var servicedata = _serviceMasterRepository.GetById(ServiceId);
                if (servicedata != null)
                {
                    servicedata.SetInActiveService();
                    _serviceMasterRepository.Update(servicedata);
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public List<ServiceCurrencyData> GetAllServiceConfigurationByBase(String Base)
        {
            List<ServiceCurrencyData> responsedata;
            try
            {
                var CheckBase = _marketRepository.GetSingle(m => m.CurrencyName == Base);
                if (CheckBase == null)
                    return null;
                responsedata = new List<ServiceCurrencyData>();
                var model = _serviceMasterRepository.GetSingle(ser => ser. SMSCode== Base);
                if (model == null)
                    return null;
                var serviceid = model.Id;

                var modellist = _serviceMasterRepository.List();
                foreach (var modelData in modellist)
                {
                    if (modelData.Id == serviceid)
                        continue;
                    responsedata.Add(new ServiceCurrencyData()
                    {
                        Name = modelData.Name,
                        ServiceId =modelData.Id,
                        SMSCode =modelData .SMSCode
                    });
                }
                return responsedata;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        #endregion


        #region ProviderMaster
        public IEnumerable<ServiceProviderViewModel> GetAllProvider()
        {
            try
            {
                var list = _ServiceProviderMaster.List();
                List<ServiceProviderViewModel> providerList = new List<ServiceProviderViewModel>();
                foreach (ServiceProviderMaster model in list)
                {
                    providerList.Add(new ServiceProviderViewModel
                    {
                        Id = model.Id,
                        ProviderName = model.ProviderName,
                        //Status = model.Status,

                    });
                }
                return providerList;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }

        }

        public ServiceProviderViewModel GetPoviderByID(long ID)
        {
            try
            {
                ServiceProviderMaster model = _ServiceProviderMaster.GetActiveById(ID);
                if (model == null)
                {
                    return null;
                }
                var viewmodel = new ServiceProviderViewModel
                {
                    Id = model.Id,
                    ProviderName = model.ProviderName,
                    //Status = model.Status,

                };
                return viewmodel;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }

        }

        public long AddProviderService(ServiceProviderRequest request)
        {
            try
            {
                var model = new ServiceProviderMaster
                {
                    Id = request.Id,
                    ProviderName = request.ProviderName,
                    Status = Convert.ToInt16(ServiceStatus.Active),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = null
                };
                var newModel = _ServiceProviderMaster.Add(model);
                return newModel.Id;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }

        }

        public bool UpdateProviderService(ServiceProviderRequest request)
        {
            try
            {
                ServiceProviderMaster model = _ServiceProviderMaster.GetActiveById(request.Id);
                if (model == null)
                {
                    return false;
                }
                model.ProviderName = request.ProviderName;
                model.UpdatedDate = DateTime.UtcNow;
                model.UpdatedBy = 1;

                _ServiceProviderMaster.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool SetActiveProvider(long id)
        {
            try
            {
                ServiceProviderMaster model = _ServiceProviderMaster.GetById(id);
                if (model != null)
                {
                    model.EnableProvider();
                    _ServiceProviderMaster.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool SetInActiveProvider(long id)
        {
            try
            {
                ServiceProviderMaster model = _ServiceProviderMaster.GetById(id);
                if (model != null)
                {
                    model.DisableProvider();
                    _ServiceProviderMaster.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        #endregion

        #region AppType
        public IEnumerable<AppTypeViewModel> GetAppType()
        {
            try
            {
                var list = _ApptypeRepository.List();
                List<AppTypeViewModel> AppList = new List<AppTypeViewModel>();
                foreach (AppType model in list)
                {
                    AppList.Add(new AppTypeViewModel
                    {
                        Id = model.Id,
                        AppTypeName = model.AppTypeName,
                    });
                }
                return AppList;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }

        }

        public AppTypeViewModel GetAppTypeById(long id)
        {
            try
            {
                AppType model = _ApptypeRepository.GetActiveById(id);
                if (model == null)
                {
                    return null;
                }
                var viewmodel = new AppTypeViewModel
                {
                    Id = model.Id,
                    AppTypeName = model.AppTypeName
                };
                return viewmodel;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }

        }

        public long AddAppType(AppTypeRequest request)
        {
            try
            {

                var model = new AppType
                {
                    Id = request.Id,
                    AppTypeName = request.AppTypeName,
                    Status = Convert.ToInt16(ServiceStatus.Active),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = null
                };
                var newModel = _ApptypeRepository.Add(model);
                return newModel.Id;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }

        }

        public bool UpdateAppType(AppTypeRequest request)
        {
            try
            {
                AppType model = _ApptypeRepository.GetActiveById(request.Id);
                if (model == null)
                {
                    return false;
                }
                model.AppTypeName = request.AppTypeName;
                model.UpdatedDate = DateTime.UtcNow;
                model.UpdatedBy = 1;

                _ApptypeRepository.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }

        }

        public bool SetActiveAppType(long id)
        {
            try
            {
                AppType model = _ApptypeRepository.GetById(id);
                if (model != null)
                {
                    model.EnableAppType();
                    _ApptypeRepository.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool SetInActiveAppType(long id)
        {
            try
            {
                AppType model = _ApptypeRepository.GetById(id);
                if (model != null)
                {
                    model.DisableAppType();
                    _ApptypeRepository.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        #endregion

        #region ProviderType

        public IEnumerable<ProviderTypeViewModel> GetProviderType()
        {
            try
            {
                var list = _ProviderTypeRepository.List();
                List<ProviderTypeViewModel> ProTypeList = new List<ProviderTypeViewModel>();
                foreach (ServiceProviderType model in list)
                {
                    ProTypeList.Add(new ProviderTypeViewModel
                    {
                        Id = model.Id,
                        ServiveProTypeName = model.ServiveProTypeName
                    });
                }
                return ProTypeList;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }

        }

        public ProviderTypeViewModel GetProviderTypeById(long id)
        {
            try
            {
                ServiceProviderType model = _ProviderTypeRepository.GetActiveById(id);
                if (model == null)
                {
                    return null;
                }
                var viewmodel = new ProviderTypeViewModel
                {
                    Id = model.Id,
                    ServiveProTypeName = model.ServiveProTypeName,
                    //Status = model.Status

                };
                return viewmodel;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }

        }

        public long AddProviderType(ProviderTypeRequest request)
        {
            try
            {
                var model = new ServiceProviderType
                {
                    Id = request.Id,
                    ServiveProTypeName = request.ServiveProTypeName,
                    Status = Convert.ToInt16(ServiceStatus.Active),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = null
                };
                var newModel = _ProviderTypeRepository.Add(model);
                return newModel.Id;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }

        }

        public bool UpdateProviderType(ProviderTypeRequest request)
        {
            try
            {
                var model = _ProviderTypeRepository.GetActiveById(request.Id);
                if (model == null)
                {
                    return false;
                }
                model.ServiveProTypeName = request.ServiveProTypeName;
                model.UpdatedDate = DateTime.UtcNow;
                model.UpdatedBy = 1;
                _ProviderTypeRepository.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }

        }

        public bool SetActiveProviderType(long id)
        {
            try
            {
                ServiceProviderType model = _ProviderTypeRepository.GetById(id);
                if (model != null)
                {
                    model.EnableProviderType();
                    _ProviderTypeRepository.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool SetInActiveProviderType(long id)
        {
            try
            {
                ServiceProviderType model = _ProviderTypeRepository.GetById(id);
                if (model != null)
                {
                    model.DisableProviderType();
                    _ProviderTypeRepository.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        #endregion 

        #region provider Configuration

        public ProviderConfigurationViewModel GetProviderConfiguration(long id)
        {
            try
            {
                ServiceProConfiguration model = _ProviderConfiguration.GetActiveById(id);
                if (model == null)
                {
                    return null;
                }
                var viewmodel = new ProviderConfigurationViewModel
                {
                    Id = model.Id,
                    APIKey = model.APIKey,
                    AppKey = model.AppKey,
                    SecretKey = model.SecretKey,
                    Password = model.Password,
                    UserName = model.UserName
                };
                return viewmodel;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public long AddProviderConfiguration(ProviderConfigurationRequest request)
        {
            try
            {
                ServiceProConfiguration model = new ServiceProConfiguration
                {
                    APIKey = request.APIKey,
                    AppKey = request.AppKey,
                    SecretKey = request.SecretKey,
                    Password = request.Password,
                    UserName = request.UserName,
                    CreatedBy = 1,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = null,
                    UpdatedDate = DateTime.UtcNow,
                    Status = Convert.ToInt16(ServiceStatus.Active)
                };
                var newModel = _ProviderConfiguration.Add(model);
                return newModel.Id;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool SetActiveProviderConfiguration(long id)
        {
            try
            {
                ServiceProConfiguration model = _ProviderConfiguration.GetById(id);
                if (model != null)
                {
                    model.EnableProConfiguration();
                    _ProviderConfiguration.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool SetInActiveProviderConfiguration(long id)
        {
            try
            {
                ServiceProConfiguration model = _ProviderConfiguration.GetById(id);
                if (model != null)
                {
                    model.DisableProConfiguration();
                    _ProviderConfiguration.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool UpdateProviderConfiguration(ProviderConfigurationRequest request)
        {
            try
            {
                ServiceProConfiguration model = _ProviderConfiguration.GetActiveById(request.Id);
                if (model == null)
                {
                    return false;
                }
                model.APIKey = request.APIKey;
                model.AppKey = request.AppKey;
                model.SecretKey = request.SecretKey;
                model.UserName = request.UserName;
                model.Password = request.Password;
                model.UpdatedDate = DateTime.UtcNow;
                model.UpdatedBy = 1;

                _ProviderConfiguration.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        #endregion

        #region "Demon Configuration"
        public DemonconfigurationViewModel GetDemonConfiguration(long id)
        {
            try
            {
                var model = _DemonRepository.GetActiveById(id);
                if (model == null)
                {
                    return null;
                }
                var viewmodel = new DemonconfigurationViewModel
                {
                    Id = model.Id,
                    IPAdd = model.IPAdd,
                    PortAdd = model.PortAdd,
                    Url = model.Url
                };
                return viewmodel;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public long AddDemonConfiguration(DemonConfigurationRequest request)
        {
            try
            {
                var model = new DemonConfiguration
                {
                    IPAdd = request.IPAdd,
                    PortAdd = request.PortAdd,
                    Url = request.Url,
                    Status = Convert.ToInt16(ServiceStatus.Active),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = null
                };

                var newModel = _DemonRepository.Add(model);
                return newModel.Id;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool UpdateDemonConfiguration(DemonConfigurationRequest request)
        {
            try
            {
                DemonConfiguration model = _DemonRepository.GetActiveById(request.Id);
                if (model == null)
                {
                    return false;
                }
                model.IPAdd = request.IPAdd;
                model.PortAdd = request.PortAdd;
                model.Url = request.Url;
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.UtcNow;


                _DemonRepository.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool SetActiveDemonConfig(long id)
        {
            try
            {
                DemonConfiguration model = _DemonRepository.GetById(id);
                if (model != null)
                {
                    model.EnableConfiguration();
                    _DemonRepository.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool SetInActiveDemonConfig(long id)
        {
            try
            {
                DemonConfiguration model = _DemonRepository.GetById(id);
                if (model != null)
                {
                    model.DisableConfiguration();
                    _DemonRepository.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        #endregion

        #region ProviderDetail
        public IEnumerable<ProviderDetailViewModel> GetProviderDetailList()
        {
            try
            {
                var list = _ProDetailRepository.List();
                List<ProviderDetailViewModel> providerList = new List<ProviderDetailViewModel>();
                foreach (ServiceProviderDetail model in list)
                {
                    providerList.Add(new ProviderDetailViewModel
                    {
                        Id = model.Id,
                        ServiceProID = model.ServiceProID,
                        ProTypeID = model.ProTypeID,
                        AppTypeID = model.AppTypeID,
                        TrnTypeID = model.TrnTypeID,
                        LimitID = model.LimitID,
                        DemonConfigID = model.DemonConfigID,
                        ServiceProConfigID = model.ServiceProConfigID,
                        ThirPartyAPIID = model.ThirPartyAPIID
                    });
                }
                return providerList;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public ProviderDetailViewModel GetProviderDetailById(long id)
        {
            try
            {
                var model = _ProDetailRepository.GetActiveById(id);
                if (model == null)
                {
                    return null;
                }
                var viewmodel = new ProviderDetailViewModel
                {
                    Id = model.Id,
                    ServiceProID = model.ServiceProID,
                    ProTypeID = model.ProTypeID,
                    AppTypeID = model.AppTypeID,
                    TrnTypeID = model.TrnTypeID,
                    LimitID = model.LimitID,
                    DemonConfigID = model.DemonConfigID,
                    ServiceProConfigID = model.ServiceProConfigID,
                    ThirPartyAPIID = model.ThirPartyAPIID,
                };
                return viewmodel;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public long AddProviderDetail(ProviderDetailRequest request)
        {
            try
            {
                ServiceProviderDetail model = new ServiceProviderDetail
                {
                    ServiceProID = request.ServiceProID,
                    ProTypeID = request.ProTypeID,
                    AppTypeID = request.AppTypeID,
                    TrnTypeID = request.TrnTypeID,
                    LimitID = request.LimitID,
                    DemonConfigID = request.DemonConfigID,
                    ServiceProConfigID = request.ServiceProConfigID,
                    ThirPartyAPIID = request.ThirPartyAPIID,
                    Status = Convert.ToInt16(ServiceStatus.Active),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = null
                };

                var newModel = _ProDetailRepository.Add(model);
                return newModel.Id;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool UpdateProviderDetail(ProviderDetailRequest request)
        {
            try
            {
                ServiceProviderDetail model = _ProDetailRepository.GetActiveById(request.Id);
                if (model == null)
                {
                    return false;
                }
                model.ServiceProID = request.ServiceProID;
                model.ProTypeID = request.ProTypeID;
                model.AppTypeID = request.AppTypeID;
                model.TrnTypeID = request.TrnTypeID;
                model.LimitID = request.LimitID;
                model.DemonConfigID = request.DemonConfigID;
                model.ServiceProConfigID = request.ServiceProConfigID;
                model.ThirPartyAPIID = request.ThirPartyAPIID;
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.UtcNow;
                _ProDetailRepository.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool SetActiveProviderDetail(long id)
        {
            try
            {
                ServiceProviderDetail model = _ProDetailRepository.GetById(id);
                if (model != null)
                {
                    model.EnableProvider();
                    _ProDetailRepository.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool SetInActiveProviderDetail(long id)
        {
            try
            {
                ServiceProviderDetail model = _ProDetailRepository.GetById(id);
                if (model != null)
                {
                    model.DisableProvider();
                    _ProDetailRepository.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public IEnumerable<ProviderDetailGetAllResponce> getProviderDetailsDataList(IEnumerable<ProviderDetailViewModel> dataList)
        {
            try
            {
                List<ProviderDetailGetAllResponce> responcesData = new List<ProviderDetailGetAllResponce>();
                foreach (ProviderDetailViewModel viewmodel in dataList)
                {
                    responcesData.Add(new ProviderDetailGetAllResponce
                    {
                        Id = viewmodel.Id,
                        Provider = GetPoviderByID(viewmodel.ServiceProID),
                        ProviderType = GetProviderTypeById(viewmodel.ProTypeID),
                        AppType = GetAppTypeById(viewmodel.AppTypeID),
                        TrnType = viewmodel.TrnTypeID,
                        Limit = GetLimitById(viewmodel.LimitID),
                        DemonConfiguration = GetDemonConfiguration(viewmodel.DemonConfigID),
                        ProviderConfiguration = GetProviderConfiguration(viewmodel.Id),
                        thirdParty = GetThirdPartyAPIConfigById(viewmodel.ThirPartyAPIID),
                    });
                }
                return responcesData;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public ProviderDetailGetAllResponce getProviderDetailDataById(ProviderDetailViewModel viewModel)
        {
            try
            {
                ProviderDetailGetAllResponce res = new ProviderDetailGetAllResponce();

                res.Id = viewModel.Id;
                res.Provider = GetPoviderByID(viewModel.ServiceProID);
                res.ProviderType = GetProviderTypeById(viewModel.ProTypeID);
                res.AppType = GetAppTypeById(viewModel.AppTypeID);
                res.TrnType = viewModel.TrnTypeID;
                res.Limit = GetLimitById(viewModel.LimitID);
                res.DemonConfiguration = GetDemonConfiguration(viewModel.DemonConfigID);
                res.ProviderConfiguration = GetProviderConfiguration(viewModel.Id);
                res.thirdParty = GetThirdPartyAPIConfigById(viewModel.ThirPartyAPIID);

                return res;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        #endregion

        #region ProductConfiguration
        public long AddProductConfiguration(ProductConfigurationRequest Request)
        {
            try
            {
                ProductConfiguration product = new ProductConfiguration()
                {
                    ProductName = Request.ProductName,
                    ServiceID = Request.ServiceID,
                    CountryID = Request.CountryID,
                    Status = Convert.ToInt16(ServiceStatus.Active),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = null
                };
                var newProduct = _productConfigRepository.Add(product);
                return newProduct.Id;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public long UpdateProductConfiguration(ProductConfigurationRequest Request)
        {
            try
            {
                var product = _productConfigRepository.GetActiveById(Request.Id);
                if (product != null)
                {
                    product.ProductName = Request.ProductName;
                    product.ServiceID = Request.ServiceID;
                    product.CountryID = Request.CountryID;
                    product.UpdatedDate = DateTime.UtcNow;
                    product.UpdatedBy = 1;

                    _productConfigRepository.Update(product);
                    return product.Id;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public ProductConfigrationGetInfo GetProductConfiguration(long ProductId)
        {
            ProductConfigrationGetInfo responsedata;
            try
            {
                responsedata = new ProductConfigrationGetInfo();
                var product = _productConfigRepository.GetActiveById(ProductId);
                if (product != null)
                {
                    responsedata.Id = product.Id;
                    responsedata.ProductName = product.ProductName;

                    var serviceMaster = _serviceMasterRepository.GetById(product.ServiceID);
                    var countryMaster = _countryMasterRepository.GetById(product.CountryID);
                    responsedata.CountryID = product.CountryID;
                    responsedata.ServiceID = product.ServiceID;
                    responsedata.ServiceName = serviceMaster.Name;
                    responsedata.CountryName = countryMaster.CountryName;

                    return responsedata;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public List<ProductConfigrationGetInfo> GetAllProductConfiguration()
        {
            List<ProductConfigrationGetInfo> responsedata;
            try
            {
                responsedata = new List<ProductConfigrationGetInfo>();

                var productall = _productConfigRepository.List();
                if (productall != null)
                {
                    foreach (var product in productall)
                    {
                        ProductConfigrationGetInfo response = new ProductConfigrationGetInfo();
                        response.Id = product.Id;
                        response.ProductName = product.ProductName;

                        var serviceMaster = _serviceMasterRepository.GetById(product.ServiceID);
                        var countryMaster = _countryMasterRepository.GetById(product.CountryID);
                        response.CountryID = product.CountryID;
                        response.ServiceID = product.ServiceID;
                        response.ServiceName = serviceMaster.Name;
                        response.CountryName = countryMaster.CountryName;

                        responsedata.Add(response);
                    }
                    return responsedata;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public int SetActiveProduct(long ProductId)
        {
            try
            {
                var product = _productConfigRepository.GetById(ProductId);
                if (product != null)
                {
                    product.SetActiveProduct();
                    _productConfigRepository.Update(product);
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public int SetInActiveProduct(long ProductId)
        {
            try
            {
                var product = _productConfigRepository.GetById(ProductId);
                if (product != null)
                {
                    product.SetInActiveProduct();
                    _productConfigRepository.Update(product);
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        #endregion

        #region RouteConfiguration
        public long AddRouteConfiguration(RouteConfigurationRequest Request)
        {
            try
            {
                RouteConfiguration route = new RouteConfiguration()
                {
                    RouteName = Request.RouteName,
                    ServiceID = Request.ServiceID,
                    SerProDetailID = Request.ServiceProDetailId,
                    ProductID = Request.ProductID,
                    Priority = Request.Priority,
                    StatusCheckUrl = Request.StatusCheckUrl,
                    ValidationUrl = Request.ValidationUrl,
                    TransactionUrl = Request.TransactionUrl,
                    LimitId = Request.LimitId,
                    OpCode = Request.OpCode,
                    TrnType = Request.TrnType,
                    IsDelayAddress = Request.IsDelayAddress,
                    ProviderWalletID = Request.ProviderWalletID,
                    Status = Convert.ToInt16(ServiceStatus.Active),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = null
                };
                var newProduct = _routeConfigRepository.Add(route);
                return newProduct.Id;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public long UpdateRouteConfiguration(RouteConfigurationRequest Request)
        {
            try
            {
                var route = _routeConfigRepository.GetActiveById(Request.Id);
                if (route != null)
                {
                    route.RouteName = Request.RouteName;
                    route.ServiceID = Request.ServiceID;
                    route.SerProDetailID = Request.ServiceProDetailId;
                    route.ProductID = Request.ProductID;
                    route.Priority = Request.Priority;
                    route.StatusCheckUrl = Request.StatusCheckUrl;
                    route.ValidationUrl = Request.ValidationUrl;
                    route.TransactionUrl = Request.TransactionUrl;
                    route.LimitId = Request.LimitId;
                    route.OpCode = Request.OpCode;
                    route.TrnType = Request.TrnType;
                    route.IsDelayAddress = Request.IsDelayAddress;
                    route.ProviderWalletID = Request.ProviderWalletID;
                    route.UpdatedDate = DateTime.UtcNow;
                    route.UpdatedBy = 1;

                    _routeConfigRepository.Update(route);
                    return route.Id;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public RouteConfigurationRequest GetRouteConfiguration(long RouteId)
        {
            RouteConfigurationRequest responsedata;
            try
            {
                responsedata = new RouteConfigurationRequest();
                var route = _routeConfigRepository.GetActiveById(RouteId);
                if (route != null)
                {
                    responsedata.Id = route.Id;
                    responsedata.RouteName = route.RouteName;
                    responsedata.ServiceID = route.ServiceID;
                    responsedata.ServiceProDetailId = route.SerProDetailID;
                    responsedata.ProductID = route.ProductID;
                    responsedata.Priority = route.Priority;
                    responsedata.StatusCheckUrl = route.StatusCheckUrl;
                    responsedata.ValidationUrl = route.ValidationUrl;
                    responsedata.TransactionUrl = route.TransactionUrl;
                    responsedata.LimitId = route.LimitId;
                    responsedata.OpCode = route.OpCode;
                    responsedata.TrnType = route.TrnType;
                    responsedata.IsDelayAddress = route.IsDelayAddress;
                    responsedata.ProviderWalletID = route.ProviderWalletID;
                    return responsedata;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public List<RouteConfigurationRequest> GetAllRouteConfiguration()
        {
            List<RouteConfigurationRequest> responsedata;
            try
            {
                responsedata = new List<RouteConfigurationRequest>();

                var routeall = _routeConfigRepository.List();
                if (routeall != null)
                {
                    foreach (var route in routeall)
                    {
                        RouteConfigurationRequest response = new RouteConfigurationRequest();

                        response.Id = route.Id;
                        response.RouteName = route.RouteName;
                        response.ServiceID = route.ServiceID;
                        response.ServiceProDetailId = route.SerProDetailID;
                        response.ProductID = route.ProductID;
                        response.Priority = route.Priority;
                        response.StatusCheckUrl = route.StatusCheckUrl;
                        response.ValidationUrl = route.ValidationUrl;
                        response.TransactionUrl = route.TransactionUrl;
                        response.LimitId = route.LimitId;
                        response.OpCode = route.OpCode;
                        response.TrnType = route.TrnType;
                        response.IsDelayAddress = route.IsDelayAddress;
                        response.ProviderWalletID = route.ProviderWalletID;

                        responsedata.Add(response);
                    }
                    return responsedata;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public int SetActiveRoute(long RouteId)
        {
            try
            {
                var route = _routeConfigRepository.GetById(RouteId);
                if (route != null)
                {
                    route.SetActiveRoute();
                    _routeConfigRepository.Update(route);
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public int SetInActiveRoute(long RouteId)
        {
            try
            {
                var route = _routeConfigRepository.GetById(RouteId);
                if (route != null)
                {
                    route.SetInActiveRoute();
                    _routeConfigRepository.Update(route);
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        #endregion

        #region ThirdPartyAPIConfig
        public List<ThirdPartyAPIConfigViewModel> GetAllThirdPartyAPIConfig()
        {
            try
            {
                var list = _thirdPartyAPIRepository.List();
                List<ThirdPartyAPIConfigViewModel> thirdPartyAPIs = new List<ThirdPartyAPIConfigViewModel>();
                foreach (ThirdPartyAPIConfiguration model in list)
                {
                    thirdPartyAPIs.Add(new ThirdPartyAPIConfigViewModel()
                    {
                        Id = model.Id,
                        APIBalURL = model.APIBalURL,
                        APIName = model.APIName,
                        APIRequestBody = model.APIRequestBody,
                        APISendURL = model.APISendURL,
                        APIStatusCheckURL = model.APIStatusCheckURL,
                        APIValidateURL = model.APIValidateURL,
                        AppType = model.AppType,
                        AuthHeader = model.AuthHeader,
                        ContentType = model.ContentType,
                        HashCode = model.HashCode,
                        HashCodeRecheck = model.HashCodeRecheck,
                        HashType = model.HashType,
                        MerchantCode = model.MerchantCode,
                        MethodType = model.MethodType,
                        ParsingDataID = model.ParsingDataID,
                        ResponseFailure = model.ResponseFailure,
                        ResponseHold = model.ResponseHold,
                        ResponseSuccess = model.ResponseSuccess,
                        //SerProConfigurationID = model.SerProConfigurationID,
                        TransactionIdPrefix = model.TransactionIdPrefix
                    });
                }

                return thirdPartyAPIs;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public ThirdPartyAPIConfigViewModel GetThirdPartyAPIConfigById(long Id)
        {
            try
            {
                ThirdPartyAPIConfiguration model = _thirdPartyAPIRepository.GetActiveById(Id);
                if (model == null)
                {
                    return null;
                }
                var viewmodel = new ThirdPartyAPIConfigViewModel
                {
                    Id = model.Id ,
                    APIBalURL =model.APIBalURL,
                    APIName =model .APIName ,
                    APIRequestBody =model .APIRequestBody,
                    APISendURL =model .APISendURL,
                    APIStatusCheckURL =model .APIStatusCheckURL ,
                    APIValidateURL =model .APIValidateURL,
                    AppType =model .AppType ,
                    AuthHeader =model .AuthHeader ,
                    ContentType =model .ContentType ,
                    HashCode  =model .HashCode ,
                    HashCodeRecheck   =model .HashCodeRecheck,
                    HashType=model .HashType ,
                    MerchantCode =model .MerchantCode,
                    MethodType =model .MethodType ,
                    ParsingDataID =model .ParsingDataID ,
                    ResponseFailure =model .ResponseFailure ,
                    ResponseHold =model .ResponseHold ,
                    ResponseSuccess =model .ResponseSuccess,
                    //SerProConfigurationID =model .SerProConfigurationID,
                    TransactionIdPrefix =model .TransactionIdPrefix
                };
                return viewmodel;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public long AddThirdPartyAPI(ThirdPartyAPIConfigRequest request)
        {
            try
            {
                var model = new ThirdPartyAPIConfiguration()
                {
                    CreatedBy = 1,
                    CreatedDate= DateTime.UtcNow,
                    Status =1,
                    APIBalURL = request.APIBalURL,
                    APIName = request.APIName,
                    APIRequestBody = request.APIRequestBody,
                    APISendURL = request.APISendURL,
                    APIStatusCheckURL = request.APIStatusCheckURL,
                    APIValidateURL = request.APIValidateURL,
                    AppType = request.AppType,
                    AuthHeader = request.AuthHeader,
                    ContentType = request.ContentType,
                    HashCode = request.HashCode,
                    HashCodeRecheck = request.HashCodeRecheck,
                    HashType = request.HashType,
                    MerchantCode = request.MerchantCode,
                    MethodType = request.MethodType,
                    ParsingDataID = request.ParsingDataID,
                    ResponseFailure = request.ResponseFailure,
                    ResponseHold = request.ResponseHold,
                    ResponseSuccess = request.ResponseSuccess,
                    //SerProConfigurationID = request.SerProConfigurationID,
                    TransactionIdPrefix = request.TransactionIdPrefix
                    
                };
                var newModel = _thirdPartyAPIRepository.Add(model);
                return newModel.Id;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool UpdateThirdPartyAPI(ThirdPartyAPIConfigRequest request)
        {
            try
            {
                var model = _thirdPartyAPIRepository.GetActiveById(request.Id);
                if (model == null)
                {
                    return false;
                }

                model.APIBalURL = request.APIBalURL;
                model.APIName = request.APIName;
                model.APIRequestBody = request.APIRequestBody;
                model.APISendURL = request.APISendURL;
                model.APIStatusCheckURL = request.APIStatusCheckURL;
                model.APIValidateURL = request.APIValidateURL;
                model.AppType = request.AppType;
                model.AuthHeader = request.AuthHeader;
                model.ContentType = request.ContentType;
                model.HashCode = request.HashCode;
                model.HashCodeRecheck = request.HashCodeRecheck;
                model.HashType = request.HashType;
                model.MerchantCode = request.MerchantCode;
                model.MethodType = request.MethodType;
                model.ParsingDataID = request.ParsingDataID;
                model.ResponseFailure = request.ResponseFailure;
                model.ResponseHold = request.ResponseHold;
                model.ResponseSuccess = request.ResponseSuccess;
                //model.SerProConfigurationID = request.SerProConfigurationID;
                model.TransactionIdPrefix = request.TransactionIdPrefix;
                model.UpdatedDate = DateTime.UtcNow;
                model.UpdatedBy = 1;

                _thirdPartyAPIRepository.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool SetActiveThirdPartyAPI(long id)
        {
            try
            {
                ThirdPartyAPIConfiguration model = _thirdPartyAPIRepository.GetById(id);
                if (model != null)
                {
                    model.SetActive();
                    _thirdPartyAPIRepository.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool SetInActiveThirdPartyAPI(long id)
        {
            try
            {
                ThirdPartyAPIConfiguration model = _thirdPartyAPIRepository.GetById(id);
                if (model != null)
                {
                    model.SetInActive();
                    _thirdPartyAPIRepository.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        #endregion

        #region ThirdPartyAPIResponseConfig

        public List<ThirdPartyAPIResponseConfigViewModel> GetAllThirdPartyAPIResponse()
        {
            try
            {
                var list = _thirdPartyAPIResRepository.List();
                List<ThirdPartyAPIResponseConfigViewModel> APIResponseList = new List<ThirdPartyAPIResponseConfigViewModel>();
                foreach (var model in list)
                {
                    APIResponseList.Add(new ThirdPartyAPIResponseConfigViewModel()
                    {
                        BalanceRegex =model .BalanceRegex,
                        ErrorCodeRegex =model .ErrorCodeRegex,
                        Id =model .Id,
                        OprTrnRefNoRegex =model .OprTrnRefNoRegex,
                        Param1Regex =model .Param1Regex,
                        Param2Regex =model .Param2Regex ,
                        Param3Regex =model .Param3Regex,
                        ResponseCodeRegex =model .ResponseCodeRegex,
                        StatusMsgRegex =model .StatusMsgRegex ,
                        StatusRegex =model .StatusRegex,
                        TrnRefNoRegex =model .TrnRefNoRegex 
                    });
                }
                return APIResponseList;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public ThirdPartyAPIResponseConfigViewModel GetThirdPartyAPIResponseById(long id)
        {
            try
            {
                var model = _thirdPartyAPIResRepository.GetActiveById(id);
                if (model == null)
                {
                    return null;
                }
                var viewModel = new ThirdPartyAPIResponseConfigViewModel()
                {
                    BalanceRegex = model.BalanceRegex,
                    ErrorCodeRegex = model.ErrorCodeRegex,
                    Id = model.Id,
                    OprTrnRefNoRegex = model.OprTrnRefNoRegex,
                    Param1Regex = model.Param1Regex,
                    Param2Regex = model.Param2Regex,
                    Param3Regex = model.Param3Regex,
                    ResponseCodeRegex = model.ResponseCodeRegex,
                    StatusMsgRegex = model.StatusMsgRegex,
                    StatusRegex = model.StatusRegex,
                    TrnRefNoRegex = model.TrnRefNoRegex
                };
                return viewModel;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public long AddThirdPartyAPIResponse(ThirdPartyAPIResponseConfigViewModel Request)
        {
            try
            {
                var model = new ThirdPartyAPIResponseConfiguration()
                {
                    CreatedBy = 1,
                    CreatedDate=DateTime .UtcNow ,
                    Status =1,
                    BalanceRegex = Request.BalanceRegex,
                    ErrorCodeRegex = Request.ErrorCodeRegex,
                    OprTrnRefNoRegex = Request.OprTrnRefNoRegex,
                    Param1Regex = Request.Param1Regex,
                    Param2Regex = Request.Param2Regex,
                    Param3Regex = Request.Param3Regex,
                    ResponseCodeRegex = Request.ResponseCodeRegex,
                    StatusMsgRegex = Request.StatusMsgRegex,
                    StatusRegex = Request.StatusRegex,
                    TrnRefNoRegex = Request.TrnRefNoRegex
                };
                var newModel = _thirdPartyAPIResRepository.Add(model);
                return newModel.Id;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool UpdateThirdPartyAPIResponse(ThirdPartyAPIResponseConfigViewModel Request)
        {
            try
            {
                var model = _thirdPartyAPIResRepository.GetActiveById(Request.Id);
                if (model == null)
                {
                    return false;
                }
                model.BalanceRegex = Request.BalanceRegex;
                model.ErrorCodeRegex = Request.ErrorCodeRegex;
                model.OprTrnRefNoRegex = Request.OprTrnRefNoRegex;
                model.Param1Regex = Request.Param1Regex;
                model.Param2Regex = Request.Param2Regex;
                model.Param3Regex = Request.Param3Regex;
                model.ResponseCodeRegex = Request.ResponseCodeRegex;
                model.StatusMsgRegex = Request.StatusMsgRegex;
                model.StatusRegex = Request.StatusRegex;
                model.TrnRefNoRegex = Request.TrnRefNoRegex;
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.UtcNow;

                _thirdPartyAPIResRepository.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool SetActiveThirdPartyAPIResponse(long id)
        {
            try
            {
                ThirdPartyAPIResponseConfiguration model = _thirdPartyAPIResRepository.GetById(id);
                if (model != null)
                {
                    model.SetActive();
                    _thirdPartyAPIResRepository.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public bool SetInActiveThirdPartyAPIResponse(long id)
        {
            try
            {
                ThirdPartyAPIResponseConfiguration model = _thirdPartyAPIResRepository.GetById(id);
                if (model != null)
                {
                    model.SetInActive ();
                    _thirdPartyAPIResRepository.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        #endregion

        #region TradePairConfiguration
        public long AddPairConfiguration(TradePairConfigRequest Request)
        {
            try
            {
                var baseCurrency = _serviceMasterRepository.GetById(Request.BaseCurrencyId);
                var secondCurrency = _serviceMasterRepository.GetById(Request.SecondaryCurrencyId);
                var pairName = secondCurrency.SMSCode + "_" + baseCurrency.SMSCode;

                var pairMaster = new TradePairMaster()
                {
                    PairName = pairName,
                    SecondaryCurrencyId = Request.SecondaryCurrencyId,
                    WalletMasterID = Request.WalletMasterID,
                    BaseCurrencyId = Request.BaseCurrencyId,
                    Status = Convert.ToInt16(ServiceStatus.Active),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = null
                };

                var newPairMaster = _tradePairMasterRepository.Add(pairMaster);

                var pairDetail = new TradePairDetail()
                {
                    PairId = newPairMaster.Id,
                    BuyMinQty = Request.BuyMinQty,
                    BuyMaxQty = Request.BuyMaxQty,
                    SellMinQty = Request.SellMinQty,
                    SellMaxQty = Request.SellMaxQty,
                    SellPrice = Request.SellPrice,
                    BuyPrice = Request.BuyPrice,
                    BuyMinPrice = Request.BuyMinPrice,
                    BuyMaxPrice = Request.BuyMaxPrice,
                    SellMinPrice = Request.SellMinPrice,
                    SellMaxPrice = Request.SellMaxPrice,
                    SellFees = Request.SellFees,
                    BuyFees = Request.BuyFees,
                    FeesCurrency = Request.FeesCurrency,
                    Status = Convert.ToInt16(ServiceStatus.Active),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = null
                };
                var newPairDetail = _tradePairDetailRepository.Add(pairDetail);

                var pairStastic = new TradePairStastics
                {
                    PairId = newPairMaster.Id,
                    CurrentRate = Request.Currentrate,
                    ChangeVol24 = Request.Volume,
                    CurrencyPrice = Request.CurrencyPrice,
                    Status = Convert.ToInt16(ServiceStatus.Active),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = null
                };
                var newPairStatics = _tradePairStastics.Add(pairStastic);

                return newPairMaster.Id;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public long UpdatePairConfiguration(TradePairConfigRequest Request)
        {
            try
            {
                var pairMaster = _tradePairMasterRepository.GetActiveById(Request.Id);
                if (pairMaster != null)
                {
                    var baseCurrency = _serviceMasterRepository.GetById(Request.BaseCurrencyId);
                    var secondCurrency = _serviceMasterRepository.GetById(Request.SecondaryCurrencyId);
                    var pairName = secondCurrency.SMSCode + "_" + baseCurrency.SMSCode;

                    pairMaster.PairName = pairName;
                    pairMaster.SecondaryCurrencyId = Request.SecondaryCurrencyId;
                    pairMaster.WalletMasterID = Request.WalletMasterID;
                    pairMaster.BaseCurrencyId = Request.BaseCurrencyId;
                    pairMaster.UpdatedDate = DateTime.UtcNow;
                    pairMaster.UpdatedBy = 1;

                    _tradePairMasterRepository.Update(pairMaster);

                    var pairDetail = _tradePairDetailRepository.GetSingle(pair => pair.PairId == Request.Id);

                    pairDetail.BuyMinQty = Request.BuyMinQty;
                    pairDetail.BuyMaxQty = Request.BuyMaxQty;
                    pairDetail.SellMinQty = Request.SellMinQty;
                    pairDetail.SellMaxQty = Request.SellMaxQty;
                    pairDetail.SellPrice = Request.SellPrice;
                    pairDetail.BuyPrice = Request.BuyPrice;
                    pairDetail.BuyMinPrice = Request.BuyMinPrice;
                    pairDetail.BuyMaxPrice = Request.BuyMaxPrice;
                    pairDetail.SellMinPrice = Request.SellMinPrice;
                    pairDetail.SellMaxPrice = Request.SellMaxPrice;
                    pairDetail.SellFees = Request.SellFees;
                    pairDetail.BuyFees = Request.BuyFees;
                    pairDetail.FeesCurrency = Request.FeesCurrency;
                    pairDetail.UpdatedDate = DateTime.UtcNow;
                    pairDetail.UpdatedBy = 1;
                    _tradePairDetailRepository.Update(pairDetail);

                    var pairStastics = _tradePairStastics.GetSingle(pair => pair.PairId == Request.Id);
                    pairStastics.CurrentRate = Request.Currentrate;
                    pairStastics.ChangeVol24 = Request.Volume;
                    pairStastics.CurrencyPrice = Request.CurrencyPrice;
                    pairStastics.UpdatedDate = DateTime.UtcNow;
                    pairStastics.UpdatedBy = 1;
                    _tradePairStastics.Update(pairStastics);

                    return Request.Id;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public TradePairConfigRequest GetPairConfiguration(long PairId)
        {
            TradePairConfigRequest responsedata;
            try
            {
                responsedata = new TradePairConfigRequest();
                var pairMaster = _tradePairMasterRepository.GetActiveById(PairId);
                if (pairMaster != null)
                {
                    responsedata.Id = pairMaster.Id;
                    responsedata.PairName = pairMaster.PairName;
                    responsedata.SecondaryCurrencyId = pairMaster.SecondaryCurrencyId;
                    responsedata.WalletMasterID = pairMaster.WalletMasterID;
                    responsedata.BaseCurrencyId = pairMaster.BaseCurrencyId;

                    var pairDetail = _tradePairDetailRepository.GetSingle(pair => pair.PairId == PairId);

                    responsedata.BuyMinQty = pairDetail.BuyMinQty;
                    responsedata.BuyMaxQty = pairDetail.BuyMaxQty;
                    responsedata.SellMinQty = pairDetail.SellMinQty;
                    responsedata.SellMaxQty = pairDetail.SellMaxQty;
                    responsedata.SellPrice = pairDetail.SellPrice;
                    responsedata.BuyPrice = pairDetail.BuyPrice;
                    responsedata.BuyMinPrice = pairDetail.BuyMinPrice;
                    responsedata.BuyMaxPrice = pairDetail.BuyMaxPrice;
                    responsedata.SellMinPrice = pairDetail.SellMinPrice;
                    responsedata.SellMaxPrice = pairDetail.SellMaxPrice;
                    responsedata.BuyFees = pairDetail.BuyFees;
                    responsedata.SellFees = pairDetail.SellFees;
                    responsedata.FeesCurrency = pairDetail.FeesCurrency;

                    var pairStastics = _tradePairStastics.GetSingle(pair => pair.PairId == PairId);
                    responsedata.Volume = pairStastics.ChangeVol24;
                    responsedata.Currentrate = pairStastics.CurrentRate;
                    responsedata.CurrencyPrice = pairStastics.CurrencyPrice;

                    return responsedata;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public List<TradePairConfigRequest> GetAllPairConfiguration()
        {
            List<TradePairConfigRequest> responsedata;
            try
            {
                responsedata = new List<TradePairConfigRequest>();

                var pairMaster = _tradePairMasterRepository.List();
                if (pairMaster != null)
                {
                    foreach (var pair in pairMaster)
                    {
                        TradePairConfigRequest response = new TradePairConfigRequest();
                        response.Id = pair.Id;
                        response.PairName = pair.PairName;
                        response.SecondaryCurrencyId = pair.SecondaryCurrencyId;
                        response.WalletMasterID = pair.WalletMasterID;
                        response.BaseCurrencyId = pair.BaseCurrencyId;

                        var pairDetail = _tradePairDetailRepository.GetSingle(x => x.PairId == pair.Id);

                        response.BuyMinQty = pairDetail.BuyMinQty;
                        response.BuyMaxQty = pairDetail.BuyMaxQty;
                        response.SellMinQty = pairDetail.SellMinQty;
                        response.SellMaxQty = pairDetail.SellMaxQty;
                        response.SellPrice = pairDetail.SellPrice;
                        response.BuyPrice = pairDetail.BuyPrice;
                        response.BuyMinPrice = pairDetail.BuyMinPrice;
                        response.BuyMaxPrice = pairDetail.BuyMaxPrice;
                        response.SellMinPrice = pairDetail.SellMinPrice;
                        response.SellMaxPrice = pairDetail.SellMaxPrice;
                        response.BuyFees = pairDetail.BuyFees;
                        response.SellFees = pairDetail.SellFees;
                        response.FeesCurrency = pairDetail.FeesCurrency;

                        var pairStastics = _tradePairStastics.GetSingle(x => x.PairId == pair.Id);
                        response.Volume = pairStastics.ChangeVol24;
                        response.Currentrate = pairStastics.CurrentRate;
                        response.CurrencyPrice = pairStastics.CurrencyPrice;

                        responsedata.Add(response);
                    }
                    return responsedata;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public int SetActivePair(long PairId)
        {
            try
            {
                var pairdata = _tradePairMasterRepository.GetById(PairId);
                if (pairdata != null)
                {
                    pairdata.MakePairActive();
                    _tradePairMasterRepository.Update(pairdata);
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public int SetInActivePair(long PairId)
        {
            try
            {
                var pairdata = _tradePairMasterRepository.GetById(PairId);
                if (pairdata != null)
                {
                    pairdata.MakePairInActive();
                    _tradePairMasterRepository.Update(pairdata);
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        #endregion

        #region Other Configuration
        public List<ServiceTypeMasterInfo> GetAllServiceTypeMaster()
        {
            List<ServiceTypeMasterInfo> responsedata;
            try
            {
                responsedata = new List<ServiceTypeMasterInfo>();

                String[] serviceTypeName = Enum.GetNames(typeof(enServiceType));
                int[] serviceTypeValues = (int[])Enum.GetValues(typeof(enServiceType));
                if (serviceTypeName.Length > 0)
                {
                    for (int i = 0; i < serviceTypeName.Length; i++)
                    {
                        ServiceTypeMasterInfo response = new ServiceTypeMasterInfo();
                        response.Id = serviceTypeValues[i];
                        response.SerTypeName = serviceTypeName[i];

                        responsedata.Add(response);
                    }
                    return responsedata;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public List<TransactionTypeInfo> GetAllTransactionType()
        {
            List<TransactionTypeInfo> responsedata;
            try
            {
                responsedata = new List<TransactionTypeInfo>();

                String[] trnTypeName = Enum.GetNames(typeof(enTrnType));
                int[] trnTypeValues = (int[])Enum.GetValues(typeof(enTrnType));
                if (trnTypeName.Length > 0)
                {
                    for (int i = 0; i < trnTypeName.Length; i++)
                    {
                        TransactionTypeInfo response = new TransactionTypeInfo();
                        response.Id = trnTypeValues[i];
                        response.TrnTypeName = trnTypeName[i];

                        responsedata.Add(response);
                    }
                    return responsedata;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        #endregion

        #region Limit

        public List<LimitViewModel> GetAllLimitData()
        {
            try
            {
                var list = _limitRepository.List();
                List<LimitViewModel> limitList = new List<LimitViewModel>();
                foreach (var model in list)
                {
                    limitList.Add(new LimitViewModel()
                    {
                        Id = model.Id,
                        MaxAmt = model.MaxAmt,
                        MaxAmtDaily = model.MaxAmtDaily,
                        MaxAmtMonthly = model.MaxAmtMonthly,
                        MaxAmtWeekly = model.MaxAmtWeekly,
                        Maxrange = model.Maxrange,
                        MaxRangeDaily = model.MaxRangeDaily,
                        MaxRangeMonthly = model.MaxRangeMonthly,
                        MaxRangeWeekly = model.MaxRangeWeekly,
                        MinAmt = model.MinAmt,
                        MinAmtDaily = model.MinAmtDaily,
                        MinAmtMonthly = model.MinAmtMonthly,
                        MinAmtWeekly = model.MinAmtWeekly,
                        MinRange = model.MinRange,
                        MinRangeDaily = model.MinRangeDaily,
                        MinRangeMonthly = model.MinRangeMonthly,
                        MinRangeWeekly = model.MinRangeWeekly
                    });
                }

                return limitList;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public LimitViewModel GetLimitById(long id)
        {
            try
            {
                var model = _limitRepository.GetActiveById(id);
                if (model == null)
                    return null;

                var viewModel = new LimitViewModel()
                {
                    Id = model.Id,
                    MaxAmt = model.MaxAmt,
                    MaxAmtDaily = model.MaxAmtDaily,
                    MaxAmtMonthly = model.MaxAmtMonthly,
                    MaxAmtWeekly = model.MaxAmtWeekly,
                    Maxrange = model.Maxrange,
                    MaxRangeDaily = model.MaxRangeDaily,
                    MaxRangeMonthly = model.MaxRangeMonthly,
                    MaxRangeWeekly = model.MaxRangeWeekly,
                    MinAmt = model.MinAmt,
                    MinAmtDaily = model.MinAmtDaily,
                    MinAmtMonthly = model.MinAmtMonthly,
                    MinAmtWeekly = model.MinAmtWeekly,
                    MinRange = model.MinRange,
                    MinRangeDaily = model.MinRangeDaily,
                    MinRangeMonthly = model.MinRangeMonthly,
                    MinRangeWeekly = model.MinRangeWeekly
                };
                return viewModel;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public long AddLimitData(LimitRequest Request)
        {
            try
            {
                var model = new Limits()
                {
                    CreatedBy = 1,
                    CreatedDate = DateTime.UtcNow,
                    Status = 1,
                    MaxAmt = Request.MaxAmt,
                    MaxAmtDaily = Request.MaxAmtDaily,
                    MaxAmtMonthly = Request.MaxAmtMonthly,
                    MaxAmtWeekly = Request.MaxAmtWeekly,
                    Maxrange = Request.Maxrange,
                    MaxRangeDaily = Request.MaxRangeDaily,
                    MaxRangeMonthly = Request.MaxRangeMonthly,
                    MaxRangeWeekly = Request.MaxRangeWeekly,
                    MinAmt = Request.MinAmt,
                    MinAmtDaily = Request.MinAmtDaily,
                    MinAmtMonthly = Request.MinAmtMonthly,
                    MinAmtWeekly = Request.MinAmtWeekly,
                    MinRange = Request.MinRange,
                    MinRangeDaily = Request.MinRangeDaily,
                    MinRangeMonthly = Request.MinRangeMonthly,
                    MinRangeWeekly = Request.MinRangeWeekly
                };
                var newModel = _limitRepository.Add(model);
                return newModel.Id;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool UpdateLimitData(LimitRequest Request)
        {
            try
            {
                var model = _limitRepository.GetActiveById(Request.Id);
                if (model == null)
                    return false;

                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.UtcNow;
                model.MaxAmt = Request.MaxAmt;
                model.MaxAmtDaily = Request.MaxAmtDaily;
                model.MaxAmtMonthly = Request.MaxAmtMonthly;
                model.MaxAmtWeekly = Request.MaxAmtWeekly;
                model.Maxrange = Request.Maxrange;
                model.MaxRangeDaily = Request.MaxRangeDaily;
                model.MaxRangeMonthly = Request.MaxRangeMonthly;
                model.MaxRangeWeekly = Request.MaxRangeWeekly;
                model.MinAmt = Request.MinAmt;
                model.MinAmtDaily = Request.MinAmtDaily;
                model.MinAmtMonthly = Request.MinAmtMonthly;
                model.MinAmtWeekly = Request.MinAmtWeekly;
                model.MinRange = Request.MinRange;
                model.MinRangeDaily = Request.MinRangeDaily;
                model.MinRangeMonthly = Request.MinRangeMonthly;
                model.MinRangeWeekly = Request.MinRangeWeekly;

                _limitRepository.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool SetActiveLimit(long id)
        {
            try
            {
                Limits model = _limitRepository.GetById(id);
                if (model != null)
                {
                    model.SetActiveLimit();
                    _limitRepository.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool SetInActiveLimit(long id)
        {
            try
            {
                Limits model = _limitRepository.GetById(id);
                if (model != null)
                {
                    model.SetInActiveLimit();
                    _limitRepository.Update(model);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }



        #endregion

        #region Market
        public MarketViewModel AddMarketData(MarketViewModel viewModel)
        {
            try
            {
                Market market = new Market
                {
                    CurrencyName = viewModel.CurrencyName,
                    ServiceID = viewModel.ServiceID,
                    isBaseCurrency = viewModel.isBaseCurrency,
                    Status = Convert.ToInt16(ServiceStatus.Active),
                    CreatedBy = 1,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = null
                };
                var newMarket = _marketRepository.Add(market);

                return viewModel;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public List<MarketViewModel> GetAllMarketData()
        {
            try
            {
                List<MarketViewModel> list = new List<MarketViewModel>();
                var modellist = _marketRepository.List();

                foreach (var model in modellist)
                {
                    list.Add(new MarketViewModel()
                    {
                        ID = model.Id,
                        CurrencyName = model.CurrencyName,
                        ServiceID = model.ServiceID,
                        isBaseCurrency = model.isBaseCurrency,
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public MarketViewModel GetMarketDataByMarket(long Id)
        {
            try
            {
                MarketViewModel marketView = new MarketViewModel();
                var model = _marketRepository.GetById(Id);
                if (model == null)
                    return null;

                marketView.CurrencyName = model.CurrencyName;
                marketView.ID = model.Id;
                marketView.isBaseCurrency = model.isBaseCurrency;
                marketView.ServiceID = model.ServiceID;
                return marketView;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        #endregion
    }
}
