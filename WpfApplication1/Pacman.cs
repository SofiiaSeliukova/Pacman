using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class Pacman : GameCharacter
    {

        public Pacman(Field field, Point currentPoint, int width, int height)
            : base(currentPoint, width, height,field)
		{
			
		}

        public override void Move()
        {
            base.Move();

        }
    }
}
