using System;
using System.Linq;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Game.Environment;
using SdlDotNet.Audio;
using SdlDotNet.Core;
using SdlDotNet.Input;

namespace MrCrusher.Framework.Input {

    public class KeyboardInteractionHandler {

        public static void HandleLocalPlayersGameSystemInputs() {
            HandleLocalKeyboardGameSystemInput(GameEnv.LocalPlayer);
        }

        public static void HandleAllPlayersGamePlayInputs() {

            if (GameEnv.RunningAspect == PublicFrameworkEnums.RunningAspect.Client) {
                throw new ApplicationException("Only a server is allowed to handle game play input");
            }

            foreach (var player in GameEnv.Players) {
                HandleKeyboardGamePlayInput(player);
            }

            // Clean client key interactions 
            foreach (var player in GameEnv.Players.Where(p => !p.IsHost)) {
                player.LastPlayersKeybardInteraction.Reset();
            }
        }

        private static void HandleLocalKeyboardGameSystemInput(Player.Player localPlayer) {
            if (localPlayer != GameEnv.LocalPlayer || !localPlayer.IsLocalPlayer) {
                throw new ApplicationException("player {0} is not a local player");
            }

            // Quit application
            if (IsKeyPressed(localPlayer, Key.Escape)) {
                MusicPlayer.Stop();
                Events.QuitApplication();
            }

            // Restart game
            if (localPlayer.IsHost && IsKeyPressed(localPlayer, Key.R)) {
                GameEnv.ResetGame();
                GameStartupConditions.SetPlayersTankAndBunkerAtRandomLocaltionAtTheCenter();

                foreach (var existingPlayer in GameEnv.Players) {
                    existingPlayer.CreateNewSoldierAtRandomPosition();
                    
                }
            }
        }

        private static void HandleKeyboardGamePlayInput(Player.Player player) {

            ISoldier controlledSoldier          = player.MainControlledSoldier;
            IGameObject currentControlledObject = player.CurrentControlledGameObject;

            if (controlledSoldier == null || currentControlledObject == null || currentControlledObject.Dead) {
                return;
            }

            if (IsKeyPressed(player, Key.LeftControl)) {
                // God mode
                if (IsKeyPressed(player, Key.G)) {
                    currentControlledObject.Vulnerable = !currentControlledObject.Vulnerable;
                }

                // Kill players object
                if (IsKeyPressed(player, Key.LeftControl) && IsKeyPressed(player, Key.D)) {
                    if (player.CurrentControlledGameObject.Dead == false) {
                        currentControlledObject.Die(null);
                    }
                }

            } else if (currentControlledObject.CanReseiveKeyCommands) {

                // Leave object
                var objectThatCanBeEntered = currentControlledObject as ICanBeEntered;
                if (objectThatCanBeEntered != null && IsKeyPressed(player, Key.E)) {

                    if (controlledSoldier.CurrentEnterStatus == EnterStatus.Outside) {
                        throw new ApplicationException(string.Format("Current controlled object is of type ICanBeEntered but the soldier큦 'EnterStatus' is not 'ObjectIsEntered', it큦 {0}", 
                            controlledSoldier.CurrentEnterStatus));
                    }

                    controlledSoldier.LeaveObject();
                }

                var controlledTank = currentControlledObject as ITank;
                if (controlledTank != null) {

                    // Rotation
                    if (IsKeyPressed(player, Key.A)) {
                        controlledTank.RotatePlatformToTheLeft();
                    }
                    if (IsKeyPressed(player, Key.D)) {
                        controlledTank.RotatePlatformToTheRight();
                    }

                    // Movement
                    if (IsKeyPressed(player, Key.W)) {
                        controlledTank.MoveForward();
                    }
                    if (IsKeyPressed(player, Key.S)) {
                        controlledTank.MoveBackwards();
                    }

                } else {

                    // Enter object
                    if (IsKeyPressed(player, Key.E)) {

                        if (controlledSoldier.CurrentEnterStatus == EnterStatus.ObjectIsEntered) {
                        throw new ApplicationException(string.Format("An object should be entered but the soldier큦 'EnterStatus' is not 'Outside', it큦 {0}", 
                            controlledSoldier.CurrentEnterStatus));
                        }
                            
                        controlledSoldier.EnterObjectInFront();
                    }

                    // Rotation
                    if (IsKeyPressed(player, Key.A)) {
                        controlledSoldier.RotateSoldierToTheLeftStepwise();
                    }
                    if (IsKeyPressed(player, Key.D)) {
                        controlledSoldier.RotateSoldierToTheRightStepwise();
                    }

                    // Movement
                    if (IsKeyPressed(player, Key.W)) {
                        controlledSoldier.MoveForward();
                    }
                    if (IsKeyPressed(player, Key.S)) {
                        controlledSoldier.MoveBackwards();
                    }
                }


                // Play video for running / driving
                if (IsKeyPressed(player, Key.W) || IsKeyPressed(player, Key.S)) {
                    var soldier = currentControlledObject as Soldier;
                    if (soldier != null) {
                        if (soldier.VideoCollection.ActiveVideoPlayer.Freezed) {
                            soldier.VideoCollection.ActiveVideoPlayer.PlayFromBeginning();
                        }
                    }
                }
            }
        }

        private static bool IsKeyPressed(Player.Player player, Key key) {
            return player.LastPlayersKeybardInteraction.KeyPressedList.ContainsKey(key);
        }
    }
}