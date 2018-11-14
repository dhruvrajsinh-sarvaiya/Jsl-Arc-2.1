using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.ViewModels.Configuration;
using CleanArchitecture.Core.ViewModels.Wallet;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Interfaces
{
    public interface ICommunicationService
    {
        #region "CommServiceTypeMaster"

        //vsoalnki 13-11-2018
        CommServiceTypeRes GetAllCommServiceTypeMaster();

        #endregion

        #region "TemplateMaster"

        //vsoalnki 13-11-2018
        ListTemplateMasterRes GetAllTemplateMaster();
        BizResponseClass AddTemplateMaster(TemplateMasterReq Request, long userid);
        BizResponseClass UpdateTemplateMaster(long TemplateMasterId, TemplateMasterReq Request, long userid);
        BizResponseClass DisableTemplateMaster(long TemplateMasterId);
        TemplateMasterRes GetTemplateMasterById(long TemplateMasterId);

        #endregion

        #region "MessagingQueue"

        //vsolanki 13-11-2018
        ListMessagingQueueRes GetMessagingQueue(DateTime FromDate, DateTime ToDate, short? Status, long? MobileNo, int Page);

        #endregion

        #region "EmailQueue"

        //vsolanki 14-11-2018
        ListEmailQueueRes GetEmailQueue(DateTime FromDate, DateTime ToDate, short? Status, string Email, int Page);

        #endregion

        #region "WalletLedger"

        //vsolanki 14-11-2018
        ListWalletLedgerResponse GetWalletLedger(DateTime FromDate, DateTime ToDate, long WalletId, int page, int? PageSize);

        #endregion

        #region "LimitRuleMaster"

        //vsoalnki 13-11-2018
        ListLimitRuleMasterRes GetAllLimitRule();

        //vsoalnki 13-11-2018
        BizResponseClass AddLimitRule(LimitRuleMasterReq Request, long userid);

        //vsoalnki 13-11-2018
        BizResponseClass UpdateLimitRule(long LimitRuleMasterId, LimitRuleMasterReq Request, long userid);

        //vsoalnki 13-11-2018
        BizResponseClass DisableLimitRule(long LimitRuleMasterId);

        //vsoalnki 13-11-2018
        ListLimitRuleMasterRes GetLimitRuleById(long LimitRuleMasterId);

        #endregion
    }
}
