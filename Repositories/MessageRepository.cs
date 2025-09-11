using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private MessageDAO _messageDAO = new MessageDAO();
        public bool CreateMessage(Message message)
        {
            return _messageDAO.CreateMessage(message);
        }

        public List<Message> GetAllMessages()
        {
            return _messageDAO.GetAllMessages();
        }

        public List<Message> GetMessagesByConversationId(int conversationId)
        {
            return _messageDAO.GetMessagesByConversationId(conversationId);
        }
    }
}
