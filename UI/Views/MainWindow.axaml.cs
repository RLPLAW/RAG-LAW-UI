using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using BusinessObjects;
using Microsoft.IdentityModel.Tokens;

namespace UI.Views
{
    public partial class MainWindow : Window
    {
        private User? _currentUser = new User();
        private char _userAvatarChar = 'U';
        private const string DefaultMessageInputText = "Type your message here...";
        private bool _isProcessingMessage = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeInterface();
        }

        public MainWindow(User? user = null)
        {
            InitializeComponent();
            _currentUser = user;
            InitializeInterface();
        }

        private void InitializeInterface()
        {
            if (_currentUser != null)
            {
                txtUsername.Text = _currentUser.Username ?? "User Account";

                if (!string.IsNullOrEmpty(_currentUser.Username) && _currentUser.Username.Length >= 1)
                {
                    _userAvatarChar = char.ToUpper(_currentUser.Username[0]);
                }
                else
                {
                    _userAvatarChar = 'U';
                    txtUsername.Text = "User Account";
                }

                txtbAvatar.Text = _userAvatarChar.ToString();
            }
            else
            {
                txtUsername.Text = "User Account";
                txtbAvatar.Text = _userAvatarChar.ToString();
            }
            SetPlaceholderText();
        }

        private void SetPlaceholderText()
        {
            if (string.IsNullOrEmpty(txtbMessageInput.Text) || txtbMessageInput.Text == DefaultMessageInputText)
            {
                txtbMessageInput.Text = DefaultMessageInputText;
                txtbMessageInput.Foreground = Brushes.Gray;
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement settings functionality
        }

        private void btnHelps_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement help functionality
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (_isProcessingMessage) return;

            string message = txtbMessageInput.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(message) || message == DefaultMessageInputText)
                return;

            _isProcessingMessage = true;
            btnSend.IsEnabled = false;

            try
            {
                AddUserMessage(message);

                txtbMessageInput.Text = string.Empty;

                var typingIndicator = ShowTypingIndicator();

                await Task.Delay(1500);

                RemoveTypingIndicator(typingIndicator);

                string botResponse = await RetrieveAnswerAsync(message);
                AddBotMessage(botResponse);
            }
            finally
            {
                _isProcessingMessage = false;
                btnSend.IsEnabled = true;
            }
        }

        private void AddUserMessage(string message)
        {
            var userMessagePanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 16, 0, 0)
            };

            var messageBubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0, 123, 255)), //Blue
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(12, 8),
                Margin = new Thickness(0, 0, 8, 0),
                MaxWidth = 300
            };

            var messageText = new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 14
            };

            messageBubble.Child = messageText;
            userMessagePanel.Children.Add(messageBubble);

            var userAvatar = CreateAvatar(_userAvatarChar.ToString(), Brushes.Blue);
            userMessagePanel.Children.Add(userAvatar);

            spMessages.Children.Add(userMessagePanel);
            ScrollToEnd();
        }

        private StackPanel ShowTypingIndicator()
        {
            var typingBubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(12, 8),
                Margin = new Thickness(8, 0, 0, 0)
            };

            var typingText = new TextBlock
            {
                Text = "AI is typing...",
                Foreground = Brushes.Gray,
                FontStyle = FontStyle.Italic,
                FontSize = 14
            };

            typingBubble.Child = typingText;

            var typingStack = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 16, 0, 0)
            };

            typingStack.Children.Add(CreateAvatar("AI", Brushes.Green));
            typingStack.Children.Add(typingBubble);

            spMessages.Children.Add(typingStack);
            ScrollToEnd();

            return typingStack;
        }

        private void RemoveTypingIndicator(StackPanel typingIndicator)
        {
            if (spMessages.Children.Contains(typingIndicator))
            {
                spMessages.Children.Remove(typingIndicator);
            }
        }

        private void AddBotMessage(string message)
        {
            var botMessagePanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 16, 0, 0)
            };

            botMessagePanel.Children.Add(CreateAvatar("AI", Brushes.Green));

            var messageBubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)), // Light gray background
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(12, 8),
                Margin = new Thickness(8, 0, 0, 0),
                MaxWidth = 300
            };

            var messageText = new TextBlock
            {
                Text = message,
                Foreground = Brushes.Black,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 14
            };

            messageBubble.Child = messageText;
            botMessagePanel.Children.Add(messageBubble);

            spMessages.Children.Add(botMessagePanel);
            ScrollToEnd();
        }

        private Border CreateAvatar(string text, IBrush backgroundColor)
        {
            var avatar = new Border
            {
                Background = backgroundColor,
                CornerRadius = new CornerRadius(20),
                Width = 40,
                Height = 40,
                Margin = new Thickness(4)
            };

            avatar.Child = new TextBlock
            {
                Text = text,
                Foreground = Brushes.White,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 14
            };

            return avatar;
        }

        private void ScrollToEnd()
        {
            // Use Dispatcher to ensure UI updates are complete before scrolling
            Dispatcher.UIThread.Post(() =>
            {
                svChat?.ScrollToEnd();
            }, Avalonia.Threading.DispatcherPriority.Render);
        }

        private async Task<string> RetrieveAnswerAsync(string userMessage)
        {
            // TODO: Implement actual RAG logic here
            // This is where you would:
            // 1. Process the user's message
            // 2. Retrieve relevant documents
            // 3. Generate a response using AI

            await Task.Delay(100); // Simulate processing time
            return $"This is a sample response to: '{userMessage}' from your RAG Assistant!";
        }

        private void txtbMessageInput_GotFocus(object sender, GotFocusEventArgs e)
        {
            if (txtbMessageInput.Text == DefaultMessageInputText)
            {
                txtbMessageInput.Text = string.Empty;
                txtbMessageInput.Foreground = Brushes.Gray;
            }
        }

        private void txtbMessageInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbMessageInput.Text))
            {
                SetPlaceholderText();
            }
        }

        private void txtbMessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.KeyModifiers.HasFlag(KeyModifiers.Shift))
            {
                e.Handled = true;
                if (!_isProcessingMessage)
                {
                    btnSend_Click(sender, e);
                }
            }
        }

        // Optional: Add method to clear chat
        public void ClearChat()
        {
            spMessages.Children.Clear();
        }

        // Optional: Add method to set user
        public void SetUser(User user)
        {
            _currentUser = user;
            InitializeInterface();
        }
    }
}