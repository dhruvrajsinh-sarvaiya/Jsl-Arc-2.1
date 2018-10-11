using StackExchange.Redis;


namespace CleanArchitecture.Core.Services.RadisDatabase
{
   public interface IRedisConnectionFactory
    {
        ConnectionMultiplexer Connection();
    }
}
