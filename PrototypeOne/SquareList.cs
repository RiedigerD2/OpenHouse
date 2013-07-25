using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls;

namespace PrototypeOne
{
    public class SquareList : IEnumerable
    {


        private List<Square> list;
        public SquareList()
        {
            list = new List<Square>();
        }

        public Square Get(int i)
        {
            return (Square)list[i];
        }
        /**
         * returns the square that was drawn using path p
         */
        public Square Get(SurfaceButton p)
        {
            foreach (Square s in list)
            {
                if (s.Button != null && s.Button.Equals(p))
                {
                    return s;
                }
            }
            return null;
        }
        public void Add(Square s)
        {
            list.Add(s);
            list.Sort();//not efficient fixLater
        }
        public void Clear()
        {
            list.Clear();
        }
        public int Count()
        {
            return list.Count();
        }


        /// <summary>
        /// squares from start too count will all be set with the 
        /// same width. the sum of those squares heights will equal total hieght
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="totalHeight"></param>
        public void SetSameWidth(int start, int count, double totalHeight)
        {
            double width = 0;
            for (int i = 0; i < count && i + start < list.Count(); i++)
            {
                width += list[start + i].Area;
            }
            width = width / totalHeight;
            for (int i = 0; i < count && i + start < list.Count(); i++)
            {
                list[start + i].Width = width;
            }
        }

        /// <summary>
        /// squares from start too count will all be set with the 
        /// same height. the sum of those squares widths will equal totalwidth
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="totalWidth"></param>
        public void SetSameHeight(int start, int count, double totalWidth)
        {
            double height = 0;
            for (int i = 0; i < count && i + start < list.Count(); i++)
            {
                height += list[start + i].Area;
            }
            height = height / totalWidth;
            for (int i = 0; i < count && i + start < list.Count(); i++)
            {
                list[start + i].Height = height;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns>the new average aspect ratio of squares from start to count
        /// in list</returns>
        private double AverageNewAR(int start, int count)
        {
            double AR = 0;

            for (int i = 0; i < count && i + start < list.Count(); i++)
            {
                AR += list[start + i].newAR();
            }
            return AR / count;
        }
        /// <summary>
        /// returns the previous average aspect ratio 
        /// for the count squares from list  starting at start position
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private double AverageLastAR(int start, int count)
        {
            double AR = 0;

            for (int i = 0; i < count && i + start < list.Count(); i++)
            {
                AR += list[start + i].lastAR();
            }
            return AR / count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns>true when the new average aspect ratio is closer to one 
        /// than the previous aspect ratio average</returns>
        public bool IsAverageARLess(int start, int count)
        {
            if (count == 1) return true;
            return AverageLastAR(start, count - 1) > AverageNewAR(start, count);
        }
        public void ResizeAreas(double area)
        {
            foreach (Square sqr in list)
            {
                sqr.Area = area * sqr.Ratio;
            }
        }

        public SquareList sublist(int start, int count)
        {
            SquareList sublist = new SquareList();
            for (int i = 0; i < count && i + start < list.Count(); i++)
            {
                sublist.Add(list[start + i]);
            }
            return sublist;
        }

        /// <summary>
        /// adds target to PreviewTouchUp Event to the button fields for all squares in list
        /// </summary>
        /// <param name="target"></param>
        public void addTouchUpHandler(EventHandler<TouchEventArgs> target)
        {

            foreach (Square sqr in list)
            {
                if (sqr.Button != null && target != null)
                {
                    sqr.Button.PreviewTouchUp += target;

                }
            }
        }
        /// <summary>
        /// adds target to PreviewDownUp Event to the button fields for all squares in list
        /// </summary>
        /// <param name="target"></param>
        public void addTouchDownHandler(EventHandler<TouchEventArgs> target)
        {

            foreach (Square sqr in list)
            {
                if (sqr.Button != null && target != null)
                {
                    sqr.Button.PreviewTouchDown += target;

                }
            }
        }
        /// <summary>
        /// adds target to TouchEnter Event to the button fields for all squares in list
        /// </summary>
        /// <param name="target"></param>
        public void addTouchEnterHandler(EventHandler<TouchEventArgs> target)
        {

            foreach (Square sqr in list)
            {
                if (sqr.Button != null && target != null)
                {
                    sqr.Button.TouchEnter += target;
                }
            }
        }
        /// <summary>
        /// adds target to TouchLeave Event to the button fields for all squares in list
        /// </summary>
        /// <param name="target"></param>
        public void addTouchLeaveHandler(EventHandler<TouchEventArgs> target)
        {

            foreach (Square sqr in list)
            {
                if (sqr.Button != null && target != null)
                {
                    sqr.Button.TouchLeave += target;
                }
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }


        public static SquareList createSquareList(SquareList simpleList)
        {
            SquareList newList = new SquareList();
            for (int i = simpleList.Count() - 1; i >= 0; i--)
            {
                Square cat = simpleList.Get(i);

                Square square = new Square(SurfaceWindow1.treeArea * cat.Ratio, cat.Name);
                square.setBackGround(cat.BackGroundColor);
                if (cat.TextColor == null)
                    square.setTextColor(Colors.Black);
                else
                    square.setTextColor(cat.TextColor);
                square.Ratio = cat.Ratio;
                if (cat.SubFile != null && !cat.SubFile.Equals(""))
                {
                    square.SubFile = cat.SubFile;
                }
                if (cat.Explanation != null && !cat.Explanation.Equals(""))
                {
                    square.Explanation = cat.Explanation;
                }
                //Images for explanation
                /* if (cat.Image!=null && !cat.Image.Equals(""))
                 {
                     square.Image=cat.Image;
                 }*/
                //video for explanation
                if (cat.Video != null && !cat.Video.Equals(""))
                {
                    square.setVideo(cat.Video.Source.AbsolutePath);
                }
                //background image to use instead of backgroundcolor
                /*if (cat.BackGroundImage!=null && cat.BackGroundImage.Equals(cat.BackGroundImage))
                {
                    square.BackGroundBrush=cat.BackGroundBrush;
                }*/
                newList.Add(square);
            }
            return newList;
        }
    }
}
      