using Media_Player.ViewModel;
using System.Windows;

namespace Media_Player;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainWindowViewModel vm = new MainWindowViewModel(VideoPlayer, TimerLabel);
        DataContext = vm;
        vm.SetVideoSlider(VideoSlider);
        vm.SetPropertiesTextBlock(PropertiesTextBlock);
        vm.SetPlayPauseIconText(PlayPauseIconText);
        vm.SetRepeatIconText(RepeatIconText);
        vm.SetShuffleIconText(ShuffleIconText);
        vm.SetShuffleButtonItself(ShuffleButton);
    }

    private void TitleBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        // Only allow dragging if the left mouse button is pressed
        if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
        {
            this.DragMove();
        }
    }
    private void VideoListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm && vm.SelectedVideo != null)
        {
            vm.PlaySelectedVideo();
            vm.SetProperties(vm.SelectedVideo);
        }
    }

    private void VideoSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.VideoSlider_ValueChanged(sender, e);
        }
    }

    private void VideoSlider_DragStarted(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.VideoSlider_DragStarted(sender, e);
        }
    }

    private void VideoSlider_DragCompleted(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.VideoSlider_DragCompleted(sender, e);
        }
    }
}