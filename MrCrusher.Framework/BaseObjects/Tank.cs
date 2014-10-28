using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using MrCrusher.Framework.AI;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.BaseObjectsFactories;
using MrCrusher.Framework.BaseObjectsFactories.Interfaces;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Drawable;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.MediaManagement;
using MrCrusher.Framework.SDL;
using MrCrusher.Framework.SpecialObjectsFactories;
using SdlDotNet.Graphics;

namespace MrCrusher.Framework.BaseObjects {

    [DataContract]
    public class Tank : Weapon, ITank {

        private const double TankPlatformRotationDegreeStep = 22.5;

        [DataMember]
        public PlayersTankType PlayersType = PlayersTankType.None;
        [DataMember]
        public EnemysTankType EnemysType = EnemysTankType.None;

        public string TowerImageName { get; protected set; }

        private Surface _platformRotatedSurface;
        private Surface _towerCurrentSurface;
        private Surface _towerRotatedSurface;

        private VideoPlayerCollection _towerDestroyedVideo;

        [DataMember]
        private Degree _towerOrientationInDegrees;
        [DataMember]
        public Degree TowerOrientationInDegrees { 
            get { return _towerOrientationInDegrees; }
            protected set { _towerOrientationInDegrees = value; } 
        }
        [DataMember]
        public Degree TowerRotationDegreesStep { get; private set; }
        [DataMember]
        public Degree TowerRotationDestination { get; protected set; }
        public bool PendingTowerRotationToDo {
            get {
                return Math.Abs(TowerRotationDestination - TowerOrientationInDegrees) > TowerRotationDegreesStep;
            }
        }
        public bool HasTowerAlreadyRotated { get; protected set; }

        public IExplosionCascadeFactory ExplosionCascadeFactory { get; private set; }

        public IShotFactory SecondaryShotFactory { get; set; }
        public bool IsManned { get; set; }
        
        public int ViewRange { get; private set; }

        public Weapon PrimaryWeapon { get; set; }
        public Weapon SecondaryWeapon { get; set; }
        
        public int SecondaryWeaoponPower;
        private readonly int _lengthOfCannonFromCenter;

        public List<ITriggerAttackStrategy> TriggerAttackStrategies { get; private set; }
        public IAimStrategy AimStrategy { get; private set; }
        public IShootStrategy ShootStrategy { get; private set; }
        public IMoveStrategy MoveStrategy { get; private set; }
        
        public Tank(bool isControledByHumanPlayer, string imageFileName, string tankTowerImageFileName, int health, Point positionCenter, int cannonLength, int primaryWeaponPower, Rectangle rectangleForCollisionDetection, int range, int viewRange, double accuracy, int fireRate)
            : base(isControledByHumanPlayer, imageFileName, null, health, positionCenter) {
            InitComponents();

            _towerCurrentSurface = ImageContainer.GetImage(tankTowerImageFileName);
            TowerImageName = tankTowerImageFileName;

            _lengthOfCannonFromCenter = cannonLength;
            Power = primaryWeaponPower;
            SecondaryWeaoponPower = 5;
            RectangleForCollisionDetection = rectangleForCollisionDetection;
            WeaponRange = range;
            ViewRange = viewRange;
            Accuracy = accuracy;
            FireRate = fireRate;

            PrimaryShotFactory = new TankShotFactory();
            SecondaryShotFactory = new RifleShotFactory();

            ExplosionCascadeFactory = new ExplosionCascadeFactory();

            Parent = this;
        }

        private void InitComponents(){
            SetDefaultTankValues();

            OrientationInDegrees = 0;
            TowerOrientationInDegrees = 0;
            TowerRotationDestination = 0;
            
            _platformRotatedSurface = null;
            _towerDestroyedVideo = new VideoPlayerCollection(new VideoStartParameter("TankTowerOnFire2.png", true, false, false, 4), true);

            TriggerAttackStrategies = new List<ITriggerAttackStrategy> { new TriggerAttackIfInViewRage(), new TriggerAttackAfterHit() };
            AimStrategy = new AimAtTargetWithRotationStepStrategy();
            ShootStrategy = new ShootAtTargetInWeaponRageStrategy();
            MoveStrategy = new MoveDirectlyAndCloseToTargetStrategy();
        }

