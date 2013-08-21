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
        /// logs an interaction
        /// </summary>
        /// <param name="parent"> Parent of the container</param>
        /// <param name="Td"></param>
        /// <param name="touched">button that was touched with</param>
        /// <param name="Container">container of the button touched</param>
        public static void Interaction(Object parent,TouchDevice Td,Object touched,Object Container){

           
                
                string Device="Interacted with : ";
                if (Td.GetIsFingerRecognized())
                {
                    Device+= "TypeOfDevice: Finger";
                }
                else
                    if (Td.GetIsTagRecognized())
                    {
                        Device+="TypeOfDevice: Tag";
                    }
                    else
                    {
                        Device+="TypeOfDevice: Blob";
                    }
                WriteLog(Device + " X=" + Td.GetPosition((SurfaceWindow1)parent).X + ", Y=" + Td.GetPosition((SurfaceWindow1)parent).Y + " Angle=" + Td.GetOrientation((SurfaceWindow1)parent) + " ; " + Container.ToString() + "; " + touched.ToString());
                    
            
        }
        public static void Moved(Point center, double angle,Menu.Menu menu)
        {
            WriteLog( "Moved: X="+center.X+", Y="+center.Y+" Angle="+angle+" ; "+menu.ToString());
           
        }
        /// <summary>
        /// logs for tree menu deletion
        /// </summary>
        /// <param name="method">Enum of deletion method</param>
        /// <param name="menu"> menu that was deleted</param>
        public static void Deleted(DeletionMethod method, Menu.Menu menu)
        {
            WriteLog("Deleted: "+method.ToString()+"; "+menu.ToString());
        }

        /// <summary>
        /// logs when menus are resized
        /// </summary>
        /// <param name="menu">new width</param>
        /// <param name="factor">new height</param>
        public static void Resized( Menu.Menu menu,double width,double height)
        {
            WriteLog( "Resized: " + " Width:"+ width +" Height: "+ height+"; "+menu.ToString());
        }
        public static void FromHistory(Menu.Menu menu)
        {
            WriteLog("From History: " + menu.ToString());
        }
        public static void CrumbUse(Menu.Menu menu)
        {
            WriteLog( "Crumb Use: " + menu.ToString());
        }
        private static void WriteLog(string logString)
        {
            StreamWriter writer = new StreamWriter(DateTime.Now.Day+"LogFile.txt", true);
            writer.WriteLine(DateTime.Now.TimeOfDay.ToString()+"; " + logString);
            writer.Close();
        }

    }
}
