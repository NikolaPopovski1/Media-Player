using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Media_Player
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool isPlaying = true;
        private bool isRepeating = false;
        private bool isShuffling = false;
        private string videoDirectory = "./Videos/";
        string[] parts;
        private MediaElement placeholder;
        private ObservableCollection<VideoFile> videoList = new ObservableCollection<VideoFile>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadVideoList();
        }

        public ObservableCollection<VideoFile> VideoList
        {
            get { return videoList; }
            set
            {
                videoList = value;
                OnPropertyChanged(nameof(VideoList));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private void FindAndPlayVideo(VideoFile videoFile)
        {
            foreach (var video in VideoList)
            {
                if (video.Name[0] == videoFile.Name[0])
                {
                    if(video.Name == videoFile.Name)
                    {
                        VideoPlayer.Source = new Uri(Path.GetFullPath(video.Path));
                        VideoPlayer.Play();
                        // set video on repeat if isRepeating is true
                        VideoPlayer.MediaEnded += (sender, e) =>
                        {
                            if (isRepeating)
                            {
                                VideoPlayer.Position = TimeSpan.Zero;
                                VideoPlayer.Play();
                            }
                            else if (isShuffling) // play a random video if isShuffling is true
                            {
                                Random random = new Random();
                                int randomIndex = random.Next(0, VideoList.Count);
                                while (randomIndex == VideoList.IndexOf(video))
                                {
                                    randomIndex = random.Next(0, VideoList.Count);
                                }
                                VideoPlayer.Source = new Uri(Path.GetFullPath(VideoList[randomIndex].Path));
                                VideoPlayer.Play();
                                VideoListView.SelectedItem = VideoList[randomIndex];
                            }
                        };
                        isPlaying = true;
                        PlayPauseIconText.Text = "\uE769";

                        SetProperties(video);
                        break;
                    }
                }
            }
        }

        private void SetProperties(VideoFile videoFile)
        {
            PropertiesTextBlock.Text = $"Name: {videoFile.Name}\n" +
                $"\nPath: {videoFile.Path}\n" +
                $"\nThumbnail: {videoFile.Thumbnail}\n" +
                $"\nLast Modified: {videoFile.LastModified}\n" +
                $"\nFile Type: {videoFile.FileType}\n" +
                $"\nSize: {videoFile.Size / 1024 / 1024} MB";
        }


        private void VideoListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VideoListView.SelectedItem != null && VideoListView.SelectedItem is VideoFile selectedVideoFile)
            {
                string selectedVideo = Path.Combine(videoDirectory, selectedVideoFile.Name);
                MessageBox.Show($"Selected Video: {selectedVideo}");
            }
        }

        private void VideoListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (VideoListView.SelectedItem is VideoFile selectedVideoFile)
            {
                FindAndPlayVideo(selectedVideoFile); // This will play the video when double-clicked
            }
        }




        // Minimize Button Click Event Handler
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Maximize Button Click Event Handler
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        // Close Button Click Event Handler
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // MouseDown event handler to enable dragging the window
        private void TitleBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Only allow dragging if the left mouse button is pressed
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Izhod_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MenuItem_Click_Meni(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_Seznam(object sender, RoutedEventArgs e)
        {

        }

        private void SaveMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            isPlaying = !isPlaying;
            PlayPauseIconText.Text = isPlaying ? "\uE769" : "\uE768"; // Pause = ⏸, Play = ▶
            //PlayPauseIconText.Text = isPlaying ? "\uF8AE" : "\uF5B0"; // Pause = ⏸, Play = ▶ bold
            if (isPlaying)
            {
                VideoPlayer.Play();
            }
            else
            {
                VideoPlayer.Pause();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (isShuffling)
            {
                MessageBox.Show("Can't repeat when shuffle is on.");
            }
            else { 
                if (isRepeating)
                {
                    isRepeating = false;
                    RepeatIconText.Text = "\uE8EE";
                }
                else
                {
                    isRepeating = true;
                    RepeatIconText.Text = "\uF5E7";
                }
            }
        }

        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRepeating)
            {
                MessageBox.Show("Can't shuffle when repeat is on.");
            }
            else
            {
                if (isShuffling)
                {
                    isShuffling = false;
                    ShuffleButton.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(53, 53, 53));
                }
                else
                {
                    isShuffling = true;
                    ShuffleButton.Background = System.Windows.Media.Brushes.Green;
                }
            }
        }

        private void VideoSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VideoPlayer.Position = TimeSpan.FromSeconds(VideoSlider.Value);
        }
    }
}
