using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.AI
{
    public interface IHasShootStrategy {
        IShootStrategy ShootStrategy { get; }

        void AI_Shoot(IGameObject target);
    }
}
