using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.AI {
    public class AimAtObjectInstantlyStrategy : IAimStrategy {

        public bool AI_AimAtObject(IGameObject performer, IGameObject target) {

            if (target == null) {
                return false;
            }

            return AI_AimAtPosition(performer, target.PositionCenter);
        }

        public bool AI_AimAtPosition(IGameObject performer, Point position) {
            if (position.Equals(performer.PositionCenter)) {
                return false;
            }

            var rotatingObject = performer as RotatingObject;
            if (rotatingObject != null) {
                rotatingObject.RotateInstantly(position);
            }

            return true;
        }
    }
}