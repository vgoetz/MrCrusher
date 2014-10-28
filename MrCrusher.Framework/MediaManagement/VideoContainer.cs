using System;
using System.Collections.Generic;
using System.IO;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.SDL;

namespace MrCrusher.Framework.MediaManagement {
    public sealed class VideoContainer : AbstractContainer {
        private static Dictionary<string, SpriteVideoData> _standardVideoContainer = new Dictionary<string, SpriteVideoData>();
        private readonly bool _initialized;
        public bool Initialized {
            get {
                if (_initialized && _standardVideoContainer.Count == 0) {
                    throw new ApplicationException("Der VideoContainer wurde zwar initialisiert, aber er enthält keine Images!");
                }
                return _initialized;
            }
        }

        #region ctor and dtor
        public VideoContainer() {
            ReadAllFiles("*.png");
            CheckForOtherFiles("*.gif", "*.bmp");
            _initialized = true;
        }

        ~VideoContainer() {
            Dispose(false);
        }

        public override void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (disposing) {
                foreach (var surface in _standardVideoContainer) {
                    surface.Value.Dispose();
                }
                _standardVideoContainer.Clear();
                _standardVideoContainer = null;
            }
        }
        #endregion ctor and dtor

        public override void ReadAllFiles(string searchPattern) {
            Console.WriteLine("------------- Read all sprite-images ...");
            long totalFileSize = 0;
            long totalMemSize = 0;
            int videosLoadedCounter = 0;
            string[] allImagePaths = Directory.GetFiles(GameEnv.VideoResourcesSubDir, searchPattern, SearchOption.AllDirectories);


            foreach (var imagePath in allImagePaths) {
                string fileName = Path.GetFileName(imagePath);

                // Nur hinzufügen, wenn dieses File noch nicht geladen wurde!
                if (fileName != null && _standardVideoContainer.ContainsKey(fileName)) {
                    continue;
                }
                
                var fileInfo = new FileInfo(imagePath);
                //Console.WriteLine("Load sprite-image file \"{0}\"", fileName);

                if (string.IsNullOrEmpty(imagePath) || string.IsNullOrEmpty(fileName)) {
                    throw new ApplicationException(string.Format("Der Pfad {0} oder der Dateiname {1} ist leer!", imagePath, fileName));
                }
                var tmpImage = ImageHelper.LoadImage(imagePath, false);
                var tmpVideo = new SpriteVideoData(fileName);
                _standardVideoContainer.Add(fileName, tmpVideo);
                videosLoadedCounter++;
                totalFileSize += fileInfo.Length;
                totalMemSize += tmpImage.Height * tmpImage.Width * tmpImage.BytesPerPixel;

            }

            Console.WriteLine("Read {0} sprite-images into container (Filesize: {1}, Memsize: {2}).",
                videosLoadedCounter, Calculator.ToFuzzyByteString(totalFileSize), Calculator.ToFuzzyByteString(totalMemSize));
        }

        public static SpriteVideoData GetVideo(string videoName) {
            if (GameEnv.RunningAspect == PublicFrameworkEnums.RunningAspect.TestsOnly) {
                return new SpriteVideoData("dummyImage.png");
            }

            if(_standardVideoContainer.ContainsKey(videoName) == false) {
                throw new ApplicationException(string.Format("Das Video mit dem Namen '{0}' konnte im Container nicht gefunden werden.", videoName));
            }

            SpriteVideoData videoData;
            _standardVideoContainer.TryGetValue(videoName, out videoData);

            if(videoData == null) {
                throw new ApplicationException(string.Format("Das Video mit dem Namen '{0}' konnte dem Container nicht entnommen werden.", videoName));
            }

            return videoData;
        }
    }
}
