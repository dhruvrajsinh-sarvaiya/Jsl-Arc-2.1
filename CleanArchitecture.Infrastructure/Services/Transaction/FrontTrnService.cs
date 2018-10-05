using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.Transaction
{
    public class FrontTrnService : IFrontTrnService
    {
        private readonly IFrontTrnRepository _frontTrnRepository;
        public FrontTrnService(IFrontTrnRepository frontTrnRepository)
        {
            _frontTrnRepository = frontTrnRepository;
        }
        public List<GetActiveOrderInfo> GetActiveOrder(long MemberID)
        {
            List<GetActiveOrderInfo> ActiveOrderList = _frontTrnRepository.GetActiveOrder(MemberID);
            return ActiveOrderList; 
        }
    }
}
