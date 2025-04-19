using System.IO;
using System.Windows;
using System.Windows.Controls;
using Media_Player.Model;
using Media_Player.MVVM;
using Media_Player.ViewModel;
using Microsoft.Win32;

namespace Media_Player
{
    public partial class AddVideoWindow : Window
    {
        private MainWindowViewModel vm;
        public VideoFile VideoFile { get; private set; }

        public AddVideoWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
            DataContext = this.vm;
            vm.SetVideoLabel(VideoLabel);
            vm.SetNameTextBox(NameTextBox);
            vm.SetFilePathTextBox(FilePathTextBox);
            vm.SetThumbnailPathTextBox(ThumbnailPathTextBox);
            vm.SetLastModifiedTextBox(LastModifiedTextBox);
            vm.SetFileTypeTextBox(FileTypeTextBox);
        }
        public void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (vm.Ok_Click())
            {
                if (ThumbnailPathTextBox.Text == "")
                {
                    ThumbnailPathTextBox.Text = MainWindowViewModel.THUMBNAIL_DIR + "default.png";
                }
                VideoFile = new VideoFile
                {
                    Name = NameTextBox.Text,
                    Path = FilePathTextBox.Text,
                    Thumbnail = ThumbnailPathTextBox.Text,
                    LastModified = LastModifiedTextBox.Text,
                    FileType = FileTypeTextBox.Text,
                    Size = (int)new FileInfo(FilePathTextBox.Text).Length
                };
                DialogResult = true;
            }
        }

        public void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void DodajFilmOkno_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void NameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
        }
        private void FilePathTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
        }
        private void ThumbnailPathTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            vm.ThumbnailPathChanged();
        }
        private void LastModifiedTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
        }
        private void FileTypeTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void FilePathButton_Click(object sender, RoutedEventArgs e)
        {
            vm.OpenVideFileDialog();
        }

        private void ThumbnailPathButton_Click(object sender, RoutedEventArgs e)
        {
            vm.OpenThumbnailPathDialog();
        }
    }
}
