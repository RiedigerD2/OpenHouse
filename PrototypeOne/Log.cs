using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.IO;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Windows.Media;
using System.Windows;
namespace PrototypeOne
{
    public enum DeletionMethod{Swipe,TimeOut,X};
    public static class Log
    {
        /// <summary>
        /// logs an interactions
        /// </summary>
        /// <param name="mutex">use SurfaceWindow1</param>
        /// <param name="Td"></param>
        /// <param name="touched">button that was touched with</param>
        /// <param name="Container">container of the button touched</param>
        public static void Interaction(Object mutex,TouchDevice Td,Object touched,Object Container){

            lock (mutex)
            {
                StreamWriter writer = new StreamWriter("LogFile.txt", true);
                string TimeDevice=DateTime.Now.TimeOfDay.ToString()+"; Interacted with : ";
                if (Td.GetIsFingerRecognized())
                {
                    TimeDevice+= "TypeOfDevice: Finger";
                }
                else
                    if (Td.GetIsTagRecognized())
                    {
                        TimeDevice+="TypeOfDevice: Tag";
                    }
                    else
                    {
                        TimeDevice+="TypeOfDevice: Blob";
                    }
                    writer.WriteLine(TimeDevice+" X={0}, Y={1} Angle={2} ; " + Container.ToString()+"; " + touched.ToString() , Td.GetPosition((SurfaceWindow1)mutex).X, Td.GetPosition((SurfaceWindow1)mutex).Y,Td.GetOrientation((SurfaceWindow1)mutex));
                    writer.Close();
            }    
        }
        public static void Moved(Point center, double angle,Menu.Menu menu)
        {
            StreamWriter writer = new StreamWriter("LogFile.txt", true);
            writer.WriteLine(DateTime.Now.TimeOfDay.ToString() + "; Moved: X={0}, Y={1} Angle={2} ; "+menu.ToString() , center.X,center.Y,angle);
            writer.Close();
        }
        /// <summary>
        /// logs for tree menu deletion
        /// </summary>
        /// <param name="method">Enum of deletion method</param>
        /// <param name="menu"> menu that was deleted</param>
        public static void Deleted(DeletionMethod method, Menu.Menu menu)
        {
            StreamWriter writer = new StreamWriter("LogFile.txt", true);
            writer.WriteLine(DateTime.Now.TimeOfDay.ToString()+"; Deleted: "+method.ToString()+"; "+menu.ToString());
            writer.Close();
        }

        /// <summary>
        /// logs when menus are resized
        /// </summary>
        /// <param name="menu">new width</param>
        /// <param name="factor">new height</param>
        public static void Resized( Menu.Menu menu,double width,double height)
        {
            StreamWriter writer = new StreamWriter("LogFile.txt", true);
            writer.WriteLine(DateTime.Now.TimeOfDay.ToString() + "; Resized: " + " Width:"+ width +" Height: "+ height+"; "+menu.ToString());
            writer.Close();
        }
        public static void FromHistory(Menu.Menu menu){
            StreamWriter writer = new StreamWriter("LogFile.txt", true);
            writer.WriteLine(DateTime.Now.TimeOfDay.ToString() + "; From History: " + menu.ToString());
            writer.Close();
        }
        public static void CrumbUse(Menu.Menu menu)
        {
            StreamWriter writer = new StreamWriter("LogFile.txt", true);
            writer.WriteLine(DateTime.Now.TimeOfDay.ToString() + "; Crumb Use: " + menu.ToString());
            writer.Close();
        }

    }
}
