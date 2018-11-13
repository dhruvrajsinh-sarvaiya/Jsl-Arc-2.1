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
    public class MasterConfigServices : IMasterConfiguration
    {
        private readonly ICommonRepository<CityMaster> _commonRepoCity;
        private readonly ICommonRepository<StateMaster> _commonRepoState;
        private readonly ICommonRepository<CountryMaster> _commonRepoCountry;
        private readonly ICommonRepository<ZipCodeMaster> _commonRepoZipCode;

        public MasterConfigServices(ICommonRepository<CityMaster> commonRepoCity,ICommonRepository<StateMaster> commonRepoState,ICommonRepository<CountryMaster> commonRepoCountry,ICommonRepository<ZipCodeMaster> commonRepoZipCode)
        {
            _commonRepoCity = commonRepoCity;
            _commonRepoState = commonRepoState;
            _commonRepoCountry = commonRepoCountry;
            _commonRepoZipCode = commonRepoZipCode;
        }


        public BizResponseClass AddCity(string CityName, long StateID, long UserID)
        {
            //try
            //{
            //    CityMaster obj = new CityMaster();
            //    obj.CityName = CityName;
            //    obj.StateID = StateID;
            //    obj.Status = Convert.ToInt16(ServiceStatus.Active);
            //    obj.CreatedBy = UserID;
            //    obj.CreatedDate = UTC_To_IST();
            //}
            //catch (Exception ex)
            //{
            //    HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
            //    throw ex;
            //}
            throw new NotImplementedException();
        }

        public BizResponseClass AddCountry(string CountryName, string CountryCode, long UserID)
        {
            throw new NotImplementedException();
        }

        public BizResponseClass AddState(string StateName, string StateCode, long CountryID, long UserID)
        {
            throw new NotImplementedException();
        }

        public BizResponseClass AddZipCode(long ZipCode, string AreaName, long CityID, long UserID)
        {
            throw new NotImplementedException();
        }
        //public BizResponseClass AddIntoConvertFund(ConvertTockenReq Request, long userid, string accessToken = null)
        //{
        //    try
        //    {
        //        ConvertFundHistory h = new ConvertFundHistory();
        //        h.CreatedBy = userid;
        //        h.CreatedDate = UTC_To_IST();
        //        h.UpdatedBy = userid;
        //        h.UpdatedDate = UTC_To_IST();
        //        h.Status = Convert.ToInt16(ServiceStatus.InActive);
        //        h.SourcePrice = Request.SourcePrice;
        //        h.DestinationPrice = Request.DestinationPrice;
        //        h.FromWalletId = Request.SourceWalletId;
        //        h.ToWalletId = Request.DestinationWalletId;
        //        h.Price = 10;
        //        h.TrnDate = UTC_To_IST();
        //        _ConvertFundHistory.Add(h);
        //        if (accessToken != null)
        //        {
        //            var msg = EnResponseMessage.ConvertFund;
        //            msg = msg.Replace("#SourcePrice#", h.SourcePrice.ToString());
        //            msg = msg.Replace("#DestinationPrice#", h.DestinationPrice.ToString());
        //            _signalRService.SendActivityNotification(msg, accessToken);
        //        }
        //        return new BizResponseClass { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.RecordAdded, ErrorCode = enErrorCode.Success };
        //    }
        //    catch (Exception ex)
        //    {
        //        HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
        //        throw ex;
        //    }
        //}
    }
}
