using System;
using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Core;

namespace MrCrusher.Framework.AI {
    public class MoveDirectlyAndCloseToTargetStrategy : IMoveStrategy {

        public bool AI_MoveToObject(IGameObject performer, Point targetPoint) {
            var objectToMove = performer as MovingObject;
            var hasPrimaryWeapon = performer as IHasPrimaryWeapon;
            double weaponRange = 0;
            if (hasPrimaryWeapon != null) {
                weaponRange = hasPrimaryWeapon.PrimaryWeapon.WeaponRange;
            }

            if (objectToMove != null) {

                var distance = Calculator.CalculateDistance(objectToMove.PositionCenter, targetPoint);

                if (distance < weaponRange - (weaponRange / 2)) {
                    objectToMove.Stop();
                    if (objectToMove.VideoCollection.ActiveVideoPlayer != null) {
                        objectToMove.VideoCollection.ActiveVideoPlayer.Freeze();
                    }
                    return false;
                }
                
                objectToMove.SetMovingDestination(targetPoint);
                
                if (objectToMove.VideoCollection.ActiveVideoPlayer != null && objectToMove.VideoCollection.ActiveVideoPlayer.Freezed) {
                    objectToMove.VideoCollection.ActiveVideoPlayer.PlayFromBeginning();
                }
                return true;
            }

            return false;
        }
    }
}