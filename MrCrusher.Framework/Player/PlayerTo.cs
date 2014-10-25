using System;

namespace MrCrusher.Framework.Player {

    [Serializable]
    public class PlayerTo : IPlayerTo {

        public PlayerTo(string name, Guid clientGuid, string colorAsString) {
            Name = name;
            ClientGuid = clientGuid;
            ColorAsString = colorAsString;
        }

        public string Name { get; private set; }
        public Guid ClientGuid { get; private set; }
        public string ColorAsString { get; private set; }
    }
}