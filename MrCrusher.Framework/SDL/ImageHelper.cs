using System;
using System.Drawing;
using System.IO;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Game.Environment;
using SdlDotNet.Graphics;

namespace MrCrusher.Framework.SDL
{
    public class ImageHelper {

        public static Surface LoadImage(string dirAndfileName, bool makeTransparent) {

            var image = new Surface(dirAndfileName);
            Color? transparentColor = makeTransparent ? GetTransparentColorFromSDMFile(dirAndfileName) : null;
            image = ConvertImage(image);

            if (transparentColor.HasValue) {
                MakeTransparent(image, (Color) transparentColor);
            }

            return image;
        }

        public static Surface ConvertImage(Surface surface){
            // Im Falle eines Tests z.B.
            if (GameEnv.StdVideoScreen == null) {
                GameEnv.StdVideoScreen = Video.SetVideoMode(GameEnv.ScreenWidth, GameEnv.ScreenHeight, 32, false, false, false, true);
            }
            return surface.Convert(GameEnv.StdVideoScreen, true, false);
        }

        public static void MakeTransparent(Surface surface, Color transparentColor)
        {
            surface.Transparent = true;
            surface.TransparentColor = transparentColor;
        }


        public static Surface CreateRotatedSurface(Surface sourceSurface, Degree degreesOfRotation, bool transparent = true, bool antiAlias = false) {

            Surface rotatedSurface = sourceSurface.CreateRotatedSurface(Convert.ToInt32(degreesOfRotation), false);

            // Größenunterschied feststellen
            int srcWidth = sourceSurface.Width;
            int srcHeight = sourceSurface.Height;

            

            if (srcWidth % 2 > 0) {
                throw new ArgumentException("CreateRotatedSurface: Wrong surface width! Should not be an odd number!");
            }

            if (srcHeight % 2 > 0) {
                throw new ArgumentException("CreateRotatedSurface: Wrong surface height! Should not be an odd number!");
            }

            int diffWidth = rotatedSurface.Width - srcWidth;
            int diffHeight = rotatedSurface.Height - srcHeight;

            // Die Größe hat sich geändert => Größe wieder reduzieren / zentrierten Inhalt verwenden
            if (diffWidth != 0 || diffHeight != 0) {
                // Bildgröße reduzieren, weil das Bild nun größer geworden ist (zwischen 1% und 50% größer)
                var clip = new Rectangle(diffWidth / 2, diffHeight / 2, srcWidth, srcHeight);
                rotatedSurface = rotatedSurface.CreateSurfaceFromClipRectangle(clip);
            }
            
            // Transparenz
            if(transparent) {
                MakeTransparent(rotatedSurface, sourceSurface.TransparentColor);
            }
            
            return rotatedSurface;
        }

        public static Color? GetTransparentColorFromSDMFile(string surfaceDirAndFileName) {
            string sdmFileName = surfaceDirAndFileName.Substring(0, surfaceDirAndFileName.Length - 3) + "sdm";
            Color? transparentColor = null;

            if(surfaceDirAndFileName.ToLower().Contains("background")) {
                return null;
            }

            try {
                if(Directory.Exists(Path.GetDirectoryName(surfaceDirAndFileName)) == false) {
                    throw new DirectoryNotFoundException(Path.GetDirectoryName(surfaceDirAndFileName));
                }

                using (var sdmFile = new StreamReader(sdmFileName)) {
                    string line = "";

                    // Zeilen lesen
                    while ((line = sdmFile.ReadLine()) != null) {

                        if (line.Trim().StartsWith("#") && line.Contains("TransparentColor:")) {
                            var lineEntries = line.Split(':');
                            transparentColor = Color.FromName(lineEntries[1]);
                        }
                    }
                }
            } catch(DirectoryNotFoundException) {
                return Color.Magenta;
            } catch (FileNotFoundException) {
                return Color.Magenta;
            }

            return transparentColor;
        }
    }
}
