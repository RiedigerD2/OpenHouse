using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Collections;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Xna.Framework.Input.Touch;
using System.Windows.Forms;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using PrototypeOne;
using System.ComponentModel;

namespace PrototypeOne
{
    public class TreeMenu : Menu.Menu,INotifyPropertyChanged
    {
        private double height, width;
        private Canvas canvas;
        TreeMenu child;
        private SurfaceButton Exit;
        List<Menu.Menu> ParentList;
        private List<SurfaceButton> breadCrumbs;
        DrawingImage image;
         public event PropertyChangedEventHandler PropertyChanged;
    
        /// <summary>
        /// gives image to use for the histroy
        /// </summary>
        public ImageSource Source
        {
            get
            {
                if (child != null)
                {
                    return child.Source;
                }
                else
                {
                    DrawingGroup squares = new DrawingGroup();
                    foreach (Square block in children)
                    {
                        System.Windows.Rect rect = new System.Windows.Rect(block.X, block.Y, block.Width, block.Height);
                        RectangleGeometry rectangle = new RectangleGeometry(rect);
                        GeometryDrawing drawing = new GeometryDrawing();

                        drawing.Brush = block.BackGroundBrush;
                        Pen mypen = new Pen(block.BackGroundBrush, 0.1);
                        drawing.Pen = mypen;
                        drawing.Geometry = rectangle;

                        squares.Children.Add(drawing);

                    }
                    image = new DrawingImage(squares);
                    return image;
                }
            }
        }
        public string Name
        {
            get
            {
                string list = "";
                for (int i = 0; i < breadCrumbs.Count; i++)
                {
                    if (i == breadCrumbs.Count - 1)
                        list += (breadCrumbs[i].Content);
                    else
                        list += (breadCrumbs[i].Content) + "/";
                }
                return  list;
            }
        }
        /// <summary>
        /// returns the canvas that the menu is drawn on
        /// </summary>
        /// <returns></returns>
        public override Canvas DrawMenu()
        {
            return canvas;
        }
        /// <summary>
        /// returns a new canvas that the menu is drawn on
        /// because the same canvas cannot be added to the window twice
        /// </summary>
        /// <returns></returns>
        public  Canvas DrawNewMenu()
        {
            canvas = null;
            FillDrawing();
            return DrawMenu();
        }
        /// <summary>
        /// Creates new treeMenu
        /// </summary>
        /// <param name="children">list used to populate the menu</param>
        /// <param name="creator">the square that is linked with the button that created the treemenu</param>
        /// <param name="ParentList">the list of menus this treemenu will be placed in</param>
        public TreeMenu(SquareList children, Square creator, List<Menu.Menu> ParentList)
            : base(children)
        {
            breadCrumbs = new List<SurfaceButton>();
            this.height = SurfaceWindow1.treeHeight;
            this.width = SurfaceWindow1.treeWidth;
            this.canvas = new Canvas();
            children.ResizeAreas((height-0.2*height) * width);
            MakeTree(0, width, height-0.2*height);

            this.ParentList = ParentList;

            SurfaceButton crumb = new SurfaceButton();
            crumb.Height = 0.2 * height;
            crumb.Background = creator.BackGroundBrush;
            crumb.Foreground = creator.TextBrush;
            crumb.Content = creator.Name;
            crumb.PreviewTouchUp += new EventHandler<TouchEventArgs>(RetraceToBreadCrumb);
            breadCrumbs.Add(crumb);
            canvas.Children.Add(crumb);

            Exit = new SurfaceButton();
            try
            {//this file is in bin/ debug or release directories
                ImageBrush buttonBrush = new ImageBrush(new BitmapImage(new Uri("Resources/X.jpg", UriKind.Relative)));
                Exit.Background = buttonBrush;
            }
            catch (Exception e)
            {
                Exit.Content = e.Message;
            }
            Exit.PreviewTouchUp += new EventHandler<TouchEventArgs>(ExitUp);

            canvas.Children.Add(Exit);
            SizeCrumbs();
            myTimer = new Timer();
            myTimer.Tick += new EventHandler(myTimer_Tick);
            myTimer.Interval = 20000;
            myTimer.Enabled = true;
            
            FillDrawing();
        }
        /// <summary>
        /// used in the cloning process
        /// </summary>
        /// <param name="children">children that are to be cloned</param>
        /// <param name="oldCrumbs">crumbs in the other treemenu</param>
        /// <param name="ParentList">the list of menus that this new menu will be added to</param>
        private TreeMenu(SquareList children, List<SurfaceButton> oldCrumbs, List<Menu.Menu> ParentList)
            : base(children)
        {
            breadCrumbs = new List<SurfaceButton>();
            this.children = SquareList.createSquareList(children);
            this.height = SurfaceWindow1.treeHeight;
            this.width = SurfaceWindow1.treeWidth;
            this.canvas = new Canvas();
            children.ResizeAreas((height - 0.2 * height) * width);
            MakeTree(0, width, height - 0.2 * height);

            this.ParentList = ParentList;

            foreach (SurfaceButton oldc in oldCrumbs)
            {
                SurfaceButton crumb = new SurfaceButton();
                crumb.Height = 0.2 * height;
                crumb.Background = oldc.Background;
                crumb.Content = oldc.Content.ToString();
                crumb.Foreground = oldc.Foreground;
                crumb.PreviewTouchUp += new EventHandler<TouchEventArgs>(RetraceToBreadCrumb);
                breadCrumbs.Add(crumb);
                canvas.Children.Add(crumb);
            }
            Exit = new SurfaceButton();
            try
            {//this file is in bin/ debug or release directories
                ImageBrush buttonBrush = new ImageBrush(new BitmapImage(new Uri("Resources/X.jpg", UriKind.Relative)));
                Exit.Background = buttonBrush;
            }
            catch (Exception e)
            {
                Exit.Content = e.Message;
            }

            Exit.PreviewTouchUp += new EventHandler<TouchEventArgs>(ExitUp);

            canvas.Children.Add(Exit);
            SizeCrumbs();
            myTimer = new Timer();
            myTimer.Tick += new EventHandler(myTimer_Tick);
            myTimer.Interval = 20000;
            myTimer.Enabled = true;
            
            FillDrawing();
        }
        /// <summary>
        /// Only to be used in the public addchild
        /// this is to ensure the breadcrumbs and parentlist is presevered
        /// </summary>
        /// <param name="children">list used to fill the menu</param>
        private TreeMenu(SquareList children)
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
        /// <summary>
        /// Adds a child to the menu if the menu has a child a
        /// recursively trys to add the child until a leaf menu is found
        /// </summary>
        /// <param name="grandChildren">list used to populate the menu</param>
        /// <param name="creator">square that wants to create a treemenu</param>
        /// <returns></returns>
        public TreeMenu addChild(SquareList grandChildren, Square creator)
        {
            if (child == null)
            {
                child = new TreeMenu(grandChildren);
                canvas.Children.Add(child.DrawMenu());
                child.ReDraw(width, height);

                SurfaceButton crumb = new SurfaceButton();
                crumb.Background = creator.BackGroundBrush;
                crumb.Foreground = creator.TextBrush;
                crumb.Content = creator.Name;
                crumb.PreviewTouchUp += new EventHandler<TouchEventArgs>(RetraceToBreadCrumb);
                breadCrumbs.Add(crumb);
                canvas.Children.Add(crumb);
                SizeCrumbs();
                NotifyChange("Source");
                NotifyChange("Name");
                return child;
            }
            else
            {
                SurfaceButton crumb = new SurfaceButton();
                crumb.Background = creator.BackGroundBrush;
                crumb.Foreground = creator.TextBrush;
                crumb.Content = creator.Name;
                crumb.PreviewTouchUp += new EventHandler<TouchEventArgs>(RetraceToBreadCrumb);
                breadCrumbs.Add(crumb);
                canvas.Children.Add(crumb);
                SizeCrumbs();
                
                TreeMenu grandchild= child.addChild(grandChildren);
                NotifyChange("Source");
                NotifyChange("Name");
                return grandchild;
            }
            
        } 
        /// <summary>
        /// Only to be used in the public addchild
        /// this is to ensure the breadcrumbs and parentlist is presevered
        /// </summary>
        /// <param name="grandChildren"></param>
        /// <returns></returns>
        private TreeMenu addChild(SquareList grandChildren)
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
        /// <summary>
        /// Uses recursion to create a squarified treeMenu
        /// changes the height width x and y information of the
        /// squares in the Squarelist "children" appropriatly
        /// </summary>
        /// <param name="used">number of children that have been prossed</param>
        /// <param name="widthLeft">horizontal space left to fill</param>
        /// <param name="heightLeft">vertical space left to fill</param>
        private void MakeTree(int used, double widthLeft, double heightLeft)
        {
            if (widthLeft <= 0 || heightLeft <= 0)
            {
                return;
            }

            if (used >= children.Count()-1)
            {
                children.SetSameHeight(used, 1, widthLeft);
                //need to double set last block to insure the actual width and height are correct
                //look at implementation of Square setting height and width
                children.SetSameHeight(used, 1, widthLeft);
                AddChildrenSide(widthLeft, heightLeft, used, 1);
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


                AddChildrenSide(widthLeft, heightLeft, used, divisions - 1);
               
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
                AddChildrenTop(widthLeft, height - heightLeft, used, divisions - 1);
                MakeTree(used + divisions - 1, widthLeft, heightLeft - children.Get(used).Height);
            }
        }
        /// <summary>
        /// Positions count number of children from
        /// the start location to the side of the map
        /// </summary>
        /// <param name="curWidth">width of the treeMenu</param>
        /// <param name="curHeight">Height of the treeMenu</param>
        /// <param name="start">distance in children to start</param>
        /// <param name="count">number of children to place on the side</param>
        private void AddChildrenSide(double curWidth, double curHeight, int start, int count)
        {
            double heightUsed = 0;
            for (int i = 0; i < count; i++)
            {
                children.Get(start + i).X = curWidth - children.Get(start + i).Width;
                children.Get(start + i).Y = height-curHeight +heightUsed;
                heightUsed += children.Get(start + i).Height;
               
            }
        }
        /// <summary>
        /// Positions count number of children from
        /// the start location to the top of the map
        /// </summary>
        /// <param name="curWidth">width of the treeMenu</param>
        /// <param name="curHeight">Height of the treeMenu</param>
        /// <param name="start">distance in children to start</param>
        /// <param name="count">number of children to place on the top</param>
        private void AddChildrenTop(double curWidth, double curHeight, int start, int count)
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
        /// <summary>
        /// Once the children have been set up useing makeTree
        /// fills the Menus canvas with buttons to correspond to the sizes of the Squares
        /// </summary>
        private void FillDrawing()
        {
            if (canvas == null)
            {
                canvas = new Canvas();
            }
            foreach (Square block in children)
            {
               SurfaceButton button = new SurfaceButton();
                button.Background = block.BackGroundBrush;
                button.Height = block.Height;
                button.Width = block.Width;
                button.RenderTransform = new TranslateTransform(block.X, block.Y);
                button.Foreground = block.TextBrush;
                button.BorderBrush = Brushes.Black;
                button.BorderThickness = new Thickness(20);
                block.Button = button;

                canvas.Children.Add(button);
                
                if (children.Count() == 1)
                {
                    canvas.Children.Add(block.GetTextBlockTop());
                }
                else
                {
                    canvas.Children.Add(block.GetTextBlock());
                }
               
            }
        }
        /// <summary>
        /// Called after calls MakeTree
        /// if each square in Children already has a button associated with it
        /// </summary>
        private void RepositionButtons()
        {

            foreach (Square block in children)
            {
                
                block.Button.Height = block.Height;
                block.Button.Width = block.Width;
                block.Button.RenderTransform = new TranslateTransform(block.X, block.Y);

                if (children.Count() == 1)
                {
                    canvas.Children.Add(block.GetTextBlockTop());
                }
                else
                {
                    canvas.Children.Add(block.GetTextBlock());
                }
            }
        }
       /// <summary>
       /// Whenever a TreeMenu is resized used to 
       /// replace and resize all buttons
       /// </summary>
       /// <param name="width">new width to be drawn at</param>
       /// <param name="height">new height to be drawn at</param>
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
        /// <summary>
        /// removes anyText block associated with a square
        /// from the canvas.
        /// used to layer the canvas properly
        /// </summary>
        private void removeTextBlocksFromCanvas()
        {

            foreach (Square sqr in children)
            {
                canvas.Children.Remove(sqr.GetTextBlock());
            }
        
        }
        /// <summary>
        /// sizes the bread crumbs to the availablity  space at the top of the 
        /// treeMenu
        /// </summary>
        public void SizeCrumbs(){

            for (int i=0; i < breadCrumbs.Count; i++)
            {
                SurfaceButton button = breadCrumbs[i];
                button.Width = (width-0.2*height)/breadCrumbs.Count;
                button.Height = 0.2 * height;
                button.RenderTransform = new TranslateTransform((width-0.2*height) / breadCrumbs.Count * i, 0);
            }
            Exit.Width =  0.2 * height;
            Exit.Height = 0.2 * height;
            Exit.RenderTransform = new TranslateTransform(width - 0.2 * height, 0);
        }
        /// <summary>
        /// returns  the square that exists in children
        /// or in any of its sub nodes' children
        /// that contains the button sqr
        /// </summary>
        /// <param name="sqr">button used to find a square</param>
        /// <returns></returns>
        public override Square Get(SurfaceButton sqr)
        {
            if (children.Get(sqr) == null)
            {
                return child.Get(sqr);
            }
            return children.Get(sqr);
        }
        /// <summary>
        /// checks children and sub menus children
        /// </summary>
        /// <param name="sender"></param>
        /// <returns>true if sender exists anywhere in the menu. false otherwise</returns>
        public override bool ContainsButton(SurfaceButton sender)
        {
            return children.Get(sender) !=null || (child!=null && child.ContainsButton(sender));
        }
        /// <summary>
        /// checks if treeMenu ever use
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns>if the canvas provided is a canvas used at anylevel in the treeMenu</returns>
        public bool CanvasIs(Canvas canvas)
        {
            return this.canvas.Equals(canvas) || (child != null && child.CanvasIs(canvas));
        }
        /// <summary>
        /// Finds the Square that contains sender
        /// then returns the file from that square
        /// </summary>
        /// <param name="sender"></param>
        /// <returns>a string path to a file containing information for another treeMenu</returns>
        public override string FileToOpen(SurfaceButton sender)
        {
            if (child!=null && child.ContainsButton(sender))
            {
                return child.FileToOpen(sender);
            }
            else
            {
                return children.Get(sender).SubFile;
            }
        }
        /// <summary>
        /// returns if the square related to the button sender
        /// has the explanation field filled out
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public bool HasExplanation(SurfaceButton sender)
        {
            if (child != null && child.ContainsButton(sender))
            {
                return child.HasExplanation(sender);

            }
            else
            {
                return children.Get(sender).Explanation!=null;
            }
        }
        /// <summary>
        /// When a bread crumb is clicked the TreeMenu up to that
        /// Level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                NotifyChange("Source");
                NotifyChange("Name");
                ReDraw( width,height);
            }
        }
        /// <summary>
        /// removes subtrees after the keeping children has passed
        /// </summary>
        /// <param name="keeping">number of children to keep</param>
        /// <param name="parentCanvas">canvas to remove from</param>
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
        //removes the Map from the window, and from the
        //parentList
        private void ExitUp(object sender, TouchEventArgs e)
        {
            ParentList.Remove(this);
            ((ScatterView)((ScatterViewItem)((Canvas)((SurfaceButton)sender).Parent).Parent).Parent).Items.Remove(((ScatterViewItem)((Canvas)((SurfaceButton)sender).Parent).Parent));
            Log.Deleted(DeletionMethod.X, this);
            StopTimer();
        }
        /// <summary>
        /// Delete Menu after extended period of no use
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void myTimer_Tick(object sender, EventArgs e)
        {
            

            if (!interactive)
            {
                ParentList.Remove(this);
                ((ScatterView)((ScatterViewItem)(canvas.Parent)).Parent).Items.Remove(canvas.Parent);
                Log.Deleted(DeletionMethod.TimeOut, this);
                myTimer.Stop();
            }
            interactive = false;
        }
        /// <summary>
        /// stoping the time prevents the menu from being redeleted
        /// before the garbage collector properly removes it
        /// </summary>
        public void StopTimer()
        {
            if (myTimer != null)
            {
                myTimer.Stop();
            }
        }
        public override string ToString()
        {
            string list = "";
            for (int i = 0; i < breadCrumbs.Count; i++)
            {
                if (i == breadCrumbs.Count-1)
                    list += (breadCrumbs[i].Content) + " ";
                else
                list += (breadCrumbs[i].Content)+", ";
            }
            return base.ToString()+" TreeMenu: "+list;
        }         

        /// <summary>
        /// allows for updating the history
        /// </summary>
        /// <param name="property"></param>
         public void NotifyChange(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        /// <summary>
        /// create a new tree menu based on 
        /// this treeMenu
        /// </summary>
        /// <returns></returns>
         public TreeMenu Clone()
         {
             TreeMenu newmenu = new TreeMenu(children, breadCrumbs, ParentList);
             newmenu.addChildOf(this);

             return newmenu;
         }
        /// <summary>
        /// adds children to newly created clones
        /// </summary>
        /// <param name="menu">menu that has children to be coppied from</param>
         private void addChildOf(TreeMenu menu)
         {
             if (menu.child == null)
             {
                 return;
             }
             child = new TreeMenu(SquareList.createSquareList(menu.child.children));
             child.addChildOf(menu.child);
         }
    }
}
