using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.Interfaces.User;
using CleanArchitecture.Core.ViewModels.AccountViewModels.Login;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services.User
{
    public partial class CustomPasswordService : ICustomPassword
    {
        private readonly CleanArchitectureContext _dbContext;
        private readonly IUserService _userService;
        private readonly ICustomRepository<CustomPassword> _customRepository;
        private readonly ILogger<CustomPassword> _logger;

        public CustomPasswordService(
            CleanArchitectureContext dbContext, IUserService userService,
            ICustomRepository<CustomPassword> customRepository,
            //IMessageRepository<Customtoken> customRepository,
            ILogger<CustomPassword> logger)
        {
            _dbContext = dbContext;
            _userService = userService;
            _customRepository = customRepository;
            _logger = logger;
        }

        public async Task<CustomtokenViewModel> AddPassword(CustomtokenViewModel model)
        {
            try
            {
                var token = new CustomPassword
                {
                    UserId = model.UserId,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = model.UserId,
                    EnableStatus = false,
                    Password = model.Password
                };
                _customRepository.Insert(token);

                CustomtokenViewModel datamodel = new CustomtokenViewModel();
                if (token != null)
                {
                    model.UserId = token.UserId;
                    model.EnableStatus = token.EnableStatus;
                    model.Password = token.Password;
                    return model;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw;
            }
        }

        public async Task<CustomtokenViewModel> GetPassword(long userid)
        {
            try
            {
                var data = _customRepository.Table.Where(i => i.UserId == userid && i.EnableStatus == false).LastOrDefault();
                CustomtokenViewModel model = new CustomtokenViewModel();
                if (data != null)
                {
                    model.Password = data.Password;
                    model.EnableStatus = data.EnableStatus;
                    model.UserId = data.UserId;
                    model.Id = data.Id;
                    return model;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        /// added by nirav savariya for set password for login with mobile and email
        /// </summary>
        public async Task<CustomtokenViewModel> IsValidPassword(string appkey, string otp)
        {
            try
            {
                if (!string.IsNullOrEmpty(appkey) && !string.IsNullOrEmpty(otp))
                {
                    string _Pass1 = appkey.Substring(0, 20);
                    string _Pass11 = _Pass1 + otp.Substring(0, 3);
                    string _Pass2 = appkey.Substring(20, 10);
                    string _Pass22 = _Pass2 + otp.Substring(3, 3);
                    string _Pass3 = appkey.Substring(30, 28);
                    string password = _Pass11 + _Pass22 + _Pass3;

                    var data = _customRepository.Table.Where(i => i.Password == password && i.EnableStatus == false).FirstOrDefault();
                    if (data != null)
                    {
                        CustomtokenViewModel model = new CustomtokenViewModel();
                        model.Password = data.Password;
                        model.EnableStatus = data.EnableStatus;
                        model.UserId = data.UserId;
                        model.Id = data.Id;
                        return model;
                    }

                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return null;
            }
        }

        public void UpdateOtp(long Id)
        {
            try
            {
                var tempdata = _customRepository.GetById(Id);
                tempdata.SetAsPasswordStatus();
                tempdata.SetAsUpdateDate(tempdata.Id);
                _customRepository.Update(tempdata);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw;
            }
        }
    }
}
