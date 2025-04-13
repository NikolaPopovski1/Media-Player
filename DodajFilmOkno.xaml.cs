using System.Windows;
using Media_Player.MVVM;

namespace Media_Player
{
    public partial class DodajFilmOkno : Window
    {
        public DodajFilmOkno()
        {
            InitializeComponent();
        }

        private void DodajFilmOkno_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Only allow dragging if the left mouse button is pressed
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


    }
}
