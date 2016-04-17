using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace WpfApplication1
{ 
	
    public enum CharacterState
	{
		Moving = 0,
		Stopped = 1,
	}
   

	
    public abstract class GameCharacter:INotifyPropertyChanged
    {
        protected Point currentPoint;
        protected Point previousPoint;
        protected Point startingPoint;
        protected int width;
        protected int height;
        protected Direction direction;
        protected Direction attemptedDirection;
        protected Direction previousDirection;
        protected bool visible;

        public CharacterState currentState;
        
        public Field Field {get; set;}

        public event PropertyChangedEventHandler PropertyChanged;

     
        public int Width
        {
            get { return width; }
            
        }

        public int Height
        {
            get { return height; }
           
        }

        public bool Visible
        {
            get { return visible; }
            set 
            {
                visible = value;
                PropertyChanged(this,new PropertyChangedEventArgs("Visible"));
            }
           
        }
        
        public Direction Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public Direction AttemptedDirection
        {
            get { return attemptedDirection; }
            set { attemptedDirection = value; }
            
        }

        public Direction PreviousDirection
        {
            get { return previousDirection; }
            set { previousDirection = value; }
        }

        public Point StartingPoint
        {
            get { return startingPoint; }
            
        }

        public Point CurrentPoint
        {
            get { return currentPoint; }
           
        }

        public Point PreviousPoint
        {
            get { return previousPoint; }
            
        }
        
       

       
        public GameCharacter(Point startingPoint, int width, int height, Field field )
        {
           
            this.previousPoint = this.currentPoint = this.startingPoint = startingPoint;
            this.width = width;
            this.height = height;
            this.Field = field;
            this.currentState = CharacterState.Stopped;
            this.direction = this.attemptedDirection = this.previousDirection = Direction.Left;
            this.visible = true;
           
        }

       
        public virtual void Move()
        {
            currentState = CharacterState.Stopped;

            // if in tunnel
            if (CurrentPoint.CoordX == Field.Cols - 1 && Direction == Direction.Right)
            {
               
               Field.RemoveDots(new Dot(CurrentPoint.CoordX, CurrentPoint.CoordY));
               CurrentPoint.CoordX = 0;
                Field.RemoveDots(new Dot(CurrentPoint.CoordX, CurrentPoint.CoordY));
                
            }
            else if (CurrentPoint.CoordX == 0 && Direction == Direction.Left)
            {
                
                Field.RemoveDots(new Dot(CurrentPoint.CoordX, CurrentPoint.CoordY));
                CurrentPoint.CoordX = Field.Cols - 1;
                Field.RemoveDots(new Dot(CurrentPoint.CoordX, CurrentPoint.CoordY));
                
            }

                if (AttemptedDirection != Direction.None && Field.CanMove(GetNextPoint(AttemptedDirection, CurrentPoint)))
                {
                    Field.RemoveDots(new Dot(CurrentPoint.CoordX, CurrentPoint.CoordY));
                    MoveOnNextPoint(AttemptedDirection);
                   
                    
                }
                else
                {
                    if (Direction != Direction.None && Field.CanMove(GetNextPoint(Direction, CurrentPoint)))
                    {
                        Field.RemoveDots(new Dot(CurrentPoint.CoordX, CurrentPoint.CoordY));
                        MoveOnNextPoint(Direction);
                       
                    }
                    else
                    {
                        PreviousDirection = Direction;
                        Direction = Direction.None;
                        currentState = CharacterState.Stopped;
                    }

                    
                }
}
       
        public void MoveOnNextPoint(Direction dir)
        {

                Point nextPoint = GetNextPoint(dir, CurrentPoint);

                previousPoint = currentPoint;

                currentPoint = nextPoint;

               PropertyChanged(this, new PropertyChangedEventArgs("currentPoint"));

                if (AttemptedDirection == dir)
                {
                    PreviousDirection = Direction;

                    Direction = AttemptedDirection;

                    AttemptedDirection = Direction.None;

                    currentState = CharacterState.Moving;
                }
                else
                {
                    PreviousDirection = Direction;
                    currentState = CharacterState.Moving;
                }
             
               
                return;
            }

        public void MoveToStartingPoint()
        {
            currentPoint = startingPoint;
        }

        public Point GetNextPoint(Direction direction, Point currentPoint)
        {
            int x = currentPoint.CoordX;
            int y = currentPoint.CoordY;

            switch (direction)
            {
                case Direction.Right:
                    x++;
                    break;
                case Direction.Left:
                    x--;
                    break;
                case Direction.Bottom:
                    y++;
                    break;
                case Direction.Top:
                    y--;
                    break;
            }
            return new Point(x, y);
        }

       
    
    }
}
