using System;
using System.Linq;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.Input;
using MrCrusher.Framework.Player;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;

namespace MrCrusher.XSocketsServer {

    public class GameConnectionController : XSocketController {

        public GameConnectionController() {

            OnOpen += (sender, args) => {
                if (this.HasParameterKey("clientName")) {

                    var newName = this.GetParameter("clientName");

                    if (GameEnv.Players.Any(player => player.Name == newName) == false) {

                        var newPlayer = new Player(newName, false, false) { ClientGuid = ClientGuid };
                        newPlayer.CreateNewSoldierAtRandomPosition();
                        GameEnv.Players.Add(newPlayer);

                        Console.WriteLine("Client '{0}' connected.", newName);
                    }
                }};

        }

        //public void GameConnection(Dictionary<Key, bool> keyboardInteraction) {

        //    var player = GameEnv.Players.FirstOrDefault(p => p.ClientGuid == ClientGuid);

        //    if (player != null) {
        //        player.LastPlayersKeybardInteraction.KeyPressedList = keyboardInteraction;

        //        //Console.WriteLine("<-- Players interactions from client {0} received. <--");
        //        string keys = keyboardInteraction.Aggregate(string.Empty, (current, keyValue) => current + ", " + keyValue.Key);
        //        Console.WriteLine("{0}: Keys received: {1}", player.Name, keys);
        //    } else {
        //        Console.WriteLine("ERROR: <-? Unknow player/client send his interactions - ClientGuid is unknown! <--");
        //    }
        //}

        public void GameConnection(KeyboardAndMouseInteractionTO keyAndMouseTo) {

            var player = GameEnv.Players.FirstOrDefault(p => p.ClientGuid == ClientGuid);

            if (player != null) {
                player.LastPlayersKeybardInteraction.KeyPressedList = keyAndMouseTo.KeyPressedList;

                player.LastPlayersMouseInteraction.CursorMoved = keyAndMouseTo.CursorMoved;
                player.LastPlayersMouseInteraction.CursorPositionX = keyAndMouseTo.CursorPositionX;
                player.LastPlayersMouseInteraction.CursorPositionY = keyAndMouseTo.CursorPositionY;
                player.LastPlayersMouseInteraction.PrimaryButtonPressed = keyAndMouseTo.PrimaryButtonPressed;
                player.LastPlayersMouseInteraction.SecondaryButtonPressed = keyAndMouseTo.SecondaryButtonPressed;
                player.LastPlayersMouseInteraction.MiddleButtonPressed    = keyAndMouseTo.MiddleButtonPressed;

                //Console.WriteLine("<-- Players interactions from client {0} received. <--");
                //string keys = keyAndMouseTo.KeyboardInteraction.KeyPressedList.Aggregate(string.Empty, (current, keyValue) => current + ", " + keyValue.Key);
                //Console.WriteLine("{0}: Keys received: {1}", player.Name, keys);
            } else {
                Console.WriteLine("ERROR: <-? Unknow player/client send his interactions - ClientGuid is unknown! <--");
            }
        }
        
        public void SendGameObjectsToClients(IGameSessionTransferObject gameSessionTransferObject) {
            //Console.WriteLine("--> sending objects -->");
            this.SendToAll(gameSessionTransferObject, "GameConnection");
        }
    }
}
