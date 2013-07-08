using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Collections;
using System.Windows.Input;
using System.Windows.Controls;


namespace PrototypeOne
{
     class Tree
    {
        public SquareList children;
        private double height, width;
        private DrawingGroup squares;
        public ArrayList paths;
        private Canvas canvas;

        public Canvas Picture {  get { return canvas; } private set { canvas = value; } }
       public DrawingGroup Squares {  set { squares = value; }  get { return squares; } }

        public Tree(SquareList children, double height,double width)
        {
            this.children = children;
            this.height = height;
            this.width = width;
            this.canvas = new Canvas();
            squares = new DrawingGroup();
            paths = new ArrayList();
            MakeTree(0, width, height);
            FillDrawing();
        }
         public int Count(){
             return children.Count();
         }

        public void FillDrawing()
        {

            foreach (Square block in children)
            {
                System.Windows.Rect rect = new System.Windows.Rect(block.X, block.Y, block.Width, block.Height);
                RectangleGeometry rectangle = new RectangleGeometry(rect);

                System.Windows.Rect bor = new System.Windows.Rect(block.X, block.Y, block.GetTextBlock().Width, block.GetTextBlock().Height);
                RectangleGeometry border = new RectangleGeometry(bor);

                
                Path path = new Path();
                path.Data = rectangle;
              
                
                block.Path = path;
                
                path.Fill = block.Fill.Brush;
                path.Stroke = Brushes.Black;
                path.TouchLeave+= new EventHandler<TouchEventArgs>(PreviewTouchUp);
                path.TouchEnter+= new EventHandler<TouchEventArgs>(PreviewTouchDown);
               // path.MouseWheel+=new MouseWheelEventHandler(Path_MouseWheel);
                
                canvas.Children.Add(path);
                canvas.Children.Add(block.GetTextBlock());
                
                
                Pen mypen = new Pen(block.Fill.Brush, 0.1);
                GeometryDrawing drawing = new GeometryDrawing();
                drawing.Pen = mypen;
                drawing.Geometry = rectangle;
                squares.Children.Add(drawing);
                Console.Out.WriteLine("\n\n\n" + block.GetTextBlock().RenderSize + "\n" + block.GetTextBlock().RenderSize.Height);
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

        private void PreviewTouchUp(object sender, TouchEventArgs e)
        {

            TouchDevice finger= e.TouchDevice;
            if (sender is Path) {
                Path path = ((Path)sender);
                Square sqr = children.Get(path);
                path.Fill = children.Get((Path)sender).Fill.Brush;
                path.Data = new RectangleGeometry(new Rect(sqr.X, sqr.Y, sqr.Width, sqr.Height));
            }
            Console.Out.WriteLine("UP "+finger.GetTouchPoint((UIElement)sender).Position.X + " " + finger.GetTouchPoint((UIElement)sender).Position.Y);
            
        }

        private void PreviewTouchDown(object sender, TouchEventArgs e)
        {
            if (sender is Path)
            {
                Path path = ((Path)sender);
                Square sqr = children.Get(path);
                TextBlock textBlock = null;
                //path.Fill = Brushes.Gray;
                path.Data = new RectangleGeometry(new Rect(0, 0, width, height));


                Canvas mom = (Canvas)(path.Parent);
                int above = ((Canvas)(path.Parent)).Children.IndexOf(path) + 1;
                if (above <= ((Canvas)(path.Parent)).Children.Count)
                {
                    textBlock = (TextBlock)((Canvas)(path.Parent)).Children[above];
                    ((Canvas)(path.Parent)).Children.Remove(textBlock);
                }
                ((Canvas)(path.Parent)).Children.Remove(path);

                mom.Children.Add(path);

                mom.Children.Add(textBlock);



            }
            Console.Out.WriteLine("Down ");
        }
    }
}
