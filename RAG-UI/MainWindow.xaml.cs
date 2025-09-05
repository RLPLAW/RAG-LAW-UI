using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusinessObjects;
using Microsoft.IdentityModel.Tokens;

namespace RAG_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User? _currentUser = new User();
        private char userAvatarChars = 'U';
        private string defaultMessageInputText = "Type your message here...";
        public MainWindow(/*User? user*/)
        {
            InitializeComponent();
            initializeInterface();
            //_currentUser = user;
        }

        private void initializeInterface()
        {
            if(_currentUser != null)
            {
                txtUsername.Text = _currentUser.Username;
                //Set avatar character based on username
                if (!string.IsNullOrEmpty(_currentUser.Username) && _currentUser.Username.Length >= 1)
                {
                    userAvatarChars = char.ToUpper(_currentUser.Username[0]);
                    txtbAvatar.Text = userAvatarChars.ToString();
                }
                else
                {
                    userAvatarChars = 'U';
                    txtUsername.Text = "User Account";
                    txtbAvatar.Text = userAvatarChars.ToString();
                }

                if(string.IsNullOrEmpty(txtbMessageInput.Text))
                {
                    txtbMessageInput.Text = defaultMessageInputText;
                }
            } else
            {
                txtUsername.Text = "User Account";
            }
            
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnHelps_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string message = txtbMessageInput.Text.Trim();
            if (!message.IsNullOrEmpty() && message != defaultMessageInputText)
            {
                // ====== Bubble của User ======
                StackPanel userMessagePanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 16, 0, 0)
                };

                Border messageBubble = new Border
                {
                    Style = (Style)FindResource("UserMessageBubble")
                };
                messageBubble.Child = new TextBlock
                {
                    Text = message,
                    Style = (Style)FindResource("MessageTextStyle")
                };

                userMessagePanel.Children.Add(messageBubble);

                Border userAvatar = new Border
                {
                    Style = (Style)FindResource("UserAvatarStyle")
                };
                userAvatar.Child = new TextBlock
                {
                    Text = "U",
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 12
                };

                userMessagePanel.Children.Add(userAvatar);

                spMessages.Children.Add(userMessagePanel);

                txtbMessageInput.Clear();

                svChat.ScrollToEnd();

                // ====== Bubble "..." (Typing) ======
                var typingBubble = (Border)FindResource("TypingIndicatorBubble");
                typingBubble.Tag = "Typing";
                var typingStack = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(0, 0, 0, 16),
                    Children =
                    {
                        new Border
                        {
                            Style = (Style)FindResource("AvatarStyle"),
                            Child = new TextBlock
                            {
                                Text = "AI",
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.Bold,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                FontSize = 12
                            }
                        },
                        typingBubble
                    }
                };

                spMessages.Children.Add(typingStack);
                svChat.ScrollToEnd();

                // ====== Giả lập chờ bot phản hồi ======
                await Task.Delay(1500); // delay 1.5 giây

                // Xóa bubble "..."
                spMessages.Children.Remove(typingStack);

                // Lấy câu trả lời mẫu
                string botResponse = RetrieveAnswer();

                // ====== Bubble câu trả lời thật ======
                var answerBubble = new Border
                {
                    Style = (Style)FindResource("AssistantMessageBubble"),
                    Child = new TextBlock
                    {
                        Style = (Style)FindResource("MessageTextStyle"),
                        Text = botResponse
                    }
                };

                spMessages.Children.Add(
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(0, 0, 0, 16),
                        Children =
                        {
                    new Border
                    {
                        Style = (Style)FindResource("AvatarStyle"),
                        Child = new TextBlock
                        {
                            Text = "AI",
                            Foreground = Brushes.White,
                            FontWeight = FontWeights.Bold,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 12
                        }
                    },
                    answerBubble
                        }
                    });

                svChat.ScrollToEnd();
            }
        }

        private string RetrieveAnswer()
        {
            return "This is a sample response from your RAG Assistant!";
        }


        private void txtbMessageInput_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbMessageInput.Text = string.Empty;
        }

        private void txtbMessageInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbMessageInput.Text))
            {
                txtbMessageInput.Text = defaultMessageInputText;
            }
        }

        private void btnSend_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtbMessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
            {
                e.Handled = true;
                btnSend_Click(sender, e);
            }
        }
    }
}