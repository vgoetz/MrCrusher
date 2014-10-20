using MrCrusher.Framework.AI;
using MrCrusher.Framework.BaseObjectsFactories.Interfaces;

namespace MrCrusher.Framework.BaseObjects.Interfaces {
    public interface ITank : IRotatingObject, IHasPrimaryWeapon, IHasSecondaryWeapon, IHitable, ICanSmash, IHasAimStrategy, IHasShootStrategy, IHasMoveStrategy, IHasTriggerAttackStrategy, ICanBeEntered {
        IShotFactory SecondaryShotFactory { get; set; }

        void RotatePlatformToTheLeft();
        void RotatePlatformToTheRight();

        int ViewRange { get; }
    }
}
