using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace PrototypeOne
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            PlaceTreeMap();
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }
        public void PlaceTreeMap()
        {
            {
            Square one;
            FillInfo oneFill = new FillInfo(Brushes.Aqua);
            Square two;
            FillInfo twoFill = new FillInfo(Brushes.Red);
            Square three;
            FillInfo threeFill = new FillInfo(Brushes.Green);
            Square four;
            FillInfo fourFill = new FillInfo(Brushes.Ivory);
            SquareList thislist = new SquareList();
            one = new Square(4.8,oneFill);
            two = new Square(3.6,twoFill);
            three = new Square(2.4,threeFill);
            four = new Square(1.2,fourFill);
            thislist.Add(one);
            thislist.Add(two);
            thislist.Add(three);
            thislist.Add(four);
            
            Tree map = new Tree(thislist, 3, 4);

            /*DrawingImage myDrawingImage = new DrawingImage();
            myDrawingImage.Drawing = map.Squares;

            Image thisImage = new Image();
            thisImage.Source = myDrawingImage;
         */
            DrawingBrush theBrush = new DrawingBrush();
            theBrush.Stretch = Stretch.Uniform;
            theBrush.Drawing = map.Squares;

            Imagescatter.Background = theBrush;
            }
            {
            Square one;
            FillInfo oneFill = new FillInfo(Brushes.Aqua);
            Square two;
            FillInfo twoFill = new FillInfo(Brushes.Red);
            Square three;
            FillInfo threeFill = new FillInfo(Brushes.Green);
            Square four;
            FillInfo fourFill = new FillInfo(Brushes.Ivory);
            Square five;
            FillInfo fiveFill = new FillInfo(Brushes.Purple);
            Square six;
            FillInfo sixFill = new FillInfo(Brushes.LightGoldenrodYellow);
            Square seven;
            FillInfo sevenFill = new FillInfo(Brushes.Firebrick);
            SquareList thislist = new SquareList();
            one = new Square(6, oneFill);
            two = new Square(6, twoFill);
            three = new Square(4, threeFill);
            four = new Square(3, fourFill);
            five = new Square(2, fiveFill);
            six = new Square(2, sixFill);
            seven = new Square(1, sevenFill);
            thislist.Add(one);
            thislist.Add(two);
            thislist.Add(three);
            thislist.Add(four);
            thislist.Add(five);
            thislist.Add(six);
            thislist.Add(seven);

            Tree map = new Tree(thislist, 4, 6);

            DrawingImage myDrawingImage = new DrawingImage();
            myDrawingImage.Drawing = map.Squares;

            Image thisImage = new Image();
            thisImage.Source = myDrawingImage;

            brushscatter.Content = thisImage;
            }
        }
    }
}