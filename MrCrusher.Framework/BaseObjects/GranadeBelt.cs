using System.Drawing;
using System.Runtime.Serialization;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.BaseObjectsFactories;

namespace MrCrusher.Framework.BaseObjects {

    [DataContract]
    public class GranadeBelt : Weapon {

        public GranadeBelt(string imageFileName, string videoFileName, double range, double accuracy, int power, int fireRate, GameObject parent)
            : base(false, imageFileName, videoFileName) {
            WeaponRange = range;
            Accuracy = accuracy;
            Power = power;
            Parent = parent;
            FireRate = fireRate;
            PrimaryShotFactory = new FlyingGranadeFactory();
        }

        public override bool Shoot(Point startPositionCenter, Point endPositionCenter, IGameObject shooter) {
            if (CanIShootNow()) {
                var granade = PrimaryShotFactory.CreateProjectile(startPositionCenter, endPositionCenter, Power, shooter);
                return Shoot(granade);
            }
            return false;
        }

        public override void RotateToMousePosition(Point destination) {}

        
    }
}