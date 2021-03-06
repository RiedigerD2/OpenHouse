﻿using System;
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
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Media.Animation;
namespace PrototypeOne.Menu
{
    
    public class IndividualMenu:Menu
    {
        private Canvas canvas;
        private Storyboard board;
        ObservableCollection<Menu> history = new ObservableCollection<Menu>();
        public ObservableCollection<Menu> History
        {
            get { return history; }
        }

        public double Width { get { return children.Count() * SurfaceWindow1.MenuTileSize - 10; } private set { } }
        public double Height { get { return  SurfaceWindow1.MenuTileSize-2; } private set { } }
        /// <summary>
        /// Creates Individual menu
        /// </summary>
        /// <param name="children">the list used to populate the menu</param>
        /// <param name="board">board will begin after a set time of no interactions</param>
        public IndividualMenu(SquareList children,Storyboard board)
            : base(children)
        {
            this.board=board;
            myTimer.Tick += new EventHandler(myTimer_Tick);
        }
        

        /// <summary>
        /// adds buttons to a canvas based on 
        /// the information from the children list
        /// </summary>
        /// <returns>Ca</returns>
        public override Canvas DrawMenu()
        {
            if (canvas != null)
            {
                return canvas;
            }
            canvas = new Canvas();
            ScatterViewItem sizeItem=new ScatterViewItem();
            double X = -(SurfaceWindow1.MenuTileSize * (double)children.Count() / 2) + 0.5 * SurfaceWindow1.MenuTileSize;
            for (int i = 0 ; i < children.Count();i++ ,X+=SurfaceWindow1.MenuTileSize-1)
            {
                Square sqr = children.Get(i);

                System.Windows.Rect rect = new System.Windows.Rect(X,0,SurfaceWindow1.MenuTileSize,SurfaceWindow1.MenuTileSize);
                RectangleGeometry rectangle = new RectangleGeometry(rect);

                SurfaceButton button = new SurfaceButton();
                button.Height = SurfaceWindow1.MenuTileSize;
                button.Width = SurfaceWindow1.MenuTileSize;
                button.RenderTransform = new TranslateTransform(X, -4);//-4 hides the button bellow the menu
                button.BorderBrush = Brushes.Black;
                button.BorderThickness = new Thickness(0.2); 
                sqr.Button = button;

                button.Background = sqr.BackGroundBrush;
               


                canvas.Children.Add(button);
                TextBlock block = sqr.GetTextBlockCenter();/*********/
                block.RenderTransform = new TranslateTransform(X, SurfaceWindow1.MenuTileSize*1.2);
                block.Height = SurfaceWindow1.MenuTileSize;
                block.Width = SurfaceWindow1.MenuTileSize;
                canvas.Children.Add(block);
            }
            return canvas;
        }

        /// <summary>
        /// Executed at end of the timer interval
        /// </summary>
        /// <param name="sender">timer that finished an interval</param>
        /// <param name="e"></param>
        void myTimer_Tick(object sender, EventArgs e)
        {
            if (!interactive)
            {

                try
                {
                    board.Begin();
                }
                catch (Exception el)
                {
                    Console.Out.WriteLine("Timed Story board Individual menu:" + el.Message);
                }
            }
            interactive = false;
        }
        /// <summary>
        /// stops story board associated with the menu
        /// </summary>
        public void StopAnimation() {
            board.Stop();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>returns true if the board is active</returns>
        public bool IsAnimating() {
            return board.GetCurrentGlobalSpeed() != 0;
        }
        /// <summary>
        /// adds the menu opoened to the observable collection history
        /// </summary>
        /// <param name="opened"></param>
        public void AddToHistory(Menu opened)
        {
            history.Insert(0,opened);

            if (history.Count > 9)
            {
                TreeMenu removed = (TreeMenu)history[history.Count - 1];
                history.RemoveAt(history.Count - 1);
                removed.Delete();
            }
        }
        
        public override string ToString()
        {
            if (board.GetCurrentGlobalSpeed() == 0)
            {
                return base.ToString() + "SideMenu: Active";
            }
            return base.ToString() + "SideMenu: Animating";
        }
    }
}
