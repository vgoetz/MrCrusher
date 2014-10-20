using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.BaseObjectsFactories.Interfaces;
using MrCrusher.Framework.Game.Environment;

namespace MrCrusher.Framework.BaseObjectsFactories
{
    public class RifleShotFactory : IShotFactory
    {
        public Projectile CreateProjectile(Point startPositionCenter, Point endPositionCenter, int power, IGameObject shooter) {

            var rifleShot = new RifleShot(startPositionCenter, endPositionCenter, power, shooter);
            rifleShot.ClassTypesInteractionDictionary.Add(typeof(RifleShot), false);

            // Registration
            GameEnv.RegisterGameObjectForAdding(rifleShot);

            return rifleShot; 
        }
    }
}