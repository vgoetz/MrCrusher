using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Game.Environment;

namespace MrCrusher.Framework.BaseObjects
{
    [DataContract]
    [KnownType(typeof(RotatingObject))]
    [KnownType(typeof(Soldier))]
    [KnownType(typeof(Weapon))]
     [KnownType(typeof(Tank))]
     [KnownType(typeof(Bunker))]
     [KnownType(typeof(Rifle))]
    [KnownType(typeof(Projectile))]
     [KnownType(typeof(RifleShot))]
     [KnownType(typeof(TankShot))]
    public abstract class MovingObject : GameObject {

        protected MovingObject(bool isControledByHumanPlayer, string imageFileName, string videoFileName)
            : this(isControledByHumanPlayer, imageFileName, videoFileName, 1) {
            InitComponents(0);
        }

        protected MovingObject(bool isControledByHumanPlayer, string imageFileName, string videoFileName, int health) 
            : base(isControledByHumanPlayer, imageFileName, videoFileName) {
            InitComponents(health);
        }

        protected MovingObject(bool isControledByHumanPlayer, string imageFileName, string videoFileName, int health, Point positionCenter)
            : base(isControledByHumanPlayer, imageFileName, videoFileName, positionCenter) {
            InitComponents(health);
        }

        private void InitComponents(int health) {
            MaxHealth = health;
            Health = health;
            MaxForwardSpeed = 0.0;
            MaxBackwardsSpeed = 0.0;
            CurrentSpeed = 1.0;
            Accelration = 1.0;

            Route = new Queue<Point>();
        }

        public double MaxForwardSpeed { get; protected set; }
        public double MaxBackwardsSpeed { get; protected set; }
        public double CurrentSpeed { get; protected set; }
        public double Accelration { get; protected set; }
        
        public bool PendingMoveToDo { get{
            return Route != null
                   && Route.Count > 0
                   && PositionCenter.Equals(Route.Peek()) == false
                   && PositionCenter.Equals(DestinationPoint) == false
                   && HasAlreadyMoved == false;
        } }

        public Point NextPosition {
            get {
                if (Route.Count > 0) {
                    return Route.Peek();
                }
                return Point.Empty;
            }
        }

        public Point DestinationPoint { get; private set; }
        public bool HasAlreadyMoved { get; protected set; }
        public Queue<Point> Route { get; protected set; }

        public void Stop(){
            Accelration = 1.0;
            CurrentSpeed = 1.0;
            Route.Clear();
        }

        public void SetMovingDestination(int x, int y) {
            SetMovingDestination(new Point(x, y));
        }

        public void SetMovingDestination(Point destination){
            DestinationPoint = destination;
            Route = Calculator.CalculateRoute(PositionCenter, destination);
        }

        public virtual bool PendingMove(){
            // Should I move?)
            if (PendingMoveToDo && CurrentSpeed > 0) {

                var nextPosition = Point.Empty;
                for (int i = 1; i <= CurrentSpeed && Route.Count > 0; i++) {
                    nextPosition = Route.Dequeue();
                }

                return MoveToPosition(nextPosition);
            }

            return false;
        }

        private bool MoveToPosition(Point destination)
        {
            if (destination.Equals(PositionCenter) || HasAlreadyMoved) {
                return false;
            }

            // Move Tank to new destination
            PositionCenter = destination;
            HasAlreadyMoved = true;
            return true;
        }

        public virtual void MakeReadyForMotion(){
            HasAlreadyMoved = false;
        }

        public bool IsPointReachable(Point position) {
            // Validation if it´s human player`s object

            if (IsControlledByHumanPlayer) {
                // The player is not allowed to go out of screen borders
                if (position.X < 0 + GetCurrentSurface().Width / 2 ||
                    position.Y < 0 + GameEnv.TopMenuHeight + GetCurrentSurface().Height / 2 ||
                    position.X > GameEnv.ScreenWidth - GetCurrentSurface().Width / 2 ||
                    position.Y > GameEnv.ScreenHeight - GetCurrentSurface().Height / 2) {
                    return false;
                }
            }

            var newRectangleForCollisionDetection = RectangleForCollisionDetection;
            newRectangleForCollisionDetection.Offset(NextPosition.X - PositionCenter.X, NextPosition.Y - PositionCenter.Y);

            var listOfCollisions = CollisionDetection.CollisionDetectionForSpecificObject(this, newRectangleForCollisionDetection);

            if (listOfCollisions != null && listOfCollisions.Any()) {
                foreach (var collision in listOfCollisions) {
                    return !CollisionHandler.CollisionHandlingFor2Objects(this, collision);
                }
            } 
            
            return true;
        }

        public override void Die() {
            base.Die();
            CurrentSpeed = 0.0;
            Accelration = 0.0;
        }
        
        
    }
}
