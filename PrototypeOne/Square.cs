using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media.Imaging;
namespace PrototypeOne
{
    public class Square: IComparable<Square>
    {
        /// <summary>
        /// cur and new variables to test wether or not hte 
        /// cur aspect ratio is better or worse than the potential new asspect ratio
        /// </summary>
        private double curWidth, curHeight, newWidth, newHeight;
  
        private TextBlock textBlock;
        
        public double Width {
            get { return curWidth; }
            set { curWidth = newWidth;
                  curHeight = newHeight;
                  newWidth = value;
                  newHeight = Area / newWidth;
            }
        }      
        public double Height
        {
            get { return curHeight; }
            set
            {
                curHeight = newHeight;
                curWidth = newWidth;
                newHeight = value;
                newWidth = Area / newHeight;
            }
        }
        public double X { get; set; }
        public double Y { get; set; }
        public SurfaceButton Button
        {
            get;
            set;
        }

        public double Ratio;
        public double Area;
        public string Name;
        public string SubFile;
        public string Explanation;

        //public Image Image { get; private set; }
        public string ImageString;
        //public MediaElement Video { get; private set; }
        public string VideoString;

        private Brush backGroundColorBrush;
        private Brush backGroundImageBrush;
        public Color BackGroundColor { get { return ((GradientBrush)backGroundColorBrush).GradientStops[0].Color; } private set { BackGroundColor = value; } }
        public Brush BackGroundBrush {
            get
            {
                if (backGroundImageBrush != null)
                {
                    return backGroundImageBrush;
                }
                return backGroundColorBrush;
            }
            private set { BackGroundBrush = value; }
        }
        public Color TextColor { get { return ((SolidColorBrush)TextBrush).Color; } set { TextColor = value; } }
        public Brush TextBrush { get; private set; }



        
        public void setBackGround(Color backGroundColor){
            
            Brush brush = new LinearGradientBrush(); 

            GradientStop start = new GradientStop();
            start.Color = backGroundColor;
            start.Offset = 0;
            GradientStop end = new GradientStop();
            end.Color = Colors.Black;
            end.Offset = 2.5;


            ((LinearGradientBrush)brush).GradientStops.Add(start);
            ((LinearGradientBrush)brush).GradientStops.Add(end);
            ((LinearGradientBrush)brush).StartPoint = new Point(0, 0);
            ((LinearGradientBrush)brush).EndPoint = new Point(0, 1);
            backGroundColorBrush = brush;
        
        }
        /// <summary>
        /// uses the path to an image
        /// to create a ImageBrush to use for backgrounds
        /// </summary>
        /// <param name="imagePath">path to an image file</param>
        public void setBackGround(string imagePath)
        {

             backGroundImageBrush = new ImageBrush(new BitmapImage(new Uri(imagePath, UriKind.Relative)));
        }
        public void setTextColor(Color textColor)
        {
            TextBrush = new SolidColorBrush(textColor);
        }


       
        public Square(Double size,string name) 
        {
            Area = size;
            Name = name;
        }
        /// <summary>
        /// needed to create a squarelist
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int IComparable<Square>.CompareTo(Square obj) {
            if (this.Area > obj.Area)
            {
                return -1;
            }
            if (obj.Area > this.Area)
            {
                return 1;
            }
            return 0;
        }     
        /// <summary>
        /// 
        /// </summary>
        /// <returns>previous aspect ratio</returns>
        public double lastAR()
        {
            return curHeight / curWidth > curWidth / curHeight ? curHeight / curWidth : curWidth / curHeight; 
        }
        /// <summary>
        /// returns potential new aspect ratio
        /// </summary>
        /// <returns></returns>
        public double newAR()
        {
            return newHeight / newWidth > newWidth / newHeight ? newHeight / newWidth : newWidth / newHeight;
        }
        /// <summary>
        /// returns a text block wit all the right 
        /// transforms so the block 
        /// ends in the middle the button 
        /// it's associated with
        /// </summary>
        /// <returns></returns>
        public TextBlock GetTextBlock()
        {
            if(textBlock==null){
             textBlock = new TextBlock();
            }
            textBlock.Text = Name;
            textBlock.RenderTransform = new TranslateTransform(X, Y + 0.5 * curHeight);
            textBlock.Height = curHeight*0.5;
            textBlock.Width = curWidth;
            textBlock.Focusable = false;
            textBlock.IsHitTestVisible = false;
            textBlock.Foreground = TextBrush;
            textBlock.TextAlignment = TextAlignment.Center;

            return textBlock;
        }
        /// <summary>
        /// returns a text block wit all the right 
        /// transforms so the block 
        /// at the top of the button 
        /// it's associated with 
        /// </summary>
        /// <returns></returns>
        public TextBlock GetTextBlockTop()
        {
            if (textBlock == null)
            {
                textBlock = new TextBlock();
            }
            
            textBlock.Text = Name;
            textBlock.RenderTransform = new TranslateTransform(X, Y);
            textBlock.Height = curHeight;
            textBlock.Width = curWidth;
            textBlock.Focusable = false;
            textBlock.IsHitTestVisible = false;
            textBlock.Foreground = TextBrush;
            textBlock.TextAlignment = TextAlignment.Center;

            return textBlock;
        }     
        /// <summary>
        /// returns a text block with no transformations
        /// </summary>
        /// <returns></returns>
        public TextBlock GetTextBlockNoTransform()
        {
            TextBlock text = new TextBlock();

            text.Text = Name;
            text.Focusable = false;
            text.IsHitTestVisible = false;
            text.Foreground = TextBrush;
            text.VerticalAlignment = VerticalAlignment.Center;
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.TextAlignment = TextAlignment.Center;

            return text;
        }
        public override string ToString()
        {
            return "Square: " + Name + ", Color: " + BackGroundColor;
        }
    }

   
}
