using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Infrastructure.Interfaces;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Infrastructure.DTOClasses;
using System.Threading.Tasks;
using CleanArchitecture.Core.ViewModels.WalletOperations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CleanArchitecture.Core.ViewModels.Wallet;
using CleanArchitecture.Core.Entities.Wallet;
using System.Linq;
using CleanArchitecture.Core.ViewModels.WalletConfiguration;
using System.Collections;
using System.Globalization;
using CleanArchitecture.Core.Helpers;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Entities.Configuration;

namespace CleanArchitecture.Infrastructure.Services.Configuration
{
    public class MasterConfigServices : BasePage,IMasterConfiguration
    {

        private readonly ICommonRepository<CityMaster> _commonRepoCity;
        private readonly ICommonRepository<StateMaster> _commonRepoState;
        private readonly ICommonRepository<CountryMaster> _commonRepoCountry;
        private readonly ICommonRepository<ZipCodeMaster> _commonRepoZipCode;
        private readonly ILogger<MasterConfigServices> _log;

        public MasterConfigServices(ILogger<MasterConfigServices> log, ILogger<BasePage> logger, ICommonRepository<CityMaster> commonRepoCity,ICommonRepository<StateMaster> commonRepoState,ICommonRepository<CountryMaster> commonRepoCountry,ICommonRepository<ZipCodeMaster> commonRepoZipCode):base(logger)
        {
            _commonRepoCity = commonRepoCity;
            _commonRepoState = commonRepoState;
            _commonRepoCountry = commonRepoCountry;
            _commonRepoZipCode = commonRepoZipCode;
            _log = log;
        }

        #region AddMethods
        public BizResponseClass AddCity(string CityName, long StateID, long UserID)
        {
            BizResponseClass Resp = new BizResponseClass();
            try
            {
                CityMaster obj = new CityMaster();
                obj.CityName = CityName;
                obj.StateID = StateID;
                obj.Status = Convert.ToInt16(ServiceStatus.Active);
                obj.CreatedBy = UserID;
                obj.CreatedDate = UTC_To_IST();
                _commonRepoCity.Add(obj);
                Resp.ErrorCode = enErrorCode.Success;
                Resp.ReturnCode = enResponseCode.Success;
                Resp.ReturnMsg = EnResponseMessage.RecordAdded;
                return Resp;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public BizResponseClass AddCountry(string CountryName, string CountryCode, long UserID)
        {
            BizResponseClass Resp = new BizResponseClass();
            try
            {
                CountryMaster obj = new CountryMaster();
                obj.CountryName = CountryName;
                obj.CountryCode = CountryCode;
                obj.Status = Convert.ToInt16(ServiceStatus.Active);
                obj.CreatedBy = UserID;
                obj.CreatedDate = UTC_To_IST();
                _commonRepoCountry.Add(obj);
                Resp.ErrorCode = enErrorCode.Success;
                Resp.ReturnCode = enResponseCode.Success;
                Resp.ReturnMsg = EnResponseMessage.RecordAdded;
                return Resp;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public BizResponseClass AddState(string StateName, string StateCode, long CountryID, long UserID)
        {
            BizResponseClass Resp = new BizResponseClass();
            try
            {
                StateMaster obj = new StateMaster();
                obj.StateName = StateName;
                obj.StateCode = StateCode;
                obj.Status = Convert.ToInt16(ServiceStatus.Active);
                obj.CreatedBy = UserID;
                obj.CreatedDate = UTC_To_IST();
                _commonRepoState.Add(obj);
                Resp.ErrorCode = enErrorCode.Success;
                Resp.ReturnCode = enResponseCode.Success;
                Resp.ReturnMsg = EnResponseMessage.RecordAdded;
                return Resp;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public BizResponseClass AddZipCode(long ZipCode, string AreaName, long CityID, long UserID)
        {
            BizResponseClass Resp = new BizResponseClass();
            try
            {
                ZipCodeMaster obj = new ZipCodeMaster();
                obj.ZipCode = ZipCode;
                obj.ZipAreaName = AreaName;
                obj.CityID = CityID;
                obj.Status = Convert.ToInt16(ServiceStatus.Active);
                obj.CreatedBy = UserID;
                obj.CreatedDate = UTC_To_IST();
                _commonRepoZipCode.Add(obj);
                Resp.ErrorCode = enErrorCode.Success;
                Resp.ReturnCode = enResponseCode.Success;
                Resp.ReturnMsg = EnResponseMessage.RecordAdded;
                return Resp;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        #endregion

        #region UpdateMethods
        #endregion

        #region GetByIDMethods
        #endregion

        #region GetListMethods
        #endregion
    }
}
