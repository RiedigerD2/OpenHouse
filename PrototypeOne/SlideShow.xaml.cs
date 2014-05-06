using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using PrototypeOne.XmlFiles;
using PrototypeOne;
using VideoButton;

namespace PrototypeOne
{
    /// <summary>
    /// Interaction logic for SlideShow.xaml
    /// </summary>
    /// 
    
    [TemplatePart(Name = "NextButton", Type = typeof(Path))]
    [TemplatePart(Name = "BackButton", Type = typeof(Path))]
    public partial class SlideShow : ContentControl
    {
        private Path nextButton;

        private Path NextButton
        {
            get
            {
                return nextButton;
            }

            set
            {
                if (nextButton != null)
                {
                    nextButton.TouchUp -=
                        new EventHandler<TouchEventArgs>(NextButton_PreviewTouchUp);
                }
                nextButton = value;

                if (nextButton != null)
                {
                    nextButton.TouchUp += new EventHandler<TouchEventArgs>(NextButton_PreviewTouchUp);
                }
            }
        }

        private Path backButton;

        private Path BackButton
        {
            get
            {
                return backButton;
            }

            set
            {
                if (backButton != null)
                {
                    backButton.TouchUp -=
                        new EventHandler<TouchEventArgs>(BackButton_PreviewTouchUp);
                }
                backButton = value;

                if (backButton != null)
                {
                    backButton.TouchUp += new EventHandler<TouchEventArgs>(BackButton_PreviewTouchUp);
                }
            }
        }


        public override void OnApplyTemplate()
        {
          //Allows us to add event handlers to the Paths
            NextButton = GetTemplateChild("NextButton") as Path;
            BackButton = GetTemplateChild("BackButton") as Path;
        }
        public List<MediaFile> Slides { get; set; }
        private int current;
        public SlideShow()
        {
            InitializeComponent();
            current=0;
        }
        public SlideShow(List<MediaFile> newSlides)
        {
            InitializeComponent();

            Slides = newSlides;
            current = 0;
            UpdateContent();
        }
        private void NextButton_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            if (current + 1 < Slides.Count)
            {
                current++;
            }
            else
            {
                current = 0;
            }
            UpdateContent();
        }

        private void BackButton_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            if (current - 1 >=0 )
            {
                current--;
            }
            else
            {
                current = Slides.Count-1;
            }
            UpdateContent();
        }

        private void UpdateContent()
        {
            MediaFile currentFile = Slides[current];
            if (Slides.Count == 0)
            {
                current = 0;
                return;
            }
            if (currentFile is ImageFile)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(currentFile.Path, UriKind.Relative));
                this.Content = img;
            }
            if (currentFile is VideoFile)
            {
                VideoPlayer player = new VideoPlayer(currentFile.Path);
                this.Content = player;
            }

        }
    }
}
