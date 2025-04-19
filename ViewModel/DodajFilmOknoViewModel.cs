using Media_Player.Model;
using Media_Player.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Media_Player.ViewModel
{
    class DodajFilmOknoViewModel : ViewModelBase
    {
        private Label AddWindowLabel;
        private string selectedImg = MainWindowViewModel.THUMBNAIL_DIR + "default.png";

        private TextBox _nameTextBox;
        private TextBox _filePathTextBox;
        private TextBox _thumbnailPathTextBox;
        private DatePicker _lastModifiedTextBox;
        private Label _addVideoLabel;

        public TextBox NameTextBox
        {
            get { return _nameTextBox; }
            set
            {
                _nameTextBox = value;
                OnPropertyChanged();
            }
        }
        public TextBox FilePathTextBox
        {
            get { return _filePathTextBox; }
            set
            {
                _filePathTextBox = value;
                OnPropertyChanged();
            }
        }
        public TextBox ThumbnailPathTextBox
        {
            get { return _thumbnailPathTextBox; }
            set
            {
                _thumbnailPathTextBox = value;
                OnPropertyChanged();
            }
        }
        public DatePicker LastModifiedTextBox
        {
            get { return _lastModifiedTextBox; }
            set
            {
                _lastModifiedTextBox = value;
                OnPropertyChanged();
            }
        }
        public Label AddVideoLabel
        {
            get { return _addVideoLabel; }
            set
            {
                _addVideoLabel = value;
                OnPropertyChanged();
            }
        }

        public void SetNameTextBox(TextBox textBox)
        {
            NameTextBox = textBox;
        }
        public void SetFilePathTextBox(TextBox textBox)
        {
            FilePathTextBox = textBox;
        }
        public void SetThumbnailPathTextBox(TextBox textBox)
        {
            ThumbnailPathTextBox = textBox;
        }
        public void SetLastModifiedTextBox(DatePicker textBox)
        {
            LastModifiedTextBox = textBox;
        }
        public void SetAddVideoLabel(Label label)
        {
            AddVideoLabel = label;
        }

        public DodajFilmOknoViewModel(Label label)
        {
            this.AddWindowLabel = label;
        }

        public string SelectedImg
        {
            get { return selectedImg; }
            set
            {
                selectedImg = value;
                OnPropertyChanged();
            }
        }

        public void SetAddWindowLabel(Label label)
        {
            AddWindowLabel = label;
        }

        public bool Ok_Click()
        {
            if (
                !string.IsNullOrWhiteSpace(NameTextBox.Text) &&
               !string.IsNullOrWhiteSpace(FilePathTextBox.Text) &&
               !string.IsNullOrWhiteSpace(ThumbnailPathTextBox.Text) &&
               !string.IsNullOrWhiteSpace(LastModifiedTextBox.Text))
            {
                if (
                    System.IO.File.Exists(FilePathTextBox.Text) &&
                    System.IO.File.Exists(ThumbnailPathTextBox.Text))
                {
                    return true;
                }
                else
                {
                    AddVideoLabel.Content = "File or thumbnail path is incorrect!";
                    return false;
                }
            }
            else
            {
                AddVideoLabel.Content = "All fields must be filled!";
                return false;
            }
        }
    }
}
