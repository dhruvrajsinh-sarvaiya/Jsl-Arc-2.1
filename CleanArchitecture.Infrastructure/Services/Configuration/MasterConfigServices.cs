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
using CleanArchitecture.Core.ViewModels.MasterConfiguration;

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
        public BizResponseClass AddCity(AddCityReq Request, long UserID)//string CityName, long StateID, short Status
        {
            BizResponseClass Resp = new BizResponseClass();
            try
            {
                CityMaster obj = new CityMaster();
                obj.CityName = Request.CityName;
                obj.StateID = Request.StateID;
                obj.Status = Request.Status;
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

        public BizResponseClass AddCountry(AddCountryReq Request, long UserID)//string CountryName, string CountryCode, long UserID, short Status
        {
            BizResponseClass Resp = new BizResponseClass();
            try
            {
                CountryMaster obj = new CountryMaster();
                obj.CountryName = Request.CountryName;
                obj.CountryCode = Request.CountryCode;
                obj.Status = Request.Status;
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

        public BizResponseClass AddState(AddStateReq Request, long UserID)//string StateName, string StateCode, long CountryID, short Status
        {
            BizResponseClass Resp = new BizResponseClass();
            try
            {
                StateMaster obj = new StateMaster();
                obj.StateName = Request.StateName;
                obj.StateCode = Request.StateCode;
                obj.Status = Request.Status;
                obj.CountryID = Request.CountryID;
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

        public BizResponseClass AddZipCode(AddZipCodeReq Request, long UserID)//long ZipCode, string AreaName, long CityID, short Status
        {
            BizResponseClass Resp = new BizResponseClass();
            try
            {
                ZipCodeMaster obj = new ZipCodeMaster();
                obj.ZipCode = Request.ZipCode;
                obj.ZipAreaName = Request.AreaName;
                obj.CityID = Request.CityID;
                obj.Status = Request.Status;
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

        public BizResponseClass UpdateCountry(AddCountryReq Request, long UserID)
        {
            try
            {
                BizResponseClass Resp = new BizResponseClass();
                var IsExist = _commonRepoCountry.GetSingle(item => item.Id == Request.CountryID && item.Status == Convert.ToInt16(ServiceStatus.Active));
                if (IsExist != null)
                {
                    IsExist.CountryCode = Request.CountryCode;
                    IsExist.CountryName = Request.CountryName;
                    IsExist.Status = Request.Status;
                    IsExist.UpdatedBy = UserID;
                    IsExist.UpdatedDate = UTC_To_IST();
                    _commonRepoCountry.Update(IsExist);
                    Resp.ErrorCode = enErrorCode.Success;
                    Resp.ReturnCode = enResponseCode.Success;
                    Resp.ReturnMsg = EnResponseMessage.RecordUpdated;
                }
                else
                {
                    Resp.ErrorCode = enErrorCode.NotFound;
                    Resp.ReturnCode = enResponseCode.Fail;
                    Resp.ReturnMsg = EnResponseMessage.NotFound;
                }
                return Resp;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw;
            }
        }

        public BizResponseClass UpdateState(AddStateReq Request, long UserID)
        {
            try
            {
                BizResponseClass Resp = new BizResponseClass();
                var IsExist = _commonRepoState.GetSingle(item => item.Id == Request.StateID && item.Status == Convert.ToInt16(ServiceStatus.Active));
                if (IsExist != null)
                {
                    IsExist.StateName = Request.StateName;
                    IsExist.StateCode = Request.StateCode;
                    IsExist.CountryID = Request.CountryID;
                    IsExist.Status = Request.Status;
                    IsExist.UpdatedBy = UserID;
                    IsExist.UpdatedDate = UTC_To_IST();
                    _commonRepoState.Update(IsExist);
                    Resp.ErrorCode = enErrorCode.Success;
                    Resp.ReturnCode = enResponseCode.Success;
                    Resp.ReturnMsg = EnResponseMessage.RecordUpdated;
                }
                else
                {
                    Resp.ErrorCode = enErrorCode.NotFound;
                    Resp.ReturnCode = enResponseCode.Fail;
                    Resp.ReturnMsg = EnResponseMessage.NotFound;
                }
                return Resp;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw;
            }
        }

        public BizResponseClass UpdateCity(AddCityReq Request, long UserID)
        {
            try
            {
                BizResponseClass Resp = new BizResponseClass();
                var IsExist = _commonRepoCity.GetSingle(item => item.Id == Request.CityID && item.Status == Convert.ToInt16(ServiceStatus.Active));
                if (IsExist != null)
                {
                    IsExist.CityName = Request.CityName;
                    IsExist.StateID = Request.StateID;
                    IsExist.Status = Request.Status;
                    IsExist.UpdatedBy = UserID;
                    IsExist.UpdatedDate = UTC_To_IST();
                    _commonRepoCity.Update(IsExist);
                    Resp.ErrorCode = enErrorCode.Success;
                    Resp.ReturnCode = enResponseCode.Success;
                    Resp.ReturnMsg = EnResponseMessage.RecordUpdated;
                }
                else
                {
                    Resp.ErrorCode = enErrorCode.NotFound;
                    Resp.ReturnCode = enResponseCode.Fail;
                    Resp.ReturnMsg = EnResponseMessage.NotFound;
                }
                return Resp;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw;
            }
        }

        public BizResponseClass UpdateZipCode(AddZipCodeReq Request, long UserID)
        {
            try
            {
                BizResponseClass Resp = new BizResponseClass();
                var IsExist = _commonRepoZipCode.GetSingle(item => item.Id == Request.ZipCodeID && item.Status == Convert.ToInt16(ServiceStatus.Active));
                if (IsExist != null)
                {
                    IsExist.ZipCode = Request.ZipCode;
                    IsExist.ZipAreaName = Request.AreaName;
                    IsExist.CityID = Request.CityID;
                    IsExist.Status = Request.Status;
                    IsExist.UpdatedBy = UserID;
                    IsExist.UpdatedDate = UTC_To_IST();
                    _commonRepoZipCode.Update(IsExist);
                    Resp.ErrorCode = enErrorCode.Success;
                    Resp.ReturnCode = enResponseCode.Success;
                    Resp.ReturnMsg = EnResponseMessage.RecordUpdated;
                }
                else
                {
                    Resp.ErrorCode = enErrorCode.NotFound;
                    Resp.ReturnCode = enResponseCode.Fail;
                    Resp.ReturnMsg = EnResponseMessage.NotFound;
                }
                return Resp;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw;
            }
        }

        #endregion

        #region GetByIDMethods

        public Countries GetCountry(long CountryID)
        {
            try
            {
                Countries country = new Countries();
                BizResponseClass Resp = new BizResponseClass();
                var IsExist = _commonRepoCountry.GetSingle(item => item.Id == CountryID);
                if (IsExist != null)
                {
                    country.CountryName = IsExist.CountryName;
                    country.CountryCode = IsExist.CountryCode;
                    country.Status = IsExist.Status;
                    Resp.ErrorCode = enErrorCode.Success;
                    Resp.ReturnCode = enResponseCode.Success;
                    Resp.ReturnMsg = EnResponseMessage.FindRecored;
                }
                else
                {
                    Resp.ErrorCode = enErrorCode.NotFound;
                    Resp.ReturnCode = enResponseCode.Fail;
                    Resp.ReturnMsg = EnResponseMessage.NotFound;
                }
                country.BizResponseObj = Resp;
                return country;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw;
            }
        }

        public States GetState(long StateID)
        {
            try
            {
                States states = new States();
                BizResponseClass Resp = new BizResponseClass();
                var IsExist = _commonRepoState.GetSingle(item => item.Id == StateID);
                if (IsExist != null)
                {
                    states.StateName = IsExist.StateName;
                    states.StateCode = IsExist.StateCode;
                    states.Status = IsExist.Status;
                    Resp.ErrorCode = enErrorCode.Success;
                    Resp.ReturnCode = enResponseCode.Success;
                    Resp.ReturnMsg = EnResponseMessage.FindRecored;
                }
                else
                {
                    Resp.ErrorCode = enErrorCode.NotFound;
                    Resp.ReturnCode = enResponseCode.Fail;
                    Resp.ReturnMsg = EnResponseMessage.NotFound;
                }
                states.BizResponseObj = Resp;
                return states;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw;
            }
        }

        public Cities GetCity(long CityID)
        {
            try
            {
                Cities cities = new Cities();
                BizResponseClass Resp = new BizResponseClass();
                var IsExist = _commonRepoCity.GetSingle(item => item.Id == CityID);
                if (IsExist != null)
                {
                    cities.CityName = IsExist.CityName;
                    cities.Status = IsExist.Status;
                    Resp.ErrorCode = enErrorCode.Success;
                    Resp.ReturnCode = enResponseCode.Success;
                    Resp.ReturnMsg = EnResponseMessage.FindRecored;
                }
                else
                {
                    Resp.ErrorCode = enErrorCode.NotFound;
                    Resp.ReturnCode = enResponseCode.Fail;
                    Resp.ReturnMsg = EnResponseMessage.NotFound;
                }
                cities.BizResponseObj = Resp;
                return cities;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw;
            }
        }

        #endregion

        #region GetListMethods



        #endregion
    }
}
