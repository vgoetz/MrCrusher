using System.Drawing;
using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.AI {

    public class AttackObjectMission : IAiMission {
        
        private readonly IGameObject _missionTargetAsGameObject;

        public AttackObjectMission(IGameObject target) {
            _missionTargetAsGameObject = target;
        }

        public Point GetMissionTargetAsPoint() {
            return _missionTargetAsGameObject.PositionCenter;
        }

        public IGameObject GetMissionTargetAsObject() {
            return _missionTargetAsGameObject;
        }
    }
}
