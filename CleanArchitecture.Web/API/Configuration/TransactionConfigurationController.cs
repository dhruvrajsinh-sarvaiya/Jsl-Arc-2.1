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

        [HttpGet("GetProviderList")]
        public IActionResult GetProviderList()
        {
            ServiceProviderResponce res = new ServiceProviderResponce();
            res.responce = _transactionConfigService.GetAllProvider();
            res.ReturnCode = enResponseCode.Success;
            return Ok(res);
        }

        [HttpGet("GetProviderListById/{id:long}")]
        public IActionResult GetProviderListById(long id)
        {
            ServiceProviderResponceData res = new ServiceProviderResponceData();

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

        [HttpPost("AddServiceProvider")]
        public IActionResult AddServiceProvider(ServiceProviderRequest request)
        {
            _transactionConfigService.AddProviderService(request);
            return Ok();
        }

        [HttpGet("GetAppType")]
        public IActionResult GetAppType()
        {
            AppTypeResponce res = new AppTypeResponce();
            res.responce = _transactionConfigService.GetAppType();
            res.ReturnCode = enResponseCode.Success;
            return Ok(res);
        }

        [HttpGet("GetAppTypeById/{id:long}")]
        public IActionResult GetAppTypeById(long id)
        {
            AppTypeResponceData res = new AppTypeResponceData();
            res.responce = _transactionConfigService.GetAppTypeById(id);
            res.ReturnCode = enResponseCode.Success;
            return Ok(res);
        }

        [HttpGet("GetServiceProviderType")]
        public IActionResult GetServiceProviderType()
        {
            ProviderTypeResponce res = new ProviderTypeResponce();
            res.responce = _transactionConfigService.GetProviderType();
            res.ReturnCode = enResponseCode.Success;
            return Ok(res);
        }

        [HttpGet("GetServiceProviderTypeById/{id:long}")]
        public IActionResult GetServiceProviderTypeById(long id)
        {
            ProviderTypeResponceData res = new ProviderTypeResponceData();
            res.responce = _transactionConfigService.GetProviderTypeById(id);
            res.ReturnCode = enResponseCode.Success;
            return Ok(res);
        }
    }
}