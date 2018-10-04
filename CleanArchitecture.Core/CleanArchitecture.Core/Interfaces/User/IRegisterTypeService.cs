using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;

namespace CleanArchitecture.Core.Interfaces.User
{
   public interface IRegisterTypeService
    {
        Task<bool> GetRegisterType(string Type);
        void AddRegisterType(RegisterType model);
        Task<int> GetRegisterId(enRegisterType registertype);
    }
}
