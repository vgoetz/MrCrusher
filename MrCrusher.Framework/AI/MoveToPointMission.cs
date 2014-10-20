using System.Drawing;
using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.AI {

    public class MoveToPointMission : IAiMission {
        
        private readonly Point _missionTargetAsPoint;

        public MoveToPointMission(Point targetPoint) {
            _missionTargetAsPoint = targetPoint;
        }

        public  Point GetMissionTargetAsPoint() {
            return _missionTargetAsPoint;
        }

        public IGameObject GetMissionTargetAsObject() {
            return null;
        }
    }
}
