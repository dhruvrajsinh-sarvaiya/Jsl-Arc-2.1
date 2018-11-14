using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.Configuration
{
    public interface IMasterConfigurationRepository
    {
        #region "Methods"

        //vsoalnki 14-11-2018
        List<TemplateResponse> GetAllTemplateMaster();

        //vsoalnki 14-11-2018
        TemplateResponse GetTemplateMasterById(long TemplateMasterId);

        //vsoalnki 14-11-2018
        List<MessagingQueueRes> GetMessagingQueue(DateTime FromDate, DateTime ToDate, short? Status, long? MobileNo, int Page);

        //vsoalnki 14-11-2018
        List<EmailQueueRes> GetEmailQueue(DateTime FromDate, DateTime ToDate, short? Status,string Email, int Page);

        #endregion
    }
}
