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
            
            
            List<Catagory> simpleList=null;
            try
            {
                StreamReader reader = new StreamReader(file);
                simpleList = (List<Catagory>)serializer.Deserialize(reader);
                reader.Close();
            }catch(Exception e){
                Catagory error = new Catagory();
                error.BackGroundColor = Colors.Yellow;
                error.TextColor = Colors.Black;
                
                error.Ratio = 1;
                if(e is FileNotFoundException){
                        error.Title = "File Not Found";
                }
                else if (e is DirectoryNotFoundException)
                {
                    error.Title = "Invalid Path to File";
                }
                else if (e is PathTooLongException)
                {
                    error.Title = "Wow Thats along path to follow";
                }
                else if (e is IOException)
                {
                    if (e.InnerException != null)
                    {
                        
                        error.Title = e.InnerException.ToString();
                    }else
                    error.Title = e.Message;
                }
                else
                {
                    error.Title = e.Message;
                }

                
                simpleList = new List<Catagory>();
                simpleList.Add(error);
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
                Console.Out.WriteLine("\n\n\nproblem WritingFile: " + e.InnerException + "\n\n\n");
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
                square.ratio = cat.Ratio;
                if (cat.SubCatagoryFile!=null && !cat.SubCatagoryFile.Equals(""))
                {
                    square.SubFile = cat.SubCatagoryFile;
                }
                if (cat.Explanation != null && !cat.Explanation.Equals(""))
                {
                    square.Explanation = cat.Explanation;
                }
                newList.Add(square);
            }
            return newList;
        }

        public static SquareList createSquareList(SquareList simpleList)
        {
            SquareList newList = new SquareList();
            for (int i = simpleList.Count() - 1; i >= 0; i--)
            {
                Square cat = simpleList.Get(i);
                FillInfo info = new FillInfo(cat.BackGround, cat.Name, ((SolidColorBrush)cat.Fill.TextColor).Color);
                Square square = new Square(SurfaceWindow1.treeArea * cat.ratio, info);
                square.ratio = cat.ratio;
                if (cat.SubFile != null && !cat.SubFile.Equals(""))
                {
                    square.SubFile = cat.SubFile;
                }
                if (cat.Explanation != null && !cat.Explanation.Equals(""))
                {
                    square.Explanation = cat.Explanation;
                }
                newList.Add(square);
            }
            return newList;
        }
    }
}
