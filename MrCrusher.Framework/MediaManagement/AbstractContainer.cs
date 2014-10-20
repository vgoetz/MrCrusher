using System;
using System.IO;
using MrCrusher.Framework.Game.Environment;

namespace MrCrusher.Framework.MediaManagement {
    public abstract class AbstractContainer : IDisposable{
        public abstract void Dispose();

        public abstract void ReadAllFiles(string searchPattern);

        public void CheckForOtherFiles(params string[] searchPatterns) {
            foreach (var searchPattern in searchPatterns) {
                string[] allXyzImagePaths = Directory.GetFiles(GameEnv.ImageResourcesSubDir, searchPattern, SearchOption.AllDirectories);

                if (allXyzImagePaths.Length > 0) {
                    Console.WriteLine("Warning: Es wurden {0} {1} gefunden, die aber nicht verwendet werden!", allXyzImagePaths.Length, allXyzImagePaths);
                }
            }
        }

    }
}
