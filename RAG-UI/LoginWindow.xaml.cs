using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BusinessObjects;
using Services;

namespace RAG_UI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IUserService _userService = new UserService();
        private User user = new User();
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            if (_userService.ValidateUser(username) != null)
            {
                //MainWindow mainWindow = new MainWindow(user);
                //mainWindow.Show();
                //this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username. Please try again or click Sign Up.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            List<User> userList = _userService.GetAllUsers();

            if (userList.Any(u => u.Username.Equals(txtUsername.Text.Trim(), StringComparison.Ordinal)))
            {
                MessageBox.Show("Username already exists. Please choose a different username.", "Sign Up Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } else
            {
                user.Username = txtUsername.Text.Trim();
                if (_userService.CreateUser(user))
                {
                    MessageBox.Show("User created successfully! You can now sign in.", "Sign Up Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to create user. Please try again.", "Sign Up Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
