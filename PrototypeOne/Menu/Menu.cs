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
    public abstract class Menu
    {
        protected SquareList children;
        public bool interactive = false;
        protected Timer myTimer;

        public Menu(SquareList list)
        {
            children = list;
        }
        public int Count()
        {
            return children.Count();
        }
        public abstract Canvas DrawMenu();

        public void AddDownListenerToButtons(EventHandler<TouchEventArgs> target)
        {
            children.addTouchDownHandler(target);
        }
        public void AddUpListenerToButtons(EventHandler<TouchEventArgs> target)
        {
            children.addTouchUpHandler(target);
        }
        public void AddEnterListenerToButtons(EventHandler<TouchEventArgs> target)
        {
            children.addTouchEnterHandler(target);
        }
        public void AddLeaveListenerToButtons(EventHandler<TouchEventArgs> target)
        {
            children.addTouchLeaveHandler(target);
        }
        public virtual string FileToOpen(SurfaceButton sender)
        {
            return children.Get(sender).SubFile;
        }
        
        public virtual bool ContainsButton(SurfaceButton sender)
        {
            return children.Get(sender) !=null ;
        }
        public virtual Square Get(SurfaceButton sqr)
        {
            return children.Get(sqr);
        }
       
    }
}
