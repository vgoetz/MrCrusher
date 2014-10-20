using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Game.Environment;

namespace MrCrusher.Framework.Core {

    public class CollisionDetection {

        public static int CollisionDetectionAndHandlingForAllDrawingObjects() {
            int collisions = 0;

            if (GameEnv.GameObjectsToDrawAndMove.Any() == false) {
                return 0;
            }

            for (var i = 0; i < GameEnv.GameObjectsToDrawAndMove.Count; i++) {

                for (var j = i + 1; j < GameEnv.GameObjectsToDrawAndMove.Count; j++) {

                    if (GameEnv.GameObjectsToDrawAndMove[i].Dead || GameEnv.GameObjectsToDrawAndMove[i].ShouldBeDeleted ||
                        GameEnv.GameObjectsToDrawAndMove[j].Dead || GameEnv.GameObjectsToDrawAndMove[j].ShouldBeDeleted) {
                        
                        continue;
                    }

                    // Should this two objects interact?
                    if (GameEnv.GameObjectsToDrawAndMove[i].IsInteractingWith(GameEnv.GameObjectsToDrawAndMove[j]) == false ||
                        GameEnv.GameObjectsToDrawAndMove[j].IsInteractingWith(GameEnv.GameObjectsToDrawAndMove[i]) == false) {
                        
                        continue;
                    }

                    if (CollisionDetectionFor2Objects(GameEnv.GameObjectsToDrawAndMove[i], GameEnv.GameObjectsToDrawAndMove[j])) {

                        if (CollisionHandler.CollisionHandlingFor2Objects(GameEnv.GameObjectsToDrawAndMove[i], GameEnv.GameObjectsToDrawAndMove[j])) {
                            collisions++;
                        }
                    }
                }
            }

            return collisions;
        }

        public static IList<IGameObject> CollisionDetectionForSpecificObject(IGameObject obj, Rectangle rectangleForCollisionDetection) {
            if (GameEnv.GameObjectsToDrawAndMove.Any() == false) {
                return null;
            }

            return GameEnv.GameObjectsToDrawAndMove.Where(other => !other.Dead && 
                                                                   !other.ShouldBeDeleted &&
                                                                   !obj.Equals(other) &&
                                                                   (other is Projectile == false || (((Projectile)other).Active)))
                                                   .Where(other => obj.IsInteractingWith(other) &&
                                                                   other.IsInteractingWith(obj))
                                                   .Where(other => CollisionDetectionFor2Objects(rectangleForCollisionDetection, other)).ToList();
        }
        
        public static bool CollisionDetectionFor2Objects(IGameObject obj1, IGameObject obj2) {
            if (obj1 != null &&
                obj2 != null &&
                obj1 != obj2) {
                var isIntersecting = obj1.RectangleForCollisionDetection.IntersectsWith(obj2.RectangleForCollisionDetection);
                if (isIntersecting) {
                    return true;
                }
            }

            return false;
        }

        public static bool CollisionDetectionFor2Objects(Rectangle collisionRectangle, IGameObject obj) {

            if (obj != null) {
                var isIntersecting = collisionRectangle.IntersectsWith(obj.RectangleForCollisionDetection);
                if (isIntersecting) {
                    return true;
                }
            }

            return false;
        }
        
        public static bool CollisionDetectionForRectangle_ForAddingNewObjects(Point centerPositionOfObjcet, Size collisionSizeOfObject) {
            var topLeftPosition = new Point(centerPositionOfObjcet.X - collisionSizeOfObject.Width / 2, centerPositionOfObjcet.Y - collisionSizeOfObject.Height / 2);
            var collisionRectangle = new Rectangle(topLeftPosition, collisionSizeOfObject);

            return CollisionDetectionForRectangle_ForAddingNewObjects(collisionRectangle);
        }

        public static bool CollisionDetectionForRectangle_ForAddingNewObjects(Rectangle collisionRectangle) {
            return GameEnv.ExistingAndRegisteredGameObjects.Any(gameObject => gameObject.RectangleForCollisionDetection.IntersectsWith(collisionRectangle));
        }
    }
}
