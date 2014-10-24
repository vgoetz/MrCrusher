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
    public class Bunker : Weapon, IBunker {

        [DataMember]
        public PlayersBunkerType PlayersType = PlayersBunkerType.None;

        public string TowerImageName { get; protected set; }

        private Surface _towerCurrentSurface;
        private Surface _towerRotatedSurface;

        private VideoPlayerCollection _towerDestroyedVideo;

        [DataMember]
        private Degree _towerOrientationInDegrees;
        public Degree TowerOrientationInDegrees { 
            get { return _towerOrientationInDegrees; }
            protected set { _towerOrientationInDegrees = value; } 
        }

        public Degree TowerRotationDegreesStep { get; private set; }
        [DataMember]
        public Degree TowerDestinationRotation { get; protected set; }
        public bool PendingTowerRotationToDo {
            get {
                return Math.Abs(TowerDestinationRotation - TowerOrientationInDegrees) > TowerRotationDegreesStep;
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
        
        public Bunker(bool isControledByHumanPlayer, string imageFileName, string bunkerTowerImageFileName, int health, Point positionCenter, int cannonLength, int primaryWeaponPower, Rectangle rectangleForCollisionDetection, int range, int viewRange, double accuracy, int fireRate)
            : base(isControledByHumanPlayer, imageFileName, null, health, positionCenter) {
            InitComponents();

            _towerCurrentSurface = ImageContainer.GetImage(bunkerTowerImageFileName);
            TowerImageName = bunkerTowerImageFileName;

            _lengthOfCannonFromCenter = cannonLength;
            Power = primaryWeaponPower;
            SecondaryWeaoponPower = 5;
            RectangleForCollisionDetection = rectangleForCollisionDetection;
            WeaponRange = range;
            ViewRange = viewRange;
            Accuracy = accuracy;
            FireRate = fireRate;

            VideoCollection.Add(new VideoStartParameter("Bunker1_Plattform_OnFire.png", true, false, true, 2), false);

            PrimaryShotFactory = new TankShotFactory();
            SecondaryShotFactory = new RifleShotFactory();

            ExplosionCascadeFactory = new ExplosionCascadeFactory();

            Parent = this;
        }

        private void InitComponents(){
            SetDefaultBunkerValues();
            _towerDestroyedVideo = new VideoPlayerCollection(new VideoStartParameter("TankTowerOnFire2.png", true, false, false, 4), false);

            OrientationInDegrees = 0;
            TowerOrientationInDegrees = 0;
            TowerDestinationRotation = 0;

            TriggerAttackStrategies = new List<ITriggerAttackStrategy> { new TriggerAttackIfInViewRage(), new TriggerAttackAfterHit() };
            AimStrategy = new AimAtTargetWithRotationStepStrategy();
            ShootStrategy = new ShootAtTargetInWeaponRageStrategy();
        }

        private void SetDefaultBunkerValues() {
            Vulnerable = true;

            PrimaryWeapon = this;
            SecondaryWeapon = new Rifle(null, null, WeaponRange, Accuracy, 15, 27, this);
            
            CurrentSpeed = 0;
            MaxForwardSpeed = 0;
            MaxBackwardsSpeed = 0;
            Accelration = 0;
            RotationDegreesStep = 0;
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

        public override IEnumerable<Sprite> GetCurrentSprites() {

            List<Sprite> list = base.GetCurrentSprites().ToList();

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
                var surfaceAndInfo = new Sprite(
                                            _towerRotatedSurface,
                                            new SpriteInfo {
                                                Orientation = TowerOrientationInDegrees, 
                                                Name = TowerImageName, 
                                                SpriteType = SpriteType.Image, 
                                                SurfacePositionTopLeftX = SurfacePositionTopLeft.X,
                                                SurfacePositionTopLeftY = SurfacePositionTopLeft.Y
                                            });
                list.Add(surfaceAndInfo);
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

        public override bool PendingRotation() {
            PendingTowerRotation();

            return base.PendingRotation();
        }

        public void SetTowerRotationDestination(Point destination) {
            TowerDestinationRotation = Calculator.CalculateDegree(PositionCenter, destination);
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
        }

        public void RotateTowerInstantly(Degree degree) {
            HasTowerAlreadyRotated = true;
            TowerOrientationInDegrees = degree;
            TowerDestinationRotation = TowerOrientationInDegrees;
        }
        
        public bool PendingTowerRotation() {
            // Should Tower rotate?
            if (!Dead && PendingTowerRotationToDo) {
                
                var newOrientation = Calculator.GetNewDegreeForStepwiseRotation(TowerOrientationInDegrees, TowerDestinationRotation, TowerRotationDegreesStep);
                RotateTowerInstantly(newOrientation);
                
                return true;
            }

            return false;
        }
        
        public override void Die() {
            // Explosionenkaskade
            ExplosionCascadeFactory.CreateExplosionCascade(PositionCenter, RectangleForCollisionDetection.Width / 2, 3, 2, this);
            ExplosionCascadeFactory.CreateExplosionCascade(PositionCenter, RectangleForCollisionDetection.Width / 2, 3, 20, this);

            VideoCollection.SetActiveVideo("Bunker1_Plattform_OnFire.png");
            _towerDestroyedVideo.SetActiveVideo("TankTowerOnFire2.png");

            base.Die();
            if (IsControlledByHumanPlayer && PlayerAsController != null && PlayerAsController.MainControlledSoldier != null) {
                PlayerAsController.MainControlledSoldier.Die();
            }

            if (IsControlledByHumanPlayer == false) {
                SoundHandler.PlayRandomHahaAndQuotesSound();
                
                // TODO: inc. players body count
            }
        }

        public void WasHit(int hitpoints) {
            if (Dead || Vulnerable == false) {
                return;
            }

            Health = Health - hitpoints;

            if(Health <= 0) {
                Die();
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
