using System.Drawing;
using MrCrusher.Framework.BaseObjects;

namespace MrCrusher.Framework.BaseObjectsFactories.Interfaces
{
    public abstract class ITankFactory {
        protected abstract Tank CreateTank(string imageFileName, string tankTowerImageFileName, int health, Point positionCenter, int cannonLength,
            int primaryWeaponPower, Rectangle rectangleForCollisionDetection, int range, int viewRange, double accuracy, int fireRate);
    }
}