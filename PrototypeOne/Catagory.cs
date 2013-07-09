using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Markup;
namespace PrototypeOne
{
    public class Catagory
    {
        public Color TextColor { get; set; }
        public Color BackGroundColor{ get; set; }
        public Double Ratio { get; set; }
        public String Title { get; set; }
        public string Explanation { get; set; }
        public string SubCatagoryFile { get; set; }
        public Catagory() { }
        public Catagory(Color BackGroundColor, double Ratio) {
            this.BackGroundColor = BackGroundColor;
            this.Ratio = Ratio;
        }

        public static SquareList ReadFile(string file)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Catagory>));
            
            FileStream reader =new FileStream(file, FileMode.OpenOrCreate);
            List<Catagory> simpleList=null;
            try
            {
                simpleList = (List<Catagory>)serializer.Deserialize(reader);

            }catch(Exception e){
                Console.Out.WriteLine("\n\n\nProblem Reading Found: "+e.InnerException+"\n\n\n");
            }
    
            return createSquareList(simpleList);

              
        }

        public static void WriteFile(List<Catagory> saveList,string file)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(List<Catagory>));
            FileStream reader = new FileStream(file, FileMode.OpenOrCreate);
            try
            {//simpleList=(List<Catagory>)
                serializer.Serialize(reader, saveList);

            }
            catch (Exception e)
            {
                Console.Out.WriteLine("\n\n\nproblem Found: " + e.InnerException + "\n\n\n");
            }

        }

        public static SquareList createSquareList(List<Catagory> simpleList)
        {
            SquareList newList=new SquareList();
            for(int i=simpleList.Count()-1;i>=0;i--)
            {
                Catagory cat = simpleList[i];
                FillInfo info = new FillInfo(cat.BackGroundColor, cat.Title, cat.TextColor);
                Square square = new Square(SurfaceWindow1.treeArea * cat.Ratio, info);
                newList.Add(square);
            }
            return newList;
        }
    }
}
