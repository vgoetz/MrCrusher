using System.Drawing;

namespace MrCrusher.Framework.AI
{
    public interface IHasMoveStrategy {
        IMoveStrategy MoveStrategy { get; }

        void AI_SetMoveDestination(Point targetPoint);

        void Stop();
    }
}
