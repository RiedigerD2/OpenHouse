﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using PrototypeOne.XmlFiles;

namespace PrototypeOne
{
    public class Square: IComparable<Square>
    {
        /// <summary>
        /// cur and new variables to test wether or not hte 
        /// cur aspect ratio is better or worse than the potential new asspect ratio
        /// </summary>
        private double curWidth, curHeight, newWidth, newHeight;
        public ImageInformation singleImage { get; set; }
        private TextBlock textBlock;
        public List<MediaFile> Slides { get; set; }
        public double Width {
            get { return curWidth; }
            set { curWidth = newWidth;
                  curHeight = newHeight;
                  newWidth = value;
                  newHeight = Area / newWidth; 
            }
        }      
        public double Height
        {
            get { return curHeight; }
            set
            {
                curHeight = newHeight;
                curWidth = newWidth;
                newHeight = value;
                newWidth = Area / newHeight;
            }
        }
        public double X { get; set; }
        public double Y { get; set; }
        public SurfaceButton Button
        {
            get;
            set;
        }

        public double Ratio;
        public double Area;
        public string Name;
        public string SubFile;
        public string Explanation;
        public string ImageString;
        public string VideoString;
        private Brush backGroundColorBrush;
        private Brush backGroundImageBrush;
        public Color BackGroundColor {
            get
            {
                if(backGroundColorBrush!=null)
                return ((SolidColorBrush)backGroundColorBrush).Color;
                else return Colors.Black;
            }
            private set { setBackGround(value); } }
        
        public Brush BackGroundBrush {
            get
            {
                if (backGroundImageBrush != null)
                {
                    return backGroundImageBrush;
                }
                return backGroundColorBrush;
            }
            private set { BackGroundBrush = value; }
        }

        public Color TextColor {
            get
            {
                if (TextBrush != null)
                    return ((SolidColorBrush)TextBrush).Color;
                else return Colors.Blue;//if I see black and blue there is a problem
            }
            set { TextColor = value; }
        }
        public Brush TextBrush { get; private set; }



        
        public void setBackGround(Color backGroundColor){ 
            Brush brush = new SolidColorBrush();
            ((SolidColorBrush)brush).Color = backGroundColor;
            backGroundColorBrush = brush;
        }
        /// <summary>
        /// uses the path to an image
        /// to create a ImageBrush to use for backgrounds
        /// </summary>
        /// <param name="imagePath">path to an image file assumed to be in Resources/Images/</param>
        public void setBackGround(string imagePath)
        {

             backGroundImageBrush = new ImageBrush(new BitmapImage(new Uri(@"Resources/Images/"+imagePath, UriKind.Relative)));
        }
        public void setTextColor(Color textColor)
        {
            TextBrush = new SolidColorBrush(textColor);
        }

        public bool HasExplanation()
        {

            return Explanation != null || ImageString != null ||VideoString != null || (Slides!=null && Slides.Count>0) || singleImage!=null;
        
        }
       
