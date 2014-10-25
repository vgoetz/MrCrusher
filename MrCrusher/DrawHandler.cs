using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Drawable;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.MediaManagement;
using MrCrusher.Framework.SDL;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Input;

namespace MrCrusher {

    public class DrawHandler {
        
        static Surface _background;
        static Surface _topMenu;

        public static void Init() {
            _background = ImageHelper.LoadImage(GameEnv.ImageResourcesSubDir + "Background_Sand_800x600.png", false);
            _topMenu = ImageHelper.LoadImage(GameEnv.ImageResourcesSubDir + "TopMenu.png", false);
        }

        public static void DoDrawings() {
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

        private static void DrawBackground() {
            // Level 0 - Background
            GameEnv.StdVideoScreen.Blit(_background, new Point(0, GameEnv.TopMenuHeight));
        }

        private static void DrawImageTransferObjects() {
            // Blit all transfer objects

            if (GameEnv.ImageTransferObjects.Any()) {

                foreach (IImageTransferObject ito in GameEnv.ImageTransferObjects.Where(ito => ito != null)) {

                    Surface surfaceToDraw;

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

                        if (ito.Dead) {
                            continue;
                        }

                        if (ito.IsControlledByHumanPlayer) {
                            if (!String.IsNullOrWhiteSpace(ito.IdCircleColorAsString)) {
                                
                                var colorConverter = new ColorConverter();
                                object convertFromInvariantString = colorConverter.ConvertFromInvariantString(ito.IdCircleColorAsString);
                                if (convertFromInvariantString != null) {
                                    var positionCenter = new Point(ito.Infos.SurfacePositionCenterX, ito.Infos.SurfacePositionCenterY);
                                    DrawIdentifierCircle(positionCenter, ito.IdCircleRadius, (Color) convertFromInvariantString);
                                }
                            }
                        }

                    } else {
                        throw new ApplicationException(string.Format("No Surface to draw! Image name: {0}", ito.Infos.Name));
                    }
                }
            }
        }

