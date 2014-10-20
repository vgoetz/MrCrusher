using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.AI {

    public interface IAiMission {
        Point GetMissionTargetAsPoint();
        IGameObject GetMissionTargetAsObject();
    }
}
