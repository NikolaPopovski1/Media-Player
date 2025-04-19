using System.Windows;
using System.Windows.Controls;
using Media_Player.Model;
using Media_Player.MVVM;
using Media_Player.ViewModel;
using Microsoft.Win32;

namespace Media_Player
{
    public partial class DodajFilmOkno : Window
    {
        VideoFile VideoFile { get; set; }
        DodajFilmOknoViewModel vm;

        public DodajFilmOkno()
        {
            InitializeComponent();
            vm = new DodajFilmOknoViewModel(AddVideoLabel);
            DataContext = vm;
            vm.SetAddVideoLabel(AddVideoLabel);
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
            string filter = "";
            if (FilePathTextBox.Text == "")
            {
                filter = "Video Files (*.mp4; *.avi; *.mkv; *.flv; *.mov)|*.mp4;*.avi;*.mkv;*.flv;*.mov";
            }
            else
            {
                filter = FilePathTextBox.Text;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = filter,
                Title = "Select a Video File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = openFileDialog.FileName;
                string fileType = System.IO.Path.GetExtension(openFileDialog.FileName);
                FileTypeTextBox.Text = fileType;
                LastModifiedTextBox.Text = System.IO.File.GetLastWriteTime(openFileDialog.FileName).ToString();
                FilePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void ThumbnailPathButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PNG Files (*.png)|*.png",
                Title = "Select a PNG File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ThumbnailPathTextBox.Text = openFileDialog.FileName;
            }
        }
    }
}
