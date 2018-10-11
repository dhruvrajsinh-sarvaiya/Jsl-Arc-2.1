using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Interfaces.Configuration;
using CleanArchitecture.Core.ViewModels.WalletConfiguration;
using CleanArchitecture.Infrastructure.Services.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanArchitecture.Web.API.Configuration
{
    [Route("api/[controller]/[action]")]
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
            var items = _walletConfigurationService.ListAllWalletTypeMaster();
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalletTypeMaster(WalletTypeMasterRequest addWalletTypeMasterRequest)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            long Userid = 8/*user.Id*/;
            var items = _walletConfigurationService.AddWalletTypeMaster(addWalletTypeMasterRequest, Userid);
            return Ok(items);
        }

        [HttpPut("{WalletTypeId}")]
        public async Task<IActionResult> UpdateWalletTypeMaster(WalletTypeMasterRequest updateWalletTypeMasterRequest, long WalletTypeId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            long Userid = 8/*user.Id*/;
            var items = _walletConfigurationService.UpdateWalletTypeMaster(updateWalletTypeMasterRequest, Userid, WalletTypeId);
            return Ok(items);
        }

        [HttpDelete("{WalletTypeId}")]
        public IActionResult DeleteWalletTypeMaster(long WalletTypeId)
        {
           var items= _walletConfigurationService.DisableWalletTypeMaster(WalletTypeId);
            return Ok(items);
        }

        [HttpGet("{WalletTypeId}")]
        public IActionResult GetWalletTypeMasterById(long WalletTypeId)
        {
            var items = _walletConfigurationService.GetWalletTypeMasterById(WalletTypeId);
            return Ok(items);
        }

        #endregion

        #endregion
    }
}
