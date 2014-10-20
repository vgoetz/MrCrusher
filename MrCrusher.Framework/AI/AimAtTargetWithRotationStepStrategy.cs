using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Core;

namespace MrCrusher.Framework.AI {
    public class AimAtTargetWithRotationStepStrategy : IAimStrategy {

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

            var tank = performer as Tank;
            var soldier = performer as Soldier;

            if (tank != null) {
                tank.SetTowerRotationDestination(position);

            } else if (soldier != null) {
                soldier.SetDestinationRotationForStepwiseRotation(position);

            } else {
                var rotatingObject = performer as RotatingObject;

                if (rotatingObject != null) {
                    rotatingObject.SetDestinationRotationForStepwiseRotation(position);
                }
            }

            return Calculator.CalculateDegree(performer.PositionCenter, position) < 2;
        }
    }
}