using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataAccessLayer
{
    public class UserDAO
    {
        private RlpContext _dbContext = new RlpContext();
        public List<User> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }
        public User? ValidateUser(string username)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.Ordinal));
        }

        public bool CreateUser(User user)
        {
            try
            {
                _dbContext.Users.Add(user);
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
