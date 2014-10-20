using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.AI
{
    public interface ITriggerAttackStrategy {

        bool AI_TriggerAttackOnTarget(IGameObject performer, IGameObject target);
    }
}
