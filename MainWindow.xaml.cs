using Media_Player.ViewModel;
using System.Windows;

namespace Media_Player;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        //MainWindowViewModel vm = new MainWindowViewModel();
        DataContext = new MainWindowViewModel();
    }
}