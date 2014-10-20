using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.AI {
    public class ShootAtTargetStrategy : IShootStrategy {

        public void AI_ShootAtObject(IGameObject performer, IGameObject target) {
            if (!(performer is MovingObject) || (!(performer is IHasPrimaryWeapon) && !(performer is IHasSecondaryWeapon))) {
                return;
            }

            var objWithPrimaryWeapon = performer as IHasPrimaryWeapon;
            var objWithSecondaryWeapon = performer as IHasSecondaryWeapon;
                
            if (objWithPrimaryWeapon != null) {
                objWithPrimaryWeapon.ShootPrimaryWeapon();
                return;
            }

            objWithSecondaryWeapon.ShootSecondaryWeapon();
        }
    }
}