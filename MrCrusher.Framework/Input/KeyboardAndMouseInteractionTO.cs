using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SdlDotNet.Input;

namespace MrCrusher.Framework.Input {

    [Serializable]
    public class KeyboardAndMouseInteractionTO {
        private IDictionary<Key, bool> _keyPressedList;

        public IDictionary<Key, bool> KeyPressedList {
            get { return _keyPressedList; }
            set { _keyPressedList = value; }
        }

        [DataMember]
        public int CursorPositionX { get; set; }
        [DataMember]
        public int CursorPositionY { get; set; }
        [DataMember]
        public bool CursorMoved { get; set; }

        [DataMember]
        public bool PrimaryButtonPressed { get; set; }
        [DataMember]
        public bool SecondaryButtonPressed { get; set; }
        [DataMember]
        public bool MiddleButtonPressed { get; set; }
    }
}