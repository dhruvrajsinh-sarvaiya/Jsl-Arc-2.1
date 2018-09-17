using CleanArchitecture.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    public class MessageConfiguration : IMessageConfiguration
    {
        public Task GetAPIConfigurationAsync(long ServiceTypeID, long CommServiceTypeID, long CommSerproID, long CommServiceID, long APIId, int priority)
        {
            return Task.FromResult(0);
        }

        public Task GetTemplateConfigurationAsync(long ServiceTypeID, long CommServiceID, int TemplateID)
        {
            return Task.FromResult(0);
        }
    }
}
