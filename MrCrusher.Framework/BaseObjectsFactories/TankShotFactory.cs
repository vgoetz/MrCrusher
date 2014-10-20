using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.BaseObjectsFactories.Interfaces;
using MrCrusher.Framework.Game.Environment;

namespace MrCrusher.Framework.BaseObjectsFactories
{
    public class TankShotFactory : IShotFactory
    {
        public Projectile CreateProjectile(Point startPositionCenter, Point endPositionCenter, int power, IGameObject shooter)
        {
            var tankShot = new TankShot(startPositionCenter, endPositionCenter, power, shooter);
            tankShot.ClassTypesInteractionDictionary.Add(typeof(TankShot), false);

            GameEnv.RegisterGameObjectForAdding(tankShot);

            return tankShot;
        }
    }
}