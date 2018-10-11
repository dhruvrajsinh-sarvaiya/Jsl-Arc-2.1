using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Events
{
    class WalletDrEvent<T> : BaseDomainEvent
    {
        public T WalletStatus;
        public WalletDrEvent(T walletStatus)
        {
            WalletStatus = walletStatus;
        }
    }

    class WalletCrEvent<T> : BaseDomainEvent
    {
        public T WalletStatus;
        public WalletCrEvent(T walletStatus)
        {
            WalletStatus = walletStatus;
        }
    }
    class WalletStatusDisable<T> : BaseDomainEvent
    {
        public T WalletStatus;
        public WalletStatusDisable(T walletStatus)
        {
            WalletStatus = walletStatus;
        }
    }
}
