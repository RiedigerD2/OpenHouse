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

namespace PrototypeOne
{
    public enum DeletionMethod{Swipe,TimeOut,X};
    public static class Log
    {
        public static void Interaction(Object mutex,TouchDevice Td,Object touched,Object Container){

            lock (mutex)
            {
                StreamWriter writer = new StreamWriter("LogFile.txt", true);
                string TimeDevice=DateTime.Now.TimeOfDay.ToString()+"; ";
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
                    writer.WriteLine(TimeDevice+" X={0}, Y={1} Angle={2} ; Interacted With: " + touched.ToString()+"; Container:" + Container.ToString() , Td.GetPosition((SurfaceWindow1)mutex).X, Td.GetPosition((SurfaceWindow1)mutex).Y,Td.GetOrientation((SurfaceWindow1)mutex));
                    writer.Close();
            }    
        }

        public static void Deleted(DeletionMethod method, Menu.Menu menu)
        {
            StreamWriter writer = new StreamWriter("LogFile.txt", true);
            writer.WriteLine(DateTime.Now.TimeOfDay.ToString()+"; "+menu.ToString()+"; Deleted: "+method.ToString());
            writer.Close();
        }

        public static void Resized( Menu.Menu menu,double factor)
        {
            StreamWriter writer = new StreamWriter("LogFile.txt", true);
            writer.WriteLine(DateTime.Now.TimeOfDay.ToString() + "; Resized:" + menu.ToString()+" Factor:"+ factor);
            writer.Close();
        }

    }
}
