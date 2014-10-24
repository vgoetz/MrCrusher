using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using MrCrusher.Framework.AI;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Drawable;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.MediaManagement;
using MrCrusher.Framework.SpecialObjects;
using SdlDotNet.Graphics;

namespace MrCrusher.Framework.BaseObjects
{
    [DataContract]
    [KnownType(typeof(ExplosionCascade))]
    [KnownType(typeof(MovingObject))]
     [KnownType(typeof(RotatingObject))]
      [KnownType(typeof(Soldier))]
      [KnownType(typeof(Bunker))]
      [KnownType(typeof(Weapon))]
       [KnownType(typeof(Tank))]
       [KnownType(typeof(Rifle))]
      [KnownType(typeof(Projectile))]
       [KnownType(typeof(RifleShot))]
       [KnownType(typeof(TankShot))]
    public abstract class GameObject : IGameObject {

        [DataMember]
        private Point _positionCenter;

        [DataMember]
        public Point PositionCenter {
            get { return _positionCenter; }
            set { _positionCenter = value; }
        }

        public virtual Point SurfacePositionTopLeft { 
            get { return new Point(PositionCenter.X + SurfaceOffsetTopLeft.X, PositionCenter.Y + SurfaceOffsetTopLeft.Y); }
        }

        private Point SurfaceOffsetTopLeft {
            get {
                var currentRectangle = GetCurrentSurfaceRectangle();
                return new Point(-currentRectangle.Width / 2, -currentRectangle.Height / 2);
            }
        }

        [DataMember]
        public int Health { get; protected set; }

        [DataMember]
        public int MaxHealth { get; protected set; }

        public IAiMission CurrentMission { get; set; }

        private Rectangle _rectangleForCollisionDetection;
        public Rectangle RectangleForCollisionDetection {
            get {
                _rectangleForCollisionDetection.X = PositionCenter.X - _rectangleForCollisionDetection.Width / 2;
                _rectangleForCollisionDetection.Y = PositionCenter.Y - _rectangleForCollisionDetection.Height / 2; 
                return _rectangleForCollisionDetection;
            }
            set { 
                value.X = PositionCenter.X - value.Width / 2;
                value.Y = PositionCenter.Y - value.Height / 2;
                _rectangleForCollisionDetection = value;
            }
        }

        private Surface _objectSurface;
        public string ImageName { get; protected set; }
        public VideoPlayerCollection VideoCollection = new VideoPlayerCollection();

        public Dictionary<IGameObject, bool> GameObjectsInteractionDictionary;
        public Dictionary<Type, bool> ClassTypesInteractionDictionary; 


        [DataMember]
        private Guid _guid;

        [DataMember]
        public Guid Guid {
            get { return _guid; }
            set { _guid = value; }
        }

        public string GetVideoName() {
            return VideoCollection == null ? null : VideoCollection.ActiveVideoPlayer.VideoParameter.VideoFileName;
        }

        protected GameObject(bool isControledByHumanPlayer, string surfaceFileName, VideoStartParameter videoParameter) {
            if(string.IsNullOrEmpty(surfaceFileName) == false) {
                SetSurface(surfaceFileName);
            }

            if(videoParameter != null) {
                VideoCollection = new VideoPlayerCollection(videoParameter, true);
            }
            
            InitComponents();
        }

        protected GameObject(bool isControledByHumanPlayer, string surfaceFileName, VideoStartParameter videoParameter, Point positionCenter)
            : this(isControledByHumanPlayer, surfaceFileName, videoParameter) {
            PositionCenter = positionCenter;
        }

        protected GameObject(bool isControledByHumanPlayer, string surfaceFileName, string videoFileName)
            : this(isControledByHumanPlayer, surfaceFileName, !string.IsNullOrEmpty(videoFileName) ? new VideoStartParameter(videoFileName, true) : null) {
        }

        protected GameObject(bool isControledByHumanPlayer, string surfaceFileName, string videoFileName, Point positionCenter)
            : this(isControledByHumanPlayer, surfaceFileName, !string.IsNullOrEmpty(videoFileName) ? new VideoStartParameter(videoFileName, true) : null) {
            PositionCenter = positionCenter;
        }

        private void InitComponents() {
            GameObjectsInteractionDictionary = new Dictionary<IGameObject, bool>();
            ClassTypesInteractionDictionary = new Dictionary<Type, bool>();

            Guid = Guid.NewGuid();
            Vulnerable = false;
            Visible = true;
            CanInteractWithOthers = true;
            ShouldBeDeleted = false;
            Dead = false;

            CanBeControlledByHumanPlayer = true;
            CanReseiveKeyCommands = true;
        }
        
