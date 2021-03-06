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
using PrototypeOne.XmlFiles;

using System.Windows.Media.Animation;
namespace PrototypeOne
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {

        public static double treeHeight = 320;
        public static double treeWidth = 450;
        public static double treeArea = treeWidth * treeHeight;
        public static double MenuTileSize = 85;
        public static int MaxMenus = 15;
        public static int MaxHistorySize = 9;

        /// menu sizes
        public static double MaxWidthItem = 560;
        public static double MaxHeightItem = 400;
        
        public static double MinWidthItem = 450;
        public static double MinHeightItem = 320;
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

            //CreateXAML();
           

            MenuList = new List<Menu.Menu>(MaxMenus);
            MakeIndividualMenus();
            DataContext = this;
            BeginStoryboard(Resources["Rotate"] as Storyboard);
            
            
            
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

            first.Video = @"C:\Users\Public\Videos\Sample Videos\Wildlife.wmv";
            first.Image = "X.jpg";
            first.Explanation = "long string with\n to make sence i should say newline\nLol\n";
            first.SubCatagoryFile = "Top";
            first.BackGroundImage = @"bond.jpg";
          
            first.ImageSetup = new ImageInformation();
            second.BackGroundColor = Colors.AntiqueWhite;
            second.TextColor = Colors.Black;
            second.Ratio = 0.55;
            second.Title = "SecondTitle";
            
          
            saveList.Add(first);
            saveList.Add(second);
            
            Catagory.WriteFile(saveList, "Resources/Information/Example.xml");

        }

        /// <summary>
        /// Creates one instance of the top most menu 
        /// </summary>
        /// <param name="center">The Menus center</param>
        /// <param name="angle">The angle of the menu</param>
        public IndividualMenu BuildMenu(Point center, double angle)
        {
            ScatterViewItem item = NonMovingItemFactory(center, angle);
           
            IndividualMenu sideMenu = new Menu.IndividualMenu(Catagory.ReadFile("Resources/Information/Top.xml"), AddAnimation(item));
            MenuList.Add(sideMenu);

            TextBlock block = new TextBlock();
            block.Text="Touch Here to Begin";
            block.FontSize = 35;
            ScatterViewItem touchme = NonMovingItemFactory(center, angle);
            touchme.Content = block;
            touchme.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            touchme.Focusable = false;
            touchme.Width = sideMenu.Width;
            touchme.Height = sideMenu.Height;
            scatter.Items.Add(touchme);
            Canvas canvas = sideMenu.DrawMenu();
            
            item.Content = canvas;
            item.Background = Brushes.Transparent;
            scatter.Items.Add(item);

            ElementMenu historyMenu = new ElementMenu();
            TransformGroup transform = new TransformGroup();
            transform.Children.Add(new ScaleTransform(-1, -1));
            transform.Children.Add(new TranslateTransform(sideMenu.Width * 0.5 + 1.7 * MenuTileSize, 2.2 * MenuTileSize));
            historyMenu.RenderTransform = transform;
            historyMenu.ActivationMode = ElementMenuActivationMode.AlwaysActive;
            sideMenu.AddUpListenerToButtons(new EventHandler<TouchEventArgs>(TouchUpMenu));
            canvas.Children.Insert(0, historyMenu);

            return sideMenu;
        }

        /// <summary>
        /// adds a looping opacity animation to item
        /// </summary>
        /// <param name="item"></param>
        private Storyboard AddAnimation(ScatterViewItem item)
        {

            DoubleAnimation Animation = new DoubleAnimation();
            Animation.From = 1.0;
            Animation.To = 0.02;
            //Duration needs to divide the timer length(set in Menu.Menu) evenly for the animation to be smooth
            Animation.Duration = new Duration(TimeSpan.FromSeconds(7.5));
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


            //left
            created = BuildMenu(new Point(this.Width * 0.08, this.Height / 2), 90);
            SetUpHistory(leftHistory, created);
            MoveHistory(leftHistory, new Point(this.Width * 0.08 - 0.5 * created.Height, this.Height * .5 - 0.495 * created.Width), 90);
            created.DrawMenu().Children[0].PreviewTouchDown += new EventHandler<TouchEventArgs>(ShowHistoryLeft);

            //top
            created = BuildMenu(new Point(this.Width / 2, this.Height * 0.155), 180);
            SetUpHistory(topHistory, created);
            MoveHistory(topHistory, new Point(this.Width * 0.5 + 0.495 * created.Width, this.Height * 0.155 - 0.5 *created.Height), 180);
            created.DrawMenu().Children[0].PreviewTouchDown += new EventHandler<TouchEventArgs>(ShowHistoryTop);

            //right
            created = BuildMenu(new Point(this.Width * 0.92, this.Height / 2), -90);
            SetUpHistory(rightHistory, created);
            MoveHistory(rightHistory, new Point(this.Width * 0.92 + 0.5 * created.Height, this.Height * 0.5 + 0.495 * created.Width), -90);
            created.DrawMenu().Children[0].PreviewTouchDown += new EventHandler<TouchEventArgs>(ShowHistoryRight);


            //bottom
            created = BuildMenu(new Point(this.Width * 0.5, this.Height * 0.85), 0);
            SetUpHistory(bottomHistory, created);
            MoveHistory(bottomHistory, new Point(this.Width * 0.5 - 0.495 * created.Width, this.Height * 0.85 + 0.5 *created.Height), 0);
            created.DrawMenu().Children[0].PreviewTouchDown += new EventHandler<TouchEventArgs>(ShowHistoryBottom);
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
        private void MoveHistory(SurfaceListBox container, Point place, double angle)
        {
            TransformGroup transform = new TransformGroup();
            transform.Children.Add(new ScaleTransform(0.45, 0.35));
            transform.Children.Add(new RotateTransform(angle));
            transform.Children.Add(new TranslateTransform(place.X, place.Y));
            container.ToolTip = "History";
            container.RenderTransform = transform;
        }
        /// <summary>
        /// binds the history information to the appropriate menu
        /// </summary>
        /// <param name="container"></param>
        /// <param name="menu"></param>
        private void SetUpHistory(SurfaceListBox container, IndividualMenu menu)
        {
            //2.22222 is the inverse of the scaling factor used in moveHistory 35 was picked for looks so the menu doesn't over lap the ItemMenu
            container.MaxWidth = menu.Width * 2.2222- 35;
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
        public void PlaceTreeMap(String fileName, Square caller, double angle, Point parent)
        {

            TreeMenu map = new TreeMenu(Catagory.ReadFile( fileName), caller, MenuList);


            map.AddUpListenerToButtons(TouchUpMenu);

            ScatterViewItem item = new ScatterViewItem();
            item.Center = findPosition(parent, angle);
            item.Orientation = angle + 90;
            item.MinHeight = MinHeightItem;
            item.MinWidth = MinWidthItem;
            item.MaxHeight = MaxHeightItem;
            item.MaxWidth = MaxWidthItem;
            item.ContainerManipulationDelta += new ContainerManipulationDeltaEventHandler(OnManipulation);
            item.ContainerManipulationCompleted += new ContainerManipulationCompletedEventHandler(AfterManipulationCompleted);
            item.Width = treeWidth;
            item.Height = treeHeight;
            item.Content = map.DrawMenu();
            item.ClipToBounds = false;
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

            if (sender is SurfaceButton && e.TouchDevice.GetIsFingerRecognized())
            {
                SurfaceButton button = (SurfaceButton)sender;
                Menu.Menu menu = FindTheMenu(button);

                if (menu == null)
                {
                    return;
                }
                Log.Interaction(this, e.TouchDevice, menu.Get((SurfaceButton)sender), menu);
                menu.interactive = true;
                string file = menu.FileToOpen(button);
                if (menu is TreeMenu)
                {
                    TreeMenu map = (TreeMenu)menu;
                    if (file != null && !file.Equals(""))
                    {
                        TreeMenu childAdded = map.addChild(Catagory.ReadFile( file), map.Get(button));
                        childAdded.AddUpListenerToButtons(TouchUpMenu);
                    }
                    else
                    {
                        if (map.HasExplanation(button))
                        {
                            /*
                             * Creates a new TreeMenu from the information stored in the Square
                             * selected
                             */
                            SquareList list = new SquareList();
                            Square sqr = new Square(1, map.Get(button).Explanation);
                            if (map.Get(button).VideoString != null)
                            {
                                sqr.VideoString = map.Get(button).VideoString;
                            }
                            if (map.Get(button).ImageString != null)
                            {
                                sqr.ImageString = map.Get(button).ImageString;
                            }
                         
                            sqr.setBackGround(map.Get(button).BackGroundColor);
                            sqr.setTextColor((map.Get(button).TextColor));
                            sqr.singleImage = map.Get(button).singleImage;
                            sqr.Slides = map.Get(button).Slides;
                            sqr.Ratio = 1;
                            list.Add(sqr);
                            map.addChild(list, map.Get(button));
                        }
                       
                    }
                    map.ReDraw();
                }
                else//non tree menu
                {
                    if (((IndividualMenu)menu).IsAnimating())
                    {
                        ((IndividualMenu)menu).StopAnimation();
                    }
                    else
                    {
                        if (MenuList.Count < MaxMenus)
                        {
                            TouchDevice c = (TouchDevice)e.TouchDevice;
                            PlaceTreeMap(menu.FileToOpen(button), menu.Get(button), c.GetOrientation(this), c.GetPosition(this));

                        }
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
           

            Menu.Menu menu = FindTheMenu((Canvas)(item.Content));
            if (menu == null)
            {
                return;
            }
            //if the menu has been moved to the edge of the screen
            if (item.Center.X <= -item.Width * 0.10 || item.Center.Y <= -item.Height * 0.10 || item.Center.Y >= this.Height + item.Height * 0.10 || item.Center.X >= this.Width + item.Width * 0.10)
            {

                if (menu != null)
                {
                    ((TreeMenu)menu).StopTimer();
                    MenuList.Remove(menu);
                    scatter.Items.Remove(item);
                    
                    Log.Deleted(DeletionMethod.Swipe, menu);
                    ((TreeMenu)menu).Delete();
                }
            }
            else
            {
                if (e.ScaleFactor != 1)
                {
                    ((TreeMenu)menu).ReDraw(item.Width, item.Height);
                    ((TreeMenu)menu).SizeCrumbs();
                }
                menu.interactive = true;
            }
        }
        /// <summary>
        /// Used for logging the new Height Width X and Y After Manipulation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AfterManipulationCompleted(object sender, ContainerManipulationCompletedEventArgs e)
        {
            ScatterViewItem item = (ScatterViewItem)sender;
            Menu.Menu menu = FindTheMenu((Canvas)(item.Content));
            if (menu == null)
            {
                return;
            }
            if (e.ScaleFactor != 1)
            {
                Log.Resized(menu, item.Width, item.Height);
            }
            Log.Moved(item.Center, item.Orientation, menu);
        }

        /// <summary>
        /// Finds a good position to open a new tree menu
        /// based on the position and angle of a finger
        /// that requested a treemenu be created
        /// </summary>
        /// <param name="parent">location of finger in scatterview</param>
        /// <param name="angle">angle of finger in scatterview degrees</param>
        /// <returns></returns>
        private Point findPosition(Point parent, double angle)
        {
            Point center = new Point();
            bool left, top, right, bottom;
            double smallAngle = 0.0, distance, slope, crossingX, crossingY, placement;
            double ratioBetween = 0.2;
            while (angle > 360) angle -= 360;
            while (angle < 0) angle += 360;
            center.X = 100;// parent.X;
            center.Y = 100;// parent.Y;
            slope = Math.Tan(angle * 3.14 / 180);

            //useing the formula (y2-y1)=(x2-x1)m

            
            //y2=m(x2-x1)+y1
            placement = (slope * (0 - parent.X) + parent.Y);
            left = placement > 0 && placement < this.Height;

            //at right of screen
            //y2=m(x2-x1)+y1
            placement = slope * (this.Width - parent.X) + parent.Y;
            right = placement > 0 && placement < this.Height;

            //true if the x value line is within the window
            //at top of screen
            //x2=(y2-y1)/m+x1
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

            if (center.X < treeWidth * 0.52)
            {
                center.X = treeWidth * 0.5;
            }
            else if (center.X > this.Width - treeWidth * 0.52)
            {
                center.X = this.Width - treeWidth * 0.52;
            }

            if (center.Y < treeWidth * 0.55)
            {
                center.Y = treeWidth * 0.55;
            }
            else if (center.Y > this.Height - treeWidth * 0.55)
            {
                center.Y = this.Height - treeWidth * 0.55;
            }
            return center;
        }


        /// <summary>
        /// Removes the dragged item from the associated collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragCompleted(object sender, SurfaceDragCompletedEventArgs e)
        {
            Menu.Menu removed = e.Cursor.Data as Menu.Menu;
            SurfaceListBox bar = e.Cursor.DragSource as SurfaceListBox;
            if (e.Cursor.CurrentTarget != bar)
            {
                ObservableCollection<Menu.Menu> collection = bar.GetValue(SurfaceListBox.ItemsSourceProperty) as ObservableCollection<Menu.Menu>;
                int index = collection.IndexOf(removed);
                if (index < 0 || index>collection.Count)
                {
                    return;
                }
                collection.Remove(removed);
                if (MenuList.Count >= MaxMenus)
                {
                    //puts the removed Menu back into the history 
                    //because it will not be accepted in the 
                    //scatterview
                    collection.Insert(index, removed);
                }
                e.Handled = true;
            }
        }
        /// <summary>
        /// Adds the dragged map to the scatter view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scatter_Drop(object sender, SurfaceDragDropEventArgs e)
        {
            if (MenuList.Count < MaxMenus)
            {
                TreeMenu old = e.Cursor.Data as TreeMenu;
                TreeMenu map = old.Clone();
                map.AddUpListenerToButtons(TouchUpMenu);
                ScatterViewItem item = new ScatterViewItem();
                item.Center = findPosition(e.Cursor.GetPosition(this), e.Cursor.GetOrientation(scatter) - 90);                
                item.Orientation = e.Cursor.GetOrientation(scatter);
                item.MinHeight = MinHeightItem;
                item.MinWidth = MinWidthItem;
                item.MaxHeight = MaxHeightItem;
                item.MaxWidth = MaxWidthItem;
                item.ContainerManipulationDelta += new ContainerManipulationDeltaEventHandler(OnManipulation);
                item.ContainerManipulationCompleted += new ContainerManipulationCompletedEventHandler(AfterManipulationCompleted);
                item.Width = treeWidth;
                item.Height = treeHeight;
                item.Content = map.DrawMenu();
                map.ReDraw(item.Width, item.Height);
                scatter.Items.Add(item);
                MenuList.Add(map);
                SurfaceListBox origin = e.Cursor.DragSource as SurfaceListBox;
                ObservableCollection<Menu.Menu> collection = origin.GetValue(SurfaceListBox.ItemsSourceProperty) as ObservableCollection<Menu.Menu>;
                collection.Insert(0, map);
                Log.FromHistory(map);

                //remove oldest item
                if (collection.Count > MaxHistorySize)
                {
                    TreeMenu removed=(TreeMenu)collection[collection.Count - 1];
                    collection.RemoveAt(collection.Count - 1);
                    removed.Delete();
                }
                
            }
        }

        
        /// <summary>
        /// sets up the information needed for the drag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                FrameworkElement findSource = e.OriginalSource as FrameworkElement;
                SurfaceListBoxItem draggedElement = null;

                // Find the SurfaceListBoxItem object that is being touched.
                while (draggedElement == null && findSource != null)
                {
                    if ((draggedElement = findSource as SurfaceListBoxItem) == null)
                    {
                        findSource = VisualTreeHelper.GetParent(findSource) as FrameworkElement;
                    }
                }
                if (draggedElement == null)
                {
                    return;
                }

                //PhotoData data = draggedElement.Content as PhotoData;

                // Create the cursor visual
                ContentControl cursorVisual = new ContentControl()
                {
                    Content = draggedElement.DataContext,
                    Style = FindResource("CursorStyle") as Style
                };

                // Create a list of input devices. Add the touches that
                // are currently captured within the dragged element and
                // the current touch (if it isn't already in the list).
                List<InputDevice> devices = new List<InputDevice>();
                devices.Add(e.TouchDevice);
                foreach (TouchDevice touch in draggedElement.TouchesCapturedWithin)
                {
                    if (touch != e.TouchDevice)
                    {
                        devices.Add(touch);
                    }
                }

                // Get the drag source object
                ItemsControl dragSource = ItemsControl.ItemsControlFromItemContainer(draggedElement);

                SurfaceDragDrop.BeginDragDrop(
                    dragSource,
                    draggedElement,
                    cursorVisual,
                    draggedElement.DataContext,
                    devices,
                    System.Windows.DragDropEffects.Move);

                // Prevents the default touch behavior from happening and disrupting our code.
                e.Handled = true;

                // Gray out the SurfaceListBoxItem for now. We will remove it if the DragDrop is successful.
                draggedElement.Opacity = 0.5;
            }
        }
    }

   
}
