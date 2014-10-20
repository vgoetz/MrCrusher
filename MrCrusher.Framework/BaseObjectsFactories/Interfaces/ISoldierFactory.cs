using System.Drawing;
using MrCrusher.Framework.BaseObjects;

namespace MrCrusher.Framework.BaseObjectsFactories.Interfaces
{
    public abstract class ISoldierFactory {
        protected abstract Soldier CreateSoldier(string imageFileName, string videoFileName, int health, Point positionCenter, int primaryWeaponPower, Rectangle rectangleForCollisionDetection, int weaponRange, int viewRange, double accuracy, int fireRate);
    }
}