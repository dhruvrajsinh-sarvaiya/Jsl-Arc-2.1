using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces.Configuration;
using CleanArchitecture.Core.ViewModels.WalletConfiguration;
using CleanArchitecture.Infrastructure.Services.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanArchitecture.Web.API.Configuration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WalletConfigurationController : Controller
    {
        //vsolanki 11-10-2018
        #region "DI"

        private readonly IWalletConfigurationService _walletConfigurationService;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region "cotr"

        public WalletConfigurationController(IWalletConfigurationService walletConfigurationService, UserManager<ApplicationUser> userManager)
        {
            _walletConfigurationService = walletConfigurationService;
            _userManager = userManager;
        }

        #endregion

        #region "Methods"

        //vsolanki 11-10-2018
        #region "WalletTypeMaster"
        [HttpGet]
        public IActionResult ListAllWalletTypeMaster()
        {
            try
            {
                var items = _walletConfigurationService.ListAllWalletTypeMaster();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass
                {
                    ReturnCode = enResponseCode.InternalError,
                    ReturnMsg = ex.ToString(),
                    ErrorCode = enErrorCode.Status500InternalServerError
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddWalletTypeMaster(WalletTypeMasterRequest addWalletTypeMasterRequest)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                long Userid = 8/*user.Id*/;
                var items = _walletConfigurationService.AddWalletTypeMaster(addWalletTypeMasterRequest, Userid);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass
                {
                    ReturnCode = enResponseCode.InternalError,
                    ReturnMsg = ex.ToString(),
                    ErrorCode = enErrorCode.Status500InternalServerError
                });
            }
        }

        [HttpPut("{WalletTypeId}")]
        public async Task<IActionResult> UpdateWalletTypeMaster(WalletTypeMasterRequest updateWalletTypeMasterRequest, long WalletTypeId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                long Userid = 8/*user.Id*/;
                var items = _walletConfigurationService.UpdateWalletTypeMaster(updateWalletTypeMasterRequest, Userid, WalletTypeId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass
                {
                    ReturnCode = enResponseCode.InternalError,
                    ReturnMsg = ex.ToString(),
                    ErrorCode = enErrorCode.Status500InternalServerError
                });
            }
        }

        [HttpDelete("{WalletTypeId}")]
        public IActionResult DeleteWalletTypeMaster(long WalletTypeId)
        {
            try
            {
                var items = _walletConfigurationService.DisableWalletTypeMaster(WalletTypeId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass
                {
                    ReturnCode = enResponseCode.InternalError,
                    ReturnMsg = ex.ToString(),
                    ErrorCode = enErrorCode.Status500InternalServerError
                });
            }
        }

        [HttpGet("{WalletTypeId}")]
        public IActionResult GetWalletTypeMasterById(long WalletTypeId)
        {
           try
            {
                var items = _walletConfigurationService.GetWalletTypeMasterById(WalletTypeId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass
                {
                    ReturnCode = enResponseCode.InternalError,
                    ReturnMsg = ex.ToString(),
                    ErrorCode = enErrorCode.Status500InternalServerError
                });
            }
        }

        #endregion

        #region "WalletTypeMaster"

        //vsoalnki 2018-11-2
        [HttpGet("{Coin}")]
        public IActionResult GetTransferIn(string Coin, DateTime? FromDate, DateTime? ToDate, short Status)
        {
            try
            {
                var items = _walletConfigurationService.GetTransferIn(Coin, FromDate, ToDate, Status);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass
                {
                    ReturnCode = enResponseCode.InternalError,
                    ReturnMsg = ex.ToString(),
                    ErrorCode = enErrorCode.Status500InternalServerError
                });
            }
        }

        #endregion


        #endregion
    }
}
