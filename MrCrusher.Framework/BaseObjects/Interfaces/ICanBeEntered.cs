
namespace MrCrusher.Framework.BaseObjects.Interfaces {

    public interface ICanBeEntered : IGameObject {

        /// <summary>
        /// Ist diese Object besetzt / betreten werden?
        /// </summary>
        bool IsManned { get; }
    }
}