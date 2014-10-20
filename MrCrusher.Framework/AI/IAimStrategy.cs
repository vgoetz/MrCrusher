using System.Drawing;
using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.AI
{
    public interface IAimStrategy {

        /// <summary>
        /// Aim on target
        /// </summary>
        /// <param name="performer"></param>
        /// <param name="target"></param>
        /// <returns>Target sighted</returns>
        bool AI_AimAtObject(IGameObject performer, IGameObject target);
        bool AI_AimAtPosition(IGameObject performer, Point position);
    }
}