        public Square(Double size,string name) 
        {
            Area = size;
            Name = name;
        }
        /// <summary>
        /// needed to create a squarelist
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int IComparable<Square>.CompareTo(Square obj) {
            if (this.Area > obj.Area)
            {
                return -1;
            }
            if (obj.Area > this.Area)
            {
                return 1;
            }
            return 0;
        }     
        /// <summary>
        /// 
        /// </summary>
        /// <returns>previous aspect ratio</returns>
        public double lastAR()
        {
            return curHeight / curWidth > curWidth / curHeight ? curHeight / curWidth : curWidth / curHeight; 
        }
        /// <summary>
        /// returns potential new aspect ratio
        /// </summary>
        /// <returns></returns>
        public double newAR()
        {
            return newHeight / newWidth > newWidth / newHeight ? newHeight / newWidth : newWidth / newHeight;
        }
        /// <summary>
        /// returns a text block with all the right 
        /// transforms so the block 
        /// ends in the middle the button 
        /// it's associated with
        /// </summary>
        /// <returns></returns>
        public TextBlock GetTextBlock()
        {
            if(textBlock==null){
             textBlock = new TextBlock();
             textBlock.Text = Name;
             textBlock.Measure(new Size(0, 0));
             textBlock.Arrange(new Rect(0, 0, 0, 0));
            }

            if (textBlock.ActualHeight < curHeight)
            {

                textBlock.RenderTransform = new TranslateTransform(X, Y + 0.5 * curHeight - 0.5 * textBlock.ActualHeight);
                textBlock.MaxHeight = 0.5 * curHeight + 0.5 * textBlock.ActualHeight;
            }
            else
            {
                textBlock.RenderTransform = new TranslateTransform(X, Y);
                textBlock.MaxHeight =  curHeight;
            }
            textBlock.Width = Width;
            textBlock.Focusable = false;
            textBlock.IsHitTestVisible = false;
            textBlock.Foreground = TextBrush;
            textBlock.TextWrapping = TextWrapping.WrapWithOverflow;
            textBlock.TextAlignment = TextAlignment.Center;

            return textBlock;
        }
        /// <summary>
        /// returns a text block with all the right 
        /// transforms so the block 
        /// at the top of the button 
        /// it's associated with 
        /// same text block everytime
        /// Could contain images
        /// </summary>
        /// <returns></returns>
        public TextBlock GetTextBlockTop()
        {
          
           
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Clear();
            
            //If it is placed on the top we will place the image in TreeMenu
            if (singleImage != null && singleImage.Placement==_Placement.Inline)
            {
               
                Image freshImage = new Image();
                freshImage.Source = new BitmapImage(new Uri(this.singleImage.Path, UriKind.Relative));
              
                freshImage.HorizontalAlignment = HorizontalAlignment.Center;
                freshImage.Width = this.singleImage.Width;
                freshImage.Height = this.singleImage.Height;
                freshImage.Visibility = Visibility.Visible;
               
                 InlineUIContainer InlineImage = new InlineUIContainer(freshImage);
                 InlineImage.BaselineAlignment = BaselineAlignment.Bottom;
               
              
                    textBlock.Inlines.Add(InlineImage);
                    textBlock.Inlines.Add(new System.Windows.Documents.Run(Name));
              
            }
            else
            {
                textBlock.Inlines.Add(new System.Windows.Documents.Run(Name));
            }
                
           
            textBlock.Focusable = false;
            textBlock.IsHitTestVisible = false;
            textBlock.Foreground = TextBrush;
            textBlock.TextWrapping = TextWrapping.WrapWithOverflow;
            textBlock.TextAlignment = TextAlignment.Left;

            
           
            return textBlock;
        }     
        /// <summary>
        /// returns a text block with no transformations
        /// will be the same text block everytime
        /// </summary>
        /// <returns></returns>
        public TextBlock GetTextBlockCenter()
        {
            if (textBlock == null)
            {
                textBlock = new TextBlock();
            }

            textBlock.Text = Name;
            textBlock.Focusable = false;
            textBlock.IsHitTestVisible = false;
            textBlock.Foreground = TextBrush;
            textBlock.TextWrapping = TextWrapping.WrapWithOverflow;
            textBlock.TextAlignment = TextAlignment.Center;

            return textBlock;
        }
        /// <summary>
        /// returns a text block with no transformations
        /// Left aligned text This is a new textblock every time
        /// </summary>
        /// <returns></returns>
        public TextBlock GetTextBlockLeft()
        {

            TextBlock text = new TextBlock();

            text.Text = Name;
            text.Focusable = false;
            text.IsHitTestVisible = false;
            text.Foreground = TextBrush;
            text.TextAlignment = TextAlignment.Left;
            text.TextWrapping = TextWrapping.WrapWithOverflow;
            text.Width = Width;

            return text;
        }
        public override string ToString()
        {
            return "Square: " + Name + ", Color: " + BackGroundColor;
        }

        public void Delete()
        {
            Button = null;
            textBlock = null;
            TextBrush = null;
            backGroundColorBrush = null;
            backGroundImageBrush = null;
        }
    }

   
}
