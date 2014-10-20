using System.Drawing;
using System.Runtime.Serialization;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.BaseObjectsFactories.Interfaces;
using MrCrusher.Framework.Game.Environment;

namespace MrCrusher.Framework.BaseObjects {
    [DataContract]
    [KnownType(typeof(Tank))]
    [KnownType(typeof(Rifle))]
    [KnownType(typeof(Bunker))]
    public abstract class Weapon : RotatingObject {
        protected Weapon(bool isControledByHumanPlayer, string imageFileName, string videoFileName) : base(isControledByHumanPlayer, imageFileName, videoFileName) { }
        protected Weapon(bool isControledByHumanPlayer, string imageFileName, string videoFileName, int health) : base(isControledByHumanPlayer, imageFileName, videoFileName, health) { }
        protected Weapon(bool isControledByHumanPlayer, string imageFileName, string videoFileName, int health, Point positionCenter) : base(isControledByHumanPlayer, imageFileName, videoFileName, health, positionCenter) { }

        protected IShotFactory PrimaryShotFactory;
        protected GameObject Parent;

        public int Power { get; protected set; }
        public double WeaponRange { get; protected set; }
        public double Accuracy { get; protected set; }
        public int FireRate { get; protected set; }
        private double _fireRateDelay;

        public abstract bool Shoot(Point startPositionCenter, Point endPositionCenter, IGameObject shooter);

        public bool CanIShootNow() {
            return _fireRateDelay <= 0;
        }

        // Muss innerhalb der großen Loop aufgerufen werden
        public void HandleFireRate() {
            if (_fireRateDelay > 0)
                _fireRateDelay = _fireRateDelay - 0.5;
        }

        protected bool Shoot(Projectile projectile) {
            if (CanIShootNow()) {
                projectile.GameObjectsInteractionDictionary.Add(Parent, false); // This tank must not been hit by his own shot
                projectile.Fly();
                _fireRateDelay = GameEnv.Fps - FireRate;
                return true;
            }
            return false;
        }
    }
}
