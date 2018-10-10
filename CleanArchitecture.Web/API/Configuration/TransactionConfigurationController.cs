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
                if(responsedata==1)
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

        //providermaster
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

        [HttpGet("GetProviderListById/{id:long}")]
        public IActionResult GetProviderListById(long id)
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

        //Apptype
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
                    res.ReturnCode = enResponseCode.Fail ;
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

        [HttpPost("UpdateAppType")]
        public IActionResult UpdateAppType([FromBody]AppTypeRequest request)
        {
            AppTypeResponceData res = new AppTypeResponceData();
            bool state = false;
            try
            {

                state=_transactionConfigService.UpdateAppType(request);
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

        //providerType
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

        [HttpPost("UpdateProviderType")]
        public IActionResult UpdateProviderType([FromBody]ProviderTypeRequest request)
        {
            ProviderTypeResponceData res = new ProviderTypeResponceData();
            bool state = false;
            try
            {
                state=_transactionConfigService.UpdateProviderType(request);
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

    }
}