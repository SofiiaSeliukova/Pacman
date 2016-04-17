using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public enum FruitType
    {
        Cherry = 0,
        Strawberry = 1,
        Orange = 2,
        Apple = 3,
        Melon = 4
    }
    public class Level
    {
       
		private FruitType	fruit;
		private int			levelNumber;

		public Level(int LevelNumber, FruitType Fruit)
		{
            this.levelNumber = LevelNumber;
			this.fruit = Fruit;
			
		}

		

		public int LevelNumber
		{
			get {return levelNumber;}
			set {levelNumber = value;}
		}


		public FruitType Fruit
		{
			get {return fruit;}
			set {fruit = value;}
		}
    }
}
