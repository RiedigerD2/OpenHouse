﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Shapes;
using System.Windows.Input;


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
        public Square Get(Path p)
        {
            foreach (Square s in list)
            {
                if (s.Path!=null && s.Path.Equals(p))
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
            for (int i = 0; i < count; i++)
            {
                width += list[start+i].Area;
            }
            width = width / totalHeight;
            for (int i = 0; i < count; i++)
            {
                list[start+i].Width = width;
            }
        }

        //same as above for height
        public void SetSameHeight(int start, int count, double totalWidth)
        {
            double height = 0;
            for (int i = 0; i < count; i++)
            {
                height += list[start+i].Area;
            }
            height = height / totalWidth;
            for (int i = 0; i < count; i++)
            {
                list[start+i].Height = height;
            }
        }


        private double AverageNewAR(int start,int count) {
            double AR=0;

            for (int i = 0; i < count; i++)
            {
                AR += list[start+i].newAR();
            }
            return AR/count;
        }
        private double AverageLastAR(int start, int count)
        {
            double AR = 0;

            for (int i = 0; i < count; i++)
            {
                AR += list[start+i].lastAR();
            }
            return AR / count;
        }
        public bool IsAverageARLess(int start, int count) {
            if (count == 1) return true;
            return AverageLastAR(start,count-1) > AverageNewAR(start,count);
        }

        public SquareList sublist(int start, int count)
        {
            SquareList sublist = new SquareList();
            for (int i = 0; i < count; i++)
            {
                sublist.Add(list[start + i]);
            }
            return sublist;
        }
        public void addTouchUpHandler( EventHandler<TouchEventArgs> target)
        {

            foreach (Square sqr in list)
            {
                if (sqr.Path != null && target != null)
                {
                    sqr.Path.PreviewTouchUp += target;
                    sqr.Path.GotTouchCapture += target;
                }
            }
        }
        public void addTouchDownHandler(EventHandler<TouchEventArgs> target)
        {

            foreach (Square sqr in list)
            {
                if (sqr.Path != null && target != null)
                {
                    sqr.Path.PreviewTouchDown += target;
                
                }
            }
        }
        public void addTouchEnterHandler(EventHandler<TouchEventArgs> target)
        {

            foreach (Square sqr in list)
            {
                if (sqr.Path != null && target != null)
                {
                    sqr.Path.TouchEnter += target;
                }
            }
        }
        public void addTouchLeaveHandler(EventHandler<TouchEventArgs> target)
        {

            foreach (Square sqr in list)
            {
                if (sqr.Path != null && target != null)
                {
                    sqr.Path.TouchLeave += target;
                }
            }
        }
       
        IEnumerator IEnumerable.GetEnumerator() {
            return list.GetEnumerator();
        }

       
    }
}
