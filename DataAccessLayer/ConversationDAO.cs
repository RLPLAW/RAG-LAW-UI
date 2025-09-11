using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataAccessLayer
{
    public class ConversationDAO
    {
        private RlpContext _dbContext = new RlpContext();
        public List<Conversation> GetAllConversations()
        {
            return _dbContext.Conversations.ToList();
        }
        public Conversation? GetConversationById(int id)
        {
            return _dbContext.Conversations.FirstOrDefault(c => c.ConversationId == id);
        }
        public bool CreateConversation(Conversation conversation)
        {
            try
            {
                _dbContext.Conversations.Add(conversation);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
