using System.Drawing;
using System.Runtime.Serialization;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.BaseObjectsFactories;

namespace MrCrusher.Framework.BaseObjects {

    [DataContract]
    public class Rifle : Weapon {
        public Rifle(string imageFileName, string videoFileName, double range, double accuracy, int power, int fireRate, GameObject parent)
            : base(false, imageFileName, videoFileName) {
            WeaponRange = range;
            Accuracy = accuracy;
            Power = power;
            Parent = parent;
            FireRate = fireRate;
            PrimaryShotFactory = new RifleShotFactory();
        }

        public override bool Shoot(Point startPositionCenter, Point endPositionCenter, IGameObject shooter) {
            if (CanIShootNow()) {
                var shot = PrimaryShotFactory.CreateProjectile(startPositionCenter, endPositionCenter, Power, shooter);
                return Shoot(shot);
            }
            return false;
        }

        public override void RotateToMousePosition(Point destination) {}

        
    }
}