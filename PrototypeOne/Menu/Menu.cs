using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace PrototypeOne.Menu
{
    public abstract class Menu
    {
        protected SquareList children;


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
        public virtual string FileToOpen(Path sender)
        {
            return children.Get(sender).SubFile;
        }
        
        public virtual bool ContainsPath(Path sender)
        {
            return children.Get(sender) !=null ;
        }
       
    }
}
