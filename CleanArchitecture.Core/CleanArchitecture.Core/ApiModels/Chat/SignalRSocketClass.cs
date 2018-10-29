using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ApiModels.Chat
{
    public class ConnetedClientList
    {
        public string ConnectionId { get; set; }
    }

    public class ConnetedUserDetail
    {
        public string UserName { get; set; }
    }

    public class ConnetedClientToken
    {
        public string Token { get; set; }
    }

    public class BlockUserDetail
    {
        public bool IsBlock { get; set; } = false;
    }

    public class ChatHistory
    {
        public string Message { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
