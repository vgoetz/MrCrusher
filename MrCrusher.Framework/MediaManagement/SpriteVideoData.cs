using System;
using System.Collections.Generic;
using MrCrusher.Framework.SDL;
using SdlDotNet.Graphics;

namespace MrCrusher.Framework.MediaManagement {

    public class SpriteVideoData : IDisposable {

        private List<Surface> _surfaces;
        public int VideoLength { get { return _surfaces.Count; } }

        #region ctor and dtor
        public SpriteVideoData(string fileName) {
            _surfaces = VideoHelper.CreateVideoFromSprites(fileName);
        }
        
        ~SpriteVideoData() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (disposing) {
                foreach (var surface in _surfaces) {
                    surface.Dispose();
                }
                _surfaces.Clear();
                _surfaces = null;
            }
        }
        #endregion ctor and dtor

        public Surface GetVideoSurface(int index) {
            return _surfaces[index];
        }
    }
}
