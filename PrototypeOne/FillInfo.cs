using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace PrototypeOne
{
    public class FillInfo
    {
        Brush colour;
        Brush textColor;
        string name;

        public Brush Brush
        {
            get { return colour; }
            private set { colour = value; }
        }
        public Brush TextColor
        {
            get { return textColor; }
            private set { textColor = value; }
        }
        public string Name
        {
            get { return name; }
            private set { name = value; }
        }

        public FillInfo(Color fill,string name,Color textColor){
           
            Brush brush = new LinearGradientBrush(); //   Color.FromRgb(255, 255, 255);

            GradientStop start = new GradientStop();
            start.Color = fill;
            start.Offset = 0;
            GradientStop end = new GradientStop();
            end.Color = Colors.Black;
            end.Offset = 2.5;

            TextColor = new SolidColorBrush(textColor);
     
            ((LinearGradientBrush)brush).GradientStops.Add(start);
            ((LinearGradientBrush)brush).GradientStops.Add(end);
            ((LinearGradientBrush)brush).StartPoint = new Point(0,0);
            ((LinearGradientBrush)brush).EndPoint = new Point(0, 1);
            colour = brush;
            this.name= name;
        }
    }
}
