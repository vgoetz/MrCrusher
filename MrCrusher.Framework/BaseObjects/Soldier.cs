using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using MrCrusher.Framework.AI;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Drawable;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.MediaManagement;
using MrCrusher.Framework.SDL;
using SdlDotNet.Graphics;

namespace MrCrusher.Framework.BaseObjects {

    [DataContract]
    public class Soldier : RotatingObject, ISoldier {

        [DataMember]
        public PlayersSoldierType PlayersType { get; set; }
        [DataMember]
        public EnemysSoldierType EnemysType { get; set; }

        public int ViewRange { get; private set; }
        
        public EnterStatus CurrentEnterStatus { get; private set; }

        private ICanBeEntered _objectToEnterOrLeave;
        public ICanBeEntered ObjectToEnterOrLeave {
            get { return _objectToEnterOrLeave; }
        }

        private ICanBeEntered _enteredObject;
        public ICanBeEntered EnteredObject {
            get { return _enteredObject; }
        }


        public Weapon PrimaryWeapon { get; set; }
        public Weapon SecondaryWeapon { get; set; }

        public List<ITriggerAttackStrategy> TriggerAttackStrategies { get; private set;}
        public IAimStrategy AimStrategy { get; private set; }
        public IShootStrategy ShootStrategy { get; private set; }
        public IMoveStrategy MoveStrategy { get; private set; }

        private Surface _soldierRotatedSurface;
        public double WeaponRange { get; private set; }
        public double Accuracy { get; private set; }

        public Soldier(bool isControledByHumanPlayer, string imageFileName, string videoFileName, int health, Point positionCenter, int primaryWeaponPower, Rectangle rectangleForCollisionDetection, int weaponRange, int viewRange, double accuracy, int fireRate)
            : base(isControledByHumanPlayer, imageFileName, videoFileName, health, positionCenter) {
            InitComponents();

            PrimaryWeapon = new Rifle(null, null, weaponRange, accuracy, primaryWeaponPower, fireRate, this);
            SecondaryWeapon = new GranadeBelt(null, null, weaponRange - 20, accuracy - 0.50, 30, 10, this);

            RectangleForCollisionDetection = rectangleForCollisionDetection;
            WeaponRange = weaponRange;
            ViewRange = viewRange;
            Accuracy = accuracy;
            
            VideoCollection.Add(new VideoStartParameter("Soldat_stirbt.png", false, false, true, 2), false);

            TriggerAttackStrategies = new List<ITriggerAttackStrategy> {new TriggerAttackIfInViewRage(), new TriggerAttackAfterHit()};
            AimStrategy = new AimAtTargetWithRotationStepStrategy();
            ShootStrategy = new ShootAtTargetInWeaponRageStrategy();
            MoveStrategy = new MoveDirectlyAndCloseToTargetStrategy();
        }

        private void InitComponents(){
            SetDefaultSoldierValues();
            
            _soldierRotatedSurface = null;
        }

        private void SetDefaultSoldierValues() {
            Vulnerable = true;
            
            CurrentSpeed = 1;
            MaxForwardSpeed = 1;
            MaxBackwardsSpeed = 1;
            Accelration = 1.4;
            RotationDegreesStep = 15;
            DecayTimeSpanAfterDeath = new TimeSpan(hours: 0, minutes: 0, seconds: 30);
        }

        public void EnterObjectInFront() {
            var poi = Calculator.CalculateDestinationPoint(PositionCenter, 15, OrientationInDegrees);
            List<IGameObject> allObjectsInTheNear = MapHelper.GetGameObjectsInNearAreaOfPoint(poi, 30).ToList();
            if (allObjectsInTheNear.Contains(this)) {
                allObjectsInTheNear.Remove(this);
            }
            List<ICanBeEntered> allObjectsCouldBeEntered = allObjectsInTheNear.OfType<ICanBeEntered>().Where(obj => obj.IsManned == false).ToList();

            if (allObjectsCouldBeEntered.Any()) {
                var nearestGameObjectToEnter = MapHelper.GetNearestGameObject(allObjectsCouldBeEntered, PositionCenter);
                EnterObject(nearestGameObjectToEnter);
            }
        }

        private void EnterObject(ICanBeEntered objectToEnter) {

            if (CurrentEnterStatus != EnterStatus.Outside ||
                IsObjectToEnterInSight(objectToEnter) == false) {
                
                return;
            }

            _objectToEnterOrLeave = objectToEnter;
            CurrentEnterStatus = EnterStatus.OnTheWayToEnter;
            Route = Calculator.CalculateRoute(PositionCenter, objectToEnter.PositionCenter);
            OrientationInDegrees = Calculator.CalculateDegree(PositionCenter, objectToEnter.PositionCenter);
            GameObjectsInteractionDictionary.Add(objectToEnter, false);
            CanReseiveKeyCommands = false;
        }

