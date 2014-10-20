using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SdlDotNet.Input;

namespace MrCrusher.Framework.Input {
    
    [Serializable]
    public class KeyboardInteraction : PlayersInteraction {

        public KeyboardInteraction(Player.Player playerReference)
            : base(playerReference) {

            KeyPressedList = new Dictionary<Key, bool>();
        }

        [DataMember]
        public IDictionary<Key, bool> KeyPressedList { get; set; }

        public override string ToString() {
            return "Keys pressed: [" + String.Join("], [", KeyPressedList) + "]";
        }

        public void Reset() {
            KeyPressedList.Clear();
        }
    }
}