using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
   public class Point
    {
       private int coordX;
       private int coordY;

       public int CoordX { get { return coordX; } set { coordX = value; } }
       public int CoordY { get { return coordY; } set { coordY = value; } }

       public Point(int coordX, int coordY)
       {
           this.coordX = coordX;
           this.coordY = coordY;
       }
      
       public static Point operator ! (Point p)
       {
           return new Point(-p.CoordX, -p.CoordY);
       }

      
       public override bool Equals(System.Object obj)
       {
           
           if (obj == null)
           {
               return false;
           }

          
           Point p = obj as Point;
           if ((System.Object)p == null)
           {
               return false;
           }

           
           return (CoordX == p.CoordX) && (CoordY == p.CoordY);
       }

       public bool Equals(Point p)
       {
          
           if ((object)p == null)
           {
               return false;
           }

           
           return (CoordX == p.CoordX) && (CoordY == p.CoordY);
       }

       public override int GetHashCode()
       {
           return CoordX ^ CoordY;
       }
    }
}
