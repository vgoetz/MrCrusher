using System.Linq;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Game.Environment;

namespace MrCrusher.Framework.AI {
    public class TriggerAttackAfterHit : ITriggerAttackStrategy {

        public bool AI_TriggerAttackOnTarget(IGameObject performer, IGameObject target) {

            if (target == null) {
                return false;
            }

            if (performer is MovingObject) {
                return GameEnv.GameObjectsToDrawAndMove.OfType<Projectile>()
                    .Select(projectile => Calculator.CalculateDistance(projectile.PositionCenter, performer.PositionCenter))
                    .Any(distanceBetweenProjectileAndTarget => distanceBetweenProjectileAndTarget <= target.RectangleForCollisionDetection.Width || 
                                                               distanceBetweenProjectileAndTarget <= target.RectangleForCollisionDetection.Height);
            }

            return false;
        }
    }
}