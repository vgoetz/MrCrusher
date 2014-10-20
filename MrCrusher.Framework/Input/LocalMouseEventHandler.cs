using MrCrusher.Framework.Game.Environment;
using SdlDotNet.Input;

namespace MrCrusher.Framework.Input {

    public class LocalMouseEventHandler {

        private static bool _mouseInteractionAllowed;

        public LocalMouseEventHandler() {
            _mouseInteractionAllowed = true;
        }

        public void ApplicationMouseButtonDownEventHandler(object sender, MouseButtonEventArgs args) {

            if (!_mouseInteractionAllowed) {
                if (GameEnv.LocalPlayer.LastPlayersMouseInteraction.NoneButtonPressed == false) {

                    GameEnv.LocalPlayer.LastPlayersMouseInteraction.PrimaryButtonPressed = false;
                    GameEnv.LocalPlayer.LastPlayersMouseInteraction.SecondaryButtonPressed = false;
                    GameEnv.LocalPlayer.LastPlayersMouseInteraction.MiddleButtonPressed = false;
                }
                return;
            }

            SetMouseButtons(args, true);
        }

        public void ApplicationMouseButtonUpEventHandler(object sender, MouseButtonEventArgs args) {
            if (_mouseInteractionAllowed) {
                SetMouseButtons(args, false);
            }
        }

        private static void SetMouseButtons(MouseButtonEventArgs args, bool buttonDown) {
            
            switch (args.Button) {
                case MouseButton.PrimaryButton:
                    GameEnv.LocalPlayer.LastPlayersMouseInteraction.PrimaryButtonPressed = buttonDown;
                    break;
                case MouseButton.SecondaryButton:
                    GameEnv.LocalPlayer.LastPlayersMouseInteraction.SecondaryButtonPressed = buttonDown;
                    break;
                case MouseButton.MiddleButton:
                    GameEnv.LocalPlayer.LastPlayersMouseInteraction.MiddleButtonPressed = buttonDown;
                    break;
                case MouseButton.WheelUp:
                    break;
                case MouseButton.WheelDown:
                    break;
            }
        }

        public void ApplicationMouseMotionEventHandler(object sender, MouseMotionEventArgs args) {
            GameEnv.LocalPlayer.LastPlayersMouseInteraction.CursorPositionX = args.Position.X;
            GameEnv.LocalPlayer.LastPlayersMouseInteraction.CursorPositionY = args.Position.Y;
            GameEnv.LocalPlayer.LastPlayersMouseInteraction.CursorMoved = true;
        }

    }
}