        private static void DrawGameObjects() {

            // Blit all objects

            // Level 1 - Alle am Boden liegenden Objekte
            List<IGameObject> level1Objects = GameEnv.GameObjectsToDrawAndMove.Where(gameObject => gameObject.Dead && gameObject.ShouldBeDeleted == false && gameObject.Visible).ToList();

            // Level 2 - Alle sich am Boden bewegenden Objekte
            List<IGameObject> level2Objects = GameEnv.GameObjectsToDrawAndMove.Where(gameObject => gameObject.ShouldBeDeleted == false && gameObject.Visible && level1Objects.Contains(gameObject) == false).ToList();

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

                    if (gameObject.Dead) {
                        continue;
                    }

                    var tank = gameObject as Tank;
                    var bunker = gameObject as Bunker;
                    var soldier = gameObject as Soldier;
                    var movingObj = gameObject as MovingObject;

                    int viewRange = (tank != null) ? tank.ViewRange : (soldier != null) ? soldier.ViewRange : (bunker != null) ? bunker.ViewRange : 0;
                    double orientation = (tank != null) ? tank.TowerOrientationInDegrees : (soldier != null) ? soldier.OrientationInDegrees : (bunker != null) ? bunker.TowerOrientationInDegrees : 0;

                    // Draw human players identifier circle 
                    if (gameObject.IsControlledByHumanPlayer) {
                        DrawIdentifierCircle(gameObject.PositionCenter, (short)(soldier != null ? 15 : 25), gameObject.PlayerAsController.PlayersColor);
                    }

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

        private static void DrawIdentifierCircle(Point positionCenter, short radius, Color color) {
            var idCircle = new Circle(positionCenter, radius);
            GameEnv.StdVideoScreen.Draw(idCircle, color);
            idCircle = new Circle(positionCenter, ++radius);
            GameEnv.StdVideoScreen.Draw(idCircle, color);
            idCircle = new Circle(positionCenter, ++radius);
            GameEnv.StdVideoScreen.Draw(idCircle, color);
        }

        private static void DrawMenuAndStatistics() {
            // Top menu
            // - Health
            const int maxHealthWith = 135;
            var currentHealthWith = 0;
            if (GameEnv.LocalPlayer != null) {

                if (GameEnv.LocalPlayer.CurrentControlledGameObject != null &&
                    GameEnv.LocalPlayer.CurrentControlledGameObject.MaxHealth != 0) {

                    currentHealthWith = GameEnv.LocalPlayer.CurrentControlledGameObject.Health * maxHealthWith /
                                        GameEnv.LocalPlayer.CurrentControlledGameObject.MaxHealth;

                } else if (GameEnv.LocalPlayer.CurrentControlledTransferObject != null) {
                    if (GameEnv.LocalPlayer.CurrentControlledTransferObject.MaxHealth != 0) {
                        currentHealthWith = GameEnv.LocalPlayer.CurrentControlledTransferObject.Health * maxHealthWith /
                                            GameEnv.LocalPlayer.CurrentControlledTransferObject.MaxHealth;
                    }
                }
            }
            var topMenuHealthPointsRectangle = new Rectangle(65, 13, currentHealthWith, 6);
            GameEnv.StdVideoScreen.Blit(_topMenu);
            GameEnv.StdVideoScreen.Fill(topMenuHealthPointsRectangle, Color.Red);

            // - Body count
            if (GameEnv.LocalPlayer != null) {
                Surface fontTopMenuSurface = GameEnv.BodyCountFont.Render(GameEnv.LocalPlayer.BodyCount.KillsTotal.ToString(CultureInfo.InvariantCulture), Color.Gray, true);
                GameEnv.StdVideoScreen.Blit(fontTopMenuSurface, new Point(65, 25));
            }

            // - Username
            if (GameEnv.LocalPlayer != null) {
                Surface fontTopMenuSurface = GameEnv.UsernameInMenuFont.Render(GameEnv.LocalPlayer.Name, GameEnv.LocalPlayer.PlayersColor, true);
                GameEnv.StdVideoScreen.Blit(fontTopMenuSurface, new Point(250, 10));
            }


            // - Stats - Orientation
            //var fontStatsOrientation = new SdlDotNet.Graphics.Font(GameEnv.FontResourcesSubDir + "Arial.ttf", 12);
            //var soldier = GameEnv.CurrentObjectControledByUser as Soldier;
            //var tank    = GameEnv.CurrentObjectControledByUser as Tank;
            //Degree orientation = soldier != null ? soldier.OrientationInDegrees : tank != null ? tank.TowerOrientationInDegrees : null;

            //if (orientation != null) {
            //    fontTopMenuSurface = fontStats.Render("Degree: " + orientation, Color.White, true);
            //    GameEnv.StdVideoScreen.Blit(fontTopMenuSurface, new Point(350, 10));
            //}

            // - Status - Time elapsed
            
            TimeSpan timeElapsed = (GameEnv.EndTime != null ? GameEnv.EndTime.Value : DateTime.Now) - GameEnv.StartTime;
            Color fontColorTimeElapsed = GameEnv.EndTime != null ? Color.OrangeRed : Color.White;
            double hours = timeElapsed.Hours;
            double minutes = timeElapsed.Minutes;
            double seconds = timeElapsed.Seconds;
            var timeLeftSurface = GameEnv.TimeElapsedFont.Render(string.Format("{0}{1:00}:{2:00}", hours > 0 ? hours + ":" : "", minutes, seconds), fontColorTimeElapsed, true);
            GameEnv.StdVideoScreen.Blit(timeLeftSurface, new Point(GameEnv.ScreenWidth - timeLeftSurface.Width - 10, 10));

            // - Status - Game over
            
            bool isGameOver = GameEnv.EndTime != null;

            if (isGameOver) {
                Color fontBgColorGameOver = Color.Black;
                Color fontFgColorGameOver = Color.OrangeRed;
                var gameOverSurface1Bg = GameEnv.GameOver1Font.Render("Game over", fontBgColorGameOver, true);
                var gameOverSurface1Fg = GameEnv.GameOver1Font.Render("Game over", fontFgColorGameOver, true);
                var gameOverSurface2Bg = GameEnv.GameOver2Font.Render("Press 'R' to restart", fontBgColorGameOver, true);
                var gameOverSurface2Fg = GameEnv.GameOver2Font.Render("Press 'R' to restart", fontFgColorGameOver, true);
                GameEnv.StdVideoScreen.Blit(gameOverSurface1Bg, new Point(GameEnv.ScreenWidth / 2 - gameOverSurface1Bg.Width / 2 - 4, GameEnv.ScreenHeight / 2 - gameOverSurface1Bg.Height / 2 - 4));
                GameEnv.StdVideoScreen.Blit(gameOverSurface1Fg, new Point(GameEnv.ScreenWidth / 2 - gameOverSurface1Fg.Width / 2, GameEnv.ScreenHeight / 2 - gameOverSurface1Fg.Height / 2));
                GameEnv.StdVideoScreen.Blit(gameOverSurface2Bg, new Point(GameEnv.ScreenWidth / 2 - gameOverSurface2Bg.Width / 2 - 1, GameEnv.ScreenHeight / 2 - gameOverSurface2Bg.Height / 2 + gameOverSurface1Fg.Height - 1));
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
    }
}