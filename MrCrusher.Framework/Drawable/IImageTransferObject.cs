using System;

namespace MrCrusher.Framework.Drawable {

    public interface IImageTransferObject {

        Guid GameObjectGuid { get; }
        Guid? ClientGuid { get; }
        SpriteInfo Infos { get; }

        int Health { get; }
        int MaxHealth { get; }
        bool Dead { get; }
        bool IsControlledByHumanPlayer { get; }
        string IdCircleColorAsString { get; }
        short IdCircleRadius { get; }
    }
}