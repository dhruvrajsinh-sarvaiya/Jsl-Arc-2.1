using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.Interfaces.User;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PhoneNumbers;
using System;
using System.Collections.Generic;
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
                string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
                string sOTP = String.Empty;
                long sTempChars;
                Random rand = new Random();
                int iOTPLength = 6;
                for (int i = 0; i < iOTPLength; i++)
                {
                    int p = rand.Next(0, saAllowedCharacters.Length);
                    sTempChars = Convert.ToInt64(saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)]);
                    sOTP += sTempChars;
                }
                return Convert.ToInt64(sOTP);
                //Random generator = new Random();
                //String sOTP = generator.Next(1, 999999).ToString("D6");
                //return Convert.ToInt64(sOTP);

            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        /// <summary>
        /// added by nirav savariya for random generate otp with password on 10/15/2018
        /// </summary>
        /// <returns></returns>

        public string GenerateRandomOTPWithPassword(PasswordOptions opts = null)
        {
            try
            {
                if (opts == null) opts = new PasswordOptions()
                {
                    RequiredLength = 64,
                    RequiredUniqueChars = 4,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireNonAlphanumeric = true,
                    RequireUppercase = true,

                };

                string[] randomChars = new[] {
        "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
        "abcdefghijkmnopqrstuvwxyz",    // lowercase
        "0123456789",                   // digits
        "!@$^*"                      // non-alphanumeric
    };
                Random rand = new Random(Environment.TickCount);
                List<char> chars = new List<char>();

                if (opts.RequireUppercase)
                    chars.Insert(rand.Next(0, chars.Count),
                        randomChars[0][rand.Next(0, randomChars[0].Length)]);

                if (opts.RequireLowercase)
                    chars.Insert(rand.Next(0, chars.Count),
                        randomChars[1][rand.Next(0, randomChars[1].Length)]);

                if (opts.RequireDigit)
                    chars.Insert(rand.Next(0, chars.Count),
                        randomChars[2][rand.Next(0, randomChars[2].Length)]);

                if (opts.RequireNonAlphanumeric)
                    chars.Insert(rand.Next(0, chars.Count),
                        randomChars[3][rand.Next(0, randomChars[3].Length)]);

                for (int i = chars.Count; i < opts.RequiredLength
                    || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
                {
                    string rcs = randomChars[rand.Next(0, randomChars.Length)];
                    chars.Insert(rand.Next(0, chars.Count),
                        rcs[rand.Next(0, rcs.Length)]);
                }

                return new string(chars.ToArray());

                //string lowers = "abcdefghijklmnopqrstuvwxyz";
                //string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                //string number = "0123456789";
                //string specialChars = "@$?()";
                //Random random = new Random();

                //string generated = "!";
                //for (int i = 1; i <= 18; i++)
                //    generated = generated.Insert(
                //        random.Next(generated.Length),
                //        lowers[random.Next(lowers.Length - 1)].ToString()
                //    );

                //for (int i = 1; i <= 15; i++)
                //    generated = generated.Insert(
                //        random.Next(generated.Length),
                //        uppers[random.Next(uppers.Length - 1)].ToString()
                //    );

                //for (int i = 1; i <= 27; i++)
                //    generated = generated.Insert(
                //        random.Next(generated.Length),
                //        number[random.Next(number.Length - 1)].ToString()
                //    );

                //for (int i = 1; i <= 4; i++)
                //    generated = generated.Insert(
                //        random.Next(generated.Length),
                //        specialChars[random.Next(specialChars.Length - 1)].ToString()
                //    );

                //return generated.Replace("!", string.Empty);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public SocialCustomPasswordViewMoel GenerateRamdomSocialPassword(string ProvideKey)
        {
            try
            {
                if (!string.IsNullOrEmpty(ProvideKey))
                {
                    string str64 = GenerateRandomOTPWithPassword();
                    string alpha = string.Empty; string numeric = string.Empty, password = string.Empty;
                    foreach (char str in str64)
                    {
                        if (char.IsDigit(str))
                        {
                            if (numeric.Length < 6)
                                numeric += str.ToString();
                            else
                                alpha += str.ToString();
                        }
                        else
                            alpha += str.ToString();
                    }
                    if (ProvideKey.Length > 12)
                    {
                        string _Pass1 = alpha.Substring(0, 20);
                        string _Pass11 = _Pass1 + ProvideKey.Substring(1, 2);
                        string _Pass2 = alpha.Substring(20, 10);
                        string _Pass22 = _Pass2 + ProvideKey.Substring(6, 3);
                        string _Pass3 = alpha.Substring(30, 28);
                        string _Pass33 = _Pass3 + ProvideKey.Substring(10, 1);
                        password = _Pass11 + _Pass22 + _Pass33;
                    }
                    else
                    {
                        string _Pass1 = alpha.Substring(0, 20);   // If the provider key length is less then  then provide key combination is skip 2 and then add 6 to password.
                        string _Pass11 = _Pass1 + ProvideKey.Substring(1, 6);
                        string _Pass2 = alpha.Substring(20, 10);
                        string _Pass3 = alpha.Substring(30, 28);
                        password = _Pass11 + _Pass2 + _Pass3;
                    }

                    return new SocialCustomPasswordViewMoel()
                    {
                        Password = password,
                        AppKey = alpha
                    };

                }

            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);

                throw ex;
            }
            return null;
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

    }
}
