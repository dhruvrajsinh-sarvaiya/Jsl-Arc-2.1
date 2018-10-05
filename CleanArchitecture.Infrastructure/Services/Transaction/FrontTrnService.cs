using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
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
            List<ActiveOrderDataResponse> ActiveOrderList = _frontTrnRepository.GetActiveOrder(MemberID);

            var response = ActiveOrderList.ConvertAll(x => new GetActiveOrderInfo { order_id = x.Id, pair_name = x.Order_Currency, price = x.Price, side = x.Type});

            return response; 
        }
    }
}
