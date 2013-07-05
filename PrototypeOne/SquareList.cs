using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace PrototypeOne
{
    public class SquareList : IEnumerable
    {
       
        
        private List<Square> list;
        public SquareList() {
            list = new List<Square>();
        }
      /*  public Square Get(int x, int y) {

            return (Square)list[0];//later return the square containing x,y
        }*/
        public Square Get(int i)
        {
            return (Square)list[i];
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

        IEnumerator IEnumerable.GetEnumerator() {
            return list.GetEnumerator();
        }
    }
}
