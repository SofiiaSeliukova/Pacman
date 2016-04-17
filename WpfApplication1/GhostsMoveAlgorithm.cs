using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfApplication1
{
    public abstract class GhostMoveAlgorithm
{
    protected ArrayList GetNextPossibleCoordinates(Direction Direction, Point CurrentPoint, Ghost ghost)
        {
            int x = CurrentPoint.CoordX;
            int y = CurrentPoint.CoordY;

            ArrayList pts = new ArrayList();
            Point pntCheck;

            switch (Direction)
            {
                case Direction.Right:
                    
                    pntCheck = new Point(x + 1, y);
                    if (ghost.Field.CanGhostMove(pntCheck)) pts.Add(pntCheck);

                   
                    pntCheck = new Point(x, y - 1);
                    if (ghost.Field.CanGhostMove(pntCheck)) pts.Add(pntCheck);

                   
                    pntCheck = new Point(x, y + 1);
                    if (ghost.Field.CanGhostMove(pntCheck)) pts.Add(pntCheck);

                    break;

                case Direction.Left:

                   
                    pntCheck = new Point(x - 1, y);
                    if (ghost.Field.CanGhostMove(pntCheck)) pts.Add(pntCheck);

                    
                    pntCheck = new Point(x, y - 1);
                    if (ghost.Field.CanGhostMove(pntCheck)) pts.Add(pntCheck);

                    pntCheck = new Point(x, y + 1);
                    if (ghost.Field.CanGhostMove(pntCheck)) pts.Add(pntCheck);

                    break;

                case Direction.Bottom:

                    
                    pntCheck = new Point(x + 1, y);
                    if (ghost.Field.CanGhostMove(pntCheck)) pts.Add(pntCheck);

                   
                    pntCheck = new Point(x, y + 1);
                    if (ghost.Field.CanGhostMove(pntCheck)) pts.Add(pntCheck);

                    
                    pntCheck = new Point(x - 1, y);
                    if (ghost.Field.CanGhostMove(pntCheck)) pts.Add(pntCheck);

                    break;

                case Direction.Top:

                   
                    pntCheck = new Point(x + 1, y);
                    if (ghost.Field.CanGhostMove(pntCheck)) pts.Add(pntCheck);

                    
                    pntCheck = new Point(x - 1, y);
                    if (ghost.Field.CanGhostMove(pntCheck)) pts.Add(pntCheck);

                    
                    pntCheck = new Point(x, y - 1);
                    if (ghost.Field.CanGhostMove(pntCheck)) pts.Add(pntCheck);

                    break;
            }
            return pts;
        }
    protected ArrayList GetAllPossibleCoordinates(Point CurrentPoint, Ghost ghost)
    {
        int x = CurrentPoint.CoordX;
        int y = CurrentPoint.CoordY;

        ArrayList pts = new ArrayList();
        Point pntCheck;

        
                pntCheck = new Point(x + 1, y);
                if (ghost.Field.CanGhostMove(pntCheck)) pts.Add(pntCheck);

                pntCheck = new Point(x - 1, y);
                if (ghost.Field.CanGhostMove(pntCheck)) pts.Add(pntCheck);

                
                pntCheck = new Point(x, y - 1);
                if (ghost.Field.CanGhostMove(pntCheck)) pts.Add(pntCheck);

                
                pntCheck = new Point(x, y + 1);
                if (ghost.Field.CanGhostMove(pntCheck)) pts.Add(pntCheck);

               

       
        return pts;
    }
    protected Point FindNearestPoint(ArrayList pts,Ghost ghost)
    {
        Point nearestPoint = null;
        int distance, minDistance = 1000;
        foreach (Point p in pts)
        {
            distance = FindDistance(p, ghost.TargetPoint);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPoint = p;
            }
        }
        return nearestPoint;
    }
    protected Point FindNearestPointNotPrevious (ArrayList pts, Ghost ghost)
    {
        Point nearestPoint = null;
        int distance, minDistance = 1000;
        foreach (Point p in pts)
        {
            if (!p.Equals(ghost.PreviousPoint))
            {
                distance = FindDistance(p, ghost.TargetPoint);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPoint = p;
                }
            }
        }
        return nearestPoint;
    }
    protected void GetDirection(Point p, Ghost ghost)
    {
        if (ghost.CurrentPoint.CoordY < p.CoordY)
            ghost.AttemptedDirection = Direction.Bottom;
        else if (ghost.CurrentPoint.CoordX < p.CoordX)
            ghost.AttemptedDirection = Direction.Right;
        else if (ghost.CurrentPoint.CoordX > p.CoordX)
            ghost.AttemptedDirection = Direction.Left;
        else if (ghost.CurrentPoint.CoordY > p.CoordY)
            ghost.AttemptedDirection = Direction.Top;


    }
    protected int FindDistance(Point p, Point targetPoint)
    {
        return (int)Math.Sqrt((targetPoint.CoordX - p.CoordX) * (targetPoint.CoordX - p.CoordX) + (targetPoint.CoordY - p.CoordY) * (targetPoint.CoordY - p.CoordY));
    }
    protected Point GetNextPoint(Direction dir, Point p, Ghost ghost, int steps)
    {
        Point nextPoint = p;
        for (int i = 0; i < steps; i++)
        {
            nextPoint = ghost.GetNextPoint(dir, nextPoint);
        }
        return nextPoint;

    }
    protected virtual void MoveToTargetPoint(Object objectToMove, Point targetPoint)
    {
        Ghost ghost = objectToMove as Ghost;
        if (ghost != null)
        {
            ghost.TargetPoint = targetPoint;

            GetDirection(ghost.TargetPoint, ghost);
            Point nextPoint = ghost.GetNextPoint(ghost.AttemptedDirection, ghost.CurrentPoint);
            if (ghost.AttemptedDirection != Direction.None && ghost.Field.CanGhostMove(nextPoint))
            {
                ghost.MoveOnNextPoint(ghost.AttemptedDirection);
            }
            else
            {
                ArrayList pts = GetNextPossibleCoordinates(ghost.PreviousDirection, ghost.CurrentPoint, ghost);
                Point nearestPoint = FindNearestPoint(pts, ghost);

                if (nearestPoint != null && !nearestPoint.Equals(ghost.PreviousPoint))
                {
                    GetDirection(nearestPoint, ghost);
                    ghost.MoveOnNextPoint(ghost.AttemptedDirection);
                }
                else
                {
                    pts = GetAllPossibleCoordinates(ghost.CurrentPoint, ghost);
                    nearestPoint = FindNearestPointNotPrevious(pts, ghost);
                    if (nearestPoint != null)
                    {
                        GetDirection(nearestPoint, ghost);

                        ghost.MoveOnNextPoint(ghost.AttemptedDirection);

                    }
                    else if (pts.Count == 1 && pts[0].Equals(ghost.PreviousPoint))
                    {
                        GetDirection((Point)pts[0], ghost);
                        ghost.MoveOnNextPoint(ghost.AttemptedDirection);
                    }

                }

            }
        }
        

    }
   
    protected void MoveRandomly(Object objectToMove)
    {
         Ghost ghost = objectToMove as Ghost;
         if (ghost != null)
         {
             ArrayList pts = GetAllPossibleCoordinates(ghost.CurrentPoint, ghost);
             int iSelected = ghost.Field.getRandomInt(1, pts.Count + 1);
             Point SelectedPoint = (Point)pts[iSelected - 1];
             this.MoveToTargetPoint(ghost, SelectedPoint);
         }
    }
}
    public class BlinkyMoveAlgorithm : GhostMoveAlgorithm, IGhostsMovingAlgorithm
    {
        void IGhostsMovingAlgorithm.Wander(Object ghost)
        {
            base.MoveToTargetPoint(ghost, new Point(20,-1));
        }

        void IGhostsMovingAlgorithm.Hunt(Object objectToMove)
        {
          Ghost ghost = objectToMove as Ghost;
        if (ghost != null)
        {
            base.MoveToTargetPoint(ghost,ghost.Field.Pacman.CurrentPoint);
        }
          
        }

        void IGhostsMovingAlgorithm.Escape(Object ghost)
        {
            base.MoveRandomly(ghost);
        }
    }

    public class PinkyMoveAlgorithm : GhostMoveAlgorithm,  IGhostsMovingAlgorithm
    {
       void IGhostsMovingAlgorithm.Wander(Object objectToMove)
        {
            base.MoveToTargetPoint(objectToMove, new Point(2, -1));
         }
        
       void IGhostsMovingAlgorithm.Hunt(Object objectToMove)
        {
            Ghost ghost = objectToMove as Ghost;
        if (ghost != null)
        {
            Point pacmanFuturePoint = GetNextPoint(ghost.Field.Pacman.Direction, ghost.Field.Pacman.CurrentPoint, ghost, 4);
            
            base.MoveToTargetPoint(ghost, pacmanFuturePoint);
        }
        }

       void IGhostsMovingAlgorithm.Escape(Object objectToMove)
        {
            base.MoveRandomly(objectToMove);
        }

       
    }

    public class InkyMoveAlgorithm : GhostMoveAlgorithm, IGhostsMovingAlgorithm
    {
         void IGhostsMovingAlgorithm.Wander(Object objectToMove)
        {

            base.MoveToTargetPoint(objectToMove, new Point(20, 21));
            
        }

        void IGhostsMovingAlgorithm.Hunt(Object objectToMove)
        {
            Ghost ghost = objectToMove as Ghost;
        if (ghost != null)
        {
            Point pacmanFuturePoint = GetNextPoint(ghost.Field.Pacman.Direction, ghost.Field.Pacman.CurrentPoint, ghost, 2);
            int blinkyDistance = FindDistance(ghost.Field.Blinky.CurrentPoint, pacmanFuturePoint);
            blinkyDistance *= 2;
            int newCoordX, newCoordY;
            int x = Math.Abs(ghost.Field.Blinky.CurrentPoint.CoordX - pacmanFuturePoint.CoordX);
            int y = Math.Abs(ghost.Field.Blinky.CurrentPoint.CoordY - pacmanFuturePoint.CoordY);
            if (ghost.Field.Blinky.CurrentPoint.CoordX < pacmanFuturePoint.CoordX)
            {
                newCoordX = pacmanFuturePoint.CoordX + x;
            }
            else
            {
                newCoordX = pacmanFuturePoint.CoordX - x;
            }
            if (ghost.Field.Blinky.CurrentPoint.CoordY < pacmanFuturePoint.CoordY)
            {
                newCoordY = pacmanFuturePoint.CoordY + y;
            }
            else
            {
                newCoordY = pacmanFuturePoint.CoordY - y;
            }
            base.MoveToTargetPoint(ghost, new Point(newCoordX,newCoordY));
        }
        }

       void IGhostsMovingAlgorithm.Escape(Object ghost)
        {
            base.MoveRandomly(ghost);
        }

       
    }

    public class ClydeMoveAlgorithm : GhostMoveAlgorithm, IGhostsMovingAlgorithm
    {
     void IGhostsMovingAlgorithm.Wander(Object objectToMove)
        {
            base.MoveToTargetPoint(objectToMove, new Point(0, 21));
           
        }

        void IGhostsMovingAlgorithm.Hunt(Object objectToMove)
        {
             Ghost ghost = objectToMove as Ghost;
             if (ghost != null)
             {
                 int distance = FindDistance(ghost.CurrentPoint, ghost.Field.Pacman.CurrentPoint);
                 if (distance > 8)
                 {
                     base.MoveToTargetPoint(ghost, ghost.Field.Pacman.CurrentPoint);
                 }
                 else
                 {
                     base.MoveToTargetPoint(ghost, new Point(0, 21));
                 }
             }
        }

        void IGhostsMovingAlgorithm.Escape(Object objectToMove)
        {
            base.MoveRandomly(objectToMove);
        }


    }
}
