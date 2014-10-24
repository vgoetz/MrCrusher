using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Drawable;
using MrCrusher.Framework.SDL;
using SdlDotNet.Core;

namespace MrCrusher.Framework.BaseObjects {

    [DataContract]
    [KnownType(typeof(Weapon))]
    [KnownType(typeof(Soldier))]
    [KnownType(typeof(Tank))]
    [KnownType(typeof(Bunker))]
    [KnownType(typeof(Rifle))]
    public abstract class RotatingObject : MovingObject, IRotatingObject {
        private Degree _orientationInDegrees;

        protected RotatingObject(bool isControledByHumanPlayer, string imageFileName, string videoFileName) 
            : base(isControledByHumanPlayer, imageFileName, videoFileName) {
            
            InitComponents(0);
        }

        protected RotatingObject(bool isControledByHumanPlayer, string imageFileName, string videoFileName, int health) 
            : base(isControledByHumanPlayer, imageFileName, videoFileName, health) {

            InitComponents(0);
        }

        protected RotatingObject(bool isControledByHumanPlayer, string imageFileName, string videoFileName, int health, Point positionCenter) 
            : base(isControledByHumanPlayer, imageFileName, videoFileName, health, positionCenter) {

            InitComponents(0);
        }

        private void InitComponents(int orientationInDegrees) {
            OrientationInDegrees = orientationInDegrees;
            DestinationRotationForStepwiseRotation = 0;
        }

        [DataMember]
        public Degree OrientationInDegrees {
            get { return _orientationInDegrees; }
            protected set {
                //if (this is ITank && value%22.5 != 0) {
                //    ;
                //}
                _orientationInDegrees = value;
            }
        }

        [DataMember]
        public Degree RotationDegreesStep { get; protected set; }
        [DataMember]
        public Degree DestinationRotationForStepwiseRotation { get; protected set; }
        public bool PendingRotationToDo { get{
            return Math.Abs(DestinationRotationForStepwiseRotation - OrientationInDegrees) > 1; 
        } }
        public bool HasAlreadyRotated { get; protected set; }

        public virtual bool PendingRotation() {

            // Should I rotate?
            if (!Dead && PendingRotationToDo) {

                var newOrientation = Calculator.GetNewDegreeForStepwiseRotation(OrientationInDegrees, DestinationRotationForStepwiseRotation, RotationDegreesStep);
                RotateInstantly(newOrientation);

                return true;
            }

            return false;
        }

        public virtual void MakeReadyForRotation() {
            HasAlreadyRotated = false;
        }


        public abstract void RotateToMousePosition(Point destination);

        /// <param name="degrees"></param>
        public void RotateInstantly(Degree degrees) {

            OrientationInDegrees = degrees;
        }

        public void RotateInstantly(Point destination) {
            var newDegree = Calculator.CalculateDegree(PositionCenter, destination);
            RotateInstantly(newDegree);
            SetDestinationRotationForStepwiseRotation(newDegree);
        }

        public void SetDestinationRotationForStepwiseRotation(Point destinationPoint) {
            DestinationRotationForStepwiseRotation = Calculator.CalculateDegree(PositionCenter, destinationPoint);
        }

        public void SetDestinationRotationForStepwiseRotation(Degree destinationDegree) {
            DestinationRotationForStepwiseRotation = destinationDegree;
        }

        public override IEnumerable<Sprite> GetCurrentSprites() {
            var sprites = base.GetCurrentSprites().ToList();

            foreach (var sprite in sprites) {
                
                var rotatedSurface = ImageHelper.CreateRotatedSurface(sprite.Surface, OrientationInDegrees);

                // update sprite 
                sprite.Surface = rotatedSurface;
                sprite.Infos.Orientation = OrientationInDegrees;
                sprite.Infos.SurfacePositionTopLeftX = SurfacePositionTopLeft.X;
                sprite.Infos.SurfacePositionTopLeftY = SurfacePositionTopLeft.Y;
                sprite.Infos.SurfacePositionCenterX = PositionCenter.X;
                sprite.Infos.SurfacePositionCenterY = PositionCenter.Y;
            }

            return sprites;
        }

        public virtual bool MoveForward() {
            if (MaxForwardSpeed < 1 && HasAlreadyMoved) {
                return false;
            }

            CurrentSpeed = Calculator.GetAccelratedSpeed(CurrentSpeed, Accelration, MaxForwardSpeed);

            if (Route.Count == 0) {
                Route = Calculator.CalculateRoute(PositionCenter, 8, OrientationInDegrees);
            }

            return PendingMove();
        }

        public bool MoveBackwards() {
            if (MaxBackwardsSpeed < 1 && HasAlreadyMoved) {
                return false;
            }

            var newSpeed = Calculator.GetAccelratedSpeed(CurrentSpeed, Accelration, MaxBackwardsSpeed);

            if (Route.Count == 0) {
                Route = Calculator.CalculateRoute(PositionCenter, 5, OrientationInDegrees + 180);
            }

            var moved = PendingMove();

            if (moved || PositionCenter.Equals(NextPosition)) {
                CurrentSpeed = newSpeed;
            }

            return moved;
        }

        public IGameObject TargetedGameObject { get; set; }
    }
}
