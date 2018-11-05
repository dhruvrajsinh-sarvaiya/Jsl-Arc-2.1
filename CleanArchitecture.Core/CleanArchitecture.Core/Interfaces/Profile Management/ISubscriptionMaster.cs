using CleanArchitecture.Core.ViewModels.Profile_Management;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.Profile_Management
{
    public interface ISubscriptionMaster
    {
        long AddSubscription(SubscriptionViewModel model);
        bool GetSubscriptionData(int UserId,long ProfileId);
        long AddMultiSubscription(int UserId,long ProfileId);
    }
}
