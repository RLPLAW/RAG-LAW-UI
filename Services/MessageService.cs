using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public class MessageService : IMessageService
    {
        private readonly MessageRepository _messageRepository;
        public MessageService()
        {
            _messageRepository = new MessageRepository();
        }

        public bool CreateMessage(Message message)
        {
            return _messageRepository.CreateMessage(message);
        }

        public List<Message> GetAllMessages()
        {
            return _messageRepository.GetAllMessages();
        }

        public List<Message> GetMessagesByConversationId(int conversationId)
        {
            return _messageRepository.GetMessagesByConversationId(conversationId);
        }
    }
}
