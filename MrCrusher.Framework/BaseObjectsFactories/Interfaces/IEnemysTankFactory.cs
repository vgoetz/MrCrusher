using System;
using System.Collections.Generic;
using System.Drawing;
using MrCrusher.Framework.BaseObjects;

namespace MrCrusher.Framework.BaseObjectsFactories.Interfaces
{
    public abstract class IEnemysTankFactory : ITankFactory {
        public abstract Tank CreateTank(EnemysTankType type, int health, Point positionCenter, Guid guid);
        public abstract Tank CreateTank(int type, int health, Point positionCenter, Guid guid);
        public abstract Tank CreateTank(EnemysTankType type, int health, Point positionCenter);
        public abstract Tank CreateTank(int type, int health, Point positionCenter);
        
        public abstract IEnumerable<Tank> CreateManyTanks(List<EnemysTankType> types, int numberOfNewObjects, List<Rectangle> limitationRectanglesInMap);
    }
}