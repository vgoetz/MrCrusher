
using System.Drawing;

namespace MrCrusher.Framework {
    
    class GameObjectTypeConstructionPlan : IGameObjectTypeConstructionPlan {
        public GameObjectTypeConstructionPlan(object type, int healthPoints, Size collisionSize) {
            Type = type;
            HealthPoints = healthPoints;
            CollisionSize = collisionSize;
        }

        public object Type { get; private set; }
        public int HealthPoints { get; private set; }
        public Size CollisionSize { get; private set; }
    }
}
