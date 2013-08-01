using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Forms;
namespace PrototypeOne.Menu
{
    /// <summary>
    /// base menu
    /// </summary>
    public abstract class Menu
    {
        /// <summary>
        /// Used to populate the menu
        /// </summary>
        protected SquareList children;
        /// <summary>
        /// True when the menu has been interacted with 
        /// with in the last timer interval
        /// when true changes the effect of the end interval
        /// handler
        /// </summary>
        public bool interactive = false;

        /// <summary>
        /// calls a function on an interval bases 
        /// </summary>
        protected Timer myTimer;
      
        public Menu(SquareList list)
        {
            children = list;
            myTimer = new Timer();
            myTimer.Interval = 60000;
            myTimer.Enabled = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>number of children</returns>
        public int Count()
        {
            return children.Count();

        }
        /// <summary>
        /// provide a canvas populated based on the information
        /// stored in children Squarelist
        /// </summary>
        /// <returns></returns>
        public abstract Canvas DrawMenu();

        /// <summary>
        /// adds event handler to all children
        /// </summary>
        /// <param name="target"></param>
        public virtual void AddDownListenerToButtons(EventHandler<TouchEventArgs> target)
        {
            children.addTouchDownHandler(target);
        }
        /// <summary>
        /// adds event handler to all children
        /// </summary>
        /// <param name="target"></param>
        public virtual void AddUpListenerToButtons(EventHandler<TouchEventArgs> target)
        {
            children.addTouchUpHandler(target);
        }
        /// <summary>
        /// adds event handler to all children
        /// </summary>
        /// <param name="target"></param>
        public virtual void AddEnterListenerToButtons(EventHandler<TouchEventArgs> target)
        {
            children.addTouchEnterHandler(target);
        }
        /// <summary>
        /// adds event handler to all children
        /// </summary>
        /// <param name="target"></param>
        public virtual void AddLeaveListenerToButtons(EventHandler<TouchEventArgs> target)
        {
            children.addTouchLeaveHandler(target);
        }
        /// <summary>
        /// adds event handler to all children
        /// </summary>
        /// <param name="target"></param>
        public virtual string FileToOpen(SurfaceButton sender)
        {
            return children.Get(sender).SubFile;
        }
        /// <summary>
        /// adds event handler to all children
        /// </summary>
        /// <param name="target"></param>
        public virtual bool ContainsButton(SurfaceButton sender)
        {
            return children.Get(sender) !=null ;
        }
        /// <summary>
        /// adds event handler to all children
        /// </summary>
        /// <param name="target"></param>
        public virtual Square Get(SurfaceButton sqr)
        {
            return children.Get(sqr);
        }
        /// <summary>
        /// adds event handler to all children
        /// </summary>
        /// <param name="target"></param>
        public override string ToString()
        {
            return "";
        }
    }
}
