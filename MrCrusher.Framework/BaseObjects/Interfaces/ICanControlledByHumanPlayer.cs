using System;

namespace MrCrusher.Framework.BaseObjects.Interfaces {

    public interface ICanControlledByHumanPlayer {

        Player.Player PlayerAsController { get; set; }

        bool CanReseiveKeyCommands { get; }
        bool CanBeControlledByHumanPlayer { get; }
        bool IsControlledByHumanPlayer { get; }
    }
}