using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media.Animation;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Input;

namespace VideoButton
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : SurfaceButton
    {
        public Uri video;
        private Boolean paused = true;     // Keeps track of the video's state

        public Uri Video
        {
            get { return video; }
        }

        public VideoPlayer(string el)
        {
            InitializeComponent();
            video =  new Uri(el, UriKind.Relative);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Set the sample video provided in Windows 7 as the source.
            DataContext = this;
           
            // Pause the MediaElement so that it fills the area.
            Media.Pause();

            Media.TouchDown += new EventHandler<TouchEventArgs>(Media_TouchDown);
            Media.MediaEnded += new RoutedEventHandler(Media_MediaEnded);
            //Media.MouseDown += new MouseButtonEventHandler(Media_TouchDown);

            if (!SurfaceEnvironment.IsSurfaceEnvironmentAvailable ||
                InteractiveSurface.PrimarySurfaceDevice.Tilt != Tilt.Horizontal)
            {
               // this.CanRotate = false;
               // this.Orientation = 0;
            }
        }

        /// <summary>
        /// Once the MediaEnded event is fired, Return the VideoPlayer
        /// to its starting state, ready to play again.
        /// </summary>
        void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            Media.Stop();
            BeginStoryboard(Resources["Pause"] as Storyboard);
            paused = true;
        }

        /// <summary>
        /// On a TouchDown event check whether the MediaElement is
        /// playing or paused and respond by performing the inverse.
        /// </summary>
        void Media_TouchDown(object sender, InputEventArgs e)
        {
            if (paused)
            {
                BeginStoryboard(Resources["Play"] as Storyboard);
                Media.Play();
                paused = false;
            }
            else
            {
                BeginStoryboard(Resources["Pause"] as Storyboard);
                Media.Pause();
                paused = true;
            }
            
        }
    }
}
