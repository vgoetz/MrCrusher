using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.BaseObjectsFactories.Interfaces
{
    public interface IShotFactory {
        Projectile CreateProjectile(Point startPositionCenter, Point endPositionCenter, int power, IGameObject shooter);
    }
}