using Avalonia.Controls;

namespace UI.Views;

public partial class MainView : UserControl
{
    public string Greeting => "Welcome to Avalonia!";
    public MainView()
    {
        InitializeComponent();
    }
}
