using System.Collections.Generic;
using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.AI
{
    public interface IHasTriggerAttackStrategy {
        List<ITriggerAttackStrategy> TriggerAttackStrategies { get; }

        bool AI_TriggerAttackOnTarget(IGameObject target);

        void Stop();
    }
}
