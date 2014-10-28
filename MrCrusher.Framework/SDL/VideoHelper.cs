using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Game.Environment;
using SdlDotNet.Graphics;

namespace MrCrusher.Framework.SDL {
    class VideoHelper {

        public static List<Surface> CreateVideoFromSprites(string videoFileName) {

            if (GameEnv.RunningAspect == PublicFrameworkEnums.RunningAspect.TestsOnly) {
                return new List<Surface> {GameEnv.DummySurfaceForTest};
            }

            // Image einlesen
            Surface completeImage = ImageHelper.LoadImage(GameEnv.VideoResourcesSubDir + videoFileName, false);
            Color? transparentColor = ImageHelper.GetTransparentColorFromSDMFile(GameEnv.VideoResourcesSubDir + videoFileName);
            var imageList = new List<Surface>();

            // Textfile einlesen (Sprite Description Map, SDM)
            var sdmRectangles = GetSDMRectanglesFromSDMFile(GameEnv.VideoResourcesSubDir + videoFileName);

            var lengthOfFirstRowOfRectangles = (from rect in sdmRectangles
                                                where rect.Rectangle.Y == 0
                                                select rect).Sum(x => x.Rectangle.Width);

            if (lengthOfFirstRowOfRectangles > completeImage.Width) {
                throw new ApplicationException(string.Format("Das Bild {0} müsste lt. seiner SDM größer sein! SOLL: {1} px, IST: {2} px", 
                                                             videoFileName, lengthOfFirstRowOfRectangles, completeImage.Width));
            }

            // Erstellen der Surfaces aufgrund der SDM
            foreach (var sdmRectangle in sdmRectangles) {
                var tempImage = completeImage.CreateSurfaceFromClipRectangle(sdmRectangle.Rectangle);
                tempImage = ImageHelper.ConvertImage(tempImage);
                if(transparentColor.HasValue) {
                    ImageHelper.MakeTransparent(tempImage, (Color) transparentColor);
                }
                tempImage.Alpha = sdmRectangle.Alpha;
                tempImage.AlphaBlending = sdmRectangle.Alpha < 255;
                imageList.Add(tempImage);
            }

            return imageList;
        }

        private static List<SdmRectangle> GetSDMRectanglesFromSDMFile(string surfaceDirAndFileName) {
            var rectangles = new List<SdmRectangle>();
            string sdmFileName = surfaceDirAndFileName.Substring(0, surfaceDirAndFileName.Length - 3) + "sdm";

            using (var sdmFile = new StreamReader(sdmFileName)) {
                string line = "";
                int lineNumber = 0;

                // Zeilen lesen
                while ((line = sdmFile.ReadLine()) != null) {
                    lineNumber++;

                    if (line.Trim().StartsWith("#")) {
                        continue;
                    }

                    // Prüfung
                    int count = line.Count(f => f == ';');
                    if (count > 0 && count < 4 && count > 5) {
                        throw new ApplicationException(String.Format("Wrong number of \";\" in line {0} in file {1}", lineNumber, sdmFileName));
                    }

                    line = line.Replace(" ", "");
                    if (line.Contains(",,")) {
                        throw new ApplicationException(String.Format("Wrong fromat in line number {0}. It contains \";;\" - file {1}", lineNumber, sdmFileName));
                    }

                    var lineEntries = line.Split(';');
                    byte alpha = count > 4 && string.IsNullOrWhiteSpace(lineEntries[5]) == false 
                        ? Convert.ToByte(lineEntries[5]) 
                        : (byte) 255;

                    rectangles.Add(new SdmRectangle( new Rectangle(
                                                            Convert.ToInt32(lineEntries[1]),
                                                            Convert.ToInt32(lineEntries[2]),
                                                            Convert.ToInt32(lineEntries[3]),
                                                            Convert.ToInt32(lineEntries[4])),
                                                     alpha));

                    if (rectangles.Last().Rectangle.Width == 0) {
                        throw new ApplicationException(String.Format("Width must be greater than 0, line number {0}, file {1}", lineNumber, sdmFileName));
                    }

                    if (rectangles.Last().Rectangle.Height == 0) {
                        throw new ApplicationException(String.Format("Height must be greater than 0, line number {0}, file {1}", lineNumber, sdmFileName));
                    }
                }

                // Zeilen wurden eingelesen => zurückgeben
                return rectangles;
            }

        }
    }
}
