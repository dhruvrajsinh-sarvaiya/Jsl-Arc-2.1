using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Services.RadisDatabase
{
   
   public class RadisServices<T>  :BaseService<T>, IRedisService<T>
    {
        internal readonly IDatabase Db;
        protected readonly IRedisConnectionFactory ConnectionFactory;
        public RadisServices(IRedisConnectionFactory connectionFactory)
        {
            this.ConnectionFactory = connectionFactory;
            this.Db = this.ConnectionFactory.Connection().GetDatabase();
        }
        public void Delete(string key)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Contains(":")) throw new ArgumentException("invalid key");

            key = this.GenerateKey(key);
            this.Db.KeyDelete(key);
        }

        public T Get(string key)
        {
            try
            {
                key = this.GenerateKey(key);
                var hash = this.Db.HashGetAll(key);
                return this.MapFromHash(hash);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public void Save(string key, T obj)
        {
            try
            {
                if (obj != null)
                {
                    var hash = this.GenerateHash(obj);
                    key = this.GenerateKey(key);

                    //if (this.Db.HashLength(key) == 0)
                    //{
                        this.Db.HashSet(key, hash);
                    //}
                    //else
                    //{
                    //    var props = this.Properties;
                    //    foreach (var item in props)
                    //    {
                    //        if (this.Db.HashExists(key, item.Name))
                    //        {
                    //            this.Db.HashIncrement(key, item.Name, Convert.ToInt64(item.GetValue(obj)));
                    //        }
                    //    }
                    //}

                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}
