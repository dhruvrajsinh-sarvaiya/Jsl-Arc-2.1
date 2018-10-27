using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.User;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services.User
{
    public class UserConfigurationService : IUserConfiguration
    {
        private readonly CleanArchitectureContext _dbContext;
        private readonly IUserService _userService;
        private readonly IMessageRepository<UserConfigurationMaster> _customRepository;
        private readonly IRegisterTypeService _registerTypeService;
        private readonly IMediator _mediator;
        public UserConfigurationService(CleanArchitectureContext dbContext, IUserService userService,

            IMessageRepository<UserConfigurationMaster> customRepository,
        IRegisterTypeService registerTypeService, IMediator mediator)
        {
            _dbContext = dbContext;
            _userService = userService;
            _customRepository = customRepository;
            _registerTypeService = registerTypeService;
            _mediator = mediator;
        }
        public void Add(int UserId, string Type, string ConfigurationValue, bool EnableStatus)
        {

        }

        public Task<UserConfigurationMasterViewModel> Get(int Id)
        {
            //var userConfigurationMaster = _dbContext.UserConfigurationMasters.Where(i => i.UserId == Id).LastOrDefault();
            //if (userConfigurationMaster != null)
            //{
            //    return UserConfigurationMasterViewModel = new UserConfigurationMasterViewModel()
            //    {
            //        UserId = userConfigurationMaster.UserId,


            //    };
            //}
            return null;
        }

        public void update(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
