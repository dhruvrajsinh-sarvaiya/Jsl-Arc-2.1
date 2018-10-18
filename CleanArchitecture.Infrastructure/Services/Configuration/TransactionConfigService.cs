using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Configuration;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Configuration;
using CleanArchitecture.Core.ViewModels.Configuration;
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
        private readonly ICommonRepository<StateMaster> _stateMasterRepository;
        private readonly ICommonRepository<RouteConfiguration> _routeConfigRepository;

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
            ICommonRepository<StateMaster> stateMasterRepository,
            ICommonRepository<RouteConfiguration> routeConfigRepository)
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
            _stateMasterRepository = stateMasterRepository;
            _routeConfigRepository = routeConfigRepository;
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
                    Introduction = Request.Introduction
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
                    CirculatingSupply = Request.CirculatingSupply
                };
                var newServiceStastics = _serviceStasticsRepository.Add(serviceStastics);

                return newServiceMaster.Id;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public long UpdateServiceConfiguration(ServiceConfigurationRequest Request)
        {
            try
            {
                var serviceMaster = _serviceMasterRepository.GetById(Request.ServiceId);
                if (serviceMaster != null)
                {
                    serviceMaster.Name = Request.Name;
                    serviceMaster.SMSCode = Request.SMSCode;
                    serviceMaster.ServiceType = Request.Type;

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
                    _serviceStasticsRepository.Update(serviceStastics);

                    return Request.ServiceId;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public List<ServiceConfigurationRequest> GetAllServiceConfiguration()
        {
            List<ServiceConfigurationRequest> responsedata;
            try
            {
                responsedata = new List<ServiceConfigurationRequest>();

                var serviceMaster = _serviceMasterRepository.GetAll();
                if (serviceMaster != null)
                {
                    foreach (var service in serviceMaster)
                    {
                        ServiceConfigurationRequest response = new ServiceConfigurationRequest();
                        response.ServiceId = service.Id;
                        response.Name = service.Name;
                        response.SMSCode = service.SMSCode;
                        response.Type = service.ServiceType;

                        var serviceDetail = _serviceDetailRepository.GetSingle(ser => ser.ServiceId == service.Id);
                        var serviceDetailJson = JsonConvert.DeserializeObject<ServiceDetailJsonData>(serviceDetail.ServiceDetailJson);

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

                        var serviceStastics = _serviceStasticsRepository.GetSingle(ser => ser.ServiceId == service.Id);
                        response.CirculatingSupply = serviceStastics.CirculatingSupply;
                        response.IssueDate = serviceStastics.IssueDate;
                        response.IssuePrice = serviceStastics.IssuePrice;

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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public ServiceConfigurationRequest GetServiceConfiguration(long ServiceId)
        {
            ServiceConfigurationRequest responsedata;
            try
            {
                responsedata = new ServiceConfigurationRequest();
                var serviceMaster = _serviceMasterRepository.GetById(ServiceId);
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

                    return responsedata;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public int SetActiveService(int ServiceId)
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public int SetInActiveService(int ServiceId)
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }

        }

        public ServiceProviderViewModel GetPoviderByID(long ID)
        {
            try
            {
                ServiceProviderMaster model = _ServiceProviderMaster.GetById(ID);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = null
                };
                var newModel = _ServiceProviderMaster.Add(model);
                return newModel.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }

        }

        public bool UpdateProviderService(ServiceProviderRequest request)
        {
            try
            {
                ServiceProviderMaster model = _ServiceProviderMaster.GetById(request.Id);
                if (model == null)
                {
                    return false;
                }
                model.ProviderName = request.ProviderName;
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = 1;

                _ServiceProviderMaster.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }

        }

        public AppTypeViewModel GetAppTypeById(long id)
        {
            try
            {
                AppType model = _ApptypeRepository.GetById(id);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = null
                };
                var newModel = _ApptypeRepository.Add(model);
                return newModel.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }

        }

        public bool UpdateAppType(AppTypeRequest request)
        {
            try
            {
                AppType model = _ApptypeRepository.GetById(request.Id);
                if (model == null)
                {
                    return false;
                }
                model.AppTypeName = request.AppTypeName;
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = 1;

                _ApptypeRepository.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }

        }

        public ProviderTypeViewModel GetProviderTypeById(long id)
        {
            try
            {
                ServiceProviderType model = _ProviderTypeRepository.GetById(id);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = null
                };
                var newModel = _ProviderTypeRepository.Add(model);
                return newModel.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }

        }

        public bool UpdateProviderType(ProviderTypeRequest request)
        {
            try
            {
                var model = _ProviderTypeRepository.GetById(request.Id);
                if (model == null)
                {
                    return false;
                }
                model.ServiveProTypeName = request.ServiveProTypeName;
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = 1;
                _ProviderTypeRepository.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        #endregion 

        #region provider Configuration

        public ProviderConfigurationViewModel GetProviderConfiguration(long id)
        {
            try
            {
                ServiceProConfiguration model = _ProviderConfiguration.GetById(id);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                    CreatedDate = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedDate = DateTime.Now,
                    Status = Convert.ToInt16(ServiceStatus.Active)
                };
                var newModel = _ProviderConfiguration.Add(model);
                return newModel.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public bool UpdateProviderConfiguration(ProviderConfigurationRequest request)
        {
            try
            {
                ServiceProConfiguration model = _ProviderConfiguration.GetById(request.Id);
                if (model == null)
                {
                    return false;
                }
                model.APIKey = request.APIKey;
                model.AppKey = request.AppKey;
                model.SecretKey = request.SecretKey;
                model.UserName = request.UserName;
                model.Password = request.Password;
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = 1;

                _ProviderConfiguration.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        #endregion

        #region "Demon Configuration"
        public DemonconfigurationViewModel GetDemonConfiguration(long id)
        {
            try
            {
                var model = _DemonRepository.GetById(id);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = null
                };

                var newModel = _DemonRepository.Add(model);
                return newModel.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public bool UpdateDemonConfiguration(DemonConfigurationRequest request)
        {
            try
            {
                DemonConfiguration model = _DemonRepository.GetById(request.Id);
                if (model == null)
                {
                    return false;
                }
                model.IPAdd = request.IPAdd;
                model.PortAdd = request.PortAdd;
                model.Url = request.Url;
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now;


                _DemonRepository.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public ProviderDetailViewModel GetProviderDetailById(long id)
        {
            try
            {
                var model = _ProDetailRepository.GetById(id);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = null
                };

                var newModel = _ProDetailRepository.Add(model);
                return newModel.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public bool UpdateProviderDetail(ProviderDetailRequest request)
        {
            try
            {
                ServiceProviderDetail model = _ProDetailRepository.GetById(request.Id);
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

                _ProDetailRepository.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                        TrnType = null,
                        Limit = null,
                        DemonConfiguration = GetDemonConfiguration(viewmodel.DemonConfigID),
                        ProviderConfiguration = GetProviderConfiguration(viewmodel.Id),
                        thirdParty = null
                    });
                }
                return responcesData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                res.TrnType = null;
                res.Limit = null;
                res.DemonConfiguration = GetDemonConfiguration(viewModel.DemonConfigID);
                res.ProviderConfiguration = GetProviderConfiguration(viewModel.Id);
                res.thirdParty = null;

                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                    StateID = Request.StateID
                };
                var newProduct = _productConfigRepository.Add(product);
                return newProduct.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public long UpdateProductConfiguration(ProductConfigurationRequest Request)
        {
            try
            {
                var product = _productConfigRepository.GetById(Request.Id);
                if (product != null)
                {
                    product.ProductName = Request.ProductName;
                    product.ServiceID = Request.ServiceID;
                    product.StateID = Request.StateID;

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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public ProductConfigrationGetInfo GetProductConfiguration(long ProductId)
        {
            ProductConfigrationGetInfo responsedata;
            try
            {
                responsedata = new ProductConfigrationGetInfo();
                var product = _productConfigRepository.GetById(ProductId);
                if (product != null)
                {
                    responsedata.Id = product.Id;
                    responsedata.ProductName = product.ProductName;
                    
                    var serviceMaster = _serviceMasterRepository.GetById(product.ServiceID);
                    var stateMaster = _stateMasterRepository.GetById(product.StateID);
                    responsedata.StateID = product.StateID;
                    responsedata.ServiceID = product.ServiceID;
                    responsedata.ServiceName = serviceMaster.Name;
                    responsedata.StateName = stateMaster.StateName;

                    return responsedata;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public List<ProductConfigrationGetInfo> GetAllProductConfiguration()
        {
            List<ProductConfigrationGetInfo> responsedata;
            try
            {
                responsedata = new List<ProductConfigrationGetInfo>();

                var productall = _productConfigRepository.GetAll();
                if (productall != null)
                {
                    foreach (var product in productall)
                    {
                        ProductConfigrationGetInfo response = new ProductConfigrationGetInfo();
                        response.Id = product.Id;
                        response.ProductName = product.ProductName;

                        var serviceMaster = _serviceMasterRepository.GetById(product.ServiceID);
                        var stateMaster = _stateMasterRepository.GetById(product.StateID);
                        response.StateID = product.StateID;
                        response.ServiceID = product.ServiceID;
                        response.ServiceName = serviceMaster.Name;
                        response.StateName = stateMaster.StateName;

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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public int SetActiveProduct(int ProductId)
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public int SetInActiveProduct(int ProductId)
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                    ProviderWalletID = Request.ProviderWalletID
                };
                var newProduct = _routeConfigRepository.Add(route);
                return newProduct.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public long UpdateRouteConfiguration(RouteConfigurationRequest Request)
        {
            try
            {
                var route = _routeConfigRepository.GetById(Request.Id);
                if(route != null)
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public RouteConfigurationRequest GetRouteConfiguration(long RouteId)
        {
            RouteConfigurationRequest responsedata;
            try
            {
                responsedata = new RouteConfigurationRequest();
                var route = _routeConfigRepository.GetById(RouteId);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public List<RouteConfigurationRequest> GetAllRouteConfiguration()
        {
            List<RouteConfigurationRequest> responsedata;
            try
            {
                responsedata = new List<RouteConfigurationRequest>();

                var routeall = _routeConfigRepository.GetAll();
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public int SetActiveRoute(int RouteId)
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public int SetInActiveRoute(int RouteId)
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        #endregion
    }
}
