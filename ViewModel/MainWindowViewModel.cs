using Media_Player.Model;
using Media_Player.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;

namespace Media_Player.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public string videoDirectory = "./Videos/";
        public string thumbnailsDirectory = "./Thumbnails/";
        private ObservableCollection<VideoFile> videoList = new ObservableCollection<VideoFile>();
        public MainWindowViewModel()
        {
            LoadVideoList();
        
        }

        public ObservableCollection<VideoFile> VideoList
        {
            get { return videoList; }
            set
            {
                videoList = value;
                OnPropertyChanged();
            }
        }

        private void LoadVideoList()
        {
            if (!Directory.Exists(videoDirectory))
                Directory.CreateDirectory(videoDirectory);

            string[] videoFiles = Directory.GetFiles(videoDirectory, "*.*")
                .Where(f => f.EndsWith(".mp4") || f.EndsWith(".avi") || f.EndsWith(".mkv"))
                .ToArray();

            VideoList.Clear();

            foreach (var file in videoFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string thumbnailPath = Path.Combine("./Thumbnails/", fileName + ".png");

                VideoList.Add(new VideoFile
                {
                    Name = fileName,
                    Path = file,
                    Thumbnail = File.Exists(Path.GetFullPath(thumbnailPath)) ? Path.GetFullPath(thumbnailPath) : "default.jpg",
                    LastModified = File.GetLastWriteTime(file).ToString(),
                    FileType = Path.GetExtension(file),
                    Size = (int)new FileInfo(file).Length
                });
            }
        }
    }
}
