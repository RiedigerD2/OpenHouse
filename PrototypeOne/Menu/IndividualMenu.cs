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

using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
//using System.Threading;
using System.Windows.Forms;
using System.Windows.Media.Animation;
namespace PrototypeOne.Menu
{
    //enum Location {Top,Right,Bottom,Left}
    public class IndividualMenu:Menu
    {
        public Canvas canvas;
        public Storyboard board;
        
        
        //public Point Center{get; set;}
        //public Location location{get; set;}

        public IndividualMenu(SquareList children,Storyboard board)
            : base(children)
        {
            this.board=board;
            myTimer=new Timer();
            myTimer.Tick += new EventHandler(myTimer_Tick);
            myTimer.Interval = 20000;
            myTimer.Enabled=true;
        }

        void myTimer_Tick(object sender, EventArgs e)
        {
            if (!interactive)
            {

                try
                {
                    Console.Out.WriteLine("\nThis\nState:" + board);
                    board.Begin();
                }
                catch (Exception el)
                {
                    Console.Out.WriteLine("\nThis\nException:" + el.Message);
                }
            }
            interactive = false;
        }
        public void CallBack(object state)
        {
           // if (!interactive)
            try
            {
                Console.Out.WriteLine("\nThis\nState:" + state);
                ((Storyboard)state).Begin();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("\nThis\nException:" + e.Message);
            }
        }

        public override Canvas DrawMenu()
        {
            if (canvas == null)
            {
                canvas = new Canvas();
            }
            double X=-(SurfaceWindow1.MenuTileSize*(double)children.Count())/2+0.5*SurfaceWindow1.MenuTileSize;
            for (int i = 0 ; i < children.Count();i++ ,X+=SurfaceWindow1.MenuTileSize)
            {
                Square sqr = children.Get(i);

                System.Windows.Rect rect = new System.Windows.Rect(X,0,SurfaceWindow1.MenuTileSize,SurfaceWindow1.MenuTileSize);
                RectangleGeometry rectangle = new RectangleGeometry(rect);

                SurfaceButton button = new SurfaceButton();
                button.Height = SurfaceWindow1.MenuTileSize;
                button.Width = SurfaceWindow1.MenuTileSize;
                button.RenderTransform = new TranslateTransform(X, 0);
                button.BorderBrush = Brushes.Black;
                button.BorderThickness = new Thickness(0.2); 
                sqr.Button = button;

                button.Background = sqr.Fill.Brush;
               


                canvas.Children.Add(button);
                TextBlock block = sqr.GetTextBlockNoTransform();
                block.RenderTransform = new TranslateTransform(X, 0);
                block.Height = SurfaceWindow1.MenuTileSize;
                block.Width = SurfaceWindow1.MenuTileSize;
                canvas.Children.Add(block);
            }

            return canvas;
        }
    }
}
