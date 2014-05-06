using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

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
    }


   public class VideoFile : MediaFile
    {
       [XmlAttribute]
       public override string Path { get { return @"Resources/Videos/" + path; } set { path = value; } }
        public VideoFile():base(){}
        public VideoFile(string path):base(path){}
    }

   public  class ImageFile : MediaFile
    {
       [XmlAttribute]
       public override string Path { get { return @"Resources/Images/" + path; } set { path = value; } }
        public ImageFile():base(){}
        public ImageFile(string path) : base(path) { }
    }
}
