using System.Drawing;
using MrCrusher.Framework.BaseObjects;

namespace MrCrusher.Framework.BaseObjectsFactories.Interfaces
{
    public abstract class IBunkerFactory {
        protected abstract Bunker CreateBunker(string imageFileName, string bunkerTowerImageFileName, int health, Point positionCenter, int cannonLength,
            int primaryWeaponPower, Rectangle rectangleForCollisionDetection, int range, int viewRange, double accuracy, int fireRate);
    }
}