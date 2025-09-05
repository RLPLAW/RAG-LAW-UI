using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using BusinessObjects;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.ComponentModel;

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
                SetPlaceholderText();

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

            var messageBubble = new Border();
            if (this.TryFindResource("UserMessageBubble", out var userBubbleStyle) && userBubbleStyle is Style style)
            {
                messageBubble.Styles.Add(style);
            }

            var messageText = new TextBlock { Text = message };
            if (this.TryFindResource("MessageTextStyle", out var textStyle) && textStyle is Style textStyleCast)
            {
                messageText.Styles.Add(textStyleCast);
            }

            messageBubble.Child = messageText;
            userMessagePanel.Children.Add(messageBubble);

            var userAvatar = CreateAvatar(_userAvatarChar.ToString());
            userMessagePanel.Children.Add(userAvatar);

            spMessages.Children.Add(userMessagePanel);
            ScrollToEnd();
        }

        private StackPanel ShowTypingIndicator()
        {
            var typingBubble = new Border();
            if (this.TryFindResource("TypingIndicatorBubble", out var typingStyle) && typingStyle is Style style)
            {
                typingBubble.Styles.Add(style);
            }

            var typingStack = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 0, 0, 16)
            };

            typingStack.Children.Add(CreateAvatar("AI"));
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
                Margin = new Thickness(0, 0, 0, 16)
            };

            botMessagePanel.Children.Add(CreateAvatar("AI"));

            var messageBubble = new Border();
            if (this.TryFindResource("AssistantMessageBubble", out var botBubbleStyle) && botBubbleStyle is Style style)
            {
                messageBubble.Styles.Add(style);
            }

            var messageText = new TextBlock { Text = message };
            if (this.TryFindResource("MessageTextStyle", out var textStyle) && textStyle is Style textStyleCast)
            {
                messageText.Styles.Add(textStyleCast);
            }

            messageBubble.Child = messageText;
            botMessagePanel.Children.Add(messageBubble);

            spMessages.Children.Add(botMessagePanel);
            ScrollToEnd();
        }

        private Border CreateAvatar(string text)
        {
            var avatar = new Border();
            if (this.TryFindResource("AvatarStyle", out var avatarStyle) && avatarStyle is Style style)
            {
                avatar.Styles.Add(style);
            }

            avatar.Child = new TextBlock
            {
                Text = text,
                Foreground = Brushes.White,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 12
            };

            return avatar;
        }

        private void ScrollToEnd()
        {
            svChat?.ScrollToEnd();
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
                txtbMessageInput.Foreground = Brushes.Black; // Reset to normal color
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