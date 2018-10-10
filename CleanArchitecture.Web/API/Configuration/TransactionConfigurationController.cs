using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.API.Configuration
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionConfigurationController : ControllerBase
    {
        private readonly ITransactionConfigService _transactionConfigService;
        public TransactionConfigurationController(ITransactionConfigService transactionConfigService)
        {
            _transactionConfigService = transactionConfigService;
        }

    }
}