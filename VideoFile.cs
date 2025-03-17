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
    class VideoFile : INotifyPropertyChanged
    {
        private string name;
        private string path;
        private string thumbnail;
        private int duration;
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
        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
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
    }
}
