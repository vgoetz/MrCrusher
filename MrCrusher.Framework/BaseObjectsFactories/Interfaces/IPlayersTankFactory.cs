using System;
using System.Drawing;
using MrCrusher.Framework.BaseObjects;

namespace MrCrusher.Framework.BaseObjectsFactories.Interfaces
{
    public abstract class IPlayersTankFactory : ITankFactory {
        public abstract Tank CreateTank(PlayersTankType type, int health, Point positionCenter, Guid guid);
        public abstract Tank CreateTank(int type, int health, Point positionCenter, Guid guid);
        public abstract Tank CreateTank(PlayersTankType type, int health, Point positionCenter);
        public abstract Tank CreateTank(int type, int health, Point positionCenter);
    }
}