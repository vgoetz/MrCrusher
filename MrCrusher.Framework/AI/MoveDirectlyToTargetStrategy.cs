using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.AI {
    public class MoveDirectlyToTargetStrategy : IMoveStrategy {
        
        public bool AI_MoveToObject(IGameObject performer, Point targetPoint) {
            var objectToMove = performer as MovingObject;

            if (objectToMove != null) {
                objectToMove.SetMovingDestination(targetPoint);
                return true;
            }

            return false;
        }
    }
}