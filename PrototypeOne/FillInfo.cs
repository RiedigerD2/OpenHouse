using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
namespace PrototypeOne
{
    public class FillInfo
    {
        Brush colour;
        string name;

        public Brush Brush
        {
            get { return colour; }
            private set { colour = value; }
        }
        public string Name
        {
            get { return name; }
            private set { name = value; }
        }

        public FillInfo(Brush value){
            colour=value;
        }
    }
}
