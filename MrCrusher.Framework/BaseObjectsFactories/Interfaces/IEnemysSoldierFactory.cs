using System;
using System.Collections.Generic;
using System.Drawing;
using MrCrusher.Framework.BaseObjects;

namespace MrCrusher.Framework.BaseObjectsFactories.Interfaces
{
    public abstract class IEnemysSoldierFactory : ISoldierFactory {
        public abstract Soldier CreateSoldier(EnemysSoldierType type, int health, Point positionCenter, Guid guid);
        public abstract Soldier CreateSoldier(int type, int health, Point positionCenter, Guid guid);
        public abstract Soldier CreateSoldier(EnemysSoldierType type, int health, Point positionCenter);
        public abstract Soldier CreateSoldier(int type, int health, Point positionCenter);

        public abstract IEnumerable<Soldier> CreateManySoldiers(List<EnemysSoldierType> types, int numberOfNewObjects, List<Rectangle> limitationRectanglesInMap);
    }
}