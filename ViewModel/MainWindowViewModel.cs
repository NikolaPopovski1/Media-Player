using Media_Player.Model;
using Media_Player.MVVM;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;


namespace Media_Player.ViewModel
{
    public class MainWindowViewModel : ViewModelBase //zakaj ni treba vlkjučiti še RelayCommand?
    {
        public static string VIDEO_DIR = "./Videos/";
        public static string THUMBNAIL_DIR = "./Thumbnails/";

        private bool isPlaying = false;
        private bool isRepeating = false;
        private bool isShuffling = false;
        private bool isSliderDragging = false;

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
        private AddVideoWindow addVideoWindow = null;
        private EditVideoWindow editVideoWindow = null;

        private TextBox _nameTextBox;
        private TextBox _filePathTextBox;
        private TextBox _fileTypeTextBox;
        private DatePicker _lastModifiedTextBox;
        private TextBox _thumbnailPathTextBox;
        private Label _videoLabel;
        private string _selectedImg = "./." + THUMBNAIL_DIR + "default.png";

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
        public TextBox FileTypeTextBox
        {
            get { return _fileTypeTextBox; }
            set
            {
                _fileTypeTextBox = value;
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
        public TextBox ThumbnailPathTextBox
        {
            get { return _thumbnailPathTextBox; }
            set
            {
                _thumbnailPathTextBox = value;
                OnPropertyChanged();
            }
        }
        public Label VideoLabel
        {
            get { return _videoLabel; }
            set
            {
                _videoLabel = value;
                OnPropertyChanged();
            }
        }
        public string SelectedImg
        {
            get { return _selectedImg; }
            set
            {
                _selectedImg = value;
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

        public void SetNameTextBox(TextBox textBox)
        {
            _nameTextBox = textBox;
        }
        public void SetFilePathTextBox(TextBox textBox)
        {
            _filePathTextBox = textBox;
        }
        public void SetThumbnailPathTextBox(TextBox textBox)
        {
            _thumbnailPathTextBox = textBox;
        }
        public void SetLastModifiedTextBox(DatePicker datePicker)
        {
            _lastModifiedTextBox = datePicker;
        }
        public void SetFileTypeTextBox(TextBox textBox)
        {
            _fileTypeTextBox = textBox;
        }
        public void SetVideoLabel(Label label)
        {
            _videoLabel = label;
        }



        // MAIN WINDOW
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

        public RelayCommand CloseButton => new RelayCommand(execute =>
        {
            System.Windows.Application.Current.Shutdown();
        });
        public RelayCommand MinimizeButton => new RelayCommand(execute =>
        {
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
        public RelayCommand AddVideoStaticButton => new RelayCommand(
            execute =>
            {
                string videoPath = VIDEO_DIR + "Deagle3k.mp4";
                VideoList.Add(new VideoFile
                {
                    Name = "Deagle3k",
                    Path = videoPath,
                    Thumbnail = File.Exists(Path.GetFullPath(THUMBNAIL_DIR + "Deagle3k.png")) ? Path.GetFullPath(THUMBNAIL_DIR + "Deagle3k.png") : THUMBNAIL_DIR + "default.png",
                    LastModified = File.GetLastWriteTime(videoPath).ToString(),
                    FileType = Path.GetExtension(videoPath),
                    Size = (int)new FileInfo(videoPath).Length
                });
            }
        );
        public RelayCommand EditVideoStaticButton => new RelayCommand(
            execute =>
            {
                SelectedVideo.Name = "SPREMEMBA!!!";
            },
            canExecute =>
                VideoList.Count > 0 && SelectedVideo != null
        );
        public RelayCommand RemoveVideoButton => new RelayCommand(
            execute =>
            {
                VideoList.Remove(SelectedVideo);
            },
            canExecute =>
                VideoList.Count > 0 && SelectedVideo != null
        );
        public RelayCommand AddVideoButton => new RelayCommand(
            execute =>
            {
                addVideoWindow = new AddVideoWindow(this);
                SelectedImg = "./." + THUMBNAIL_DIR + "default.png";
                if (addVideoWindow.ShowDialog() == true)
                {
                    VideoList.Add(addVideoWindow.VideoFile);
                }
                addVideoWindow.Close();
                addVideoWindow = null;
            },
            canExecute =>
                addVideoWindow == null && VideoList.Count < 100 && editVideoWindow == null
        );
        public RelayCommand EditVideoButton => new RelayCommand(
            execute =>
            {
                if (editVideoWindow == null)
                {
                    editVideoWindow = new EditVideoWindow(this);
                    editVideoWindow.Owner = Application.Current.MainWindow;
                    editVideoWindow.Show();

                    editVideoWindow.Closed += (s, e) =>
                    {
                        editVideoWindow = null;
                    };
                }
            },
            canExecute =>
                VideoList.Count > 0 && SelectedVideo != null && editVideoWindow == null && addVideoWindow == null
        );


        // ADD VIDEO WINDOW
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
                        )
                    {
                        if (
                            FileTypeTextBox.Text == ".avi"
                            || FileTypeTextBox.Text == ".mp4"
                            || FileTypeTextBox.Text == ".mkv"
                            || FileTypeTextBox.Text == ".flv"
                            || FileTypeTextBox.Text == ".mov"
                            )
                        {
                            return true;
                        }
                        VideoLabel.Content = "Accepts only .avi, .mp4, .mkv, .flv and .mov file types!";
                    }
                    else
                    {
                        VideoLabel.Content = "File type must be the same as file path type!";
                    }
                }
                else
                {
                    VideoLabel.Content = "File or thumbnail path is incorrect!";
                }
                return false;
            }
            else
            {
                VideoLabel.Content = "All fields are not filled or video path is incorrect!";
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
                    VideoLabel.Content = "";
                }
                else
                {
                    VideoLabel.Content = "Thumbnail path is incorrect!";
                }
            }
            else
            {
                SelectedImg = "./." + THUMBNAIL_DIR + "default.png";
            }
        }

        // EDIT VIDEO WINDOW
        public void OpenVideFileDialog()
        {
            string filter = "Video Files (*.mp4;*.avi;*.mkv;*.flv;*.mov)|*.mp4;*.avi;*.mkv;*.flv;*.mov";

            OpenFileDialog openVideoFileDialog = new OpenFileDialog
            {
                Filter = filter,
                Title = "Select a Video File"
            };

            if (openVideoFileDialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = openVideoFileDialog.FileName;
                string fileType = System.IO.Path.GetExtension(openVideoFileDialog.FileName);
                FileTypeTextBox.Text = fileType;
                LastModifiedTextBox.Text = System.IO.File.GetLastWriteTime(openVideoFileDialog.FileName).ToString();
                FilePathTextBox.Text = openVideoFileDialog.FileName;
            }
        }
        public void OpenThumbnailPathDialog()
        {
            OpenFileDialog openThumbnailFileDialog = new OpenFileDialog
            {
                Filter = "PNG Files (*.png)|*.png",
                Title = "Select a PNG File"
            };

            if (openThumbnailFileDialog.ShowDialog() == true)
            {
                ThumbnailPathTextBox.Text = openThumbnailFileDialog.FileName;
            }
        }
        public void ApplyChanges()
        {
            string tmpImg = ThumbnailPathTextBox.Text;
            string tmpVid = FilePathTextBox.Text;

            if (
                tmpImg != "" && System.IO.File.Exists(tmpImg)
                && tmpVid != "" && System.IO.File.Exists(tmpVid)
                && FilePathTextBox.Text.Length >= 4
                && FilePathTextBox.Text[^4..] == FileTypeTextBox.Text
                && (
                    FileTypeTextBox.Text == ".avi"
                    || FileTypeTextBox.Text == ".mp4"
                    || FileTypeTextBox.Text == ".mkv"
                    || FileTypeTextBox.Text == ".flv"
                    || FileTypeTextBox.Text == ".mov"
                    )
                )
            {
                SelectedVideo.Path = FilePathTextBox.Text;
                SelectedVideo.Thumbnail = ThumbnailPathTextBox.Text;
                SelectedVideo.FileType = FileTypeTextBox.Text;

                VideoLabel.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));
                VideoLabel.Content = "Successfully applied video and thumbnail path!";

                System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(2);
                timer.Tick += (s, e) =>
                {
                    VideoLabel.Content = "";
                    VideoLabel.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
                    timer.Stop();
                };
                timer.Start(); 
            }
            else
            {
                if (
                    tmpImg == "" && !System.IO.File.Exists(tmpImg)
                    && tmpVid == "" && !System.IO.File.Exists(tmpVid)
                    )
                {
                    VideoLabel.Content = "Video and thumbnail path is incorrect!";
                }
                else if (tmpImg == "" && !System.IO.File.Exists(tmpImg))
                {
                    VideoLabel.Content = "Thumbnail path is incorrect!";
                }
                else if (tmpVid == "" && !System.IO.File.Exists(tmpVid))
                {
                    VideoLabel.Content = "Video path is incorrect!";
                }
                else if (
                    FilePathTextBox.Text[^4..] == FileTypeTextBox.Text
                    && (
                        FileTypeTextBox.Text == ".avi"
                        || FileTypeTextBox.Text == ".mp4"
                        || FileTypeTextBox.Text == ".mkv"
                        || FileTypeTextBox.Text == ".flv"
                        || FileTypeTextBox.Text == ".mov"
                        )
                    )
                {
                    VideoLabel.Content = "File type incorrect!";
                }
                else
                {
                    VideoLabel.Content = "Something went wrong!";
                }
            }
        }
    }
}