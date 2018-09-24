using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IUserSessionService
    {
        string GetSessionValue(string Key, object obj);

        void SetSessionValue(string Key, object obj);

        void SetSessionToken(string value);
    }
}
