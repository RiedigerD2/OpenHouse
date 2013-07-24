using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using Microsoft.Surface.Presentation.Controls;
namespace PrototypeOne
{
    public class Square: IComparable<Square>
    {
        private double curWidth, curHeight, newWidth, newHeight, x,y;//x,y bottom left hand corner of square
        private double area;
        public double ratio;
        FillInfo fill;
        private TextBlock textBlock;
        //new class later with display info string,background color, text color
        //once placed need to set a their location x,y
        public double Width {
            get { return curWidth; }
            set { curWidth = newWidth;
                  curHeight = newHeight;
                  newWidth = value;
                  newHeight = area / newWidth;
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
                newWidth = area / newHeight;
            }
        }
        public SurfaceButton Button
        {
            get;
            set;
        }
        public double Area
        {
            get { return area; }
             set { area = value; }
        }
        public string Name {
            get { return fill.Name; }
        }
        public string SubFile { get; set; }
        public string Explanation { get; set; }
        public Color BackGround { get { return ((GradientBrush)fill.Brush).GradientStops[0].Color; } }
        public  FillInfo Fill
        {
            get { return fill; }
            private set { fill = value; }
        }
        
        public double X { get { return x; } set { x = value; } }
        public double Y { get { return y; } set { y = value; } }
        public Square(Double size,FillInfo filler) 
        {
            fill = filler;
            area = size;
          
        }

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
        /*override bool equals(object obj)
        {
            return Button.Equals(obj);
        }*/

        public double lastAR()
        {
            return curHeight / curWidth > curWidth / curHeight ? curHeight / curWidth : curWidth / curHeight; 
        }

        public double newAR()
        {
            return newHeight / newWidth > newWidth / newHeight ? newHeight / newWidth : newWidth / newHeight;
        }

        public TextBlock GetTextBlock()
        {
            if(textBlock==null){
             textBlock = new TextBlock();
            }
            textBlock.Text = fill.Name;
            textBlock.RenderTransform = new TranslateTransform(X, Y + 0.5 * curHeight);
            textBlock.Height = curHeight*0.5;
            textBlock.Width = curWidth;
            textBlock.Focusable = false;
            textBlock.IsHitTestVisible = false;
            textBlock.Foreground = fill.TextColor;
            textBlock.TextAlignment = TextAlignment.Center;

            return textBlock;
        }
        public TextBlock GetTextBlockTop()
        {
            if (textBlock == null)
            {
                textBlock = new TextBlock();
            }
            textBlock.Text = fill.Name;
            textBlock.RenderTransform = new TranslateTransform(X, Y);
            textBlock.Height = curHeight * 0.5;
            textBlock.Width = curWidth;
            textBlock.Focusable = false;
            textBlock.IsHitTestVisible = false;
            textBlock.Foreground = fill.TextColor;
            textBlock.TextAlignment = TextAlignment.Center;

            return textBlock;
        }
        
        public TextBlock GetTextBlockNoTransform()
        {
            TextBlock text = new TextBlock();

            text.Text = fill.Name;
            text.Focusable = false;
            text.IsHitTestVisible = false;
            text.Foreground = fill.TextColor;
            text.VerticalAlignment = VerticalAlignment.Center;
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.TextAlignment = TextAlignment.Center;

            return text;
        }

        public override string ToString()
        {
            return "Square: " + Name + ", Color: " + BackGround;
        }
    }
}
