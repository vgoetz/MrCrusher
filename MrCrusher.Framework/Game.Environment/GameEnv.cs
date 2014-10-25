using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Drawable;
using MrCrusher.Framework.Player;
using SdlDotNet.Graphics;
using Font = SdlDotNet.Graphics.Font;

namespace MrCrusher.Framework.Game.Environment {

    public static class GameEnv {

        public static Random Random = new Random(DateTime.UtcNow.Millisecond);

        public static string FontResourcesSubDir  = @".\Resources\Fonts\";
        public static string ImageResourcesSubDir = @".\Resources\Images\";
        public static string SoundResourcesSubDir = @".\Resources\Sounds\";
        public static string VideoResourcesSubDir = @".\Resources\Videos\";

        public static ColorConverter ColorConverter = new ColorConverter();

        public static Font BodyCountFont = new Font(FontResourcesSubDir + "Autumn__.ttf", 15);
        public static Font TimeElapsedFont = new Font(FontResourcesSubDir + "Arial.ttf", 18);

        public static int Fps = 30;

        public static int ScreenWidth = 800;
        public static int ScreenHeight = 600;
        public static bool Fullscreen = false;

        public static int TopMenuWidth = ScreenWidth;
        public static int TopMenuHeight = 50;

        public static IList<Player.Player> Players { get; set; }
        public static Player.Player LocalPlayer {
            get { return Players != null ? Players.FirstOrDefault(p => p.IsLocalPlayer) : null; }
        }

        public static IGameObject RandomGameObjectControlledByAPlayer {
            get {
                List<Player.Player> allLivingPlayersObjects =
                    Players.Where(p => p.CurrentControlledGameObject != null && !p.CurrentControlledGameObject.Dead).ToList();

                if (allLivingPlayersObjects.Any()) {
                    
                    var random = new Random(DateTime.UtcNow.Millisecond);
                    var playerIndex = random.Next(0, allLivingPlayersObjects.Count - 1);

                    return allLivingPlayersObjects[playerIndex].CurrentControlledGameObject;
                }

                return null;
            }
        }

        public static DateTime StartTime;
        public static DateTime? EndTime;

        public static PublicFrameworkEnums.RunningAspect RunningAspect;

        public static Surface StdVideoScreen;

        public static GameObjectTypeConstructionPlanCollection ConstructionPlanCollection = new GameObjectTypeConstructionPlanCollection();

        

        private static List<IGameObject> _gameObjectsToDrawAndMove;
        public static IList<IGameObject> GameObjectsToDrawAndMove {
            get {
                if (_gameObjectsToDrawAndMove == null) {
                    _gameObjectsToDrawAndMove = new List<IGameObject>();
                } 
                return _gameObjectsToDrawAndMove.AsReadOnly();
            }
        }

        private static List<IImageTransferObject> _imageTransferObjects;
        public static IList<IImageTransferObject> ImageTransferObjects {
            get {
                if (_imageTransferObjects == null) {
                    _imageTransferObjects = new List<IImageTransferObject>();
                }
                return _imageTransferObjects.AsReadOnly();
            }
        }

        private static readonly object LockerForAdding = new object();
        private static List<IImageTransferObject> _pendingImageTransferObjectsForAdding;
        
        public static void RegisterImageTransferObjectForAdding(List<ImageTransferObject> newObjects) {

            lock (LockerForAdding) {
                if (_pendingImageTransferObjectsForAdding == null) {
                    _pendingImageTransferObjectsForAdding = new List<IImageTransferObject>();
                }

                if (newObjects != null) {
                    _pendingImageTransferObjectsForAdding.AddRange(newObjects);
                }
            }
        }

        public static void AddRegisteredImageTransferObjects() {
            if (_imageTransferObjects == null) {
                _imageTransferObjects = new List<IImageTransferObject>();
            }
            if (_pendingImageTransferObjectsForAdding == null) {
                _pendingImageTransferObjectsForAdding = new List<IImageTransferObject>();
            }

            // There are new images for each new frame 
            lock (LockerForAdding) {
                if (_pendingImageTransferObjectsForAdding.Any()) {
                    _imageTransferObjects.Clear();

                    foreach (var imageTransferObject in _pendingImageTransferObjectsForAdding) {

                        if (imageTransferObject != null && 
                            _imageTransferObjects != null && 
                            _imageTransferObjects.Contains(imageTransferObject) == false) {

                            _imageTransferObjects.Add(imageTransferObject);

                            if (imageTransferObject.ClientGuid != null && imageTransferObject.ClientGuid != Guid.Empty) {
                                if (LocalPlayer.ClientGuid.Equals(imageTransferObject.ClientGuid)) {
                                    LocalPlayer.CurrentControlledTransferObject = imageTransferObject;
                                }
                            }
                        }
                    }

                    _pendingImageTransferObjectsForAdding.Clear();
                }
            }
        }

