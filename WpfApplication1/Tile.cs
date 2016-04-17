using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApplication1
{
    public class Tile : Point
    {
        private int tileWidth = 20;
        private int tileHeight = 20;
      
        public int TileWidth
        {
            get { return tileWidth; }
            
        }

        public int TileHeight
        {
            get { return tileHeight; }

        }

        public Tile(int tileWidth, int tileHeight, int coordX, int coordY)
            : base(coordX, coordY)
        {
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
          
        }

        public Tile(int coordX, int coordY)
            : base(coordX, coordY)
        {
          
            
        }

       

       

    }
}
