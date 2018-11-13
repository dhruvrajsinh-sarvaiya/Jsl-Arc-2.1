using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Interfaces
{
    public interface ICommunicationService
    {
        #region "CommServiceTypeMaster"

        //vsoalnki 13-11-2018
        IEnumerable<CommServiceTypeMaster> GetAllCommServiceTypeMaster();

        #endregion

        #region "TemplateMaster"

        //vsoalnki 13-11-2018
        IEnumerable<TemplateMaster> GetAllTemplateMaster();
        BizResponseClass AddTemplateMaster(TemplateMasterReq Request, long userid);
        BizResponseClass UpdateTemplateMaster(long TemplateMasterId, TemplateMasterReq Request, long userid);
        BizResponseClass DisableTemplateMaster(long TemplateMasterId);
        TemplateMasterRes GetAllTemplateMaster(long TemplateMasterId);

        #endregion
    }
}
