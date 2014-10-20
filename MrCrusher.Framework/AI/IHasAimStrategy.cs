using System.Drawing;
using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.AI
{
    public interface IHasAimStrategy {
        IAimStrategy AimStrategy { get; }

        void AI_SetAimTarget(IGameObject target);
        void AI_SetAimPosition(Point position);
    }
}
