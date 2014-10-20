using System;
using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Game.Environment;
using SdlDotNet.Core;
using SdlDotNet.Input;

namespace MrCrusher {

    public class EventHandler {

        public EventHandler(KeyboardEventHandler keyboardEventHandler) {
            _keyboardEventHandler = keyboardEventHandler;
        }

        private readonly KeyboardEventHandler _keyboardEventHandler;
        private MouseButtonEventArgs _mouseButtonEventArgs;
        private bool _primaryMouseButtonHeld;
        private bool _secondaryMouseButtonHeld;
        private bool _middleButtonMouseButtonHeld;
        
        public void BindApplicationEvents(EventHandler<TickEventArgs> bigLoopMethod) {
            Events.Quit += ApplicationQuitEventHandler;
            Events.Tick += bigLoopMethod;
        }

        public void BindKeyboardAndMouseEvents() {
            Events.KeyboardDown    += _keyboardEventHandler.KeyboardDownEventHandler;
            Events.KeyboardUp      += _keyboardEventHandler.KeyboardUpEventHandler;
            Events.MouseMotion     += ApplicationMouseMotionEventHandler;
            Events.MouseButtonDown += ApplicationMouseButtonDownEventHandler;
            Events.MouseButtonUp   += ApplicationMouseButtonUpEventHandler;
        }

        private void ApplicationQuitEventHandler(object sender, QuitEventArgs args) {
            Events.QuitApplication();
        }

        private void ApplicationMouseButtonDownEventHandler(object sender, MouseButtonEventArgs args) {
            _mouseButtonEventArgs = args;
            _primaryMouseButtonHeld = _mouseButtonEventArgs.Button == MouseButton.PrimaryButton;
            _secondaryMouseButtonHeld = _mouseButtonEventArgs.Button == MouseButton.SecondaryButton;
            _middleButtonMouseButtonHeld = _mouseButtonEventArgs.Button == MouseButton.MiddleButton;
        }

        private void ApplicationMouseButtonUpEventHandler(object sender, MouseButtonEventArgs args) {
            _mouseButtonEventArgs = args;

            switch (args.Button) {
                case MouseButton.PrimaryButton:
                    _primaryMouseButtonHeld = false;
                    break;
                case MouseButton.SecondaryButton:
                    _secondaryMouseButtonHeld = false;
                    break;
                case MouseButton.MiddleButton:
                    _middleButtonMouseButtonHeld = false;
                    break;
                case MouseButton.WheelUp:
                    break;
                case MouseButton.WheelDown:
                    break;
            }
        }

        public void DoMouseActions() {
            if (_mouseButtonEventArgs == null || 
                GameEnv.CurrentObjectControledByUser == null ||
                GameEnv.CurrentObjectControledByUser.Dead ||
                GameEnv.CurrentObjectControledByUser.CanReseiveKeyCommands == false) {

                return;
            }

            if (_middleButtonMouseButtonHeld) {
                GameEnv.CurrentObjectControledByUser.PositionCenter = new Point(_mouseButtonEventArgs.X, _mouseButtonEventArgs.Y);
                if (GameEnv.CurrentObjectControledByUser is MovingObject) {
                    ((MovingObject)GameEnv.CurrentObjectControledByUser).Stop();
                }
            }

            if (_primaryMouseButtonHeld) {
                var primaryWeapon = GameEnv.CurrentObjectControledByUser as IHasPrimaryWeapon;

                if (primaryWeapon != null) {
                    primaryWeapon.ShootPrimaryWeapon();
                }
            }

            if (_secondaryMouseButtonHeld) {
                var secondaryWeapon = GameEnv.CurrentObjectControledByUser as IHasSecondaryWeapon;

                if (secondaryWeapon != null) {
                    secondaryWeapon.ShootSecondaryWeapon();
                }
            }
        }

        private void ApplicationMouseMotionEventHandler(object sender, MouseMotionEventArgs args) {

            if (GameEnv.CurrentObjectControledByUser == null || GameEnv.CurrentObjectControledByUser.Dead) {
                return;
            }

            if (GameEnv.CurrentObjectControledByUser is IRotatingObject) {
                if (Math.Abs(GameEnv.CurrentObjectControledByUser.PositionCenter.X - args.Position.X) > 5 ||
                    Math.Abs(GameEnv.CurrentObjectControledByUser.PositionCenter.Y - args.Position.Y) > 5) {

                    ((IRotatingObject) GameEnv.CurrentObjectControledByUser).RotateToMousePosition(args.Position);
                }
            }
        }
    }
}
