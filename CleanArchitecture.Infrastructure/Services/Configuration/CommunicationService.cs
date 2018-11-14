using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels.Configuration;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CleanArchitecture.Core.Interfaces.Configuration;
using CleanArchitecture.Core.ViewModels.Wallet;
using CleanArchitecture.Core.Entities.Wallet;

namespace CleanArchitecture.Infrastructure.Services.Configuration
{
    public class CommunicationService : BasePage, ICommunicationService
    {
        #region "Variables"

        private readonly ICommonRepository<CommServiceTypeMaster> _CommServiceTypeMaster;
        private readonly ICommonRepository<LimitRuleMaster> _LimitRuleMaster;
        private readonly ICommonRepository<TemplateMaster> _TemplateMaster;
        private readonly ILogger<CommunicationService> _log;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly IMasterConfigurationRepository _masterConfigurationRepository;

        #endregion

        #region "cotr"

        public CommunicationService(ILogger<CommunicationService> log, ILogger<BasePage> logger, ICommonRepository<CommServiceTypeMaster> CommServiceTypeMaster, ICommonRepository<TemplateMaster> TemplateMaster, CleanArchitectureContext dbContext, Microsoft.Extensions.Configuration.IConfiguration configuration, IMasterConfigurationRepository masterConfigurationRepository, ICommonRepository<LimitRuleMaster> LimitRuleMaster) : base(logger)
        {
            _CommServiceTypeMaster = CommServiceTypeMaster;
            _TemplateMaster = TemplateMaster;
            _log = log;
            _masterConfigurationRepository = masterConfigurationRepository;
            _configuration = configuration;
            _LimitRuleMaster = LimitRuleMaster;
        }

        #endregion

        #region "CommServiceTypeMaster"

