using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.BaseObjectsFactories.Interfaces;
using MrCrusher.Framework.Game.Environment;

namespace MrCrusher.Framework.BaseObjectsFactories {

    public class FlyingGranadeFactory : IShotFactory {

        public Projectile CreateProjectile(Point startPositionCenter, Point endPositionCenter, int power, IGameObject shooter) {

            var granade = new Granade(startPositionCenter, endPositionCenter, power, shooter);
            granade.ClassTypesInteractionDictionary.Add(typeof(Granade), false);

            // Registration
            GameEnv.RegisterGameObjectForAdding(granade);

            return granade; 
        }
    }
}