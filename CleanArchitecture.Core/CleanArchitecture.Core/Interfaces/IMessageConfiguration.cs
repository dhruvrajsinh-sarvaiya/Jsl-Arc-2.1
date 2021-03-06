﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IMessageConfiguration
    {
        Task<IQueryable> GetAPIConfigurationAsync(long ServiceTypeID, long CommServiceTypeID);
        
        Task<IQueryable> GetTemplateConfigurationAsync(long ServiceTypeID, int TemplateID, long CommServiceID);
    }
}
