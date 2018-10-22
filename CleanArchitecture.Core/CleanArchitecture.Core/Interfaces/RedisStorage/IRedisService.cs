using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Services.RadisDatabase
{
    public interface IRedisService<T>
    {
        T Get(string key);

        void Save(string key, T obj);

        void Delete(string key);

        // khushali 18-10-2018 For signalr scaleout with Redis

        string GetSetData(string key);

        T GetConnectionID(string key);

        void SaveToSet(string key, T obj, string Tag);

        void SaveToHash(string key, T obj, string Tag);
    }
}
