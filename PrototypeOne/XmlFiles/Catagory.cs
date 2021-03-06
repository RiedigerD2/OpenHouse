﻿using System;
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
using PrototypeOne.XmlFiles;
using System.Xml;


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

        public ImageInformation ImageSetup { get; set; }

        [XmlArray(ElementName = "Slides")]
        [
             XmlArrayItem(typeof(ImageFile), ElementName = "ImageFile"),
             XmlArrayItem(typeof(VideoFile), ElementName = "VideoFile"),
             XmlArrayItem(typeof(TextFile), ElementName = "TextPage")
        ]
        public List<MediaFile> Slides { get; set; }

        public Catagory() {
           /// Slides = new List<Int32>(6);
        }
        public Catagory(Color BackGroundColor, double Ratio) {
           // Slides = new List<Int32>(6);
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
                        
                        error.Title = e.InnerException.Message;
                    }else
                    error.Title += e.Message;
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
            
            try
            {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Catagory>));
            FileStream writer = new FileStream(file, FileMode.OpenOrCreate);
            
                serializer.Serialize(writer, saveList);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("\nproblem WritingFile: " + e.InnerException + "\n\n\n");
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
                    square.SubFile = @"Resources/Information/" + cat.SubCatagoryFile;
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
                    square.ImageString = @"Resources/Images/" + cat.Image;
                }
                if (cat.Slides != null && cat.Slides.Count>0)
                {
                    square.Slides = cat.Slides;
                }
                if (cat.ImageSetup != null)
                {
                    square.singleImage = cat.ImageSetup;
                    square.singleImage.Path = @"Resources/Images/" + square.singleImage.Path;
                }
                //video for explanation
                if (cat.Video!=null && !cat.Video.Equals(""))
                {
                    square.VideoString = @"Resources/Videos/" + cat.Video;
                }
                //background image to use instead of backgroundcolor
                if (cat.BackGroundImage!=null && !cat.BackGroundImage.Equals(""))
                {
                    square.setBackGround(cat.BackGroundImage);
                }
                newList.Add(square);
            }
            
            return newList;
        }

        
    }
}
