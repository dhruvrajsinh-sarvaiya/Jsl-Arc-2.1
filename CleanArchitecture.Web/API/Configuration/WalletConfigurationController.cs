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
        private readonly IWalletConfigurationService _walletConfigurationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public WalletConfigurationController(IWalletConfigurationService walletConfigurationService, UserManager<ApplicationUser> userManager)
        {
            _walletConfigurationService = walletConfigurationService;
            _userManager=userManager;
    }
        [HttpGet]
        public IActionResult ListAllWalletTypeMaster()
        {
            var items = _walletConfigurationService.ListAllWalletTypeMaster();
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalletTypeMasterAsync(AddWalletTypeMasterRequest addWalletTypeMasterRequest)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            long Userid= 8/*user.Id*/;
            var items = _walletConfigurationService.AddWalletTypeMaster(addWalletTypeMasterRequest, Userid);
            return Ok(items);
        }
    }
}
