using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services
{
    public interface IMessageService
    {
        public List<Message> GetAllMessages();
        public List<Message> GetMessagesByConversationId(int conversationId);
        public bool CreateMessage(Message message);
    }
}
