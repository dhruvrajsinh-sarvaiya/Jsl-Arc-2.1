using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Configuration;
using CleanArchitecture.Infrastructure.Services.Configuration;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanArchitecture.Web.API.Configuration
{
    [Route("api/[controller]/[action]")]
    public class WalletConfigurationController : Controller
    {
        private readonly IWalletConfigurationService _walletConfigurationService;
        public WalletConfigurationController(IWalletConfigurationService walletConfigurationService)
        {
            _walletConfigurationService = walletConfigurationService;
        }
       [HttpGet]
        public IActionResult ListAllWalletTypeMaster()
        {
            var items = _walletConfigurationService.ListAllWalletTypeMaster();
            return Ok(items);
        }
    }
}
