using Media_Player.ViewModel;
using Microsoft.Win32;
using System.Windows;

namespace Media_Player
{
    public partial class EditVideoWindow : Window
    {
        private MainWindowViewModel vm;
        public EditVideoWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
            DataContext = this.vm;
            vm.SetNameTextBox(NameTextBox);
            vm.SetFilePathTextBox(FilePathTextBox);
            vm.SetThumbnailPathTextBox(ThumbnailPathTextBox);
            vm.SetLastModifiedTextBox(LastModifiedTextBox);
            vm.SetFileTypeTextBox(FileTypeTextBox);
            vm.SetVideoLabel(VideoLabel);
        }
        
        private void UrediFilmOkno_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            vm.ApplyChanges();
        }

        private void ThumbnailPathButton_Click(object sender, RoutedEventArgs e)
        {
            vm.OpenThumbnailPathDialog();
        }

        private void FilePathButton_Click(object sender, RoutedEventArgs e)
        {
            vm.OpenVideFileDialog();
        }

        private void FilePathTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void FileTypeTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void NameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void ThumbnailPathTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            vm.ThumbnailPathChanged();
        }
    }
}
