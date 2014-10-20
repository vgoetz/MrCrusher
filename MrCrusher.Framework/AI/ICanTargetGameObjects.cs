using MrCrusher.Framework.BaseObjects.Interfaces;

namespace MrCrusher.Framework.AI {

    public interface ICanTargetGameObjects {

        IGameObject TargetedGameObject { get; set; }
    }
}
