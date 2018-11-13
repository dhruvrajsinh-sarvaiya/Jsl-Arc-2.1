using System;
using System.Collections.Generic;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.ViewModels;
using System.Text;

namespace CleanArchitecture.Infrastructure.Interfaces
{
    public interface IMasterConfiguration
    {
        BizResponseClass AddCountry(string CountryName, string CountryCode,long UserID);
        BizResponseClass AddState(string StateName, string StateCode, long CountryID, long UserID);
        BizResponseClass AddCity(string CityName, long StateID, long UserID);
        BizResponseClass AddZipCode(long ZipCode, string AreaName, long CityID, long UserID);
    }
}
