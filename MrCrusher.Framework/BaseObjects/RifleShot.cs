using System.Drawing;
using System.Runtime.Serialization;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.MediaManagement;

namespace MrCrusher.Framework.BaseObjects {

    [DataContract]
    public class RifleShot : Projectile {
        private const string SHOT = "vidSchussStartUndFlug.png";
        private const string HIT_GROUND = "vidRifleShotHitsGround.png";
        private const string HIT_OBJECT = "vidRifleShotHitsGround.png";

        public RifleShot(Point startPositionCenter, Point endPositionCenter, int power, IGameObject shooter)
            : base(3, shooter) {

            VideoCollection.Add(new VideoStartParameter(SHOT, false, true, true, 0), true);
            VideoCollection.Add(new VideoStartParameter(HIT_GROUND, false, true, false, 0), false);
            VideoCollection.Add(new VideoStartParameter(HIT_OBJECT, false, true, false, 0), false);
            
            PositionCenter = startPositionCenter;
            SetMovingDestination(endPositionCenter);
            RectangleForCollisionDetection = new Rectangle(0, 0, 3, 3);

            Visible = true;
            Vulnerable = false;
            Active = true;
            Power = power;
        }

        public override void Fly() {
            // Sound
            SoundHandler.PlayRandomRifleShotSound();

            base.Fly();
        }

        public override bool PendingMove() {
            bool moved = base.PendingMove();

            if (moved == false) {
                if (GetVideoName() == SHOT) {
                    HitGround();
                } else if (VideoCollection.ActiveVideoPlayer.IsAtEnd() && 
                            (GetVideoName() == HIT_GROUND || 
                             GetVideoName() == HIT_OBJECT)) {
                    MarkAsToBeDeleted();
                }
            }

            return moved;
        }
        
        public override void HitObject(IHitable obj) {
            VideoCollection.SetActiveVideo(HIT_OBJECT);
            if (obj is ITank) {
                SoundHandler.PlayRandomTankWasHitWithoutDamageSound();
                obj.WasHit(0);
            } else {
                SoundHandler.PlayRandomRifleShotHitsSoftMaterialSound();
                obj.WasHit(Power);
            }

            StopAndDefuseProjectile();
        }

        public override void HitGround() {
            VideoCollection.SetActiveVideo(HIT_GROUND);
            SoundHandler.PlayRandomRifleShotHitsSoftMaterialSound();
            StopAndDefuseProjectile();
        }

    }
}