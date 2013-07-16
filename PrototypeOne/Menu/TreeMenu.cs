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

        private List<SurfaceButton> breadCrumbs;
        public override Canvas DrawMenu()
        {
            return canvas;
        }

        public TreeMenu(SquareList children,Square creator):base(children)
        {
            breadCrumbs = new List<SurfaceButton>();
            this.children = children;
            this.height = SurfaceWindow1.treeHeight;
            this.width = SurfaceWindow1.treeWidth;
            this.canvas = new Canvas();
            children.ResizeAreas((height-0.2*height) * width);
            MakeTree(0, width, height-0.2*height);

            SurfaceButton crumb = new SurfaceButton();
            crumb.Height = 0.2 * height;
            crumb.Background = creator.Button.Background;
            crumb.Content = creator.GetTextBlockNoTransform();
            crumb.PreviewTouchUp += new EventHandler<TouchEventArgs>(RetraceToBreadCrumb);
            breadCrumbs.Add(crumb);
            SizeCrumbs();
            
            canvas.Children.Add(crumb);
            FillDrawing();
        }
        public TreeMenu(SquareList children)
            : base(children)
        {
            breadCrumbs = new List<SurfaceButton>();
            this.children = children;
            this.height = SurfaceWindow1.treeHeight;
            this.width = SurfaceWindow1.treeWidth;
            this.canvas = new Canvas();
            children.ResizeAreas((height - 0.2 * height) * width);
            MakeTree(0, width, height - 0.2 * height);

            FillDrawing();
        }
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
                AddChildrenToDrawBottom(widthLeft,height-heightLeft,used,divisions-1);
                MakeTree(used + divisions - 1, widthLeft, heightLeft - children.Get(used).Height);
            }
        }
        void AddChildrenToDrawSide(double curWidth, double curHeight, int start, int count)
        {
            double heightUsed = 0;
            for (int i = 0; i < count; i++)
            {
                children.Get(start + i).X = curWidth - children.Get(start + i).Width;
                children.Get(start + i).Y = height-curHeight +heightUsed;
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
                    children.Get(start + i).Y = curHeight;// -children.Get(start + i).Height;
                    widthUsed += children.Get(start + i).Width;
                    
                }
            }
        }


        public void FillDrawing()
        {

            foreach (Square block in children)
            {
               SurfaceButton button = new SurfaceButton();
                button.Background = block.Fill.Brush;
                button.Height = block.Height;
                button.Width = block.Width;
                button.RenderTransform = new TranslateTransform(block.X, block.Y);
                //button.Content = block.GetTextBlockNoTransform();
                button.Foreground = block.Fill.TextColor;
                button.BorderBrush = Brushes.Black;
                button.BorderThickness = new Thickness(20);
                block.Button = button;

                canvas.Children.Add(button);
                canvas.Children.Add(block.GetTextBlock());
               
            }
        }
        public void RepositionButtons()
        {

            foreach (Square block in children)
            {
                
                block.Button.Height = block.Height;
                block.Button.Width = block.Width;
                block.Button.RenderTransform = new TranslateTransform(block.X, block.Y);

               
                canvas.Children.Add(block.GetTextBlock());
            }
        }
        public void ReDraw(double width, double height) {
            this.height = height;
            this.width = width;
            removeTextBlocksFromCanvas();
            children.ResizeAreas(width * (height - 0.2 * height));
            MakeTree(0, width, height - 0.2 * height);
            RepositionButtons();
            if (child != null)
            {
                
                child.ReDraw(width, height);
                canvas.Children.Remove(child.DrawMenu());
                canvas.Children.Insert(canvas.Children.Count, child.DrawMenu());
            }
        
        }
        public void removeTextBlocksFromCanvas()
        {

            foreach (Square sqr in children)
            {
                canvas.Children.Remove(sqr.GetTextBlock());
            }
        
        }
        /**
         * used: Children prosessed 
         * widthLeft, heightLeft space remaining to fill with children
         **/
        

        public TreeMenu addChild(SquareList grandChildren,Square creator){

            if (child == null)
            {
                child = new TreeMenu(grandChildren);
                canvas.Children.Add(child.DrawMenu());
                child.ReDraw(width, height);

                SurfaceButton crumb = new SurfaceButton();
                crumb.Background = creator.Button.Background;
                
                crumb.Content = creator.GetTextBlockNoTransform();
                crumb.PreviewTouchUp += new EventHandler<TouchEventArgs>(RetraceToBreadCrumb);
                breadCrumbs.Add(crumb);
                canvas.Children.Add(crumb);
                SizeCrumbs();

                return child;
            }
            else
            {
                SurfaceButton crumb = new SurfaceButton();
                crumb.Background = creator.Button.Background;
                
                crumb.Content = creator.GetTextBlockNoTransform();
                crumb.PreviewTouchUp += new EventHandler<TouchEventArgs>(RetraceToBreadCrumb);
                breadCrumbs.Add(crumb);
                canvas.Children.Add(crumb);
                SizeCrumbs();
                return child.addChild(grandChildren);
            }

        }
        public TreeMenu addChild(SquareList grandChildren)
        {

            if (child == null)
            {
                child = new TreeMenu(grandChildren);
                canvas.Children.Add(child.DrawMenu());
                child.ReDraw(width, height);
                return child;
            }
            else
                return child.addChild(grandChildren);

        }
        public void SizeCrumbs(){

            for (int i=0; i < breadCrumbs.Count; i++)
            {
                SurfaceButton button = breadCrumbs[i];
                button.Width = (width-0.2*height)/breadCrumbs.Count;
                button.Height = 0.2 * height;
                button.RenderTransform = new TranslateTransform((width * 0.8) / breadCrumbs.Count * i, 0);
            }

        }
        
        public override Square Get(SurfaceButton sqr)
        {
            if (children.Get(sqr) == null)
            {
                return child.Get(sqr);
            }
            return children.Get(sqr);
        }

        public override bool ContainsPath(SurfaceButton sender)
        {
            return children.Get(sender) !=null || (child!=null && child.ContainsPath(sender));
        }
        public bool CanvasIs(Canvas canvas)
        {
            return this.canvas.Equals(canvas) || (child != null && child.CanvasIs(canvas));
        }

        public override string FileToOpen(SurfaceButton sender)
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

        public bool HasExplanation(SurfaceButton sender)
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


        public void Explane(SurfaceButton sender) {
            Explaning = true;
           
            
            sender.Width=width;
            sender.Height=height*0.8;
            sender.RenderTransform=new TranslateTransform(0,height*0.2);



            Square sqr = children.Get(sender);
            if (sqr == null)
            {
                sqr = child.Get(sender);
            }
            Canvas mom = (Canvas)(sender.Parent);

           
            mom.Children.Remove(sender);
            mom.Children.Add(sender);

            TextBlock Title = new TextBlock();
            Title.TextAlignment = TextAlignment.Center;
            Title.TextDecorations = TextDecorations.Underline;
            Title.Text = "\n"+sqr.Name;
            Title.Width = width;
            Title.Height = 0.2 * height;
            Title.RenderTransform = new TranslateTransform(width * 0.05, 0.25 * height);
            Title.IsHitTestVisible = false;

            mom.Children.Add(Title);

            TextBlock Body = new TextBlock();
            Body.Text = sqr.Explanation;
            Body.IsHitTestVisible = false;
            Body.Width = width*0.9;
            Body.Height = 0.8 * height;
            Body.RenderTransform = new TranslateTransform(width*0.05, 0.35 * height);
            mom.Children.Add(Body);
            
        }

        public void RemoveExplanation(SurfaceButton sender)
        {


            Square sqr = children.Get(sender);
            if (sqr == null)
            {
                sqr = child.Get(sender);
            }
          
            sender.Height = sqr.Height;
            sender.Width = sqr.Width;
            sender.RenderTransform = new TranslateTransform(sqr.X, sqr.Y);

            Canvas mom = (Canvas)(sender.Parent);
           
            int above = mom.Children.IndexOf(sender) + 1;
           
            for (int i = 0; i < 2 && (TextBlock)((Canvas)(sender.Parent)).Children[above] is TextBlock;i++ )
            {
                TextBlock textBlock = (TextBlock)((Canvas)(sender.Parent)).Children[above];
                mom.Children.Remove(textBlock);
            }
            mom.Children.Remove(sender);
            mom.Children.Insert(0, sender);
            Explaning = false;
        }

        private void PreviewTouchUp(object sender, TouchEventArgs e)
        {

            TouchDevice finger= e.TouchDevice;
            if (sender is SurfaceButton) {
                SurfaceButton path = ((SurfaceButton)sender);
                Square sqr = children.Get(path);
                path.Background = children.Get((SurfaceButton)sender).Fill.Brush;
                //dont think needed: path.Data = new RectangleGeometry(new Rect(sqr.X, sqr.Y, sqr.Width, sqr.Height));
            }
            Console.Out.WriteLine("UP "+finger.GetTouchPoint((UIElement)sender).Position.X + " " + finger.GetTouchPoint((UIElement)sender).Position.Y);
            
        }

        private void PreviewTouchDown(object sender, TouchEventArgs e)
        {
            if (sender is SurfaceButton)
            {
                SurfaceButton path = ((SurfaceButton)sender);
                Square sqr = children.Get(path);
                TextBlock textBlock = null;
                //path.Fill = Brushes.Gray;
                //path.Data = new RectangleGeometry(new Rect(0, 0, width, height));


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

        private void RetraceToBreadCrumb(object sender, TouchEventArgs e)
        {

            if (sender is SurfaceButton)
            {
                Console.Out.WriteLine("Don't You know it\n\n\n");
                SurfaceButton button = (SurfaceButton)sender;
                if (breadCrumbs.IndexOf(button) < breadCrumbs.Count - 1)
                {
                    KeepChildren(breadCrumbs.IndexOf(button) + 1, this.canvas);
                    for (int i = breadCrumbs.Count - 1; i > breadCrumbs.IndexOf(button); i--)
                    {
                        canvas.Children.Remove(breadCrumbs[i]);

                        breadCrumbs.RemoveAt(i);
                        SizeCrumbs();
                    }
                }
            }
        }
        private void KeepChildren(int keeping, Canvas parentCanvas)
        {
            if (keeping > 0)
            {
                child.KeepChildren(keeping - 1, this.canvas);
                if (keeping == 1)
                {
                    
                    child = null;
                }
                
                return;
            }
            else
            {
                if (child != null)
                {
                    child.KeepChildren(-1, this.canvas);
                    child = null;
                }
                parentCanvas.Children.Remove(this.canvas);
                return;
            }
        }
        
    }
}
