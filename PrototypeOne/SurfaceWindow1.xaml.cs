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
using PrototypeOne.Menu;

using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;

using Microsoft.Surface.Presentation.Input;

using Microsoft.Xna.Framework.Input.Touch;


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
        public static double MenuTileSize = 80;
        //private Menu.Menu constMenu;
        private List<Menu.Menu> MenuList;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();
           
            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
           
            //CreateXAML();
           
            MenuList = new List<Menu.Menu>(15);
            Console.Out.WriteLine();
           // MakeCenterMenu();
            MakeIndividualMenus();
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

        public void CreateXAML()
        {

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

        public void MakeCenterMenu()
        {
            SquareList thislist = Catagory.ReadFile("Information/Top.xml");

            StartMenu centerMenu = new StartMenu(thislist);
            ScatterViewItem item = new ScatterViewItem();
            item.Center = new Point(this.Width / 2, this.Height / 2);
            item.CanMove = false;
            item.CanScale = false;
            item.Content = centerMenu.DrawMenu();
            scatter.Items.Add(item);

            centerMenu.AddEnterListenerToButtons(new EventHandler<System.Windows.Input.TouchEventArgs>(TouchEnterMenu));
            centerMenu.AddLeaveListenerToButtons(new EventHandler<System.Windows.Input.TouchEventArgs>(TouchLeaveMenu));
            centerMenu.AddUpListenerToButtons(new EventHandler<System.Windows.Input.TouchEventArgs>(TouchUpMenu));
            MenuList.Add(centerMenu);
        }

        public void BuildMenu(Point center,double angle) {
           
            IndividualMenu sideMenu = new Menu.IndividualMenu(Catagory.ReadFile("Information/Top.xml"));
            MenuList.Add(sideMenu);
            ScatterViewItem item = new ScatterViewItem();
            item.Center = center;
            item.CanMove = false;
            item.CanScale = false;
            item.CanRotate = false;
            Canvas canvas = sideMenu.DrawMenu();
            item.Orientation = angle;
          
           
            item.Content = canvas;
            scatter.Items.Add(item);
            sideMenu.AddEnterListenerToButtons(new EventHandler<System.Windows.Input.TouchEventArgs>(TouchEnterMenu));
            sideMenu.AddLeaveListenerToButtons(new EventHandler<System.Windows.Input.TouchEventArgs>(TouchLeaveMenu));
            sideMenu.AddUpListenerToButtons(new EventHandler<System.Windows.Input.TouchEventArgs>(TouchUpMenu));
            sideMenu.AddDownListenerToButtons(new EventHandler<System.Windows.Input.TouchEventArgs>(TouchEnterMenu));
           
        }

        public void MakeIndividualMenus()
        {
            //top
            BuildMenu(new Point(this.Width / 2, this.Height * 0.1), 180);
            //right
            BuildMenu(new Point(this.Width * 0.95, this.Height / 2), -90);
            //left
            BuildMenu(new Point(this.Width * 0.05, this.Height / 2), 90);
            //bottom
            BuildMenu(new Point(this.Width / 2, this.Height * 0.85), 0);
        }

        public void PlaceTreeMap(String fileName)
        {
            Console.Out.WriteLine("\n\ncalled with name: Information/" + fileName + "\n\n");
            TreeMenu map = new TreeMenu(Catagory.ReadFile("Information/" + fileName));


            map.AddUpListenerToButtons(TouchUpMenu);
            map.AddEnterListenerToButtons(new EventHandler<System.Windows.Input.TouchEventArgs>(TouchEnterMenu));
            map.AddLeaveListenerToButtons(new EventHandler<System.Windows.Input.TouchEventArgs>(TouchLeaveMenu));
            ScatterViewItem item = new ScatterViewItem();
            item.CanScale = false;
            item.Width = treeWidth;
            item.Height = treeHeight;
            item.Content = map.DrawMenu();
            scatter.Items.Add(item);
            MenuList.Add(map);

        }

     /*   public void TouchUpMenu(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsFingerRecognized() && sender is Path)
            {
                Path path = (Path)sender;
                if (path.Fill is GradientBrush)
                {
                    GradientBrush brush = (GradientBrush)path.Fill;
                    brush.GradientStops[1].Color = Colors.Black;
                }
                if (MenuList.Count < 15)//maybe remove the first ones opened
                {

                    PlaceTreeMap(constMenu.FileToOpen(path));
                }

            }
        }*/
        public void TouchEnterMenu(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsFingerRecognized() && sender is Path)
            {
                Path path = (Path)sender;
                if (path.Fill is GradientBrush)
                {
                    GradientBrush brush = (GradientBrush)path.Fill;
                    brush.GradientStops[1].Color = brush.GradientStops[0].Color;
                }

            }
        }
        public void TouchLeaveMenu(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsFingerRecognized() && sender is Path)
            {
                Path path = (Path)sender;
                if (path.Fill is GradientBrush)
                {
                    GradientBrush brush = (GradientBrush)path.Fill;
                    brush.GradientStops[1].Color =Colors.Black;
                }

            }
        }
        public void TouchUpMenu(object sender, System.Windows.Input.TouchEventArgs e)
        {
            
            if (sender is Path)
            {
                Path path = (Path)sender;
                Menu.Menu menu = FindTheMenu(path);
                string file = menu.FileToOpen(path);
                if (menu is TreeMenu)
                {
                    TreeMenu map = (TreeMenu)menu;
                    if (file != null && !file.Equals(""))
                    {
                        Console.Out.WriteLine("\n\nfile Exists as:\n" + file);
                        TreeMenu childAdded = map.addChild(Catagory.ReadFile("Information/" + file));
                        childAdded.AddUpListenerToButtons(TouchUpMenu);
                        childAdded.AddEnterListenerToButtons(new EventHandler<System.Windows.Input.TouchEventArgs>(TouchEnterMenu));
                        childAdded.AddDownListenerToButtons(new EventHandler<System.Windows.Input.TouchEventArgs>(TouchEnterMenu));
                        childAdded.AddLeaveListenerToButtons(new EventHandler<System.Windows.Input.TouchEventArgs>(TouchLeaveMenu));
                    }
                    else
                    {
                        if (map.HasExplanation(path))
                        {
                            if (map.Explaning)
                            {
                                map.RemoveExplanation(path);
                            }
                            else
                                map.Explane(path);

                        }

                    }

                }
                else//non tree menu
                {
                    if (path.Fill is GradientBrush)
                    {
                        GradientBrush brush = (GradientBrush)path.Fill;
                        brush.GradientStops[1].Color = Colors.Black;
                    }
                    if (MenuList.Count < 15)//maybe remove the first ones opened
                    {

                        PlaceTreeMap(menu.FileToOpen(path));
                    }
                }
            }
        }
        public Menu.Menu FindTheMenu(Path path)
        {
            foreach (Menu.Menu menu in MenuList)
            {
                if (menu.ContainsPath(path))
                {
                    return menu;
                }
            }
            return null;
        }


    }
}