using CachingFramework.Redis;
using CachingFramework.Redis.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Services.RadisDatabase
{
   
   public class RadisServices<T>  :BaseService<T>, IRedisService<T>
    {
        internal readonly IDatabase Db;
        //protected readonly IRedisConnectionFactory ConnectionFactory;
        //public RadisServices(IRedisConnectionFactory connectionFactory)
        //{
        //    this.ConnectionFactory = connectionFactory;
        //    this.Db = this.ConnectionFactory.Connection().GetDatabase();
        //}
        protected readonly RedisConnectionFactory ConnectionFactory;
        public RadisServices(RedisConnectionFactory connectionFactory)
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

        // khushali 18-10-2018 For signalr scaleout with Redis

        public void scan(string value)
        {
            try
            {
                RedisContext context = new RedisContext();
                IEnumerable<TagMember> members = context.Cache.GetMembersByTag(value);
                foreach (TagMember member in members)
                {
                    var key = member.Key;
                    var type = member.MemberType;
                    var user = member.GetMemberAs<RedisUserdata>();
                    Delete(key);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetSetData(string key)
        {
            try
            {
                RedisContext context = new RedisContext();
                var Messages = this.Db.SetMembers(key);
                string x = "[" + string.Join(",", Messages) + "]"; // make json format
                return x;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T GetConnectionID(string key)
        {
            try
            {
                RedisContext context = new RedisContext();
                return context.Cache.GetObject<T>(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveToSet(string key, T obj, string Tag)
        {
            try
            {
                if (obj != null)
                {
                    var hash = this.GenerateHash(obj);
                    RedisContext context = new RedisContext();
                    context.Cache.AddToSet(key, obj, new[] { Tag});
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void SaveToHash(string key, T obj, string Tag)
        {
            try
            {
                if (obj != null)
                {
                    var hash = this.GenerateHash(obj);
                    RedisContext context = new RedisContext();
                    context.Cache.SetObject(key, obj, new[] { Tag });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
