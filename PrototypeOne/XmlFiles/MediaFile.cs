using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PrototypeOne.XmlFiles
{
    public  class MediaFile
    {
        protected string path;

        [XmlAttribute]
        virtual public string Path { get { return path; } set { path = value; } }

       public MediaFile()
       {
           Path = "Empty";
       }
       public MediaFile(string path)
       {
           Path = path;
       }

       public override string ToString()
       {
           return Path;
       }
       virtual public UIElement Contents()
       {

           return new TextBlock(new System.Windows.Documents.Run (Path));
       }
    }


   public class VideoFile : MediaFile
    {
        [XmlAttribute]
        public override string Path { get { return @"Resources/Videos/" + path; } set { path = value; } }
        public VideoFile():base(){}
        public VideoFile(string path):base(path){}
        public override UIElement Contents()
        {
            return new VideoButton.VideoPlayer(Path);
        }
    }

   public  class ImageFile : MediaFile
    {
       [XmlAttribute]
        public override string Path { get { return @"Resources/Images/" + path; } set { path = value; } }
        public ImageFile():base(){}
        public ImageFile(string path) : base(path) { }
        public override UIElement Contents()
        {
            Image img = new Image();
            img.Source = new BitmapImage(new Uri(Path, UriKind.Relative));
            return img;
        }
    }

   public class TextFile : MediaFile
   {
       [XmlElement]
       public string Text { get; set; }
       public TextFile():base(){}
       public TextFile(string path) : base(path) { }
       public override UIElement Contents()
       {
           return new TextBlock(new System.Windows.Documents.Run(Text));
       }
   }
}
