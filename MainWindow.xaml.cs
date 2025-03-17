using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Media_Player
{
    public partial class MainWindow : Window
    {
        private bool isPlaying = true;
        private string videoDirectory = "./Videos/";
        string[] parts;
        List<VideoFile> videoList = new List<VideoFile>();
        private MediaElement placeholder;
        public MainWindow()
        {
            InitializeComponent();
            LoadVideoList();
        }



        private void LoadVideoList()
        {
            if (!Directory.Exists(videoDirectory))
                Directory.CreateDirectory(videoDirectory);

            string[] videoFiles = Directory.GetFiles(videoDirectory, "*.*")
                .Where(f => f.EndsWith(".mp4") || f.EndsWith(".avi") || f.EndsWith(".mkv"))
                .ToArray();

            VideoListView.Items.Clear(); 
            
            foreach (var file in videoFiles)
            {
                VideoFile video = new VideoFile();
                parts = file.Replace("./Videos/", "").Split(new[] { '.' }, 2);
                placeholder = new MediaElement();
                placeholder.Source = new Uri(file);
                video.Name = parts[0];
                video.Path = file;
                video.Thumbnail = file;
                video.Duration = (int)placeholder.NaturalDuration.TimeSpan.TotalSeconds;
                video.FileType = parts[1];
                video.Size = (int)new FileInfo(file).Length;
                videoList.Add(video);
                placeholder.Close();
                //VideoListView.Items.Add(video.Name);
            }
        }

        private void VideoListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VideoListView.SelectedItem != null)
            {
                // string selectedVideo = Path.Combine(videoDirectory, VideoListView.SelectedItem.ToString());
                // MessageBox.Show($"Selected Video: {selectedVideo}");
                // PropertiesTextBlock.Text = $"Name: {selectedVideo}\nPath: {selectedVideo}\nThumbnail: {selectedVideo}\nDuration: {selectedVideo}\nFile Type: {selectedVideo}\nSize: {selectedVideo}";


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
            PlayPauseIconText.Text = isPlaying ? "\uE768" : "\uE769"; // Pause = ⏸, Play = ▶
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

        }

        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VideoSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VideoPlayer.Position = TimeSpan.FromSeconds(VideoSlider.Value);
        }
    }
}
