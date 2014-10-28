using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.SDL;
using SdlDotNet.Graphics;

namespace MrCrusher.Framework.MediaManagement {
    public sealed class ImageContainer : AbstractContainer {
        private static Dictionary<string, Surface> _standardImageContainer = new Dictionary<string, Surface>();
        private readonly bool _initialized;
        public bool Initialized {
            get {
                if (_initialized && _standardImageContainer.Count == 0) {
                    throw new ApplicationException("Der ImageContainer wurde zwar initialisiert, aber er enthält keine Images!");
                }
                return _initialized;
            }
        }

        #region ctor and dtor
        public ImageContainer() {
            ReadAllFiles("*.png");
            CheckForOtherFiles("*.gif", "*.bmp");
            _initialized = true;
        }

        ~ImageContainer() {
            Dispose(false);
        }

        public override void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (disposing) {
                foreach (var surface in _standardImageContainer) {
                    surface.Value.Dispose();
                }
                _standardImageContainer.Clear();
                _standardImageContainer = null;
            }
        }
        #endregion ctor and dtor

        public override void ReadAllFiles(string searchPattern) {
            Console.WriteLine("------------- Read all images ...");
            long totalFileSize = 0;
            long totalMemSize = 0;
            int imagesLoadedCounter = 0;
            string[] allImagePaths = Directory.GetFiles(GameEnv.ImageResourcesSubDir, searchPattern, SearchOption.AllDirectories);


            foreach (var imagePath in allImagePaths) {
                string fileName = Path.GetFileName(imagePath);

                // Nur hinzufügen, wenn dieses File noch nicht geladen wurde!
                if (fileName != null && _standardImageContainer.ContainsKey(fileName)) {
                    continue;
                }

                var fileInfo = new FileInfo(imagePath);
                //Console.WriteLine("Load image file \"{0}\"", fileName);

                if (string.IsNullOrEmpty(imagePath) || string.IsNullOrEmpty(fileName)) {
                    throw new ApplicationException(string.Format("Der Pfad {0} oder der Dateiname {1} ist leer!", imagePath, fileName));
                }
                var tmpImage = ImageHelper.LoadImage(imagePath, true);
                _standardImageContainer.Add(fileName, tmpImage);
                imagesLoadedCounter++;
                totalFileSize += fileInfo.Length;
                totalMemSize += tmpImage.Height * tmpImage.Width * tmpImage.BytesPerPixel;
            }

            Console.WriteLine("Read {0} images into container (Filesize: {1}, Memsize: {2}).",
                imagesLoadedCounter, Calculator.ToFuzzyByteString(totalFileSize), Calculator.ToFuzzyByteString(totalMemSize));
        }

        public static Surface GetImage(string imageName) {
            if (GameEnv.RunningAspect == PublicFrameworkEnums.RunningAspect.TestsOnly) {
                return GameEnv.DummySurfaceForTest;
            }

            if (_standardImageContainer.ContainsKey(imageName) == false) {
                throw new ApplicationException(string.Format("Das Image mit dem Namen '{0}' konnte im Container nicht gefunden werden.", imageName));
            }

            Surface imageData;
            _standardImageContainer.TryGetValue(imageName, out imageData);

            if (imageData == null) {
                throw new ApplicationException(string.Format("Das Image mit dem Namen '{0}' konnte dem Container nicht entnommen werden.", imageName));
            }

            return imageData;
        }
    }
}
