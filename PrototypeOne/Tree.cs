using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace PrototypeOne
{
     class Tree
    {
        SquareList children;
        double height, width;
        DrawingGroup squares;

       public DrawingGroup Squares {  set { squares = value; }  get { return squares; } }

        public Tree(SquareList children, double height,double width)
        {
            this.children = children;
            this.height = height;
            this.width = width;
            squares = new DrawingGroup();
            
            MakeTree(0, width, height);
            FillDrawing();
        }

        public void FillDrawing()
        {

            foreach (Square block in children)
            {
                System.Windows.Rect rect = new System.Windows.Rect(block.X, block.Y, block.Width, block.Height);
                RectangleGeometry rectangle = new RectangleGeometry(rect);
                GeometryDrawing drawing = new GeometryDrawing();
                

                Pen mypen = new Pen(block.Fill.Brush, 0.1);

                drawing.Pen = mypen;
                drawing.Geometry = rectangle;
                squares.Children.Add(drawing);

            }
        }
        /**
         * used: Children prosessed 
         * widthLeft, heightLeft space remaining to fill with children
         **/
        void MakeTree(int used, double widthLeft, double heightLeft)
        {
            if (widthLeft <= 0 || heightLeft <= 0)
            {
                return;
            }

            if (used >= children.Count()-1)
            {
                children.SetSameHeight(used, 1, widthLeft);
                children.SetSameHeight(used, 1, widthLeft);
                AddChildrenToDrawSide(widthLeft, heightLeft, used, 1);
                for (int i = 0; i < children.Count(); i++)
                {
                    Console.Out.WriteLine(children.Get(i));
                }
                return;
            }
          
            
            if (widthLeft > heightLeft)
            {
                int divisions = 1;
                children.SetSameWidth(used, divisions, heightLeft);
                do
                {
                    divisions++;
                    children.SetSameWidth(used, divisions, heightLeft);
                } while (children.IsAverageARLess(used, divisions));
                for (int i = 0; i < children.Count(); i++)
                {
                    Console.Out.WriteLine(children.Get(i));
                }
                
                AddChildrenToDrawSide(widthLeft,heightLeft,used,divisions-1);
               
                MakeTree(used + divisions - 1, widthLeft - children.Get(used).Width, heightLeft);
            }
            else
            {
                int divisions = 1;
                children.SetSameHeight(used, divisions,widthLeft);
             
                do
                {
                    divisions++;
                    children.SetSameHeight(used, divisions, widthLeft);
                } while (children.IsAverageARLess(used, divisions));      
                AddChildrenToDrawBottom(widthLeft,heightLeft,used,divisions-1);
                MakeTree(used + divisions - 1, widthLeft, heightLeft - children.Get(used).Height);
            }
            

        }

        void AddChildrenToDrawSide(double curWidth, double curHeight, int start, int count)
        {
            double heightUsed = 0;
            for (int i = 0; i < count; i++)
            {
                children.Get(start + i).X = curWidth - children.Get(start + i).Width;
                children.Get(start + i).Y = curHeight - children.Get(start + i).Height-heightUsed;
                heightUsed += children.Get(start + i).Height;
               
            }
        }

        void AddChildrenToDrawBottom(double curWidth, double curHeight, int start, int count)
        {
            {
                double widthUsed = 0;
                for (int i = 0; i < count; i++)
                {
                    children.Get(start + i).X = curWidth - children.Get(start + i).Width-widthUsed;
                    children.Get(start + i).Y = curHeight - children.Get(start + i).Height;
                    widthUsed += children.Get(start + i).Width;
                    
                }
            }
        }
    }
}
