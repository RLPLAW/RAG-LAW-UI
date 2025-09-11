using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using DynamicData;
using Microsoft.IdentityModel.Tokens;
using Services;

namespace UI.Views
{
    public partial class MainWindow : Window
    {
        private User? _currentUser = new User();
        private char _userAvatarChar = 'U';
        private const string DefaultMessageInputText = "Type your message here...";
        private bool _isProcessingMessage = false;
        private List<Conversation> conversations = new List<Conversation>();
        private ConversationService conversationService = new ConversationService();
        private MessageService messageService = new MessageService();
        private List<Message> messages = new List<Message>();
        public MainWindow()
        {
            InitializeComponent();
            InitializeInterface();
            InitializeData();
        }

        public MainWindow(User? user = null)
        {
            InitializeComponent();
            _currentUser = user;
            InitializeInterface();
            InitializeData();
        }

        private void InitializeData()
        {
            //Dang dung stackpanel nen bi nguoc
            conversations = conversationService.GetAllConversations().OrderByDescending(comparer => comparer.UpdatedAt).ToList();
            // Clear existing items
            spConversations.Children.Clear();

            // Add header again
            var header = new TextBlock
            {
                Text = "Recent Conversations",
                Foreground = new SolidColorBrush(Color.Parse("#B0B0B0")),
                FontSize = 12,
                FontWeight = FontWeight.Medium,
                Margin = new Thickness(12, 8, 12, 12)
            };

            spConversations.Children.Add(header);

            if (conversations == null || !conversations.Any())
            {
                // Show placeholder when no data
                var emptyText = new TextBlock
                {
                    Text = "No conversations yet.",
                    Foreground = new SolidColorBrush(Color.Parse("#888888")),
                    FontSize = 12,
                    Margin = new Thickness(12, 8, 12, 12)
                };
                spConversations.Children.Add(emptyText);
                return;
            }

            foreach (var conv in conversations)
            {
                // Outer container
                var border = new Border
                {  
                    Background = new SolidColorBrush(Color.Parse("#2A2A2A")),
                    Margin = new Thickness(0, 0, 0, 8), 
                    Child = new StackPanel
                    {
                        Children =
                {
                    new TextBlock
                    {
                        Text = conv.Title ?? "(Untitled)",
                        Foreground = Brushes.White,
                        FontSize = 13,
                        TextTrimming = TextTrimming.CharacterEllipsis
                    },
                    new TextBlock
                    {
                        Text = FormatTimeAgo(conv.UpdatedAt), 
                        Foreground = new SolidColorBrush(Color.Parse("#888888")),
                        FontSize = 11,
                        Margin = new Thickness(0,2,0,0)
                    }
                }
                    }
                };

                border.PointerPressed += ConversationChild_onClick;
                border.Classes.Add("ConversationItemStyle");
                spConversations.Children.Add(border);
            }
        }

        private string FormatTimeAgo(DateTime updatedAt)
        {
            DateTime currentTime = DateTime.Now;
            var span = currentTime - updatedAt;

            if (span.TotalMinutes < 1) return "Just now";
            else if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes} minutes ago";
            else if (span.TotalHours < 24) return $"{(int)span.TotalHours} hours ago";
            else if (span.TotalDays < 7) return $"{(int)span.TotalDays} days ago";
            else if (span.TotalDays < 30) return $"{(int)(span.TotalDays / 7)} weeks ago";
            else if (span.TotalDays < 365) return $"{(int)(span.TotalDays / 30)} months ago";
            else return $"{(int)(span.TotalDays / 365)} years ago";
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
                txtTime.Text = DateTime.Now.ToString("hh:mm tt");
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

            Dispatcher.UIThread.Post(() =>
            {
                double targetY = Math.Max(0, svChat.Extent.Height - svChat.Viewport.Height + 50);
                svChat.Offset = new Vector(0, targetY);
            }, DispatcherPriority.Background);
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

        private void ConversationChild_onClick(object? sender, RoutedEventArgs e)
        {
            if (sender is Border border && border.Child is StackPanel stackPanel && stackPanel.Children[0] is TextBlock textBlock)
            {
                string conversationTitle = textBlock.Text;
                ClearChat();

                messages = messageService.GetMessagesByConversationId(conversations.FirstOrDefault(c => c.Title == conversationTitle)?.ConversationId ?? 0);
                
                for (int i = 0; i < messages.Count; i++)
                {
                    AddUserMessage(messages[i].UserMessage ?? "");
                    AddBotMessage(messages[i].ChatResponse ?? "");
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