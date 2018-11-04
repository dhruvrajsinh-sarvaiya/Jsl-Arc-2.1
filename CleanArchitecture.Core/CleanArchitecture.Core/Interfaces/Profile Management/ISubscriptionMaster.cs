using CleanArchitecture.Core.ViewModels.Profile_Management;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.Profile_Management
{
    public interface ISubscriptionMaster
    {
        long AddSubscription(SubscriptionViewModel model);
    }
}
