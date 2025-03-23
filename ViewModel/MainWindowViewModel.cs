using Media_Player.Model;
using Media_Player.MVVM;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Media_Player.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase //zakaj ni treba vlkjučiti še RelayCommand?
    {
        private bool isPlaying = false;
        private bool isRepeating = false;
        private bool isShuffling = false;
        private bool isSliderDragging = false;

        public string VIDEO_DIR = "./Videos/";
        public string THUMBNAIL_DIR = "./Thumbnails/";

        private ObservableCollection<VideoFile> videoList = new ObservableCollection<VideoFile>();
        DispatcherTimer timer = new DispatcherTimer();

        private VideoFile selectedVideo;
        private MediaElement VideoPlayer;
        private Slider VideoSlider;
        private Label TimerLabel;
        private TextBlock PropertiesTextBlock;
        private TextBlock PlayPauseIconText;
        private TextBlock RepeatIconText;
        private TextBlock ShuffleIconText;
        private Button ShuffleButtonItself;

        public MainWindowViewModel(MediaElement VideoPlayer, Label TimerLabel)
        {
            LoadVideoList();

            this.VideoPlayer = VideoPlayer;
            this.TimerLabel = TimerLabel;
            if (VideoPlayer != null)
            {
                VideoPlayer.MediaOpened += VideoPlayer_MediaOpened;
                VideoPlayer.MediaEnded += VideoPlayer_MediaEnded;

                timer.Interval = TimeSpan.FromMilliseconds(500);
                timer.Tick += (s, e) => UpdateSlider();
                timer.Tick += (s, e) => UpdateTimer();
                
            }
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
        public VideoFile SelectedVideo
        {
            get { return selectedVideo; }
            set
            {
                selectedVideo = value;
                OnPropertyChanged();
            }
        }



        public void SetMediaElement(MediaElement mediaElement)
        {
            VideoPlayer = mediaElement;
        }
        public void SetVideoSlider(Slider slider)
        {
            VideoSlider = slider;
        }
        public void SetTimerLabel(Label label)
        {
            TimerLabel = label;
        }
        public void SetPropertiesTextBlock(TextBlock textBlock)
        {
            PropertiesTextBlock = textBlock;
        }
        public void SetPlayPauseIconText(TextBlock textBlock)
        {
            PlayPauseIconText = textBlock;
        }
        public void SetRepeatIconText(TextBlock textBlock)
        {
            RepeatIconText = textBlock;
        }
        public void SetShuffleIconText(TextBlock textBlock)
        {
            ShuffleIconText = textBlock;
        }
        public void SetShuffleButtonItself(Button button)
        {
            ShuffleButtonItself = button;
        }



        private void LoadVideoList()
        {
            if (!Directory.Exists(VIDEO_DIR))
                Directory.CreateDirectory(VIDEO_DIR);

            string[] videoFiles = Directory.GetFiles(VIDEO_DIR, "*.*")
                .Where(f => f.EndsWith(".mp4") || f.EndsWith(".avi") || f.EndsWith(".mkv"))
                .ToArray();

            VideoList.Clear();

            foreach (var file in videoFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string thumbnailPath = Path.Combine(THUMBNAIL_DIR, fileName + ".png");

                VideoList.Add(new VideoFile
                {
                    Name = fileName,
                    Path = file,
                    Thumbnail = File.Exists(Path.GetFullPath(thumbnailPath)) ? Path.GetFullPath(thumbnailPath) : THUMBNAIL_DIR + "default.png",
                    LastModified = File.GetLastWriteTime(file).ToString(),
                    FileType = Path.GetExtension(file),
                    Size = (int)new FileInfo(file).Length
                });
            }
        }
        private void UpdateTimer()
        {
            TimerLabel.Content = VideoPlayer.Position.ToString(@"mm\:ss");
        }
        private void UpdateSlider()
        {
            if (!isSliderDragging && VideoPlayer.NaturalDuration.HasTimeSpan)
            {
                VideoSlider.Value = VideoPlayer.Position.TotalSeconds;
            }
        }
        public void SetProperties(VideoFile videoFile)
        {
            PropertiesTextBlock.Text = $"Name: {videoFile.Name}\n" +
                $"\nPath: {videoFile.Path}\n" +
                $"\nThumbnail: {videoFile.Thumbnail}\n" +
                $"\nLast Modified: {videoFile.LastModified}\n" +
                $"\nFile Type: {videoFile.FileType}\n" +
                $"\nSize: {videoFile.Size / 1024 / 1024} MB";
        }
        public void PlaySelectedVideo()
        {
            if (SelectedVideo != null && VideoPlayer != null)
            {
                string absolutePath = System.IO.Path.GetFullPath(SelectedVideo.Path);
                VideoPlayer.Source = new Uri(absolutePath);
                isPlaying = true;
                PlayPauseButton.Execute(null);
            }
        }
        public void VideoSlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (isSliderDragging)
            {
                VideoPlayer.Position = TimeSpan.FromSeconds(VideoSlider.Value);
            }
        }
        public void VideoSlider_DragStarted(object sender, RoutedEventArgs e)
        {
            isSliderDragging = true;
        }
        public void VideoSlider_DragCompleted(object sender, RoutedEventArgs e)
        {
            isSliderDragging = false;
            VideoPlayer.Position = TimeSpan.FromSeconds(VideoSlider.Value);
            UpdateTimer();
        }
        public void VideoPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (VideoPlayer.NaturalDuration.HasTimeSpan)
            {
                VideoSlider.Maximum = VideoPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            }
        }
        public void VideoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (isRepeating)
            {
                VideoPlayer.Position = TimeSpan.FromSeconds(0);
                VideoPlayer.Play();
            }
            else if (isShuffling)
            {
                int randomIndex = new System.Random().Next(0, VideoList.Count);
                SelectedVideo = VideoList[randomIndex];
                PlaySelectedVideo();
            }
            else
            {
                PlayPauseButton.Execute(null);
            }
        }



        public RelayCommand CloseButton => new RelayCommand(execute => {
            System.Windows.Application.Current.Shutdown(); 
        });
        public RelayCommand MinimizeButton => new RelayCommand(execute => { 
            System.Windows.Application.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
        });
        public RelayCommand ResizeButton => new RelayCommand(
            execute => 
            { 
                if (System.Windows.Application.Current.MainWindow.WindowState == System.Windows.WindowState.Normal)
                    System.Windows.Application.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized;
                else
                    System.Windows.Application.Current.MainWindow.WindowState = System.Windows.WindowState.Normal;
            }
        );
        public RelayCommand PlayPauseButton => new RelayCommand(
            execute =>
            {
                if (!isPlaying && VideoPlayer.NaturalDuration.HasTimeSpan && VideoPlayer.Position.TotalSeconds == VideoPlayer.NaturalDuration.TimeSpan.TotalSeconds)
                {
                    VideoPlayer.Position = TimeSpan.FromSeconds(0);
                }
                PlayPauseIconText.Text = isPlaying ? "\uE769" : "\uE768";
                isPlaying = !isPlaying;
                if (isPlaying)
                {
                    VideoPlayer.Pause();
                    timer.Stop();
                }
                else
                {
                    VideoPlayer.Play();
                    timer.Start();
                }
            }, 
            canExecute => 
                VideoPlayer.Source != null
        );
        public RelayCommand RepeatButton => new RelayCommand(
            execute =>
            {
                if (isRepeating)
                    RepeatIconText.Text = "\uE8EE";
                else
                    RepeatIconText.Text = "\uF5E7";
                isRepeating = !isRepeating;
            }, 
            canExecute => 
                VideoPlayer.Source != null && !isShuffling
        );
        public RelayCommand ShuffleButton => new RelayCommand(
            execute =>
            {
                if (isShuffling)
                    ShuffleButtonItself.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(53, 53, 53));
                else
                    ShuffleButtonItself.Background = System.Windows.Media.Brushes.Green;
                isShuffling = !isShuffling;
            }, 
            canExecute => 
                VideoPlayer.Source != null && !isRepeating
        );
        public RelayCommand ForwardButton => new RelayCommand(
            execute =>
            {
                int currentIndex = VideoList.IndexOf(SelectedVideo);
                if (currentIndex < VideoList.Count - 1)
                {
                    SelectedVideo = VideoList[currentIndex + 1];
                    PlaySelectedVideo();
                }
            },
            canExecute =>
                VideoPlayer.Source != null
        );
        public RelayCommand PreviousButton => new RelayCommand(
            execute =>
            {
                int currentIndex = VideoList.IndexOf(SelectedVideo);
                if (currentIndex > 0)
                {
                    SelectedVideo = VideoList[currentIndex - 1];
                    PlaySelectedVideo();
                }
            },
            canExecute =>
                VideoPlayer.Source != null
        );
    }
}
