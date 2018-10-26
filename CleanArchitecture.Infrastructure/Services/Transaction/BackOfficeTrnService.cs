using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.Transaction.BackOffice;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.Transaction
{
    public class BackOfficeTrnService : IBackOfficeTrnService
    {
        private readonly ILogger<FrontTrnService> _logger;
        private readonly IBackOfficeTrnRepository _backOfficeTrnRepository;

        public BackOfficeTrnService(ILogger<FrontTrnService> logger, IBackOfficeTrnRepository backOfficeTrnRepository)
        {
            _logger = logger;
            _backOfficeTrnRepository = backOfficeTrnRepository;
        }

        public List<TradingSummaryViewModel> GetTradingSummary(long MemberID, string FromDate, string ToDate, long TrnNo, short status, string SMSCode, long PairID, short trade)
        {
            try
            {
                List<TradingSummaryViewModel> list = new List<TradingSummaryViewModel>();
                var Modellist = _backOfficeTrnRepository.GetTradingSummary(MemberID, FromDate, ToDate, TrnNo, status, SMSCode, PairID, trade);
                if (Modellist == null)
                    return null;

                foreach (var model in Modellist)
                {
                    list.Add(new TradingSummaryViewModel()
                    {
                        Amount = model.Amount,
                        ChargeRs = model.ChargeRs,
                        DateTime = model.DateTime.Date,
                        MemberID =model .MemberID,
                        PairID =model .PairID,
                        PairName =model .PairName,
                        PostBal =model .PostBal,
                        PreBal =model .PreBal,
                        Price =model .Price,
                        StatusText =model .StatusText,
                        Total = model.Type == "BUY" ? ((model.Price * model.Amount) - model.ChargeRs) : ((model.Price * model.Amount)),
                        TrnNo =model .TrnNo,
                        Type =model .Type 
                    });
                }

                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
    }
}
