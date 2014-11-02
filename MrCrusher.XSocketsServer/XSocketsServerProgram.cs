using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.Player;
using XSockets.Core.Common.Socket;
using XSockets.Plugin.Framework;

namespace MrCrusher.XSocketsServer {

    internal class XSocketsServerProgram {

        private const int Fps = 30;
        private static MainProgram _mainProgram;
        private static GameConnectionController _gameConCtrl;

        static void Main(string[] args) {

            bool multiplayer = true;

            Console.WriteLine("Starting server\n");
            if (args.Length > 0) {
                Console.WriteLine("Parameter: [ {0} ]\n", String.Join(" | ", args));
            }
            
            string playersName = args.Length > 0 ? args[0] : "ServerPlayer";

            GameEnv.RunningAspect = PublicFrameworkEnums.RunningAspect.Server;
            _mainProgram = new MainProgram();

            var localPlayer = new Player(playersName, true, true);
            localPlayer.CreateNewSoldierAtRandomPosition();
            GameEnv.Players = new List<Player> { localPlayer };

            GameStartupConditions.SetPlayersTankAndBunkerAtRandomLocaltionAtTheCenter();

            if (multiplayer) {
                // Bugfix for lost directories while using "new Uri(...)" inside of XSockets
                Composable.AddLocation(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

                using (var container = Composable.GetExport<IXSocketServerContainer>()) {

                    _gameConCtrl = new GameConnectionController();
                    _mainProgram.SendGameObjectStatesToClients += SendGameObjectsToClients;

                    container.StartServers(withInterceptors : true);

                    foreach (var server in container.Servers) {
                        Console.Write("Start XSockets-Server: ");
                        Console.WriteLine(server.ConfigurationSetting.Endpoint);
                    }

                    _mainProgram.Run(Fps);

                    //Console.WriteLine("Hit any key to quit server");
                    //Console.ReadKey();
                    container.StopServers();
                }

            } else {
                _mainProgram.Run(Fps);
            }
        }

        public static void SendGameObjectsToClients(EventArgs eventArgs) {
            var objectToSend = GameEnv.ToGameSessionTransferObject();

            if (objectToSend.PlayerTos.Length > 1) {
                _gameConCtrl.SendGameObjectsToClients(objectToSend);
            } else {
                //Console.WriteLine("Waiting for other player ...");
            }
        }


    }
}