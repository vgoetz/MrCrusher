using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.AI
{
    public interface IShootStrategy {
        /// <summary>
        /// Shooting at a target
        /// </summary>
        /// <param name="performer"></param>
        /// <param name="target"></param>
        /// <returns>Target sighted</returns>
        void AI_ShootAtObject(IGameObject performer, IGameObject target);
    }
}
