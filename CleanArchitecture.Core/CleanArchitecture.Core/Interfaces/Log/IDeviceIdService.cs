﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.ViewModels.AccountViewModels.Log;

namespace CleanArchitecture.Core.Interfaces.Log
{
    public interface IDeviceIdService
    {
        Task<long> AddDeviceId(DeviceMasterViewModel model);
    }
}