        private void SetDefaultTankValues() {
            Vulnerable = true;

            PrimaryWeapon = this;
            SecondaryWeapon = new Rifle(null, null, WeaponRange, Accuracy, 10, 26, this);
            
            CurrentSpeed = 1;
            MaxForwardSpeed = 1;
            MaxBackwardsSpeed = 1;
            Accelration = 1.4;
            RotationDegreesStep = 1.5;
            TowerRotationDegreesStep = 5;
            IsManned = false;
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

            // Create rotated platform
            _platformRotatedSurface = ImageHelper.CreateRotatedSurface(GetCurrentSurface(), OrientationInDegrees);
            var platformSurfaceAndInfo = new Sprite(
                                            _platformRotatedSurface,
                                            new SpriteInfo {
                                                Orientation = OrientationInDegrees,
                                                Name = ImageName,
                                                SpriteType = SpriteType.Image,
                                                SurfacePositionTopLeftX = SurfacePositionTopLeft.X,
                                                SurfacePositionTopLeftY = SurfacePositionTopLeft.Y,
                                                SurfacePositionCenterX = PositionCenter.X,
                                                SurfacePositionCenterY = PositionCenter.Y
                                            });
            list.Add(platformSurfaceAndInfo);

            // Create rotated tower
            if (Dead) {
                Sprite towerSurfaceAndInfo = _towerDestroyedVideo.ActiveVideoPlayer.GetAndPlayVideoSprite();

                _towerRotatedSurface = ImageHelper.CreateRotatedSurface(towerSurfaceAndInfo.Surface, TowerOrientationInDegrees);
                towerSurfaceAndInfo.Surface = _towerRotatedSurface;
                towerSurfaceAndInfo.Infos.SurfacePositionTopLeftX = SurfacePositionTopLeft.X;
                towerSurfaceAndInfo.Infos.SurfacePositionTopLeftY = SurfacePositionTopLeft.Y;
                towerSurfaceAndInfo.Infos.SurfacePositionCenterX = PositionCenter.X;
                towerSurfaceAndInfo.Infos.SurfacePositionCenterY = PositionCenter.Y;
                TowerImageName = towerSurfaceAndInfo.Infos.Name;

                list.Add(towerSurfaceAndInfo);
            } else {
                _towerRotatedSurface = ImageHelper.CreateRotatedSurface(_towerCurrentSurface, TowerOrientationInDegrees);
                var towerSurfaceAndInfo = new Sprite(
                                            _towerRotatedSurface,
                                            new SpriteInfo {
                                                Orientation = TowerOrientationInDegrees,
                                                Name = TowerImageName,
                                                SpriteType = SpriteType.Image,
                                                SurfacePositionTopLeftX = SurfacePositionTopLeft.X,
                                                SurfacePositionTopLeftY = SurfacePositionTopLeft.Y
                                            });
                list.Add(towerSurfaceAndInfo);
            }
            
            return list;
        }

        public bool ShootPrimaryWeapon() {
            return Shoot(CalculateStartPositionForNewShotDepentsOnTower(), CalculateDestinationForNewShot(), this);
        }

        public bool ShootSecondaryWeapon() {
            return SecondaryWeapon.Shoot(CalculateStartPositionForNewShotDepentsOnTower(), CalculateDestinationForNewShot(), this);
        }
        
        public override bool Shoot(Point startPositionCenter, Point endPositionCenter, IGameObject shooter) {
            if (CanIShootNow()) {
                var shot = PrimaryShotFactory.CreateProjectile(startPositionCenter, endPositionCenter, Power, shooter);
                return Shoot(shot);
            }
            return false;
        }
        
        public Point CalculateStartPositionForNewShotDepentsOnTower(){
            return Calculator.CalculateDestinationPoint(PositionCenter, _lengthOfCannonFromCenter, TowerOrientationInDegrees);
        }

        private Point CalculateDestinationForNewShot(){
            // regard accuracy
            var maxDiff = WeaponRange - (WeaponRange * Accuracy);
            var rdn = new Random(DateTime.Now.Millisecond);
            var newRange = rdn.Next((int)(WeaponRange - maxDiff), (int)(WeaponRange + maxDiff));
            var newDegrees = TowerOrientationInDegrees + rdn.Next((int)(-maxDiff), (int)maxDiff);

            Point destinationForNewShot = Calculator.CalculateDestinationPoint(CalculateStartPositionForNewShotDepentsOnTower(), newRange, newDegrees);
            return destinationForNewShot;
        }

        public override void MakeReadyForMotion() {
            HasTowerAlreadyRotated = false;

            base.MakeReadyForMotion();
        }



        public override bool PendingRotation() {
            PendingTowerRotation();

            return base.PendingRotation();
        }

        public void SetTowerRotationDestination(Point destination) {
            TowerRotationDestination = Calculator.CalculateDegree(PositionCenter, destination);
        }

        public void RotateTowerToTheLeftInstantly(Degree degreesOffset) {
            RotateTowerOffset(degreesOffset);
        }

        public void RotateTowerToTheRightInstantly(Degree degreesOffset) {
            RotateTowerOffset(-degreesOffset);
        }

        public void RotateTowerOffset(Degree degreesOffset) {
            TowerOrientationInDegrees += degreesOffset;
            HasTowerAlreadyRotated = true;
        }

        public void RotateTowerToTheLeftInstantly() {
            HasTowerAlreadyRotated = true;
            TowerOrientationInDegrees += TowerRotationDegreesStep;
        }

        public void RotateTowerToTheRightInstantly() {
            HasTowerAlreadyRotated = true;
            TowerOrientationInDegrees -= TowerRotationDegreesStep;
        }

        public override void RotateToMousePosition(Point destination) {
            TowerOrientationInDegrees = Calculator.CalculateDegree(PositionCenter, destination);
            RotateTowerInstantly(TowerOrientationInDegrees);
            TowerRotationDestination = TowerOrientationInDegrees;
        }
        
