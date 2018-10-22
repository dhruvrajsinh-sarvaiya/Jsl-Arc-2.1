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
                if (Request.ServiceId == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    //Response.ErrorCode = enErrorCode.InValid_ID;
                    return Ok(Response);
                }
                long ServiceId = _transactionConfigService.UpdateServiceConfiguration(Request);

                if (ServiceId != 0)
                {
                    Response.response = new ServiceConfigurationInfo() { ServiceId = ServiceId };
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
                if (ServiceId == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    //Response.ErrorCode = enErrorCode.InValid_ID;
                    return Ok(Response);
                }
                var responsedata = _transactionConfigService.GetServiceConfiguration(ServiceId);
                if (responsedata != null)
                {
                    Response.ReturnCode = enResponseCode.Success;
                    Response.response = responsedata;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
                if (responsedata.Count != 0)
                {
                    Response.ReturnCode = enResponseCode.Success;
                    Response.response = responsedata;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
        public IActionResult SetActiveService(long ServiceId)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                if (ServiceId == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    //Response.ErrorCode = enErrorCode.InValid_ID;
                    return Ok(Response);
                }

                var responsedata = _transactionConfigService.SetActiveService(ServiceId);
                if (responsedata == 1)
                {
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
        public IActionResult SetInActiveService(long ServiceId)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                if (ServiceId == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    //Response.ErrorCode = enErrorCode.InValid_ID;
                    return Ok(Response);
                }
                var responsedata = _transactionConfigService.SetInActiveService(ServiceId);
                if (responsedata == 1)
                {
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
                    res.ErrorCode = enErrorCode.NoDataFound;
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
                    res.ErrorCode = enErrorCode.NoDataFound;
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
                    res.ErrorCode = enErrorCode.NoDataFound;
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
                res.response = _transactionConfigService.GetAppType();
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
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
                res.response = _transactionConfigService.GetAppTypeById(id);
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
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
                    res.response = _transactionConfigService.GetAppTypeById(id);
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
                    res.ErrorCode = enErrorCode.InValid_ID;
                    return Ok(res);
                }
                state =_transactionConfigService.UpdateAppType(request);
                if(state == false )
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(res);
                }
                res.response = _transactionConfigService.GetAppTypeById(request.Id);
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
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
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                }
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
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                }
                    
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
                res.response = _transactionConfigService.GetProviderType();
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
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
                res.response = _transactionConfigService.GetProviderTypeById(id);
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;

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
                    res.response = _transactionConfigService.GetProviderTypeById(id);
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
                    res.ErrorCode = enErrorCode.InValid_ID;
                    return Ok(res);
                }
                state =_transactionConfigService.UpdateProviderType(request);
                if (state == false)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(res);
                }
                res.response = _transactionConfigService.GetProviderTypeById(request.Id);
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
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
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                }
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
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                }
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
                res.response = _transactionConfigService.GetProviderConfiguration(id);
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.InValid_ID;
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
                    res.response = _transactionConfigService.GetProviderConfiguration(id);
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
                    res.ErrorCode = enErrorCode.InValid_ID;
                    return Ok(res);
                }
                state = _transactionConfigService.UpdateProviderConfiguration(request);
                if (state == false)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(res);
                }
                res.response = _transactionConfigService.GetProviderConfiguration(request.Id);
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
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
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                }
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
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                }
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
                res.response = _transactionConfigService.GetDemonConfiguration(id);
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.InValid_ID;
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
                    res.response = _transactionConfigService.GetDemonConfiguration(id);
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
                    res.ErrorCode = enErrorCode.InValid_ID;
                    return Ok(res);
                }
                state = _transactionConfigService.UpdateDemonConfiguration (request);
                if (state == false)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(res);
                }
                res.response = _transactionConfigService.GetDemonConfiguration(request.Id);
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
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
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                }
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
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                }
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
                    res.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(res);
                }
                res.response = _transactionConfigService.getProviderDetailsDataList(list);
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
                    res.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(res);
                }
                res.response = _transactionConfigService.getProviderDetailDataById(obj);
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(res);
                }
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
                    res.ErrorCode = enErrorCode.InValid_ID;
                    return Ok(res);
                }
                state = _transactionConfigService.UpdateProviderDetail(request);
                if (state == false)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(res);
                }
                ProviderDetailViewModel obj = _transactionConfigService.GetProviderDetailById(request.Id);
                if (obj == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(res);
                }

                res.response.Id = obj.Id;
                res.response.Provider = _transactionConfigService.GetPoviderByID(obj.ServiceProID);
                res.response.ProviderType = _transactionConfigService.GetProviderTypeById(obj.ProTypeID);
                res.response.AppType = _transactionConfigService.GetAppTypeById(obj.AppTypeID);
                res.response.TrnType = null;
                res.response.Limit = null;
                res.response.DemonConfiguration = _transactionConfigService.GetDemonConfiguration(obj.DemonConfigID);
                res.response.ProviderConfiguration = _transactionConfigService.GetProviderConfiguration(obj.ServiceProConfigID);
                res.response.thirdParty = null;
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
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                }
                    
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
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                }
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

        #region ProductConfiguration

        [HttpPost("AddProductConfiguration")]
        public IActionResult AddProductConfiguration([FromBody]ProductConfigurationRequest Request)
        {
            ProductConfigurationResponse Response = new ProductConfigurationResponse();
            try
            {
                long ProductId = _transactionConfigService.AddProductConfiguration(Request);

                if (ProductId != 0)
                {
                    Response.response = new ProductConfigurationInfo() { ProductId = ProductId };
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
        [HttpPost("UpdateProductConfiguration")]
        public IActionResult UpdateProductConfiguration([FromBody]ProductConfigurationRequest Request)
        {
            ProductConfigurationResponse Response = new ProductConfigurationResponse();
            try
            {
                long ProductId = _transactionConfigService.UpdateProductConfiguration(Request);

                if (ProductId != 0)
                {
                    Response.response = new ProductConfigurationInfo() { ProductId = ProductId };
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
        [HttpGet("GetProductConfiguration/{ProductId}")]
        public IActionResult GetProductConfiguration(long ProductId)
        {
            ProductConfigurationGetResponse Response = new ProductConfigurationGetResponse();
            try
            {
                var responsedata = _transactionConfigService.GetProductConfiguration(ProductId);
                if (responsedata != null)
                {
                    Response.ReturnCode = enResponseCode.Success;
                    Response.response = responsedata;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
        [HttpGet("GetAllProductConfiguration")]
        public IActionResult GetAllProductConfiguration()
        {
            ProductConfigurationGetAllResponse Response = new ProductConfigurationGetAllResponse();
            try
            {
                var responsedata = _transactionConfigService.GetAllProductConfiguration();
                if (responsedata.Count != 0)
                {
                    Response.ReturnCode = enResponseCode.Success;
                    Response.response = responsedata;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
        [HttpPost("SetActiveProduct/{ProductId}")]
        public IActionResult SetActiveProduct(long ProductId)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                var responsedata = _transactionConfigService.SetActiveProduct(ProductId);
                if (responsedata == 1)
                {
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
        [HttpPost("SetInActiveProduct/{ProductId}")]
        public IActionResult SetInActiveProduct(long ProductId)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                var responsedata = _transactionConfigService.SetInActiveProduct(ProductId);
                if (responsedata == 1)
                {
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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

        #region RouteConfiguration
        [HttpPost("AddRouteConfiguration")]
        public IActionResult AddRouteConfiguration([FromBody]RouteConfigurationRequest Request)
        {
            RouteConfigurationResponse Response = new RouteConfigurationResponse();
            try
            {
                long RouteId = _transactionConfigService.AddRouteConfiguration(Request);

                if (RouteId != 0)
                {
                    Response.response = new RouteConfigurationInfo() { RouteId = RouteId };
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
        [HttpPost("UpdateRouteConfiguration")]
        public IActionResult UpdateRouteConfiguration([FromBody]RouteConfigurationRequest Request)
        {
            RouteConfigurationResponse Response = new RouteConfigurationResponse();
            try
            {
                long RouteId = _transactionConfigService.UpdateRouteConfiguration(Request);

                if (RouteId != 0)
                {
                    Response.response = new RouteConfigurationInfo() { RouteId = RouteId };
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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

        [HttpGet("GetRouteConfiguration/{RouteId}")]
        public IActionResult GetRouteConfiguration(long RouteId)
        {
            RouteConfigurationGetResponse Response = new RouteConfigurationGetResponse();
            try
            {
                var responsedata = _transactionConfigService.GetRouteConfiguration(RouteId);
                if (responsedata != null)
                {
                    Response.ReturnCode = enResponseCode.Success;
                    Response.response = responsedata;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
        [HttpGet("GetAllRouteConfiguration")]
        public IActionResult GetAllRouteConfiguration()
        {
            RouteConfigurationGetAllResponse Response = new RouteConfigurationGetAllResponse();
            try
            {
                var responsedata = _transactionConfigService.GetAllRouteConfiguration();
                if (responsedata.Count != 0)
                {
                    Response.ReturnCode = enResponseCode.Success;
                    Response.response = responsedata;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
        [HttpPost("SetActiveRoute/{RouteId}")]
        public IActionResult SetActiveRoute(long RouteId)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                var responsedata = _transactionConfigService.SetActiveRoute(RouteId);
                if (responsedata == 1)
                {
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
        [HttpPost("SetInActiveRoute/{RouteId}")]
        public IActionResult SetInActiveRoute(long RouteId)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                var responsedata = _transactionConfigService.SetInActiveRoute(RouteId);
                if (responsedata == 1)
                {
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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

        #region ThirdPartyAPIConfiguration

        [HttpGet("GetAllThirdPartyAPI")]
        public IActionResult GetAllThirdPartyAPI()
        {
            ThirdPartyAPIConfigResponseAllData res = new ThirdPartyAPIConfigResponseAllData();
            try
            {
                res.response  = _transactionConfigService.GetAllThirdPartyAPIConfig();
                if (res.response.Count ==0)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
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

        [HttpGet("GetThirdPartyAPIById/{Id:long}")]
        public IActionResult GetThirdPartyAPIById(long Id)
        {
            ThirdPartyAPIConfigResponse res = new ThirdPartyAPIConfigResponse();
            try
            {
                res.response = _transactionConfigService.GetThirdPartyAPIConfigById(Id);
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
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

        [HttpPost("AddThirdPartyAPIConfig")]
        public IActionResult AddThirdPartyAPIConfig([FromBody]ThirdPartyAPIConfigRequest Request)
        {
            ThirdPartyAPIConfigResponse  Response = new ThirdPartyAPIConfigResponse();
            try
            {
                long Id= _transactionConfigService.AddThirdPartyAPI(Request);
                if (Id != 0)
                {
                    Response.response = _transactionConfigService.GetThirdPartyAPIConfigById(Id);
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    //Response.ErrorCode = enErrorCode.NoDataFound;
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

        [HttpPost("UpdateThirdPartyAPIConfig")]
        public IActionResult UpdateThirdPartyAPIConfig([FromBody]ThirdPartyAPIConfigRequest request)
        {
            ThirdPartyAPIConfigResponse res = new ThirdPartyAPIConfigResponse();
            bool state = false;
            try
            {
                if (request.Id == 0)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.InValid_ID;
                    return Ok(res);
                }
                state = _transactionConfigService.UpdateThirdPartyAPI(request);
                if (state == false)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(res);
                }
                res.response = _transactionConfigService.GetThirdPartyAPIConfigById(request.Id);
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
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

        [HttpPost("SetActiveThirdPartyAPIConfig/{id:long}")]
        public IActionResult SetActiveThirdPartyAPIConfig(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetActiveThirdPartyAPI(id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        [HttpPost("SetInActiveThirdPartyAPIConfig/{id:long}")]
        public IActionResult SetInActiveThirdPartyAPIConfig(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetInActiveThirdPartyAPI(id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                }

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

        #region ThirdPartyAPIResponse

        [HttpGet("GetAllThirdPartyAPIRespose")]
        public IActionResult GetAllThirdPartyAPIRespose()
        {
            ThirdPartyAPIResponseConfigResponseAllData res = new ThirdPartyAPIResponseConfigResponseAllData();
            try
            {
                res.response = _transactionConfigService.GetAllThirdPartyAPIResponse();
                if (res.response.Count == 0)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
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

        [HttpGet("GetThirdPartyAPIResposeById/{Id:long}")]
        public IActionResult GetThirdPartyAPIResposeById(long Id)
        {
            ThirdPartyAPIResponseConfigResponse  res = new ThirdPartyAPIResponseConfigResponse();
            try
            {
                res.response = _transactionConfigService.GetThirdPartyAPIResponseById(Id);
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
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

        [HttpPost("AddThirdPartyAPIRespose")]
        public IActionResult AddThirdPartyAPIRespose([FromBody]ThirdPartyAPIResponseConfigRequest Request)
        {
            ThirdPartyAPIResponseConfigResponse  Response = new ThirdPartyAPIResponseConfigResponse();
            try
            {
                long Id = _transactionConfigService.AddThirdPartyAPIResponse(Request);
                if (Id != 0)
                {
                    Response.response = _transactionConfigService.GetThirdPartyAPIResponseById(Id);
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    //Response.ErrorCode = enErrorCode.NoDataFound;
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

        [HttpPost("UpdateThirdPartyAPIResponse")]
        public IActionResult UpdateThirdPartyAPIResponse([FromBody]ThirdPartyAPIResponseConfigRequest request)
        {
            ThirdPartyAPIResponseConfigResponse  res = new ThirdPartyAPIResponseConfigResponse();
            bool state = false;
            try
            {
                if (request.Id == 0)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.InValid_ID;
                    return Ok(res);
                }
                state = _transactionConfigService.UpdateThirdPartyAPIResponse (request);
                if (state == false)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(res);
                }
                res.response = _transactionConfigService.GetThirdPartyAPIResponseById(request.Id);
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
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

        [HttpPost("SetActiveThirdPartyAPIResponse/{id:long}")]
        public IActionResult SetActiveThirdPartyAPIResponse(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetActiveThirdPartyAPIResponse (id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        [HttpPost("SetInActiveThirdPartyAPIResponse/{id:long}")]
        public IActionResult SetInActiveThirdPartyAPIResponse(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetInActiveThirdPartyAPIResponse (id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                }

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

        #region TradePairConfiguration
        [HttpPost("AddPairConfiguration")]
        public IActionResult AddPairConfiguration([FromBody]TradePairConfigRequest Request)
        {
            TradePairConfigResponse Response = new TradePairConfigResponse();
            try
            {
                long PairId = _transactionConfigService.AddPairConfiguration(Request);

                if (PairId != 0)
                {
                    Response.response = new TradePairConfigInfo() { PairId = PairId };
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
        [HttpPost("UpdatePairConfiguration")]
        public IActionResult UpdatePairConfiguration([FromBody]TradePairConfigRequest Request)
        {
            TradePairConfigResponse Response = new TradePairConfigResponse();
            try
            {
                if (Request.Id == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    //Response.ErrorCode = enErrorCode.InValid_ID;
                    return Ok(Response);
                }
                long PairId = _transactionConfigService.UpdatePairConfiguration(Request);

                if (PairId != 0)
                {
                    Response.response = new TradePairConfigInfo() { PairId = PairId };
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
        [HttpGet("GetPairConfiguration")]
        public IActionResult GetPairConfiguration(long PairId)
        {
            TradePairConfigGetResponse Response = new TradePairConfigGetResponse();
            try
            {
                if (PairId == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    //Response.ErrorCode = enErrorCode.InValid_ID;
                    return Ok(Response);
                }
                var responsedata = _transactionConfigService.GetPairConfiguration(PairId);
                if (responsedata != null)
                {
                    Response.ReturnCode = enResponseCode.Success;
                    Response.response = responsedata;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
        [HttpGet("GetAllPairConfiguration")]
        public IActionResult GetAllPairConfiguration()
        {
            TradePairConfigGetAllResponse Response = new TradePairConfigGetAllResponse();
            try
            {
                var responsedata = _transactionConfigService.GetAllPairConfiguration();
                if (responsedata != null)
                {
                    Response.ReturnCode = enResponseCode.Success;
                    Response.response = responsedata;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
        [HttpPost("SetActivePair")]
        public IActionResult SetActivePair(long PairId)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                if (PairId == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    //Response.ErrorCode = enErrorCode.InValid_ID;
                    return Ok(Response);
                }
                var responsedata = _transactionConfigService.SetActivePair(PairId);
                if (responsedata == 1)
                {
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
        [HttpPost("SetInActivePair/PairId")]
        public IActionResult SetInActivePair(long PairId)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                if (PairId == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    //Response.ErrorCode = enErrorCode.InValid_ID;
                    return Ok(Response);
                }
                var responsedata = _transactionConfigService.SetInActivePair(PairId);
                if (responsedata == 1)
                {
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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

        #region Other Configuration
        [HttpGet("GetAllServiceTypeMaster")]
        public IActionResult GetAllServiceTypeMaster()
        {
            ServiceTypeMasterResponse Response = new ServiceTypeMasterResponse();
            try
            {
                var responsedata = _transactionConfigService.GetAllServiceTypeMaster();
                if (responsedata != null)
                {
                    Response.ReturnCode = enResponseCode.Success;
                    Response.response = responsedata;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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
        [HttpGet("GetAllTransactionType")]
        public IActionResult GetAllTransactionType()
        {
            TransactionTypeResponse Response = new TransactionTypeResponse();
            try
            {
                var responsedata = _transactionConfigService.GetAllTransactionType();
                if (responsedata != null)
                {
                    Response.ReturnCode = enResponseCode.Success;
                    Response.response = responsedata;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
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

        #region Limit

        [HttpGet("GetAllLimitData")]
        public IActionResult GetAllLimitData()
        {
            LimitResponseAllData  res = new LimitResponseAllData();
            try
            {
                res.response = _transactionConfigService.GetAllLimitData();
                if (res.response.Count == 0)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
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

        [HttpGet("GetLimitsById/{Id:long}")]
        public IActionResult GetLimitsById(long Id)
        {
            LimitResponse  res = new LimitResponse();
            try
            {
                res.response = _transactionConfigService.GetLimitById(Id);
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
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

        [HttpPost("AddLimits")]
        public IActionResult AddLimits([FromBody]LimitRequest  Request)
        {
            LimitResponse  Response = new LimitResponse();
            try
            {
                long Id = _transactionConfigService.AddLimitData(Request);
                if (Id != 0)
                {
                    Response.response = _transactionConfigService.GetLimitById(Id);
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    //Response.ErrorCode = enErrorCode.NoDataFound;
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

        [HttpPost("UpdateLimits")]
        public IActionResult UpdateLimits([FromBody]LimitRequest request)
        {
            LimitResponse  res = new LimitResponse();
            bool state = false;
            try
            {
                if (request.Id == 0)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.InValid_ID;
                    return Ok(res);
                }
                state = _transactionConfigService.UpdateLimitData(request);
                if (state == false)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(res);
                }
                res.response = _transactionConfigService.GetLimitById(request.Id);
                if (res.response == null)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
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

        [HttpPost("SetActiveLimit/{id:long}")]
        public IActionResult SetActiveLimit(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetActiveLimit (id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                res.ReturnCode = enResponseCode.InternalError;
                return Ok(res);
            }
        }

        [HttpPost("SetInActiveLimit/{id:long}")]
        public IActionResult SetInActiveLimit(long id)
        {
            BizResponseClass res = new BizResponseClass();
            try
            {
                var responce = _transactionConfigService.SetInActiveLimit (id);
                if (responce == true)
                    res.ReturnCode = enResponseCode.Success;
                else
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.NoDataFound;
                }

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