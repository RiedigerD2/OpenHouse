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
    /// <summary>
    /// This class is used to read from the 
    /// xml config files
    /// </summary>
    public class Catagory
    {
        public Color TextColor { get; set; }
        public Color BackGroundColor{ get; set; }
        public Double Ratio { get; set; }
        public String Title { get; set; }
        public string Explanation { get; set; }
        public string SubCatagoryFile { get; set; }
   
        public string Image { get; set; }
        public string BackGroundImage { get; set; }
        public string Video { get; set; }
   
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
            {
                serializer.Serialize(reader, saveList);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("\n\n\nproblem WritingFile: " + e.InnerException + "\n\n\n");
            }

        }
        /// <summary>
        /// creates a SqruarList based on simpleList
        /// </summary>
        /// <param name="simpleList">list of catagories typically popullated by a xml reader</param>
        /// <returns></returns>
        public static SquareList createSquareList(List<Catagory> simpleList)
        {
            SquareList newList=new SquareList();
            for(int i=simpleList.Count()-1;i>=0;i--)
            {
                Catagory cat = simpleList[i];
                 Square square = new Square(SurfaceWindow1.treeArea * cat.Ratio, cat.Title);

                // stated in the xsd that all catagories must have at least name ratio and backGroundColor
                square.setBackGround(cat.BackGroundColor);
                square.Ratio = cat.Ratio;
               
                //set file to open if this square is followed
                if (cat.SubCatagoryFile!=null && !cat.SubCatagoryFile.Equals(""))
                {
                    square.SubFile = cat.SubCatagoryFile;
                }

                //provide text explanation if no file is provided to follow
                if (cat.Explanation != null && !cat.Explanation.Equals(""))
                {
                    square.Explanation = cat.Explanation;
                }
                
                //if the text color is not set
                //set the text color to the inverse of the background color
                //so the text is at least visible
                if (cat.TextColor==null || cat.TextColor.A == 0)
                {
                    Color textColor = new Color();
                    textColor.ScA = 1;
                    textColor.ScR = 1 - cat.BackGroundColor.ScR;
                    textColor.ScG = 1 - cat.BackGroundColor.ScG;
                    textColor.ScB = 1 - cat.BackGroundColor.ScB;
                    square.setTextColor(textColor);
                }
                else
                {
                    square.setTextColor(cat.TextColor);
                }
                
                //Images for explanation
                if (cat.Image!=null && !cat.Image.Equals(""))
                {
                    square.setImage(cat.Image);
                }
                //video for explanation
                if (cat.Video!=null && !cat.Video.Equals(""))
                {
                    square.setVideo(cat.Video);
                }
                //background image to use instead of backgroundcolor
                if (cat.BackGroundImage!=null && cat.BackGroundImage.Equals(cat.BackGroundImage))
                {
                    square.setBackGround(cat.BackGroundImage);
                }
               



                newList.Add(square);
            }
            return newList;
        }

        
    }
}
