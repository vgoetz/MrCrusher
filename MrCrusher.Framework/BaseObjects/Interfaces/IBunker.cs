using MrCrusher.Framework.AI;
using MrCrusher.Framework.BaseObjectsFactories.Interfaces;

namespace MrCrusher.Framework.BaseObjects.Interfaces {
    public interface IBunker : IHasPrimaryWeapon, IHasSecondaryWeapon, IHitable, IHasAimStrategy, IHasShootStrategy, IHasTriggerAttackStrategy, ICanBeEntered {
        IShotFactory SecondaryShotFactory { get; set; }
        string TowerImageName { get; }
        int ViewRange { get; }
    }
}
