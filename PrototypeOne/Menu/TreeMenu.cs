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

using Microsoft.Xna.Framework.Input.Touch;

using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;


namespace PrototypeOne
{
    public class TreeMenu : Menu.Menu
    {
        
        private double height, width;
       
        private Canvas canvas;
        
        TreeMenu child;

        public bool Explaning {  get;  set; }

        public override Canvas DrawMenu()
        {
            return canvas;
        }

        public TreeMenu(SquareList children):base(children)
        {
            
            this.children = children;
            this.height = SurfaceWindow1.treeHeight;
            this.width = SurfaceWindow1.treeWidth;
            this.canvas = new Canvas();
            
            MakeTree(0, width, height);
            FillDrawing();
        }
        
        public void FillDrawing()
        {

            foreach (Square block in children)
            {
                System.Windows.Rect rect = new System.Windows.Rect(block.X, block.Y, block.Width, block.Height);
                RectangleGeometry rectangle = new RectangleGeometry(rect);

                Path path = new Path();
                path.Data = rectangle;
              
                
                block.Path = path;
                
                path.Fill = block.Fill.Brush;
                path.Stroke = Brushes.Black;

                SurfaceButton button = new SurfaceButton();
                button.Background = block.Fill.Brush;
                button.Height = block.Height;
                button.Width = block.Width;
                button.RenderTransform = new TranslateTransform(block.X, block.Y);
                button.Content = block.GetTextBlock().Text;
                canvas.Children.Add(button);
                //canvas.Children.Add(block.GetTextBlock());
                
                
               
                
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

        public TreeMenu addChild(SquareList grandChildren){
            child = new TreeMenu(grandChildren);    
            canvas.Children.Add(child.DrawMenu());
            return child;
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


        public override bool ContainsPath(Path sender)
        {
            return children.Get(sender) !=null || (child!=null && child.ContainsPath(sender));
        }

        public override string FileToOpen(Path sender)
        {
            if (child!=null && child.ContainsPath(sender))
            {
                return child.FileToOpen(sender);

            }
            else
            {
                return children.Get(sender).SubFile;
            }
        }

        public void RemoveFromParent() {
            ((ScatterView)((ScatterViewItem)canvas.Parent).Parent).Items.Remove(canvas.Parent);
        }

        public bool HasExplanation(Path sender)
        {
            if (child != null && child.ContainsPath(sender))
            {
                return child.HasExplanation(sender);

            }
            else
            {
                return children.Get(sender).Explanation!=null;
            }
        }


        public void Explane(Path sender) {
            Explaning = true;
            sender.Data = new RectangleGeometry(new Rect(0, 0, width, height));
            Path path = ((Path)sender);
            Square sqr = children.Get(path);
            Canvas mom = (Canvas)(path.Parent);        
           
            int above = ((Canvas)(path.Parent)).Children.IndexOf(path) + 1;
            
            if (above < mom.Children.Count)
            {
                TextBlock textBlock = (TextBlock)((Canvas)(path.Parent)).Children[above];
                ((Canvas)(path.Parent)).Children.Remove(textBlock);
            }
            mom.Children.Remove(path);
            mom.Children.Add(path);

            TextBlock Title = new TextBlock();
            Title.TextAlignment = TextAlignment.Center;
            Title.TextDecorations = TextDecorations.Underline;
            Title.Text = "\n"+sqr.Name;
            Title.Width = width;
            Title.Height = 0.2 * height;
            Title.IsHitTestVisible = false;

            mom.Children.Add(Title);

            TextBlock Body = new TextBlock();
            Body.Text = sqr.Explanation;
            Body.IsHitTestVisible = false;
            Body.Width = width*0.9;
            Body.Height = 0.8 * height;
            Body.RenderTransform = new TranslateTransform(width*0.05, 0.15 * height);
            mom.Children.Add(Body);
            
        }

        public void RemoveExplanation(Path sender)
        {
            
            
            Square sqr = children.Get(sender);
            sender.Data = new RectangleGeometry(new Rect(sqr.X, sqr.Y, sqr.Width, sqr.Height));

            Canvas mom = (Canvas)(sender.Parent);
           
            int above = mom.Children.IndexOf(sender) + 1;
           
            for (int i = 0; i < 2 && (TextBlock)((Canvas)(sender.Parent)).Children[above] is TextBlock;i++ )
            {
                TextBlock textBlock = (TextBlock)((Canvas)(sender.Parent)).Children[above];
                mom.Children.Remove(textBlock);
            }
            mom.Children.Remove(sender);
            mom.Children.Add(sender);
            mom.Children.Add(sqr.GetTextBlock());
            Explaning = false;
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
