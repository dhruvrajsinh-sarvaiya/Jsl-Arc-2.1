using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces.Configuration;
using CleanArchitecture.Core.ViewModels.Configuration;
using CleanArchitecture.Core.ViewModels.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanArchitecture.Infrastructure.Data.Configuration
{
    public class MasterConfigurationRepository : IMasterConfigurationRepository
    {
        #region "DI"
        private readonly CleanArchitectureContext _dbContext;

        public MasterConfigurationRepository(CleanArchitectureContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region "Methods"

        //vsoalnki 14-11-2018
        public List<TemplateResponse> GetAllTemplateMaster()
        {
            try
            {
                List<int> AllowTrnType = Helpers.GetEnumValue<EnTemplateType>();

                var val = Enum.GetNames(typeof(EnTemplateType))
                  .Cast<string>()
                  .Select(x => x.ToString())
                  .ToArray();

                var items = (from tm in _dbContext.TemplateMaster
                             join cm in _dbContext.CommServiceTypeMaster
                             on tm.CommServiceTypeID equals cm.CommServiceTypeID
                             join q in AllowTrnType on tm.TemplateID equals q
                             select new TemplateResponse
                             {
                                 Status = tm.Status,
                                 TemplateID = tm.TemplateID,
                                 TemplateType = val[q - 1],
                                 CommServiceTypeID = tm.CommServiceTypeID,
                                 CommServiceType = cm.CommServiceTypeName,
                                 TemplateName = tm.TemplateName,
                                 Content = tm.Content,
                                 AdditionalInfo = tm.AdditionalInfo
                             }
                             ).ToList();
                //var items = _TemplateMaster.List();
                // var items = _TemplateMaster.FindBy(i => i.Status == Convert.ToInt16(ServiceStatus.Active));
                return items;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsoalnki 14-11-2018
        public TemplateResponse GetTemplateMasterById(long TemplateMasterId)
        {
            try
            {
                List<int> AllowTrnType = Helpers.GetEnumValue<EnTemplateType>();

                var val = Enum.GetNames(typeof(EnTemplateType))
                  .Cast<string>()
                  .Select(x => x.ToString())
                  .ToArray();

                var template = (from tm in _dbContext.TemplateMaster
                                join cm in _dbContext.CommServiceTypeMaster
                                on tm.CommServiceTypeID equals cm.CommServiceTypeID
                                join q in AllowTrnType on tm.TemplateID equals q
                                where tm.Id == TemplateMasterId
                                select new TemplateResponse
                                {
                                    Status = tm.Status,
                                    TemplateID = tm.TemplateID,
                                    TemplateType = val[q - 1],
                                    CommServiceTypeID = tm.CommServiceTypeID,
                                    CommServiceType = cm.CommServiceTypeName,
                                    TemplateName = tm.TemplateName,
                                    Content = tm.Content,
                                    AdditionalInfo = tm.AdditionalInfo
                                }
                             ).First();
                return template;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsoalnki 14-11-2018
        public List<MessagingQueueRes> GetMessagingQueue(DateTime FromDate, DateTime ToDate, short? Status, long? MobileNo, int Page)
        {
            try
            {
                //MessageStatusType
                //var val = Enum.GetNames(typeof(MessageStatusType))
                //    .Cast<string>()
                //    .Select(x => x.ToString())
                //    .ToArray();
                //List<int> msgInt = Helpers.GetEnumValue<MessageStatusType>();

                var items = (from u in _dbContext.MessagingQueue
                                 //join q in msgInt
                                 //on u.Status equals q
                             where u.CreatedDate >= FromDate && u.CreatedDate <= ToDate && (Status == null || (u.Status == Status && Status != null)) && (MobileNo == null || (u.MobileNo == MobileNo && MobileNo != null))
                             select new MessagingQueueRes
                             {
                                 Status = u.Status,
                                 MobileNo = u.MobileNo,
                                 SMSDate = u.CreatedDate.ToString("dd-MM-yyyy h:mm:ss tt"),
                                 SMSText = u.SMSText,
                                 StrStatus = (u.Status == 0) ? "Initialize" : (u.Status == 1) ? "Success" : (u.Status == 6) ? "Pending" : "Fail"
                             }
                             ).ToList();

                return items;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsoalnki 14-11-2018
        public List<EmailQueueRes> GetEmailQueue(DateTime FromDate, DateTime ToDate, short? Status, string Email, int Page)
        {
            try
            {
                //MessageStatusType
                //var val = Enum.GetNames(typeof(MessageStatusType))
                //    .Cast<string>()
                //    .Select(x => x.ToString())
                //    .ToArray();
                //List<int> msgInt = Helpers.GetEnumValue<MessageStatusType>();

                var items = (from u in _dbContext.EmailQueue
                                 //join q in msgInt
                                 //on u.Status equals q
                             where u.CreatedDate >= FromDate && u.CreatedDate <= ToDate && (Status == null || (u.Status == Status && Status != null)) && (Email == null || (u.Recepient == Email && Email != null))
                             select new EmailQueueRes
                             {
                                 Status = u.Status,
                                 RecepientEmail = u.Recepient,
                                 EmailDate = u.CreatedDate.ToString("dd-MM-yyyy h:mm:ss tt"),
                                 Body = u.Body,
                                 CC = u.CC,
                                 BCC = u.BCC,
                                 Subject = u.Subject,
                                 Attachment = u.Attachment,
                                 EmailType = u.EmailType.ToString(),
                                 StrStatus = (u.Status == 0) ? "Initialize" : (u.Status == 1) ? "Success" : (u.Status == 6) ? "Pending" : "Fail"
                             }
                             ).ToList();

                return items;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsolanki 14-11-2018
        public List<WalletLedgerResponse> GetWalletLedger(DateTime FromDate, DateTime ToDate, long WalletId, int page, int? PageSize)
        {
            List<WalletLedgerResponse> wl = (from w in _dbContext.WalletLedgers
                                        where w.WalletId == WalletId && w.TrnDate >= FromDate && w.TrnDate <= ToDate
                                        orderby w.TrnDate ascending
                                        select new WalletLedgerResponse
                                        {
                                            LedgerId = w.Id,
                                            PreBal = w.PreBal,
                                            PostBal = w.PreBal,
                                            Remarks = "Opening Balance",
                                            Amount = 0,
                                            CrAmount = 0,
                                            DrAmount = 0,
                                            TrnDate = w.TrnDate.ToString("dd-MM-yyyy h:mm:ss tt")
                                        }).Take(1).Union((from w in _dbContext.WalletLedgers
                                                          where w.WalletId == WalletId && w.TrnDate >= FromDate && w.TrnDate <= ToDate
                                                          select new WalletLedgerResponse
                                                          {
                                                              LedgerId = w.Id,
                                                              PreBal = w.PreBal,
                                                              PostBal = w.PostBal,
                                                              Remarks = w.Remarks,
                                                              Amount = w.CrAmt > 0 ? w.CrAmt : w.DrAmt,
                                                              CrAmount = w.CrAmt,
                                                              DrAmount = w.DrAmt,
                                                              TrnDate = w.TrnDate.ToString("dd-MM-yyyy h:mm:ss tt")
                                                          })).ToList();

            if (page > 0)
            {
                if (PageSize == null)
                {
                    int skip = Helpers.PageSize * (page - 1);
                    wl = wl.Skip(skip).Take(Helpers.PageSize).ToList();
                }
                else
                {
                    int skip = Convert.ToInt32(PageSize) * (page - 1);
                    wl = wl.Skip(skip).Take(Helpers.PageSize).ToList();
                }
            }
            return wl;
        }

        //vsoalnki 14-11-2018
        public List<LimitRuleMasterRes> GetAllLimitRule()
        {
            try
            {
                List<int> AllowTrnType = Helpers.GetEnumValue<enTrnType>();

                var trntype = Enum.GetNames(typeof(enTrnType))
                .Cast<string>()
                .Select(x => x.ToString())
                .ToArray();

                var items = (from tm in _dbContext.LimitRuleMaster
                             join cm in _dbContext.WalletTypeMasters
                             on tm.WalletType equals cm.Id
                             select new LimitRuleMasterRes
                             {
                                 Status = tm.Status,
                                 Id = tm.Id,
                                 Name = tm.Name,
                                 TrnType = tm.TrnType,
                                 StrTrnType = trntype[(Convert.ToInt32(tm.TrnType) - 1)],
                                 MinAmount = tm.MinAmount,
                                 MaxAmount = tm.MaxAmount,
                                 WalletTypeName = cm.WalletTypeName,
                                 WalletType = tm.WalletType,
                                 StatusStr = (tm.Status == 9) ? "Disable" : (tm.Status == 1) ? "Active" : "InActive"
                             }
                             ).ToList();
                return items;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsoalnki 14-11-2018
        public List<LimitRuleMasterRes> GetLimitRuleById(long LimitRuleMasterId)
        {
            try
            {
                List<int> AllowTrnType = Helpers.GetEnumValue<enTrnType>();

                var trntype = Enum.GetNames(typeof(enTrnType))
                .Cast<string>()
                .Select(x => x.ToString())
                .ToArray();

                
                var template = (from tm in _dbContext.LimitRuleMaster
                                join cm in _dbContext.WalletTypeMasters
                                on tm.WalletType equals cm.Id
                                where tm.Id == LimitRuleMasterId
                                select new LimitRuleMasterRes
                                {
                                    Status = tm.Status,
                                    Id=tm.Id,
                                    Name=tm.Name,
                                    TrnType=tm.TrnType,
                                    StrTrnType= trntype[Convert.ToInt32(tm.TrnType)-1],
                                    MinAmount=tm.MinAmount,
                                    MaxAmount=tm.MaxAmount,
                                    WalletTypeName=cm.WalletTypeName,
                                    WalletType=tm.WalletType,
                                    StatusStr  = (tm.Status == 9) ? "Disable" : (tm.Status == 1) ? "Active" : "InActive"
                                }
                             ).ToList();
                return template;
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
