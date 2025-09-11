using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public interface IConversationRepository
    {
        public List<Conversation> GetAllConversations();
        public Conversation? GetConversationById(int id);
        public bool CreateConversation(Conversation conversation);
    }
}
