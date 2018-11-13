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

namespace CleanArchitecture.Infrastructure.Services.Configuration
{
    public class CommunicationService : BasePage, ICommunicationService
    {
        #region "Variables"

        private readonly ICommonRepository<CommServiceTypeMaster> _CommServiceTypeMaster;
        private readonly ICommonRepository<TemplateMaster> _TemplateMaster;
        private readonly ILogger<CommunicationService> _log;

        #endregion

        #region "cotr"

        public CommunicationService(ILogger<CommunicationService> log, ILogger<BasePage> logger, ICommonRepository<CommServiceTypeMaster> CommServiceTypeMaster, ICommonRepository<TemplateMaster> TemplateMaster) : base(logger)
        {
            _CommServiceTypeMaster = CommServiceTypeMaster;
            _TemplateMaster = TemplateMaster;
            _log = log;
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
                if (items.Count==0)
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
                var items = _TemplateMaster.List();
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
                    template.CreatedDate = UTC_To_IST();
                    template.CreatedBy = userid;
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
        public TemplateMasterRes GetAllTemplateMaster(long TemplateMasterId)
        {
            TemplateMasterRes res = new TemplateMasterRes();
            try
            {
                var template = _TemplateMaster.GetById(TemplateMasterId);
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
    }
}
