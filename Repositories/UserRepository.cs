using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private UserDAO _user = new UserDAO();

        public List<User> GetAllUsers()
        {
            return _user.GetAllUsers();
        }

        public User? ValidateUser(string username)
        {
            return _user.ValidateUser(username);
        }
        public bool CreateUser(User user)
        {
            return _user.CreateUser(user);
        }
    }
}
