using System;
using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.BaseObjectsFactories.Interfaces {

    public abstract class IPlayersSoldierFactory : ISoldierFactory {
        public abstract Soldier CreateSoldier(PlayersSoldierType type, int health, Point positionCenter, Guid guid);
        public abstract Soldier CreateSoldier(int type, int health, Point positionCenter, Guid guid);
        public abstract Soldier CreateSoldier(PlayersSoldierType type, int health, Point positionCenter);
        public abstract Soldier CreateSoldier(int type, int health, Point positionCenter);
        
        public abstract ISoldier CreatePlayersSoldier();
    }
}