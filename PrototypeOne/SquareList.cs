using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Shapes;
using System.Windows.Input;
using Microsoft.Surface.Presentation.Controls;

namespace PrototypeOne
{
    public class SquareList : IEnumerable
    {
       
        
        private List<Square> list;
        public SquareList() {
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
                if (s.Button!=null && s.Button.Equals(p))
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
        /**
         * count number of squares from the start of the list to have the same Width
         * totalHeight the height the sum of the squares heights must equal
         **/
        public void SetSameWidth(int start,int count, double totalHeight)
        {
            double width=0;
            for (int i = 0; i < count && i + start < list.Count(); i++)
            {
                width += list[start+i].Area;
            }
            width = width / totalHeight;
            for (int i = 0; i < count && i + start < list.Count(); i++)
            {
                list[start+i].Width = width;
            }
        }

        //same as above for height
        public void SetSameHeight(int start, int count, double totalWidth)
        {
            double height = 0;
            for (int i = 0; i < count && i+start<list.Count(); i++)
            {
                height += list[start+i].Area;
            }
            height = height / totalWidth;
            for (int i = 0; i < count && i + start < list.Count(); i++)
            {
                list[start+i].Height = height;
            }
        }


        private double AverageNewAR(int start,int count) {
            double AR=0;

            for (int i = 0; i < count && i + start < list.Count(); i++)
            {
                AR += list[start+i].newAR();
            }
            return AR/count;
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
                AR += list[start+i].lastAR();
            }
            return AR / count;
        }
        public bool IsAverageARLess(int start, int count) {
            if (count == 1) return true;
            return AverageLastAR(start,count-1) > AverageNewAR(start,count);
        }
        public void ResizeAreas(double area) {
            foreach (Square sqr in list)
            {
                sqr.Area =area*sqr.ratio;
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
        public void addTouchUpHandler( EventHandler<TouchEventArgs> target)
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
       

        IEnumerator IEnumerable.GetEnumerator() {
            return list.GetEnumerator();
        }

       
    }
}
