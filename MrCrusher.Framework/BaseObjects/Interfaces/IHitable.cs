
using System;

namespace MrCrusher.Framework.BaseObjects.Interfaces
{
    public interface IHitable {
        void WasHit(int hitpoints, IGameObject shooter);
    }

    public interface IDecayable {
        bool Decayed { get; set; }
        TimeSpan? DecayTimeSpanAfterDeath { get; set; }

        void Decay();

        void AfterDecay();
    }
}
