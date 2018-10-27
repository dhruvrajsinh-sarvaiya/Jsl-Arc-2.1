﻿using CachingFramework.Redis;
using CachingFramework.Redis.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
        internal readonly RedisContext Context;        
        public RadisServices(RedisConnectionFactory connectionFactory)
        {
            this.ConnectionFactory = connectionFactory;
            this.Db = this.ConnectionFactory.Connection().GetDatabase();
            this.Context = new RedisContext(this.ConnectionFactory.Connection());
        }
        public void Delete(string key)
        {
           if (string.IsNullOrWhiteSpace(key) || key.Contains(":")) throw new ArgumentException("invalid key");

            key = this.GenerateKey(key);
            this.Db.KeyDelete(key);
        }

        public void DeleteHash(string key)
        {
            // if (string.IsNullOrWhiteSpace(key) || key.Contains(":")) throw new ArgumentException("invalid key");
            
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

        public void Scan(string value,string SpecialText)
        {
            try
            {
                IEnumerable<TagMember> members = this.Context.Cache.GetMembersByTag(value);
                foreach (TagMember member in members)
                {
                    if (string.IsNullOrWhiteSpace(member.Key) || member.Key.Contains(SpecialText))
                    {
                        var key = member.Key;
                        var type = member.MemberType;
                        var user = member.GetMemberAs<RedisUserdata>();
                        DeleteHash(key);
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IReadOnlyList<T> GetSetList(string Tag)
        {
            try
            {
                //IEnumerable<TagMember> members = this.Context.Cache.GetMembersByTag(Tag);
                //foreach (TagMember member in members)
                //{
                //    if (string.IsNullOrWhiteSpace(member.Key) || member.Key.Contains(":"))
                //    {
                //        var key = member.Key;
                //        var type = member.MemberType;
                //        var user = member.GetMemberAs<string>();
                //        return user;
                //    }
                //}
                IReadOnlyList<T> Members = this.Context.Cache.GetObjectsByTag<T>(new[] { Tag }).ToList().AsReadOnly();
                return Members;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetPair(string Value,string keySplitString)
        {
            try
            {
                IEnumerable<TagMember> members = this.Context.Cache.GetMembersByTag(Value);
                foreach (TagMember member in members)
                {
                    var Key = member.Key;
                    if (string.IsNullOrWhiteSpace(Key) || Key.Contains(keySplitString))
                    {
                        Key = Key.Split(keySplitString)[1];
                    }
                    return Key;
                }
                return "";
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
                //RedisContext context = new RedisContext();
                var Messages = this.Db.SetMembers(key);
                string Data = "[" + string.Join(",", Messages) + "]"; // make json format
                return Data;
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
                return this.Context.Cache.GetObject<T>(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<T> GetConnectionID1(string Token)
        {
            try
            {
                return this.Context.Cache.GetObjectsByTag<T>(Token);
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
                    //RedisContext context = new RedisContext();
                    this.Context.Cache.AddToSet(key, obj, new[] { Tag});
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void SaveTagsToSetMember(string Key, string Value, string Tag)
        {
            try
            {
                if (Value != null)
                {
                    //var hash = this.GenerateHash(obj);
                    //RedisContext context = new RedisContext();
                    //this.Context.Cache.AddToSet(key, obj, new[] { Tag });                    
                    this.Context.Cache.AddToSet(Key, Value, new[] { Tag , Value });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveSetMember(string Key, string Value)
        {
            try
            {
                if (Value != null)
                {
                    //var hash = this.GenerateHash(obj);
                    //RedisContext context = new RedisContext();
                    //this.Context.Cache.AddToSet(key, obj, new[] { Tag });
                    this.Context.Cache.RemoveFromSet(Key, Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveToHash(string key, T obj, string Tag1, string Tag2)
        {
            try
            {
                if (obj != null)
                {
                    var hash = this.GenerateHash(obj);
                    //RedisContext context = new RedisContext();
                    this.Context.Cache.SetObject(key, obj, new[] { Tag1,Tag2 });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveToHash(string key, T obj, string Tag1)
        {
            try
            {
                if (obj != null)
                {
                    var hash = this.GenerateHash(obj);
                    //RedisContext context = new RedisContext();
                    this.Context.Cache.SetObject(key, obj, new[] { Tag1 });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
