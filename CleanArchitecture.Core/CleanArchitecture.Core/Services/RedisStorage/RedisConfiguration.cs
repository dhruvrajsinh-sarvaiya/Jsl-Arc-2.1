using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Services.RadisDatabase
{
    public class RedisConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
    }
}
