using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ApiModels.Chat
{
    public class ConnetedClientList
    {
        public string ConnectionId { get; set; }
    }

    public class ChatHistory
    {
        public string Message { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
