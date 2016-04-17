using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class GhostRoom
    {
        private int coordX;
        private int coordY;
        private int width;
        private int height;

        public int CoordX { get { return coordX; } }
        public int CoordY { get { return coordY; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public GhostRoom(int coordX, int coordY, int width, int height)
        {
            this.coordX = coordX;
            this.coordY = coordY;
            this.width = width;
            this.height = height;
        }

        public GhostRoom()
        {
            this.coordX = 8;
            this.coordY =8;
            this.width = 4;
            this.height = 4;
        }

        public bool Contains(Ghost ghost)
        {
            Point p = ghost.CurrentPoint;
            if (p.CoordX > this.CoordX && p.CoordY >= this.CoordY && p.CoordX < this.CoordX + this.Width && p.CoordY < this.CoordY + this.Height)
                return true;
            else return false;
        }
    }
}
