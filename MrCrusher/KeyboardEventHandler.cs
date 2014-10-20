using System;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Game.Environment;
using SdlDotNet.Audio;
using SdlDotNet.Core;
using SdlDotNet.Input;

namespace MrCrusher {

    public class KeyboardEventHandler {

        private static bool _keyInteractionAllowed;

        public KeyboardEventHandler() {
            _keyInteractionAllowed = true;
        }

        public void KeyboardDownEventHandler(object sender, KeyboardEventArgs args) {
            if (!_keyInteractionAllowed) {
                if (GameEnv.Player.LastPlayersKeybardInteraction.KeyPressedList.Any()) {
                    GameEnv.Player.LastPlayersKeybardInteraction.KeyPressedList.Clear();
                }
                return;
            }

            if (GameEnv.Player.LastPlayersKeybardInteraction.KeyPressedList.ContainsKey(args.Key) == false) {
                GameEnv.Player.LastPlayersKeybardInteraction.KeyPressedList.Add(args.Key, false);
            }
        }

        public void KeyboardUpEventHandler(object sender, KeyboardEventArgs args) {
            if (!_keyInteractionAllowed) {
                if (GameEnv.Player.LastPlayersKeybardInteraction.KeyPressedList.Any()) {
                    GameEnv.Player.LastPlayersKeybardInteraction.KeyPressedList.Clear();
                }
                return;
            }

            if (GameEnv.Player.LastPlayersKeybardInteraction.KeyPressedList.ContainsKey(args.Key)) {
                GameEnv.Player.LastPlayersKeybardInteraction.KeyPressedList.Remove(args.Key);
            }
        }

        private bool IsKeyPressed(Key key) {
            return GameEnv.Player.LastPlayersKeybardInteraction.KeyPressedList.ContainsKey(key);
        }

        public void HandlePlayersKeyboardInput() {

            if (IsKeyPressed(Key.Escape)) {
                MusicPlayer.Stop();
                Events.QuitApplication();
            }

            if (_keyInteractionAllowed == false || GameEnv.CurrentObjectControledByUser == null) {
                return;
            }

            // Restart 
            if (IsKeyPressed(Key.R)) {
                GameEnv.ResetGame();
                GameStartupConditions.SetPlayersSoldierTankBunkerAtRandomLocaltionAtTheCenter();
            }

            if (IsKeyPressed(Key.LeftControl)) {
                // God mode
                if (IsKeyPressed(Key.G)) {
                    GameEnv.CurrentObjectControledByUser.Vulnerable = !GameEnv.CurrentObjectControledByUser.Vulnerable;
                }

                // Kill Players Character
                if (IsKeyPressed(Key.LeftControl) && IsKeyPressed(Key.D)) {
                    if (GameEnv.CurrentObjectControledByUser.Dead == false) {
                        GameEnv.CurrentObjectControledByUser.Die();
                    }
                }

            } else if (GameEnv.CurrentObjectControledByUser.Dead == false &&
                       GameEnv.CurrentObjectControledByUser.CanReseiveKeyCommands) {

                if (IsKeyPressed(Key.E)) {

                    if (GameEnv.CurrentObjectControledByUser == GameEnv.PlayersSoldier) {
                        GameEnv.PlayersSoldier.EnterObjectInFront();
                        
                    } else if (GameEnv.CurrentObjectControledByUser is ICanBeEntered) {
                        GameEnv.PlayersSoldier.LeaveObject();
                    }
                }

                // Rotation
                if (IsKeyPressed(Key.A)) {
                    if(GameEnv.CurrentObjectControledByUser is ITank) GameEnv.PlayersTank.RotatePlatformToTheLeft();
                    if (GameEnv.CurrentObjectControledByUser is ISoldier) GameEnv.PlayersSoldier.RotateSoldierToTheLeftStepwise();
                }
                if (IsKeyPressed(Key.D)) {
                    if (GameEnv.CurrentObjectControledByUser is ITank) GameEnv.PlayersTank.RotatePlatformToTheRight();
                    if (GameEnv.CurrentObjectControledByUser is ISoldier) GameEnv.PlayersSoldier.RotateSoldierToTheRightStepwise();
                }

                // Movement
                if (IsKeyPressed(Key.W)) {
                    if (GameEnv.CurrentObjectControledByUser is ITank) GameEnv.PlayersTank.MoveForward();
                    if (GameEnv.CurrentObjectControledByUser is ISoldier) GameEnv.PlayersSoldier.MoveForward();
                }
                if (IsKeyPressed(Key.S)) {
                    if (GameEnv.CurrentObjectControledByUser is ITank) GameEnv.PlayersTank.MoveBackwards();
                    if (GameEnv.CurrentObjectControledByUser is ISoldier) GameEnv.PlayersSoldier.MoveBackwards();
                }

                // Play video for running / driving
                if (IsKeyPressed(Key.W) || IsKeyPressed(Key.S)) {
                    if (GameEnv.CurrentObjectControledByUser is ISoldier) {
                        if (GameEnv.PlayersSoldier.VideoCollection.ActiveVideoPlayer.Freezed) {
                            GameEnv.PlayersSoldier.VideoCollection.ActiveVideoPlayer.PlayFromBeginning();
                        }
                    }
                }
            } 
        }
    }
}
