using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BusinessObjects;
// Ensure you have the correct NuGet package installed for MessageBox.Avalonia.
// You need to install the "MessageBox.Avalonia" package in your project.
// To do this, use the NuGet Package Manager in Visual Studio or run the following command in the Package Manager Console:
// Install-Package MessageBox.Avalonia
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Services;
using UI.Views;

namespace UI;

public partial class LoginWindow : Window
{
    private readonly IUserService _userService = new UserService();
    private User user = new User();
    public LoginWindow()
    {
        InitializeComponent();
    }

    private async void btnSignIn_Click(object sender, RoutedEventArgs e)
    {
        string username = txtUsername.Text.Trim();
        if (_userService.ValidateUser(username) != null)
        {
            MainWindow mainWindow = new MainWindow(user);
            mainWindow.Show();
            this.Close();
        }
        else
        {
            await ShowErrorMessage("Invalid username. Please try again or click Sign Up.", "Login Failed");
        }
    }

    private async void btnSignUp_Click(object sender, RoutedEventArgs e)
    {
        List<User> userList = _userService.GetAllUsers();

        if (userList.Any(u => u.Username.Equals(txtUsername.Text.Trim(), StringComparison.Ordinal)))
        {
            await ShowErrorMessage("Username already exists. Please choose a different username.", "Sign Up Failed");
            return;
        }
        else
        {
            user.Username = txtUsername.Text.Trim();
            if (!_userService.CreateUser(user))
            {
                await ShowErrorMessage("Failed to create user. Please try again.", "Sign Up Failed");
            }
            else
            {
                await ShowErrorMessage("User created successfully! You can now sign in.", "Sign Up Successful");
            }
        }
    }

    private void btnCloseButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
    {
        // Implement forgot password functionality here
    }
    private async System.Threading.Tasks.Task ShowErrorMessage(string message, string title)
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard(title, message, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
        await box.ShowWindowDialogAsync(this);
    }
    private async System.Threading.Tasks.Task ShowInfoMessage(string message, string title)
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard(title, message, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info);
        await box.ShowWindowDialogAsync(this);
    }

    private async System.Threading.Tasks.Task<ButtonResult> ShowConfirmationMessage(string message, string title)
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard(title, message, ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Question);
        return await box.ShowWindowDialogAsync(this);
    }
}