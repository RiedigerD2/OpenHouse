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
        public static double treeHeight = 325;
        public static double treeWidth = 450;
        public static double treeArea = treeWidth * treeHeight;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            //CreateXAML();
            PlaceTreeMap();
            MakeMenu();
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

        public void CreateXAML() {

            List<Catagory> saveList = new List<Catagory>();
            Catagory first = new Catagory();
            Catagory second = new Catagory();
            first.BackGroundColor = Colors.AntiqueWhite;
            first.TextColor = Colors.Black;
            first.Ratio = 0.45;
            first.Title = "firstTitle";

            second.BackGroundColor = Colors.AntiqueWhite;
            second.TextColor = Colors.Black;
            second.Ratio = 0.55;
            second.Title = "SecondTitle";

            saveList.Add(first);
            saveList.Add(second);

            Catagory.WriteFile(saveList, "Information/XMLFile4.xml");
        
        }

        public void MakeMenu()
        {
            menuLocation.Center = new Point(this.Width / 2, this.Height / 2);
            SquareList thislist = Catagory.ReadFile("Information/Top.xml");
            StartMenu wheel = new StartMenu(thislist);

            //foreach (Path path in map.paths)
            menu.Children.Add(wheel.DrawMenu());
           
        }

        public void PlaceTreeMap()
        {
          
            Tree map2 = new Tree(Catagory.ReadFile("Information/Carrers.xml"));
            canvas.Children.Add(map2.Picture);
                
            
        }

        
    }
}