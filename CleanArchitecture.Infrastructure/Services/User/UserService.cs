using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.Interfaces.User;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;
using Microsoft.Extensions.Logging;
using PhoneNumbers;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace CleanArchitecture.Infrastructure.Services.User
{
    public class UserService : IUserService
    {
        private readonly CleanArchitectureContext _dbContext;
        private readonly ILogger<UserService> _log;
        public UserService(CleanArchitectureContext dbContext, ILogger<UserService> log)
        {
            _dbContext = dbContext;
            _log = log;
        }

        public bool GetMobileNumber(string MobileNumber)
        {
            var userdata = _dbContext.Users.Where(i => i.Mobile == MobileNumber).FirstOrDefault();
            if (userdata?.Mobile == MobileNumber)
                return false;
            else
                return true;
        }
        /// <summary>
        /// Get User Data
        /// </summary>
        /// <param name="MobileNumber"></param>
        /// <returns></returns>
        public async Task<TempUserRegister> FindByMobileNumber(string MobileNumber)
        {
            var userdata = _dbContext.Users.Where(i => i.Mobile == MobileNumber).FirstOrDefault();
            if (userdata != null)
            {
                TempUserRegister model = new TempUserRegister();
                model.Mobile = userdata.Mobile;
                model.Id = userdata.Id;
                return model;
            }
            else
                return null;
        }

        /// <summary>
        /// Get User Data
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public async Task<TempUserRegister> FindByEmail(string Email)
        {
            var userdata = _dbContext.Users.Where(i => i.Email == Email).FirstOrDefault();
            if (userdata != null)
            {
                TempUserRegister model = new TempUserRegister();
                model.Email = userdata.Email;
                model.Id = userdata.Id;
                return model;
            }
            else
                return null;
        }

        /// <summary>
        /// added by nirav savariya for random generate otp on 9/26/2018
        /// </summary>
        /// <returns></returns>
        public long GenerateRandomOTP()
        {
            try
            {
                //string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
                //string sOTP = String.Empty;
                //long sTempChars ;
                //Random rand = new Random();
                //int iOTPLength = 6;
                //for (int i = 0; i < iOTPLength; i++)
                //{
                //    int p = rand.Next(0, saAllowedCharacters.Length);
                //    sTempChars = Convert.ToInt64(saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)]);
                //    sOTP += sTempChars;
                //}
                //return Convert.ToInt64(sOTP);


                Random generator = new Random();
                String sOTP = generator.Next(1, 999999).ToString("D6");
                return Convert.ToInt64(sOTP);

            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }


        public async Task<bool> IsValidPhoneNumber(string Mobilenumber, string CountryCode)
        {
            try
            {
                PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
                //string countryCode = "IN";
                //string Code = GetCountryByIP(IpAddress);
                PhoneNumbers.PhoneNumber phoneNumber = phoneUtil.Parse(Mobilenumber, CountryCode);

                return phoneUtil.IsValidNumber(phoneNumber); // returns true for valid number    
            }
            catch(Exception ex)
            {
                return false;
            }
        }


        public async Task<string> GetCountryByIP(string ipAddress)
        {
            try
            {
                var url = "http://ip-api.com/xml/" + ipAddress;
                var request = System.Net.WebRequest.Create(url);
                string strReturnVal;
                using (WebResponse wrs = request.GetResponse())
                using (Stream stream = wrs.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string response = reader.ReadToEnd();

                    XmlDocument ipInfoXML = new XmlDocument();
                    ipInfoXML.LoadXml(response);
                    XmlNodeList responseXML = ipInfoXML.GetElementsByTagName("query");
                    NameValueCollection dataXML = new NameValueCollection();

                    dataXML.Add(responseXML.Item(0).ChildNodes[2].InnerText, responseXML.Item(0).ChildNodes[2].Value);

                    //strReturnVal = responseXML.Item(0).ChildNodes[1].InnerText.ToString(); // Contry
                    //strReturnVal += "(" +responseXML.Item(0).ChildNodes[2].InnerText.ToString() + ")";  // Contry Code 
                    string Status = responseXML.Item(0).ChildNodes[0].InnerText.ToString();
                    if (!string.IsNullOrEmpty(Status) && Status == "fail")
                        return Status;

                    strReturnVal = responseXML.Item(0).ChildNodes[2].InnerText.ToString();  // Contry Code 
                    return strReturnVal;

                }
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }

        }
    }
}
