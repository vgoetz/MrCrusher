using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.Input;
using MrCrusher.Framework.Player;
using XSockets.Client40;
using XSockets.Client40.Common.Event.Arguments;
using XSockets.Core.XSocket.Helpers;
using System.Drawing;

namespace MrCrusher.XSocketsClient {
    
    public class XSocketsClientProgram {

        static XSocketClient _client;
        public static int Fps = 30;
        private static MainProgram _mainProgram;

        static void Main(string[] args) {

            Console.WriteLine("Starting client\n");
            if (args.Length > 0) {
                Console.WriteLine("Parameter: [ {0} ]\n", String.Join(" | ", args));
            }

            string serverIpAdress = args.Length > 0 ? args[0] : "127.0.0.1";
            string playersName    = args.Length > 1 ? args[1] : String.Format("ClientPlayer_{0}", GameEnv.Random.Next(0, 99999));

            var localPlayer = new Player(playersName, true, false);
            GameEnv.Players = new List<Player> {localPlayer};

            GameEnv.RunningAspect = PublicFrameworkEnums.RunningAspect.Client;
            _mainProgram = new MainProgram();
            _mainProgram.SendClientInteractionsToServer += SendPlayerInteractions;

            Thread.Sleep(3000);
            string connectionString = String.Format("ws://{0}:4502/GameConnectionController", serverIpAdress);
            Console.WriteLine("Create Client for connection {0} ...", connectionString);
            _client = new XSocketClient(connectionString, "*");
            _client.QueryString.Add("clientName", playersName);

            _client.OnConnected += ClientConnected;
            _client.OnDisconnected += ClientDisconnected;
            _client.OnError += HandleError;

            ConsoleKeyInfo? key = null;
            while (key != new ConsoleKeyInfo(Convert.ToChar(27), ConsoleKey.Escape, false, false, false)) {
                try {
                    _client.Controller("GameConnection").On<IGameSessionTransferObject>("HandleNewDataFromServer", HandleNewDataFromServer);
                    _client.Open();
                    
                    _mainProgram.Run(Fps);

                } catch (Exception e) {
                    Console.WriteLine("\nException: {0}", e.Message);

                    Thread.Sleep(2000);
                    Console.WriteLine("\nPress any key to retry or ESC to quit");
                    key = Console.ReadKey();
                }
            }

            try {
                _client.Disconnect();
            
            // ReSharper disable once EmptyGeneralCatchClause --- Das ist so gewollt
            } catch {}
        }

        private static void SendPlayerInteractions(KeyboardInteraction keyboardInteraction, MouseInteraction mouseInteraction, EventArgs eventargs) {

            if (keyboardInteraction.KeyPressedList.Any() || mouseInteraction.CursorMoved || !mouseInteraction.NoneButtonPressed) {

                var keyAndMouseTo = new KeyboardAndMouseInteractionTO {
                    KeyPressedList         = keyboardInteraction.KeyPressedList, 
                    CursorMoved            = mouseInteraction.CursorMoved,
                    CursorPositionX        = mouseInteraction.CursorPositionX, 
                    CursorPositionY        = mouseInteraction.CursorPositionY,
                    PrimaryButtonPressed   = mouseInteraction.PrimaryButtonPressed,
                    SecondaryButtonPressed = mouseInteraction.SecondaryButtonPressed,
                    MiddleButtonPressed    = mouseInteraction.MiddleButtonPressed
                };

                _client.Controller("GameConnection").Invoke<KeyboardAndMouseInteractionTO>("HandleKeyAndMouseEvents", keyAndMouseTo);
            }
        }

        //When the connection is open we bind with confirmation
        private static void ClientConnected(object sender, EventArgs eventArgs) {
            Console.WriteLine("Open Socket ...");
            //_client.Bind("GameConnection", OnCallbackUsed, OnConnenctionConfirmed);
        }
        static void ClientDisconnected(object sender, EventArgs e) {
            Console.WriteLine("Connection closed");
        }
        private static void HandleError(object sender, OnErrorArgs onErrorArgs) {
            Console.WriteLine("Socket-Error: " + onErrorArgs.Exception.Message);
        }

        //private static void OnConnenctionConfirmed(XSockets.Client40.Common.Event.Interface.ITextArgs textArgs) {
        //    Console.WriteLine("Connection confirmed " + textArgs.data);
        //    GameEnv.LocalPlayer.ClientGuid = _client.ClientInfo.ClientGuid;
        //}

        private static void HandleNewDataFromServer(IGameSessionTransferObject receivedData) {
            //string data = textArgs.data;
            //var receivedData = data.Deserialize<GameSessionTransferObject>();
            if (receivedData != null) {
                if (receivedData.ImageTransferObjects == null || !receivedData.ImageTransferObjects.Any() || receivedData.ImageTransferObjects.All(ito => ito == null)) {
                    throw new ApplicationException("Daten vom Server empfangen aber keine Image-Daten vorhanden.");
                    // Hint: Never use nested Interface-Types in sended type! Always use class instead!
                }

                // Set client-player´s user color (actually it´s set by the server, not choosen by client)
                var localPlayerTo = receivedData.PlayerTos.FirstOrDefault(p => p.ClientGuid == GameEnv.LocalPlayer.ClientGuid);
                if (localPlayerTo != null) {
                    object convertedColor = GameEnv.ColorConverter.ConvertFromInvariantString(localPlayerTo.ColorAsString);
                    if (convertedColor != null) {
                        GameEnv.LocalPlayer.PlayersColor = (Color) convertedColor;
                    }
                }

                GameEnv.RegisterImageTransferObjectForAdding(receivedData.ImageTransferObjects.ToList());

                if (receivedData.GameOver) {
                    if (GameEnv.EndTime == null) {
                        GameEnv.EndTime = DateTime.Now;
                    }
                } else {
                    GameEnv.EndTime = null;
                }
            } else {
                throw new ApplicationException("Daten vom Server empfangen, aber gesendetes Datenobjekt ist null!");
            }
        }


    }
}
