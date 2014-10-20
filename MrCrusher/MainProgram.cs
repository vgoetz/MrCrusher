using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using MrCrusher.Framework.AI;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Drawable;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.GameConditions;
using MrCrusher.Framework.Input;
using MrCrusher.Framework.MediaManagement;
using MrCrusher.Framework.SDL;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Input;

namespace MrCrusher {
    
    public class MainProgram {

        private ImageContainer _imageContainer;
        private VideoContainer _videoContainer;
        private LocalInputEventHandler _localInputEventHandler;
        
        Surface _background;
        Surface _topMenu;

        private List<IGameObject> _objectsToDelete;

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

                    LoadBackgroundAndMenus();
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

            _objectsToDelete = new List<IGameObject>();
        }

        private void LoadBackgroundAndMenus() {
            _background = ImageHelper.LoadImage(GameEnv.ImageResourcesSubDir + "Background_Sand_800x600.png", false);
            _topMenu    = ImageHelper.LoadImage(GameEnv.ImageResourcesSubDir + "TopMenu.png", false);
        }

        public void Run(int? fps) {
            if(fps != null) {
                GameEnv.Fps = fps.Value;
            }

            Events.Fps = GameEnv.Fps;
            Events.Run();
        }

        private void DoDrawings() {
            if (GameEnv.RunningAspect == PublicFrameworkEnums.RunningAspect.DedicatedServer) {
               return;
            }

            DrawBackground();
            if (GameEnv.RunningAspect == PublicFrameworkEnums.RunningAspect.Server) {
                DrawGameObjects();
            } else if (GameEnv.RunningAspect == PublicFrameworkEnums.RunningAspect.Client) {
                DrawImageTransferObjects();
            }
            DrawMenuAndStatistics();
            //DrawSoftwareMouseCursor();
            GameEnv.StdVideoScreen.Update();
        }

        private void DrawBackground() {
            // Level 0 - Background
            GameEnv.StdVideoScreen.Blit(_background, new Point(0, GameEnv.TopMenuHeight));
        }

