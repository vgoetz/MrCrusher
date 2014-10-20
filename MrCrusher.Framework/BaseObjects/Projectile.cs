using System.Runtime.Serialization;
using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.BaseObjects
{
    [DataContract]
    [KnownType(typeof(RifleShot))]
    [KnownType(typeof(TankShot))]
    public abstract class Projectile : MovingObject {
        public IGameObject Shooter { get; protected set; }
        public int Power { get; protected set; }
        public bool Active { get; protected set; }

        protected Projectile(int speed, IGameObject shooter) : base(false, null, null) {
            Shooter = shooter;
            CurrentSpeed = speed;
            MaxForwardSpeed = speed;
        }

        public virtual void Fly() {
            VideoCollection.ActiveVideoPlayer.PlayFromBeginning();
        }

        public abstract void HitObject(IHitable obj);
        public abstract void HitGround();

        public void StopAndDefuseProjectile() {
            Stop();
            Power = 0;
            Active = false;
        }

    }
}
