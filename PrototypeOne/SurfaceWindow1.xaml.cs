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
using System.Windows.Forms;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;

using Microsoft.Surface.Presentation.Input;
using System.Collections.ObjectModel;

using System.Windows.Media.Animation;
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
        public List<Menu.Menu> MenuList;
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();
           
            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
           
            CreateXAML();
            MenuList = new List<Menu.Menu>(15);
            Console.Out.WriteLine();
           // MakeCenterMenu();
            MakeIndividualMenus();

            DataContext = this;
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


        /// <summary>
        /// Creates XML Document So A user can see what the document should/can look like
        /// </summary>
        public void CreateXAML()
        {

            List<Catagory> saveList = new List<Catagory>();
            Catagory first = new Catagory();
            Catagory second = new Catagory();
            first.BackGroundColor = Colors.AntiqueWhite;
            first.TextColor = Colors.Black;
            first.Ratio = 0.45;
            first.Title = "firstTitle";

            first.Video = "C:\\Users\\Public\\Videos\\Sample Videos\\Wildlife.wmv";
            first.Image = "Resources/X.jpg";
            first.Explanation = "long string with\n to make sence i should say newline\nLol";
            first.SubCatagoryFile = "Top";
            second.BackGroundColor = Colors.AntiqueWhite;
            second.TextColor = Colors.Black;
            second.Ratio = 0.55;
            second.Title = "SecondTitle";

            saveList.Add(first);
            saveList.Add(second);

            Catagory.WriteFile(saveList, "Information/XMLFile4.xml");

        }
        /// <summary>
        /// Implemented for circular menu will no work Easily with surface buttons
        /// Keeping incase of another menu change
        /// </summary>
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

        /// <summary>
        /// Creates one instance of the top most menu 
        /// </summary>
        /// <param name="center">The Menus center</param>
        /// <param name="angle">The angle of the menu</param>
        public IndividualMenu BuildMenu(Point center,double angle) {
            ScatterViewItem item = NonMovingItemFactory(center, angle);

            

            IndividualMenu sideMenu = new Menu.IndividualMenu(Catagory.ReadFile("Information/Top.xml"), AddAnimation(item));
            MenuList.Add(sideMenu);


            ScatterViewItem touchme = NonMovingItemFactory(center, angle);
            touchme.Content = "Touch Here To begin";
            touchme.Background = Brushes.Transparent;     
            touchme.Focusable = false;
            scatter.Items.Add(touchme);
            
            Canvas canvas = sideMenu.DrawMenu();
            canvas.VerticalAlignment = VerticalAlignment.Top;
            item.Content = canvas;
            scatter.Items.Add(item);


            sideMenu.AddUpListenerToButtons(new EventHandler<System.Windows.Input.TouchEventArgs>(TouchUpMenu));

            return sideMenu;
        }

        /// <summary>
        /// adds a opacity animation to item
        /// </summary>
        /// <param name="item"></param>
        private Storyboard AddAnimation(ScatterViewItem item)
        {

            DoubleAnimation Animation = new DoubleAnimation();
            Animation.From = 1.0;
            Animation.To = -0.3;
            Animation.Duration = new Duration(TimeSpan.FromSeconds(5));
            Animation.AutoReverse = true;
            Animation.RepeatBehavior = RepeatBehavior.Forever;

            Storyboard Storyboard = new Storyboard();
            Storyboard.Children.Add(Animation);
            Storyboard.SetTarget(Animation, item);
            Storyboard.SetTargetProperty(Animation, new PropertyPath(ScatterViewItem.OpacityProperty));
            Storyboard.Begin();
            return Storyboard;
        }
        /// <summary>
        /// Items created cannot be moved scaled or rotated
        /// </summary>
        /// <param name="center"></param>
        /// <param name="angle"></param>
        /// <returns>returns an Item at center orientation of angle</returns>
        private ScatterViewItem NonMovingItemFactory(Point center, double angle)
        {
            ScatterViewItem item = new ScatterViewItem();
            item.Center = center;
            item.CanMove = false;
            item.CanScale = false;
            item.CanRotate = false;
            item.Orientation = angle;
            return item;
        }
        /// <summary>
        /// Calls build menu for times createing a menu for each side of the surface
        /// </summary>
        public void MakeIndividualMenus()
        {
            IndividualMenu created;
             SurfaceButton button;
             ScatterViewItem item;

            //left
            created = BuildMenu(new Point(this.Width * 0.15, this.Height / 2), 90);
            SetUpHistory(leftHistory, created);
            MoveHistory(leftHistory, new Point(this.Width * 0.15-0.5*MenuTileSize, this.Height * .5 - 0.5 * (created.Count() * MenuTileSize)), 90);
            button = new SurfaceButton();
            item = NonMovingItemFactory(new Point(this.Width * 0.15 -  MenuTileSize, this.Height * .5 + 0.5* (created.Count() * MenuTileSize)), 90);
            button.Background = Brushes.Transparent;
            button.Content = "History";
            item.Content = button;
            item.Background = Brushes.Transparent;
            item.PreviewTouchUp += new EventHandler<TouchEventArgs>(ShowHistoryLeft);
            scatter.Items.Add(item);

            //top
            created=BuildMenu(new Point(this.Width / 2, this.Height * 0.2), 180);
            SetUpHistory(topHistory, created);
            MoveHistory(topHistory, new Point(this.Width * 0.5 + 0.5 * (created.Count() * MenuTileSize), this.Height * 0.2 - 0.5 * MenuTileSize), 180);
            button = new SurfaceButton();
            item = NonMovingItemFactory(new Point(this.Width * 0.5 - 0.5 * (created.Count() * MenuTileSize), this.Height * 0.2 -  MenuTileSize), 180);
            button.Background = Brushes.Transparent;
            button.Content = "History";
            item.Content = button;
            item.Background = Brushes.Transparent;
            item.PreviewTouchUp += new EventHandler<TouchEventArgs>(ShowHistoryTop);
            scatter.Items.Add(item);

            //right
            created = BuildMenu(new Point(this.Width * 0.85, this.Height / 2), -90);
            SetUpHistory(rightHistory, created);
            MoveHistory(rightHistory, new Point(this.Width * 0.85 + 0.5 * MenuTileSize, this.Height * 0.5 + 0.5 * (created.Count() * MenuTileSize)), -90);
            button = new SurfaceButton();
            item = NonMovingItemFactory(new Point(this.Width * 0.85 + MenuTileSize, this.Height * 0.5 - 0.5 * (created.Count() * MenuTileSize)), -90);
            button.Background = Brushes.Transparent;
            button.Content = "History";
            item.Content = button;
            item.Background = Brushes.Transparent;
            item.PreviewTouchUp += new EventHandler<TouchEventArgs>(ShowHistoryRight);
            scatter.Items.Add(item);

            //bottom
            created = BuildMenu(new Point(this.Width *0.5, this.Height * 0.75), 0);
            SetUpHistory(bottomHistory, created);
            MoveHistory(bottomHistory, new Point(this.Width * 0.5 - 0.5 * (created.Count() * MenuTileSize), this.Height * 0.75 + 0.5 * MenuTileSize), 0);
            button = new SurfaceButton();
            item = NonMovingItemFactory(new Point(this.Width * 0.5 + 0.5 * (created.Count() * MenuTileSize), this.Height * 0.75 + MenuTileSize), 0);
            button.Background = Brushes.Transparent;
            button.Content = "History";
            item.Content=button;
            item.Background = Brushes.Transparent;
            item.PreviewTouchUp += new EventHandler<TouchEventArgs>(ShowHistoryBottom);
            scatter.Items.Add(item);
        }

       
        void ShowHistoryLeft(object sender, TouchEventArgs e)
        {
            if (leftHistory.Visibility == Visibility.Hidden)
                leftHistory.Visibility = Visibility.Visible;
            else
                leftHistory.Visibility = Visibility.Hidden;
        }
        void ShowHistoryTop(object sender, TouchEventArgs e)
        {
            if (topHistory.Visibility == Visibility.Hidden)
                topHistory.Visibility = Visibility.Visible;
            else
                topHistory.Visibility = Visibility.Hidden;
        }
        void ShowHistoryRight(object sender, TouchEventArgs e)
        {
            if (rightHistory.Visibility == Visibility.Hidden)
                rightHistory.Visibility = Visibility.Visible;
            else
                rightHistory.Visibility = Visibility.Hidden;
        }
        void ShowHistoryBottom(object sender, TouchEventArgs e)
        {
            if (bottomHistory.Visibility == Visibility.Hidden)
                bottomHistory.Visibility = Visibility.Visible;
            else
                bottomHistory.Visibility = Visibility.Hidden;
        }
        private void MoveHistory(LibraryBar container, Point place, double angle)
        {
            TransformGroup transform = new TransformGroup();
            transform.Children.Add(new ScaleTransform(0.45, 0.35));
            transform.Children.Add(new RotateTransform(angle)); 
            transform.Children.Add(new TranslateTransform(place.X,place.Y));
            container.RenderTransform = transform;
        }
        private void SetUpHistory(LibraryBar container,IndividualMenu menu){
            System.Windows.Data.Binding bind = new System.Windows.Data.Binding("History");
            bind.Source = menu;
            container.SetBinding(LibraryBar.ItemsSourceProperty, bind);
        }

        /// <summary>
        /// Creates a treeMap and places it in a scatterview item in the scatterview scatter
        /// the treemap will be facing the user
        /// </summary>
        /// <param name="fileName">the path to follow to the xml document to read</param>
        /// <param name="caller">the square linked with the button that made the event</param>
        /// <param name="angle">angle of the finger that raised the event</param>
        /// <param name="parent">the location of the button that raised the event</param>
        public void PlaceTreeMap(String fileName,Square caller,double angle,Point parent)
        {

            TreeMenu map = new TreeMenu(Catagory.ReadFile("Information/" + fileName),caller,MenuList);


            map.AddUpListenerToButtons(TouchUpMenu);
            
            ScatterViewItem item = new ScatterViewItem();
            item.Center = findPosition( parent,  angle);
            item.Orientation = angle + 90;
          
            item.ContainerManipulationDelta +=new ContainerManipulationDeltaEventHandler(OnManipulation);

            item.Width = treeWidth;
            item.Height = treeHeight;
            item.Content = map.DrawMenu();
            scatter.Items.Add(item);
            MenuList.Add(map);

            ((IndividualMenu)FindTheMenu(caller.Button)).AddToHistory(map);
        }
        
        /// <summary>
        /// Event Handler finds and executes the correct response to menu Surfacebutton that 
        /// raised an event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TouchUpMenu(object sender, TouchEventArgs e)
        {
            
            if (sender is SurfaceButton)
            {
                SurfaceButton button = (SurfaceButton)sender;
                Menu.Menu menu = FindTheMenu(button);
                Log.Interaction(this,e.TouchDevice, menu.Get((SurfaceButton)sender),menu);
                if (menu == null)
                {
                    ScatterViewItem item = new ScatterViewItem();
                    item.Content = "NULL";
                    scatter.Items.Add(item);
                    return;
                }
                menu.interactive = true;
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
                            SquareList list = new SquareList();
                            Square sqr = new Square(1, new FillInfo(((GradientBrush)map.Get(button).Fill.Brush).GradientStops[0].Color, map.Get(button).Explanation, ((SolidColorBrush)map.Get(button).Fill.TextColor).Color));
                            sqr.ratio = 1;
                            list.Add(sqr);
                            map.addChild(list,map.Get(button));
                        }
                    }
                }
                else//non tree menu
                {
                    if (((IndividualMenu)menu).IsAnimating())
                    {
                        ((IndividualMenu)menu).StopAnimation();
                    }
                    else
                    if (MenuList.Count < 15)//maybe remove the first ones opened
                    {
                        TouchDevice c = (TouchDevice)e.TouchDevice;
                        PlaceTreeMap(menu.FileToOpen(button), menu.Get(button), c.GetOrientation(this), c.GetPosition(this));
                       
                    }
                }
            }
        }
        /// <summary>
        /// returns the Menu that contains the surfaceButton
        /// if the menu is a treemenu returns the top most TreeMenu
        /// </summary>
        /// <param name="path">button in a menu</param>
        /// <returns></returns>
        public Menu.Menu FindTheMenu(SurfaceButton button)
        {
            foreach (Menu.Menu menu in MenuList)
            {
                if (menu.ContainsButton(button))
                {
                    return menu;
                }
            }
            return null;
        }
        /// <summary>
        /// returns the menus that contains the canvas
        /// if the menu is a TreeMenu returns the highest possible parent of the TreeMenu
        /// </summary>
        /// <param name="canvas">Canvas beloning to a menu</param>
        /// <returns></returns>
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
        /// <summary>
        /// Called when a ScatterViewItem is resized or moved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnManipulation(object sender, ContainerManipulationDeltaEventArgs e)
        {
            ScatterViewItem item = (ScatterViewItem)sender;
            
            if (item.Center.X <= -item.Width * 0.10 || item.Center.Y <= -item.Height * 0.10 || item.Center.Y >= this.Height + item.Height * 0.10 || item.Center.X >= this.Width + item.Width * 0.10)
            {
                Menu.Menu menu = FindTheMenu((Canvas)(item.Content));
                if (menu != null)
                {
                    ((TreeMenu)menu).StopTimer();
                    MenuList.Remove(menu);
                    scatter.Items.Remove(item);
                    Log.Deleted(DeletionMethod.Swipe, menu);
                }
            }
            if (e.ScaleFactor != 1)
            {

                Menu.Menu menu = FindTheMenu((Canvas)(item.Content));
                ((TreeMenu)menu).ReDraw(item.Width, item.Height);
                ((TreeMenu)menu).SizeCrumbs();
                Log.Resized(menu, e.ScaleFactor);
            }
        }
       /// <summary>
       /// Finds a good position to open a new tree menu
       /// based on the position and angle of a finger
       /// that requested a treemenu be created
       /// </summary>
       /// <param name="parent">location of finger in scatterview</param>
       /// <param name="angle">angle of finger in scatterview</param>
       /// <returns></returns>
        private Point findPosition(Point parent, double angle)
        {
            Point center = new Point();
            bool left, top, right, bottom;
            double smallAngle = 0.0, distance, slope, crossingX, crossingY, placement;
            double ratioBetween = 0.45;
            center.X = 300;
            center.Y = 300;
            slope = Math.Tan(angle * 3.14 / 180);

            //True if the y line is within the window
            //at left of screen
            placement = (slope * (0 - parent.X) + parent.Y);
            left = placement > 0 && placement < this.Height;

            //at right of screen
            placement = slope * (this.Width - parent.X) + parent.Y;
            right = placement > 0 && placement < this.Height;

            //true if the x value line is within the window
            //at top of screen
            placement = (0 - parent.Y) / slope + parent.X;
            top = placement > 0 && placement < this.Width;

            //at bottom of screen
            placement = (this.Height - parent.Y) / slope + parent.X;
            bottom = placement > 0 && placement < this.Width;




            if (top && angle > 0 && angle < 180)
            {

                crossingX = (0 - parent.Y) / slope + parent.X;
                crossingY = 0;
                distance = Math.Sqrt(Math.Pow(parent.Y - crossingY, 2) + Math.Pow(parent.X - crossingX, 2));
                smallAngle = 90 - angle;
                center.X = parent.X - Math.Sin(smallAngle * 3.14 / 180) * distance * ratioBetween;
                center.Y = parent.Y - Math.Cos(smallAngle * 3.14 / 180) * distance * ratioBetween;

            }
            else if (right && angle > 90 && angle < 270)
            {
                crossingX = this.Width;
                crossingY = slope * (this.Width - parent.X) + parent.Y;
                distance = Math.Sqrt(Math.Pow(parent.Y - crossingY, 2) + Math.Pow(parent.X - crossingX, 2));
                smallAngle = 180 - angle;
                center.Y = parent.Y - Math.Sin(smallAngle * 3.14 / 180) * distance * ratioBetween;
                center.X = parent.X + Math.Cos(smallAngle * 3.14 / 180) * distance * ratioBetween;

            }
            else if (bottom && angle > 180 && angle < 360)
            {
                crossingX = (this.Height - parent.Y) / slope + parent.X;
                crossingY = this.Height;
                distance = Math.Sqrt(Math.Pow(parent.Y - crossingY, 2) + Math.Pow(parent.X - crossingX, 2));
                smallAngle = 270 - angle;
                center.X = parent.X + Math.Sin(smallAngle * 3.14 / 180) * distance * ratioBetween;
                center.Y = parent.Y + Math.Cos(smallAngle * 3.14 / 180) * distance * ratioBetween;

            }
            else if (left && (angle < 90 || angle > 270))
            {
                crossingX = 0;
                crossingY = slope * (0 - parent.X) + parent.Y;
                distance = Math.Sqrt(Math.Pow(parent.Y - crossingY, 2) + Math.Pow(parent.X - crossingX, 2));

                if (angle > 270)
                {
                    smallAngle = angle - 360;
                }
                else
                {
                    smallAngle = angle;
                }
                center.Y = parent.Y - Math.Sin(smallAngle * 3.14 / 180) * distance * ratioBetween;
                center.X = parent.X - Math.Cos(smallAngle * 3.14 / 180) * distance * ratioBetween;

            }

            if (center.X < treeHeight*0.5)
            {
                center.X = treeHeight*0.5;
            }
            else if (center.X > this.Width-treeHeight*0.5)
            {
                center.X = this.Width - treeHeight*0.5;
            }

            if (center.Y < treeWidth*0.45)
            {
                center.Y = treeWidth*0.45;
            }
            else if (center.Y > this.Height-treeWidth*0.45)
            {
                center.Y = this.Height - treeWidth*0.45;
            }
            return center;
        }
        
        private void DragCompletedLeft(object sender, SurfaceDragCompletedEventArgs e)
        {
            if (e.Cursor.CurrentTarget != leftHistory && e.Cursor.Effects == System.Windows.DragDropEffects.Move)
            {
                Menu.Menu removed = e.Cursor.Data as Menu.Menu;
                ((IndividualMenu)MenuList[0]).History.Move(((IndividualMenu)MenuList[0]).History.IndexOf(removed), 0);
                e.Handled = true;
            }
        }
       
        private void DragCompletedTop(object sender, SurfaceDragCompletedEventArgs e)
        {
            if (e.Cursor.CurrentTarget != topHistory && e.Cursor.Effects == System.Windows.DragDropEffects.Move)
            {
                Menu.Menu removed = e.Cursor.Data as Menu.Menu;
                ((IndividualMenu)MenuList[1]).History.Move(((IndividualMenu)MenuList[1]).History.IndexOf(removed), 0);
                e.Handled = true;
            }
        }
      
        private void DragCompletedRight(object sender, SurfaceDragCompletedEventArgs e)
        {
            if (e.Cursor.CurrentTarget != rightHistory && e.Cursor.Effects == System.Windows.DragDropEffects.Move)
            {
                Menu.Menu removed = e.Cursor.Data as Menu.Menu;
                ((IndividualMenu)MenuList[2]).History.Move(((IndividualMenu)MenuList[2]).History.IndexOf(removed), 0);
                e.Handled = true;
            }
        }
     

        private void DragCompletedBottom(object sender, SurfaceDragCompletedEventArgs e)
        {
            if (e.Cursor.CurrentTarget != bottomHistory && e.Cursor.Effects == System.Windows.DragDropEffects.Move)
            {
                Menu.Menu removed = e.Cursor.Data as Menu.Menu;
                ((IndividualMenu)MenuList[3]).History.Move(((IndividualMenu)MenuList[3]).History.IndexOf(removed), 0);
                e.Handled = true;
            }
        }
        
        private void Scatter_Drop(object sender, SurfaceDragDropEventArgs e)
        {
            
                TreeMenu old = e.Cursor.Data as TreeMenu;
                TreeMenu map = old.Clone();
                map.AddUpListenerToButtons(TouchUpMenu);

                ScatterViewItem item = new ScatterViewItem();
                item.Center = e.Cursor.GetPosition(scatter);
                item.Orientation = e.Cursor.GetOrientation(scatter);

                item.ContainerManipulationDelta += new ContainerManipulationDeltaEventHandler(OnManipulation);

                item.Width = treeWidth;
                item.Height = treeHeight;
                item.Content = map.DrawMenu();
                map.ReDraw(item.Width, item.Height);
                scatter.Items.Add(item);
                MenuList.Add(map);
        }
    }
}
