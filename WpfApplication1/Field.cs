using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data.Entity;

namespace WpfApplication1
{
    
    public class Field : INotifyPropertyChanged
    {
        private int rows;
        private int cols;
        private byte[,] matrixforWalls;
        private List<Dot> dots;
        private List<Point> path;
        private List<Point> ghostPath;
        private GhostRoom ghostRoom;
        private int inTheGhostRoom;
       
        private Pacman pacman;
        private Ghost inky;
        private Ghost pinky;
        private Ghost blinky;
        private Ghost clyde;
        private Player currentPlayer;
        private Hashtable levels;
        private Random rnd = new Random();

        public byte[,] MatrixForWalls { get { return matrixforWalls; } }
        public Ghost Inky { get { return inky; } }
        public Ghost Pinky { get { return pinky; } }
        public Ghost Blinky { get { return blinky; } }
        public Ghost Clyde { get { return clyde; } }
        public Pacman Pacman{get{return pacman;}}
        public GhostRoom GhostRoom {get{return ghostRoom;}}
        public int Rows { get { return rows; } }
        public int Cols { get { return cols; } }
        
        public List<Dot> Dots { get { return dots; } }
        public List<Point> Path{get{return path;}}
        public List<Point> GhostPath { get { return ghostPath; }  }
        
        public int InTheGhostRoom { get { return inTheGhostRoom; } set { if(value<=3 && value>=0)inTheGhostRoom = value; } }
        
        public event PropertyChangedEventHandler PropertyChanged;
       
        public bool IsEscapingMode { get; set; }
       
        public Field(int rows, int cols, string playerName)
        {
            this.rows = rows;
            this.cols = cols;
            levels = CreateLevels();
            dots = new List<Dot>();
            matrixforWalls = new byte[rows, cols];
            this.ghostRoom = new GhostRoom();
            inTheGhostRoom = 3;
            currentPlayer = new Player(playerName);
            currentPlayer.CurrentLevel = NextLevel();
            this.IsEscapingMode = false;
            currentPlayer.PropertyChanged += currentPlayer_PropertyChanged;
            
        }

        private void gameCharacter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged(sender, e);
        }