        private bool IsObjectToEnterInSight(IGameObject gameObjectToEnter) {
            // Ist das Objekt in Reichweite?
            var distance = Calculator.CalculateDistance(PositionCenter, gameObjectToEnter.PositionCenter);
            var inSight = distance <= 22 && gameObjectToEnter.Dead == false;

            return inSight;
        }

        private void FinishEnteringMode() {
            _enteredObject = _objectToEnterOrLeave;
            _enteredObject.IsManned = true;
            _objectToEnterOrLeave = null;
            DestinationRotationForStepwiseRotation = OrientationInDegrees;
            PlayerAsController.TakeAdditionallyControllOf(_enteredObject);
            CurrentEnterStatus = EnterStatus.ObjectIsEntered;

            Visible = false;
            Vulnerable = false;
            CanInteractWithOthers = false;
        }

        public void LeaveObject() {
            if (CurrentEnterStatus != EnterStatus.ObjectIsEntered) {
                return;
            }

            Tuple<Point?, Degree> positionToLeave = GetPositionToLeave();
            if (positionToLeave == null || positionToLeave.Item1 == null) {
                return;
            }

            PositionCenter = _enteredObject.PositionCenter;
            OrientationInDegrees = positionToLeave.Item2;
            CurrentEnterStatus = EnterStatus.OnTheWayToLeave;

            Visible = true;
            Vulnerable = true;
            Route = Calculator.CalculateRoute(PositionCenter, positionToLeave.Item1.Value);
            OrientationInDegrees = Calculator.CalculateDegree(PositionCenter, positionToLeave.Item1.Value);
            CanInteractWithOthers = true;
            _objectToEnterOrLeave = _enteredObject;
            _enteredObject = null;
        }

        private void FinishLeavingMode() {
            CanReseiveKeyCommands = true;
            
            PlayerAsController.ReleaseAdditionallyControllOfAnObject();
            _objectToEnterOrLeave.IsManned = false;
            GameObjectsInteractionDictionary.Remove(_objectToEnterOrLeave);
            _objectToEnterOrLeave = null;

            CurrentEnterStatus = EnterStatus.Outside;
        }

        private Tuple<Point?, Degree> GetPositionToLeave() {
            // Wohin aussteigen
            Degree leavingOnTheLeftDegree = 0;
            Degree leavingOnTheRightDegree = 0;
            if (_enteredObject is ITank) {
                leavingOnTheLeftDegree = ((Tank) _enteredObject).OrientationInDegrees + 90;
                leavingOnTheRightDegree = ((Tank) _enteredObject).OrientationInDegrees - 90;
            }

            // Found new Position to leave the vehicle - first try
            Point newPositionToLeave = Calculator.CalculateDestinationPoint(_enteredObject.PositionCenter, 18, leavingOnTheRightDegree);

            if (IsPointReachable(newPositionToLeave)) {
                return new Tuple<Point?, Degree>(newPositionToLeave, leavingOnTheRightDegree);
            } else {
                // second try
                newPositionToLeave = Calculator.CalculateDestinationPoint(_enteredObject.PositionCenter, 18, leavingOnTheLeftDegree);
                if (IsPointReachable(newPositionToLeave)) {
                    return new Tuple<Point?, Degree>(newPositionToLeave, leavingOnTheLeftDegree);
                    ;
                }
            }

            // Aussteigen nicht möglich!
            return null;
        }

        public bool AI_TriggerAttackOnTarget(IGameObject target) {
            foreach (var triggerAttackStrategy in TriggerAttackStrategies) {
                var triggered = triggerAttackStrategy.AI_TriggerAttackOnTarget(this, target);
                if (triggered) {
                    return true;
                }
            }

            return false;
        }

        public void AI_SetAimTarget(IGameObject target) {
            AimStrategy.AI_AimAtObject(this, target);
        }

        public void AI_SetAimPosition(Point position) {
            AimStrategy.AI_AimAtPosition(this, position);
        }

        public void AI_Shoot(IGameObject target) {
            ShootStrategy.AI_ShootAtObject(this, target);
        }

        public void AI_SetMoveDestination(Point targetPoint) {
            MoveStrategy.AI_MoveToObject(this, targetPoint);
        }

        public override IEnumerable<Sprite> GetCurrentSprites() {
            
            var list = new List<Sprite>();

            // Create rotated soldier
            IEnumerable<Sprite> sprites = base.GetCurrentSprites();

            foreach (var sprite in sprites) {
                bool imageWasThereBefore = false;
                bool alphaBlending = false;
                byte alpha = 0;
                if (_soldierRotatedSurface != null) {
                    imageWasThereBefore = true;
                    // ... so adopt existing AlphaBlending config 
                    alphaBlending = _soldierRotatedSurface.AlphaBlending;
                    alpha = _soldierRotatedSurface.Alpha;
                }

                _soldierRotatedSurface = sprite.Surface;
                
                if (imageWasThereBefore) {
                    _soldierRotatedSurface.AlphaBlending = alphaBlending;
                    _soldierRotatedSurface.Alpha = alpha;
                    sprite.Surface.AlphaBlending = alphaBlending;
                    sprite.Surface.Alpha = alpha;
                    sprite.Infos.AlphaBlending = alphaBlending;
                    sprite.Infos.Alpha = alpha;
                }

                list.Add(sprite);
            }

            return list;
        }

