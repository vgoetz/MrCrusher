using System;
using System.Drawing;
using MrCrusher.Framework.BaseObjects;

namespace MrCrusher.Framework.BaseObjectsFactories.Interfaces
{
    public abstract class IPlayersBunkerFactory : IBunkerFactory {
        public abstract Bunker CreateBunker(PlayersBunkerType type, int health, Point positionCenter, Guid guid);
        public abstract Bunker CreateBunker(int type, int health, Point positionCenter, Guid guid);
        public abstract Bunker CreateBunker(PlayersBunkerType type, int health, Point positionCenter);
        public abstract Bunker CreateBunker(int type, int health, Point positionCenter);
    }
}