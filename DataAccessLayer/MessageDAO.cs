using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataAccessLayer
{
    public class MessageDAO
    {
        private RlpContext _dbContext = new RlpContext();

        public List<Message> GetAllMessages()
        {
            return _dbContext.Messages.ToList();
        }

        public List<Message> GetMessagesByConversationId(int conversationId)
        {
            return _dbContext.Messages.Where(m => m.ConversationId == conversationId).ToList();
        }

        public bool CreateMessage(Message message)
        {
            try
            {
                _dbContext.Messages.Add(message);
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
