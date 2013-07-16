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

namespace PrototypeOne.Menu
{
    //enum Location {Top,Right,Bottom,Left}
    public class IndividualMenu:Menu
    {
        public Point Center{get; set;}
        //public Location location{get; set;}
       
        public IndividualMenu(SquareList children)
            : base(children) { }
       

        public override Canvas DrawMenu()
        {
            Canvas canvas = new Canvas();
            double X=-(SurfaceWindow1.MenuTileSize*children.Count())/2;
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