        public void SetSurface(string surfaceFileName) {
            _objectSurface = ImageContainer.GetImage(surfaceFileName);
            ImageName = surfaceFileName;
        }

        public Surface GetCurrentSurface() {
            if (VideoCollection.ActiveVideoPlayer != null) {
                return VideoCollection.ActiveVideoPlayer.GetCurrentVideoSurface();
            }

            if (_objectSurface != null) {
                return _objectSurface;
            }

            return null;
        }

        private Rectangle GetCurrentSurfaceRectangle() {
            if (VideoCollection.ActiveVideoPlayer != null) {
                return VideoCollection.ActiveVideoPlayer.GetCurrentVideoSurfaceRectangle();
            }

            if (_objectSurface != null) {
                return _objectSurface.Rectangle;
            }

            return new Rectangle();
        }

        private Sprite GetImageOrPlayVideo() {

            if (VideoCollection.ActiveVideoPlayer != null) {
                var sprite = VideoCollection.ActiveVideoPlayer.GetAndPlayVideoSprite();
                sprite.Infos.SurfacePositionTopLeftX = SurfacePositionTopLeft.X;
                sprite.Infos.SurfacePositionTopLeftY = SurfacePositionTopLeft.Y;
                return sprite;
            }

            if (_objectSurface != null) {
                return new Sprite(_objectSurface, new SpriteInfo { Name = ImageName, SpriteType = SpriteType.Image, SurfacePositionTopLeftX = SurfacePositionTopLeft.X, SurfacePositionTopLeftY = SurfacePositionTopLeft.Y });
            }

            return null;
        }

        public virtual IEnumerable<Sprite> GetCurrentSprites() {

            var list = new List<Sprite>();
            Sprite surfaceAndInfo = GetImageOrPlayVideo();

            if (surfaceAndInfo != null) {
                list.Add(surfaceAndInfo);
            }

            return list;
        }

        public bool IsInteractingWith(IGameObject obj) {
            if (CanInteractWithOthers == false ||
                obj.CanInteractWithOthers == false) {

                return false;
            }

            bool thisWithObj = GameObjectsInteractionDictionary.ContainsKey(obj) == false ||
                               GameObjectsInteractionDictionary[obj];
            bool objWithThis = ((obj is MovingObject == false) ||
                     ((MovingObject)obj).GameObjectsInteractionDictionary.ContainsKey(this) == false ||
                     ((MovingObject)obj).GameObjectsInteractionDictionary[this]);

            bool thisWithObjType = ClassTypesInteractionDictionary.ContainsKey(obj.GetType()) == false ||
                               ClassTypesInteractionDictionary[obj.GetType()];
            bool objWithThisType = ((obj is MovingObject == false) ||
                     ((MovingObject)obj).ClassTypesInteractionDictionary.ContainsKey(this.GetType()) == false ||
                     ((MovingObject)obj).ClassTypesInteractionDictionary[this.GetType()]);

            return thisWithObj && objWithThis && thisWithObjType && objWithThisType;
        }

        [DataMember]
        public bool ShouldBeDeleted { get; set; }
        [DataMember]
        public bool Visible { get; set; }
        public bool Vulnerable { get; set; }
        public bool CanInteractWithOthers { get; protected set; }
        [DataMember]
        public bool Dead { get; set; }

        public DateTime? TimeOfDeath { get; private set; }
        
        public virtual void Die() {
            Health = 0;
            Dead = true;
            TimeOfDeath = DateTime.Now;

            if (GameEnv.GameObjectsToDrawAndMove.All(obj => !obj.IsControlledByHumanPlayer || (obj.IsControlledByHumanPlayer && obj.Dead)) &&
                GameEnv.ImageTransferObjects.All(    obj => !obj.IsControlledByHumanPlayer || (obj.IsControlledByHumanPlayer && obj.Dead))) {

                GameEnv.EndTime = TimeOfDeath;
            }
        }

        public void MarkAsToBeDeleted() {
            ShouldBeDeleted = true;
        }

        public Player.Player PlayerAsController { get; set; }
        public bool CanBeControlledByHumanPlayer { get; protected set; }
        public bool IsControlledByHumanPlayer { get { return PlayerAsController != null; } }
        public bool CanReseiveKeyCommands { get; protected set; }

        public IEnumerable<ImageTransferObject> ToImageTransferObjects() {
            IEnumerable<Sprite> sprites = GetCurrentSprites();

            return sprites.Select(sprite => new ImageTransferObject(Guid, PlayerAsController, sprite.Infos, Health, MaxHealth, Dead,
                IsControlledByHumanPlayer, PlayerAsController != null ? PlayerAsController.PlayersColor : (Color?) null, (short)(this is ISoldier ? 15 : 25)));
        }
    }
}
