using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class Fruit:Dot
    {
        private FruitType type;
        public FruitType Type { get { return type; } }

        public Fruit(int coordX, int coordY, int width, int heigth, FruitType type)
            : base(coordX, coordY, width, heigth)
        {
            this.type = type;
        }
    }
}
