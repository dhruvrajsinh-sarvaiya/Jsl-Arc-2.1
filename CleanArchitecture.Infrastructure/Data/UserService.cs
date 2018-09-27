using CleanArchitecture.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Data
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
                String sOTP = generator.Next(000000, 999999).ToString("D6");
                return Convert.ToInt64(sOTP);

            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
    }
}
