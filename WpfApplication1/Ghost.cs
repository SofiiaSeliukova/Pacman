using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApplication1
{
    public enum GhostModes
    {
        Hunting = 1,
        Wandering = 2,
        Escaping = 3
    }
    public class Ghost:GameCharacter
    {
        private GhostModes currentMode;
        
        private Point targetPoint;
        
        public Point TargetPoint { get { return targetPoint; } set { targetPoint = value; } }
        
        public GhostModes CurrentMode { get { return currentMode; } set { currentMode = value; } }
        
        public IGhostsMovingAlgorithm algorithm;


        public Ghost(Point startingPoint, int width, int height, Field field)
            : base(startingPoint, width, height, field)
        {
            
        }

       
        public override void Move()
        {
           

            if (InTheGhostRoom())
            {
                GetOut();
                return;
            }

            // Tunnel
            if (CurrentPoint.CoordX == Field.Cols - 1 && Direction == Direction.Right)
            {
                CurrentPoint.CoordX = 0;


            }
            else if (CurrentPoint.CoordX == 0 && Direction == Direction.Left)
            {

                CurrentPoint.CoordX = Field.Cols - 1;


            }
            if (currentMode == GhostModes.Hunting)
            {
                algorithm.Hunt(this);
                return;
            }
            else if (currentMode == GhostModes.Wandering)
            {
                algorithm.Wander(this);
                return;
            }
            else if (currentMode == GhostModes.Escaping)
            {
                algorithm.Escape(this);
                return;
            }
            
                       
        }

       private void GetOut()
        {
            Direction = Direction.Top;
            MoveOnNextPoint(Direction);
            if (!InTheGhostRoom())
            {
                Field.InTheGhostRoom--;
                if (Field.InTheGhostRoom == 0)
                {
                    Field.CloseGhostRoom();

                }
            }

        }

        private bool InTheGhostRoom()
        {
           return Field.GhostRoom.Contains(this);
        }
      
    


    }
}
