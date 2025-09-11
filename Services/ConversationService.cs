using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public class ConversationService : IConversationService
    {
        private readonly ConversationRepository _conversationService;

        public ConversationService()
        {
            _conversationService = new ConversationRepository();
        }
        public bool CreateConversation(Conversation conversation)
        {
            return _conversationService.CreateConversation(conversation);
        }

        public List<Conversation> GetAllConversations()
        {
            return _conversationService.GetAllConversations();
        }

        public Conversation? GetConversationById(int id)
        {
            return _conversationService.GetConversationById(id);
        }
    }
}
