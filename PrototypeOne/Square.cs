using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeOne
{
    public class Square: IComparable<Square>
    {
        private double curWidth, curHeight, newWidth, newHeight, x,y;//x,y bottom left hand corner of square
        private double area;
        FillInfo fill;
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
        public double Area
        {
            get { return area; }
            private set { area = value; }
        }
        public  FillInfo Fill
        {
            get { return fill; }
            private set { fill = value; }
        }
        public double X { get { return x; } set { x = value; } }
        public double Y { get { return y; } set { y = value; } }
        public Square(Double size,FillInfo brush) 
        {
            fill = brush;
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
      

        public double lastAR()
        {
            return curHeight / curWidth > curWidth / curHeight ? curHeight / curWidth : curWidth / curHeight; 
        }

        public double newAR()
        {
            return newHeight / newWidth > newWidth / newHeight ? newHeight / newWidth : newWidth / newHeight;
        }
        

        public override string ToString()
        {
            return "current Height " + curHeight + "\ncurrent Width " + curWidth + "\nnew height " + newHeight + "\nnew Width" + newWidth +
                "\nArea " + area+ "\nX:"+x+"\nY"+y;
        }
    }
}
