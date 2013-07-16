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
        public static double MenuTileSize = 85;
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
            canvas.VerticalAlignment = VerticalAlignment.Top;
           
            item.Content = canvas;
            scatter.Items.Add(item);
            
            sideMenu.AddUpListenerToButtons(new EventHandler<System.Windows.Input.TouchEventArgs>(TouchUpMenu));
           
           
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

        public void PlaceTreeMap(String fileName,Square caller)
        {

            TreeMenu map = new TreeMenu(Catagory.ReadFile("Information/" + fileName),caller);


            map.AddUpListenerToButtons(TouchUpMenu);
            ScatterViewItem item = new ScatterViewItem();

          
            item.ContainerManipulationDelta +=new ContainerManipulationDeltaEventHandler(OnManipulation);
            //item.ContainerManipulationCompleted+=new ContainerManipulationCompletedEventHandler(ManipulationOver);

          
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
       
        
        public void TouchUpMenu(object sender, System.Windows.Input.TouchEventArgs e)
        {
            
            if (sender is SurfaceButton)
            {
                SurfaceButton button = (SurfaceButton)sender;
                Menu.Menu menu = FindTheMenu(button);
                if (menu == null)
                {
                    ScatterViewItem item = new ScatterViewItem();
                    item.Content = "NULL";
                    scatter.Items.Add(item);
                    return;
                }
                string file = menu.FileToOpen(button);
                if (menu is TreeMenu)
                {
                    TreeMenu map = (TreeMenu)menu;
                    if (file != null && !file.Equals(""))
                    {
                        Console.Out.WriteLine("\n\nfile Exists as:\n" + file);
                        TreeMenu childAdded = map.addChild(Catagory.ReadFile("Information/" + file),map.Get(button));
                        childAdded.AddUpListenerToButtons(TouchUpMenu);   
                    }
                    else
                    {
                        if (map.HasExplanation(button))
                        {
                            if (map.Explaning)
                            {
                                map.RemoveExplanation(button);
                            }
                            else
                                map.Explane(button);

                        }

                    }

                }
                else//non tree menu
                {
                    if (button.Background is GradientBrush)
                    {
                        GradientBrush brush = (GradientBrush)button.Background;
                        brush.GradientStops[1].Color = Colors.Black;
                    }
                    if (MenuList.Count < 15)//maybe remove the first ones opened
                    {
                        PlaceTreeMap(menu.FileToOpen(button),menu.Get(button));
                    }
                }
            }
        }
        public Menu.Menu FindTheMenu(SurfaceButton path)
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
        public Menu.Menu FindTheMenu(Canvas canvas)
        {
            foreach (Menu.Menu menu in MenuList)
            {
                if (menu is TreeMenu && ((TreeMenu)menu).CanvasIs(canvas))
                {
                    return menu;
                }
            }
            return null;
        }
        /*private void ManipulationOver(object sender, ContainerManipulationCompletedEventArgs e) {
           
            if (e.ScaleFactor != 1)
            {
                ScatterViewItem item = (ScatterViewItem)sender;
                Menu.Menu menu = FindTheMenu((Canvas)(item.Content));
                ((TreeMenu)menu).ReDraw(item.Width, item.Height-50);
            }
        
        }*/

        
      

        private void OnManipulation(object sender, ContainerManipulationDeltaEventArgs e)
        {
            ScatterViewItem item = (ScatterViewItem)sender;
           
            if (item.Center.X <= -item.Width * 0.20 || item.Center.Y <= -item.Height * 0.20 || item.Center.Y >= this.Height + item.Height * 0.20 || item.Center.X >= this.Width + item.Width * 0.20)
            {
                Menu.Menu menu = FindTheMenu((Canvas)(item.Content));
                scatter.Items.Remove(item);
                MenuList.Remove(menu);
            }
            if (e.ScaleFactor != 1)
            {
                //ScatterViewItem item = (ScatterViewItem)sender;
                Menu.Menu menu = FindTheMenu((Canvas)(item.Content));
                ((TreeMenu)menu).ReDraw(item.Width, item.Height);
                ((TreeMenu)menu).SizeCrumbs();
            }
            
        }
       

    }
}