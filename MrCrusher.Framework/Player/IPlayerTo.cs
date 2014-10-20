using System;

namespace MrCrusher.Framework.Player {

    public interface IPlayerTo {
        string Name { get; }
        Guid ClientGuid { get; }
    }
}