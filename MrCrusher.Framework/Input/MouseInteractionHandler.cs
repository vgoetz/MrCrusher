using System;
using System.Drawing;
using System.Linq;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Game.Environment;
using SdlDotNet.Input;

namespace MrCrusher.Framework.Input {

    public class MouseInteractionHandler {

        public static void HandleLocalPlayersGameSystemInputs() {
            HandleLocalMouseGameSystemInput(GameEnv.LocalPlayer);
        }

        public static void HandleAllPlayersGamePlayInputs() {

            if (GameEnv.RunningAspect == PublicFrameworkEnums.RunningAspect.Client) {
                throw new ApplicationException("Only a server is allowed to handle game play input");
            }

            foreach (var player in GameEnv.Players) {
                HandleMouseGamePlayInput(player);
            }

            // Clean client key interactions 
            foreach (var player in GameEnv.Players.Where(p => !p.IsHost)) {
                player.LastPlayersMouseInteraction.Reset(true);
            }
        }

        private static void HandleLocalMouseGameSystemInput(Player.Player player) {
            if (player != GameEnv.LocalPlayer || !player.IsLocalPlayer) {
                throw new ApplicationException("player {0} is not a local player");
            }

            // TODO?
        }

        private static void HandleMouseGamePlayInput(Player.Player player) {

            ISoldier controlledSoldier          = player.MainControlledSoldier;
            IGameObject currentControlledObject = player.CurrentControlledGameObject;
            Point mouseCursorPos                = new Point(player.LastPlayersMouseInteraction.CursorPositionX, player.LastPlayersMouseInteraction.CursorPositionY);
            bool mouseCursorMoved               = player.LastPlayersMouseInteraction.CursorMoved;

            // Reset cursor movement
            player.LastPlayersMouseInteraction.CursorMoved = false;
            
            if (controlledSoldier == null || currentControlledObject == null || currentControlledObject.Dead) {
                return;
            }

            if (currentControlledObject.CanReseiveKeyCommands) {

                // Rotation
                var rotatingObject = currentControlledObject as IRotatingObject;
                if (rotatingObject != null && mouseCursorMoved) {
                    if (Math.Abs(rotatingObject.PositionCenter.X - mouseCursorPos.X) > 5 ||
                        Math.Abs(rotatingObject.PositionCenter.Y - mouseCursorPos.Y) > 5) {

                        rotatingObject.RotateToMousePosition(mouseCursorPos);
                    }
                }

                // Teleporting :)
                if (IsMouseButtonPressed(player, MouseButton.MiddleButton)) {

                    currentControlledObject.PositionCenter = mouseCursorPos;
                    var movingObject = currentControlledObject as MovingObject;
                    if (movingObject != null) {
                        movingObject.Stop();
                    }
                }


                if (IsMouseButtonPressed(player, MouseButton.PrimaryButton)) {

                    var primaryWeapon = currentControlledObject as IHasPrimaryWeapon;
                    if (primaryWeapon != null) {
                        primaryWeapon.ShootPrimaryWeapon();
                    }
                }

                if (IsMouseButtonPressed(player, MouseButton.SecondaryButton)) {
                    
                    var secondaryWeapon = currentControlledObject as IHasSecondaryWeapon;
                    if (secondaryWeapon != null) {
                        secondaryWeapon.ShootSecondaryWeapon();
                    }
                }
            }
        }

        private static bool IsMouseButtonPressed(Player.Player player, MouseButton button) {

            switch (button) {
                case MouseButton.None:
                    return player.LastPlayersMouseInteraction.NoneButtonPressed;
                case MouseButton.PrimaryButton:
                    return player.LastPlayersMouseInteraction.PrimaryButtonPressed;
                case MouseButton.SecondaryButton:
                    return player.LastPlayersMouseInteraction.SecondaryButtonPressed;
                case MouseButton.MiddleButton:
                    return player.LastPlayersMouseInteraction.MiddleButtonPressed;
                case MouseButton.WheelUp:
                    break;
                case MouseButton.WheelDown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("button");
            }

            return false;
        }
    }
}