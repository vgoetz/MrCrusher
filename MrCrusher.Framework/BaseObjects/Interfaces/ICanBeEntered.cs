
namespace MrCrusher.Framework.BaseObjects.Interfaces {

    public interface ICanBeEntered : IGameObject {

        /// <summary>
        /// Ist diese Objekt besetzt?
        /// </summary>
        bool IsManned { get; set; }
    }
}