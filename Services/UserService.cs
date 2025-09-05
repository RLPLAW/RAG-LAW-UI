using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public User? ValidateUser(string username)
        {
            return _userRepository.ValidateUser(username);
        }
        public bool CreateUser(User user)
        {
            return _userRepository.CreateUser(user);
        }
    }
}