        public bool ShootPrimaryWeapon() {
            return PrimaryWeapon.Shoot(CalculateStartPositionForNewShot(), CalculateDestinationForNewShot(), this);
        }

        public bool ShootSecondaryWeapon() {
            return SecondaryWeapon.Shoot(CalculateStartPositionForNewShot(), CalculateDestinationForNewShot(), this);
        }

        private Point CalculateStartPositionForNewShot(){
            return Calculator.CalculateDestinationPoint(PositionCenter, 7, OrientationInDegrees + 340);
        }

        private Point CalculateDestinationForNewShot(){
            // regard accuracy
            var maxDiff = WeaponRange - (WeaponRange * Accuracy);
            var rdn = new Random(DateTime.Now.Millisecond);
            var newRange = rdn.Next((int) (WeaponRange - maxDiff), (int) (WeaponRange + maxDiff));
            var newDegrees = OrientationInDegrees + rdn.Next((int) (-maxDiff), (int) maxDiff);

            Point destinationForNewShot = Calculator.CalculateDestinationPoint(CalculateStartPositionForNewShot(), newRange, newDegrees);
            return destinationForNewShot;
        }

        public override void RotateToMousePosition(Point destination) {
            RotateInstantly(destination);
        }

        public void RotateSoldierToTheLeftStepwise() {
            DestinationRotationForStepwiseRotation = OrientationInDegrees + RotationDegreesStep;
        }

        public void RotateSoldierToTheRightStepwise() {
            DestinationRotationForStepwiseRotation = OrientationInDegrees - RotationDegreesStep;
        }

        public void RotateSoldierToPointStepwise(Point destination) {
            DestinationRotationForStepwiseRotation = OrientationInDegrees = Calculator.CalculateDegree(PositionCenter, destination);
        }

        public override bool PendingMove() {

            // switch entering mode
            if (CurrentEnterStatus == EnterStatus.OnTheWayToEnter &&
                _objectToEnterOrLeave != null &&
                _objectToEnterOrLeave.PositionCenter == PositionCenter) {
                
                FinishEnteringMode();
            } else if (CurrentEnterStatus == EnterStatus.OnTheWayToLeave &&
                !Route.Any()) {
                FinishLeavingMode();
            }

            if (PendingMoveToDo && IsPointReachable(NextPosition)) {
                // Move Soldier to new destination
                return base.PendingMove();
            } else {
                if (HasAlreadyMoved) {
                    return false;
                }
                Stop();
                if (GetVideoName().Contains("Run")) {
                    VideoCollection.ActiveVideoPlayer.Stop();
                }
            }

            return false;
        }

        public void SlowDown(){
            if(CurrentSpeed > 1) {
                CurrentSpeed = Calculator.GetAccelratedSpeed(CurrentSpeed, 0.8, 10);
            }
            else {
                CurrentSpeed = 1;
            }
            
        }

        public override void Die(IGameObject killer) {
            SoundHandler.PlayRandomSoldierDiedSound();
            VideoCollection.SetActiveVideo("Soldat_stirbt.png");

            base.Die(killer);

            if (IsControlledByHumanPlayer == false) {
                SoundHandler.PlayRandomHahaAndQuotesSound();
                
                // TODO: inc. players body count
            }
        }

        public bool Decayed { get; set; }
        public TimeSpan? DecayTimeSpanAfterDeath { get; set; }

        public void Decay() {
            if (!Dead || Decayed || TimeOfDeath == null || DecayTimeSpanAfterDeath == null) {
                return;
            }

            TimeSpan? currentDiff = DateTime.Now - TimeOfDeath;

            if (currentDiff.HasValue &&  DecayTimeSpanAfterDeath.Value.Ticks > 0) {
                long proportionOfDecay = currentDiff.Value.Ticks * 100 / DecayTimeSpanAfterDeath.Value.Ticks;

                if (proportionOfDecay < 100) {
                    _soldierRotatedSurface.AlphaBlending = true;
                    _soldierRotatedSurface.Alpha = Convert.ToByte(255 - 150 * proportionOfDecay / 100);
                } else {
                    AfterDecay();
                }
            }
        }

        public void AfterDecay() {
            Decayed = true;
        }

        public void WasHit(int hitpoints, IGameObject shooter) {

            if(Dead || Vulnerable == false) {
                return;
            }

            Health = Health - hitpoints;

            if (Health <= 0) {
                Die(shooter);
            } else {
                SoundHandler.PlayRandomSoldierWasHitSound();
            }
        }
    }
}
