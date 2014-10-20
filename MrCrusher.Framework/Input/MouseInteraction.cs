using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace MrCrusher.Framework.Input {

    [Serializable]
    public class MouseInteraction : PlayersInteraction{

        public MouseInteraction(Player.Player playerReference, Point cursorPosition, bool primaryButtonPressed, bool secondaryButtonPressed, bool middleButtonPressed) 
            : base(playerReference) {

            CursorPositionX = cursorPosition.X;
            CursorPositionY = cursorPosition.Y;
            PrimaryButtonPressed = primaryButtonPressed;
            SecondaryButtonPressed = secondaryButtonPressed;
            MiddleButtonPressed = middleButtonPressed;
            CursorMoved = false;
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
        public bool NoneButtonPressed { get { return !PrimaryButtonPressed && !SecondaryButtonPressed && !MiddleButtonPressed; }}

        public override string ToString() {
            return string.Format("X:{0},Y:{1}, [{2}|{3}|{4}]", CursorPositionX, CursorPositionY, PrimaryButtonPressed ? "X" : " ", SecondaryButtonPressed ? "X" : " ", MiddleButtonPressed ? "X" : " ");
        }

        public void Reset(bool keepCursorPosition) {
            if (!keepCursorPosition) {
                CursorPositionX = 0;
                CursorPositionY = 0;
            }

            PrimaryButtonPressed = false;
            SecondaryButtonPressed = false;
            MiddleButtonPressed = false;
            CursorMoved = false;
        }
    }
}