        void currentPlayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged(sender, e);
           
        }

       

        public void Add(Dot dot)
        {
            if (dots.Find(x => x.CoordY == dot.CoordY && x.CoordX == dot.CoordX) == null)
            {
                dots.Add(dot);
            }
        
        }
       
        public void RemoveDots(Dot d)
        {
            if (dots.Count > 1)
            {
                Dot testdot = dots.Find(x => x.CoordY == d.CoordY && x.CoordX == d.CoordX);
                if (testdot != null)
                {
                    if (testdot as BigDot == null)
                    {
                        if (testdot as Fruit != null)
                        {
                            currentPlayer.FreeLives += 1;
                        }
                        else
                        currentPlayer.Score += 10;
                    }
                    else
                    {
                        currentPlayer.Score += 20;
                        PropertyChanged(this, new PropertyChangedEventArgs("Mode"));
                    }

                    dots.Remove(testdot);

                }
            }
            else 
            {
                
                currentPlayer.CurrentLevel = NextLevel();
                PropertyChanged(this, new PropertyChangedEventArgs("Level"));
            }

        }
       
       

        public Ghost IsPacmanCollidedGhost()
        {
            Point PacmanCurrentPoint = Pacman.CurrentPoint;

            if (Blinky.CurrentPoint.Equals(PacmanCurrentPoint) && Blinky.Visible)
                return Blinky;
            else if (Inky.CurrentPoint.Equals(PacmanCurrentPoint) && Blinky.Visible)
                return Inky;
            else if (Pinky.CurrentPoint.Equals(PacmanCurrentPoint) && Blinky.Visible)
                return Pinky;
            else if (Clyde.CurrentPoint.Equals(PacmanCurrentPoint) && Blinky.Visible)
                return Clyde;
            else return null;

        }
      
        public int getRandomInt(int min, int max)
        {
            return rnd.Next(min, max);
        }
       
        public void CollidedCheck()
        {
            if (IsPacmanCollidedGhost() != null)
            {
                if (IsEscapingMode)
                {
                    Ghost gc = IsPacmanCollidedGhost();
                    if (gc != null)
                    {
                        gc.Visible = false;
                        currentPlayer.Score += 200;
                    }
                  
                }

                else
                {
                    if (currentPlayer.Lives > 0)
                        currentPlayer.Lives--;
                    else if (currentPlayer.FreeLives > 0)
                        currentPlayer.FreeLives--;
                    Thread.Sleep(1000);
                
                }
            }
            
        }

        public void GenerateWalls()
        {
            Array.Clear(matrixforWalls, 0, matrixforWalls.Length);

            for (int i = 0; i < rows / 2 + 1; i += 2)
            {
                for (int j = i; j < cols - i; j++)
                {

                    matrixforWalls[i, j] = matrixforWalls[rows - 1 - i, j] =
                         matrixforWalls[j, i] = matrixforWalls[j, cols - i - 1] = 1;
                    if (i == j || i == cols - j - 1 || i == 0 || i == rows - 1)
                        matrixforWalls[i, j] = matrixforWalls[rows - i - 1, j] =
                            matrixforWalls[rows - i - 1, cols - j - 1] = 2;
                }

            }
            for (int i = 0; i < rows; i++)
            {
                matrixforWalls[i, 0] = matrixforWalls[i, cols - 1] = 2;
            }
            //ghost room
            matrixforWalls[8, 9] = matrixforWalls[8, 10] = matrixforWalls[10, 11] = matrixforWalls[9, 11] = matrixforWalls[11, 9] = matrixforWalls[11, 10] = 2;
          //in the ghost room
            matrixforWalls[9, 9] = matrixforWalls[10, 10] = matrixforWalls[9, 10] = matrixforWalls[10, 9] = matrixforWalls[9, 8] = matrixforWalls[10, 8] = 3;
            //tunnel
            matrixforWalls[0, 10] = matrixforWalls[cols - 1, 10] = 0;

        }
        public void GenerateMaze()
        {

            int count = getRandomInt(30, 40);
            int i, j;
            while (count > 0)
            {
                i = getRandomInt(0, rows);
                j = getRandomInt(0, cols);
                if (matrixforWalls[i, j] == 1)
                {
                    matrixforWalls[i, j] = 0;
                    if (matrixforWalls[i + 1, j] == 1) matrixforWalls[i + 1, j] = 2;
                    if (matrixforWalls[i, j + 1] == 1) matrixforWalls[i, j + 1] = 2;
                    if (matrixforWalls[i - 1, j] == 1) matrixforWalls[i - 1, j] = 2;
                    if (matrixforWalls[i, j - 1] == 1) matrixforWalls[i, j - 1] = 2;

                    count--;
                }
            }

        }
        public void GenerateDots()
        {
            dots.Clear();
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    if (matrixforWalls[i, j] == 0)
                        dots.Add(new Dot(i, j));
                }
            AddBigDots();
            AddFruits();
        }

        public void AddBigDots()
        {
            int index = 0;
            for (int i = 0; i < 4; i++)
            {
                index = getRandomInt(i, dots.Count);
                Dot dot = Dots[index];
                Dots[index] = new BigDot(dot.CoordX, dot.CoordY, 15, 15);
            }
        }

        public void AddFruits()
        {
            int index = 0;
            for (int i = 0; i < 2; i++)
            {
                index = getRandomInt(i, dots.Count);
                Dot dot = Dots[index];
                if (dot as BigDot == null)
                    Dots[index] = new Fruit(dot.CoordX, dot.CoordY, 15, 15, currentPlayer.CurrentLevel.Fruit);
                else i--;
            }
        }
        
        public void InitializeCharacters(IGhostsMovingAlgorithm algorithm = null)
        {
            Point startPoint = path[getRandomInt(1, path.Count)];
           
            pacman = new Pacman(this, startPoint, 20, 20);
            inky = new Ghost(new Point(ghostRoom.CoordX + 2, ghostRoom.CoordY+1 ), 20, 20, this);
            pinky = new Ghost(new Point(ghostRoom.CoordX + 2, ghostRoom.CoordY +2), 20, 20, this);
            blinky = new Ghost(new Point(ghostRoom.CoordX + 1, ghostRoom.CoordY - 1), 20, 20, this);
            clyde = new Ghost(new Point(ghostRoom.CoordX + 1, ghostRoom.CoordY + 1), 20, 20, this);

            pacman.PropertyChanged += gameCharacter_PropertyChanged;
            Inky.PropertyChanged += gameCharacter_PropertyChanged;
            Pinky.PropertyChanged += gameCharacter_PropertyChanged;
            Blinky.PropertyChanged += gameCharacter_PropertyChanged;
            Clyde.PropertyChanged += gameCharacter_PropertyChanged;


            if (algorithm == null)
            {
                inky.algorithm = new InkyMoveAlgorithm();
                pinky.algorithm = new PinkyMoveAlgorithm();
                blinky.algorithm = new BlinkyMoveAlgorithm();
                clyde.algorithm = new ClydeMoveAlgorithm();

            }
            else 
            {
                inky.algorithm = algorithm;
                pinky.algorithm = algorithm;
                blinky.algorithm = algorithm;
                clyde.algorithm = algorithm;
            }
            
           
            

            WanderingMode();
        }

        public void InitializePath()
        {
            int dotscount = dots.Count;
            path = new List<Point>(dotscount);

            for (int i = 0; i < dotscount; i++)
            {
                path.Add((Point)dots[i]);
            }
        }
        
        public void InitializeGhostPath()
        {
            
            ghostPath = new List<Point>();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
               if (matrixforWalls[i, j] == 0 || matrixforWalls[i, j] == 3)
                 ghostPath.Add(new Point(i,j));
                }
                   
            }
        }
        
       
        public bool CanMove(Point p)
        {
          return path.Find(x => x.CoordX == p.CoordX && x.CoordY == p.CoordY) != null;
        }
        
        public bool CanMove(Point p, Direction dir)
        { 
            int coordx = p.CoordX;
            int coordy = p.CoordY;
            switch (dir) { 
                case Direction.Left:
            {
                coordx--;
                break;
            }
                case Direction.Right:
            {
                coordx++;
                break;
            }
                case Direction.Top:
            {
                coordy--;
                break;
            }
                case Direction.Bottom:
            {
                coordy++;
                break;
            }
        }
            return CanMove(new Point(coordx, coordy));
        }

        public bool CanGhostMove(Point pntCheck)
        {
            return ghostPath.Find(x => x.CoordX == pntCheck.CoordX && x.CoordY == pntCheck.CoordY) != null;
        }

        public void CloseGhostRoom()
        {
            ghostPath = path; 
        }

        public void HuntingMode()
        {
            this.IsEscapingMode = false;
            this.Inky.Visible = this.Pinky.Visible = this.Blinky.Visible = this.Clyde.Visible = true;
            Inky.CurrentMode = GhostModes.Hunting;
            Blinky.CurrentMode = GhostModes.Hunting;
            Pinky.CurrentMode = GhostModes.Hunting;
            Clyde.CurrentMode = GhostModes.Hunting;
        }

        public void WanderingMode()
        {
            Inky.CurrentMode = GhostModes.Wandering;
            Inky.currentState = CharacterState.Stopped;
            Blinky.CurrentMode = GhostModes.Wandering;
            Blinky.currentState = CharacterState.Stopped;
            Pinky.CurrentMode = GhostModes.Wandering;
            Pinky.currentState = CharacterState.Stopped;
            Clyde.CurrentMode = GhostModes.Wandering;
            Clyde.currentState = CharacterState.Stopped;
        }

        public void EscapingMode()
        {
            this.IsEscapingMode = true;
            Inky.CurrentMode = GhostModes.Escaping;
            Blinky.CurrentMode = GhostModes.Escaping;
            Pinky.CurrentMode = GhostModes.Escaping;
            Clyde.CurrentMode = GhostModes.Escaping;
        }

        public Level NextLevel()
        {
            Level level = null;
            int intlevel = 1;

            if (currentPlayer == null)
            {
                level = (Level)levels[1];
                level.LevelNumber = intlevel;

            }
            else if (currentPlayer.CurrentLevel == null)
            {
                level = (Level)levels[1];
                level.LevelNumber = intlevel;
            }
            else
            {
                intlevel = currentPlayer.CurrentLevel.LevelNumber;
                intlevel++;
                if ((Level)levels[intlevel] == null)
                {
                    level = (Level)levels[10];
                }
                else
                {
                    level = (Level)levels[intlevel];
                }
                level.LevelNumber = intlevel;
            }
            return level;
        }

        private Hashtable CreateLevels()
        {
            Hashtable levels = new Hashtable();
            levels.Add(1, new Level(1, FruitType.Cherry));
            levels.Add(2, new Level(2, FruitType.Cherry));
            levels.Add(3, new Level(3, FruitType.Strawberry));
            levels.Add(4, new Level(4, FruitType.Strawberry));
            levels.Add(5, new Level(5, FruitType.Orange));
            levels.Add(6, new Level(6, FruitType.Orange));
            levels.Add(7, new Level(7, FruitType.Apple));
            levels.Add(8, new Level(8, FruitType.Apple));
            levels.Add(9, new Level(9, FruitType.Melon));
            levels.Add(10, new Level(10, FruitType.Melon));
         

            return levels;
        }

        public void MoveCharacters()
        {
           CollidedCheck();
           Pacman.Move();
           Blinky.Move();
           Inky.Move();
           if (InTheGhostRoom <= 2)
               Pinky.Move();
           if (InTheGhostRoom <= 1)
               Clyde.Move();
           
        }

        public int GetLives()
        {
            return currentPlayer.Lives;
        }

        public int GetScore()
        {
            return currentPlayer.Score;
        }

        public void ResetCharacter()
        {
            this.Pacman.MoveToStartingPoint();
            this.Inky.MoveToStartingPoint();
            this.Pinky.MoveToStartingPoint();
            this.Blinky.MoveToStartingPoint();
            this.Clyde.MoveToStartingPoint();
            inTheGhostRoom = 3;
            WanderingMode();
            
        }

        public int GetLevelNumber()
        {
            return currentPlayer.CurrentLevel.LevelNumber;
        }
        public FruitType GetLevelFruitType ()
        {
            return currentPlayer.CurrentLevel.Fruit;
        }

        public string GetPlayerName()
        {
            return currentPlayer.Name;
        }
    }
}
