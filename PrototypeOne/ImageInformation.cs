using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace PrototypeOne
{

    public enum _Placement {Top,Bottom,Inline};
    public class ImageInformation : ICloneable
    {
       [XmlAttribute]
        public string Path;
     [XmlAttribute]
        public double Width;
        [XmlAttribute]
        public double Height;
      [XmlAttribute]
        public _Placement Placement;

        public ImageInformation()
        { 
            Path = "EMPTY PATH";
            Width = 234.3;
            Height=123.3;
            Placement = _Placement.Inline;
        }
        public ImageInformation(string path,double height,double width,_Placement placement)
        {
            Path = (string)path.Clone();
            Width = width;
            Height = height;
            Placement = placement;
        }
        public Object Clone(){
           return new ImageInformation(this.Path,this.Height,this.Width,this.Placement);   
        }
        
    }
}