        private void DrawImageTransferObjects() {
            // Blit all transfer objects

            if (GameEnv.ImageTransferObjects.Any()) {

                foreach (IImageTransferObject ito in GameEnv.ImageTransferObjects.Where(ito => ito != null)) {

                    Surface surfaceToDraw = null;

                    switch (ito.Infos.SpriteType) {
                        case SpriteType.Image:
                            surfaceToDraw = ImageContainer.GetImage(ito.Infos.Name);
                            break;
                        case SpriteType.Video:
                            surfaceToDraw = VideoContainer.GetVideo(ito.Infos.Name).GetVideoSurface(ito.Infos.SpriteIndex);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (surfaceToDraw != null) {
                        if (ito.Infos.Orientation != null && ito.Infos.Orientation != Degree.Default) {
                            
                            if (ito.Infos.AlphaBlending) {
                                surfaceToDraw.AlphaBlending = true;
                                surfaceToDraw.Alpha = ito.Infos.Alpha;
                            }

                            surfaceToDraw = ImageHelper.CreateRotatedSurface(surfaceToDraw, (int)ito.Infos.Orientation);
                        }
                        GameEnv.StdVideoScreen.Blit(surfaceToDraw, new Point(ito.Infos.SurfacePositionTopLeftX, ito.Infos.SurfacePositionTopLeftY));
                    } else {
                        throw new ApplicationException(string.Format("No Surface to draw! Image name: {0}", ito.Infos.Name));
                    }
                }
            }
        }

        private void DrawGameObjects() {

            // Blit all objects

            // Level 1 - Alle am Boden liegenden Objekte
            var level1Objects = GameEnv.GameObjectsToDrawAndMove.Where(gameObject => gameObject.Dead && gameObject.ShouldBeDeleted == false && gameObject.Visible).ToList();

            // Level 2 - Alle sich am Boden bewegenden Objekte
            var level2Objects = GameEnv.GameObjectsToDrawAndMove.Where(gameObject => gameObject.ShouldBeDeleted == false && gameObject.Visible && level1Objects.Contains(gameObject) == false).ToList();

            // Level 3 - Nahe am Boden fliegende Objekte TODO
            // Level 4 - Weit oben fliegende Objekte TODO

            var allObjects = new List<IGameObject>();

            allObjects.AddRange(level1Objects);
            allObjects.AddRange(level2Objects);
            
            foreach (var gameObject in allObjects.Distinct()) {

                List<Sprite> surfacesToDraw = gameObject.GetCurrentSprites().ToList();
                
                if (surfacesToDraw.Any()) {

                    foreach (Sprite obj in surfacesToDraw) {
                        GameEnv.StdVideoScreen.Blit(obj.Surface, new Point(obj.Infos.SurfacePositionTopLeftX, obj.Infos.SurfacePositionTopLeftY));
                    }

                    if (gameObject.Dead) { continue;}

                    var tank = gameObject as Tank;
                    var bunker = gameObject as Bunker;
                    var soldier = gameObject as Soldier;
                    var movingObj = gameObject as MovingObject;
                    int viewRange = 0;
                    double orientation = 0;
                    
                    viewRange = (tank != null) ? tank.ViewRange : (soldier != null) ? soldier.ViewRange : (bunker != null) ? bunker.ViewRange : 0;
                    orientation = (tank != null) ? tank.TowerOrientationInDegrees : (soldier != null) ? soldier.OrientationInDegrees : (bunker != null) ? bunker.TowerOrientationInDegrees : 0;
                    
                    // Draw line of view
                    if (viewRange > 0) {
                        var l = new Line(gameObject.PositionCenter, Calculator.CalculateDestinationPoint(gameObject.PositionCenter, viewRange, orientation));
                        //GameEnv.StdVideoScreen.Draw(l, Color.Red);
                    }

                    // Draw line of walk
                    if (movingObj != null && movingObj.DestinationPoint != Point.Empty && movingObj is Projectile == false) {
                        var l = new Line(gameObject.PositionCenter, movingObj.DestinationPoint);
                        //GameEnv.StdVideoScreen.Draw(l, Color.Blue);
                    }

                    // Draw CollisionRectangle
                    //Rectangle r = gameObject.RectangleForCollisionDetection;
                    //GameEnv.StdVideoScreen.Draw(new Box(r.Location, r.Size), Color.Red);
                }
            }
        }

        private void DrawMenuAndStatistics() {
            // Top menu
            // - Health
            const int maxHealthWith = 135;
            var currentHealthWith = 0;
            if (GameEnv.LocalPlayer != null) {

                if (GameEnv.LocalPlayer.CurrentControlledGameObject != null &&
                    GameEnv.LocalPlayer.CurrentControlledGameObject.MaxHealth != 0) {

                    currentHealthWith = GameEnv.LocalPlayer.CurrentControlledGameObject.Health * maxHealthWith/
                                        GameEnv.LocalPlayer.CurrentControlledGameObject.MaxHealth;

                } else if (GameEnv.LocalPlayer.CurrentControlledTransferObject != null) {
                    
                    
                    if (GameEnv.LocalPlayer.CurrentControlledTransferObject.MaxHealth != 0) {

                        currentHealthWith = GameEnv.LocalPlayer.CurrentControlledTransferObject.Health * maxHealthWith/
                                            GameEnv.LocalPlayer.CurrentControlledTransferObject.MaxHealth;
                    }
                }

            }
            var topMenuHealthPointsRectangle = new Rectangle(65, 13, currentHealthWith, 6);
            GameEnv.StdVideoScreen.Blit(_topMenu);
            GameEnv.StdVideoScreen.Fill(topMenuHealthPointsRectangle, Color.Red);

            // - Body count
            if (GameEnv.LocalPlayer != null) {
                var font = new SdlDotNet.Graphics.Font(GameEnv.FontResourcesSubDir + "Autumn__.ttf", 15);
                Surface fontTopMenuSurface = font.Render(GameEnv.LocalPlayer.BodyCount.KillsTotal.ToString(CultureInfo.InvariantCulture), Color.Gray, true);
                GameEnv.StdVideoScreen.Blit(fontTopMenuSurface, new Point(65, 25));
            }
            

            // - Stats - Orientation
            //var fontStatsOrientation = new SdlDotNet.Graphics.Font(GameEnv.FontResourcesSubDir + "Arial.ttf", 12);
            //var soldier = GameEnv.CurrentObjectControledByUser as Soldier;
            //var tank    = GameEnv.CurrentObjectControledByUser as Tank;
            //Degree orientation = soldier != null ? soldier.OrientationInDegrees : tank != null ? tank.TowerOrientationInDegrees : null;

            //if (orientation != null) {
            //    fontTopMenuSurface = fontStats.Render("Degree: " + orientation, Color.White, true);
            //    GameEnv.StdVideoScreen.Blit(fontTopMenuSurface, new Point(250, 10));
            //}

            // - Status - Time elapsed
            var fontStatsTimeElapsed = new SdlDotNet.Graphics.Font(GameEnv.FontResourcesSubDir + "Arial.ttf", 18);
            TimeSpan timeElapsed = (GameEnv.EndTime != null ? GameEnv.EndTime.Value : DateTime.Now) - GameEnv.StartTime;
            Color fontColorTimeElapsed = GameEnv.EndTime != null ? Color.OrangeRed : Color.White;
            double hours   = timeElapsed.Hours;
            double minutes = timeElapsed.Minutes;
            double seconds = timeElapsed.Seconds;
            var timeLeftSurface = fontStatsTimeElapsed.Render(string.Format("{0}{1:00}:{2:00}", hours > 0 ? hours + ":" : "", minutes, seconds), fontColorTimeElapsed, true);
            GameEnv.StdVideoScreen.Blit(timeLeftSurface, new Point(GameEnv.ScreenWidth - timeLeftSurface.Width - 10, 10));

            // - Status - Game over
            var fontStatsGameOver1 = new SdlDotNet.Graphics.Font(GameEnv.FontResourcesSubDir + "Arial.ttf", 42) { Bold = true };
            var fontStatsGameOver2 = new SdlDotNet.Graphics.Font(GameEnv.FontResourcesSubDir + "Arial.ttf", 14) { Bold = true };
            bool isGameOver = GameEnv.EndTime != null;

            if (isGameOver) {
                Color fontBgColorGameOver = Color.Black;
                Color fontFgColorGameOver = Color.OrangeRed;
                var gameOverSurface1Bg = fontStatsGameOver1.Render("Game over", fontBgColorGameOver, true);
                var gameOverSurface1Fg = fontStatsGameOver1.Render("Game over", fontFgColorGameOver, true);
                var gameOverSurface2Bg = fontStatsGameOver2.Render("Press 'R' to restart", fontBgColorGameOver, true);
                var gameOverSurface2Fg = fontStatsGameOver2.Render("Press 'R' to restart", fontFgColorGameOver, true);
                GameEnv.StdVideoScreen.Blit(gameOverSurface1Bg, new Point(GameEnv.ScreenWidth / 2 - gameOverSurface1Bg.Width / 2 -4, GameEnv.ScreenHeight / 2 - gameOverSurface1Bg.Height / 2 -4));
                GameEnv.StdVideoScreen.Blit(gameOverSurface1Fg, new Point(GameEnv.ScreenWidth / 2 - gameOverSurface1Fg.Width / 2, GameEnv.ScreenHeight / 2 - gameOverSurface1Fg.Height / 2));
                GameEnv.StdVideoScreen.Blit(gameOverSurface2Bg, new Point(GameEnv.ScreenWidth / 2 - gameOverSurface2Bg.Width / 2 -1, GameEnv.ScreenHeight / 2 - gameOverSurface2Bg.Height / 2 + gameOverSurface1Fg.Height -1));
                GameEnv.StdVideoScreen.Blit(gameOverSurface2Fg, new Point(GameEnv.ScreenWidth / 2 - gameOverSurface2Fg.Width / 2, GameEnv.ScreenHeight / 2 - gameOverSurface2Fg.Height / 2 + gameOverSurface1Fg.Height));
            }
            
        }

        private void DrawSoftwareMouseCursor() {
            var playableMapRectangle = new Rectangle(0, GameEnv.TopMenuHeight, GameEnv.ScreenWidth, GameEnv.ScreenHeight - GameEnv.TopMenuHeight);

            if (playableMapRectangle.IntersectsWith(new Rectangle(Mouse.MousePosition, new Size(1, 1)))) {
                Mouse.ShowCursor = false;
                var mp = Mouse.MousePosition;
                mp.Offset(-8, -8);
                GameEnv.StdVideoScreen.Blit(ImageContainer.GetImage("MouseCursorCrosshair.png"), mp);
                
            } else {
                Mouse.ShowCursor = true;
            }
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
                    AiHandler.AiPerforming();

                    KeyboardInteractionHandler.HandleLocalPlayersGameSystemInputs();
                    KeyboardInteractionHandler.HandleAllPlayersGamePlayInputs();
                    
                    MouseInteractionHandler.HandleLocalPlayersGameSystemInputs();
                    MouseInteractionHandler.HandleAllPlayersGamePlayInputs();

                    CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects();

                    DoDrawings();
                    

                    SendGameObjectStatesToClients(EventArgs.Empty);
                    break;

                case PublicFrameworkEnums.RunningAspect.Client:

                    GameEnv.AddRegisteredImageTransferObjects();
                    DoDrawings();
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

        private void LetObjectsDie() {

            foreach (var gameObject in GameEnv.GameObjectsToDrawAndMove) {
                if (gameObject.ShouldBeDeleted) {

                    gameObject.Die();
                    _objectsToDelete.Add(gameObject);
                }
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
