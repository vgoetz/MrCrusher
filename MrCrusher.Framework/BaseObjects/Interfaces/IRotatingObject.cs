using System.Drawing;
using MrCrusher.Framework.AI;
using MrCrusher.Framework.Core;

namespace MrCrusher.Framework.BaseObjects.Interfaces {
    
    public interface IRotatingObject : IGameObject, ICanTargetGameObjects {

        Degree OrientationInDegrees { get; }
        Degree RotationDegreesStep { get; }
        Degree DestinationRotationForStepwiseRotation { get; }

        bool HasAlreadyRotated { get; }
        bool PendingRotationToDo { get; }
        bool PendingRotation();

        bool MoveForward();
        bool MoveBackwards();

        void MakeReadyForRotation();
        void RotateToMousePosition(Point destination);
        void RotateInstantly(Degree degrees);
        void RotateInstantly(Point destination);
        void SetDestinationRotationForStepwiseRotation(Point destination);
        
    }
}
