using System.Drawing;
using MrCrusher.Framework.AI;

namespace MrCrusher.Framework.BaseObjects.Interfaces {
    
    public interface ISoldier : IRotatingObject, IHasPrimaryWeapon, IHasSecondaryWeapon, IHitable, ISmashable, IDecayable, IHasTriggerAttackStrategy, IHasAimStrategy, IHasShootStrategy, IHasMoveStrategy {
        
        int ViewRange { get; }

        EnterStatus CurrentEnterStatus { get; }

        ICanBeEntered ObjectToEnterOrLeave { get; }
        ICanBeEntered EnteredObject { get; }

        void EnterObjectInFront();
        void LeaveObject();

        void RotateSoldierToTheLeftStepwise();
        void RotateSoldierToTheRightStepwise();
        void RotateSoldierToPointStepwise(Point destination);
    }

    public enum EnterStatus {
        Outside,
        OnTheWayToEnter,
        OnTheWayToLeave,
        ObjectIsEntered
    }
}
