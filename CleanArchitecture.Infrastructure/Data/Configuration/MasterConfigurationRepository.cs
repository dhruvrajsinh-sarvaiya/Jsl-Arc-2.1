using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces.Configuration;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanArchitecture.Infrastructure.Data.Configuration
{
    public class MasterConfigurationRepository: IMasterConfigurationRepository
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
                                 SMSDate = u.CreatedDate.ToString(),
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
                                 EmailDate = u.CreatedDate.ToString(),
                                 Body = u.Body,
                                 CC = u.CC,
                                 BCC=u.BCC,
                                 Subject=u.Subject,
                                 Attachment=u.Attachment,
                                 EmailType=u.EmailType.ToString(),
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

        #endregion
    }
}