        public void RotateTowerInstantly(Degree degree) {
            HasTowerAlreadyRotated = true;
            TowerOrientationInDegrees = degree;
        }
        
        public void RotatePlatformToTheLeft() {
            if(HasAlreadyRotated == false && PendingRotationToDo == false) {
                DestinationRotationForStepwiseRotation = OrientationInDegrees + TankPlatformRotationDegreeStep;
            }
        }

        public void RotatePlatformToTheRight() {
            if (HasAlreadyRotated == false && PendingRotationToDo == false) {
                DestinationRotationForStepwiseRotation = OrientationInDegrees - TankPlatformRotationDegreeStep;
            }
        }

        public bool CanDriveThereWithoutRotationForAiPlayer() {
            if (IsControlledByHumanPlayer) {
                return true;
            }

            var degreeToNextPosition = Calculator.CalculateDegree(PositionCenter, DestinationPoint);
            var degreeDiffToNextPosition = Calculator.CalculateDegreeDifferenceBetweenToDegrees(OrientationInDegrees, degreeToNextPosition);

            if (Math.Abs(degreeDiffToNextPosition) <= 5) {
                return true;
            } else if (Math.Abs(degreeDiffToNextPosition) <= TankPlatformRotationDegreeStep) {
                // Calculate new almost equal route 
                var distance = Calculator.CalculateDistance(PositionCenter, DestinationPoint);
                SetMovingDestination(Calculator.CalculateDestinationPoint(PositionCenter, distance, OrientationInDegrees));

                return true;
            }

            //Console.WriteLine("Tank cant drive to next position - Current degree={0}, diffToNextPosition={1}", OrientationInDegrees, degreeDiffToNextPosition);
            return false;
        }

        public override bool PendingMove() {

            if (!Dead && PendingMoveToDo && IsPointReachable(NextPosition)) {

                if (IsControlledByHumanPlayer || CanDriveThereWithoutRotationForAiPlayer()) {
                    return base.PendingMove();
                } else {
                    SetDestinationRotationForStepwiseRotation(NextPosition);
                }
            } else {
                Stop();
            }
            
            return false;
        }

        public bool PendingTowerRotation() {
            // Should Tower rotate?
            if (!Dead && PendingTowerRotationToDo) {
                
                var newOrientation = Calculator.GetNewDegreeForStepwiseRotation(TowerOrientationInDegrees, TowerRotationDestination, TowerRotationDegreesStep);
                RotateTowerInstantly(newOrientation);
                
                return true;
            }

            return false;
        }

        public void SlowDown(){
            if(CurrentSpeed > 1) {
                CurrentSpeed = Calculator.GetAccelratedSpeed(CurrentSpeed, 0.8, 10);
            } else {
                CurrentSpeed = 1;
            }
        }


        public bool IsPointReachableForMovement(Point destination) {
            // Validation if it´s human player´s tank
            if (PlayersType != PlayersTankType.None) {

                // The player is not allowed to drive off screen borders
                if (GameEnv.StdVideoScreen.IsWithinBounds(destination, GetCurrentSurface().Width, GetCurrentSurface().Height) == false) {
                    return false;
                }
            }

            var newRectangleForCollisionDetection = RectangleForCollisionDetection;
            newRectangleForCollisionDetection.Offset(NextPosition.X - PositionCenter.X, NextPosition.Y - PositionCenter.Y);

            var listOfCollisions = CollisionDetection.CollisionDetectionForSpecificObject(this, newRectangleForCollisionDetection);

            if (listOfCollisions.Any()) {
                foreach (var collision in listOfCollisions) {
                    return !CollisionHandler.CollisionHandlingFor2Objects(this, collision);
                }
            }
            
            return true;
        }

        public override void Die(IGameObject killer) {
            // Explosionenkaskade
            ExplosionCascadeFactory.CreateExplosionCascade(PositionCenter, RectangleForCollisionDetection.Width / 2, 2, 3, this);
            
            SetSurface("PanzerPlattformZerstoert2.png");
            _towerCurrentSurface = _towerDestroyedVideo.ActiveVideoPlayer.GetAndPlayVideoSprite().Surface;

            base.Die(killer);
            if (IsControlledByHumanPlayer && PlayerAsController != null && PlayerAsController.MainControlledSoldier != null) {
                PlayerAsController.MainControlledSoldier.Die(killer);
            }

            if (IsControlledByHumanPlayer == false) {
                SoundHandler.PlayRandomHahaAndQuotesSound();
                
                // TODO: inc. players body count
            }
        }

        public void WasHit(int hitpoints, IGameObject shooter) {
            if (Dead || Vulnerable == false) {
                return;
            }

            Health = Health - hitpoints;

            if(Health <= 0) {
                Die(shooter);
            } else {
                if (hitpoints > 0) {
                    SoundHandler.PlayRandomTankWasHitWithoutDamageSound();
                } else {
                    SoundHandler.PlayRandomTankWasHitWithoutDamageSound();
                }
                
            }
        }
    }
}
