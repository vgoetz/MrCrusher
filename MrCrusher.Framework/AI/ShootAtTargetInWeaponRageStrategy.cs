using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Core;

namespace MrCrusher.Framework.AI {
    public class ShootAtTargetInWeaponRageStrategy : IShootStrategy {

        public void AI_ShootAtObject(IGameObject performer, IGameObject target) {

            if (target == null) {
                return;
            }

            if (performer.IsControlledByHumanPlayer) {
                return;
            } 
            
            var performerWithPrimaryWeapon = performer as IHasPrimaryWeapon;
            var performerWithSecondaryWeapon = performer as IHasSecondaryWeapon;
            
            if (performerWithPrimaryWeapon == null && performerWithSecondaryWeapon == null) {
                return;
            }

            var tank = performer as Tank;
            var soldier = performer as Soldier;

            if (tank != null) {
                if (Calculator.CalculateDistance(tank.CalculateStartPositionForNewShotDepentsOnTower(), target.PositionCenter) > tank.WeaponRange) {
                    return;
                }
            }

            if (soldier != null) {
                if (Calculator.CalculateDistance(soldier.PositionCenter, target.PositionCenter) > soldier.WeaponRange) {
                    return;
                }
            }

            if (performerWithPrimaryWeapon != null) {
                performerWithPrimaryWeapon.ShootPrimaryWeapon();
                return;
            }

            performerWithSecondaryWeapon.ShootSecondaryWeapon();
        }
    }
}