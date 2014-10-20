using System.Drawing;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.SpecialObjects;

namespace MrCrusher.Framework.SpecialObjectsFactories
{
    public interface IExplosionCascadeFactory {

        ExplosionCascade CreateExplosionCascade(Point startPositionCenter, int radius, int numberOfExplosions, int framesBetweenExplosions, IGameObject victim);
    }
}