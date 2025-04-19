using System.Windows;
using System.Windows.Controls;
using Media_Player.Model;
using Media_Player.MVVM;
using Media_Player.ViewModel;

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
    }
}
