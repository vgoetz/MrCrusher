using System;
using MrCrusher.Framework.Core;

namespace MrCrusher.Framework.Drawable {
    [Serializable]
    public enum SpriteType {
        Image,
        Video
    }

    [Serializable]
    public class SpriteInfo {

        private string _name;
        public string Name {
            get { return _name; }
            set {
                if (value == null) {
                    throw new ApplicationException("Name ist null");
                }
                _name = value;
            }
        }

        public SpriteType SpriteType { get; set; }
        public int SpriteIndex { get; set; }
        public int SurfacePositionTopLeftX { get; set; }
        public int SurfacePositionTopLeftY { get; set; }
        public Degree Orientation { get; set; }
        public bool AlphaBlending { get; set; }
        public byte Alpha { get; set; }
    }
}