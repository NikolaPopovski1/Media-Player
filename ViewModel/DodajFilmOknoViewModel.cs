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
        private TextBox _fileTypeTextBox;
        private DatePicker _lastModifiedTextBox;
        private TextBox _thumbnailPathTextBox;
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
        public TextBox FileTypeTextBox
        {
            get { return _fileTypeTextBox; }
            set
            {
                _fileTypeTextBox = value;
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
        public void SetFileTypeTextBox(TextBox textBox)
        {
            FileTypeTextBox = textBox;
        }
        public void SetAddWindowLabel(Label label)
        {
            AddWindowLabel = label;
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
        public bool Ok_Click()
        {
            if (
                !string.IsNullOrWhiteSpace(NameTextBox.Text) &&
               !string.IsNullOrWhiteSpace(FilePathTextBox.Text) &&
               !string.IsNullOrWhiteSpace(LastModifiedTextBox.Text))
            {
                if (
                    System.IO.File.Exists(FilePathTextBox.Text)
                    && (
                        System.IO.File.Exists(ThumbnailPathTextBox.Text)
                        || ThumbnailPathTextBox.Text == ""
                        )
                    )
                {
                    if (
                        FilePathTextBox.Text.Length >= 4
                        && FilePathTextBox.Text[^4..] == FileTypeTextBox.Text
                        && (
                            FilePathTextBox.Text[^4..] != ".avi"
                            || FilePathTextBox.Text[^4..] != ".mp4"
                            || FilePathTextBox.Text[^4..] != ".mkv"
                            || FilePathTextBox.Text[^4..] != ".flv"
                            || FilePathTextBox.Text[^4..] != ".mov"
                            )
                        )
                    {
                        return true;
                    }
                    else
                    {
                        AddVideoLabel.Content = "File type must be the same as file path type!";
                        return false;
                    }
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
        public void ThumbnailPathChanged()
        {
            string tmp = ThumbnailPathTextBox.Text;
            if (tmp != "")
            {
                if (System.IO.File.Exists(tmp))
                {
                    SelectedImg = tmp;
                    AddVideoLabel.Content = "";
                }
                else
                {
                    AddVideoLabel.Content = "Thumbnail path is incorrect!";
                }
            }
            else
            {
                SelectedImg = MainWindowViewModel.THUMBNAIL_DIR + "default.png";
            }
        }
    }
}
