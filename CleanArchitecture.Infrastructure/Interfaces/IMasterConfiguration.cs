using System;
using System.Collections.Generic;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.ViewModels;
using System.Text;
using CleanArchitecture.Core.ViewModels.MasterConfiguration;

namespace CleanArchitecture.Infrastructure.Interfaces
{
    public interface IMasterConfiguration
    {
        BizResponseClass AddCountry(AddCountryReq Request, long UserID);//string CountryName, string CountryCode,long UserID,short Status
        BizResponseClass AddState(AddStateReq Request, long UserID);//string StateName, string StateCode, long CountryID
        BizResponseClass AddCity(AddCityReq Request, long UserID);//string CityName, long StateID
        BizResponseClass AddZipCode(AddZipCodeReq Request, long UserID);//long ZipCode, string AreaName, long CityID

        BizResponseClass UpdateCountry(AddCountryReq Request, long UserID);
        BizResponseClass UpdateState(AddStateReq Request, long UserID);
        BizResponseClass UpdateCity(AddCityReq Request, long UserID);
        BizResponseClass UpdateZipCode(AddZipCodeReq Request, long UserID);

        Countries GetCountry(long CountryID);
        States GetState(long StateID);
        Cities GetCity(long CityID);
    }
}
