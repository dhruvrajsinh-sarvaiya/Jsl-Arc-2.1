using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Interfaces.User;

namespace CleanArchitecture.Infrastructure.Services.User
{
    public class RegisterTypeService : IRegisterTypeService
    {
        private readonly CleanArchitectureContext _dbContext;

        public RegisterTypeService(CleanArchitectureContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> GetRegisterType(string Type)
        {
            var RegTypedata = _dbContext.RegisterType.Where(i => i.Type == Type).FirstOrDefault();
            if (RegTypedata?.Type == Type)
                return true;
            else
                return false;
        }

        public void AddRegisterType(RegisterType model)
        {            
            _dbContext.Add(model);
            _dbContext.SaveChanges();            
        }
    }
}
