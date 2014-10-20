using System.Linq;
using MrCrusher.Framework.Game.Environment;
using SdlDotNet.Input;

namespace MrCrusher.Framework.Input {

    public class LocalKeyboardEventHandler {

        private static bool _keyInteractionAllowed;

        public LocalKeyboardEventHandler() {
            _keyInteractionAllowed = true;
        }

        public void KeyboardDownEventHandler(object sender, KeyboardEventArgs args) {
            if (!_keyInteractionAllowed) {
                if (GameEnv.LocalPlayer.LastPlayersKeybardInteraction.KeyPressedList.Any()) {
                    GameEnv.LocalPlayer.LastPlayersKeybardInteraction.KeyPressedList.Clear();
                }
                return;
            }

            if (GameEnv.LocalPlayer.LastPlayersKeybardInteraction.KeyPressedList.ContainsKey(args.Key) == false) {
                GameEnv.LocalPlayer.LastPlayersKeybardInteraction.KeyPressedList.Add(args.Key, false);
            }
        }

        public void KeyboardUpEventHandler(object sender, KeyboardEventArgs args) {
            if (!_keyInteractionAllowed) {
                if (GameEnv.LocalPlayer.LastPlayersKeybardInteraction.KeyPressedList.Any()) {
                    GameEnv.LocalPlayer.LastPlayersKeybardInteraction.KeyPressedList.Clear();
                }
                return;
            }

            if (GameEnv.LocalPlayer.LastPlayersKeybardInteraction.KeyPressedList.ContainsKey(args.Key)) {
                GameEnv.LocalPlayer.LastPlayersKeybardInteraction.KeyPressedList.Remove(args.Key);
            }
        }
    }
}
