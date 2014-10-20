using System.Drawing;
using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.AI
{
    public interface IMoveStrategy {
        /// <summary>
        /// Move to point strategy
        /// </summary>
        /// <param name="performer"></param>
        /// <param name="target"></param>
        /// <returns>Target sighted</returns>
        bool AI_MoveToObject(IGameObject performer, Point targetPoint);
    }
}
