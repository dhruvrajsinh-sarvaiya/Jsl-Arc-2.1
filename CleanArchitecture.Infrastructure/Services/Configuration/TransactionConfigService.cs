using CleanArchitecture.Core.Entities.Configuration;
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
        private readonly ILogger<TransactionConfigService> _logger;

        public TransactionConfigService(
            ICommonRepository<ServiceMaster> serviceMasterRepository, 
            ICommonRepository<ServiceDetail> serviceDetailRepository, 
            ICommonRepository<ServiceStastics> serviceStasticsRepository, 
            ILogger<TransactionConfigService> logger,
            ICommonRepository<ServiceProviderMaster> ServiceProviderMaster,
            ICommonRepository<AppType> ApptypeRepository,
            ICommonRepository<ServiceProviderType> ProviderTypeRepository)
        {
            _serviceMasterRepository = serviceMasterRepository;
            _serviceDetailRepository = serviceDetailRepository;
            _serviceStasticsRepository = serviceStasticsRepository;
            _ServiceProviderMaster = ServiceProviderMaster;
            _ApptypeRepository = ApptypeRepository;
            _ProviderTypeRepository = ProviderTypeRepository;
            _logger = logger;
        }

        public long AddServiceConfiguration(ServiceConfigurationRequest Request)
        {
            try
            {
                ServiceMaster serviceMaster = new ServiceMaster()
                {
                    Name = Request.Name,
                    SMSCode = Request.SMSCode,
                    Type = Request.Type,
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
                    serviceMaster.Type = Request.Type;

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
                        response.Type = service.Type;

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
                    return responsedata;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
            throw new NotImplementedException();
        }

        public ServiceConfigurationRequest GetServiceConfiguration(long ServiceId)
        {
            ServiceConfigurationRequest responsedata;
            try
            {
                responsedata = new ServiceConfigurationRequest();
                var serviceMaster = _serviceMasterRepository.GetById(ServiceId);
                if(serviceMaster != null)
                {
                    responsedata.ServiceId = serviceMaster.Id;
                    responsedata.Name = serviceMaster.Name;
                    responsedata.SMSCode = serviceMaster.SMSCode;
                    responsedata.Type = serviceMaster.Type;

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
                    return responsedata;
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
                if(servicedata != null)
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

        public bool AddProviderService(ServiceProviderRequest request)
        {
            var model = new ServiceProviderMaster
            {
                Id = request.Id,
                ProviderName = request.ProviderName,
                Status = request.Status,
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                UpdatedDate = DateTime.Now,
                UpdatedBy = null
            };
            _ServiceProviderMaster.Add(model);
            return true;
        }

        public IEnumerable<ServiceProviderViewModel> GetAllProvider()
        {
            var list = _ServiceProviderMaster.List();
            List<ServiceProviderViewModel> providerList = new List<ServiceProviderViewModel>();
            foreach (ServiceProviderMaster model in list)
            {
                providerList.Add(new ServiceProviderViewModel
                {
                    Id = model.Id,
                    ProviderName = model.ProviderName,
                    Status = model.Status,

                });
            }
            return providerList;
        }

        public ServiceProviderViewModel GetPoviderByID(long ID)
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
                Status = model.Status,

            };
            return viewmodel;
        }

        public IEnumerable<AppTypeViewModel> GetAppType()
        {
            var list = _ApptypeRepository.List();
            List<AppTypeViewModel> AppList = new List<AppTypeViewModel>();
            foreach (AppType model in list)
            {
                AppList.Add(new AppTypeViewModel
                {
                    Id = model.Id,
                    AppTypeName = model.AppTypeName,
                    Status = model.Status
                });
            }
            return AppList;
        }

        public AppTypeViewModel GetAppTypeById(long id)
        {
            AppType model = _ApptypeRepository.GetById(id);
            if (model == null)
            {
                return null;
            }
            var viewmodel = new AppTypeViewModel
            {
                Id = model.Id,
                AppTypeName = model.AppTypeName,
                Status = model.Status

            };
            return viewmodel;
        }

        public IEnumerable<ProviderTypeViewModel> GetProviderType()
        {
            var list = _ProviderTypeRepository.List();
            List<ProviderTypeViewModel> ProTypeList = new List<ProviderTypeViewModel>();
            foreach (ServiceProviderType model in list)
            {
                ProTypeList.Add(new ProviderTypeViewModel
                {
                    Id = model.Id,
                    ServiveProType = model.ServiveProTypeName,
                    Status = model.Status
                });
            }
            return ProTypeList;
        }

        public ProviderTypeViewModel GetProviderTypeById(long id)
        {
            ServiceProviderType model = _ProviderTypeRepository.GetById(id);
            if (model == null)
            {
                return null;
            }
            var viewmodel = new ProviderTypeViewModel
            {
                Id = model.Id,
                ServiveProType = model.ServiveProTypeName,
                Status = model.Status

            };
            return viewmodel;
        }
    }
}
