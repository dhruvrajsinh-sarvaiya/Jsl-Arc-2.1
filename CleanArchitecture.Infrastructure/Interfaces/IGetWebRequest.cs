﻿using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Infrastructure.DTOClasses;

namespace CleanArchitecture.Infrastructure.Interfaces
{
    public interface IGetWebRequest
    {
        ThirdPartyAPIRequest MakeWebRequest(long routeID, long thirdpartyID, long serproDetailID, TransactionQueue TQ=null);

    }
}
