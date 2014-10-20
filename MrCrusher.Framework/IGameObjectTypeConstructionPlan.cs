
using System.Drawing;

namespace MrCrusher.Framework {
    public interface IGameObjectTypeConstructionPlan {

        object Type { get; }
        
        int HealthPoints { get; }

        Size CollisionSize { get; }
    }
}