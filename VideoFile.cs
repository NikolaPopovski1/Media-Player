using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Media_Player
{
    public class VideoFile : INotifyPropertyChanged
    {
        private string name;
        private string path;
        private string thumbnail;
        private string lastModified;
        private string fileType;
        private int size;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                OnPropertyChanged();
            }
        }

        public string Thumbnail
        {
            get { return thumbnail; }
            set
            {
                thumbnail = value;
                OnPropertyChanged();
            }
        }
        public string LastModified
        {
            get { return lastModified; }
            set
            {
                lastModified = value;
                OnPropertyChanged();
            }
        }
        public string FileType
        {
            get { return fileType; }
            set
            {
                fileType = value;
                OnPropertyChanged();
            }
        }
        public int Size
        {
            get { return size; }
            set
            {
                size = value;
                OnPropertyChanged();
            }
        }

        public string SizeAsString
        {
            get
            {
                if(size < 0124)
                {
                    return size + " B";
                }
                else if (size < 1024 * 1024)
                {
                    return (size / 1024).ToString() + " KB";
                }
                else if (size < 1024 * 1024 * 1024)
                {
                    return (size / 1024 / 1024).ToString() + " MB";
                }
                else
                {
                    return (size / 1024 / 1024 / 1024).ToString() + " GB";
                }
            }
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }
    }
}
