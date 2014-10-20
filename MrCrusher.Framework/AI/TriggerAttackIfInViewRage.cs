using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Core;

namespace MrCrusher.Framework.AI {
    public class TriggerAttackIfInViewRage : ITriggerAttackStrategy {

        public bool AI_TriggerAttackOnTarget(IGameObject performer, IGameObject target) {

            if (target == null) {
                return false;
            }

            if (performer is MovingObject && (performer is IHasPrimaryWeapon || performer is IHasSecondaryWeapon)) {
                var tank = performer as Tank;
                var soldier = performer as Soldier;

                if (tank != null) {
                    if (Calculator.CalculateDistance(tank.CalculateStartPositionForNewShotDepentsOnTower(), target.PositionCenter) < tank.ViewRange) {
                        return true;
                    }
                    return false;
                }

                if (soldier != null) {
                    if (Calculator.CalculateDistance(soldier.PositionCenter, target.PositionCenter) < soldier.ViewRange) {
                        return true;
                    }
                    return false;
                } 
            }

            return false;
        }
    }
}