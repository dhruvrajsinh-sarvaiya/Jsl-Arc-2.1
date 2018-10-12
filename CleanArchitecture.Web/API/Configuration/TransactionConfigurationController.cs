using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities.Configuration;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces.Configuration;
using CleanArchitecture.Core.ViewModels.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Web.API.Configuration
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionConfigurationController : ControllerBase
    {
        private readonly ITransactionConfigService _transactionConfigService;
        private readonly ILogger<TransactionConfigurationController> _logger;

        public TransactionConfigurationController(ITransactionConfigService transactionConfigService)
        {
            _transactionConfigService = transactionConfigService;
        }

        #region Service
        [HttpPost("AddServiceConfiguration")]
        public IActionResult AddServiceConfiguration([FromBody]ServiceConfigurationRequest Request)
        {
            ServiceConfigurationResponse Response = new ServiceConfigurationResponse();
            try
            {
                long ServiceId = _transactionConfigService.AddServiceConfiguration(Request);

                if (ServiceId != 0)
                {
                    Response.response = new ServiceConfigurationInfo() { ServiceId = ServiceId };
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    //Response.ErrorCode =enErrorCode. // not inserted
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                Response.ReturnCode = enResponseCode.InternalError;
                return Ok(Response);
            }
        }

        [HttpPost("UpdateServiceConfiguration")]
        public IActionResult UpdateServiceConfiguration([FromBody]ServiceConfigurationRequest Request)
        {
            ServiceConfigurationResponse Response = new ServiceConfigurationResponse();
            try
            {
                long ServiceId = _transactionConfigService.UpdateServiceConfiguration(Request);

                if (ServiceId != 0)
                {
                    Response.response = new ServiceConfigurationInfo() { ServiceId = ServiceId };
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    //Response.ErrorCode =enErrorCode. // not found
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                Response.ReturnCode = enResponseCode.InternalError;
                return Ok(Response);
            }
        }

        [HttpGet("GetServiceConfiguration/{ServiceId}")]
        public IActionResult GetServiceConfiguration(long ServiceId)
        {
            ServiceConfigurationGetResponse Response = new ServiceConfigurationGetResponse();
            try
            {
                var responsedata = _transactionConfigService.GetServiceConfiguration(ServiceId);
                if (responsedata != null)
                {
                    Response.ReturnCode = enResponseCode.Success;
                    Response.response = responsedata;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                Response.ReturnCode = enResponseCode.InternalError;
                return Ok(Response);
            }
        }

        [HttpGet("GetAllServiceConfiguration")]
        public IActionResult GetAllServiceConfiguration()
        {
            ServiceConfigurationGetAllResponse Response = new ServiceConfigurationGetAllResponse();
            try
            {
                var responsedata = _transactionConfigService.GetAllServiceConfiguration();
                if (responsedata != null)
                {
                    Response.ReturnCode = enResponseCode.Success;
                    Response.response = responsedata;   
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                Response.ReturnCode = enResponseCode.InternalError;
                return Ok(Response);
            }
        }

        [HttpPost("SetActiveService/{ServiceId}")]
        public IActionResult SetActiveService(int ServiceId)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                var responsedata = _transactionConfigService.SetActiveService(ServiceId);
                if(responsedata == 1)
                {
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                Response.ReturnCode = enResponseCode.InternalError;
                return Ok(Response);
            }
        }

        [HttpPost("SetInActiveService/{ServiceId}")]
        public IActionResult SetInActiveService(int ServiceId)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                var responsedata = _transactionConfigService.SetInActiveService(ServiceId);
                if (responsedata == 1)
                {
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                Response.ReturnCode = enResponseCode.InternalError;
                return Ok(Response);
            }
        }

        #endregion

        #region providermaster
        [HttpGet("GetProviderList")]
        public IActionResult GetProviderList()
        {
            ServiceProviderResponce res = new ServiceProviderResponce();
            try
            {
                res.responce = _transactionConfigService.GetAllProvider();
                if (res.responce == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.ItemNotFoundForGenerateAddress;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Success;
                return Ok(res);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
            
        }

        [HttpGet("GetProviderById/{id:long}")]
        public IActionResult GetProviderById(long id)
        {
            ServiceProviderResponceData res = new ServiceProviderResponceData();
            try
            {
                res.responce = _transactionConfigService.GetPoviderByID(id);
                if (res.responce == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.ItemNotFoundForGenerateAddress;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Success;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
            
        }

        [HttpPost("AddServiceProvider")]
        public IActionResult AddServiceProvider([FromBody]ServiceProviderRequest request)
        {
            BizResponseClass  res = new BizResponseClass();
            try
            {
                long Id= _transactionConfigService.AddProviderService(request);
                if(Id != 0 )
                {
                    //res.responce = new ServiceProviderViewModel { Id = Id };
                    res.ReturnCode = enResponseCode.Success;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Fail ;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        [HttpPost("UpdateServiceProvider")]
        public IActionResult UpdateServiceProvider([FromBody]ServiceProviderRequest request)
        {
            ServiceProviderResponceData res = new ServiceProviderResponceData();
            bool state = false;
            try
            {
                if(request.Id == 0)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                state = _transactionConfigService .UpdateProviderService(request);
                if (state == false)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                res.responce = _transactionConfigService.GetPoviderByID(request.Id);
                if (res.responce == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Success;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        [HttpPost("SetActiveProvider/{id:long}")]
        public IActionResult SetActiveProvider(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetActiveProvider(id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                    res.ReturnCode = enResponseCode.Fail;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        [HttpPost("SetInActiveProvider/{id:long}")]
        public IActionResult SetInActiveProvider(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetInActiveProvider (id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                    res.ReturnCode = enResponseCode.Fail;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }
        #endregion

        #region Apptype
        [HttpGet("GetAppType")]
        public IActionResult GetAppType()
        {
            AppTypeResponce res = new AppTypeResponce();
            try
            {
                res.responce = _transactionConfigService.GetAppType();
                if(res.responce == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Success;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
            
        }

        [HttpGet("GetAppTypeById/{id:long}")]
        public IActionResult GetAppTypeById(long id)
        {
            AppTypeResponceData res = new AppTypeResponceData();
            try
            {
                res.responce = _transactionConfigService.GetAppTypeById(id);
                if (res.responce == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Success;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
            
        }

        [HttpPost("AddAppType")]
        public IActionResult AddAppType([FromBody]AppTypeRequest request)
        {
            AppTypeResponceData res = new AppTypeResponceData();
            //BizResponseClass res = new BizResponseClass();
            try
            {
                long id=_transactionConfigService.AddAppType(request);
                if(id !=0)
                {
                    res.responce = _transactionConfigService.GetAppTypeById(id);
                    res.ReturnCode = enResponseCode.Success;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Fail;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
            
        }

        [HttpPost("UpdateAppType")]
        public IActionResult UpdateAppType([FromBody]AppTypeRequest request)
        {
            AppTypeResponceData res = new AppTypeResponceData();
            bool state = false;
            try
            {
                if (request.Id == 0)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                state =_transactionConfigService.UpdateAppType(request);
                if(state == false )
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                res.responce = _transactionConfigService.GetAppTypeById(request.Id);
                if (res.responce == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Success;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
            
        }

        [HttpPost("SetActiveAppType/{id:long}")]
        public IActionResult SetActiveAppType(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetActiveAppType(id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                    res.ReturnCode = enResponseCode.Fail;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        [HttpPost("SetInActiveAppType/{id:long}")]
        public IActionResult SetInActiveAppType(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetInActiveAppType (id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                    res.ReturnCode = enResponseCode.Fail;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        #endregion

        #region providerType
        [HttpGet("GetServiceProviderType")]
        public IActionResult GetServiceProviderType()
        {

            ProviderTypeResponce res = new ProviderTypeResponce();
            try
            {
                res.responce = _transactionConfigService.GetProviderType();
                if (res.responce == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Success;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
            
        }

        [HttpGet("GetServiceProviderTypeById/{id:long}")]
        public IActionResult GetServiceProviderTypeById(long id)
        {
            ProviderTypeResponceData res = new ProviderTypeResponceData();
            try
            {
                res.responce = _transactionConfigService.GetProviderTypeById(id);
                if (res.responce == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Success;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
            
        }

        [HttpPost("AddProviderType")]
        public IActionResult AddProviderType([FromBody]ProviderTypeRequest request )
        {
            ProviderTypeResponceData res = new ProviderTypeResponceData();
            //BizResponseClass res = new BizResponseClass();
            try
            {
                long id = _transactionConfigService.AddProviderType(request);
                if (id != 0)
                {
                    res.responce = _transactionConfigService.GetProviderTypeById(id);
                    res.ReturnCode = enResponseCode.Success;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Fail;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
            
        }

        [HttpPost("UpdateProviderType")]
        public IActionResult UpdateProviderType([FromBody]ProviderTypeRequest request)
        {
            ProviderTypeResponceData res = new ProviderTypeResponceData();
            bool state = false;
            try
            {
                if (request.Id == 0)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                state =_transactionConfigService.UpdateProviderType(request);
                if (state == false)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                res.responce = _transactionConfigService.GetProviderTypeById(request.Id);
                if (res.responce == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Success;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
           
        }

        [HttpPost("SetActiveProviderType/{id:long}")]
        public IActionResult SetActiveProviderType(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetActiveProviderType(id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                    res.ReturnCode = enResponseCode.Fail;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        [HttpPost("SetInActiveProviderType/{id:long}")]
        public IActionResult SetInActiveProviderType(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetInActiveProviderType(id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                    res.ReturnCode = enResponseCode.Fail;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        #endregion

        #region  providerConfiguration

        [HttpGet("GetProviderConfigurationById/{id:long}")]
        public IActionResult GetProviderConfigurationById(long id)
        {
            ProviderConfigurationResponce  res = new ProviderConfigurationResponce();
            try
            {
                res.responce = _transactionConfigService.GetProviderConfiguration(id);
                if (res.responce == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Success;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }

        }

        [HttpPost("AddProviderConfiguration")]
        public IActionResult AddProviderConfiguration([FromBody]ProviderConfigurationRequest request)
        {
            ProviderConfigurationResponce res = new ProviderConfigurationResponce();
            //BizResponseClass res = new BizResponseClass();
            try
            {
                long id = _transactionConfigService.AddProviderConfiguration(request);
                if (id != 0)
                {
                    res.responce = _transactionConfigService.GetProviderConfiguration(id);
                    res.ReturnCode = enResponseCode.Success ;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Fail ;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }

        }

        [HttpPost("UpdateProviderConfiguration")]
        public IActionResult UpdateProviderConfiguration([FromBody]ProviderConfigurationRequest request)
        {
            ProviderConfigurationResponce  res = new ProviderConfigurationResponce();
            bool state = false;
            try
            {
                if (request.Id == 0)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                state = _transactionConfigService.UpdateProviderConfiguration(request);
                if (state == false)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                res.responce = _transactionConfigService.GetProviderConfiguration(request.Id);
                if (res.responce == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Success;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }

        }

        [HttpPost("SetActiveProviderConfiguration/{id:long}")]
        public IActionResult SetActiveProviderConfiguration(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetInActiveProviderConfiguration(id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                    res.ReturnCode = enResponseCode.Fail;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        [HttpPost("SetInActiveProviderConfiguration/{id:long}")]
        public IActionResult SetInActiveProviderConfiguration(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetInActiveProviderConfiguration (id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                    res.ReturnCode = enResponseCode.Fail;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }
        #endregion

        #region DemonConfiguration

        [HttpGet("GetDemonConfigurationById/{id:long}")]
        public IActionResult GetDemonConfigurationById(long id)
        {
            DemonConfigurationResponce  res = new DemonConfigurationResponce();
            try
            {
                res.responce = _transactionConfigService.GetDemonConfiguration(id);
                if (res.responce == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Success;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }

        }

        [HttpPost("AddDemonConfiguration")]
        public IActionResult AddDemonConfiguration([FromBody]DemonConfigurationRequest  request)
        {
            DemonConfigurationResponce res = new DemonConfigurationResponce();
            //BizResponseClass res = new BizResponseClass();
            try
            {
                long id = _transactionConfigService.AddDemonConfiguration (request);
                if (id != 0)
                {
                    res.responce = _transactionConfigService.GetDemonConfiguration(id);
                    res.ReturnCode = enResponseCode.Success;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Fail;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }

        }

        [HttpPost("UpdateDemonConfiguration")]
        public IActionResult UpdateDemonConfiguration([FromBody]DemonConfigurationRequest request)
        {
            DemonConfigurationResponce res = new DemonConfigurationResponce();
            bool state = false;
            try
            {
                if (request.Id == 0)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                state = _transactionConfigService.UpdateDemonConfiguration (request);
                if (state == false)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                res.responce = _transactionConfigService.GetDemonConfiguration(request.Id);
                if (res.responce == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Success;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }

        }

        [HttpPost("SetActiveDemonConfiguration/{id:long}")]
        public IActionResult SetActiveDemonConfiguration(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetActiveDemonConfig(id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                    res.ReturnCode = enResponseCode.Fail;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        [HttpPost("SetInActiveDemonConfiguration/{id:long}")]
        public IActionResult SetInActiveDemonConfiguration(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetInActiveDemonConfig (id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                    res.ReturnCode = enResponseCode.Fail;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }
        #endregion

        #region Provider Details

        [HttpGet("GetProviderDetailList")]
        public IActionResult GetProviderDetailList()
        {
            ProviderDetailResponceList res = new ProviderDetailResponceList();
            try
            {
                IEnumerable<ProviderDetailViewModel> list = _transactionConfigService.GetProviderDetailList();
                if (list == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.ItemNotFoundForGenerateAddress;
                    return Ok(res);
                }
                res.responce = _transactionConfigService.getProviderDetailsDataList(list);
                res.ReturnCode = enResponseCode.Success;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        [HttpGet("GetProviderDetailById/{id:long}")]
        public IActionResult GetProviderDetailById(long id)
        {
            ProviderDetailResponce  res = new ProviderDetailResponce();
            try
            {
                ProviderDetailViewModel  obj = _transactionConfigService.GetProviderDetailById(id);
                if (obj == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.ItemNotFoundForGenerateAddress;
                    return Ok(res);
                }
                res.responce = _transactionConfigService.getProviderDetailDataById(obj);
                //res.responce.Id = obj.Id;
                //res.responce.Provider = _transactionConfigService.GetPoviderByID(obj.ServiceProID);
                //res.responce.ProviderType = _transactionConfigService.GetProviderTypeById(obj.ProTypeID);
                //res.responce.AppType = _transactionConfigService.GetAppTypeById(obj.AppTypeID);
                //res.responce.TrnType = null;
                //res.responce.Limit = null;
                //res.responce.DemonConfiguration = _transactionConfigService.GetDemonConfiguration(obj.DemonConfigID);
                //res.responce.ProviderConfiguration = _transactionConfigService.GetProviderConfiguration(obj.ServiceProConfigID);
                //res.responce.thirdParty = null;

                res.ReturnCode = enResponseCode.Success;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }

        }

        [HttpPost("AddProviderDetail")]
        public IActionResult AddProviderDetail([FromBody]ProviderDetailRequest request)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                long Id = _transactionConfigService.AddProviderDetail(request);
                if (Id != 0)
                {
                    //res.responce = new ServiceProviderViewModel { Id = Id };
                    res.ReturnCode = enResponseCode.Success;
                    return Ok(res);
                }
                res.ReturnCode = enResponseCode.Fail;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        [HttpPost("UpdateProviderDetail")]
        public IActionResult UpdateProviderDetail([FromBody]ProviderDetailRequest  request)
        {
            ProviderDetailResponce  res = new ProviderDetailResponce();
            bool state = false;
            try
            {
                if (request.Id == 0)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                state = _transactionConfigService.UpdateProviderDetail(request);
                if (state == false)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    return Ok(res);
                }
                ProviderDetailViewModel obj = _transactionConfigService.GetProviderDetailById(request.Id);
                if (obj == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.ItemNotFoundForGenerateAddress;
                    return Ok(res);
                }

                res.responce.Id = obj.Id;
                res.responce.Provider = _transactionConfigService.GetPoviderByID(obj.ServiceProID);
                res.responce.ProviderType = _transactionConfigService.GetProviderTypeById(obj.ProTypeID);
                res.responce.AppType = _transactionConfigService.GetAppTypeById(obj.AppTypeID);
                res.responce.TrnType = null;
                res.responce.Limit = null;
                res.responce.DemonConfiguration = _transactionConfigService.GetDemonConfiguration(obj.DemonConfigID);
                res.responce.ProviderConfiguration = _transactionConfigService.GetProviderConfiguration(obj.ServiceProConfigID);
                res.responce.thirdParty = null;
                res.ReturnCode = enResponseCode.Success;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        [HttpPost("SetActiveProviderDetail/{id:long}")]
        public IActionResult SetActiveProviderDetail(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetActiveProviderDetail(id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                    res.ReturnCode = enResponseCode.Fail;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        [HttpPost("SetInActiveProviderDetail/{id:long}")]
        public IActionResult SetInActiveProviderDetail(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetInActiveProviderDetail (id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                    res.ReturnCode = enResponseCode.Fail;
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        #endregion
    }
}