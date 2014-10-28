using System.Linq;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.Core
{
    public class CollisionHandler {

        public static bool CollisionHandlingFor2Objects(IGameObject obj1, IGameObject obj2) {
            if (obj1 != null &&
                obj2 != null &&
                obj1 != obj2)
            {
                if (obj1 is Projectile &&
                    ((Projectile)obj1).Active &&
                    obj2 is IHitable &&
                    (obj2 is ISoldier == false ||
                     obj2.Dead == false)) {

                    ((Projectile)obj1).HitObject((IHitable)obj2);
                    return true;
                }
                
                if (obj2 is Projectile &&
                    ((Projectile)obj2).Active &&
                    obj1 is IHitable &&
                    (obj1 is ISoldier == false ||
                     obj1.Dead == false)) {

                    ((Projectile)obj2).HitObject((IHitable) obj1);
                    return true;
                }

                if (obj1 is ICanSmash && obj1 is MovingObject && ((MovingObject)obj1).Route.Any() && obj2 is ISmashable) {
                    obj2.Die(obj1);
                    return true;
                } 

                if (obj1 is ISmashable && obj2 is ICanSmash && obj2 is MovingObject && ((MovingObject)obj2).Route.Any()) {
                    obj1.Die(obj2);
                    return true;
                }

                if (obj1 is IHitable && obj2 is IHitable) {
                    return true;
                }
            }

            return false;
        }
    }
}
