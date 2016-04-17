using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class Dot:Point
    {
       
        private int height = 7;
        private int width = 7;
        public int Height { get { return height; } }
        public int Width { get { return width; } }
       
        
        public Dot(int coordX, int coordY, int dotWidth,int dotHeight):base(coordX,coordY)
        {

            this.height = dotHeight;
            this.width = dotWidth;
           
        }
        public Dot(int coordX, int coordY):base(coordX,coordY)
        {
           
        }

        
    }

    public class BigDot : Dot
    {
        public BigDot(int coordX, int coordY, int dotWidth, int dotHeight)
            : base(coordX, coordY,dotWidth,dotHeight)
        { }
    }
}
