using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private ConversationDAO _conversationDAO = new ConversationDAO();

        public bool CreateConversation(Conversation conversation)
        {
            return _conversationDAO.CreateConversation(conversation);
        }

        public List<Conversation> GetAllConversations()
        {
            return _conversationDAO.GetAllConversations();
        }

        public Conversation? GetConversationById(int id)
        {
            return _conversationDAO.GetConversationById(id);
        }
    }
}
