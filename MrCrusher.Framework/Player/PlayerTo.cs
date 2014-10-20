using System;

namespace MrCrusher.Framework.Player {

    [Serializable]
    public class PlayerTo : IPlayerTo {
        public PlayerTo(string name, Guid clientGuid) {
            Name = name;
            ClientGuid = clientGuid;
        }

        public string Name { get; private set; }
        public Guid ClientGuid { get; private set; }
    }
}