        //vsoalnki 13-11-2018
        public CommServiceTypeRes GetAllCommServiceTypeMaster()
        {
            CommServiceTypeRes res = new CommServiceTypeRes();
            try
            {
                // var items = _CommServiceTypeMaster.FindBy(i => i.Status == Convert.ToInt16(ServiceStatus.Active));
                var items = _CommServiceTypeMaster.List();
                if (items.Count == 0)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.RecordNotFound;
                    res.ReturnMsg = EnResponseMessage.NotFound;
                    return res;
                }
                res.ReturnCode = enResponseCode.Success;
                res.ErrorCode = enErrorCode.Success;
                res.ReturnMsg = EnResponseMessage.FindRecored;
                res.Response = items;
                return res;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        #endregion

        #region "TemplateMaster"

        //vsoalnki 13-11-2018
        public ListTemplateMasterRes GetAllTemplateMaster()
        {
            ListTemplateMasterRes res = new ListTemplateMasterRes();
            try
            {
                var items = _masterConfigurationRepository.GetAllTemplateMaster();
                //var items = _TemplateMaster.List();
                // var items = _TemplateMaster.FindBy(i => i.Status == Convert.ToInt16(ServiceStatus.Active));
                if (items.Count == 0)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.RecordNotFound;
                    res.ReturnMsg = EnResponseMessage.NotFound;
                    return res;
                }
                res.ReturnCode = enResponseCode.Success;
                res.ErrorCode = enErrorCode.Success;
                res.ReturnMsg = EnResponseMessage.FindRecored;
                res.TemplateMasterObj = items;
                return res;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsoalnki 13-11-2018
        public BizResponseClass AddTemplateMaster(TemplateMasterReq Request, long userid)
        {
            try
            {
                TemplateMaster template = new TemplateMaster();
                if (Request != null)
                {
                    template.Status = Convert.ToInt16(ServiceStatus.Active);
                    template.UpdatedDate = UTC_To_IST();
                    template.CreatedDate = UTC_To_IST();
                    template.CreatedBy = userid;

                    template.TemplateID = Convert.ToInt64(Request.TemplateID);
                    template.TemplateName = Request.TemplateName;
                    template.AdditionalInfo = Request.AdditionalInfo;
                    template.CommServiceTypeID = Request.CommServiceTypeID;
                    template.Content = Request.Content;

                    _TemplateMaster.Add(template);

                    return new BizResponseClass { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.RecordAdded, ErrorCode = enErrorCode.Success };
                }
                return new BizResponseClass { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidInput, ErrorCode = enErrorCode.InvalidInput };
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsoalnki 13-11-2018
        public BizResponseClass UpdateTemplateMaster(long TemplateMasterId, TemplateMasterReq Request, long userid)
        {
            try
            {
                var template = _TemplateMaster.GetSingle(i => i.Id == TemplateMasterId);
                //TemplateMaster template = new TemplateMaster();
                if (Request != null)
                {
                    template.Status = Convert.ToInt16(ServiceStatus.Active);
                    template.UpdatedDate = UTC_To_IST();
                    template.CreatedDate = template.CreatedDate; //UTC_To_IST();
                    template.CreatedBy = template.CreatedBy;// userid;
                    template.UpdatedBy = userid;

                    template.TemplateID = Convert.ToInt64(Request.TemplateID);
                    template.TemplateName = Request.TemplateName;
                    template.AdditionalInfo = Request.AdditionalInfo;
                    template.CommServiceTypeID = Request.CommServiceTypeID;
                    template.Content = Request.Content;

                    _TemplateMaster.Update(template);

                    return new BizResponseClass { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.RecordUpdated, ErrorCode = enErrorCode.Success };
                }
                return new BizResponseClass { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidInput, ErrorCode = enErrorCode.InvalidInput };
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsoalnki 13-11-2018
        public BizResponseClass DisableTemplateMaster(long TemplateMasterId)
        {
            try
            {
                var template = _TemplateMaster.GetById(TemplateMasterId);
                if (template != null)
                {
                    //disable status
                    template.DisableService();
                    //update in DB
                    _TemplateMaster.Update(template);

                    return new BizResponseClass { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.RecordDisable, ErrorCode = enErrorCode.Success };
                }
                return new BizResponseClass { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidInput, ErrorCode = enErrorCode.InvalidInput };
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsoalnki 13-11-2018
        public TemplateMasterRes GetTemplateMasterById(long TemplateMasterId)
        {
            TemplateMasterRes res = new TemplateMasterRes();
            try
            {
                var template = _masterConfigurationRepository.GetTemplateMasterById(TemplateMasterId);
                // var template = _TemplateMaster.GetById(TemplateMasterId);
                if (template != null)
                {
                    res.ReturnCode = enResponseCode.Success;
                    res.ErrorCode = enErrorCode.Success;
                    res.ReturnMsg = EnResponseMessage.FindRecored;
                    res.TemplateMasterObj = template;
                    return res;
                }
                res.ReturnCode = enResponseCode.Fail;
                res.ErrorCode = enErrorCode.RecordNotFound;
                res.ReturnMsg = EnResponseMessage.NotFound;
                return res;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        #endregion

        #region "MessagingQueue"

        //vsolanki 13-11-2018
        public ListMessagingQueueRes GetMessagingQueue(DateTime FromDate, DateTime ToDate, short? Status, long? MobileNo, int Page)
        {
            try
            {
                ListMessagingQueueRes res = new ListMessagingQueueRes();
                int Customday = Convert.ToInt32(_configuration["ReportValidDays"]);//2;
                double days = (ToDate - FromDate).TotalDays;
                if (Customday < days)
                {
                    var msg = EnResponseMessage.MoreDays;
                    msg = msg.Replace("#X#", Customday.ToString());

                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.MoreDays;
                    res.ReturnMsg = msg;
                    return res;
                }

                var items = _masterConfigurationRepository.GetMessagingQueue(FromDate, ToDate, Status, MobileNo, Page);
                if (items.Count != 0)
                {
                    if (Page > 0)
                    {
                        int skip = Helpers.PageSize * (Page - 1);
                        items = items.Skip(skip).Take(Helpers.PageSize).ToList();
                    }
                    res.ReturnCode = enResponseCode.Success;
                    res.ErrorCode = enErrorCode.Success;
                    res.ReturnMsg = EnResponseMessage.FindRecored;
                    res.MessagingQueueObj = items;
                    return res;
                }
                res.ReturnCode = enResponseCode.Fail;
                res.ErrorCode = enErrorCode.RecordNotFound;
                res.ReturnMsg = EnResponseMessage.NotFound;
                return res;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        #endregion

        #region "EmailQueue"

        //vsolanki 14-11-2018
        public ListEmailQueueRes GetEmailQueue(DateTime FromDate, DateTime ToDate, short? Status, string Email, int Page)
        {
            try
            {
                ListEmailQueueRes res = new ListEmailQueueRes();
                int Customday = Convert.ToInt32(_configuration["ReportValidDays"]);//2;
                double days = (ToDate - FromDate).TotalDays;
                if (Customday < days)
                {
                    var msg = EnResponseMessage.MoreDays;
                    msg = msg.Replace("#X#", Customday.ToString());

                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.MoreDays;
                    res.ReturnMsg = msg;
                    return res;
                }

                var items = _masterConfigurationRepository.GetEmailQueue(FromDate, ToDate, Status, Email, Page);
                if (items.Count != 0)
                {
                    if (Page > 0)
                    {
                        int skip = Helpers.PageSize * (Page - 1);
                        items = items.Skip(skip).Take(Helpers.PageSize).ToList();
                    }
                    res.ReturnCode = enResponseCode.Success;
                    res.ErrorCode = enErrorCode.Success;
                    res.ReturnMsg = EnResponseMessage.FindRecored;
                    res.EmailQueueObj = items;
                    return res;
                }
                res.ReturnCode = enResponseCode.Fail;
                res.ErrorCode = enErrorCode.RecordNotFound;
                res.ReturnMsg = EnResponseMessage.NotFound;
                return res;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        #endregion

        #region "WalletLedger"

        //vsolanki 14-11-2018
        public ListWalletLedgerResponse GetWalletLedger(DateTime FromDate, DateTime ToDate, long WalletId, int page, int? PageSize)
        {
            try
            {
                ListWalletLedgerResponse res = new ListWalletLedgerResponse();
                int Customday = Convert.ToInt32(_configuration["ReportValidDays"]);//2;
                double days = (ToDate - FromDate).TotalDays;
                if (Customday < days)
                {
                    var msg = EnResponseMessage.MoreDays;
                    msg = msg.Replace("#X#", Customday.ToString());

                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.MoreDays;
                    res.ReturnMsg = msg;
                    return res;
                }

                var items = _masterConfigurationRepository.GetWalletLedger(FromDate, ToDate, WalletId, page, PageSize);
                if (items.Count != 0)
                {
                    res.ReturnCode = enResponseCode.Success;
                    res.ErrorCode = enErrorCode.Success;
                    res.ReturnMsg = EnResponseMessage.FindRecored;
                    res.WalletLedgers = items;
                    return res;
                }
                res.ReturnCode = enResponseCode.Fail;
                res.ErrorCode = enErrorCode.RecordNotFound;
                res.ReturnMsg = EnResponseMessage.NotFound;
                return res;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        #endregion

        #region "LimitRuleMaster"

        //vsoalnki 13-11-2018
        public ListLimitRuleMasterRes GetAllLimitRule()
        {
            ListLimitRuleMasterRes res = new ListLimitRuleMasterRes();
            try
            {
                var items = _masterConfigurationRepository.GetAllLimitRule();
                if (items.Count == 0)
                {
                    res.ReturnCode = enResponseCode.Fail;
                    res.ErrorCode = enErrorCode.RecordNotFound;
                    res.ReturnMsg = EnResponseMessage.NotFound;
                    return res;
                }
                res.ReturnCode = enResponseCode.Success;
                res.ErrorCode = enErrorCode.Success;
                res.ReturnMsg = EnResponseMessage.FindRecored;
                res.LimitRuleObj = items;
                return res;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsoalnki 13-11-2018
        public BizResponseClass AddLimitRule(LimitRuleMasterReq Request, long userid)
        {
            try
            {
                LimitRuleMaster template = new LimitRuleMaster();
                if (Request != null)
                {
                    template.Status = Convert.ToInt16(ServiceStatus.Active);
                    template.UpdatedDate = UTC_To_IST();
                    template.CreatedDate = UTC_To_IST();
                    template.CreatedBy = userid;

                    template.MaxAmount = Request.MaxAmount;
                    template.MinAmount = Request.MinAmount;
                    template.Name = Request.Name;
                    template.TrnType = Request.TrnType;
                    template.WalletType = Request.WalletType;

                    _LimitRuleMaster.Add(template);

                    return new BizResponseClass { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.RecordAdded, ErrorCode = enErrorCode.Success };
                }
                return new BizResponseClass { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidInput, ErrorCode = enErrorCode.InvalidInput };
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsoalnki 13-11-2018
        public BizResponseClass UpdateLimitRule(long LimitRuleMasterId, LimitRuleMasterReq Request, long userid)
        {
            try
            {
                var template = _LimitRuleMaster.GetSingle(i => i.Id == LimitRuleMasterId);
                if (Request != null)
                {
                    template.Status = Convert.ToInt16(ServiceStatus.Active);
                    template.UpdatedDate = UTC_To_IST();
                    template.CreatedDate = template.CreatedDate; //UTC_To_IST();
                    template.CreatedBy = template.CreatedBy;// userid;
                    template.UpdatedBy = userid;

                    template.MaxAmount = Request.MaxAmount;
                    template.MinAmount = Request.MinAmount;
                    template.Name = Request.Name;
                    template.TrnType = Request.TrnType;
                    template.WalletType = Request.WalletType;

                    _LimitRuleMaster.Update(template);

                    return new BizResponseClass { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.RecordUpdated, ErrorCode = enErrorCode.Success };
                }
                return new BizResponseClass { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidInput, ErrorCode = enErrorCode.InvalidInput };
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsoalnki 13-11-2018
        public BizResponseClass DisableLimitRule(long LimitRuleMasterId)
        {
            try
            {
                var template = _LimitRuleMaster.GetById(LimitRuleMasterId);
                if (template != null)
                {
                    //disable status
                    template.DisableService();
                    //update in DB
                    _LimitRuleMaster.Update(template);

                    return new BizResponseClass { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.RecordDisable, ErrorCode = enErrorCode.Success };
                }
                return new BizResponseClass { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidInput, ErrorCode = enErrorCode.InvalidInput };
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsoalnki 13-11-2018
        public ListLimitRuleMasterRes GetLimitRuleById(long LimitRuleMasterId)
        {
            ListLimitRuleMasterRes res = new ListLimitRuleMasterRes();
            try
            {
                var template = _masterConfigurationRepository.GetLimitRuleById(LimitRuleMasterId);
                // var template = _TemplateMaster.GetById(TemplateMasterId);
                if (template != null)
                {
                    res.ReturnCode = enResponseCode.Success;
                    res.ErrorCode = enErrorCode.Success;
                    res.ReturnMsg = EnResponseMessage.FindRecored;
                    res.LimitRuleObj = template;
                    return res;
                }
                res.ReturnCode = enResponseCode.Fail;
                res.ErrorCode = enErrorCode.RecordNotFound;
                res.ReturnMsg = EnResponseMessage.NotFound;
                return res;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        #endregion
    }
}