        public static Rectangle CenterStartLimitationRectangleInMap {
            get { 
                const int width  = 150;
                const int height = 150;

                return new Rectangle(ScreenWidth/2 - width/2, ScreenHeight/2 - height/2, width, height);
            }
        }

        public static Rectangle TopStartLimitationRectangleInMap {
            get {
                int width        = ScreenWidth + 50;
                const int height = 50;

                return new Rectangle(0 - (width - ScreenWidth) / 2, -1 * height, width, height);
            }
        }

        public static Rectangle BottomStartLimitationRectangleInMap {
            get {
                int width        = ScreenWidth + 50;
                const int height = 50;

                return new Rectangle((width - ScreenWidth) / 2, ScreenHeight, width, height);
            }
        }

        public static Rectangle LeftStartLimitationRectangleInMap {
            get {
                const int width = 50;
                var height      = ScreenHeight + 50;

                return new Rectangle(0 - width, (ScreenHeight - height) / 2, width, height);
            }
        }

        public static Rectangle RightStartLimitationRectangleInMap {
            get {
                const int width = 50;
                var height = ScreenHeight + 50;

                return new Rectangle(ScreenWidth, (ScreenHeight - height) / 2, width, height);
            }
        }


        private static List<IGameObject> _pendingGameObjectsForAdding;

        public static List<IGameObject> ExistingAndRegisteredGameObjects {
            get {
                var allObjects = new List<IGameObject>();
                if ( _gameObjectsToDrawAndMove != null) allObjects.AddRange(_gameObjectsToDrawAndMove);
                if (_pendingGameObjectsForAdding != null) allObjects.AddRange(_pendingGameObjectsForAdding);
                return allObjects;
            }
        }

        public static void RegisterGameObjectForAdding(IGameObject newObject) {
            if (_pendingGameObjectsForAdding == null) {
                _pendingGameObjectsForAdding = new List<IGameObject>();
            }

            if (newObject != null && _pendingGameObjectsForAdding.Contains(newObject) == false) {
                _pendingGameObjectsForAdding.Add(newObject);
            }
        }

        public static void AddRegisteredGameObjects() {
            if (_gameObjectsToDrawAndMove == null) {
                _gameObjectsToDrawAndMove = new List<IGameObject>();
            }
            if (_pendingGameObjectsForAdding == null) {
                _pendingGameObjectsForAdding = new List<IGameObject>();
            }

            foreach (var gameObjectToAdd in _pendingGameObjectsForAdding) {
                if (_gameObjectsToDrawAndMove != null && _gameObjectsToDrawAndMove.Contains(gameObjectToAdd) == false) {
                    _gameObjectsToDrawAndMove.Add(gameObjectToAdd);
                }
            }

            _pendingGameObjectsForAdding.Clear();
        }

        public static void RemoveAllGameObjectsMarkedAsToBeDeleted() {
            if (_gameObjectsToDrawAndMove == null) {
                return;
            }

            var objectsToRemove = _gameObjectsToDrawAndMove.Where(gameObject => gameObject.ShouldBeDeleted).ToList();

            foreach (var gameObject in objectsToRemove) {
                _gameObjectsToDrawAndMove.Remove(gameObject);
            }
        }
        
        public static void ClearGameObjects() {
            if (_gameObjectsToDrawAndMove != null) {
                _gameObjectsToDrawAndMove.Clear();
            }
        }

        public static void ResetGame() {
            // Reset everything
            ClearGameObjects();
            ResetAllPlayers();
        }

        private static void ResetAllPlayers() {
            StartTime = DateTime.Now;
            EndTime = null;

            foreach (var player in Players) {
                player.Reset();
            }
        }

        public static IGameSessionTransferObject ToGameSessionTransferObject() {
            var playerTos = Players.Select(p => p.ToPlayerTo()).ToList();
            bool gameOver = Players.All(p => p.CurrentControlledGameObject.Dead);

            return new GameSessionTransferObject(playerTos, GetGameImageTransferObjects(), gameOver);
        }

        public static IList<ImageTransferObject> GetGameImageTransferObjects() {

            var list = new List<ImageTransferObject>();
            foreach (var allImageTosOfOneGameObject in GameObjectsToDrawAndMove.Where(gameObject => gameObject.Visible).Select(gameObject => gameObject.ToImageTransferObjects())) {
                list.AddRange(allImageTosOfOneGameObject);
            }

            return list;
        }
    }
}
