using System;
using System.Linq;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.GameConditions;
using MrCrusher.Framework.Input;
using MrCrusher.Framework.MediaManagement;
using MrCrusher.Framework.SDL;
using SdlDotNet.Core;
using SdlDotNet.Input;

namespace MrCrusher {
    
    public class MainProgram {

        private ImageContainer _imageContainer;
        private VideoContainer _videoContainer;
        private LocalInputEventHandler _localInputEventHandler;
        
        public event GameLoopIterationDoneDelegate SendGameObjectStatesToClients;
        public delegate void GameLoopIterationDoneDelegate(EventArgs eventArgs);

        public event GameLoopIterationStartsDelegate SendClientInteractionsToServer;
        public delegate void GameLoopIterationStartsDelegate(KeyboardInteraction keyboardInteraction, MouseInteraction mouseInteraction, EventArgs eventArgs);

        public MainProgram() {
            InitDefaultsForRun();
        }

        public void InitDefaultsForRun() {
            GameEnv.StartTime = DateTime.Now;
            GameEnv.Fullscreen = false;
            Keyboard.KeyRepeat = false;
            
            InitializeComponents();

            switch (GameEnv.RunningAspect) {
                case PublicFrameworkEnums.RunningAspect.DedicatedServer:
                    //_eventHandler.BindApplicationEvents(BIG_LOOP);
                    break;
                case PublicFrameworkEnums.RunningAspect.Server:
                case PublicFrameworkEnums.RunningAspect.Client:
                    GameEnv.StdVideoScreen = VideoDriverSettings.InitializeComponents(GameEnv.ScreenWidth, GameEnv.ScreenHeight, GameEnv.Fullscreen, GameEnv.RunningAspect);
                    _localInputEventHandler.BindApplicationEvents(BIG_LOOP);
                    _localInputEventHandler.BindKeyboardAndMouseEvents();

                    DrawHandler.Init();
                    MusicHelper.LoadMusic(GameEnv.SoundResourcesSubDir + "PanzerRollt_10001.ogg");
                    SoundHandler.LoadSounds();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void InitializeComponents() {

            _imageContainer = new ImageContainer();
            if(_imageContainer.Initialized == false) {
                throw new ApplicationException("Der ImageContainer ist nicht initialisiert!");
            }

            _videoContainer = new VideoContainer();
            if (_videoContainer.Initialized == false) {
                throw new ApplicationException("Der VideoContainer ist nicht initialisiert!");
            }

            _localInputEventHandler = new LocalInputEventHandler();
        }

        public void Run(int? fps) {
            if(fps != null) {
                GameEnv.Fps = fps.Value;
            }

            Events.Fps = GameEnv.Fps;
            Events.Run();
        }

        
// ReSharper disable InconsistentNaming
        public void BIG_LOOP(object sender, TickEventArgs args) {
// ReSharper restore InconsistentNaming

            switch (GameEnv.RunningAspect) {

                case PublicFrameworkEnums.RunningAspect.Server:
                    
                    GameEnv.RemoveAllGameObjectsMarkedAsToBeDeleted();
                    DecayOfDeadObjects();

                    GameRunningConditions.AddNewEnemySoldiersAtRandomPosition(5);
                    GameRunningConditions.AddNewEnemyTanksAtRandomPosition(2);
                    GameEnv.AddRegisteredGameObjects();

                    MoveAndRotateLivingObjects();
                    AiHandler.PerformAi();

                    KeyboardInteractionHandler.HandleLocalPlayersGameSystemInputs();
                    KeyboardInteractionHandler.HandleAllPlayersGamePlayInputs();
                    
                    MouseInteractionHandler.HandleLocalPlayersGameSystemInputs();
                    MouseInteractionHandler.HandleAllPlayersGamePlayInputs();

                    CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects();

                    DrawHandler.DoDrawings();

                    if (GameEnv.RunWithServer) {
                        SendGameObjectStatesToClients(EventArgs.Empty);
                    }
                    break;

                case PublicFrameworkEnums.RunningAspect.Client:

                    GameEnv.AddRegisteredImageTransferObjects();
                    DrawHandler.DoDrawings();
                    KeyboardInteractionHandler.HandleLocalPlayersGameSystemInputs();
                    MouseInteractionHandler.HandleLocalPlayersGameSystemInputs();
                    SendClientInteractionsToServer(GameEnv.LocalPlayer.LastPlayersKeybardInteraction, GameEnv.LocalPlayer.LastPlayersMouseInteraction, EventArgs.Empty);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void DecayOfDeadObjects() {
            foreach (var gameObject in GameEnv.GameObjectsToDrawAndMove.OfType<IDecayable>()) {
                gameObject.Decay();
            }
        }

        private void MoveAndRotateLivingObjects() {

            foreach (var gameObject in GameEnv.GameObjectsToDrawAndMove) {
                if (gameObject.ShouldBeDeleted == false) {
                    
                    var rotatingObject = gameObject as IRotatingObject;
                    var movingObject   = gameObject as MovingObject;

                    if (rotatingObject != null) {
                        rotatingObject.MakeReadyForRotation();

                        // Do all pending rotations
                        rotatingObject.PendingRotation();
                    }

                    if (movingObject != null) {
                        movingObject.MakeReadyForMotion();
                        
                        // Do all pending moves
                        movingObject.PendingMove();
                    }

                    var weapon = gameObject as Weapon;
                    if(weapon != null) {
                        weapon.HandleFireRate();
                    }

                    var objWithPrimaryWeapon = gameObject as IHasPrimaryWeapon;
                    if(objWithPrimaryWeapon != null) {
                        if (objWithPrimaryWeapon.PrimaryWeapon != null) {
                            objWithPrimaryWeapon.PrimaryWeapon.HandleFireRate();
                        }
                    }

                    var objWithSecondaryWeapon = gameObject as IHasSecondaryWeapon;
                    if (objWithSecondaryWeapon != null) {
                        if (objWithSecondaryWeapon.SecondaryWeapon != null) {
                            objWithSecondaryWeapon.SecondaryWeapon.HandleFireRate();
                        }
                    }
                }
            }
        }

    }
}
