using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Services.RadisDatabase
{
  //public  class RedisConnectionFactory  : IRedisConnectionFactory
  public  class RedisConnectionFactory
    {
        private readonly IOptions<RedisConfiguration> redis;
        private readonly Lazy<ConnectionMultiplexer> _connection;


        public RedisConnectionFactory(IOptions<RedisConfiguration> redis)
        {
            this._connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redis.Value.Host));
        }

        public ConnectionMultiplexer Connection()
        {
            return this._connection.Value;
        }
    }
}
