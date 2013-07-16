using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;


namespace PrototypeOne
{
    public class StartMenu:Menu.Menu
    {
        const double smlrad = 75;//radious of the smaller circle
        const double lrgrad = 200;// radious of the larger circle
        private double baseAngle;
            

        
        public StartMenu(SquareList list):base(list)
        {
            children = list;
            baseAngle = 2 * Math.PI / (list.Count()-1) ;
        }

        public override Canvas DrawMenu()
        {
          Canvas canvas=new Canvas();
            //draw inner circle
          EllipseGeometry center = new EllipseGeometry(new Point(0, 0), smlrad, smlrad);

          Path circle = new Path();
          circle.Data = center;
          circle.Fill = children.Get(0).Fill.Brush;
          circle.Stroke = Brushes.Black;
          canvas.Children.Add(circle);
          children.Get(0).Button.Content = circle;
          TextBlock MiddleText = children.Get(0).GetTextBlockNoTransform();
          MiddleText.Width = smlrad * 2;
          MiddleText.RenderTransform = new TranslateTransform(-smlrad, 0);
          canvas.Children.Add(MiddleText);
            //draw outer ring
            for(int i=1;i<children.Count();i++)
            {
                Square block = children.Get(i);

                PathFigure figure = new PathFigure();
                figure.StartPoint=new Point(smlrad*Math.Cos(baseAngle*i),smlrad*Math.Sin(baseAngle*i));
                figure.Segments.Add(new LineSegment(new Point(lrgrad*Math.Cos(baseAngle*i),lrgrad*Math.Sin(baseAngle*i)),true));
                figure.Segments.Add(new ArcSegment(new Point(lrgrad*Math.Cos(baseAngle*(i+1)),lrgrad*Math.Sin(baseAngle*(i+1))),new Size(lrgrad,lrgrad),0,false,SweepDirection.Clockwise,true));
                figure.Segments.Add(new LineSegment(new Point(smlrad*Math.Cos(baseAngle*(i+1)),smlrad*Math.Sin(baseAngle*(1+i))),true));
                figure.Segments.Add(new ArcSegment(new Point( smlrad * Math.Cos(baseAngle * i), smlrad * Math.Sin(baseAngle * i)), new Size(smlrad, smlrad), 0, false, SweepDirection.Counterclockwise, true));
                PathGeometry geometry=new PathGeometry();
                geometry.Figures.Add(figure);
                Path path = new Path();
                path.Data = geometry;
                

                block.Button.Content = path;

                path.Fill = block.Fill.Brush;
                path.Stroke = Brushes.Black;
               
               
               
                TextBlock text=block.GetTextBlockNoTransform();
                TransformGroup transform = new TransformGroup();
                //angle the text properly
                transform.Children.Add(new RotateTransform(- 0.5 * (baseAngle * 180 / Math.PI)));
                //move the text down into the right radious from center
                transform.Children.Add(new TranslateTransform(0,(lrgrad+smlrad)*0.5));
                //rotate to the correct section - 0.5 * (baseAngle * 180 / Math.PI)
                transform.Children.Add(new RotateTransform((baseAngle *i* 180 / Math.PI) - 0.5 * (baseAngle * 180 / Math.PI)));
                text.RenderTransform = transform;
                text.Width = FindWidthOfTriangles();
               
                // text.RenderTransform = new TranslateTransform(50, 200);
               
                canvas.Children.Add(path);
                canvas.Children.Add(text);
            }
            canvas.HorizontalAlignment = HorizontalAlignment.Center;
            canvas.VerticalAlignment = VerticalAlignment.Center;
            return canvas;
        }

        public  double FindWidthOfTriangles()
        {//not the simplest way of calculating the space
         //but the area given works better than using a one triangle calculation
            double averageRad = (lrgrad + smlrad) * 0.5;
            double hlfBase=baseAngle*0.5, smlAngle;
            double smlHeight,smlHypot;

            smlHeight = averageRad * Math.Sin(hlfBase);
            smlAngle = 90 - hlfBase;

            smlHypot=smlHeight / Math.Sin(smlAngle);
            return 2 * smlHypot;
        }

    }
}
